using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012D8 RID: 4824
	public class GenStep_UndercaveInterest : GenStep
	{
		// Token: 0x17001456 RID: 5206
		// (get) Token: 0x060077AA RID: 30634 RVA: 0x002A45EC File Offset: 0x002A27EC
		public override int SeedPart
		{
			get
			{
				return 26098423;
			}
		}

		// Token: 0x060077AB RID: 30635 RVA: 0x002A45F4 File Offset: 0x002A27F4
		public override void Generate(Map map, GenStepParams parms)
		{
			Thing pitGateExit = map.listerThings.ThingsOfDef(ThingDefOf.CaveExit).FirstOrDefault<Thing>();
			Pawn dreadmeld = map.mapPawns.AllPawnsSpawned.FirstOrDefault((Pawn p) => p.kindDef == PawnKindDefOf.Dreadmeld);
			int randomInRange = GenStep_UndercaveInterest.InterestPointCountRange.RandomInRange;
			List<IntVec3> interestPoints = new List<IntVec3>();
			Predicate<IntVec3> <>9__1;
			for (int i = 0; i < randomInRange; i++)
			{
				Map map2 = map;
				Predicate<IntVec3> validator;
				if ((validator = <>9__1) == null)
				{
					validator = (<>9__1 = ((IntVec3 c) => c.Standable(map) && c.DistanceToEdge(map) > 5 && (pitGateExit == null || !c.InHorDistOf(pitGateExit.Position, 10f)) && (dreadmeld == null || !c.InHorDistOf(dreadmeld.Position, 10f)) && !interestPoints.Any((IntVec3 p) => c.InHorDistOf(p, 10f))));
				}
				IntVec3 item;
				if (CellFinder.TryFindRandomCell(map2, validator, out item))
				{
					interestPoints.Add(item);
				}
			}
			foreach (IntVec3 intVec in interestPoints)
			{
				GenStep_UndercaveInterest.UnderCaveInterestKind underCaveInterestKind = Gen.RandomEnumValue<GenStep_UndercaveInterest.UnderCaveInterestKind>(false);
				foreach (IntVec3 c2 in GridShapeMaker.IrregularLump(intVec, map, 20, null))
				{
					foreach (Thing thing in c2.GetThingList(map).ToList<Thing>())
					{
						if (thing.def.destroyable)
						{
							Building edifice = c2.GetEdifice(map);
							bool? flag;
							if (edifice == null)
							{
								flag = null;
							}
							else
							{
								ThingDef def = edifice.def;
								if (def == null)
								{
									flag = null;
								}
								else
								{
									BuildingProperties building = def.building;
									flag = ((building != null) ? new bool?(building.isNaturalRock) : null);
								}
							}
							bool? flag2 = flag;
							if (!flag2.GetValueOrDefault())
							{
								Building edifice2 = c2.GetEdifice(map);
								if (((edifice2 != null) ? edifice2.def : null) != ThingDefOf.Fleshmass)
								{
									continue;
								}
							}
							thing.Destroy(DestroyMode.Vanish);
						}
					}
				}
				switch (underCaveInterestKind)
				{
				case GenStep_UndercaveInterest.UnderCaveInterestKind.MushroomPatch:
					this.GenerateMushroomPatch(map, intVec);
					break;
				case GenStep_UndercaveInterest.UnderCaveInterestKind.ChemfuelGenerator:
					this.GenerateChemfuel(map, intVec);
					break;
				case GenStep_UndercaveInterest.UnderCaveInterestKind.InsectHive:
					this.GenerateHive(map, intVec);
					break;
				case GenStep_UndercaveInterest.UnderCaveInterestKind.CorpseGear:
					this.GenerateCorpseGear(map, intVec);
					break;
				case GenStep_UndercaveInterest.UnderCaveInterestKind.CorpsePile:
					GenStep_UndercaveInterest.GenerateCorpsePile(map, intVec);
					break;
				case GenStep_UndercaveInterest.UnderCaveInterestKind.SleepingFleshbeasts:
					this.GenerateSleepingFleshbeasts(map, intVec);
					break;
				}
			}
		}

		// Token: 0x060077AC RID: 30636 RVA: 0x002A48EC File Offset: 0x002A2AEC
		private void GenerateMushroomPatch(Map map, IntVec3 cell)
		{
			List<ThingDef> source = new List<ThingDef>
			{
				ThingDefOf.Plant_HealrootWild,
				ThingDefOf.Glowstool,
				ThingDefOf.Bryolux,
				ThingDefOf.Agarilux
			};
			foreach (IntVec3 intVec in GridShapeMaker.IrregularLump(cell, map, GenStep_UndercaveInterest.PatchSizeRange.RandomInRange, null))
			{
				ThingDef thingDef = source.RandomElement<ThingDef>();
				if (GenSpawn.CanSpawnAt(thingDef, intVec, map, null, true))
				{
					map.terrainGrid.SetTerrain(intVec, TerrainDefOf.SoilRich);
					Thing thing = ThingMaker.MakeThing(thingDef, null);
					if (Rand.Chance(0.7f) && GenPlace.TryPlaceThing(thing, intVec, map, ThingPlaceMode.Direct, null, null, null, 1))
					{
						Plant plant = thing as Plant;
						if (plant != null)
						{
							plant.Growth = Mathf.Clamp01(WildPlantSpawner.InitialGrowthRandomRange.RandomInRange);
						}
					}
				}
			}
		}

		// Token: 0x060077AD RID: 30637 RVA: 0x002A4A04 File Offset: 0x002A2C04
		private void GenerateChemfuel(Map map, IntVec3 cell)
		{
			if (!GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.AncientGenerator ?? ThingDefOf.ChemfuelPoweredGenerator, null), cell, map, ThingPlaceMode.Direct, null, null, null, 1))
			{
				return;
			}
			int randomInRange = GenStep_UndercaveInterest.ChemfuelCountRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Chemfuel, null);
				thing.stackCount = GenStep_UndercaveInterest.ChemfuelStackCountRange.RandomInRange;
				GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Radius, null, null, null, 2);
			}
			foreach (IntVec3 c in GridShapeMaker.IrregularLump(cell, map, 20, null))
			{
				if (c.GetEdifice(map) == null)
				{
					FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_Fuel, 1, FilthSourceFlags.None, true);
				}
			}
		}

		// Token: 0x060077AE RID: 30638 RVA: 0x002A4AEC File Offset: 0x002A2CEC
		private void GenerateHive(Map map, IntVec3 cell)
		{
			HiveUtility.SpawnHive(cell, map, WipeMode.VanishOrMoveAside, false, false, true, false, true, false, true);
			int randomInRange = GenStep_UndercaveInterest.JellyCountRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.InsectJelly, null);
				thing.stackCount = GenStep_UndercaveInterest.JellyStackCountRange.RandomInRange;
				thing.SetForbidden(true, true);
				GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Radius, null, null, null, 3);
			}
		}

		// Token: 0x060077AF RID: 30639 RVA: 0x002A4B60 File Offset: 0x002A2D60
		private void GenerateCorpseGear(Map map, IntVec3 cell)
		{
			List<ThingDef> list = new List<ThingDef>();
			list.Add(ThingDefOf.MedicineIndustrial);
			list.Add(ThingDefOf.MealSurvivalPack);
			Faction faction;
			Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, true, false, TechLevel.Undefined, TechLevel.Undefined, false, false);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Drifter, faction, PawnGenerationContext.NonPlayer, new PlanetTile?(map.Tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			pawn.health.SetDead();
			Corpse corpse = pawn.MakeCorpse(null, null);
			corpse.Age = Mathf.RoundToInt((float)(GenStep_UndercaveInterest.CorpseAgeRangeDays.RandomInRange * 60000));
			corpse.GetComp<CompRottable>().RotProgress += (float)corpse.Age;
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			GenSpawn.Spawn(corpse, cell, map, WipeMode.Vanish);
			Thing thing = ThingMaker.MakeThing(list.RandomElement<ThingDef>(), null);
			thing.stackCount = GenStep_UndercaveInterest.GearStackCountRange.RandomInRange;
			GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Radius, null, null, null, 1);
		}

		// Token: 0x060077B0 RID: 30640 RVA: 0x002A4CCC File Offset: 0x002A2ECC
		public static void GenerateCorpsePile(Map map, IntVec3 cell)
		{
			int randomInRange = GenStep_UndercaveInterest.CorpseCountRange.RandomInRange;
			Faction faction;
			Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, true, false, TechLevel.Undefined, TechLevel.Undefined, false, false);
			int age = Mathf.RoundToInt((float)(GenStep_UndercaveInterest.CorpseAgeRangeDays.RandomInRange * 60000));
			for (int i = 0; i < randomInRange; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Drifter, faction, PawnGenerationContext.NonPlayer, new PlanetTile?(map.Tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				pawn.Kill(null, null);
				pawn.Corpse.Age = age;
				pawn.Corpse.GetComp<CompRottable>().RotProgress += (float)pawn.Corpse.Age;
				GenPlace.TryPlaceThing(pawn.Corpse, cell, map, ThingPlaceMode.Radius, null, null, null, 4);
				pawn.Corpse.SetForbidden(true, true);
			}
		}

		// Token: 0x060077B1 RID: 30641 RVA: 0x002A4E34 File Offset: 0x002A3034
		private void GenerateSleepingFleshbeasts(Map map, IntVec3 cell)
		{
			int randomInRange = GenStep_UndercaveInterest.NumFleshbeastsRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				Pawn thing = PawnGenerator.GeneratePawn(PawnKindDefOf.Fingerspike, Faction.OfEntities, null);
				GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Radius, null, null, null, 4);
				CompCanBeDormant compCanBeDormant;
				if (thing.TryGetComp(out compCanBeDormant))
				{
					compCanBeDormant.ToSleep();
				}
			}
		}

		// Token: 0x04004B1D RID: 19229
		private static readonly IntRange InterestPointCountRange = new IntRange(3, 5);

		// Token: 0x04004B1E RID: 19230
		private const int InterestPointSize = 20;

		// Token: 0x04004B1F RID: 19231
		private const float MinDistApart = 10f;

		// Token: 0x04004B20 RID: 19232
		private const float PatchDensity = 0.7f;

		// Token: 0x04004B21 RID: 19233
		private static readonly IntRange PatchSizeRange = new IntRange(50, 70);

		// Token: 0x04004B22 RID: 19234
		private static readonly IntRange ChemfuelCountRange = new IntRange(3, 5);

		// Token: 0x04004B23 RID: 19235
		private static readonly IntRange ChemfuelStackCountRange = new IntRange(10, 20);

		// Token: 0x04004B24 RID: 19236
		private const int ChemfuelSpawnRadius = 2;

		// Token: 0x04004B25 RID: 19237
		private const int ChemfuelPuddleSize = 20;

		// Token: 0x04004B26 RID: 19238
		private static readonly IntRange JellyCountRange = new IntRange(2, 3);

		// Token: 0x04004B27 RID: 19239
		private static readonly IntRange JellyStackCountRange = new IntRange(15, 40);

		// Token: 0x04004B28 RID: 19240
		private const int JellySpawnRadius = 3;

		// Token: 0x04004B29 RID: 19241
		private static readonly IntRange CorpseAgeRangeDays = new IntRange(15, 120);

		// Token: 0x04004B2A RID: 19242
		private const int GearSpawnRadius = 1;

		// Token: 0x04004B2B RID: 19243
		private static readonly IntRange GearStackCountRange = new IntRange(2, 5);

		// Token: 0x04004B2C RID: 19244
		private static readonly IntRange CorpseCountRange = new IntRange(3, 6);

		// Token: 0x04004B2D RID: 19245
		private const int CorpseSpawnRadius = 4;

		// Token: 0x04004B2E RID: 19246
		private static readonly IntRange NumFleshbeastsRange = new IntRange(2, 4);

		// Token: 0x04004B2F RID: 19247
		private const int SleepingFleshbeastSpawnRadius = 4;

		// Token: 0x020031D7 RID: 12759
		private enum UnderCaveInterestKind
		{
			// Token: 0x0400C911 RID: 51473
			MushroomPatch,
			// Token: 0x0400C912 RID: 51474
			ChemfuelGenerator,
			// Token: 0x0400C913 RID: 51475
			InsectHive,
			// Token: 0x0400C914 RID: 51476
			CorpseGear,
			// Token: 0x0400C915 RID: 51477
			CorpsePile,
			// Token: 0x0400C916 RID: 51478
			SleepingFleshbeasts
		}
	}
}
