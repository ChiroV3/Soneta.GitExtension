#define EXAMPLE4
#define OKNO_GLOWNE

using Soneta.Business.Licence;
using Soneta.Business.UI;
using Soneta.GitExtension.Extender;

#if OKNO_GLOWNE

[assembly: FolderView("Soneta.GitExtension",
    Priority = 10,
    Description = "Statystyki Repozytorium",
    BrickColor = FolderViewAttribute.BlueBrick,
    Contexts = new object[] { LicencjeModułu.All }
)]

#endif


#if EXAMPLE4

[assembly: FolderView("Soneta.GitExtension/UserActivity",
    Priority = 12,
    Description = "Statystyki Aktywności użytkowników repozytorium",
    ObjectType = typeof(GitUserActivity),
    ObjectPage = "GitUserActivity.Ogolne.pageform.xml",
    ReadOnlySession = false,
    ConfigSession = false
)]

#endif