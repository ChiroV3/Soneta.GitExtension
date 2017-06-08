using System.Collections.Generic;
using System.Linq;
using Soneta.Business;

namespace Soneta.GitExtension.Extender
{

    public partial class GitUserActivity
    {
        [Context(Required = true)]
        public Context Context { get; set; }

        [Context(Required = true)]
        public Session Session { get; set; }

        //#region Widoczność zakładki

        ///// <summary>
        ///// Metoda pozwalająca na sterowanie widocznościa zakładki.
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns>
        /////     true - widoczność zakładki, 
        /////     false - zakładka niewidoczna
        ///// </returns>
        //public static bool IsVisible(Context context)
        //{
        //    bool result;
        //    using (var session = context.Login.CreateSession(true, true)) {
        //        result = CfgWalutyNbpExtender.GetValue(session, "AktywneKursyNbp", false);
        //    }
        //    return result;
        //}

        //#endregion Widoczność zakładki
        public List<Commit> Commits;

        #region Property dla formularza
        private SortedDictionary<string, User> _users;
        public IEnumerable<User> Users
        {
            get
            {
                if (_users != null) return
                    _users.Values.ToArray();

                _users = new SortedDictionary<string, User> {
                    {
                        "first", new User {
                            UserName = "Matt",
                            AvgCommitsPerDay= 2.3,
                            NumberOfCommitsToday = 3
                        }
                    },
                    {

                        "second", new User {
                            UserName = "greg",
                            AvgCommitsPerDay= 43.4,
                            NumberOfCommitsToday = 6
                        }
                    }
                };
                return _users.Values.ToArray();
            }
        }

        #endregion Property dla formularza
    }
}

