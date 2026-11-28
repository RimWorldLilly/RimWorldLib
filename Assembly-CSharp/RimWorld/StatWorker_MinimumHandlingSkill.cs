using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FCA RID: 8138
	public class StatWorker_MinimumHandlingSkill : StatWorker
	{
		// Token: 0x0600CE9B RID: 52891 RVA: 0x004C0E38 File Offset: 0x004BF038
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return this.ValueFromReq(req);
		}

		// Token: 0x0600CE9C RID: 52892 RVA: 0x004C0E44 File Offset: 0x004BF044
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef.race.Humanlike)
			{
				return "StatsReport_BaseValue".Translate() + ": " + this.ValueFromReq(req).ToString("F0");
			}
			Thing thing = req.Thing;
			float f = (thing != null) ? thing.GetStatValue(StatDefOf.Wildness, true, -1) : thingDef.GetStatValueAbstract(StatDefOf.Wildness, null);
			return "Wildness".Translate() + " " + f.ToStringPercent() + ": " + this.ValueFromReq(req).ToString("F0");
		}

		// Token: 0x0600CE9D RID: 52893 RVA: 0x004C0F0C File Offset: 0x004BF10C
		private float ValueFromReq(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef.race.Humanlike)
			{
				return 0f;
			}
			Thing thing = req.Thing;
			float x = (thing != null) ? thing.GetStatValue(StatDefOf.Wildness, true, -1) : thingDef.GetStatValueAbstract(StatDefOf.Wildness, null);
			return Mathf.Clamp(StatWorker_MinimumHandlingSkill.HandlingSkillFromWildness.Evaluate(x), 0f, 20f);
		}

		// Token: 0x04008362 RID: 33634
		private static readonly SimpleCurve HandlingSkillFromWildness = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.15f, 1f),
				true
			},
			{
				new CurvePoint(0.98f, 10f),
				true
			},
			{
				new CurvePoint(0.99f, 14f),
				true
			}
		};
	}
}
