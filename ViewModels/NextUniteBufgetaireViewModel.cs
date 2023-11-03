using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using CNSSInv.Models;
using CNSSInv.Services;
using CNSSInv.Views;
namespace CNSSInv.ViewModels
{
    public class NextUniteBufgetaireViewModel : BaseViewModel
    {
        private string text;
        private string description;
        private string zone;
        private string site;
        private string empl;
        private string code_ub;
        private string lib_ub;
        private string lib_batiment;
        private string code_local;
        private string code_direction;
        private string lib_direction;
        public int indexemplunitebudgetaires;
        private List<UniteBudgetaire> listzoneuniteBudgetaires;
        private UniteBudgetaire selectedzoneuniteBudgetaires;
        private UniteBudgetaire selectedsiteuniteBudgetaires;
        private UniteBudgetaire selectedempluniteBudgetaires;
        private UniteBudgetaire selectedcode_ubuniteBudgetaires;
        private UniteBudgetaire selectedcode_localunitebudgetaires;
        private UniteBudgetaire selectedcode_directionunitebudgetaires;
        private List<UniteBudgetaire> listcode_localunitebudgetaires;
        private List<UniteBudgetaire> listsiteuniteBudgetaires;
        private List<UniteBudgetaire> listcodedirectionunitebudgetaires;
        private List<UniteBudgetaire> listempuniteBudgetaires;
        private List<UniteBudgetaire> listcodeuniteBudgetaires;
        private UniteBudgetaire selectedallunitebudgetaires = new UniteBudgetaire();
        private List<UniteBudgetaire> listalluniteBudgetaires = new List<UniteBudgetaire>();

        private async void AllZoneUniteBudgetaires()
        {
            ListZoneUniteBudgetaires = new List<UniteBudgetaire>();
            UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            ListZoneUniteBudgetaires = await uniteBudgetaireDatabaseController.GetZoneUniteBudgetaires();
        }

        private async void AllCodeLocalUniteBudgetaires()
        {
            ListCode_LocalUniteBudgetaires = new List<UniteBudgetaire>();
            UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            ListCode_LocalUniteBudgetaires = await uniteBudgetaireDatabaseController.GetCodeLocaleUniteBudgetaires();
        }
        private async void AllCodeBatimentUniteBudgetaires()
        {
            ListEmpUniteBudgetaires = new List<UniteBudgetaire>();
            UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            ListEmpUniteBudgetaires = await uniteBudgetaireDatabaseController.GetCodeBatimentUniteBudgetaires();
        }
        public NextUniteBufgetaireViewModel()
        {
            DisplayCommand = new Command(OnDisplay, ValidateNext);
            NextCommand = new Command(OnNext, ValidateNext);
            CancelCommand = new Command(OnCancel);
            LogOutCommand = new Command(LogOut);
            SettingCommand = new Command(OnSetting);
            this.PropertyChanged +=
                (_, __) => NextCommand.ChangeCanExecute();
            this.PropertyChanged +=
             (_, __) => DisplayCommand.ChangeCanExecute();
        }

        private bool ValidateNext()
        {
            return SelectedAllUniteBudgetaires != null;
            //return SelectedEmplUniteBudgetaires != null && SelectedCode_ubUniteBudgetaires != null && SelectedCode_DirectionUniteBudgetaires != null && !String.IsNullOrWhiteSpace(SelectedEmplUniteBudgetaires.EMPL) && !String.IsNullOrWhiteSpace(SelectedCode_ubUniteBudgetaires.Code_UB) && !String.IsNullOrWhiteSpace(SelectedCode_DirectionUniteBudgetaires.Code_Direction);
        }

        public UniteBudgetaire SelectedAllUniteBudgetaires
        {
            get => selectedallunitebudgetaires;
            set
            {
                SetProperty(ref selectedallunitebudgetaires, value);
            }
        }


        public List<UniteBudgetaire> ListAllUniteBudgetaires
        {
            get => listalluniteBudgetaires;
            set => SetProperty(ref listalluniteBudgetaires, value);
        }

        public string ZONE
        {
            get => zone;
            set
            {
                SetProperty(ref zone, value);
                if (selectedcode_directionunitebudgetaires != null && selectedempluniteBudgetaires != null && selectedcode_localunitebudgetaires == null && !String.IsNullOrEmpty(zone))
                {
                    chargerListCode(selectedcode_directionunitebudgetaires, selectedempluniteBudgetaires, zone, Lib_BATIMENT);
                }
            }
        }
        public string Lib_BATIMENT
        {
            get => lib_batiment;
            set
            {
                SetProperty(ref lib_batiment, value);

            }
        }
        public string Lib_UB
        {
            get => lib_ub;
            set => SetProperty(ref lib_ub, value);
        }
        public string Code_Local
        {
            get => code_local;
            set => SetProperty(ref code_local, value);
        }
        public int IndexEmplUniteBudgetaires
        {
            get => indexemplunitebudgetaires;
            set => SetProperty(ref indexemplunitebudgetaires, value);
        }

        private async void chargerListCodeDirection(UniteBudgetaire selectedempl, string Lib_BAT)
        {
            UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            ListCodeDirectionUniteBudgetaires = await uniteBudgetaireDatabaseController.GetCodeDirectionUniteBudgetaires(selectedempl.EMPL, Lib_BAT);
        }

        private async void chargerListCode(UniteBudgetaire selectedemplcode, UniteBudgetaire selectedempl, string zone, string site)
        {
            UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            ListCodeUniteBudgetaires = await uniteBudgetaireDatabaseController.GetCodeUniteBudgetaires(selectedempl.EMPL, selectedemplcode.Code_Direction, site, zone);
        }

        private async void chargerLibDirectionByCodeDirection(UniteBudgetaire selectedemplcode, UniteBudgetaire selectedempl)
        {
            UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            var lstcode = await uniteBudgetaireDatabaseController.GetLibDirectionUniteBudgetairesByEmplDirection(selectedempl.EMPL, selectedemplcode.Code_Direction);
            if (lstcode.Count > 0)
                ZONE = lstcode[0].ZONE;
            else
            {
                ZONE = "";
            }
        }

        private async void chargerLib(UniteBudgetaire selectedcode, UniteBudgetaire selectedempl, UniteBudgetaire selectedemplcode)
        {
            UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            var lstcode = await uniteBudgetaireDatabaseController.GetLibUniteBudgetaires(selectedcode.Code_UB, selectedempl.EMPL, selectedemplcode.Code_Direction, ZONE, Lib_BATIMENT);
            if (lstcode.Count > 0)
                Lib_UB = lstcode[0].Lib_UB;
            else
            {
                Lib_UB = "";
            }
        }
        public UniteBudgetaire SelectedEmplUniteBudgetaires
        {
            get => selectedempluniteBudgetaires;
            set
            {
                SetProperty(ref selectedempluniteBudgetaires, value);
                if (selectedempluniteBudgetaires != null && selectedcode_localunitebudgetaires == null)
                {
                    Lib_BATIMENT = selectedempluniteBudgetaires.SITE;
                    chargerListCodeDirection(selectedempluniteBudgetaires, Lib_BATIMENT);
                }
                else
                {

                }
            }
        }
        public UniteBudgetaire SelectedCode_ubUniteBudgetaires
        {
            get => selectedcode_ubuniteBudgetaires;
            set
            {
                SetProperty(ref selectedcode_ubuniteBudgetaires, value);
                if (selectedcode_ubuniteBudgetaires != null && selectedempluniteBudgetaires != null && selectedcode_directionunitebudgetaires != null && selectedcode_localunitebudgetaires == null)
                    chargerLib(selectedcode_ubuniteBudgetaires, SelectedEmplUniteBudgetaires, selectedcode_directionunitebudgetaires);
                else
                {
                }
            }
        }

        bool verif = false;
        public UniteBudgetaire SelectedCode_DirectionUniteBudgetaires
        {
            get => selectedcode_directionunitebudgetaires;
            set
            {
                SetProperty(ref selectedcode_directionunitebudgetaires, value);
                if (selectedcode_directionunitebudgetaires != null && selectedempluniteBudgetaires != null && selectedcode_localunitebudgetaires == null)
                {
                    chargerLibDirectionByCodeDirection(selectedcode_directionunitebudgetaires, selectedempluniteBudgetaires);
                }
                else
                {
                }
            }
        }

        public async void chargerByCodeLocale()
        {
            var codelocale = selectedcode_localunitebudgetaires.Code_Local;
            UniteBudgetaireDatabaseController uniteBudgetaireDatabaseController = new UniteBudgetaireDatabaseController();
            UniteBudgetaire unitebudg = await uniteBudgetaireDatabaseController.GetUniteBudgetaireByCodeLocale(codelocale);
            IndexEmplUniteBudgetaires = 0;
        }
        public UniteBudgetaire SelectedCode_LocalUniteBudgetaires
        {
            get => selectedcode_localunitebudgetaires;
            set
            {
                SetProperty(ref selectedcode_localunitebudgetaires, value);
                if (selectedcode_localunitebudgetaires != null)
                {
                    chargerByCodeLocale();
                }
                else
                {
                    ListCode_LocalUniteBudgetaires = new List<UniteBudgetaire>();
                }
            }
        }
        public List<UniteBudgetaire> ListZoneUniteBudgetaires
        {
            get => listzoneuniteBudgetaires;
            set => SetProperty(ref listzoneuniteBudgetaires, value);
        }

        public List<UniteBudgetaire> ListSiteUniteBudgetaires
        {
            get => listsiteuniteBudgetaires;
            set => SetProperty(ref listsiteuniteBudgetaires, value);
        }
        public List<UniteBudgetaire> ListEmpUniteBudgetaires
        {
            get => listempuniteBudgetaires;
            set => SetProperty(ref listempuniteBudgetaires, value);
        }
        public List<UniteBudgetaire> ListCodeUniteBudgetaires
        {
            get => listcodeuniteBudgetaires;
            set => SetProperty(ref listcodeuniteBudgetaires, value);
        }

        public List<UniteBudgetaire> ListCode_LocalUniteBudgetaires
        {
            get => listcode_localunitebudgetaires;
            set => SetProperty(ref listcode_localunitebudgetaires, value);
        }
        public List<UniteBudgetaire> ListCodeDirectionUniteBudgetaires
        {
            get => listcodedirectionunitebudgetaires;
            set => SetProperty(ref listcodedirectionunitebudgetaires, value);
        }


        public Command NextCommand { get; }
        public Command CancelCommand { get; }
        public Command LogOutCommand { get; }
        public Command SettingCommand { get; }

        public Command DisplayCommand { get; }

        private void OnCancel()
        {
        }
        private LoginPage mainPage;

        private void LogOut()
        {
            SelectedCode_LocalUniteBudgetaires = null;
            SelectedCode_DirectionUniteBudgetaires = null;
            SelectedCode_ubUniteBudgetaires = null;
            SelectedEmplUniteBudgetaires = null;
            Lib_UB = string.Empty;
            Lib_BATIMENT = string.Empty;
            ZONE = string.Empty;
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
        private void OnNext()
        {
        }
        private void OnDisplay()
        {
        }
        private void OnSetting()
        {
        }
    }
}