using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FB4 RID: 8116
	public class StatPart_WorkTableTemperature : StatPart
	{
		// Token: 0x0600CDFF RID: 52735 RVA: 0x004BA919 File Offset: 0x004B8B19
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				val *= 0.7f;
			}
		}

		// Token: 0x0600CE00 RID: 52736 RVA: 0x004BA93C File Offset: 0x004B8B3C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				return "BadTemperature".Translate().CapitalizeFirst() + ": x" + 0.7f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600CE01 RID: 52737 RVA: 0x004BA992 File Offset: 0x004B8B92
		public static bool Applies(Thing t)
		{
			return t.Spawned && StatPart_WorkTableTemperature.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x0600CE02 RID: 52738 RVA: 0x004BA9B8 File Offset: 0x004B8BB8
		public static bool Applies(ThingDef tDef, Map map, IntVec3 c)
		{
			if (map == null)
			{
				return false;
			}
			if (tDef.building == null)
			{
				return false;
			}
			float temperatureForCell = GenTemperature.GetTemperatureForCell(c, map);
			return temperatureForCell < 9f || temperatureForCell > 35f;
		}

		// Token: 0x04008344 RID: 33604
		public const float WorkRateFactor = 0.7f;

		// Token: 0x04008345 RID: 33605
		public const float MinTemp = 9f;

		// Token: 0x04008346 RID: 33606
		public const float MaxTemp = 35f;
	}
}
