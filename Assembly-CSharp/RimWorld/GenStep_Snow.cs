using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012CC RID: 4812
	public class GenStep_Snow : GenStep
	{
		// Token: 0x1700144B RID: 5195
		// (get) Token: 0x06007768 RID: 30568 RVA: 0x002A1DB9 File Offset: 0x0029FFB9
		public override int SeedPart
		{
			get
			{
				return 306693816;
			}
		}

		// Token: 0x06007769 RID: 30569 RVA: 0x002A1DC0 File Offset: 0x0029FFC0
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = 0;
			for (int i = (int)(GenLocalDate.Twelfth(map) - Twelfth.Third); i <= (int)GenLocalDate.Twelfth(map); i++)
			{
				int num2 = i;
				if (num2 < 0)
				{
					num2 += 12;
				}
				Twelfth twelfth = (Twelfth)num2;
				if (GenTemperature.AverageTemperatureAtTileForTwelfth(map.Tile, twelfth) < 0f)
				{
					num++;
				}
			}
			float num3 = 0f;
			switch (num)
			{
			case 0:
				return;
			case 1:
				num3 = 0.3f;
				break;
			case 2:
				num3 = 0.7f;
				break;
			case 3:
				num3 = 1f;
				break;
			}
			float seasonalTemp = map.mapTemperature.SeasonalTemp;
			if (seasonalTemp > 0f)
			{
				num3 *= 0.4f;
			}
			foreach (IntVec3 c in map.AllCells)
			{
				if (ModsConfig.OdysseyActive && seasonalTemp < 0f && FreezeManager.CanEverFreeze(c, map))
				{
					map.terrainGrid.SetTempTerrain(c, TerrainDefOf.ThinIce);
				}
				if ((double)num3 > 0.4 && !c.Roofed(map))
				{
					map.steadyEnvironmentEffects.AddFallenSnowAt(c, num3);
				}
			}
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (map.terrainGrid.TerrainAt(intVec).meltSnowRadius > 0f)
				{
					WeatherBuildupUtility.AddSnowRadial(intVec, map, map.terrainGrid.TerrainAt(intVec).meltSnowRadius, -1f);
				}
			}
		}
	}
}
