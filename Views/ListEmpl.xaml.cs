using CNSSInv.Models;
using CNSSInv.ViewModels;
using CNSSInv.Services;
namespace CNSSInv.Views;

public partial class ListEmpl : ContentPage
{
	public ListEmpl()
	{
		InitializeComponent();
	}
    public static List<UniteBudgetaire> ListUnitBud { get; set; } = new List<UniteBudgetaire>();
    public static UniteBudgetaire MyUnitBud { get; set; } = new UniteBudgetaire();
    public static bool Close { get; set; } = false;
    public static bool verifempl = false;
    public ListEmpl(NextUniteBufgetaireViewModel nextUniteBufgetaireViewModel, UniteBudgetaire uniteBudgetaire)
    {
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
        Close = false;
        verifempl = false;
    }

    public async void lstchanged(string keyword)
    {
        if (keyword == "")
        {
            listEmpl.ItemsSource = ListUnitBud;
        }
        else
        {
            UniteBudgetaireDatabaseController UniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            int nbr = UniteBudgetaireDatabaseController.GetCountUnitBudgetaireByEmpl(keyword);
            if (nbr > 0)
            {
                listEmpl.ItemsSource =
                 ListUnitBud.Where(i => i.EMPL.ToLower().Contains(keyword.ToLower()));
                var lstInventory = await UniteBudgetaireDatabaseController.GetUniteBudgetaireByEmpl(keyword);
            }
            else
            {
                listEmpl.ItemsSource = new List<UniteBudgetaire>();
            }
        }
    }


    private void txt_search_Focused(object sender, FocusEventArgs e)
    {
        txt_search.BackgroundColor = Colors.Yellow;
    }

    private void txt_search_Unfocused(object sender, FocusEventArgs e)
    {
        txt_search.BackgroundColor = Colors.Yellow;
    }

    private void txt_search_TextChanged(object sender, EventArgs e)
    {
        var keyword = txt_search.Text;
        lstchanged(keyword);
    }

    private void listEmpl_ItemSelected(object sender, DevExpress.Maui.DataGrid.SelectionChangedEventArgs e)
    { 
    }
    public static UniteBudgetaire MyNewUnitBud { get; set; } = new UniteBudgetaire();

    private async  void listEmpl_ItemTapped(object sender, DevExpress.Maui.DataGrid.DataGridGestureEventArgs e)
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

    private void listEmpl_SizeChanged(object sender, EventArgs e)
    {
    }
}