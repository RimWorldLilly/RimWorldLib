using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000C2 RID: 194
	public class FreezeManager : IExposable
	{
		// Token: 0x0600077D RID: 1917 RVA: 0x0002BAF5 File Offset: 0x00029CF5
		public FreezeManager()
		{
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0002BB08 File Offset: 0x00029D08
		public FreezeManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0002BB22 File Offset: 0x00029D22
		public void ExposeData()
		{
			Scribe_References.Look<Map>(ref this.map, "map", false);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0002BB35 File Offset: 0x00029D35
		public void DoCellSteadyEffects(IntVec3 c)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.DoWaterFreezing(c);
			this.DoIceMelting(c);
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0002BB50 File Offset: 0x00029D50
		private void DoIceMelting(IntVec3 c)
		{
			float temperature = c.GetTemperature(this.map);
			Room room = c.GetRoom(this.map);
			if (temperature < 2f)
			{
				return;
			}
			if (!Rand.Chance(0.001f * ((temperature + 5f) / 5f)))
			{
				return;
			}
			this.tmpCellsToAffect.Clear();
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(c, 2.9f, true))
			{
				if (intVec.InBounds(this.map) && intVec.GetRoom(this.map) == room && this.CanEverMelt(intVec) && this.CanMelt(intVec))
				{
					this.tmpCellsToAffect.Add(intVec);
				}
			}
			foreach (IntVec3 c2 in this.tmpCellsToAffect)
			{
				this.map.terrainGrid.RemoveTempTerrain(c2, false, false);
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0002BC74 File Offset: 0x00029E74
		private void DoWaterFreezing(IntVec3 c)
		{
			float temperature = c.GetTemperature(this.map);
			Room room = c.GetRoom(this.map);
			if (temperature > -7f)
			{
				return;
			}
			if (!this.CanFreeze(c))
			{
				return;
			}
			float num = 0.003f * (temperature / -5f);
			WaterBody waterBody = c.GetWaterBody(this.map);
			num += (1f - num) * Mathf.Pow((float)waterBody.numCellsFrozen / (float)waterBody.numCellsCanFreeze, 10f);
			if (this.SurroundedByIce(c))
			{
				num = 1f;
			}
			if (!Rand.Chance(num))
			{
				return;
			}
			this.tmpCellsToAffect.Clear();
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(c, 2.9f, true))
			{
				if (intVec.InBounds(this.map) && intVec.GetRoom(this.map) == room && this.CanFreeze(intVec))
				{
					this.tmpCellsToAffect.Add(intVec);
				}
			}
			foreach (IntVec3 c2 in this.tmpCellsToAffect)
			{
				this.map.terrainGrid.SetTempTerrain(c2, TerrainDefOf.ThinIce);
			}
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0002BDE0 File Offset: 0x00029FE0
		public static bool CanEverFreeze(IntVec3 c, Map map)
		{
			return c.GetTerrain(map).canFreeze;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0002BDF3 File Offset: 0x00029FF3
		private bool CanFreeze(IntVec3 c)
		{
			return FreezeManager.CanEverFreeze(c, this.map) && !this.SurroundedByWater(c);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0002BE0F File Offset: 0x0002A00F
		private bool CanEverMelt(IntVec3 c)
		{
			return c.GetTerrain(this.map) == TerrainDefOf.ThinIce;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0002BE24 File Offset: 0x0002A024
		private bool CanMelt(IntVec3 c)
		{
			return this.CanEverMelt(c) && !this.SurroundedByIce(c);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0002BE3C File Offset: 0x0002A03C
		private bool SurroundedByIce(IntVec3 c)
		{
			bool result = true;
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				IntVec3 c2 = c + b;
				if (c2.InBounds(this.map) && c2.GetTerrain(this.map) != TerrainDefOf.ThinIce)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0002BE98 File Offset: 0x0002A098
		private bool SurroundedByWater(IntVec3 c)
		{
			bool result = true;
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				IntVec3 c2 = c + b;
				if (c2.InBounds(this.map) && !c2.GetTerrain(this.map).IsWater)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x04000347 RID: 839
		public const float WaterFreezeThreshold = -7f;

		// Token: 0x04000348 RID: 840
		private const float IceMeltThreshold = 2f;

		// Token: 0x04000349 RID: 841
		private const float BaseWaterFreezeChance = 0.003f;

		// Token: 0x0400034A RID: 842
		private const float BaseIceMeltChance = 0.001f;

		// Token: 0x0400034B RID: 843
		private const float TemperatureFactor = 5f;

		// Token: 0x0400034C RID: 844
		private const float FreezeRadius = 2.9f;

		// Token: 0x0400034D RID: 845
		private const int LakeFrozenPctExp = 10;

		// Token: 0x0400034E RID: 846
		private Map map;

		// Token: 0x0400034F RID: 847
		[Unsaved(false)]
		private List<IntVec3> tmpCellsToAffect = new List<IntVec3>();
	}
}
