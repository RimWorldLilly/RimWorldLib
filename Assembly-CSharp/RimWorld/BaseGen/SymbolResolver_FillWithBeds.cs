using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002126 RID: 8486
	public class SymbolResolver_FillWithBeds : SymbolResolver
	{
		// Token: 0x0600D4B7 RID: 54455 RVA: 0x004EC00C File Offset: 0x004EA20C
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef;
			if (rp.singleThingDef != null)
			{
				thingDef = rp.singleThingDef;
			}
			else if (rp.faction != null && rp.faction.def.techLevel >= TechLevel.Medieval)
			{
				thingDef = ThingDefOf.Bed;
			}
			else
			{
				thingDef = Rand.Element<ThingDef>(ThingDefOf.Bed, ThingDefOf.Bedroll, ThingDefOf.SleepingSpot);
			}
			ThingDef singleThingStuff;
			if (rp.singleThingStuff != null && rp.singleThingStuff.stuffProps.CanMake(thingDef))
			{
				singleThingStuff = rp.singleThingStuff;
			}
			else
			{
				singleThingStuff = BaseGenUtility.CheapStuffFor(thingDef, rp.faction);
			}
			bool @bool = Rand.Bool;
			int valueOrDefault = rp.bedCount.GetValueOrDefault(int.MaxValue);
			int num = 0;
			foreach (IntVec3 intVec in rp.rect)
			{
				if (@bool)
				{
					if (intVec.x % 3 != 0)
					{
						continue;
					}
					if (intVec.z % 2 != 0)
					{
						continue;
					}
				}
				else if (intVec.x % 2 != 0 || intVec.z % 3 != 0)
				{
					continue;
				}
				Rot4 rot = @bool ? Rot4.West : Rot4.North;
				if (!GenSpawn.WouldWipeAnythingWith(intVec, rot, thingDef, map, (Thing x) => x.def.category == ThingCategory.Building) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(GenAdj.OccupiedRect(intVec, rot, thingDef.Size), map))
				{
					ResolveParams resolveParams = rp;
					resolveParams.rect = GenAdj.OccupiedRect(intVec, rot, thingDef.size);
					resolveParams.singleThingDef = thingDef;
					resolveParams.singleThingStuff = singleThingStuff;
					resolveParams.thingRot = new Rot4?(rot);
					BaseGen.symbolStack.Push("bed", resolveParams, null);
					num++;
					if (num >= valueOrDefault)
					{
						break;
					}
				}
			}
		}
	}
}
