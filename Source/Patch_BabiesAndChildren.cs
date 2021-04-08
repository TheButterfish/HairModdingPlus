using System;
using Verse;

namespace ButterfishHairModdingPlus
{
    class Patch_BabiesAndChildren
    {
		//credits to CentAtMoney for the basis of this snippet of code
		public static bool BCCompat_IsYoungerThanChild(Pawn pawn)
        {
			int curLifeStageIndex = pawn.ageTracker.CurLifeStageIndex;
			_ = pawn.RaceProps.lifeStageAges.Count;
			return (curLifeStageIndex < 2);
		}
    }
}
