using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F9C RID: 8092
	public class StatPart_Pollution : StatPart
	{
		// Token: 0x0600CD9D RID: 52637 RVA: 0x004B9052 File Offset: 0x004B7252
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ActiveFor(req.Thing))
			{
				val *= this.multiplier;
			}
		}

		// Token: 0x0600CD9E RID: 52638 RVA: 0x004B9070 File Offset: 0x004B7270
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_Pollution".Translate() + (": x" + this.multiplier.ToStringPercent());
			}
			return null;
		}

		// Token: 0x0600CD9F RID: 52639 RVA: 0x004B90C0 File Offset: 0x004B72C0
		private bool ActiveFor(Thing t)
		{
			return ModLister.CheckBiotech("Pollution stat part") && (t != null && t.Spawned && t.def.deteriorateFromEnvironmentalEffects) && t.Position.IsPolluted(t.Map);
		}

		// Token: 0x04008315 RID: 33557
		private float multiplier = 1f;
	}
}
