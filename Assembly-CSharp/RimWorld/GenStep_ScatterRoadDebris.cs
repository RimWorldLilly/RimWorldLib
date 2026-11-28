using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C7 RID: 4807
	public class GenStep_ScatterRoadDebris : GenStep_Scatterer
	{
		// Token: 0x17001445 RID: 5189
		// (get) Token: 0x06007748 RID: 30536 RVA: 0x002A132A File Offset: 0x0029F52A
		public override int SeedPart
		{
			get
			{
				return 765346456;
			}
		}

		// Token: 0x17001446 RID: 5190
		// (get) Token: 0x06007749 RID: 30537 RVA: 0x002A1331 File Offset: 0x0029F531
		public IEnumerable<ThingDef> Debris
		{
			get
			{
				return new GenStep_ScatterRoadDebris.<get_Debris>d__11(-2);
			}
		}

		// Token: 0x0600774A RID: 30538 RVA: 0x002A133C File Offset: 0x0029F53C
		protected override float GetPlacementFactor(Map map)
		{
			float num = 1f;
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				num *= tileMutatorDef.junkDensityFactor;
			}
			return num;
		}

		// Token: 0x0600774B RID: 30539 RVA: 0x002A1398 File Offset: 0x0029F598
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter road debris"))
			{
				return;
			}
			this.allowInWaterBiome = false;
			this.mapHasRoads = this.HasAncientRoads(map);
			this.count = (this.mapHasRoads ? GenStep_ScatterRoadDebris.VehicleRangeRoadMap : GenStep_ScatterRoadDebris.VehicleRangeNonRoadMap).RandomInRange;
			this.thingToPlace = this.Debris.RandomElement<ThingDef>();
			base.Generate(map, parms);
		}

		// Token: 0x0600774C RID: 30540 RVA: 0x002A1404 File Offset: 0x0029F604
		private bool HasAncientRoads(Map map)
		{
			SurfaceTile surfaceTile;
			if (map.TileInfo.Isnt(out surfaceTile) || surfaceTile.Roads == null)
			{
				return false;
			}
			for (int i = 0; i < surfaceTile.Roads.Count; i++)
			{
				if (surfaceTile.Roads[i].road == RoadDefOf.AncientAsphaltHighway || surfaceTile.Roads[i].road == RoadDefOf.AncientAsphaltRoad)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600774D RID: 30541 RVA: 0x002A1474 File Offset: 0x0029F674
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (this.mapHasRoads && !c.GetTerrain(map).IsRoad)
			{
				return false;
			}
			if (this.thingToPlace.rotatable)
			{
				int num = Rand.RangeInclusive(1, 4);
				for (int i = 0; i < 4; i++)
				{
					this.rotation = new Rot4((i + num) % 4);
					if (this.CanPlaceThingAt(c, this.rotation, map, this.thingToPlace))
					{
						return true;
					}
				}
				return false;
			}
			this.rotation = this.thingToPlace.defaultPlacingRot;
			return this.CanPlaceThingAt(c, this.rotation, map, this.thingToPlace);
		}

		// Token: 0x0600774E RID: 30542 RVA: 0x002A1515 File Offset: 0x0029F715
		private bool CanPlaceThingAt(IntVec3 c, Rot4 rot, Map map, ThingDef thingDef)
		{
			return ScatterDebrisUtility.CanPlaceThingAt(c, rot, map, thingDef);
		}

		// Token: 0x0600774F RID: 30543 RVA: 0x002A1524 File Offset: 0x0029F724
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(this.thingToPlace, null), loc, map, this.rotation, WipeMode.Vanish, false, false);
			ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_MachineBits, 0.75f, 1, int.MaxValue, null);
			ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_OilSmear, 0.25f, 0, int.MaxValue, null);
			ThingDef engineBlockDef = Rand.Bool ? ThingDefOf.AncientRustedEngineBlock : ThingDefOf.AncientLargeRustedEngineBlock;
			IntVec3 loc2;
			if (Rand.Chance(0.5f) && RCellFinder.TryFindRandomCellNearWith(loc, (IntVec3 c) => this.CanPlaceThingAt(c, Rot4.North, map, engineBlockDef), map, out loc2, 10, 2147483647))
			{
				ScatterDebrisUtility.ScatterFilthAroundThing(GenSpawn.Spawn(ThingMaker.MakeThing(engineBlockDef, null), loc2, map, Rot4.North, WipeMode.Vanish, false, false), map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
				ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_OilSmear, 0.15f, 0, int.MaxValue, null);
			}
			this.thingToPlace = this.Debris.RandomElement<ThingDef>();
		}

		// Token: 0x04004AD1 RID: 19153
		private const int EngineBlockMinDistance = 10;

		// Token: 0x04004AD2 RID: 19154
		private const float EngineBlockChance = 0.5f;

		// Token: 0x04004AD3 RID: 19155
		private const float OilSmearChance = 0.15f;

		// Token: 0x04004AD4 RID: 19156
		private static readonly IntRange VehicleRangeNonRoadMap = new IntRange(1, 2);

		// Token: 0x04004AD5 RID: 19157
		private static readonly IntRange VehicleRangeRoadMap = new IntRange(2, 3);

		// Token: 0x04004AD6 RID: 19158
		private ThingDef thingToPlace;

		// Token: 0x04004AD7 RID: 19159
		private Rot4 rotation;

		// Token: 0x04004AD8 RID: 19160
		private bool mapHasRoads;
	}
}
