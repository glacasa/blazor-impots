using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorImpots.Model
{
    public class Impots
    {
        private static (int impotMax, int baseCalcul) DecoteIndiv = (1595, 1196);
        private static (int impotMax, int baseCalcul) DecoteCouple = (2627, 1970);
        private static (int revenuMaxAllegementDegressif, int revenuMaxAllegementComplet, int augmentationSeuilParPart) AllegementsIndiv = (21036, 18984, 3797 * 2);

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

        public int RevenusTotaux
        {
            get
            {
                return this.Revenus.Sum(r => r.Montant);
            }
        }

        public int TotalImposable
        {
            get
            {
                return this.Revenus.Sum(r => r.AssietteIR());
            }
        }

        private int TotalImposableParPart
        {
            get
            {
                return (int)Math.Round(TotalImposable / NbParts);
            }
        }

        public int DroitsSimples
        {
            get
            {
                if (this.NbParts == 1)
                {
                    return (int)CalculImpot(TotalImposable);
                }

                return (int)Math.Round(CalculImpot(TotalImposableParPart) * NbParts);
            }
        }


        public int Decote
        {
            get
            {
                var montant = DroitsSimples;

                var calculDecote = this.EtatCivil == 1
                  ? DecoteIndiv
                  : DecoteCouple;

                double decote = 0;
                if (montant <= calculDecote.impotMax)
                {
                    decote = calculDecote.baseCalcul - montant * 0.75;
                }

                return (int)Math.Min(decote, montant);
            }
        }


        public int Allegement
        {
            get
            {
                var montant = DroitsSimples - Decote;

                double revenuMaxDegressif = AllegementsIndiv.revenuMaxAllegementDegressif * EtatCivil;
                revenuMaxDegressif += AllegementsIndiv.augmentationSeuilParPart * (NbParts - EtatCivil);

                if (TotalImposable <= revenuMaxDegressif)
                {
                    var allegementComplet = montant * 0.2;
                    var revenuMaxComplet = AllegementsIndiv.revenuMaxAllegementComplet * EtatCivil;
                    if (TotalImposable <= revenuMaxComplet)
                    {
                        return (int)Math.Round(allegementComplet);
                    }
                    else
                    {
                        return (int)Math.Round(allegementComplet * ((revenuMaxDegressif - TotalImposable) / (revenuMaxDegressif - revenuMaxComplet)));
                    }
                }

                return 0;
            }
        }

        public int MontantTotalIR => DroitsSimples - Decote - Allegement;

        private static double CalculImpot(int imposable)
        {
            int total = 0;

            foreach (var tranche in Bareme.Bareme2019())
            {
                if (imposable > tranche.Min)
                {
                    var max = Math.Min(imposable, tranche.Max);
                    total += (max - tranche.Min) * tranche.Taux;
                }
            }

            return total / 100;
        }
    }
}
