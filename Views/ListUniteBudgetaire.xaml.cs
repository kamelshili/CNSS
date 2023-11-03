using CNSSInv.Models;
using CNSSInv.Services;
using CNSSInv.ViewModels;

namespace CNSSInv.Views;

public partial class ListUniteBudgetaire : ContentPage
{
    public static NextUniteBufgetaireViewModel nextUniteBufgetaireViewModel { get; set; } = new NextUniteBufgetaireViewModel();
    public ListUniteBudgetaire()
	{
		InitializeComponent();
        AllUniteBudgetaires();
        BindingContext = nextUniteBufgetaireViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private async void AllUniteBudgetaires()
    {
        listUB.ItemsSource = new List<UniteBudgetaire>();
        UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
        listUB.ItemsSource = await uniteBudgetaireDatabaseController.GetAllUniteBudgetaires();
    }

    private async void OnNext()
    {
        var codelocale = string.Empty;
        if (listUB.SelectedItem != null)
        {
            codelocale = ((UniteBudgetaire)listUB.SelectedItem).Code_Local;
            UniteBudgetaire uniteBudgetaire = new UniteBudgetaire(codelocale, ((UniteBudgetaire)listUB.SelectedItem).Code_UB, ((UniteBudgetaire)listUB.SelectedItem).Lib_UB, ((UniteBudgetaire)listUB.SelectedItem).EMPL, ((UniteBudgetaire)listUB.SelectedItem).SITE, ((UniteBudgetaire)listUB.SelectedItem).ZONE, string.Empty, ((UniteBudgetaire)listUB.SelectedItem).Code_Direction);
            Page page = (Page)Activator.CreateInstance(typeof(Views.Inventaire), uniteBudgetaire);
            await Navigation.PushAsync(page, false);
            activity.IsRunning = false;
        }
    }

    private void btn_Suivant_Clicked(object sender, EventArgs e)
    {
        activity.IsRunning = true;
        Dispatcher.Dispatch(() =>
        {
            OnNext();
        });
    }

    private async void btn_Affiche_Clicked(object sender, EventArgs e)
    {
        var desc = ((UniteBudgetaire)listUB.SelectedItem);
        var codeub = ((UniteBudgetaire)listUB.SelectedItem);
        var codedirection = ((UniteBudgetaire)listUB.SelectedItem);
        Page page = (Page)Activator.CreateInstance(typeof(ListInventaire), desc, codeub, codedirection);
        await Navigation.PushAsync(page, false);
    }

    private async void btn_Logout_Clicked(object sender, EventArgs e)
    {
        listUB.ItemsSource = new List<UniteBudgetaire>();
        await Navigation.PopAsync();
    }

    private void btn_Cancel_Clicked(object sender, EventArgs e)
    {
        var list = (List<UniteBudgetaire>)listUB.ItemsSource;
        if (list != null && list.Count > 0 && listUB.SelectedItem != null)
        {
            var item = list[0];
            var items = ((UniteBudgetaire)listUB.SelectedItem);
            if (item != items)
            {
                listUB.SelectedItem = list[0];
            }
        }
    }

    private void listEmpl_ItemTapped(object sender, DevExpress.Maui.DataGrid.DataGridGestureEventArgs e)
    {
    }
}