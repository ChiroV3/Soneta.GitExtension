using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soneta.GitExtension.Extender;
using Soneta.Business;

namespace Soneta.GitExtension.Extender
{
    public partial class GitUserActivity
    {
        [Context(Required = true)]
        public Context Context { get; set; }

        [Context(Required = true)]
        public Session Session { get; set; }


        private List<Commit> Commits = new List<Commit>();

    }
}
