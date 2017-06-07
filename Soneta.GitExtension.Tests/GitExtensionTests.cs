using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soneta.GitExtension.Extender;
using NUnit.Framework;

namespace Soneta.GitExtension.Tests
{
    public class GitExtensionTests
    { 
        [Test]
        public void GetCommitFromLogLine_Deserialize()
        {
            string line = "ChiroV3 : 2017-06-06 14:15:25 +0200;";
            var obj = new GitUserActivity();
           // Assert.AreEqual("ChiroV3", obj.GetSingleCommitFromLogLine(line).Owner.UserName);
        }
    }
}
