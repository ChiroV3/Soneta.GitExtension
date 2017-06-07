using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Soneta.GitExtension.Extender
{
    public partial class GitUserActivity
    {
        #region Pobieranie Listy Commitów

        #region Pobieranie offline

        //generowanie pliku z danymi o wszystkich commitach z wszystkich branchy 
        private void GenerateLogFile(string folderPath)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("git log --all --pretty='tformat:% an : % cd;' --decorate --date=iso > log.txt");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }

        private List<string> GetLogsFromFile(string FolderPath)
        {
            var logFile = File.ReadAllLines(FolderPath + @"\log.txt");
            return new List<string>(logFile);
        }

        //zwraca dane o pojedynczym commicie z pojedynczego logu który został wygenerowany wcześniej
        public Commit GetSingleCommitFromLogLine(string LogLine)
        {
            //wyciaganie nicku ze zlozonego stringa 
            int index = LogLine.IndexOf(':');
            string _username = LogLine.Substring(0, index -1);
            LogLine = LogLine.Replace(";", "");
  
            //wyciaganie daty stworzenia/modyfikacji commita
            string datetime = LogLine.Substring(LogLine.IndexOf(':') + 2);
            LogLine = LogLine.Replace(":", "");
            return new Commit
            {
                Owner = new User
                {
                    UserName = _username

                },
                   CreationDate = DateTime.ParseExact(datetime, "yyyy-MM-dd HH:mm:ss +ffff", CultureInfo.InvariantCulture)
            };
        }

        #endregion


        private List<Commit> GetCommitList()
        {
            string path = " ";
            GenerateLogFile(path);
            List<string> Logs = GetLogsFromFile(path);
            List<Commit> Commits = new List<Commit>();
            foreach(var str in Logs)
            {
                Commits.Add(GetSingleCommitFromLogLine(str));
            }

            return Commits;   
        }


        #endregion


    }
}
