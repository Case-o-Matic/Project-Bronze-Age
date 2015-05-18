using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Interaction
{
    public class Log
    {
        public bool setDateTimeStd, toConsole;
        private List<string> messages;

        public Log(bool setdatetimestd, bool toconsole)
        {
            setDateTimeStd = setdatetimestd;
            toConsole = toconsole;
            messages = new List<string>();
        }

        public void WriteLine(string msg)
        {
            var text = (setDateTimeStd ? DateTime.Now.ToString() + ": " : "") + msg;
            messages.Add(text);
            if (toConsole)
                Console.WriteLine(text);
        }
        public void SaveLog(string filepath, bool append)
        {
            if (messages.Count == 0)
                throw new InvalidOperationException("You cannot save a log without any messages.");

            if (append)
            {
                messages.Insert(messages.Count - 1, "---");
                File.AppendAllLines(filepath, messages.ToArray());
            }
            else
                File.WriteAllLines(filepath, messages.ToArray());
        }
    }
}
