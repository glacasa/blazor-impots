using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorImpots.Model
{
    public abstract class Revenu
    {
        public string Libelle { get; set; } = "";

        public int Montant { get; set; } = 0;

        public double Abattment { get; set; } = 0;

        public virtual int AssietteIR() => (int)Math.Round(Montant * (1 - Abattment));

        public virtual int AssietteCsg() => 0;

        public virtual int ImpotForfaitaire() => 0;

    }

    public class Salaire : Revenu
    {
        public Salaire(int montant)
        {
            Libelle = "Revenu d'activité";
            Montant = montant;
            Abattment = 0.1;
        }
    }

    public class Foncier : Revenu
    {
        public Foncier(int montant)
        {
            Libelle = "Revenu foncier";
            Montant = montant;
            Abattment = 0.3;
        }

        public override int AssietteCsg() => this.AssietteIR();
    }

}
