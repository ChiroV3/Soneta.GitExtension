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

        public List<Commit> Commits;

        #region Property dla formularza
        private List<User> _users = new List<User>();
        public IEnumerable<User> Users
        {
            get
            {
                if (_users != null) return
                    _users.ToArray();
                
                return _users.ToArray();
            }
        }

        #endregion Property dla formularza
    }
}

