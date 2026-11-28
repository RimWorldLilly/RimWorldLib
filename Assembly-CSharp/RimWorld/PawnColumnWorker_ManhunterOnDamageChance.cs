using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DDA RID: 7642
	public class PawnColumnWorker_ManhunterOnDamageChance : PawnColumnWorker_Text
	{
		// Token: 0x0600C6B0 RID: 50864 RVA: 0x0048C830 File Offset: 0x0048AA30
		protected override string GetTextFor(Pawn pawn)
		{
			float manhunterOnDamageChance = PawnUtility.GetManhunterOnDamageChance(pawn, null, -1f);
			if (manhunterOnDamageChance == 0f)
			{
				return "-";
			}
			return manhunterOnDamageChance.ToStringPercent();
		}

		// Token: 0x0600C6B1 RID: 50865 RVA: 0x0048C85E File Offset: 0x0048AA5E
		protected override string GetTip(Pawn pawn)
		{
			return PawnUtility.GetManhunterOnDamageChanceExplanation(pawn.def, pawn);
		}

		// Token: 0x0600C6B2 RID: 50866 RVA: 0x0048C86C File Offset: 0x0048AA6C
		public override int Compare(Pawn a, Pawn b)
		{
			return PawnUtility.GetManhunterOnDamageChance(a, null, -1f).CompareTo(PawnUtility.GetManhunterOnDamageChance(b, null, -1f));
		}
	}
}
