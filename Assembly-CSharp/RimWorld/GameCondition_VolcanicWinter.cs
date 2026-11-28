using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001155 RID: 4437
	public class GameCondition_VolcanicWinter : GameCondition
	{
		// Token: 0x17001319 RID: 4889
		// (get) Token: 0x06006F76 RID: 28534 RVA: 0x0026FC2D File Offset: 0x0026DE2D
		public override int TransitionTicks
		{
			get
			{
				return 50000;
			}
		}

		// Token: 0x06006F77 RID: 28535 RVA: 0x0026FC34 File Offset: 0x0026DE34
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.3f);
		}

		// Token: 0x06006F78 RID: 28536 RVA: 0x0026FC48 File Offset: 0x0026DE48
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.55f, this.VolcanicWinterColors, 1f, 1f));
		}

		// Token: 0x06006F79 RID: 28537 RVA: 0x0026FC69 File Offset: 0x0026DE69
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, -7f);
		}

		// Token: 0x06006F7A RID: 28538 RVA: 0x0026FC7D File Offset: 0x0026DE7D
		public override float AnimalDensityFactor(Map map)
		{
			return 1f - GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.5f);
		}

		// Token: 0x06006F7B RID: 28539 RVA: 0x0026FC97 File Offset: 0x0026DE97
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		// Token: 0x04004731 RID: 18225
		private const float MaxTempOffset = -7f;

		// Token: 0x04004732 RID: 18226
		private const float AnimalDensityImpact = 0.5f;

		// Token: 0x04004733 RID: 18227
		private const float SkyGlow = 0.55f;

		// Token: 0x04004734 RID: 18228
		private const float MaxSkyLerpFactor = 0.3f;

		// Token: 0x04004735 RID: 18229
		private readonly SkyColorSet VolcanicWinterColors = new SkyColorSet(new ColorInt(0, 0, 0).ToColor, Color.white, new Color(0.6f, 0.6f, 0.6f), 0.65f);
	}
}
