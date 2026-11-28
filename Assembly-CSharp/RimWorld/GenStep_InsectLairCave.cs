using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020012D4 RID: 4820
	public class GenStep_InsectLairCave : GenStep
	{
		// Token: 0x17001451 RID: 5201
		// (get) Token: 0x06007788 RID: 30600 RVA: 0x002A2D47 File Offset: 0x002A0F47
		public InsectLairEntrance Entrance
		{
			get
			{
				return this.exit.entrance as InsectLairEntrance;
			}
		}

		// Token: 0x17001452 RID: 5202
		// (get) Token: 0x06007789 RID: 30601 RVA: 0x002A2D59 File Offset: 0x002A0F59
		public override int SeedPart
		{
			get
			{
				return 98236175;
			}
		}

		// Token: 0x0600778A RID: 30602 RVA: 0x002A2D60 File Offset: 0x002A0F60
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.poiCells.Clear();
			ModuleBase moduleBase = new Perlin(0.1, 0.9, 1.5, 4, Rand.Int, QualityMode.Medium);
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (intVec.GetEdifice(map) == null && GenConstruct.CanBuildOnTerrain(TerrainDefOf.InsectSludge, intVec, map, Rot4.North, null, null) && moduleBase.GetValue(intVec) > 0.2f)
				{
					this.TryPlaceSludge(intVec, map);
				}
			}
			for (int i = 0; i < GenStep_InsectLairCave.NumThreatPOIsRange.max + GenStep_InsectLairCave.NumMiscPOIsRange.max + 1; i++)
			{
				IntVec3 poilocation = this.GetPOILocation(map);
				if (poilocation.IsValid)
				{
					this.poiCells.Add(poilocation);
				}
			}
			GenStep_InsectLairCave.GenerateRandomBurrowWalls(map);
			foreach (IntVec3 intVec2 in map.AllCells)
			{
				if (Rand.Chance(0.02f) && intVec2.GetTerrain(map) == TerrainDefOf.InsectSludge)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.EggSac, null);
					thing.SetFaction(Faction.OfInsects, null);
					GenPlace.TryPlaceThing(thing, intVec2, map, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
			this.exit = (map.listerThings.ThingsOfDef(ThingDefOf.CaveExit).FirstOrDefault<Thing>() as PocketMapExit);
			if (this.exit == null)
			{
				Log.Error("No cave entrance found for Insect Lair POI generation.");
			}
			this.GenerateBossPOI(map);
			this.GenerateThreatPOIs(map);
			this.GenerateMiscPOIs(map);
			PocketMapParent pocketMapParent = map.Parent as PocketMapParent;
			Map target = ((pocketMapParent != null) ? pocketMapParent.sourceMap : null) ?? map;
			this.GenerateRandomHives(map, Mathf.RoundToInt(GenStep_InsectLairCave.HiveThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(target))));
		}

		// Token: 0x0600778B RID: 30603 RVA: 0x002A2F60 File Offset: 0x002A1160
		private static void GenerateRandomBurrowWalls(Map map)
		{
			Perlin perlin = new Perlin(0.08, 2.0, 2.0, 2, Rand.Int, QualityMode.Medium);
			foreach (IntVec3 intVec in map.AllCells)
			{
				float value = perlin.GetValue(intVec);
				if (intVec.GetFirstBuilding(map) == null && value > 0.7f && GenSpawn.CanSpawnAt(ThingDefOf.BurrowWall, intVec, map, null, true))
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.BurrowWall, null);
					if (GenPlace.TryPlaceThing(thing, intVec, map, ThingPlaceMode.Direct, null, null, null, 1))
					{
						thing.SetFaction(Faction.OfInsects, null);
					}
				}
			}
		}

		// Token: 0x0600778C RID: 30604 RVA: 0x002A3034 File Offset: 0x002A1234
		private void GenerateBossPOI(Map map)
		{
			IntVec3 intVec = this.poiCells.OrderBy(delegate(IntVec3 c)
			{
				PocketMapExit pocketMapExit = this.exit;
				return -c.DistanceToSquared((pocketMapExit != null) ? pocketMapExit.Position : map.Center);
			}).First<IntVec3>();
			this.poiCells.Remove(intVec);
			this.ClearArea(intVec, map);
			List<IntVec3> cells;
			this.GeneratePOIWalls(intVec, map, out cells);
			this.GenerateBossRoom(intVec, map);
			string signalTag = "queenApproached-" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			CellRect rect = CellRect.FromCellList(cells).ExpandedBy(2).ClipInsideMap(map);
			RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
			rectTrigger.signalTag = signalTag;
			rectTrigger.Rect = rect;
			rectTrigger.destroyIfUnfogged = true;
			GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
			SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
			signalAction_Letter.signalTag = signalTag;
			signalAction_Letter.letterDef = LetterDefOf.ThreatBig;
			signalAction_Letter.letterLabelKey = "LetterLabelInsectQueenWarning";
			signalAction_Letter.letterMessageKey = "LetterInsectQueenWarning";
			GenSpawn.Spawn(signalAction_Letter, rect.CenterCell, map, WipeMode.Vanish);
		}

		// Token: 0x0600778D RID: 30605 RVA: 0x002A3170 File Offset: 0x002A1370
		private void GenerateThreatPOIs(Map map)
		{
			int randomInRange = GenStep_InsectLairCave.NumThreatPOIsRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				GenStep_InsectLairCave.ThreatPOIType threatPOIType = Rand.EnumValue<GenStep_InsectLairCave.ThreatPOIType>();
				if (!this.poiCells.Empty<IntVec3>())
				{
					IntVec3 intVec = this.poiCells.RandomElement<IntVec3>();
					this.poiCells.Remove(intVec);
					this.ClearArea(intVec, map);
					List<IntVec3> list;
					this.GeneratePOIWalls(intVec, map, out list);
					switch (threatPOIType)
					{
					case GenStep_InsectLairCave.ThreatPOIType.InsectHives:
						this.GenerateInsectHivePOI(intVec, map);
						break;
					case GenStep_InsectLairCave.ThreatPOIType.InsectCocoons:
						this.GenerateInsectCocoonPOI(intVec, map);
						break;
					case GenStep_InsectLairCave.ThreatPOIType.Ambush:
						this.GenerateAmbushPOI(intVec, map);
						break;
					}
				}
			}
		}

		// Token: 0x0600778E RID: 30606 RVA: 0x002A320C File Offset: 0x002A140C
		private void GenerateMiscPOIs(Map map)
		{
			int randomInRange = GenStep_InsectLairCave.NumMiscPOIsRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				GenStep_InsectLairCave.MiscPOIType miscPOIType = Rand.EnumValue<GenStep_InsectLairCave.MiscPOIType>();
				if (!this.poiCells.Empty<IntVec3>())
				{
					IntVec3 intVec = this.poiCells.RandomElement<IntVec3>();
					this.poiCells.Remove(intVec);
					this.ClearArea(intVec, map);
					List<IntVec3> list;
					this.GeneratePOIWalls(intVec, map, out list);
					switch (miscPOIType)
					{
					case GenStep_InsectLairCave.MiscPOIType.EggSacs:
						this.GenerateEggSacs(intVec, map, Rand.Range(5, 10));
						break;
					case GenStep_InsectLairCave.MiscPOIType.BoomShrooms:
						this.GeneratePlants(ThingDefOf.Boomshroom, intVec, map, Rand.Range(5, 10));
						break;
					case GenStep_InsectLairCave.MiscPOIType.Mushrooms:
						this.GeneratePlants(ThingDefOf.Plant_Psilocap, intVec, map, Rand.Range(5, 10));
						this.GeneratePlants(ThingDefOf.Agarilux, intVec, map, Rand.Range(5, 10));
						break;
					case GenStep_InsectLairCave.MiscPOIType.Corpses:
						GenStep_UndercaveInterest.GenerateCorpsePile(map, intVec);
						break;
					case GenStep_InsectLairCave.MiscPOIType.HermeticCrate:
						this.GenerateHermeticCratePOI(intVec, map);
						break;
					}
				}
			}
		}

		// Token: 0x0600778F RID: 30607 RVA: 0x002A3308 File Offset: 0x002A1508
		private void GenerateRandomHives(Map map, int count)
		{
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < count; i++)
			{
				Map map2 = map;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.Hive, c, map, null, true) && !c.InHorDistOf(this.exit.Position, 20f)));
				}
				IntVec3 spawnCell;
				if (CellFinder.TryFindRandomCell(map2, validator, out spawnCell))
				{
					HiveUtility.SpawnHive(spawnCell, map, WipeMode.VanishOrMoveAside, true, false, true, false, true, false, true);
				}
			}
		}

		// Token: 0x06007790 RID: 30608 RVA: 0x002A3378 File Offset: 0x002A1578
		private IntVec3 GetPOILocation(Map map)
		{
			Thing entrance = map.listerThings.ThingsOfDef(ThingDefOf.CaveExit).FirstOrDefault<Thing>();
			if (entrance == null)
			{
				Log.Error("No cave entrance found for Insect Lair POI generation.");
				return IntVec3.Invalid;
			}
			IntVec3 result;
			if (!CellFinder.TryFindRandomCell(map, delegate(IntVec3 c)
			{
				if (!c.Standable(map))
				{
					return false;
				}
				if (c.DistanceToEdge(map) < 10)
				{
					return false;
				}
				if (entrance.Position.InHorDistOf(c, 20f))
				{
					return false;
				}
				foreach (IntVec3 otherLoc in this.poiCells)
				{
					if (c.InHorDistOf(otherLoc, 15f))
					{
						return false;
					}
				}
				return map.reachability.CanReach(c, entrance.Position, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors);
			}, out result))
			{
				return IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x06007791 RID: 30609 RVA: 0x002A33F4 File Offset: 0x002A15F4
		private void ClearArea(IntVec3 loc, Map map)
		{
			foreach (IntVec3 c2 in GridShapeMaker.IrregularLump(loc, map, 50, (IntVec3 c) => true))
			{
				Building edifice = c2.GetEdifice(map);
				if (edifice != null)
				{
					edifice.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06007792 RID: 30610 RVA: 0x002A3474 File Offset: 0x002A1674
		private void GeneratePOIWalls(IntVec3 loc, Map map, out List<IntVec3> shape)
		{
			shape = GridShapeMaker.IrregularLump(loc, map, 154, (IntVec3 c) => true);
			foreach (IntVec3 cell in shape)
			{
				this.TryPlaceSludge(cell, map);
			}
			List<IntVec3> list = new List<IntVec3>();
			foreach (IntVec3 intVec in shape)
			{
				bool flag = false;
				foreach (IntVec3 item in GenRadial.RadialCellsAround(intVec, 2f, false))
				{
					if (!shape.Contains(item))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					list.Add(intVec);
				}
			}
			foreach (IntVec3 item2 in list)
			{
				shape.Remove(item2);
			}
			foreach (IntVec3 intVec2 in shape)
			{
				if (intVec2.DistanceToEdge(map) > 1)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.BurrowWall, null);
					if (GenPlace.TryPlaceThing(thing, intVec2, map, ThingPlaceMode.Direct, null, null, null, 1))
					{
						thing.SetFaction(Faction.OfInsects, null);
					}
				}
			}
		}

		// Token: 0x06007793 RID: 30611 RVA: 0x002A3644 File Offset: 0x002A1844
		private void GenerateEggSacs(IntVec3 loc, Map map, int num)
		{
			for (int i = 0; i < num; i++)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.EggSac, null);
				thing.SetFaction(Faction.OfInsects, null);
				GenPlace.TryPlaceThing(thing, loc, map, ThingPlaceMode.Radius, null, null, null, 5);
			}
		}

		// Token: 0x06007794 RID: 30612 RVA: 0x002A368C File Offset: 0x002A188C
		private void GenerateHermeticCratePOI(IntVec3 loc, Map map)
		{
			Rot4 rot = Rot4.Random;
			IntVec3 cell;
			if (!CellFinder.TryRandomClosewalkCellNear(loc, map, 5, out cell, (IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.AncientHermeticCrate, c, map, new Rot4?(rot), true)))
			{
				return;
			}
			RoomGenUtility.SpawnCrate(ThingDefOf.AncientHermeticCrate, cell, map, rot, ThingSetMakerDefOf.MapGen_HighValueCrate, true, null);
			int num = Rand.Range(3, 4);
			this.GenerateEggSacs(loc, map, num);
		}

		// Token: 0x06007795 RID: 30613 RVA: 0x002A3704 File Offset: 0x002A1904
		private void GenerateBossRoom(IntVec3 loc, Map map)
		{
			GenStep_InsectLairCave.<>c__DisplayClass33_0 CS$<>8__locals1 = new GenStep_InsectLairCave.<>c__DisplayClass33_0();
			CS$<>8__locals1.map = map;
			IntVec3 intVec;
			if (!CellFinder.TryRandomClosewalkCellNear(loc, CS$<>8__locals1.map, 5, out intVec, null))
			{
				return;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.HiveQueen, Faction.OfInsects, null);
			pawn.TryGetComp<CompCanBeDormant>().ToSleep();
			GenSpawn.Spawn(pawn, intVec, CS$<>8__locals1.map, WipeMode.Vanish);
			LordMaker.MakeNewLord(Faction.OfInsects, new LordJob_HiveQueen(Faction.OfInsects, intVec, 12f, false, false), CS$<>8__locals1.map, Gen.YieldSingle<Pawn>(pawn));
			int num = Rand.Range(3, 4);
			this.GenerateEggSacs(loc, CS$<>8__locals1.map, num);
			int num2 = (int)GenStep_InsectLairCave.BossHivesThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap));
			for (int i = 0; i < num2; i++)
			{
				Map map2 = CS$<>8__locals1.map;
				int radius = 5;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = CS$<>8__locals1.<>9__0) == null)
				{
					extraValidator = (CS$<>8__locals1.<>9__0 = ((IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.Hive, c, CS$<>8__locals1.map, null, true)));
				}
				IntVec3 spawnCell;
				if (CellFinder.TryRandomClosewalkCellNear(loc, map2, radius, out spawnCell, extraValidator))
				{
					HiveUtility.SpawnHive(spawnCell, CS$<>8__locals1.map, WipeMode.VanishOrMoveAside, false, false, true, true, true, false, true);
				}
			}
			if (this.Entrance != null && this.Entrance.spawnGravcore)
			{
				GenSpawn.Spawn(ThingDefOf.Gravcore, loc, CS$<>8__locals1.map, WipeMode.Vanish).SetForbidden(true, true);
				int num3 = Rand.Range(2, 4);
				for (int j = 0; j < num3; j++)
				{
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.ShipChunk_Mech, null), loc, CS$<>8__locals1.map, ThingPlaceMode.Radius, null, null, null, 5);
				}
				int k = GenStep_InsectLairCave.GravlitePanelCount.RandomInRange;
				while (k > 0)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.GravlitePanel, null);
					thing.stackCount = Mathf.Min(k, thing.def.stackLimit);
					k -= thing.stackCount;
					thing.SetForbidden(true, true);
					GenPlace.TryPlaceThing(thing, loc, CS$<>8__locals1.map, ThingPlaceMode.Near, null, null, null, 1);
				}
				return;
			}
			Rot4 rot = Rot4.Random;
			IntVec3 cell;
			if (!CellFinder.TryRandomClosewalkCellNear(loc, CS$<>8__locals1.map, 5, out cell, (IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.AncientHermeticCrate, c, CS$<>8__locals1.map, new Rot4?(rot), true)))
			{
				return;
			}
			RoomGenUtility.SpawnCrate(ThingDefOf.AncientHermeticCrate, cell, CS$<>8__locals1.map, rot, ThingSetMakerDefOf.MapGen_HighValueCrate, true, null);
		}

		// Token: 0x06007796 RID: 30614 RVA: 0x002A3960 File Offset: 0x002A1B60
		private void GenerateInsectHivePOI(IntVec3 loc, Map map)
		{
			int num = (int)GenStep_InsectLairCave.HivesFromThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap));
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < num; i++)
			{
				Map map2 = map;
				int radius = 5;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__0) == null)
				{
					extraValidator = (<>9__0 = ((IntVec3 c) => GenSpawn.CanSpawnAt(ThingDefOf.Hive, c, map, null, true)));
				}
				IntVec3 spawnCell;
				if (CellFinder.TryRandomClosewalkCellNear(loc, map2, radius, out spawnCell, extraValidator))
				{
					HiveUtility.SpawnHive(spawnCell, map, WipeMode.VanishOrMoveAside, false, false, true, true, true, false, true);
				}
			}
			int num2 = Rand.Range(2, 4);
			this.GenerateEggSacs(loc, map, num2);
		}

		// Token: 0x06007797 RID: 30615 RVA: 0x002A39FC File Offset: 0x002A1BFC
		private void GenerateInsectCocoonPOI(IntVec3 loc, Map map)
		{
			IEnumerable<ThingDef> cocoonsToSpawn = CocoonInfestationUtility.GetCocoonsToSpawn(GenStep_InsectLairCave.CocoonThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap)));
			int nextCocoonGroupID = Find.UniqueIDsManager.GetNextCocoonGroupID();
			foreach (ThingDef def in cocoonsToSpawn)
			{
				Thing thing = ThingMaker.MakeThing(def, null);
				thing.SetFaction(Faction.OfInsects, null);
				thing.TryGetComp<CompWakeUpDormant>().groupID = nextCocoonGroupID;
				GenPlace.TryPlaceThing(thing, loc, map, ThingPlaceMode.Radius, null, null, null, 5);
			}
		}

		// Token: 0x06007798 RID: 30616 RVA: 0x002A3A94 File Offset: 0x002A1C94
		private void GenerateAmbushPOI(IntVec3 loc, Map map)
		{
			GenSpawn.Spawn(ThingDefOf.InsectAmbush, loc, map, WipeMode.Vanish);
			int num = Rand.Range(5, 10);
			this.GenerateEggSacs(loc, map, num);
		}

		// Token: 0x06007799 RID: 30617 RVA: 0x002A3AC4 File Offset: 0x002A1CC4
		private void GeneratePlants(ThingDef plantDef, IntVec3 loc, Map map, int count)
		{
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < count; i++)
			{
				Thing thing = ThingMaker.MakeThing(plantDef, null);
				Thing thing2 = thing;
				Map map2 = map;
				ThingPlaceMode mode = ThingPlaceMode.Radius;
				Action<Thing, int> placedAction = null;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__0) == null)
				{
					extraValidator = (<>9__0 = ((IntVec3 c) => c.GetPlant(map) == null));
				}
				Thing thing3;
				if (GenPlace.TryPlaceThing(thing2, loc, map2, mode, out thing3, placedAction, extraValidator, null, 5))
				{
					(thing as Plant).Growth = Mathf.Clamp01(WildPlantSpawner.InitialGrowthRandomRange.RandomInRange);
				}
			}
		}

		// Token: 0x0600779A RID: 30618 RVA: 0x002A3B50 File Offset: 0x002A1D50
		private void TryPlaceSludge(IntVec3 cell, Map map)
		{
			if (!GenConstruct.CanBuildOnTerrain(TerrainDefOf.InsectSludge, cell, map, Rot4.North, null, null))
			{
				return;
			}
			map.terrainGrid.SetTerrain(cell, TerrainDefOf.InsectSludge);
			if (Rand.Chance(0.2f))
			{
				GenSpawn.Spawn(ThingDefOf.Filth_Slime, cell, map, WipeMode.Vanish);
			}
		}

		// Token: 0x04004B01 RID: 19201
		private const int MinDistBetweenPOIs = 15;

		// Token: 0x04004B02 RID: 19202
		private const int MinDistFromEdge = 10;

		// Token: 0x04004B03 RID: 19203
		private const int MinDistFromEntrance = 20;

		// Token: 0x04004B04 RID: 19204
		private const float SludgeThreshold = 0.2f;

		// Token: 0x04004B05 RID: 19205
		public const float SlimeChance = 0.2f;

		// Token: 0x04004B06 RID: 19206
		public const float SacChance = 0.02f;

		// Token: 0x04004B07 RID: 19207
		private const int BurrowWallSize = 154;

		// Token: 0x04004B08 RID: 19208
		private static readonly IntRange NumThreatPOIsRange = new IntRange(2, 3);

		// Token: 0x04004B09 RID: 19209
		private static readonly IntRange NumMiscPOIsRange = new IntRange(3, 5);

		// Token: 0x04004B0A RID: 19210
		private static readonly IntRange GravlitePanelCount = new IntRange(100, 150);

		// Token: 0x04004B0B RID: 19211
		private static readonly SimpleCurve HivesFromThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 1f),
				true
			},
			{
				new CurvePoint(800f, 2f),
				true
			},
			{
				new CurvePoint(5000f, 4f),
				true
			},
			{
				new CurvePoint(10000f, 8f),
				true
			}
		};

		// Token: 0x04004B0C RID: 19212
		private static readonly SimpleCurve CocoonThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 100f),
				true
			},
			{
				new CurvePoint(1000f, 500f),
				true
			},
			{
				new CurvePoint(10000f, 2500f),
				true
			}
		};

		// Token: 0x04004B0D RID: 19213
		private static readonly SimpleCurve HiveThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 1f),
				true
			},
			{
				new CurvePoint(800f, 2f),
				true
			},
			{
				new CurvePoint(5000f, 7f),
				true
			},
			{
				new CurvePoint(10000f, 10f),
				true
			}
		};

		// Token: 0x04004B0E RID: 19214
		private static readonly SimpleCurve BossHivesThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 1f),
				true
			},
			{
				new CurvePoint(800f, 2f),
				true
			},
			{
				new CurvePoint(5000f, 4f),
				true
			},
			{
				new CurvePoint(10000f, 8f),
				true
			}
		};

		// Token: 0x04004B0F RID: 19215
		private List<IntVec3> poiCells = new List<IntVec3>();

		// Token: 0x04004B10 RID: 19216
		private PocketMapExit exit;

		// Token: 0x020031CA RID: 12746
		private enum ThreatPOIType
		{
			// Token: 0x0400C8EF RID: 51439
			InsectHives,
			// Token: 0x0400C8F0 RID: 51440
			InsectCocoons,
			// Token: 0x0400C8F1 RID: 51441
			Ambush
		}

		// Token: 0x020031CB RID: 12747
		private enum MiscPOIType
		{
			// Token: 0x0400C8F3 RID: 51443
			EggSacs,
			// Token: 0x0400C8F4 RID: 51444
			BoomShrooms,
			// Token: 0x0400C8F5 RID: 51445
			Mushrooms,
			// Token: 0x0400C8F6 RID: 51446
			Corpses,
			// Token: 0x0400C8F7 RID: 51447
			HermeticCrate
		}
	}
}
