﻿using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorImpots.Model
{
    public class Impots
    {
        public int EtatCivil { get; set; } = 1;

        public int NbEnfants { get; set; } = 0;

        public double PartsSupplementaires { get; set; } = 0;

        public double NbParts
        {
            get
            {
                var partsEnfants = NbEnfants switch
                {
                    0 => 0,
                    1 => 0.5,
                    _ => NbEnfants - 1,
                };

                return EtatCivil + partsEnfants + PartsSupplementaires;
            }
        }
    }
}