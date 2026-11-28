using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F95 RID: 8085
	public class StatPart_NotCarefullySlaughtered : StatPart
	{
		// Token: 0x0600CD80 RID: 52608 RVA: 0x004B8A86 File Offset: 0x004B6C86
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.HasWounds(req))
			{
				val *= this.factor;
			}
		}

		// Token: 0x0600CD81 RID: 52609 RVA: 0x004B8A9C File Offset: 0x004B6C9C
		public override string ExplanationPart(StatRequest req)
		{
			if (this.HasWounds(req))
			{
				return "StatsReport_HasHediffExplanation".Translate() + ": x" + this.factor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CD82 RID: 52610 RVA: 0x004B8AD4 File Offset: 0x004B6CD4
		private bool HasWounds(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
					for (int i = 0; i < hediffs.Count; i++)
					{
						if (hediffs[i].def != HediffDefOf.ExecutionCut && typeof(Hediff_Injury).IsAssignableFrom(hediffs[i].def.hediffClass) && !hediffs[i].IsPermanent())
						{
							return true;
						}
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x0400830D RID: 33549
		private float factor;
	}
}
