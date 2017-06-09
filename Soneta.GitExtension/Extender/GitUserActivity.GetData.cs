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
using System.Net;
using Newtonsoft.Json.Linq;

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
            //Pobieranie sciezki folderu w ktorym uzytkownik wybral plik
            int index = ns.FileName.LastIndexOf("\\");
            string DirectoryPath = ns.FileName.Substring(0, index);
            // Wczytujemy commmity 
            Commits = GetLocalRepoCommitList(DirectoryPath);
            SortCommitsByDate(Commits);
            AddCommitsToUser(Commits);
            // Wymuszamy odświeżenie listy 
            Context.Session.InvokeChanged();
            return "importowanie zakonczone";

        }

        //generowanie pliku z danymi o wszystkich commitach z wszystkich branchy 
        private void GenerateLogFile(string folderPath)
        {
            try
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
            catch(Exception)
            {
               //TODO: Wyswietlic uzytkownikowi problem
                return;
            }


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
            if (path.IsNullOrEmpty())
                return null;
            GenerateLogFile(path);
            List<string> Logs = GetLogsFromFile(path);
            List<Commit> _commits = new List<Commit>();
            foreach (var str in Logs)
            {
                _commits.Add(GetSingleCommitFromLogLine(str));
            }

            return _commits;
        }
        #endregion


        #region Pobieranie z online repo
        public MessageBoxInformation GetOnlineRepo()
        {
            return new MessageBoxInformation("Aktualizacja Danych", "Chcesz pobrac informacje z repozytorium GITHUBA?")
            {
                YesHandler = () =>{
                    GetDataFromOnlineRepo();
                    Context.Session.InvokeChanged();
                    return null;
                }
            };
           
        }
        public void GetDataFromOnlineRepo()
        {
            string responseString = ApiCall("commits");
            JArray JS = JArray.Parse(responseString);
            Commits = new List<Commit>();
            for (int i = 0; i < JS.Count(); i++)
            {

                JObject data = (JObject)JS[i]["commit"]["author"];
                string Username = data["name"].ToString();
                string datetime = data["date"].ToString();
                datetime.TrimStart();
                datetime.TrimEnd();
                Console.WriteLine(datetime);
                Commits.Add(new Commit
                {
                    Owner = Username,
                    CreationDate = DateTime.Parse(datetime)
                });
            }

            SortCommitsByDate(Commits);
            AddCommitsToUser(Commits);
        }
        private string ApiCall(string req)
        {
            string URL = "https://api.github.com/repos/ChiroV3/Soneta.GitExtension/" + req;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.ContentType = "application/json; charset=utf-8";
            request.UserAgent = "Soneta.GitExtension";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
          
        }

        #endregion

        #endregion


    }
}
