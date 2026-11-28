using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F96 RID: 8086
	public class StatPart_NoxiousHaze : StatPart
	{
		// Token: 0x0600CD84 RID: 52612 RVA: 0x004B8B6C File Offset: 0x004B6D6C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ActiveFor(req.Thing))
			{
				val *= this.multiplier;
			}
		}

		// Token: 0x0600CD85 RID: 52613 RVA: 0x004B8B88 File Offset: 0x004B6D88
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_MultiplierFor".Translate(GameConditionDefOf.NoxiousHaze.label) + (": x" + this.multiplier.ToStringPercent());
			}
			return null;
		}

		// Token: 0x0600CD86 RID: 52614 RVA: 0x004B8BE7 File Offset: 0x004B6DE7
		private bool ActiveFor(Thing t)
		{
			return ModLister.CheckBiotech("Noxious haze stat part") && (t != null && t.def.deteriorateFromEnvironmentalEffects) && NoxiousHazeUtility.IsExposedToNoxiousHaze(t);
		}

		// Token: 0x0400830E RID: 33550
		private float multiplier = 1f;
	}
}
