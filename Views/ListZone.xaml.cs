using CNSSInv.Models;
using CNSSInv.ViewModels;
using CNSSInv.Services;

namespace CNSSInv.Views;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ListZone : ContentPage
{
	public ListZone()
	{
		InitializeComponent();
	}

    public static List<UniteBudgetaire> ListUnitBud { get; set; } = new List<UniteBudgetaire>();
    public static UniteBudgetaire MyUnitBud { get; set; } = new UniteBudgetaire();
    public static bool Close { get; set; } = false;
    //on va faire les initialisation suivant avec les valeurs de la page  inventaire

    public ListZone(NextUniteBufgetaireViewModel nextUniteBufgetaireViewModel, UniteBudgetaire uniteBudgetaire)
    {
        //on faire cette valeur sur false car le compilateur compile la méthode listZone_ItemSelected la premiére méthode et si on a exécute la méthode OnDisappearing la valeur close sera true et il ne va pas exécute la méthode listZone_ItemSelected  donc on initialise close avec false.
        Close = false;
        InitializeComponent();
        BindingContext = nextUniteBufgetaireViewModel;
        List<UniteBudgetaire> list = ((List<UniteBudgetaire>)listZone.ItemsSource);
        ListUnitBud = list;
        MyUnitBud = uniteBudgetaire;
    }
    //cette méthode fonctionne a chaque fois la page apparait
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    public async void lstchanged(string keyword)
    {
        if (keyword == "")
        {
            listZone.ItemsSource = ListUnitBud;
        }
        else
        {
            UniteBudgetaireDatabaseController UniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            int nbr = UniteBudgetaireDatabaseController.GetCountUnitBudgetaireByZone(keyword);
            if (nbr > 0)
            {
                listZone.ItemsSource =
                 ListUnitBud.Where(i => i.ZONE.ToLower().Contains(keyword.ToLower()));
                var lstInventory = await UniteBudgetaireDatabaseController.GetUniteBudgetaireByZone(keyword);
            }
            else
            {
                listZone.ItemsSource = new List<UniteBudgetaire>();
            }
        }
    }

    private async void listZone_ItemSelected(object sender, DevExpress.Maui.DataGrid.DataGridGestureEventArgs e)
    {
        var selc = listZone.SelectedItem;
        if (selc != null && Close == false)
        {
            await Navigation.PopAsync();
        }
        else if (selc == null)
        {
        }
    }
    protected override void OnDisappearing()
    {
        var selc = listZone.SelectedItem;
        if (selc == null)
        {
            //je faire cette initialisation pour faire un appele indirecte a méthode txt_search_TextChanged et initialiser la liste des UniteBudgetaires avec tous les données existe dans la bdd
            txt_search.Text = "";
            listZone.IsVisible = false;
            if (selc == null)
            {
                selc = MyUnitBud;
                Close = true;
                listZone.SelectedItem = selc;
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
        listZone.SelectedItem = stackLayout.BindingContext;
    }

    private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
    {
        var stackLayout = sender as StackLayout;
        listZone.SelectedItem = stackLayout.BindingContext;
        var selc = listZone.SelectedItem;
        if (selc != null && Close == false)
        {
            await Navigation.PopAsync();
        }
        else if (selc == null)
        {
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
}