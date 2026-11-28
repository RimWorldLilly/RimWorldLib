using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A3 RID: 675
	[StaticConstructorOnStartup]
	public class FishShadowComponent : MapComponent
	{
		// Token: 0x060014BE RID: 5310 RVA: 0x00077405 File Offset: 0x00075605
		public FishShadowComponent(Map map) : base(map)
		{
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0007741C File Offset: 0x0007561C
		public override void MapComponentTick()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			foreach (WaterBody waterBody in this.map.waterBodyTracker.Bodies)
			{
				if (waterBody.HasFish && waterBody.CellCount >= 12 && GenTicks.IsTickInterval(waterBody.GetHashCode(), Mathf.CeilToInt(450f / Mathf.Max(1f, (float)waterBody.CellCount / 550f))))
				{
					IntVec3? intVec = FishShadowComponent.FindFishFleckLocation(waterBody);
					if (intVec != null)
					{
						FishShadowComponent.SpawnFishFleck(this.map, intVec.Value.ToVector3ShiftedWithAltitude(AltitudeLayer.MoteLow));
					}
				}
			}
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x000774E8 File Offset: 0x000756E8
		private static IntVec3? FindFishFleckLocation(WaterBody body)
		{
			for (int i = 0; i < 5; i++)
			{
				IntVec3 intVec = body.cells.RandomElement<IntVec3>();
				List<IntVec3> list = GenAdjFast.AdjacentCells8Way(intVec);
				bool flag = true;
				foreach (IntVec3 intVec2 in list)
				{
					if (!body.cells.Contains(intVec2) || !intVec2.GetTerrain(body.map).IsWater)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return new IntVec3?(intVec);
				}
			}
			return null;
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0007758C File Offset: 0x0007578C
		private static void SpawnFishFleck(Map map, Vector3 loc)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			bool @bool = Rand.Bool;
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, @bool ? FleckDefOf.FishShadowReverse : FleckDefOf.FishShadow, 3f * Rand.Range(0.4f, 1.1f));
			dataStatic.rotation = Rand.Range(0f, 180f);
			dataStatic.rotationRate = (@bool ? -1f : 1f) * 75f * Rand.Range(0.75f, 1.25f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x04001245 RID: 4677
		public List<IntVec3> fishWaterCells = new List<IntVec3>();

		// Token: 0x04001246 RID: 4678
		private const int SpawnFishFleckIntervalTicks = 450;

		// Token: 0x04001247 RID: 4679
		private const int CellsPerFish = 550;

		// Token: 0x04001248 RID: 4680
		private const int SpawnFishFleckMaxAttempts = 5;

		// Token: 0x04001249 RID: 4681
		private const float FishFleckRotationSpeed = 75f;

		// Token: 0x0400124A RID: 4682
		private const int MinCellCount = 12;
	}
}
