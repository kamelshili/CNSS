using System.Security.Cryptography;
using CNSSInv.Models;
using CNSSInv.ViewModels;
using CNSSInv.Services;
namespace CNSSInv.Views;

public partial class Inventaire : ContentPage
{
    public static InventaireViewModel InventaireViewModel { get; set; }
    public static bool Completed { get; set; } = false;

    public static string CodeNatureTH { get; set; }
    public static string LibNatureTH { get; set; }


    public Inventaire(UniteBudgetaire uniteBudgetaire)
    {
        InitializeComponent();
        InventaireViewModel = new InventaireViewModel(uniteBudgetaire, myscrollview);
        Thread.Sleep(5000);
        BindingContext = InventaireViewModel;
        CameraButton.Clicked += CameraButton_Clicked;
        myscrollview.IsVisible = true;
    }

    private async void CameraButton_Clicked(object sender, EventArgs e)
    {
        if (Entry_Immo != null && !String.IsNullOrWhiteSpace(Entry_Immo.Text))
        {
            Page page = (Page)Activator.CreateInstance(typeof(TakePicture), Entry_Immo.Text);

            await Navigation.PushAsync(page, false);
        }
    }

    async void LoadALLPH(string numimmo)
    {
        InventaireDatabaseController inventaireDatabaseController = new InventaireDatabaseController();
        var inventaire = await inventaireDatabaseController.GetInventaireById(numimmo);
        if (inventaire != null)
        {
            var nature = new Nature(inventaire.Code_Nature_PH, inventaire.Lib_Nature_PH, "", "", "");
            NatureDatabaseController natureDatabaseController = new NatureDatabaseController();
            int nbr = natureDatabaseController.GetCountNatureByIDLibNat(nature.Lib_Nature);
            if (nbr == 0)
            {
                CodeNatureTH = "99";
                LibNatureTH = nature.Lib_Nature;
                Entry_Nature.Text = LibNatureTH;
            }
            else
            {
                var index = -1;
                for (int i = 0; i < Picker_Description.ItemsSource.Count; i++)
                {
                    if (((Nature)Picker_Description.ItemsSource[i]).Code_Nature == nature.Code_Nature)
                    {
                        index = i;
                    }
                }
                if (index != -1)
                    Picker_Description.SelectedIndex = index;
            }
        }
    }

    async void LoadALLTH(string numimmo)
    {
        InventaireDatabaseController inventaireDatabaseController = new InventaireDatabaseController();
        var inventaire = await inventaireDatabaseController.GetInventaireById(numimmo);
        if (inventaire != null)
        {
            var nature = new Nature(inventaire.Code_Nature_TH, inventaire.Lib_Nature_TH, "", "", "");
            NatureDatabaseController natureDatabaseController = new NatureDatabaseController();
            int nbr = natureDatabaseController.GetCountNatureByIDLibNat(nature.Lib_Nature);
            if (nbr == 0)
            {
                CodeNatureTH = "99";
                LibNatureTH = nature.Lib_Nature;
                Entry_Nature.Text = LibNatureTH;
            }
            else
            {
                var index = -1;
                for (int i = 0; i < Picker_Description.ItemsSource.Count; i++)
                {
                    if (((Nature)Picker_Description.ItemsSource[i]).Code_Nature == nature.Code_Nature)
                    {
                        index = i;
                    }
                }
                if (index != -1)
                    Picker_Description.SelectedIndex = index;
            }
        }
    }

    private void Entry_Immo_TextChanged(object sender, TextChangedEventArgs e)
    {

        if (Entry_Immo != null && !String.IsNullOrWhiteSpace(Entry_Immo.Text))
        {
            Img_Button_Filter.IsEnabled = true;
            Entry_NSerie.IsEnabled = false;
            Picker_Etat.IsEnabled = false;
        }
        else
        {
            Picker_Description.SelectedItem = new Nature();
            Img_Button_Filter.IsEnabled = false;
            Entry_NSerie.Text = "";
            CameraButton.IsEnabled = false;
            Entry_NSerie.IsEnabled = false;
            Picker_Etat.IsEnabled = false;
            Entry_Immo.Focus();
        }
    }
    async void IfIsRead(string numimmo)
    {
        var action = await DisplayAlert("Update?", " Êtes - vous sûr de modifier les inventaires", "Oui", "Non");
        if (action)
        {
            Img_Button_Filter.IsEnabled = true;
            Entry_NSerie.IsEnabled = false;
            Picker_Etat.IsEnabled = false;
            string immo = Entry_Immo.Text;
            Entry_Immo.Text = "?";
            Completed = true;
            Entry_Immo.Text = numimmo;
            Picker_Description.IsEnabled = true;
            LoadALLPH(Entry_Immo.Text);
            Picker_Description.IsEnabled = false;
            Entry_NSerie.IsEnabled = true;
            Entry_NSerie.Focus();
            Entry_Desc.IsEnabled = true;
            Entry_OldCode.IsEnabled = true;
            CameraButton.IsEnabled = true;
        }
        else
        {
            Entry_Immo.Text = "";
            Entry_Immo.Focus();
            CameraButton.IsEnabled = false;
        }
    }

    async void IfNotIsRead(string numimmo)
    {
        InventaireDatabaseController inventaireDatabaseController = new InventaireDatabaseController();
        var inv = await inventaireDatabaseController.GetInventaireById(numimmo);
        Entry_Immo.Text = numimmo;
        Entry_Nature.IsVisible = true;
        Entry_Nature.Text = inv.Lib_Nature_TH;
        CodeNatureTH = inv.Code_Nature_TH;
        LibNatureTH = inv.Lib_Nature_TH;
        Entry_NSerie.IsEnabled = true;
        Entry_NSerie.Focus();
        Entry_Desc.IsEnabled = true;
        Entry_NSerie.Text = inv.Num_Serie;
        if (inv.Lib_Etat != null && inv.Lib_Etat != "")
            Picker_Etat.SelectedItem = inv.Lib_Etat;
        Picker_Etat.IsEnabled = true;
        Entry_Desc.Focus();
        CameraButton.IsEnabled = true;
    }
    void Immo_Completed(string numimmo)
    {
        if (Entry_Immo != null && !String.IsNullOrWhiteSpace(Entry_Immo.Text))
        {
            var numimmo1 = numimmo;
            InventaireDatabaseController inventaireDatabaseController = new InventaireDatabaseController();
            if (inventaireDatabaseController.GetCountInventaireByRead(numimmo1) > 0)
            {
                IfIsRead(numimmo1);
            }
            else if (inventaireDatabaseController.GetCountInventaire(numimmo1) > 0)
            {
                IfNotIsRead(numimmo1);
            }
            else
            {
                if (numimmo.Length > 3)
                {
                    var numimmo3 = numimmo.Substring(0, 3) + "0" + numimmo.Substring(3, numimmo.Length - 3);
                    if (inventaireDatabaseController.GetCountInventaireByRead(numimmo3) > 0)
                    {
                        IfIsRead(numimmo3);
                    }
                    else if (inventaireDatabaseController.GetCountInventaire(numimmo3) > 0)
                    {
                        IfNotIsRead(numimmo3);
                    }
                    else
                    {
                        Img_Button_Filter.IsEnabled = true;
                        Entry_NSerie.IsEnabled = true;
                        Entry_NSerie.Focus();
                        CameraButton.IsEnabled = true;
                    }
                }
                else
                {
                    Img_Button_Filter.IsEnabled = true;
                    Entry_NSerie.IsEnabled = true;
                    Entry_NSerie.Focus();
                    CameraButton.IsEnabled = true;
                }
            }
        }
        else
        {
            Img_Button_Filter.IsEnabled = false;
            Entry_NSerie.IsEnabled = false;
            Picker_Etat.IsEnabled = false;
            Entry_OldCode.IsEnabled = false;
            Entry_Desc.IsEnabled = false;
            CameraButton.IsEnabled = false;
            Picker_Description.SelectedItem = new Nature();
            Entry_NSerie.Text = "";
        }
    }
    private void Entry_Immo_Completed(object sender, EventArgs e)
    {
        var numimmo = Entry_Immo.Text;
        Immo_Completed(numimmo);
    }

    private void Picker_Description_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Picker_Description.SelectedItem != null && !String.IsNullOrWhiteSpace(((Models.Nature)Picker_Description.SelectedItem).Lib_Nature))
        {
            Entry_Nature.Text = ((Models.Nature)Picker_Description.SelectedItem).Lib_Nature;
            CodeNatureTH = ((Models.Nature)Picker_Description.SelectedItem).Code_Nature;
            LibNatureTH = ((Models.Nature)Picker_Description.SelectedItem).Lib_Nature;
            Entry_Desc.IsEnabled = true;
            Entry_Desc.Focus();
        }
    }
    private void Picker_Etat_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((Picker_Description.SelectedItem != null && !String.IsNullOrWhiteSpace(((Models.Nature)Picker_Description.SelectedItem).Lib_Nature)) || (Entry_Nature != null && !String.IsNullOrWhiteSpace(Entry_Nature.Text)))
        {
            Picker_Etat.IsEnabled = true;
            Entry_NSerie.IsEnabled = true;
            Entry_OldCode.IsEnabled = true;
            Entry_Desc.IsEnabled = true;
            Entry_OldCode.Focus();
        }
        else
        {
            Picker_Etat.IsEnabled = false;
            if (Picker_Etat.SelectedIndex != 0)
                Entry_NSerie.IsEnabled = false;
            Entry_OldCode.IsEnabled = false;
        }
    }

    private void Btn_Cancel_Clicked(object sender, EventArgs e)
    {
        Entry_Nature.Text = "";
        Entry_Immo.IsEnabled = true;
        Entry_Immo.Focus();
        TakePicture.Pathpicture = "";
        Entry_NSerie.IsEnabled = true;
    }

    private void OnCancel()
    {
        Entry_Nature.Text = "";
        Entry_Desc.Text = "";
        Picker_Description.SelectedIndex = -1;
        Picker_Etat.SelectedIndex = -1;
        Entry_NSerie.Text = "";
        Entry_OldCode.Text = "";
    }

    private void Btn_Save_Clicked(object sender, EventArgs e)
    {
        Entry_Nature.Text = "";
        Entry_Immo.IsEnabled = true;
        Entry_Immo.Focus();
        TakePicture.Pathpicture = "";

    }
    private async void Img_Button_Filter_Clicked(object sender, EventArgs e)
    {
        var desc = Picker_Description.SelectedItem;
        Page page = (Page)Activator.CreateInstance(typeof(ListNatures), InventaireViewModel, desc);
        await Navigation.PushAsync(page, false);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (Picker_Description.SelectedIndex != -1)
        {
            Entry_Desc.IsEnabled = true;
            Entry_Desc.Focus();
        }
        else
        {
            Entry_Immo.Focus();
        }
        if (ListNatures.verifempl)
        {
            Picker_Description.SelectedItem = ListNatures.MyNewUnitBud;
            ListNatures.verifempl = false;
        }

    }

    private void Entry_Immo_Focused(object sender, FocusEventArgs e)
    {
        Entry_Immo.BackgroundColor = Colors.Yellow;
    }

    private void Entry_Immo_Unfocused(object sender, FocusEventArgs e)
    {
        Entry_Immo.BackgroundColor = Colors.White;
    }

    private void Picker_Etat_Focused(object sender, FocusEventArgs e)
    {
        Picker_Etat.BackgroundColor = Colors.Yellow;
    }

    private void Picker_Etat_Unfocused(object sender, FocusEventArgs e)
    {
        Picker_Etat.BackgroundColor = Colors.White;
    }

    private void Entry_NSerie_Focused(object sender, FocusEventArgs e)
    {
        Entry_NSerie.BackgroundColor = Colors.Yellow;
    }

    private void Entry_NSerie_Unfocused(object sender, FocusEventArgs e)
    {
        Entry_NSerie.BackgroundColor = Colors.White;
    }

    private void Picker_Description_Focused(object sender, FocusEventArgs e)
    {
        Picker_Description.BackgroundColor = Colors.Yellow;
    }

    private void Picker_Description_Unfocused(object sender, FocusEventArgs e)
    {
        Picker_Description.BackgroundColor = Colors.White;
    }

    private void Entry_Nature_Completed(object sender, EventArgs e)
    {
        if (Entry_Nature != null && !String.IsNullOrWhiteSpace(Entry_Nature.Text))
        {
            var txt = Entry_Immo.Text;
            Entry_Immo.Text = txt + "a";
            LibNatureTH = Entry_Nature.Text;
            Entry_Immo.Text = txt;
            Entry_Desc.IsEnabled = true;
            Picker_Etat.IsEnabled = true;
            Entry_Desc.IsEnabled = true;
            Entry_Desc.Focus();
            Entry_NSerie.IsEnabled = true;
            Entry_OldCode.IsEnabled = true;
        }
        else
        {
            Picker_Etat.IsEnabled = false;
            Entry_NSerie.IsEnabled = false;
            Entry_OldCode.IsEnabled = false;
        }
    }

    private void Entry_Nature_Focused(object sender, FocusEventArgs e)
    {
        Entry_Nature.BackgroundColor = Colors.Yellow;

    }


    private void Entry_Nature_Unfocused(object sender, FocusEventArgs e)
    {
        Entry_Nature.BackgroundColor = Colors.White;

    }

    private void Entry_Description_Focused(object sender, FocusEventArgs e)
    {

    }

    private void Entry_Description_Unfocused(object sender, FocusEventArgs e)
    {

    }

    private void Entry_OldCode_Unfocused(object sender, FocusEventArgs e)
    {
        Entry_OldCode.BackgroundColor = Colors.White;

    }

    private void Entry_OldCode_Focused(object sender, FocusEventArgs e)
    {
        Entry_OldCode.BackgroundColor = Colors.Yellow;

    }

    private async void Entry_NSerie_Completed(object sender, EventArgs e)
    {
        Entry_Nature.IsEnabled = true;
        Entry_Nature.Focus();
        if (Entry_NSerie != null && !String.IsNullOrWhiteSpace(Entry_NSerie.Text))
        {
            InventaireDatabaseController inventaireDatabaseController = new InventaireDatabaseController();

            if (inventaireDatabaseController.GetCountInventaireByNserie(Entry_NSerie.Text) > 0)
            {
                if (inventaireDatabaseController.GetCountInventaireByNserieIsRead(Entry_NSerie.Text) > 0)
                {
                    var action = await DisplayAlert("Update?", " Êtes - vous sûr de modifier les inventaires", "Oui", "Non");
                    if (action)
                    {
                        var inv = await inventaireDatabaseController.GetInventaireByNSerie(Entry_NSerie.Text);
                        Entry_Immo.Text = inv.NumImmo;
                        LoadALLPH(inv.NumImmo);
                        Entry_Nature.Text = inv.Lib_Nature_PH;
                        CodeNatureTH = inv.Code_Nature_TH;
                        LibNatureTH = inv.Lib_Nature_TH;      
                        Entry_OldCode.Text = inv.Date_Acq;
                        Entry_Desc.Text = inv.Observation;
                        Picker_Etat.SelectedItem = inv.Lib_Etat;
                        Entry_NSerie.Text = inv.Num_Serie;
                        CameraButton.IsEnabled = true;
                    }
                    else
                    {
                        Entry_NSerie.Text = "";
                        Entry_NSerie.Focus();
                    }
                }
                else
                {
                    var inv = await inventaireDatabaseController.GetInventaireByNSerie(Entry_NSerie.Text);
                    Entry_Immo.Text = inv.NumImmo;
                    Entry_Nature.Text = inv.Lib_Nature_TH;
                    CodeNatureTH = inv.Code_Nature_TH;
                    LibNatureTH = inv.Lib_Nature_TH;
                    Entry_OldCode.Text = inv.Date_Acq;
                    Entry_Desc.Text = inv.Observation;
                    CameraButton.IsEnabled = true;
                    Entry_NSerie.Text = inv.Num_Serie;
                }
            }
            else
            {

            }
        }
    }

    private void Entry_Desc_Focused(object sender, FocusEventArgs e)
    {
        Entry_Desc.BackgroundColor = Colors.Yellow;

    }

    private void Entry_Desc_Unfocused(object sender, FocusEventArgs e)
    {
        Entry_Desc.BackgroundColor = Colors.White;

    }

    private void Entry_Desc_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void Entry_Desc_Completed(object sender, EventArgs e)
    {
        Picker_Etat.IsEnabled = true;
        Entry_NSerie.IsEnabled = true;
        Entry_OldCode.IsEnabled = true;
    }

    private void Entry_Immo_TextChanged(object sender, EventArgs e)
    {
        if (Entry_Immo != null && !String.IsNullOrWhiteSpace(Entry_Immo.Text))
        {
            Img_Button_Filter.IsEnabled = true;
            Entry_NSerie.IsEnabled = false;
            Picker_Etat.IsEnabled = false;
        }
        else
        {
            Picker_Description.SelectedItem = new Nature();
            Img_Button_Filter.IsEnabled = false;
            Entry_NSerie.Text = "";
            CameraButton.IsEnabled = false;
            Entry_NSerie.IsEnabled = false;
            Picker_Etat.IsEnabled = false;
            Entry_Immo.Focus();
        }
    }

    private void Entry_NSerie_TextChanged(object sender, EventArgs e)
    {

    }

    private void Entry_Nature_TextChanged(object sender, EventArgs e)
    {
        LibNatureTH = Entry_Nature.Text;
    }

    private void Entry_Desc_TextChanged(object sender, EventArgs e)
    {

    }
}