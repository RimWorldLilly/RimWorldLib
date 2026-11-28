using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A0B RID: 6667
	public class CompTargetEffect_Berserk : CompTargetEffect
	{
		// Token: 0x0600A7D3 RID: 42963 RVA: 0x003BB978 File Offset: 0x003B9B78
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead)
			{
				return;
			}
			pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, false, true, false, null, false, false, false);
			Find.BattleLog.Add(new BattleLogEntry_ItemUsed(user, target, this.parent.def, RulePackDefOf.Event_ItemUsed));
		}
	}
}
