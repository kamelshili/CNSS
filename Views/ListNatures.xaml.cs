using CNSSInv.Models;
using CNSSInv.Services;
using CNSSInv.ViewModels;

namespace CNSSInv.Views;
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ListNatures : ContentPage
{
    public static List<Nature> ListNat { get; set; } = new List<Nature>();
    public static Nature MyNature { get; set; } = new Nature();
    public static bool Close { get; set; } = false;
    //on va faire les initialisation suivant avec les valeurs de la page  inventaire
    public ListNatures(InventaireViewModel inventaireviewmodel, Nature nat)
    {
        //on faire cette valeur sur false car le compilateur compile la méthode listNature_ItemSelected la premiére méthode et si on a exécute la méthode OnDisappearing la valeur close sera true et il ne va pas exécute la méthode listNature_ItemSelected  donc on initialise close avec false.
        Close = false;
        InitializeComponent();
        BindingContext = inventaireviewmodel;
        List<Nature> list = ((List<Nature>)listNature.ItemsSource);
        ListNat = list;
        MyNature = nat;
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
            listNature.ItemsSource = ListNat;
        }
        else
        {
            NatureDatabaseController natureDatabaseController = new NatureDatabaseController();
            int nbr = natureDatabaseController.GetCountNatureByLibNat(keyword);
            if (nbr > 0)
            {
                listNature.ItemsSource =
                 ListNat.Where(i => i.Lib_Nature.ToLower().Contains(keyword.ToLower()));
                var lstInventory = await natureDatabaseController.GetNatureByLibNat(keyword);
            }
            else
            {
                listNature.ItemsSource = new List<Nature>();
            }
        }
    }

    //cette méthode pour quitter la page a chaque fois on clique sur un item avec une condition de ne pas quitter si la valeur sélectionner est null
    private async void listNature_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
    }

    //cette méthode fonctionne a chaque fois quand on quitte la page avec une condition si la valeur sélectionner quand on quitte null changer la valeur MyNature de la page Inventaire qui est changé dans le constrecture on faire cétte opération pour ne perdre pas aucune donnée. 
    protected override void OnDisappearing()
    {
        //var selc = listNature.SelectedItem;
        //if (selc == null)
        //{
        //    //je faire cette initialisation pour faire un appele indirecte a méthode txt_search_TextChanged et initialiser la liste des natures avec tous les données existe dans la bdd
        //    txt_search.Text = "";
        //    listNature.IsVisible = false;
        //    if (selc == null)
        //    {
        //        selc = MyNature;
        //        Close = true;
        //        //la liste est maintenant chargée comme le début donc on peut metrre la valeur sélectionner avec succée
        //        listNature.SelectedItem = selc;
        //        base.OnDisappearing();
        //    }
        //}
        //else
        //{
        //    base.OnDisappearing();
        //}
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        var stackLayout = sender as StackLayout;
        listNature.SelectedItem = stackLayout.BindingContext;
    }
    private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
    {
        var stackLayout = sender as StackLayout;
        listNature.SelectedItem = stackLayout.BindingContext;
        var selc = listNature.SelectedItem;
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
    public static Nature MyNewUnitBud { get; set; } = new Nature();

    private void txt_search_TextChanged(object sender, EventArgs e)
    {
        var keyword = txt_search.Text;
        lstchanged(keyword);
    }

    private async void listEmpl_ItemTapped(object sender, DevExpress.Maui.DataGrid.DataGridGestureEventArgs e)
    {
        var selc = listNature.SelectedItem;
        if (selc != null /*&& Close == false*/)
        {
            verifempl = true;
            MyNewUnitBud = (Nature)selc;
            await Navigation.PopAsync();
        }
        else if (selc == null)
        {
        }
    }
}