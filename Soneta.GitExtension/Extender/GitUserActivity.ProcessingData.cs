using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soneta.GitExtension.Extender
{
    public partial class GetUserActivity
    {
        private List<Commit> SortCommitsByDate(List<Commit> commits)
        {
            return commits.OrderBy(x => x.CreationDate).ToList();
        }

        public void AddCommitsToUser(List<Commit> commits)
        {
            List<User> users = new List<User>();
            
            foreach (var commit in commits)
            {
                var user = users.Where(x => x.UserName == commit.Owner).First();
                if (user != null)
                {
                    user.Commits.Add(commit);
                }
                else
                {
                    users.Add(new User
                    {
                        UserName = commit.Owner,
                        Commits = new List<Commit>() {commit}
                    });

                }
            }

        }

        private void GenerateUserActivityStats(User user)
        {
            user.AvgCommitsPerDay = CountAvgCommitsPerDay(user.Commits);
            user.NumberOfCommitsToday = CountCommitsToday(user.Commits);
        }

        private int CountCommitsToday(List<Commit> commits)
        {
            DateTime dt = DateTime.Now;
            string today = dt.ToString("yyyy-MM-dd HH:mm:ss +ffff");
            return commits.Where(x => x.CreationDate.ToString("yyyy-MM-dd HH:mm:ss +ffff") == today).Count();
        }

        private double CountAvgCommitsPerDay(List<Commit> commits)
        {
            DateTime dt = DateTime.Today;
            double TotalDays = (dt - commits.Last().CreationDate).TotalDays;

            return (commits.Count / TotalDays);
        }
    }
}
