using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001147 RID: 4423
	public class GameCondition_HeatVentHeat : GameCondition
	{
		// Token: 0x17001307 RID: 4871
		// (get) Token: 0x06006F0A RID: 28426 RVA: 0x0026E3A3 File Offset: 0x0026C5A3
		private int TargetTempOffset
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17001308 RID: 4872
		// (get) Token: 0x06006F0B RID: 28427 RVA: 0x0026E3A7 File Offset: 0x0026C5A7
		public override int TransitionTicks
		{
			get
			{
				return 2500;
			}
		}

		// Token: 0x06006F0C RID: 28428 RVA: 0x0026E3AE File Offset: 0x0026C5AE
		public override void PostMake()
		{
			base.PostMake();
			this.tempOffset = 0f;
		}

		// Token: 0x06006F0D RID: 28429 RVA: 0x0026E3C1 File Offset: 0x0026C5C1
		public override float TemperatureOffset()
		{
			return this.tempOffset;
		}

		// Token: 0x06006F0E RID: 28430 RVA: 0x0026E3C9 File Offset: 0x0026C5C9
		public override void GameConditionTick()
		{
			this.tempOffset += Mathf.Sign((float)this.TargetTempOffset - this.tempOffset) * 0.001f;
		}

		// Token: 0x06006F0F RID: 28431 RVA: 0x0026E3F1 File Offset: 0x0026C5F1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.tempOffset, "tempOffset", 0f, false);
		}

		// Token: 0x04004706 RID: 18182
		private const float TempChangePerTick = 0.001f;

		// Token: 0x04004707 RID: 18183
		private const float DefaultTargetTemp = 20f;

		// Token: 0x04004708 RID: 18184
		private float tempOffset;
	}
}
