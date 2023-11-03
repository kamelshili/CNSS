using CNSSInv.Models;
using CNSSInv.ViewModels;
using CNSSInv.Services;
namespace CNSSInv.Views;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ListInventaire : ContentPage
{
    public  ListInventaire()
    {
        InitializeComponent();
    }
    public static List<Models.Inventaire> ListInv { get; set; } = new List<Models.Inventaire>();
    public static UniteBudgetaire MyUnitBud { get; set; } = new UniteBudgetaire();
    public static bool Close { get; set; } = false;
    public async void initListInventaire(UniteBudgetaire uniteBudgetaireEmpl, UniteBudgetaire uniteBudgetaireCode, UniteBudgetaire uniteBudgetaireCodeDirection)
    {
        InventaireDatabaseController inventaireDatabaseController = new InventaireDatabaseController();
        List<Models.Inventaire> list = await inventaireDatabaseController.GetInventaireByEmplCodeUB(uniteBudgetaireEmpl.EMPL, uniteBudgetaireCode.Code_UB, uniteBudgetaireCodeDirection.Code_Direction);
        listInventaire.ItemsSource = list;
        ListInv = list;
        Lbl_Total.Text += list.Count.ToString();
    }
    public ListInventaire(UniteBudgetaire uniteBudgetaireSite, UniteBudgetaire uniteBudgetaireCode, UniteBudgetaire uniteBudgetaireCodeDirection)
    {
        InitializeComponent();
        initListInventaire(uniteBudgetaireSite, uniteBudgetaireCode, uniteBudgetaireCodeDirection);
    }

    //cette méthode fonctionne a chaque fois la page apparait
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    //on va changer la liste avec le contenu de txt_search si on n'écrit rien la liste sera chargée une autre fois c'est on écrit de donées n'existe pas dans la base la liste sera vide
    public void lstchanged(string keyword)
    {
        if (keyword == "")
        {
            listInventaire.ItemsSource = ListInv;
        }
        else
        {
            InventaireDatabaseController inventaireDatabaseController = new InventaireDatabaseController();
            int nbr = inventaireDatabaseController.GetCountInventaireByNumImmo(keyword);
            if (nbr > 0)
            {
                listInventaire.ItemsSource =
                 ListInv.Where(i => i.NumImmo.ToLower().Contains(keyword.ToLower()));
            }
            else
            {
                listInventaire.ItemsSource = new List<Models.Inventaire>();
            }
        }
    }

    //cette méthode pour quitter la page a chaque fois on clique sur un item avec une condition de ne pas quitter si la valeur sélectionner est null
    private async void listInventaire_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selc = listInventaire.SelectedItem;
        if (selc != null && Close == false)
        {
            await Navigation.PopAsync();
        }
        else if (selc == null)
        {
        }
    }

    //cette méthode fonctionne a chaque fois quand on quitte la page avec une condition si la valeur sélectionner quand on quitte null changer la valeur MyUniteBudgetaire de la page Inventaire qui est changé dans le constrecture on faire cétte opération pour ne perdre pas aucune donnée. 
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
    }

    private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
    {
    }

    private void txt_search_Focused(object sender, FocusEventArgs e)
    {
        txt_search.BackgroundColor = Colors.Yellow;
    }

    private void txt_search_Unfocused(object sender, FocusEventArgs e)
    {
        txt_search.BackgroundColor = Colors.White;
    }

    private void txt_search_TextChanged(object sender, EventArgs e)
    {
        var keyword = txt_search.Text;
        lstchanged(keyword);
    }

}