using Soneta.Business.UI;
using Soneta.GitExtension.Extender;

[assembly: FolderView("Soneta.GitExtension/Statystyki GIT",
    Priority = 10,
    Description = "Statystyki użytkowników repo",
    ObjectType = typeof(GitUserActivity),
    ObjectPage = "GitUserActivity.pageform.xml",
    ReadOnlySession = false,
    ConfigSession = false
)]