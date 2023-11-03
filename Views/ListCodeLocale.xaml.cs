using CNSSInv.Models;
using CNSSInv.ViewModels;
using CNSSInv.Services;
namespace CNSSInv.Views;

public partial class ListCodeLocale : ContentPage
{
    public static List<UniteBudgetaire> ListUnitBud { get; set; } = new List<UniteBudgetaire>();
    public static UniteBudgetaire MyUnitBud { get; set; } = new UniteBudgetaire();
    public static bool Close { get; set; } = false;
    public ListCodeLocale()
    {
        InitializeComponent();
    }
    public ListCodeLocale(NextUniteBufgetaireViewModel nextUniteBufgetaireViewModel, UniteBudgetaire uniteBudgetaire)
    {
        //on faire cette valeur sur false car le compilateur compile la méthode listEmpl_ItemSelected la premiére méthode et si on a exécute la méthode OnDisappearing la valeur close sera true et il ne va pas exécute la méthode listEmpl_ItemSelected  donc on initialise close avec false.
        Close = false;
        InitializeComponent();
        BindingContext = nextUniteBufgetaireViewModel;
        List<UniteBudgetaire> list = ((List<UniteBudgetaire>)listEmpl.ItemsSource);
        ListUnitBud = list;
        MyUnitBud = uniteBudgetaire;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    //on va changer la liste avec le contenu de txt_search si on n'écrit rien la liste sera chargée une autre fois c'est on écrit de donées n'existe pas dans la base la liste sera vide
    public async void lstchanged(string keyword)
    {
        if (keyword == "")
        {
            listEmpl.ItemsSource = ListUnitBud;
        }
        else
        {
            UniteBudgetaireDatabaseController UniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            int nbr = UniteBudgetaireDatabaseController.GetCountUnitBudgetaireByCodeLocale(keyword);
            if (nbr > 0)
            {
                listEmpl.ItemsSource =
                 ListUnitBud.Where(i => i.Code_Local.ToLower().Contains(keyword.ToLower()));
            }
            else
            {
                listEmpl.ItemsSource = new List<UniteBudgetaire>();
            }
        }
    }
    protected override void OnDisappearing()
    {
        var selc = listEmpl.SelectedItem;
        if (selc == null)
        {
            //je faire cette initialisation pour faire un appele indirecte a méthode txt_search_TextChanged et initialiser la liste des UniteBudgetaires avec tous les données existe dans la bdd
            txt_search.Text = "";
            listEmpl.IsVisible = false;
            if (selc == null)
            {
                selc = MyUnitBud;
                Close = true;
                //la liste est maintenant chargée comme le début donc on peut metrre la valeur sélectionner avec succée
                listEmpl.SelectedItem = selc;
                base.OnDisappearing();
            }
        }
        else
        {
            base.OnDisappearing();
        }
    }
    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        var stackLayout = sender as StackLayout;
        listEmpl.SelectedItem = stackLayout.BindingContext;
    }
    private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
    {
        var stackLayout = sender as StackLayout;
        listEmpl.SelectedItem = stackLayout.BindingContext;
        var selc = listEmpl.SelectedItem;
        if (selc != null && Close == false)
        {
            await Navigation.PopAsync();
        }
        else if (selc == null)
        {
        }
    }

    public static bool verifemplSearch = false;
    public static UniteBudgetaire MyNewUnitBudSearch { get; set; } = new UniteBudgetaire();
    private async void ToolbarItemAdd_Clicked(object sender, EventArgs e)
    {
        if (txt_search.Text != null && !String.IsNullOrWhiteSpace(txt_search.Text))
        {
            UniteBudgetaire uniteBudgetaire1 = new UniteBudgetaire(txt_search.Text, null, null, null, null, null, null, null);
            UniteBudgetaire uniteBudgetaire2 = new UniteBudgetaire(txt_search.Text, "VIDE", "VIDE", "VIDE", "VIDE", "VIDE", "VIDE", "VIDE");
            UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            int nbr = uniteBudgetaireDatabaseController.GetCountUnitBudgetaireByCodeLocale(uniteBudgetaire2.Code_Local);
            if (nbr == 0)
            {
                await uniteBudgetaireDatabaseController.SaveUniteBudgetaire(uniteBudgetaire2);
                List<UniteBudgetaire> list = await uniteBudgetaireDatabaseController.GetCodeLocaleUniteBudgetaires();
                ListUnitBud = list;
                await Navigation.PopAsync();
            }
        }
    }


    private void txt_search_TextChanged(object sender, EventArgs e)
    {
        var keyword = txt_search.Text;
        lstchanged(keyword);
    }

    private void txt_search_Unfocused(object sender, FocusEventArgs e)
    {
        txt_search.BackgroundColor = Colors.Yellow;
    }

    private void txt_search_Focused(object sender, FocusEventArgs e)
    {
        txt_search.BackgroundColor = Colors.Yellow;
    }
    public static bool verifempl = false;
    public static UniteBudgetaire MyNewUnitBud { get; set; } = new UniteBudgetaire();
    public Entry codeEntry;
    private async void listEmpl_ItemTapped(object sender, DevExpress.Maui.DataGrid.DataGridGestureEventArgs e)
    {
        var selc = listEmpl.SelectedItem;
        if (selc != null && Close == false)
        {
            verifempl = true;
            MyNewUnitBud = (UniteBudgetaire)selc;
            //codeEntry.Text = MyNewUnitBud.Code_Local;
            await Navigation.PopAsync();
        }
        else if (selc == null)
        {
            //await DisplayAlert("Selection Error", "Please select an item.", "OK");
        }
    }
}