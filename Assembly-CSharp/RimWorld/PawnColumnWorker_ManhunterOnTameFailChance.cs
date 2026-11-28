using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DDB RID: 7643
	public class PawnColumnWorker_ManhunterOnTameFailChance : PawnColumnWorker_Text
	{
		// Token: 0x0600C6B4 RID: 50868 RVA: 0x0048C8A4 File Offset: 0x0048AAA4
		protected override string GetTextFor(Pawn pawn)
		{
			float manhunterOnTameFailChance = PawnUtility.GetManhunterOnTameFailChance(pawn);
			if (manhunterOnTameFailChance == 0f)
			{
				return "-";
			}
			return manhunterOnTameFailChance.ToStringPercent();
		}

		// Token: 0x0600C6B5 RID: 50869 RVA: 0x0048C8CC File Offset: 0x0048AACC
		protected override string GetTip(Pawn pawn)
		{
			return PawnUtility.GetManhunterOnTameFailChanceExplanation(pawn.def, pawn);
		}

		// Token: 0x0600C6B6 RID: 50870 RVA: 0x0048C8DC File Offset: 0x0048AADC
		public override int Compare(Pawn a, Pawn b)
		{
			return PawnUtility.GetManhunterOnTameFailChance(a).CompareTo(PawnUtility.GetManhunterOnTameFailChance(b));
		}
	}
}
