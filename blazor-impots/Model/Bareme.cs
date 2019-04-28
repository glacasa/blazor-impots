using System.Collections.Generic;

namespace BlazorImpots.Model
{
    public class Bareme
    {
        public Bareme(int min, int max, int taux)
        {
            Min = min;
            Max = max;
            Taux = taux;
        }

        public int Min { get; set; }

        public int Max { get; set; }

        public int Taux { get; set; }

        public string AffichageIntervalle()
        {
            if (this.Min == 0)
            {
                return "Jusqu'à " + this.Max.ToString("### ##0") + " €";
            }
            if (this.Max == int.MaxValue)
            {
                return "Plus de " + this.Min.ToString("### ##0") + " €";
            }
            return "De " + this.Min.ToString("### ##0") + " € à " + this.Max.ToString("### ##0") + " €";
        }

        public static IEnumerable<Bareme> Bareme2019()
        {
            yield return new Bareme(0, 9964, 0);
            yield return new Bareme(9965, 27519, 14);
            yield return new Bareme(27520,73779, 30);
            yield return new Bareme(73780, 156244, 41);
            yield return new Bareme(156245, int.MaxValue, 45);
        }
    }
}
