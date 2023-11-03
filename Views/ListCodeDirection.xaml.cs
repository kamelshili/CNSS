using CNSSInv.Models;
using CNSSInv.ViewModels;
using CNSSInv.Services;
namespace CNSSInv.Views;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ListCodeDirection : ContentPage
{
    public ListCodeDirection()
    {
        InitializeComponent();
    }
    public static List<UniteBudgetaire> ListUnitBud { get; set; } = new List<UniteBudgetaire>();
    public static UniteBudgetaire MyUnitBud { get; set; } = new UniteBudgetaire();
    public static bool Close { get; set; } = false;
    //on va faire les initialisation suivant avec les valeurs de la page  inventaire
    public ListCodeDirection(NextUniteBufgetaireViewModel nextUniteBufgetaireViewModel, UniteBudgetaire uniteBudgetaire)
    {
        //on faire cette valeur sur false car le compilateur compile la méthode listEmpl_ItemSelected la premiére méthode et si on a exécute la méthode OnDisappearing la valeur close sera true et il ne va pas exécute la méthode listEmpl_ItemSelected  donc on initialise close avec false.
        Close = false;
        InitializeComponent();
        BindingContext = nextUniteBufgetaireViewModel;
        List<UniteBudgetaire> list = ((List<UniteBudgetaire>)listEmpl.ItemsSource);
        ListUnitBud = list;
        MyUnitBud = uniteBudgetaire;
    }

    //cette méthode fonctionne a chaque fois la page apparait
    protected override void OnAppearing()
    {
        base.OnAppearing();
        verifempl = false;
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
            int nbr = UniteBudgetaireDatabaseController.GetCountUnitBudgetaireByCodeDirection(keyword);
            if (nbr > 0)
            {
                listEmpl.ItemsSource =
                 ListUnitBud.Where(i => i.Code_Direction.ToLower().Contains(keyword.ToLower()));
            }
            else
            {
                listEmpl.ItemsSource = new List<UniteBudgetaire>();
            }
        }
    }

    //cette méthode pour quitter la page a chaque fois on clique sur un item avec une condition de ne pas quitter si la valeur sélectionner est null
    private async void listEmpl_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
    }

    //cette méthode fonctionne a chaque fois quand on quitte la page avec une condition si la valeur sélectionner quand on quitte null changer la valeur MyUniteBudgetaire de la page Inventaire qui est changé dans le constrecture on faire cétte opération pour ne perdre pas aucune donnée. 
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
                //on va fiare une animation lors de quitter la page
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

    private void txt_search_Focused(object sender, FocusEventArgs e)
    {
        txt_search.BackgroundColor = Colors.Yellow;
    }

    private void txt_search_Unfocused(object sender, FocusEventArgs e)
    {
        txt_search.BackgroundColor = Colors.White;
    }

    public static bool verifempl = false;

    public static UniteBudgetaire MyNewUnitBud { get; set; } = new UniteBudgetaire();

    private async void listEmpl_ItemTapped(object sender, ItemTappedEventArgs e)
    {
    }

    private void txt_search_TextChanged(object sender, EventArgs e)
    {
        var keyword = txt_search.Text;
        lstchanged(keyword);
    }

    private async void listEmpl_Tap(object sender, DevExpress.Maui.DataGrid.DataGridGestureEventArgs e)
    {
        var selc = listEmpl.SelectedItem;
        if (selc != null && Close == false)
        {
            verifempl = true;
            MyNewUnitBud = (UniteBudgetaire)selc;
            await Navigation.PopAsync();
        }
        else if (selc == null)
        {
        }
    }
}