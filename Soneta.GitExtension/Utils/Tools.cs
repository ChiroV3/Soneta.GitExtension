using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soneta.Types;

namespace Soneta.GitExtension.Utils
{
    public static class Tools
    {

        // STEP 2
        /// <summary>
        /// Ważne ze względnu na wczytywanie bibliotek, które mają referencje do Soneta.Types
        /// Tylko biblioteki z referencją do Soneta.Types są wczytywane podczas analizowania form.xml
        /// </summary>
#pragma warning disable 414
        private static FromTo _ft = new FromTo();
#pragma warning restore 414

        public const string Version = "1.0";
    }
}
