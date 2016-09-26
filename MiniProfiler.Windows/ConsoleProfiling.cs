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
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using StackExchange.Profiling;
using System.Data.Common;

namespace MiniProfiler.Windows
{
    public static class ConsoleProfiling
    {
        /// <summary>
        ///   Supply a separate func if you do not want to use the current running process name as the profiler URL.
        /// </summary>
        /// <remarks>
        ///   Must be set before calling "Start"
        /// </remarks>
        public static Func<string> ProfilingUrl = () => (Process.GetCurrentProcess().ProcessName);

        /// <summary>
        ///   Supply a separate func if you do not want to use the current windows user as the user name being profiled.
        /// </summary>
        /// <remarks>
        ///   Must be set before calling "Start"
        /// </remarks>
        public static Func<string> CurrentUser = () => (WindowsIdentity.GetCurrent().Name);

        /// <summary>
        ///   Start profiling using the default profile provider.
        /// </summary>
        public static void Start()
        {
            Start(new ConsoleProfilingProvider());
        }

        /// <summary>
        ///   Start profiling using a custom profile provider
        /// </summary>
        public static void Start(BaseProfilerProvider profilerProvider)
        {
            StackExchange.Profiling.MiniProfiler.Settings.ProfilerProvider = profilerProvider;
            StackExchange.Profiling.MiniProfiler.Start();
        }

        /// <summary>
        ///   Wrap the connection with a profiling connection that tracks timings 
        /// </summary>
        /// <param name="connection"></param>
        public static DbConnection WrapDatabaseConnection(DbConnection connection)
        {
            return new StackExchange.Profiling.Data.ProfiledDbConnection(connection, StackExchange.Profiling.MiniProfiler.Current);
        }

        /// <summary>
        ///   Stops profiling and returns the profiler
        /// </summary>
        /// <returns> The profiler that has been stopped </returns>
        public static StackExchange.Profiling.MiniProfiler StopAndGetProfiler()
        {
            var mp = StackExchange.Profiling.MiniProfiler.Current;
            StackExchange.Profiling.MiniProfiler.Stop();
            return mp;
        }

        /// <summary>
        ///   Stops the profiler and returns a console friendly output representation, with SQL timings if available. The SQL timings are ordered by slowest to fastest.
        /// </summary>
        /// <param name="includeSqlWithDurationMoreThanMilliseconds"> Only include sql queries with a duration of more than the supplied number of milliseconds </param>
        /// <param name="takeTopNumberOfQueries"> Only show this number of queries </param>
        /// <returns> A string that can be rendered to a console, debug window or text file which is human readable. The SQL timings are in the format: *[duration(ms)]*["(D)" if duplicate, else empty] [stack trace snippet] [Formatted command string] </returns>
        [Obsolete("Please use the StopAndGetConsoleFriendlyOutputString() instead of this one. Database timings are included automatically when using the WrapDatabaseConnection() connection for database calls.")]
        public static string StopAndGetConsoleFriendlyOutputStringWithSqlTimings(
            int includeSqlWithDurationMoreThanMilliseconds = 100, int takeTopNumberOfQueries = 10)
        {
            var output = new StringBuilder();
            output.Append(StopAndGetConsoleFriendlyOutputString());
            output.AppendLine("*** MiniProfiler SQL Timings:");
            output.AppendLine("No native Sql Timing support anymore");
            return output.ToString();
        }

        /// <summary>
        ///   Stops the profiler and returns a console friendly output representation.
        /// </summary>
        /// <returns> A string that can be rendered to a console, debug window or text file which is human readable.</returns>
        public static string StopAndGetConsoleFriendlyOutputString()
        {
            var mp = StopAndGetProfiler();
            var output = new StringBuilder();
            output.AppendLine("*** MiniProfiler Output:");
            output.AppendLine(mp.Render().ToString());
            output.AppendLine();
            return output.ToString();
        }
    }
}