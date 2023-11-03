using CNSSInv.Models;
using CNSSInv.ViewModels;
using CNSSInv.Services;

namespace CNSSInv.Views;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ListSite : ContentPage
{
	public ListSite()
	{
		InitializeComponent();
	}

    public static List<UniteBudgetaire> ListUnitBud { get; set; } = new List<UniteBudgetaire>();
    public static UniteBudgetaire MyUnitBud { get; set; } = new UniteBudgetaire();
    public static bool Close { get; set; } = false;
    //on va faire les initialisation suivant avec les valeurs de la page  inventaire
    public ListSite(NextUniteBufgetaireViewModel nextUniteBufgetaireViewModel, UniteBudgetaire uniteBudgetaire)
    {
        //on faire cette valeur sur false car le compilateur compile la méthode listSite_ItemSelected la premiére méthode et si on a exécute la méthode OnDisappearing la valeur close sera true et il ne va pas exécute la méthode listSite_ItemSelected  donc on initialise close avec false.
        Close = false;
        InitializeComponent();
        BindingContext = nextUniteBufgetaireViewModel;
        List<UniteBudgetaire> list = ((List<UniteBudgetaire>)listSite.ItemsSource);
        ListUnitBud = list;
        MyUnitBud = uniteBudgetaire;
    }

    //cette méthode fonctionne a chaque fois la page apparait
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    //on va changer la liste avec le contenu de txt_search si on n'écrit rien la liste sera chargée une autre fois c'est on écrit de donées n'existe pas dans la base la liste sera vide
    public async void lstchanged(string keyword)
    {
        if (keyword == "")
        {
            listSite.ItemsSource = ListUnitBud;
        }
        else
        {
            UniteBudgetaireDatabaseController UniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            int nbr = UniteBudgetaireDatabaseController.GetCountUnitBudgetaireBySite(keyword);
            if (nbr > 0)
            {
                listSite.ItemsSource =
                 ListUnitBud.Where(i => i.SITE.ToLower().Contains(keyword.ToLower()));
                var lstInventory = await UniteBudgetaireDatabaseController.GetUniteBudgetaireBySite(keyword);
            }
            else
            {
                listSite.ItemsSource = new List<UniteBudgetaire>();
            }
        }
    }

    private void txt_search_TextChanged(object sender, EventArgs e)
    {
        var keyword = txt_search.Text;
        lstchanged(keyword);
    }

    //cette méthode fonctionne a chaque fois quand on quitte la page avec une condition si la valeur sélectionner quand on quitte null changer la valeur MyUniteBudgetaire de la page Inventaire qui est changé dans le constrecture on faire cétte opération pour ne perdre pas aucune donnée. 
    protected override void OnDisappearing()
    {
        var selc = listSite.SelectedItem;
        if (selc == null)
        {
            //je faire cette initialisation pour faire un appele indirecte a méthode txt_search_TextChanged et initialiser la liste des UniteBudgetaires avec tous les données existe dans la bdd
            txt_search.Text = "";
            listSite.IsVisible = false;
            if (selc == null)
            {
                selc = MyUnitBud;
                Close = true;
                //la liste est maintenant chargée comme le début donc on peut metrre la valeur sélectionner avec succée
                listSite.SelectedItem = selc;
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
        listSite.SelectedItem = stackLayout.BindingContext;
    }
    private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
    {
        var stackLayout = sender as StackLayout;
        listSite.SelectedItem = stackLayout.BindingContext;
        var selc = listSite.SelectedItem;
        if (selc != null && Close == false)
        {
            await Navigation.PopAsync();
        }
        else if (selc == null)
        {
        }
    }

    private void txt_search_Focused(object sender, FocusEventArgs e)
    {
        txt_search.BackgroundColor = Colors.Yellow;
    }

    private void txt_search_Unfocused(object sender, FocusEventArgs e)
    {
        txt_search.BackgroundColor = Colors.White;
    }

    private async void listSite_ItemSelected(object sender, DevExpress.Maui.DataGrid.DataGridGestureEventArgs e)
    {
        var selc = listSite.SelectedItem;
        if (selc != null && Close == false)
        {
            await Navigation.PopAsync();
        }
        else if (selc == null)
        {
        }
    }
}
