using System;
using System.Collections;
using System.Collections.Generic;
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
                //Calcul du nombre de parts
                // https://www.service-public.fr/particuliers/vosdroits/F2702
                // https://www.service-public.fr/particuliers/vosdroits/F2705
                var partsEnfants = NbEnfants switch
                {
                    0 => 0,
                    1 => 0.5,
                    _ => NbEnfants - 1,
                };

                return EtatCivil + partsEnfants + PartsSupplementaires;
            }
        }

        public IList<Revenu> Revenus { get; } = new List<Revenu>() {
            new Salaire(21600),
        };
    }
}
