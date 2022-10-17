using Marcaj.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Marcaj.Services.LocalDbServices;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Essentials;

namespace Marcaj
{
    public partial class App : Application
    {
        public static ServiceManager manager { get; private set; }
        static LServiceManager LDatabase;
        bool noCon = false;
        private static Timer timer;
        public static LServiceManager lDatabase
        {
            get
            {
                if(LDatabase == null)
                {
                    LDatabase = new LServiceManager(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MarcajLocalDb.db3"));
                }
                return LDatabase;
            }
        }
        public App()
        {
            InitializeComponent();

            manager = new ServiceManager(new RService());
            MainPage = new NavigationPage(new StartPage());
            MessagingCenter.Subscribe<StartPage>(this, "NoCon", (sender) => {
                var startTimeSpan = TimeSpan.Zero;
                var periodTimeSpan = TimeSpan.FromSeconds(2);
                timer = new Timer((e) =>
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        Debug.WriteLine("internet ON");
                        MessagingCenter.Send<App>(this, "ConOk");
                        timer.Dispose();
                    }
                    else
                    {
                        Debug.WriteLine("internet OFF");
                    }
                }, null, startTimeSpan, periodTimeSpan);
            });
            
        }


        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
