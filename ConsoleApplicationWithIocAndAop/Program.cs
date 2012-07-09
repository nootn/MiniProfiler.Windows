// /*
// Copyright (c) 2012 Andrew Newton (http://about.me/nootn)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// */

using System;
using System.Reflection;
using Autofac;
using ConsoleApplicationWithIocAndAop.AOP;
using ConsoleApplicationWithIocAndAop.Workers;
using MiniProfiler.Windows;
using MiniProfiler.Windows.IOC;
using Snap;
using Snap.Autofac;

namespace ConsoleApplicationWithIocAndAop
{
    internal class Program
    {
        private static IContainer _container;

        private static void Main(string[] args)
        {
            //Configure building blocks
            ConfigureIocAndAop();

            //Start profiling
            ConsoleProfiling.Start();

            //Run the application
            var app = _container.Resolve<IConsoleApplication>();
            app.Run();

            //Stop profiling and show results
            Console.WriteLine(ConsoleProfiling.StopAndGetConsoleFriendlyOutputStringWithSqlTimings());

            //Allow viewing of results
            Console.WriteLine("... press 'Enter' to exit process ...");
            Console.ReadLine();
        }

        private static void ConfigureIocAndAop()
        {
            var builder = new ContainerBuilder();

            //If using an IOC container, it is a handy to be able to us an IProfiler to 
            //wrap mini profiler calls just to remove that dependency for unit testing
            builder.RegisterType<MiniProfilerWrapper>().AsImplementedInterfaces();

            //We want some classes that do some work, but we don't want to have to wrap profiler steps
            //around each method call, so use AOP to do it for us!
            SnapConfiguration.For(new AutofacAspectContainer(builder)).
                Configure(c =>
                              {
                                  c.IncludeNamespace("ConsoleApplicationWithIocAndAop.*");
                                  c.Bind<ProfileMethodInterceptor>().To<ProfileMethodAttribute>();
                              });

            //Register the types we need to run the application
            builder.RegisterType<DoQuickWork>().AsImplementedInterfaces();
            builder.RegisterType<DoSlowWork>().AsImplementedInterfaces();
            builder.RegisterType<ConsoleApplication>().AsImplementedInterfaces();

            //Ensure we get useful type load exceptions: http://stackoverflow.com/a/8978721/281177
            try
            {
                _container = builder.Build();
            }
            catch (Exception ex)
            {
                if (ex is ReflectionTypeLoadException)
                {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                    throw new AggregateException(typeLoadException.Message, loaderExceptions);
                }
                throw;
            }
        }
    }
}