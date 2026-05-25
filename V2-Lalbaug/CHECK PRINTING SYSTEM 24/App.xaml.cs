using CPS.Business;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace CPS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        ILog Log = LogManager.GetLogger(typeof(App));

        public App()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
        }

        private void MyHandler(object sender, UnhandledExceptionEventArgs ex)
        {
            Log.Error(ex.ExceptionObject);
            MessageBox.Show("Configuration error occured please contact application provider.", "Configuration Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
        }


        void App_Startup(object sender, StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<DatabaseBackup>(context);
                var dbBackup = repository.GetAll().FirstOrDefault();
                if (dbBackup != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbBackup.Path))
                    {
                        var copyPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"APP_Data\CPS.sdf");
                        if (!System.IO.Directory.Exists(dbBackup.Path))
                        {
                            System.IO.Directory.CreateDirectory(dbBackup.Path);
                        }
                        var savePath = System.IO.Path.Combine(dbBackup.Path, string.Format("CPS_{0}.sdf", DateTime.Now.ToString("yyyyMMdd")));
                        System.IO.File.Copy(copyPath, savePath, true);
                    }
                }
                else
                {
                    MessageBox.Show("You must setup the database backup path to start auto backup.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    e.ApplicationExitCode = 0;
                }
            }
        }

    }
}
