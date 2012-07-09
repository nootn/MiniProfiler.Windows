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
using StackExchange.Profiling;

namespace MiniProfiler.Windows.IOC
{
    /// <summary>
    /// Class that wraps the Step method of MiniProfiler, handy for use with IOC containers.
    /// E.g. with AutoFac you might register it like this:
    /// builder.RegisterType&lt;MiniProfilerWrapper&gt;().AsImplementedInterfaces();
    /// 
    /// And where you want to wrap some code with a profiler step, it might look like:
    /// using (_profiler.Step("Some description of whats happening")) //where "_profiler" is an IProfiler
    /// {
    ///    //Some long running code here
    /// }
    /// </summary>
    public class MiniProfilerWrapper : IProfiler
    {
        #region IProfiler Members

        public IDisposable Step(string name)
        {
            return StackExchange.Profiling.MiniProfiler.Current.Step(name);
        }

        #endregion
    }
}