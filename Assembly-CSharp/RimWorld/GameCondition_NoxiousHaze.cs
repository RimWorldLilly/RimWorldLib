using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200114B RID: 4427
	public class GameCondition_NoxiousHaze : GameCondition
	{
		// Token: 0x1700130C RID: 4876
		// (get) Token: 0x06006F24 RID: 28452 RVA: 0x0026E8B7 File Offset: 0x0026CAB7
		public override int TransitionTicks
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x1700130D RID: 4877
		// (get) Token: 0x06006F25 RID: 28453 RVA: 0x0026E8C0 File Offset: 0x0026CAC0
		public override string Description
		{
			get
			{
				float num = WorldPollutionUtility.CalculateNearbyPollutionScore(base.SingleMap.Tile);
				float num2 = this.def.mtbOverNearbyPollutionCurve.Evaluate(num);
				return this.def.description.Formatted(num.ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute).Named("NEARBYPOLLUTION"), num2.Named("MTB"));
			}
		}

		// Token: 0x06006F26 RID: 28454 RVA: 0x0026E928 File Offset: 0x0026CB28
		public override void GameConditionTick()
		{
			List<Map> affectedMaps = base.AffectedMaps;
			foreach (SkyOverlay skyOverlay in this.overlays)
			{
				foreach (Map map in affectedMaps)
				{
					if (!base.HiddenByOtherCondition(map))
					{
						skyOverlay.TickOverlay(map, 1f);
					}
				}
			}
		}

		// Token: 0x06006F27 RID: 28455 RVA: 0x0026E9CC File Offset: 0x0026CBCC
		public override void GameConditionDraw(Map map)
		{
			if (!base.HiddenByOtherCondition(map))
			{
				for (int i = 0; i < this.overlays.Count; i++)
				{
					this.overlays[i].DrawOverlay(map);
				}
			}
		}

		// Token: 0x06006F28 RID: 28456 RVA: 0x0026EA0A File Offset: 0x0026CC0A
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.5f);
		}

		// Token: 0x06006F29 RID: 28457 RVA: 0x0026EA1E File Offset: 0x0026CC1E
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.45f, GameCondition_NoxiousHaze.NoxiousHazeColors, 1f, 1f));
		}

		// Token: 0x06006F2A RID: 28458 RVA: 0x0026EA3E File Offset: 0x0026CC3E
		public override float AnimalDensityFactor(Map map)
		{
			return base.HiddenByOtherCondition(map) > false;
		}

		// Token: 0x06006F2B RID: 28459 RVA: 0x0026EA4B File Offset: 0x0026CC4B
		public override float PlantDensityFactor(Map map)
		{
			return base.HiddenByOtherCondition(map) > false;
		}

		// Token: 0x06006F2C RID: 28460 RVA: 0x0026EA58 File Offset: 0x0026CC58
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return base.HiddenByOtherCondition(map);
		}

		// Token: 0x06006F2D RID: 28461 RVA: 0x0026EA61 File Offset: 0x0026CC61
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return this.overlays;
		}

		// Token: 0x04004711 RID: 18193
		private const float MaxSkyLerpFactor = 0.5f;

		// Token: 0x04004712 RID: 18194
		private const float SkyGlow = 0.45f;

		// Token: 0x04004713 RID: 18195
		private static readonly SkyColorSet NoxiousHazeColors = new SkyColorSet(new ColorInt(216, 255, 0).ToColor, new ColorInt(234, 200, 255).ToColor, new ColorInt(207, 248, 0).ToColor, 0.45f);

		// Token: 0x04004714 RID: 18196
		private List<SkyOverlay> overlays = new List<SkyOverlay>
		{
			new WeatherOverlay_NoxiousHaze()
		};

		// Token: 0x04004715 RID: 18197
		public const float PlantGrowthRateFactor = 0.5f;
	}
}
