using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001361 RID: 4961
	[StaticConstructorOnStartup]
	public class CompPowerPlantWater : CompPowerPlant
	{
		// Token: 0x170014F9 RID: 5369
		// (get) Token: 0x06007ADE RID: 31454 RVA: 0x002BBBEE File Offset: 0x002B9DEE
		protected override float DesiredPowerOutput
		{
			get
			{
				if (this.cacheDirty)
				{
					this.RebuildCache();
				}
				if (!this.waterUsable)
				{
					return 0f;
				}
				if (this.waterDoubleUsed)
				{
					return base.DesiredPowerOutput * 0.3f;
				}
				return base.DesiredPowerOutput;
			}
		}

		// Token: 0x06007ADF RID: 31455 RVA: 0x002BBC27 File Offset: 0x002B9E27
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.spinPosition = Rand.Range(0f, 15f);
			this.RebuildCache();
			this.ForceOthersToRebuildCache(this.parent.Map);
		}

		// Token: 0x06007AE0 RID: 31456 RVA: 0x002BBC5C File Offset: 0x002B9E5C
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			this.ForceOthersToRebuildCache(map);
		}

		// Token: 0x06007AE1 RID: 31457 RVA: 0x002BBC6D File Offset: 0x002B9E6D
		private void ClearCache()
		{
			this.cacheDirty = true;
		}

		// Token: 0x06007AE2 RID: 31458 RVA: 0x002BBC78 File Offset: 0x002B9E78
		private void RebuildCache()
		{
			this.waterUsable = true;
			foreach (IntVec3 c in this.WaterCells())
			{
				if (c.InBounds(this.parent.Map) && !c.GetAffordances(this.parent.Map).Contains(TerrainAffordanceDefOf.MovingFluid))
				{
					this.waterUsable = false;
					break;
				}
			}
			this.waterDoubleUsed = false;
			List<Building> list = this.parent.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator);
			foreach (IntVec3 c2 in this.WaterUseCells())
			{
				if (c2.InBounds(this.parent.Map))
				{
					foreach (Building building in list)
					{
						if (building != this.parent && building.GetComp<CompPowerPlantWater>().WaterUseRect().Contains(c2))
						{
							this.waterDoubleUsed = true;
							break;
						}
					}
				}
			}
			if (!this.waterUsable)
			{
				this.spinRate = 0f;
				return;
			}
			Vector3 vector = Vector3.zero;
			foreach (IntVec3 intVec in this.WaterCells())
			{
				vector += this.parent.Map.waterInfo.GetWaterMovement(intVec.ToVector3Shifted());
			}
			this.spinRate = Mathf.Sign(Vector3.Dot(vector, this.parent.Rotation.Rotated(RotationDirection.Clockwise).FacingCell.ToVector3()));
			this.spinRate *= Rand.RangeSeeded(0.9f, 1.1f, this.parent.thingIDNumber * 60509 + 33151);
			if (this.waterDoubleUsed)
			{
				this.spinRate *= 0.5f;
			}
			this.cacheDirty = false;
		}

		// Token: 0x06007AE3 RID: 31459 RVA: 0x002BBED0 File Offset: 0x002BA0D0
		private void ForceOthersToRebuildCache(Map map)
		{
			foreach (Building building in map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator))
			{
				building.GetComp<CompPowerPlantWater>().ClearCache();
			}
		}

		// Token: 0x06007AE4 RID: 31460 RVA: 0x002BBF30 File Offset: 0x002BA130
		public override void CompTick()
		{
			base.CompTick();
			if (base.PowerOutput > 0.01f)
			{
				this.spinPosition = (this.spinPosition + 0.006666667f * this.spinRate + 6.2831855f) % 6.2831855f;
			}
			if (this.parent.IsHashIntervalTick(1200))
			{
				this.RebuildCache();
			}
		}

		// Token: 0x06007AE5 RID: 31461 RVA: 0x002BBF8D File Offset: 0x002BA18D
		public IEnumerable<IntVec3> WaterCells()
		{
			return CompPowerPlantWater.WaterCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06007AE6 RID: 31462 RVA: 0x002BBFAA File Offset: 0x002BA1AA
		public static IEnumerable<IntVec3> WaterCells(IntVec3 loc, Rot4 rot)
		{
			CompPowerPlantWater.<WaterCells>d__20 <WaterCells>d__ = new CompPowerPlantWater.<WaterCells>d__20(-2);
			<WaterCells>d__.<>3__loc = loc;
			<WaterCells>d__.<>3__rot = rot;
			return <WaterCells>d__;
		}

		// Token: 0x06007AE7 RID: 31463 RVA: 0x002BBFC1 File Offset: 0x002BA1C1
		public CellRect WaterUseRect()
		{
			return CompPowerPlantWater.WaterUseRect(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06007AE8 RID: 31464 RVA: 0x002BBFE0 File Offset: 0x002BA1E0
		public static CellRect WaterUseRect(IntVec3 loc, Rot4 rot)
		{
			int width = rot.IsHorizontal ? 7 : 13;
			int height = rot.IsHorizontal ? 13 : 7;
			return CellRect.CenteredOn(loc + rot.FacingCell * 4, width, height);
		}

		// Token: 0x06007AE9 RID: 31465 RVA: 0x002BC025 File Offset: 0x002BA225
		public IEnumerable<IntVec3> WaterUseCells()
		{
			return CompPowerPlantWater.WaterUseCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06007AEA RID: 31466 RVA: 0x002BC042 File Offset: 0x002BA242
		public static IEnumerable<IntVec3> WaterUseCells(IntVec3 loc, Rot4 rot)
		{
			CompPowerPlantWater.<WaterUseCells>d__24 <WaterUseCells>d__ = new CompPowerPlantWater.<WaterUseCells>d__24(-2);
			<WaterUseCells>d__.<>3__loc = loc;
			<WaterUseCells>d__.<>3__rot = rot;
			return <WaterUseCells>d__;
		}

		// Token: 0x06007AEB RID: 31467 RVA: 0x002BC059 File Offset: 0x002BA259
		public IEnumerable<IntVec3> GroundCells()
		{
			return CompPowerPlantWater.GroundCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06007AEC RID: 31468 RVA: 0x002BC076 File Offset: 0x002BA276
		public static IEnumerable<IntVec3> GroundCells(IntVec3 loc, Rot4 rot)
		{
			CompPowerPlantWater.<GroundCells>d__26 <GroundCells>d__ = new CompPowerPlantWater.<GroundCells>d__26(-2);
			<GroundCells>d__.<>3__loc = loc;
			<GroundCells>d__.<>3__rot = rot;
			return <GroundCells>d__;
		}

		// Token: 0x06007AED RID: 31469 RVA: 0x002BC090 File Offset: 0x002BA290
		public override void PostDraw()
		{
			base.PostDraw();
			Vector3 a = this.parent.TrueCenter();
			a += this.parent.Rotation.FacingCell.ToVector3() * 2.36f;
			for (int i = 0; i < 9; i++)
			{
				float num = this.spinPosition + 6.2831855f * (float)i / 9f;
				float x = Mathf.Abs(4f * Mathf.Sin(num));
				bool flag = num % 6.2831855f < 3.1415927f;
				Vector2 vector = new Vector2(x, 1f);
				Vector3 s = new Vector3(vector.x, 1f, vector.y);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(a + Vector3.up * 0.03658537f * Mathf.Cos(num), this.parent.Rotation.AsQuat, s);
				Graphics.DrawMesh(flag ? MeshPool.plane10 : MeshPool.plane10Flip, matrix, CompPowerPlantWater.BladesMat, 0);
			}
		}

		// Token: 0x06007AEE RID: 31470 RVA: 0x002BC1B0 File Offset: 0x002BA3B0
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.waterUsable && this.waterDoubleUsed)
			{
				text += "\n" + "Watermill_WaterUsedTwice".Translate();
			}
			return text;
		}

		// Token: 0x04004D8B RID: 19851
		private float spinPosition;

		// Token: 0x04004D8C RID: 19852
		private bool cacheDirty = true;

		// Token: 0x04004D8D RID: 19853
		private bool waterUsable;

		// Token: 0x04004D8E RID: 19854
		private bool waterDoubleUsed;

		// Token: 0x04004D8F RID: 19855
		private float spinRate = 1f;

		// Token: 0x04004D90 RID: 19856
		private const float PowerFactorIfWaterDoubleUsed = 0.3f;

		// Token: 0x04004D91 RID: 19857
		private const float SpinRateFactor = 0.006666667f;

		// Token: 0x04004D92 RID: 19858
		private const float BladeOffset = 2.36f;

		// Token: 0x04004D93 RID: 19859
		private const int BladeCount = 9;

		// Token: 0x04004D94 RID: 19860
		private const int RebuildCacheIntervalTicks = 1200;

		// Token: 0x04004D95 RID: 19861
		public static readonly Material BladesMat = MaterialPool.MatFrom("Things/Building/Power/WatermillGenerator/WatermillGeneratorBlades");
	}
}
