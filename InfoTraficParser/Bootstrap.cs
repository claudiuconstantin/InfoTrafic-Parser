using SimpleInjector;
using System.Configuration.Abstractions;

namespace InfotraficParser
{
    public class Bootstrap
    {
        public static Container Container;

        private static readonly IAppSettings AppSettings = ConfigurationManager.Instance.AppSettings;

        public static void Start()
        {
            Container = new Container();

            Container.RegisterSingleton(new PageCrawler(AppSettings.Get("Url")));

            Container.Verify();
        }
    }
}