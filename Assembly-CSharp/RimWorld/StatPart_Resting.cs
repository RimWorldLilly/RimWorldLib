using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FA1 RID: 8097
	public class StatPart_Resting : StatPart
	{
		// Token: 0x0600CDB3 RID: 52659 RVA: 0x004B97C4 File Offset: 0x004B79C4
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					val *= this.RestingMultiplier(pawn);
				}
			}
		}

		// Token: 0x0600CDB4 RID: 52660 RVA: 0x004B97F8 File Offset: 0x004B79F8
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					return "StatsReport_Resting".Translate() + ": x" + this.RestingMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0600CDB5 RID: 52661 RVA: 0x004B984C File Offset: 0x004B7A4C
		private float RestingMultiplier(Pawn pawn)
		{
			if (pawn.InBed() || (pawn.GetPosture() != PawnPosture.Standing && !pawn.Downed) || (pawn.IsCaravanMember() && !pawn.GetCaravan().pather.MovingNow) || pawn.InCaravanBed() || pawn.CarriedByCaravan())
			{
				return this.factor;
			}
			return 1f;
		}

		// Token: 0x04008331 RID: 33585
		public float factor = 1f;
	}
}
