using BlazorImpots.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorImpots
{
    public class NouveauRevenuEventArgs : EventArgs
    {
        public Revenu NouveauRevenu { get; }

        public NouveauRevenuEventArgs(Revenu revenu)
        {
            NouveauRevenu = revenu;
        }
    }
}
