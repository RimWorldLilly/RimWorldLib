using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FA5 RID: 8101
	public class StatPart_ShamblerCorpse : StatPart
	{
		// Token: 0x0600CDC3 RID: 52675 RVA: 0x004B9B35 File Offset: 0x004B7D35
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ActiveFor(req.Thing))
			{
				val *= this.multiplier;
			}
		}

		// Token: 0x0600CDC4 RID: 52676 RVA: 0x004B9B54 File Offset: 0x004B7D54
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_MultiplierFor".Translate(HediffDefOf.ShamblerCorpse.label) + (": x" + this.multiplier.ToStringPercent());
			}
			return null;
		}

		// Token: 0x0600CDC5 RID: 52677 RVA: 0x004B9BB4 File Offset: 0x004B7DB4
		private bool ActiveFor(Thing t)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (t == null)
			{
				return false;
			}
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.health.hediffSet.HasHediff(HediffDefOf.ShamblerCorpse, false);
		}

		// Token: 0x04008335 RID: 33589
		private float multiplier = 1f;
	}
}
