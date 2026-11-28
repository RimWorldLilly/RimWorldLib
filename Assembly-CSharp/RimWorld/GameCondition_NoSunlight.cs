using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001149 RID: 4425
	public class GameCondition_NoSunlight : GameCondition
	{
		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x06006F1D RID: 28445 RVA: 0x0026E82B File Offset: 0x0026CA2B
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06006F1E RID: 28446 RVA: 0x0026E832 File Offset: 0x0026CA32
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x06006F1F RID: 28447 RVA: 0x0026E846 File Offset: 0x0026CA46
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0f, GameCondition_NoSunlight.EclipseSkyColors, 1f, 0f));
		}

		// Token: 0x04004710 RID: 18192
		public static readonly SkyColorSet EclipseSkyColors = new SkyColorSet(new Color(0.482f, 0.603f, 0.682f), Color.white, new Color(0.6f, 0.6f, 0.6f), 1f);
	}
}
