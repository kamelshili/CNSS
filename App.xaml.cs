using CNSSInv.Models;
using CNSSInv.Services;
using CNSSInv.Views;
using Plugin.DeviceInfo;

namespace CNSSInv
{
    public partial class App : Application
    {
        public static bool LicenceValide { get; set; } = false;
        public static string Licence { get; set; } = string.Empty;
        string crypter(string key)
        {
            key += "627";
            var s = 0;
            var licence = "";
            for (int i = 0; i < key.Length - 1; i++)
            {
                s = 0;
                if (key[i] >= 'A' && key[i] <= 'z')
                {
                    s = 0;
                }
                else
                {
                    s = int.Parse(key[i].ToString());
                }
                if (key[i + 1] >= 'A' && key[i + 1] <= 'z')
                {
                    s += 0;
                }
                else
                {
                    s += int.Parse(key[i + 1].ToString());
                }

                licence += s.ToString();
            }

            Console.WriteLine(licence);
            return licence;
        }
        private void Verifexist()
        {
            try
            {
                var pathFile = Constants.pathFolder;
                if (File.Exists(pathFile))
                {
                    StreamWriter sw = new StreamWriter(pathFile, true);
                    try
                    {
                        using (StreamReader sr = new StreamReader(pathFile))
                        {
                            string line = string.Empty;
                            string licence = string.Empty;
                            string key = string.Empty;
                            int i = 0;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if (i == 0)
                                {
                                    key = line;

                                }
                                else if (i == 1)
                                {
                                    licence = line;
                                }
                                i++;
                            }
                            string mykey = CrossDeviceInfo.Current.Id;
                            if (key == mykey && crypter(key) == licence)
                            {
                                LicenceValide = true;
                                Licence = licence;
                                return;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Le fichier n'a pas pu être lu.");
                        Console.WriteLine(e.Message);
                        LicenceValide = false;
                    }
                }
                else
                {
                    LicenceValide = false;
                }
            }
            catch (Exception ex)
            {
                LicenceValide = false;
            }

            LicenceValide = false;
        }
        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            Verifexist();
             MainPage = new NavigationPage(new LoginPage());
           // MainPage = new NavigationPage(new ListUniteBudgetaire());

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
