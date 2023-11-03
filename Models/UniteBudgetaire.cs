using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNSSInv.Models
{
    public class UniteBudgetaire
    {
        //[PrimaryKey]
        public string Code_Local { get; set; }
        public string Code_UB { get; set; }
        public string Lib_UB { get; set; }
        public string EMPL { get; set; }
        public string SITE { get; set; }
        public string Code_Direction { get; set; }
        public string ZONE { get; set; }
        public UniteBudgetaire()
        {

        }
        public UniteBudgetaire(string code_local, string code_ub,string lib_ub,string empl,string site,string zone, string Code_Local, string Code_Direction)
        {
            this.Code_UB = code_ub;
            this.Lib_UB = lib_ub;
            this.EMPL = empl;
            this.SITE = site;
            this.ZONE = zone;
            this.Code_Local = Code_Local;
            this.Code_Direction = Code_Direction;
            this.Code_Local = code_local;

        }
        public UniteBudgetaire(string empl, string site, string zone)
        {
            this.EMPL = empl;
            this.SITE = site;
            this.ZONE = zone;
        }

    }
}
