﻿// /*
// Copyright (c) 2012 Andrew Newton (http://about.me/nootn)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// */

using StackExchange.Profiling;
using System;

namespace MiniProfiler.Windows
{
    public class ConsoleProfilingProvider : BaseProfilerProvider
    {
        private StackExchange.Profiling.MiniProfiler _profiler;

        public override StackExchange.Profiling.MiniProfiler GetCurrentProfiler()
        {
            return _profiler;
        }

        [Obsolete("Please use the Start(string sessionName) overload instead of this one. ProfileLevel is going away.")]
        public override StackExchange.Profiling.MiniProfiler Start(ProfileLevel level, string sessionName = null)
        {
            return this.Start(sessionName: sessionName);
        }

        public override StackExchange.Profiling.MiniProfiler Start(string sessionName = null)
        {
            _profiler = new StackExchange.Profiling.MiniProfiler(ConsoleProfiling.ProfilingUrl());
            _profiler.Name = sessionName;
            SetProfilerActive(_profiler);
            _profiler.User = ConsoleProfiling.CurrentUser();
            return _profiler;
        }

        public override void Stop(bool discardResults)
        {
            SaveProfiler(_profiler);
        }
    }
}