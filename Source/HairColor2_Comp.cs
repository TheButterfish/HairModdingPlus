using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ButterfishHairModdingPlus
{
    public class HairColor2_Comp : ThingComp
    {
        private HairColor2_Exposable hairColor2Expo;

        public HairColor2_Exposable HairColorTwoExpo
        {
            get
            {
                if (hairColor2Expo == null)
                {
                    hairColor2Expo = new HairColor2_Exposable();
                }

                return hairColor2Expo;
            }
        }
        public HairColor2_CompProperties Props
        {
            get
            {
                return (HairColor2_CompProperties) props;
            }
        }

        public override void PostExposeData()
        {
            Scribe_Deep.Look(ref hairColor2Expo, "hairColor2Expo", new object[0]);
        }

    }
}
