using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ButterfishHairModdingPlus
{
    public class HairColor2_API
    {
        public static Color GetHairColor2(Pawn pawn)
        {
            HairColor2_Comp comp = pawn.GetComp<HairColor2_Comp>();
            if (comp == null)
            {
                return Color.white;
            }
            else
            {
                return comp.HairColorTwoExpo.hairColor2;
            }  
        }

        public static void SetHairColor2(Pawn pawn, Color color)
        {
            HairColor2_Comp comp = pawn.GetComp<HairColor2_Comp>();
            if (comp == null)
            {
                return;
            }
            else
            {
                comp.HairColorTwoExpo.hairColor2 = color;
            }
        }
    }
}
