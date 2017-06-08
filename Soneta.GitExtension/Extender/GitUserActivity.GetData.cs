using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Soneta.Tools;
using Soneta.Business;
using Soneta.Business.UI;
using Soneta.GitExtension.Extender;

namespace Soneta.GitExtension.Extender
{
    public partial class GitUserActivity
    {
        #region Pobieranie Listy Commitów

        #region Pobieranie offline

        public MessageBoxInformation GetLocalRepo()
        {
            return new MessageBoxInformation(Strings.MsgTitle, Strings.MsgText)
            {
 
                YesHandler = () => QueryContextInformation.Create<NamedStream>(importFolder)
            };
        }
        private object importFolder(NamedStream ns)
        {
            int index = ns.FileName.LastIndexOf("\\");
            string DirectoryPath = ns.FileName.Substring(0, index);
            // Wczytujemy commmity 
            List<Commit> UpdatedCommits = GetLocalRepoCommitList(DirectoryPath);
            // Wymuszamy odświeżenie listy 
            Context.Session.InvokeChanged();
            return "importowanie zakonczone";
        }
        //generowanie pliku z danymi o wszystkich commitach z wszystkich branchy 
        private void GenerateLogFile(string folderPath)
        { 
            Process cmd = new Process();
            cmd.StartInfo.WorkingDirectory = folderPath;
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("git log --all --pretty=\"tformat:%an : %cd\" --date=iso > log.txt");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());


        }

        private List<string> GetLogsFromFile(string FolderPath)
        {
            return new List<string>(File.ReadAllLines(FolderPath + @"\log.txt"));
        }

        //zwraca dane o pojedynczym commicie z pojedynczego logu który został wygenerowany wcześniej
        public Commit GetSingleCommitFromLogLine(string LogLine)
        {
            if (LogLine == null)
                return null;

            //wyciaganie nicku ze zlozonego stringa
            LogLine = LogLine.TrimStart();
            int index = LogLine.IndexOf(":");
            int count = LogLine.Count();
            string _username = LogLine.Substring(0, index -1);
            //wyciaganie daty stworzenia/modyfikacji commita
            string datetime = LogLine.Substring(LogLine.IndexOf(':') + 2);
            LogLine = LogLine.Replace(":", "");
            return new Commit
            {
                Owner = _username,
                CreationDate = DateTime.ParseExact(datetime, "yyyy-MM-dd HH:mm:ss +ffff", CultureInfo.InvariantCulture)
            };
        }
        public List<Commit> GetLocalRepoCommitList(string path)
        {
                GenerateLogFile(path);
                List<string> Logs = GetLogsFromFile(path);
                Commits = new List<Commit>();
                foreach (var str in Logs)
                {
                    Commits.Add(GetSingleCommitFromLogLine(str));
                }
            return Commits;
        }
        #endregion


        #region Pobieranie z online repo
        private void GetOnlineRepo()
        {

        }
        #endregion

        #endregion


    }
}
