using CNSSInv.Models;
using Plugin.Media;
using Plugin.Toast;

namespace CNSSInv.Views;
public partial class TakePicture : ContentPage
{
    public static string Pathpicture { get; set; } = string.Empty;
    public static string NewPathpicture { get; set; } = string.Empty;

    public static string Numimmo { get; set; } = string.Empty;

    public TakePicture(string numimmo)
    {
        InitializeComponent();
        Numimmo = numimmo + ".jpg";
        NewPathpicture = Constants.pathPictureFolder + Numimmo;
        if (File.Exists(NewPathpicture))
        {
            PhotoImage.Source = ImageSource.FromFile(NewPathpicture);
        }
        else
        {
        }
        CameraButton.Clicked += CameraButton_Clicked;
    }

    private async void Take_Picture()
    {
        await CrossMedia.Current.Initialize();

        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
        {
            await DisplayAlert("No Camera", ":( No camera available.", "OK");
            return;
        }

        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
        {
            DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
            AllowCropping = true,
            Name = Numimmo
        });
        if (file == null)
            return;
        CrossToastPopUp.Current.ShowToastMessage("File Location" + file.Path + "OK");
        Pathpicture = NewPathpicture;
        PhotoImage.Source = ImageSource.FromStream(() =>
        {
            var stream = file.GetStream();
            return stream;
        });
    }

    private void Delete_Picture()
    {
        File.Delete(Constants.pathPictureFolder + Numimmo);
        PhotoImage.Source = null;
        Pathpicture = "";
    }

    private async void CameraButton_Clicked(object sender, EventArgs e)
    {
        if (PhotoImage.Source != null)
        {
            var action = await DisplayAlert("Delete?", " Si vous cliquer sur cette bouton l'ancien photo sera supprimer, êtes-vous sûr de vouloir continuer? ", "Oui", "Non");
            if (action)
            {
                Delete_Picture();
                Take_Picture();
            }
        }
        else
            Take_Picture();
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (PhotoImage.Source != null)
        {
            Delete_Picture();
        }
    }
}