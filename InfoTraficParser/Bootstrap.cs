using Common.Logging;
using Common.Logging.Simple;
using SimpleInjector;
using System.Configuration.Abstractions;

namespace InfotraficParser
{
    public static class Bootstrap
    {
        public static Container Container;

        private static readonly IAppSettings AppSettings = ConfigurationManager.Instance.AppSettings;

        public static void Start()
        {
            Container = new Container();

            Container.RegisterLogger();

            Container.RegisterSingleton(new PageCrawler(AppSettings.Get("Url")));

            Container.Verify();
        }

        public static void RegisterLogger(this Container container)
        {
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(LogLevel.All, false, false, false, "dd/MM/yyyy HH:mm:ss:fff", true);

            Container.RegisterSingleton(LogManager.GetLogger<Program>());
        }
    }
}