using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B2 RID: 4786
	public class GenStep_OrbitalSatellite : GenStep
	{
		// Token: 0x1700142A RID: 5162
		// (get) Token: 0x060076B1 RID: 30385 RVA: 0x0029D15E File Offset: 0x0029B35E
		protected Faction Faction
		{
			get
			{
				if (this.factionDef != null)
				{
					return Find.FactionManager.FirstFactionOfDef(this.factionDef);
				}
				return null;
			}
		}

		// Token: 0x1700142B RID: 5163
		// (get) Token: 0x060076B2 RID: 30386 RVA: 0x0029D17A File Offset: 0x0029B37A
		protected virtual float SpawnTemp
		{
			get
			{
				return this.temperature.GetValueOrDefault(-75f);
			}
		}

		// Token: 0x1700142C RID: 5164
		// (get) Token: 0x060076B3 RID: 30387 RVA: 0x0029D18C File Offset: 0x0029B38C
		public override int SeedPart
		{
			get
			{
				return 749387154;
			}
		}

		// Token: 0x060076B4 RID: 30388 RVA: 0x0029D194 File Offset: 0x0029B394
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Orbital Satellite"))
			{
				return;
			}
			CellRect cellRect = this.GeneratePlatform(map, parms);
			SpaceGenUtility.GenerateRing(map, cellRect, TerrainDefOf.OrbitalPlatform, this.bridgeDistance, 8, this.ringWidth, this.ringExtraOffset, this.ringExtraChance);
			this.GenerateSidePlatforms(cellRect, map, parms);
			map.FogOfWarColor = new Color?(ColorLibrary.SpaceHumanFog);
		}

		// Token: 0x060076B5 RID: 30389 RVA: 0x0029D1F8 File Offset: 0x0029B3F8
		private CellRect GeneratePlatform(Map map, GenStepParams parms)
		{
			int randomInRange = GenStep_OrbitalSatellite.CenterSizeRange.RandomInRange;
			IntVec2 size = new IntVec2(randomInRange, randomInRange);
			IntVec3 center = map.Center;
			Rot4 random = Rot4.Random;
			CellRect cellRect = center.RectAbout(size, random).ClipInsideMap(map);
			CellRect var = cellRect.ContractedBy(5);
			GenStep_OrbitalSatellite.SpawnPlatformTerrain(cellRect, map, 2);
			StructureGenParams parms2 = new StructureGenParams
			{
				size = var.Size
			};
			LayoutWorker worker = this.layoutDef.Worker;
			LayoutStructureSketch layoutStructureSketch = worker.GenerateStructureSketch(parms2);
			map.layoutStructureSketches.Add(layoutStructureSketch);
			LayoutStructureSketch layoutStructureSketch2 = layoutStructureSketch;
			IntVec3 min = var.Min;
			Faction faction = this.Faction;
			worker.Spawn(layoutStructureSketch2, map, min, null, null, true, false, faction);
			MapGenerator.SetVar<CellRect>("SpawnRect", var);
			return cellRect;
		}

		// Token: 0x060076B6 RID: 30390 RVA: 0x0029D2B6 File Offset: 0x0029B4B6
		public override void PostMapInitialized(Map map, GenStepParams parms)
		{
			MapGenUtility.SetMapRoomTemperature(map, this.layoutDef, this.SpawnTemp);
			if (this.spawnSentryDrones)
			{
				BaseGenUtility.ScatterSentryDronesInMap(GenStep_OrbitalSatellite.SentryCountFromPointsCurve, map, this.Faction, parms);
			}
		}

		// Token: 0x060076B7 RID: 30391 RVA: 0x0029D2E4 File Offset: 0x0029B4E4
		private void GenerateSidePlatforms(CellRect platformRect, Map map, GenStepParams parms)
		{
			int num = 0;
			int amountToPlace = Rand.Range(2, 4);
			int num2 = Rand.Range(0, 3);
			for (int i = 0; i < 4; i++)
			{
				Rot4 rot = new Rot4((i + num2) % 4);
				CellRect cellRect = new CellRect(0, 0, 23, 23);
				IntVec2 platformPosition = this.GetPlatformPosition(platformRect, cellRect, rot);
				cellRect = cellRect.MovedBy(platformPosition).ClipInsideMap(map);
				map.landingBlockers.Add(cellRect);
				GenStep_OrbitalSatellite.SpawnPlatformTerrain(cellRect, map, 1);
				if (Rand.DynamicChance(num, amountToPlace, 3 - i))
				{
					num++;
					GenStep_OrbitalSatellite.SpawnWalkway(platformRect, cellRect, rot, map);
				}
				this.SpawnPrefab(map, cellRect);
			}
		}

		// Token: 0x060076B8 RID: 30392 RVA: 0x0029D384 File Offset: 0x0029B584
		private void SpawnPrefab(Map map, CellRect rect)
		{
			if (this.prefabs.NullOrEmpty<GenStep_OrbitalSatellite.LayoutPrefabParms>())
			{
				return;
			}
			GenStep_OrbitalSatellite.LayoutPrefabParms layoutPrefabParms = this.prefabs.RandomElementByWeight((GenStep_OrbitalSatellite.LayoutPrefabParms p) => p.weight);
			Rot4 random = Rot4.Random;
			IntVec3 centerCell = rect.CenterCell;
			if (rect.Width % 2 == 0)
			{
				centerCell.x--;
			}
			if (rect.Height % 2 == 0)
			{
				centerCell.z--;
			}
			for (int i = 0; i < 4; i++)
			{
				random.Rotate(RotationDirection.Clockwise);
				if (PrefabUtility.CanSpawnPrefab(layoutPrefabParms.def, map, centerCell, random, false, 0))
				{
					PrefabUtility.SpawnPrefab(layoutPrefabParms.def, map, centerCell, random, this.Faction, null, null, new Action<Thing>(GenStep_OrbitalSatellite.<SpawnPrefab>g__OnSpawned|28_1), false);
					return;
				}
			}
		}

		// Token: 0x060076B9 RID: 30393 RVA: 0x0029D450 File Offset: 0x0029B650
		private static void SpawnWalkway(CellRect platform, CellRect side, Rot4 sideRot, Map map)
		{
			Rot4 opposite = sideRot.Opposite;
			int randomInRange = GenStep_OrbitalSatellite.PlatformConnectorHeightRange.RandomInRange;
			IntVec3 centerCellOnEdge = side.GetCenterCellOnEdge(opposite, -randomInRange);
			IntVec3 centerCellOnEdge2 = platform.GetCenterCellOnEdge(sideRot, randomInRange);
			CellRect cellRect = CellRect.FromLimits(centerCellOnEdge, centerCellOnEdge2).ClipInsideMap(map);
			CellRect cellRect2 = cellRect.ContractedBy((sideRot.IsVertical > false) ? 1 : 0, (sideRot.IsHorizontal > false) ? 1 : 0);
			cellRect2 = cellRect2.ExpandedBy(sideRot.IsHorizontal ? 2 : 0, sideRot.IsVertical ? 2 : 0);
			foreach (IntVec3 c in cellRect)
			{
				if (c.GetTerrain(map) == TerrainDefOf.Space)
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.OrbitalPlatform);
				}
			}
			foreach (IntVec3 c2 in cellRect2)
			{
				map.terrainGrid.SetTerrain(c2, TerrainDefOf.AncientTile);
			}
		}

		// Token: 0x060076BA RID: 30394 RVA: 0x0029D584 File Offset: 0x0029B784
		private IntVec2 GetPlatformPosition(CellRect centerRect, CellRect sideRect, Rot4 rot)
		{
			IntVec3 centerCell = centerRect.CenterCell;
			if (rot == Rot4.North)
			{
				return new IntVec2(centerCell.x - sideRect.Height / 2, centerRect.maxZ + this.bridgeDistance);
			}
			if (rot == Rot4.East)
			{
				return new IntVec2(centerRect.maxX + this.bridgeDistance, centerCell.z - sideRect.Width / 2);
			}
			if (rot == Rot4.South)
			{
				return new IntVec2(centerCell.x - sideRect.Height / 2, centerRect.minZ - this.bridgeDistance - sideRect.Height);
			}
			return new IntVec2(centerRect.minX - this.bridgeDistance - sideRect.Width, centerCell.z - sideRect.Height / 2);
		}

		// Token: 0x060076BB RID: 30395 RVA: 0x0029D65C File Offset: 0x0029B85C
		private static void SpawnPlatformTerrain(CellRect rect, Map map, int border = 1)
		{
			CellRect cellRect = rect.ContractedBy(border);
			foreach (IntVec3 c in rect)
			{
				map.terrainGrid.SetTerrain(c, TerrainDefOf.OrbitalPlatform);
				if (cellRect.Contains(c))
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
				}
			}
		}

		// Token: 0x060076BE RID: 30398 RVA: 0x0029D7B4 File Offset: 0x0029B9B4
		[CompilerGenerated]
		internal static void <SpawnPrefab>g__OnSpawned|28_1(Thing thing)
		{
			CompPowerBattery compPowerBattery;
			if (thing.TryGetComp(out compPowerBattery))
			{
				compPowerBattery.SetStoredEnergyPct(1f);
			}
			CompPowerTrader compPowerTrader;
			if (thing.TryGetComp(out compPowerTrader))
			{
				compPowerTrader.PowerOn = true;
			}
		}

		// Token: 0x04004A6C RID: 19052
		private LayoutDef layoutDef;

		// Token: 0x04004A6D RID: 19053
		private FactionDef factionDef;

		// Token: 0x04004A6E RID: 19054
		private List<GenStep_OrbitalSatellite.LayoutPrefabParms> prefabs = new List<GenStep_OrbitalSatellite.LayoutPrefabParms>();

		// Token: 0x04004A6F RID: 19055
		private float? temperature;

		// Token: 0x04004A70 RID: 19056
		private bool spawnSentryDrones;

		// Token: 0x04004A71 RID: 19057
		private int bridgeDistance = 34;

		// Token: 0x04004A72 RID: 19058
		private float ringWidth = 4.9f;

		// Token: 0x04004A73 RID: 19059
		private float ringExtraOffset = 0.5f;

		// Token: 0x04004A74 RID: 19060
		private float ringExtraChance;

		// Token: 0x04004A75 RID: 19061
		private const int ShipPadding = 5;

		// Token: 0x04004A76 RID: 19062
		private const int CenterPadding = 5;

		// Token: 0x04004A77 RID: 19063
		private static readonly IntRange CenterSizeRange = new IntRange(48, 65);

		// Token: 0x04004A78 RID: 19064
		private const int PlatformSize = 23;

		// Token: 0x04004A79 RID: 19065
		private const int RingSize = 8;

		// Token: 0x04004A7A RID: 19066
		private static readonly IntRange PlatformConnectorHeightRange = new IntRange(2, 2);

		// Token: 0x04004A7B RID: 19067
		private static readonly CellRect MinLandingArea = new CellRect(-10, -10, 20, 20);

		// Token: 0x04004A7C RID: 19068
		private static readonly SimpleCurve SentryCountFromPointsCurve = new SimpleCurve(new CurvePoint[]
		{
			new CurvePoint(0f, 0f),
			new CurvePoint(100f, 2f),
			new CurvePoint(1000f, 8f),
			new CurvePoint(5000f, 20f)
		});

		// Token: 0x020031AA RID: 12714
		public class LayoutPrefabParms
		{
			// Token: 0x06012979 RID: 76153 RVA: 0x0063A7B2 File Offset: 0x006389B2
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				XmlHelper.ParseElements(this, xmlRoot, "def", "weight");
			}

			// Token: 0x0400C89B RID: 51355
			public PrefabDef def;

			// Token: 0x0400C89C RID: 51356
			public float weight = 1f;
		}
	}
}
