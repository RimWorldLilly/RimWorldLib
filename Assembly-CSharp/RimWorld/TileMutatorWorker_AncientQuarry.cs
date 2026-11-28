using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200130C RID: 4876
	public class TileMutatorWorker_AncientQuarry : TileMutatorWorker
	{
		// Token: 0x0600791C RID: 31004 RVA: 0x002B0428 File Offset: 0x002AE628
		public TileMutatorWorker_AncientQuarry(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600791D RID: 31005 RVA: 0x002B0434 File Offset: 0x002AE634
		public override void GeneratePostFog(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			int randomInRange = TileMutatorWorker_AncientQuarry.NumQuarriedAreasRange.RandomInRange;
			Predicate<IntVec3> <>9__2;
			Predicate<IntVec3> <>9__0;
			Func<IntVec3, bool> <>9__1;
			for (int i = 0; i < randomInRange; i++)
			{
				Map map2 = map;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = delegate(IntVec3 c)
					{
						if (c.Fogged(map))
						{
							return false;
						}
						if (c.GetEdifice(map) == null)
						{
							return false;
						}
						if (!c.GetEdifice(map).def.IsNonResourceNaturalRock)
						{
							return false;
						}
						bool flag2 = false;
						foreach (IntVec3 b in GenAdj.CardinalDirections)
						{
							IntVec3 c2 = c + b;
							if (c2.InBounds(map) && c2.GetEdifice(map) == null)
							{
								flag2 = true;
							}
						}
						if (!flag2)
						{
							return false;
						}
						int size = 0;
						FloodFiller floodFiller = map.floodFiller;
						Predicate<IntVec3> passCheck;
						if ((passCheck = <>9__2) == null)
						{
							passCheck = (<>9__2 = delegate(IntVec3 x)
							{
								Building edifice2 = x.GetEdifice(map);
								return edifice2 != null && edifice2.def.mineable;
							});
						}
						floodFiller.FloodFill(c, passCheck, delegate(IntVec3 x)
						{
							int size = size;
							size++;
						}, int.MaxValue, false, null);
						return size >= TileMutatorWorker_AncientQuarry.QuarrySizeRange.max * 2;
					});
				}
				IntVec3 center;
				if (CellFinder.TryFindRandomCell(map2, validator, out center))
				{
					int randomInRange2 = TileMutatorWorker_AncientQuarry.QuarrySizeRange.RandomInRange;
					List<IntVec3> list = GridShapeMaker.IrregularLump(center, map, randomInRange2, null);
					IEnumerable<IntVec3> source = list;
					Func<IntVec3, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((IntVec3 c) => c.Roofed(map)));
					}
					bool flag = source.All(predicate);
					foreach (IntVec3 intVec in list)
					{
						Building edifice = intVec.GetEdifice(map);
						if (edifice != null)
						{
							if (!edifice.def.mineable)
							{
								continue;
							}
							edifice.Destroy(DestroyMode.Vanish);
							map.fogGrid.FloodUnfogAdjacent(intVec, false);
							if (Rand.Chance(0.8f))
							{
								GenSpawn.Spawn(ThingDefOf.Filth_RubbleRock, intVec, map, WipeMode.Vanish);
							}
							if (Rand.Chance(0.1f) && edifice.def.building.mineableThing != null)
							{
								Thing thing = GenSpawn.Spawn(edifice.def.building.mineableThing, intVec, map, WipeMode.Vanish);
								thing.stackCount = Rand.Range(1, Mathf.Max(1, edifice.def.building.EffectiveMineableYield));
								if (thing.def.EverHaulable && !thing.def.designateHaulable)
								{
									thing.SetForbidden(true, false);
								}
							}
						}
						if (!flag)
						{
							map.roofGrid.SetRoof(intVec, null);
						}
					}
					if (flag)
					{
						foreach (IntVec3 intVec2 in list)
						{
							if (!RoofCollapseUtility.WithinRangeOfRoofHolder(intVec2, map, false))
							{
								ThingDef rock = DeepDrillUtility.RockForTerrain(intVec2.GetTerrain(map));
								if (rock != null)
								{
									ThingDef thingDef = DefDatabase<ThingDef>.AllDefs.FirstOrDefault((ThingDef def) => def.IsStuff && def.stuffProps.SourceNaturalRock == rock);
									if (thingDef == null || !thingDef.stuffProps.CanMake(ThingDefOf.Column))
									{
										thingDef = GenStuff.RandomStuffByCommonalityFor(ThingDefOf.Column, TechLevel.Undefined);
									}
									GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Column, thingDef), intVec2, map, WipeMode.Vanish);
								}
							}
						}
					}
				}
			}
			RoofCollapseCellsFinder.CheckAndRemoveCollpsingRoofs(map);
		}

		// Token: 0x04004BFA RID: 19450
		private static readonly IntRange NumQuarriedAreasRange = new IntRange(10, 15);

		// Token: 0x04004BFB RID: 19451
		private static readonly IntRange QuarrySizeRange = new IntRange(100, 500);
	}
}
