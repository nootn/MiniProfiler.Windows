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
using System.Threading;
using MiniProfiler.Windows;
using StackExchange.Profiling;

namespace ConsoleApplicationSimpleExample
{
    internal class Program
    {
        private static void Main()
        {
            //Start profiling
            ConsoleProfiling.Start();

            Console.WriteLine("Starting to call methods..");

            using (StackExchange.Profiling.MiniProfiler.Current.Step("Call Methods"))
            {
                DoTheQuickWork();
                DoTheSlowWork();
            }

            //Stop profiling and show results
            Console.WriteLine(ConsoleProfiling.StopAndGetConsoleFriendlyOutputStringWithSqlTimings());

            //Allow viewing of results
            Console.WriteLine("... press 'Enter' to exit process ...");
            Console.ReadLine();
        }

        private static void DoTheQuickWork()
        {
            using (StackExchange.Profiling.MiniProfiler.Current.Step("DoTheQuickWork"))
            {
                Thread.Sleep(100);
                Console.WriteLine(" ... done quick work ... ");
            }
        }

        private static void DoTheSlowWork()
        {
            using (StackExchange.Profiling.MiniProfiler.Current.Step("DoTheSlowWork"))
            {
                Thread.Sleep(5000);
                Console.WriteLine(" ... done slow work ... ");
            }
        }
    }
}