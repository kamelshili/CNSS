using System;
using System.Collections.Generic;
using System.ComponentModel;
using CNSSInv.Models;
using CNSSInv.ViewModels;
using CNSSInv.Services;
using System.Threading.Tasks;
using System.Threading;

namespace CNSSInv.Views;
    public partial class NextUniteBudgetaire : ContentPage
    {
    public Item Item { get; set; }
    public static NextUniteBufgetaireViewModel nextUniteBufgetaireViewModel { get; set; } = new NextUniteBufgetaireViewModel();
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UniteBudgetaire uniteBudgetaire = new UniteBudgetaire("VIDE", "VIDE", "VIDE", "VIDE", "VIDE", "VIDE", "VIDE", "VIDE");
        UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
        uniteBudgetaireDatabaseController.SaveUniteBudgetaire(uniteBudgetaire);
        if (ListCodeLocale.verifemplSearch)
        {
            Picker_Locale.ItemsSource = ListCodeLocale.ListUnitBud;
            for (int i = 0; i < ListCodeLocale.ListUnitBud.Count; i++)
            {
                if (ListCodeLocale.ListUnitBud[i].Code_Local == ListCodeLocale.MyNewUnitBudSearch.Code_Local)
                {
                    Picker_Locale.SelectedItem = i;
                    Picker_Locale.SelectedItem = ListCodeLocale.ListUnitBud[i];
                    break;
                }
            }
            ListCodeLocale.verifemplSearch = false;
        }

        if (ListCodeLocale.verifempl)
        {
            Picker_Locale.ItemsSource = new List<UniteBudgetaire>();
            Picker_Locale.ItemsSource = ListCodeLocale.ListUnitBud;
            Picker_Locale.SelectedItem = ListCodeLocale.MyNewUnitBud;
            ListCodeLocale.verifempl = false;
        }

        if (ListEmpl.verifempl)
        {
            Picker_Empl.ItemsSource = new List<UniteBudgetaire>();
            Picker_Empl.ItemsSource = ListEmpl.ListUnitBud;
            Picker_Empl.SelectedItem = ListEmpl.MyNewUnitBud;
            ListEmpl.verifempl = false;
        }

        if (ListCodeDirection.verifempl)
        {
            Picker_CodeDirection.ItemsSource = new List<UniteBudgetaire>();
            Picker_CodeDirection.ItemsSource = ListCodeDirection.ListUnitBud;
            Picker_CodeDirection.SelectedItem = ListCodeDirection.MyNewUnitBud;
        }

        if (ListCodeUB.verifempl)
        {
            Picker_CodeUB.ItemsSource = new List<UniteBudgetaire>();
            Picker_CodeUB.ItemsSource = ListCodeUB.ListUnitBud;
            Picker_CodeUB.SelectedItem = ListCodeUB.MyNewUnitBud;
            ListCodeUB.verifempl = false;
        }

        if (!String.IsNullOrEmpty(Lbl_Test.Text) && Lbl_Test.Text == "b")
        {
            nextUniteBufgetaireViewModel = new NextUniteBufgetaireViewModel();
            Thread.Sleep(2000);
            BindingContext = nextUniteBufgetaireViewModel;
            Thread.Sleep(2000);
            Lbl_Test.Text = "a";
        }
    }
    public NextUniteBudgetaire()
    {
        InitializeComponent();
        BindingContext = nextUniteBufgetaireViewModel;
        Picker_Zone.SelectedIndexChanged += (s, e) => OnListViewZoneItemSelected(s, e);
        Picker_Site.SelectedIndexChanged += (s, e) => OnListViewSiteItemSelected(s, e);
        Picker_Empl.SelectionChanged += (s, e) => OnListViewEmpItemSelected(s, e);
        Picker_CodeUB.SelectionChanged += (s, e) => OnListViewCodeItemSelected(s, e);
        Picker_CodeDirection.SelectionChanged += (s, e) => OnListViewCodeDirectionItemSelected(s, e);
    }
    void OnListViewZoneItemSelected(object sender, EventArgs e)
    {
        Picker_Site.IsEnabled = true;
        Img_Button_Filter_Site.IsEnabled = true;
    }
    void OnListViewSiteItemSelected(object sender, EventArgs e)
    {
        Picker_Empl.IsEnabled = true;
        Img_Button_Filter_Empl.IsEnabled = true;
    }

    void OnListViewEmpItemSelected(object sender, EventArgs e)
    {
        Img_Button_Filter_Code_Local.IsEnabled = false;
        if (Picker_Lib_UB.Text != null && Picker_Lib_UB.Text != "")
            Picker_Lib_UB.Text = "";
        if (Picker_Locale.SelectedIndex == -1)
        {
            Picker_Locale.IsEnabled = false;
            Picker_CodeDirection.IsEnabled = true;
            Img_Button_Filter_CodeDirection.IsEnabled = true;

        }
    }

    void OnListViewCodeDirectionItemSelected(object sender, EventArgs e)
    {
        if (Picker_Locale.SelectedIndex == -1)
        {
            Picker_CodeUB.IsEnabled = true;
            Img_Button_Filter_CodeUB.IsEnabled = true;
        }
        if (ListCodeDirection.verifempl && Picker_CodeDirection.ItemsSource != null && ((List<UniteBudgetaire>)Picker_CodeDirection.ItemsSource).Count != 0)
        {
            Picker_CodeDirection.ItemsSource = ListCodeDirection.ListUnitBud;
            Picker_CodeDirection.SelectedItem = ListCodeDirection.MyNewUnitBud;
            ListCodeDirection.verifempl = false;
        }
    }
    void OnListViewCodeItemSelected(object sender, EventArgs e)
    {
        if (Picker_Locale.SelectedIndex == -1)
        {
            Picker_CodeUB.IsEnabled = true;
        }
    }
    private async void OnNext()
    {
        var codelocale = string.Empty;
        if (Picker_Locale.SelectedIndex != -1)
            codelocale = ((UniteBudgetaire)Picker_Locale.SelectedItem).Code_Local;
        UniteBudgetaire uniteBudgetaire = new UniteBudgetaire(codelocale, ((UniteBudgetaire)Picker_CodeUB.SelectedItem).Code_UB, Picker_Lib_UB.Text, ((UniteBudgetaire)Picker_Empl.SelectedItem).EMPL, Picker_Lib_BATIMENT.Text, Picker_Lib_Direction.Text, string.Empty, ((UniteBudgetaire)Picker_CodeDirection.SelectedItem).Code_Direction);
        Page page = (Page)Activator.CreateInstance(typeof(Views.Inventaire), uniteBudgetaire);
        await Navigation.PushAsync(page, false);
        activity.IsRunning = false;
    }
    private void btn_Suivant_Clicked(object sender, EventArgs e)
    {
        activity.IsRunning = true;
        Device.BeginInvokeOnMainThread(() =>
        {
            OnNext();
        });
    }

    private void Picker_Zone_Focused(object sender, FocusEventArgs e)
    {
        Picker_Zone.BackgroundColor = Colors.Yellow;

    }

    private void Picker_Zone_Unfocused(object sender, FocusEventArgs e)
    {
        Picker_Zone.BackgroundColor = Colors.White;
    }

    private void Picker_Site_Focused(object sender, FocusEventArgs e)
    {
        Picker_Site.BackgroundColor = Colors.Yellow;
    }

    private void Picker_Site_Unfocused(object sender, FocusEventArgs e)
    {
        Picker_Site.BackgroundColor = Colors.White;
    }

    private void Picker_Empl_Focused(object sender, FocusEventArgs e)
    {
        Picker_Empl.BackgroundColor = Colors.Yellow;
    }

    private void Picker_Empl_Unfocused(object sender, FocusEventArgs e)
    {
        Picker_Empl.BackgroundColor = Colors.White;
    }

    private void Picker_CodeUB_Focused(object sender, FocusEventArgs e)
    {
        Picker_CodeUB.BackgroundColor = Colors.Yellow;
    }

    private void Picker_CodeUB_Unfocused(object sender, FocusEventArgs e)
    {
        Picker_CodeUB.BackgroundColor = Colors.White;
    }

    private async void Img_Button_Filter_Zone_Clicked(object sender, EventArgs e)
    {
        var desc = Picker_Zone.SelectedItem;
        Page page = (Page)Activator.CreateInstance(typeof(ListZone), nextUniteBufgetaireViewModel, desc);
        await Navigation.PushAsync(page, false);
    }

    private async void Img_Button_Filter_Site_Clicked(object sender, EventArgs e)
    {
        var desc = Picker_Site.SelectedItem;
        Page page = (Page)Activator.CreateInstance(typeof(ListSite), nextUniteBufgetaireViewModel, desc);
        await Navigation.PushAsync(page, false);
    }

    private async void Img_Button_Filter_Empl_Clicked(object sender, EventArgs e)
    {
        var desc = Picker_Empl.SelectedItem;
        Page page = (Page)Activator.CreateInstance(typeof(ListEmpl), nextUniteBufgetaireViewModel, desc);
        await Navigation.PushAsync(page, false);
    }

    private async void Img_Button_Filter_CodeUB_Clicked(object sender, EventArgs e)
    {
        var desc = Picker_CodeUB.SelectedItem;

        Page page = (Page)Activator.CreateInstance(typeof(ListCodeUB), nextUniteBufgetaireViewModel, desc);
        await Navigation.PushAsync(page, false);
    }

    private void btn_Logout_Clicked(object sender, EventArgs e)
    {
    }

    private async void btn_Affiche_Clicked(object sender, EventArgs e)
    {
        var desc = Picker_Empl.SelectedItem;
        var codeub = Picker_CodeUB.SelectedItem;
        var codedirection = Picker_CodeDirection.SelectedItem;
        Page page = (Page)Activator.CreateInstance(typeof(ListInventaire), desc, codeub, codedirection);
        await Navigation.PushAsync(page, false);
    }

    private void Picker_CodeUB_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void Picker_CodeDirection_Focused(object sender, FocusEventArgs e)
    {
        Picker_CodeDirection.BackgroundColor = Colors.Yellow;
    }

    private void Picker_CodeDirection_Unfocused(object sender, FocusEventArgs e)
    {
        Picker_CodeDirection.BackgroundColor = Colors.White;
    }

    private void Picker_CodeDirection_SelectedIndexChanged(object sender, EventArgs e)
    {
        Img_Button_Filter_Empl.IsEnabled = false;
    }

    private async void Img_Button_Filter_CodeDirection_Clicked(object sender, EventArgs e)
    {

        var desc = Picker_CodeDirection.SelectedItem;
        var codeub = Picker_CodeUB.SelectedItem;
        Page page = (Page)Activator.CreateInstance(typeof(ListCodeDirection), nextUniteBufgetaireViewModel, desc);
        await Navigation.PushAsync(page, false);
    }

    private void Picker_Locale_Focused(object sender, FocusEventArgs e)
    {
        Picker_Locale.BackgroundColor = Colors.Yellow;
    }

    private void Picker_Locale_Unfocused(object sender, FocusEventArgs e)
    {
        Picker_Locale.BackgroundColor = Colors.White;
    }

    private async void Img_Button_Filter_Code_Local_Clicked(object sender, EventArgs e)
    {
        var desc = Picker_Locale.SelectedItem;
        var codeub = Picker_CodeUB.SelectedItem;
        Page page = (Page)Activator.CreateInstance(typeof(ListCodeLocale), nextUniteBufgetaireViewModel, desc);
        await Navigation.PushAsync(page, false);
    }

    private async void LoadEmp()
    {
        var codelocale = ((UniteBudgetaire)Picker_Locale.SelectedItem).Code_Local;
        UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
        UniteBudgetaire unitebudg = await uniteBudgetaireDatabaseController.GetUniteBudgetaireByCodeLocale(codelocale);
        // var ub = new UniteBudgetaire(null, null, unitebudg.EMPL, null, null, null, null);
        //var kjkdkd = ((List<UniteBudgetaire>)Picker_Empl.ItemsSource)[0];
        var list = ((List<UniteBudgetaire>)Picker_Empl.ItemsSource);
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].EMPL == unitebudg.EMPL)
            {
                Picker_Empl.SelectedIndex = i;
                break;
            }

        }
    }

    public static int nbr { get; set; }
    private async void LoadCodeDirection()
    {
        var codelocale = ((UniteBudgetaire)Picker_Locale.SelectedItem).Code_Local;
        UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
        UniteBudgetaire unitebudg = await uniteBudgetaireDatabaseController.GetUniteBudgetaireByCodeLocale(codelocale);
        Picker_CodeDirection.ItemsSource = await uniteBudgetaireDatabaseController.GetCodeDirectionUniteBudgetaires(unitebudg.EMPL, unitebudg.SITE);
        var list = ((List<UniteBudgetaire>)Picker_CodeDirection.ItemsSource);
        for (int i = 0; i < list.Count; i++)
        {

            if (list[i].Code_Direction == unitebudg.Code_Direction)
            {
                Picker_CodeDirection.SelectedIndex = i;
                break;
            }
        }
    }

    private async void LoadCodeUB()
    {
        var codelocale = ((UniteBudgetaire)Picker_Locale.SelectedItem).Code_Local;
        UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
        UniteBudgetaire unitebudg = await uniteBudgetaireDatabaseController.GetUniteBudgetaireByCodeLocale(codelocale);
        Picker_Lib_BATIMENT.Text = unitebudg.SITE;
        Picker_Lib_Direction.Text = unitebudg.ZONE;
        Picker_Lib_UB.Text = unitebudg.Lib_UB;
        Picker_CodeUB.ItemsSource = await uniteBudgetaireDatabaseController.GetCodeUniteBudgetaires(unitebudg.EMPL, unitebudg.Code_Direction, Picker_Lib_BATIMENT.Text, Picker_Lib_Direction.Text);
        var list = ((List<UniteBudgetaire>)Picker_CodeUB.ItemsSource);
        for (int i = 0; i < list.Count; i++)
        {

            if (list[i].Code_UB == unitebudg.Code_UB)
            {
                Picker_CodeUB.SelectedIndex = i;
                break;
            }
        }
    }
    public static bool verif { get; set; } = false;
    private void Picker_Locale_SelectedIndexChanged(object sender, EventArgs e)
    {
        UniteBudgetaire uniteBudgetaire = new UniteBudgetaire();
        if (Picker_Locale.SelectedItem != null && ((UniteBudgetaire)Picker_Locale.SelectedItem).Code_Local != null)
        {
            Picker_Empl.IsEnabled = false;
            Picker_CodeDirection.IsEnabled = false;
            Picker_CodeUB.IsEnabled = false;
            Img_Button_Filter_CodeDirection.IsEnabled = false;
            Img_Button_Filter_Code_Local.IsEnabled = false;
            Img_Button_Filter_Empl.IsEnabled = false;
            Img_Button_Filter_CodeUB.IsEnabled = false;
        }
        else if (Picker_Locale.SelectedItem != null && ((UniteBudgetaire)Picker_Locale.SelectedItem).Code_Local != null)
        {
            Picker_Locale.IsEnabled = false;
            Picker_Empl.IsEnabled = false;
            Picker_CodeDirection.IsEnabled = false;
            Picker_CodeUB.IsEnabled = false;
            Img_Button_Filter_CodeDirection.IsEnabled = false;
            Img_Button_Filter_Code_Local.IsEnabled = false;
            Img_Button_Filter_Empl.IsEnabled = false;
            Img_Button_Filter_CodeUB.IsEnabled = false;
            Picker_Empl.SelectedIndex = -1;
            Picker_CodeDirection.SelectedIndex = -1;
            Picker_CodeUB.SelectedIndex = -1;
            Picker_Lib_BATIMENT.Text = "";
            Picker_Lib_Direction.Text = "";
            Picker_Lib_UB.Text = "";
            LoadEmp();
            LoadCodeDirection();
            LoadCodeUB();
            verif = true;

        }
    }
    private async void AllCodeLocalUniteBudgetaires()
    {
        Picker_Locale.ItemsSource = new List<UniteBudgetaire>();
        UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
        Picker_Locale.ItemsSource = await uniteBudgetaireDatabaseController.GetCodeLocaleUniteBudgetaires();
    }

    private void btn_Cancel_Clicked(object sender, EventArgs e)
    {
        btn_Cancel.IsEnabled = false;
        Picker_Locale.SelectedIndex = -1;
        Picker_Empl.SelectedIndex = -1;
        Picker_CodeDirection.SelectedIndex = -1;
        Picker_CodeUB.SelectedIndex = -1;
        Picker_Lib_BATIMENT.Text = "";
        Picker_Lib_Direction.Text = "";
        Picker_Lib_UB.Text = "";
        //AllCodeLocalUniteBudgetaires();
        Picker_Locale.IsEnabled = true;
        Picker_Empl.IsEnabled = true;
        Picker_CodeDirection.IsEnabled = false;
        Picker_CodeUB.IsEnabled = false;
        Img_Button_Filter_Empl.IsEnabled = true;
        Img_Button_Filter_Code_Local.IsEnabled = true;
        btn_Cancel.IsEnabled = true;
    }

    private void Picker_Lib_UB_BindingContextChanged(object sender, EventArgs e)
    {

    }

    private async void Picker_Lib_UB_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        //if (Picker_Locale.SelectedIndex == -1 && !String.IsNullOrEmpty(Picker_Lib_UB.Text))
        //{
        //    UniteBudgetaireDatabaseController uniteBudgetaire = new UniteBudgetaireDatabaseController();
        //    var unite = await uniteBudgetaire.GetUniteBudgetaireByAll(((UniteBudgetaire)Picker_CodeUB.SelectedItem).Code_UB, ((UniteBudgetaire)Picker_Empl.SelectedItem).EMPL, ((UniteBudgetaire)Picker_CodeDirection.SelectedItem).Code_Direction, Picker_Lib_BATIMENT.Text, Picker_Lib_Direction.Text, Picker_Lib_UB.Text);
        //    if (unite != null)
        //    {
        //        var item = ((List<UniteBudgetaire>)Picker_Locale.ItemsSource).Find(i => i.Code_Local == unite.Code_Local);
        //        //  .IndexOf(unite);
        //        Picker_Locale.SelectedItem = item;
        //    }
        //}
    }
}