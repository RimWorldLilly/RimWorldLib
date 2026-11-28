using System;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020BF RID: 8383
	public class SketchResolver_AncientPowerRoom : SketchResolver
	{
		// Token: 0x0600D326 RID: 54054 RVA: 0x004DB0F0 File Offset: 0x004D92F0
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0600D327 RID: 54055 RVA: 0x004DB10C File Offset: 0x004D930C
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient power room"))
			{
				return;
			}
			CellRect value = parms.rect.Value;
			ThingDef ancientGenerator = ThingDefOf.AncientGenerator;
			foreach (IntVec3 intVec in value.Cells)
			{
				if (!Rand.Chance(0.85f))
				{
					CellRect cellRect = GenAdj.OccupiedRect(intVec, ancientGenerator.defaultPlacingRot, ancientGenerator.size);
					bool flag = true;
					foreach (IntVec3 pos in cellRect.ExpandedBy(1))
					{
						if (parms.sketch.ThingsAt(pos).Any<SketchThing>())
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						parms.sketch.AddThing(ancientGenerator, intVec, ancientGenerator.defaultPlacingRot, null, 1, null, null, true, 2f);
						ScatterDebrisUtility.ScatterAround(intVec, ancientGenerator.size, Rot4.North, parms.sketch, ThingDefOf.Filth_OilSmear, 0.45f, 1, int.MaxValue, null);
					}
				}
			}
			SketchResolveParams parms2 = parms;
			parms2.thingCentral = ancientGenerator;
			SketchResolverDefOf.AddThingsCentral.Resolve(parms2);
		}

		// Token: 0x040085D6 RID: 34262
		private const float SkipChance = 0.85f;

		// Token: 0x040085D7 RID: 34263
		private const float OilSmearChance = 0.45f;
	}
}
