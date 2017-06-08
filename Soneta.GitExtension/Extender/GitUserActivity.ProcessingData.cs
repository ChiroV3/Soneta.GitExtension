using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soneta.GitExtension.Extender
{
    public partial class GetUserActivity
    {
        private List<Commit> SortCommitsByDate(List<Commit> commits)
        {
            return commits.OrderBy(x => x.CreationDate).ToList();
        }

        private void AddCommitsToUser(string UserName, List<Commit> commits)
        {
            List<User> users = new List<User>();
            
            foreach( var commit in commits)
            {
                var user = users.Where(x => x.UserName == commit.Owner).First();
                if(user !=null)
                {
                    user.Commits.Add(commit);
                }
                else
                {
                    users.Add(new User
                    {
                        UserName = commit.Owner,
                        Commits = new List<Commit>().Add(commit)
                    });
                    
                }
            }

        }

        private void GenerateUserActivityStats(User user)
        {
            
        }
    }
}
