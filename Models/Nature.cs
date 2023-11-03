using System;
using System.Collections.Generic;
using System.Text;

namespace CNSSInv.Models
{
    public class Nature
    {

        public string Code_Nature { get; set; }
        public string Lib_Nature { get; set; }
        public string Compte_G { get; set; }
        public string Code_RB { get; set; }
        public string Code_Art { get; set; }
        public Nature(
        string Code_Nature,
         string Lib_Nature,
         string Compte_G,
        string Code_RB,
        string Code_Art)
       {
            this.Code_Nature = Code_Nature;
            this.Lib_Nature = Lib_Nature;
            this.Compte_G = Compte_G;
            this.Code_RB = Code_RB;
            this.Code_Art = Code_Art;
       }
        public Nature()
        {

        }
    }
}
