using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ButterfishHairModdingPlus
{
    public class HairColor2_Exposable : IExposable
    {
        public Color hairColor2;

        public HairColor2_Exposable()
        {
            hairColor2 = GenColor.RandomColorOpaque();
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref hairColor2, "hairColor2");
        }
    }
}
