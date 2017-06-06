using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Soneta.GitExtension.Extender
{
    public class User
    {
        public ObservableCollection<Commit> Commits { get; set; }
        public string UserName { get; set; }
    }
}
