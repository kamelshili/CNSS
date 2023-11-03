using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Input;
using CNSSInv.Models;
using CNSSInv.Services;
using CNSSInv.Views;
using Plugin.Toast;

namespace CNSSInv.ViewModels
{
    public class InventaireViewModel : BaseViewModel
    {
        private string numimmo;
        private string lib_nature;
        private string DateTimeScan { get; set; }
        private string Name_User { get; set; }
        private string Lib_UB_PH { get; set; }
        private bool IsRead { get; set; }
        private bool Importer { get; set; }

        private string new_desc;

        private string old_code;
        private string lib_etat;
        private string num_serie;
        private string zone;
        private string zoneph;
        private string site;
        private string empl;
        private string lib_batiment;
        private string code_ub;
        private string lib_ub;
        public string code_local_th;
        public string code_local_ph;
        public string code_direction_th;
        public string code_direction_ph;
        private List<Nature> listnatures;
        private List<Nature> listnaturesfiltred;
        private Nature selectednatures;
        private List<UniteBudgetaire> listzoneuniteBudgetaires;
        private UniteBudgetaire selectedzoneuniteBudgetaires;
        private List<string> listetats;
        private string selectedetat;   
        private async void AllNatures()
        {
            ListNatures = new List<Nature>();
            NatureDatabaseController natureDatabaseController = new NatureDatabaseController();
            ListNatures = await natureDatabaseController.GetAllNatures();
            ListNaturesFiltred = ListNatures;
        }
        private  void AllEtats()
        {
            ListEtats = new List<string>();
            listetats.Add("A");
            listetats.Add("T");
            listetats.Add("V");
            listetats.Add("R");
            listetats.Add("I");
            listetats.Add("D");
            listetats.Add("P");
            ListEtats = listetats;
            SelectedEtat = "A";
        }
        public async void getUnite(string id)
        {
            UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            if (uniteBudgetaireDatabaseController.GetCountUniteBudgetaireByID(id) != 0)
            {
                var inv = await uniteBudgetaireDatabaseController.GetUniteBudgetaireByCodeLocale(id);
                Code_Direction_TH = inv.Code_Direction;
                ZONE = inv.ZONE;
            }
        }
        public InventaireViewModel(UniteBudgetaire uniteBudgetaire, ScrollView myscrollview)
        {
            myscrollview.IsVisible = false;
            AllNatures();
            AllEtats();
            ZONEPH = uniteBudgetaire.ZONE;
            SITE = "";
            EMPL = uniteBudgetaire.EMPL;
            Code_UB = uniteBudgetaire.Code_UB;
            Lib_UB = uniteBudgetaire.Lib_UB;
            Lib_BATIMENT = uniteBudgetaire.SITE;
            Code_Direction_PH = uniteBudgetaire.Code_Direction;
            Code_Local_PH = uniteBudgetaire.Code_Local;
            LoadNatureCommand = new Command(AllNatures);
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            LogOutCommand = new Command(LogOut);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            myscrollview.IsVisible = true;

        }

        private bool ValidateSave()
        {
            return SelectedEtat != null && !String.IsNullOrWhiteSpace(NumImmo)
               && ( !String.IsNullOrWhiteSpace(SelectedEtat));
        }
    

        async void chargerInventaire()
        {
            InventaireDatabaseController inventaireDatabaseController = new InventaireDatabaseController();
            if (inventaireDatabaseController.GetCountInventaire(numimmo) > 0)
            {
                var inventaire = await inventaireDatabaseController.GetInventaireById(numimmo);
                if (inventaire != null && Views.Inventaire.Completed)
                {
                    var nature = new Nature(inventaire.Code_Nature_PH, inventaire.Lib_Nature_PH, "", "", "");
                    SelectedNatures = nature;
                    Lib_Nature = inventaire.Lib_Nature_PH;
                    SelectedEtat = inventaire.Lib_Etat;
                    Num_Serie = inventaire.Num_Serie;
                    Old_Code = inventaire.Date_Acq;
                    New_Desc = inventaire.Observation;
                    Views.Inventaire.Completed = false;
                }
            }
        }
        public string NumImmo
        {
            get => numimmo;
            set
            {
                SetProperty(ref numimmo, value);
                chargerInventaire();
            }
        }

        public string Lib_Nature
        {
            get => lib_nature;
            set => SetProperty(ref lib_nature, value);
        }
        public string Lib_Etat
        {
            get => lib_etat;
            set => SetProperty(ref lib_etat, value);
        }
        public string Num_Serie
        {
            get => num_serie;
            set => SetProperty(ref num_serie, value);
        }
        public string Old_Code
        {
            get => old_code;
            set => SetProperty(ref old_code, value);
        }
        public string New_Desc
        {
            get => new_desc;
            set => SetProperty(ref new_desc, value);
        }

        public string ZONE
        {
            get => zone;
            set => SetProperty(ref zone, value);
        }
        public string ZONEPH
        {
            get => zoneph;
            set => SetProperty(ref zoneph, value);
        }
        public string SITE
        {
            get => site;
            set => SetProperty(ref site, value);
        }
        public string Code_Local_TH
        {
            get => code_local_th;
            set => SetProperty(ref code_local_th, value);
        }
        public string Code_Local_PH
        {
            get => code_local_ph;
            set => SetProperty(ref code_local_ph, value);
        }
        public string Code_Direction_TH
        {
            get => code_direction_th;
            set => SetProperty(ref code_direction_th, value);
        }
        public string Code_Direction_PH
        {
            get => code_direction_ph;
            set => SetProperty(ref code_direction_ph, value);
        }
        public string EMPL
        {
            get => empl;
            set => SetProperty(ref empl, value);
        }
        public string Lib_BATIMENT
        {
            get => lib_batiment;
            set => SetProperty(ref lib_batiment, value);
        }
        public string Lib_UB
        {
            get => lib_ub;
            set => SetProperty(ref lib_ub, value);
        }
        public string Code_UB
        {
            get => code_ub;
            set => SetProperty(ref code_ub, value);
        }      
        public List<string> ListEtats
        {
            get => listetats;
            set => SetProperty(ref listetats, value);
        }
        public List<Nature> ListNatures
        {
            get => listnatures;
            set => SetProperty(ref listnatures, value);
        }
        public List<Nature> ListNaturesFiltred
        {
            get => listnaturesfiltred;
            set => SetProperty(ref listnaturesfiltred, value);
        }
        public Nature SelectedNatures
        {
            get => selectednatures;
            set
            {
                if(selectednatures != value)
                {
                    SetProperty(ref selectednatures, value);
                }
            }
        }      
        public string SelectedEtat
        {
            get => selectedetat;
            set
            {
                SetProperty(ref selectedetat, value);
            }
        }   
       public Command LoadNatureCommand { get; }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command LogOutCommand { get; }

        private  void OnCancel()
        {
            lib_nature = "";
            Lib_Nature = "";
            lib_etat = "";
            Lib_Etat = "";
            Num_Serie = "";
            num_serie = "";
            Old_Code = "";
            old_code = "";
            New_Desc = "";
            new_desc = "";
            NumImmo = "";
            numimmo = "";
            SelectedNatures = new Nature();
            selectednatures = new Nature();
            SelectedEtat = "A";
        }
        private LoginPage mainPage;

        private  void LogOut()
        {
            Application.Current.MainPage = new NavigationPage(mainPage = new LoginPage());
        }
        private async void OnSave()
        {
            DateTime dt = DateTime.Now;
            InventaireDatabaseController inventaireDatabaseController = new InventaireDatabaseController();
            if (!App.LicenceValide && inventaireDatabaseController.GetCountAllInventaire() > 20)
            {
                await Application.Current.MainPage.DisplayAlert("Attention", "Version Demo ", "Ok");
            }
            else
            {
                string codenat = string.Empty;
                string libnat = string.Empty;
                codenat = Views.Inventaire.CodeNatureTH;
                if (codenat == "")
                {
                    codenat = "99";
                }
                libnat = Views.Inventaire.LibNatureTH;
                var em = EMPL;
                Models.Inventaire newInventaire;
                if (TakePicture.Pathpicture == "")
                {
                    var numimo = numimmo + ".jpg";
                    var NewPathpicture = Constants.pathPictureFolder + numimo;
                    if (File.Exists(NewPathpicture))
                    {
                        TakePicture.Pathpicture = NewPathpicture;
                    }
                }
                var countinv = inventaireDatabaseController.GetCountInventaire(NumImmo);
                if (countinv > 0)
                {
                  var inv = await inventaireDatabaseController.GetInventaireById(NumImmo);
                  ZONE = inv.ZONE_TH;
                  Code_Local_TH = inv.Code_Local_TH;
                  Code_Direction_TH = inv.Code_Direction_TH;
                  newInventaire = new Models.Inventaire(
                  NumImmo,
                  Old_Code,
                  inv.Code_Nature_TH,
                  inv.Lib_Nature_TH,
                  inv.Code_UB_TH,
                  inv.Lib_UB_TH,
                  inv.Code_UC,
                  SelectedEtat,
                  LoginViewModel.UserId,
                  codenat,
                  libnat,
                  Code_UB,
                  Lib_UB,
                  Num_Serie,
                  inv.Bureau,
                  inv.Code_Operation1,
                  inv.Code_Operation2,
                  dt.ToString("yyyy-MM-dd HH:mm:ss"),
                  true,
                  inv.Compte_G,
                  inv.Code_RB,
                  inv.Code_Art,
                  true,
                  New_Desc,
                  inv.BureauTH,
                  inv.ServiceTH,
                  inv.ServicePH,
                  inv.EMPL_TH,
                  EMPL,
                  inv.SITE_TH,
                  Lib_BATIMENT,
                  ZONE,
                  ZONEPH,
                  TakePicture.Pathpicture,
                  Code_Direction_TH,
                  Code_Direction_PH,
                  Code_Local_TH,
                  Code_Local_PH
                   );
                }
                else
                {
                    newInventaire = new Models.Inventaire(
                    NumImmo,
                    Old_Code,
                    New_Desc,
                    SelectedEtat,
                    LoginViewModel.UserId,
                    codenat,
                    libnat,
                    Code_UB,
                    Lib_UB,
                    Num_Serie,
                    dt.ToString("yyyy-MM-dd HH:mm:ss"),
                    true,
                    true,
                    EMPL,
                    Lib_BATIMENT,
                    ZONE,
                    TakePicture.Pathpicture,
                    Code_Direction_PH,
                    Code_Local_PH
                        );
                }
                int res = await inventaireDatabaseController.SaveInventaire(newInventaire);
                if (res > 0)
                {
                    CrossToastPopUp.Current.ShowToastMessage("Inventaire Enregistrée avec succées");
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Erro", "Error, Echec ", "Ok");
                OnCancel();
            }
        }
    }
}
