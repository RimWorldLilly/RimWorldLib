using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A16 RID: 6678
	public class CompTargetEffect_Manhunter : CompTargetEffect
	{
		// Token: 0x0600A7E9 RID: 42985 RVA: 0x003BBC80 File Offset: 0x003B9E80
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			if (!pawn.Awake())
			{
				RestUtility.WakeUp(pawn, true);
			}
			pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, false, null, false, false, false);
		}
	}
}
