using CNSSInv.Models;
using CNSSInv.Services;
using Plugin.DeviceInfo;

namespace CNSSInv.Views;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Setting : ContentPage
{
	public Setting()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        string key = CrossDeviceInfo.Current.Id;
        Entry_Key.Text = key;
        if (App.LicenceValide)
        {
            Entry_Licence.Text = App.Licence;
            Entry_Licence.IsEnabled = false;
            Btn_valider.IsEnabled = false;
            Lbl_demo.IsVisible = true;
        }
        else
        {
            Entry_Licence.Focus();
        }
    }

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
    private bool Verifexist(string txtlicence, string txtkey)
    {
        try
        {
            var pathFile = Constants.pathFolder;

            if (File.Exists(pathFile))
            {
                StreamWriter sw = new StreamWriter(pathFile);
                try
                {
                    //Créez une instance de StreamReader pour lire à partir d'un fichier
                    using (StreamReader sr = new StreamReader(pathFile))
                    {
                        string line;
                        string licence = string.Empty;
                        string key = string.Empty;
                        int i = 0;
                        // Lire les lignes du fichier jusqu'à la fin.
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (i == 0)
                                licence = line;
                            else if (i == 1)
                            {
                                key = line;
                            }
                        }
                        if (key == txtkey && licence == txtlicence)
                        {
                            Entry_Licence.IsEnabled = false;
                            Btn_valider.IsEnabled = false;
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Le fichier n'a pas pu être lu.");
                    Console.WriteLine(e.Message);
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }

        return false;
    }
    private void WriteinFile(string txtlicence, string txtkey)
    {
        try
        {
            var pathFile = Constants.pathFolder;

            if (!File.Exists(pathFile))
            {
                StreamWriter sw = new StreamWriter(pathFile);
                try
                {
                    sw.WriteLine(txtkey);
                    sw.WriteLine(txtlicence);
                    sw.Close();
                    App.LicenceValide = true;
                    App.Licence = txtlicence;
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Le fichier n'a pas pu être lu.");
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void Entry_Licence_Focused(object sender, FocusEventArgs e)
    {
        Entry_Licence.BackgroundColor = Colors.White;
    }

    private void Entry_Licence_Unfocused(object sender, FocusEventArgs e)
    {
        Entry_Licence.BackgroundColor = Colors.White;
    }

    private void Btn_valider_Clicked(object sender, EventArgs e)
    {
        if (Entry_Licence.Text != null && Entry_Licence.Text.Length > 0 && Entry_Licence.Text == crypter(Entry_Key.Text))
        {
            WriteinFile(Entry_Licence.Text, Entry_Key.Text);
        }
    }

    private void Btn_fermer_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
}