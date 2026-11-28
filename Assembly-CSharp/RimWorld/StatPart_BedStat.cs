using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F6F RID: 8047
	public class StatPart_BedStat : StatPart
	{
		// Token: 0x0600CCE4 RID: 52452 RVA: 0x004B6750 File Offset: 0x004B4950
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					val *= this.BedMultiplier(pawn);
				}
			}
		}

		// Token: 0x0600CCE5 RID: 52453 RVA: 0x004B6784 File Offset: 0x004B4984
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.ageTracker != null)
				{
					return "StatsReport_InBed".Translate() + ": x" + this.BedMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0600CCE6 RID: 52454 RVA: 0x004B67DE File Offset: 0x004B49DE
		private float BedMultiplier(Pawn pawn)
		{
			if (pawn.InBed())
			{
				return pawn.CurrentBed().GetStatValue(this.stat, true, -1);
			}
			if (pawn.InCaravanBed())
			{
				return pawn.CurrentCaravanBed().GetStatValue(this.stat, true, -1);
			}
			return 1f;
		}

		// Token: 0x040082EF RID: 33519
		private StatDef stat;
	}
}
