using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace CNSSInv.Models
{
    public class Inventaire
    {
        [PrimaryKey]
        public string NumImmo { get; set; }
        public string Date_Acq { get; set; }
        public string Code_Nature_TH { get; set; }
        public string Lib_Nature_TH { get; set; }
        public string Code_UB_TH { get; set; }
        public string Lib_UB_TH { get; set; }
        public string Code_UC { get; set; }
        public string Lib_Etat { get; set; }
        public string Name_User { get; set; }
        public string Code_Nature_PH { get; set; }
        public string Lib_Nature_PH { get; set; }
        public string Code_UB_PH { get; set; }
        public string Lib_UB_PH { get; set; }
        public string Num_Serie { get; set; }
        public string Bureau { get; set; }
        public int Code_Operation1 { get; set; }
        public int Code_Operation2 { get; set; }
        public string DateTimeScan { get; set; }
        public bool IsRead { get; set; }
        public string Compte_G { get; set; }
        public string Code_RB { get; set; }
        public string Code_Art { get; set; }
        public bool Importer { get; set; }
        public string Observation { get; set; }
        public string BureauTH { get; set; }
        public string ServiceTH { get; set; }
        public string ServicePH { get; set; }
        public string EMPL_TH { get; set; }
        public string EMPL_PH { get; set; }
        public string SITE_TH { get; set; }
        public string SITE_PH { get; set; }
        public string ZONE_TH { get; set; }
        public string ZONE_PH { get; set; }
        public string Code_Direction_TH { get; set; }
        public string Code_Direction_PH { get; set; }
        public string Code_Local_TH { get; set; }
        public string Code_Local_PH { get; set; }
        public string PATH_PICTURE { get; set; }

        public Inventaire(
             string NumImmo,
       string Date_Acq,
       string Code_Nature_TH,
       string Lib_Nature_TH,
       string Code_UB_TH,
       string Lib_UB_TH,
       string Code_UC,
       string Lib_Etat,
       string Name_User,
       string Code_Nature_PH,
       string Lib_Nature_PH,
       string Code_UB_PH,
       string Lib_UB_PH,
       string Num_Serie,
       string Bureau,
       int Code_Operation1,
       int Code_Operation2,
       string DateTimeScan,
       bool IsRead,
       string Compte_G,
       string Code_RB,
       string Code_Art,
       bool Importer,
       string Observation,
       string BureauTH,
       string ServiceTH,
       string ServicePH,
       string EMPL_TH,
       string EMPL_PH,
       string SITE_TH,
       string SITE_PH,
       string ZONE_TH,
       string ZONE_PH,
       string PATH_PICTURE,
       string Code_Direction_TH,
       string Code_Direction_PH,
       string Code_Local_TH,
       string Code_Local_PH
      )
        {
            this.NumImmo = NumImmo;
            this.Date_Acq = Date_Acq;
            this.Code_Nature_TH = Code_Nature_TH;
            this.Lib_Nature_TH = Lib_Nature_TH;
            this.Code_UB_TH = Code_UB_TH;
            this.Lib_UB_TH = Lib_UB_TH;
            this.Code_UC = Code_UC;
            this.Lib_Etat = Lib_Etat;
            this.Name_User = Name_User;
            this.Code_Nature_PH = Code_Nature_PH;
            this.Lib_Nature_PH = Lib_Nature_PH;
            this.Code_UB_PH = Code_UB_PH;
            this.Lib_UB_PH = Lib_UB_PH;
            this.Num_Serie = Num_Serie;
            this.Bureau = Bureau;
            this.Code_Operation1 = Code_Operation1;
            this.Code_Operation2 = Code_Operation2;
            this.DateTimeScan = DateTimeScan;
            this.IsRead = IsRead;
            this.Compte_G = Compte_G;
            this.Code_RB = Code_RB;
            this.Code_Art = Code_Art;
            this.Importer = Importer;
            this.Observation = Observation;
            this.BureauTH = BureauTH;
            this.ServiceTH = ServiceTH;
            this.ServicePH = ServicePH;
            this.EMPL_TH = EMPL_TH;
            this.EMPL_PH = EMPL_PH;
            this.SITE_TH = SITE_TH;
            this.SITE_PH = SITE_PH;
            this.ZONE_TH = ZONE_TH;
            this.ZONE_PH = ZONE_PH;
            this.PATH_PICTURE = PATH_PICTURE;
            this.Code_Direction_TH = Code_Direction_TH;
            this.Code_Direction_PH = Code_Direction_PH;
            this.Code_Local_TH = Code_Local_TH;
            this.Code_Local_PH = Code_Local_PH;
        }
        public Inventaire()
        {
        }

        public Inventaire(
            string NumImmo,
            string Date_Acq,
            string Observation,
            string Lib_Etat,
            string Name_User,
            string Code_Nature_PH,
            string Lib_Nature_PH,
            string Code_UB_PH,
            string Lib_UB_PH,
            string Num_Serie,
            string DateTimeScan,
            bool IsRead,
            bool Importer,
            string EMPL_PH,
            string SITE_PH,
            string ZONE_PH,
            string PATH_PICTURE,
            string Code_Direction_PH,
            string Code_Local_PH 
            )
        {
            this.NumImmo = NumImmo;
            this.Date_Acq = Date_Acq;
            this.Observation = Observation;
            this.Lib_Etat = Lib_Etat;
            this.Name_User = Name_User;
            this.Code_Nature_PH = Code_Nature_PH;
            this.Lib_Nature_PH = Lib_Nature_PH;
            this.Code_UB_PH = Code_UB_PH;
            this.Lib_UB_PH = Lib_UB_PH;
            this.Num_Serie = Num_Serie;
            this.DateTimeScan = DateTimeScan;
            this.IsRead = IsRead;
            this.Importer = Importer;
            this.EMPL_PH = EMPL_PH;
            this.SITE_PH = SITE_PH;
            this.ZONE_PH = ZONE_PH;
            this.PATH_PICTURE = PATH_PICTURE;
            this.Code_Direction_PH = Code_Direction_PH;
            this.Code_Local_PH = Code_Local_PH;
        }
    }
}
