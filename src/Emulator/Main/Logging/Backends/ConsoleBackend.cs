//
// Copyright (c) 2010-2017 Antmicro
// Copyright (c) 2011-2015 Realtime Embedded
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.IO;
using System.Linq;
using Antmicro.Renode.Utilities;

namespace Antmicro.Renode.Logging
{
    public class ConsoleBackend : TextBackend 
    {
        public static ConsoleBackend Instance { get; private set; }

        public string WindowTitle
        {
            get
            {
                return Console.Title;
            }
            set
            {
                Console.Title = value;
            }
        }

        public bool ColoringEnabled { get; set; }
        public bool LogThreadId { get; set; }
        public bool ReportRepeatingLines { get; set; }

        public override void Log(LogEntry entry)
        {
            if(!ShouldBeLogged(entry))
            {
                return;
            }

            var type = entry.Type;
            var changeColor = ColoringEnabled && output == Console.Out && type.Color.HasValue && !isRedirected;
            var message = FormatLogEntry(entry);
            
            lock(syncObject)
            {
                if(changeColor)
                {
                    Console.ForegroundColor = type.Color.Value;
                }
                string line;
                if(LogThreadId && entry.ThreadId != null)
                {
                    line = string.Format("{0:HH:mm:ss.ffff} [{1}] ({3}) {2}", CustomDateTime.Now, type, message, entry.ThreadId);
                }
                else
                {
                    line = string.Format("{0:HH:mm:ss.ffff} [{1}] {2}", CustomDateTime.Now, type, message);
                }
                if(output == Console.Out && !ReportRepeatingLines && 
                   lastMessage == message && lastMessageLinesCount != -1 && lastType == type && !isRedirected)
                {
                    try
                    {
                        counter++;
                        Console.CursorVisible = false;
                        Console.CursorTop = Math.Max(0, Console.CursorTop - lastMessageLinesCount);
                        // it can happen that console is resized between one and other write
                        // in case console is widened it would not erase previous messages
                        var realLine = string.Format("{0} ({1})", line, counter);
                        var currentLinesCount = GetMessageLinesCount(realLine);
                        var lineDiff = Math.Max(0, lastMessageLinesCount - currentLinesCount);
                        var width = Console.WindowWidth;
                        
                        Console.WriteLine(realLine);
                        lineDiff.Times(() => Console.WriteLine(Enumerable.Repeat<char>(' ', width - 1).ToArray()));
                        Console.CursorVisible = true;
                        Console.CursorTop = Math.Max(0, Console.CursorTop - lineDiff);
                        lastMessageLinesCount = GetMessageLinesCount(realLine);
                    }
                    catch(ArgumentOutOfRangeException)
                    {
                        // console was resized during computations
                        Console.Clear();
                        WriteNewLine(line);
                    }
                }
                else
                {
                    WriteNewLine(line);
                    lastMessage = message;
                    lastType = type;
                }
                if(changeColor)
                {
                    Console.ResetColor();
                }
            }
        }

        public void Flush()
        {
            output.Flush();
        }

        static ConsoleBackend()
        {
            Instance = new ConsoleBackend();
        }

        private ConsoleBackend()
        {
            ColoringEnabled = true;
            syncObject = new object();
            isRedirected = Console.IsOutputRedirected;
        }

        public override void Dispose()
        {

        }

        private void WriteNewLine(string line)
        {
            counter = 1;
            output.WriteLine(line);
            if(output == Console.Out && !isRedirected)
            {
                lastMessageLinesCount = GetMessageLinesCount(line);
            }
            else
            {
                lastMessageLinesCount = -1; // -1 here means that during last message logging output wasn't console
            }
        }

        private static int GetMessageLinesCount(string message)
        {
            var cnt = message.Split(new [] { System.Environment.NewLine }, StringSplitOptions.None).Sum(x => GetMessageLinesCountForRow(x));
            return cnt;
        }
        
        private static int GetMessageLinesCountForRow(string row)
        {
            var cnt = Convert.ToInt32(Math.Ceiling(1.0 * row.Length / Console.WindowWidth));
            return cnt;
        }

        private TextWriter output = Console.Out;
        private object syncObject;
        private readonly bool isRedirected;
        private string lastMessage;
        private int lastMessageLinesCount;
        private int counter = 1;
        private LogLevel lastType;
    }
}

