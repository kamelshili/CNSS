using System;
using System.Collections.Generic;
using System.Text;


namespace Inventaire.Services;

    public class ShowHidePassEffect : RoutingEffect
    {
        public string EntryText { get; set; }
        public ShowHidePassEffect() : base("Xamarin.ShowHidePassEffect") { }
    }

