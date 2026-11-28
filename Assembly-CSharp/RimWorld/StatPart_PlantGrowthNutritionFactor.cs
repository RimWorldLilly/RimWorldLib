using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F9A RID: 8090
	public class StatPart_PlantGrowthNutritionFactor : StatPart
	{
		// Token: 0x0600CD95 RID: 52629 RVA: 0x004B8EB8 File Offset: 0x004B70B8
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600CD96 RID: 52630 RVA: 0x004B8ED8 File Offset: 0x004B70D8
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetFactor(req, out f))
			{
				Plant plant = (Plant)req.Thing;
				TaggedString taggedString = "StatsReport_PlantGrowth".Translate(plant.Growth.ToStringPercent()) + ": x" + f.ToStringPercent();
				if (!plant.def.plant.Sowable)
				{
					taggedString += " (" + "StatsReport_PlantGrowth_Wild".Translate() + ")";
				}
				return taggedString;
			}
			return null;
		}

		// Token: 0x0600CD97 RID: 52631 RVA: 0x004B8F6C File Offset: 0x004B716C
		private bool TryGetFactor(StatRequest req, out float factor)
		{
			if (!req.HasThing)
			{
				factor = 1f;
				return false;
			}
			Plant plant = req.Thing as Plant;
			if (plant == null)
			{
				factor = 1f;
				return false;
			}
			factor = PlantUtility.NutritionFactorFromGrowth(plant.def, plant.Growth);
			return true;
		}
	}
}
