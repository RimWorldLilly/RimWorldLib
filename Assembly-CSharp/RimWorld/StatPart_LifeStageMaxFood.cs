using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F74 RID: 8052
	public class StatPart_LifeStageMaxFood : StatPart
	{
		// Token: 0x0600CCF6 RID: 52470 RVA: 0x004B6A44 File Offset: 0x004B4C44
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					val *= pawn.ageTracker.CurLifeStage.foodMaxFactor;
				}
			}
		}

		// Token: 0x0600CCF7 RID: 52471 RVA: 0x004B6A80 File Offset: 0x004B4C80
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					return "LifeStageMaxFood".Translate() + ": x" + pawn.ageTracker.CurLifeStage.foodMaxFactor.ToStringPercent();
				}
			}
			return null;
		}
	}
}
