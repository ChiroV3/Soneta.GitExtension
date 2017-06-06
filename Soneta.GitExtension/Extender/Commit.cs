using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soneta.GitExtension.Extender
{
    public class Commit
    {
        public User Owner { get; set; }
        public DateTime CreationDate { get; set; }
        public string BranchName { get; set; }
    }
}
