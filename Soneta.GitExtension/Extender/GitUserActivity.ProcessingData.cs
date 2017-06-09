using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soneta.GitExtension.Extender;

namespace Soneta.GitExtension.Extender
{
    public partial class GitUserActivity
    {
        private List<Commit> SortCommitsByDate(List<Commit> commits)
        {
            return commits.OrderBy(x => x.CreationDate).ToList();
        }

        public void AddCommitsToUser(List<Commit> commits)
        {
            _users?.Clear();
            foreach (var commit in commits)
            {
               
                if (_users.Count > 0)
                {
                    _users.Where(x => x.UserName == commit.Owner).First().Commits.Add(commit);
                }
                else
                {
                    _users.Add(new User
                    {
                        UserName = commit.Owner,
                        Commits = new List<Commit>() {commit}
                    });

                }
            }
            foreach (var us in _users)
            {
                GenerateUserActivityStats(us);
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
            string today = dt.ToString("yyyy-MM-dd");
            return commits.Where(x => x.CreationDate.ToString("yyyy-MM-dd") == today).Count();
        }

        private double CountAvgCommitsPerDay(List<Commit> commits)
        {
            TimeSpan tspan = DateTime.UtcNow - commits.Last().CreationDate;
            return (commits.Count/ tspan.TotalDays);
        }
    }
}
