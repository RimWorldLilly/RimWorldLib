using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001152 RID: 4434
	public class GameCondition_UnnaturalHeat : GameCondition
	{
		// Token: 0x17001315 RID: 4885
		// (get) Token: 0x06006F5E RID: 28510 RVA: 0x0026F72C File Offset: 0x0026D92C
		private int TargetTempOffset
		{
			get
			{
				int num = 0;
				foreach (Building thing in base.SingleMap.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.AtmosphericHeater))
				{
					num += thing.TryGetComp<Comp_AtmosphericHeater>().TempOffset;
				}
				return num;
			}
		}

		// Token: 0x17001316 RID: 4886
		// (get) Token: 0x06006F5F RID: 28511 RVA: 0x0026F798 File Offset: 0x0026D998
		public override int TransitionTicks
		{
			get
			{
				return 2500;
			}
		}

		// Token: 0x06006F60 RID: 28512 RVA: 0x0026F79F File Offset: 0x0026D99F
		public override void PostMake()
		{
			base.PostMake();
			this.tempOffset = 0f;
		}

		// Token: 0x06006F61 RID: 28513 RVA: 0x0026F7B2 File Offset: 0x0026D9B2
		public override float TemperatureOffset()
		{
			if (!base.Permanent)
			{
				return Mathf.Lerp(0f, this.tempOffset, Mathf.Min(1f, (float)base.TicksLeft / (float)this.TransitionTicks));
			}
			return this.tempOffset;
		}

		// Token: 0x06006F62 RID: 28514 RVA: 0x0026F7EC File Offset: 0x0026D9EC
		public override void GameConditionTick()
		{
			this.tempOffset += Mathf.Sign((float)this.TargetTempOffset - this.tempOffset) * 0.001f;
			if (base.SingleMap.listerThings.ThingsOfDef(ThingDefOf.AtmosphericHeater).Count == 0)
			{
				base.Permanent = false;
			}
		}

		// Token: 0x06006F63 RID: 28515 RVA: 0x0026F842 File Offset: 0x0026DA42
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.tempOffset, "tempOffset", 0f, false);
			Scribe_Values.Look<float>(ref this.heaterTargetTemp, "heaterTargetTemp", 20f, false);
		}

		// Token: 0x04004725 RID: 18213
		private const float TempChangePerTick = 0.001f;

		// Token: 0x04004726 RID: 18214
		private const float DefaultTargetTemp = 20f;

		// Token: 0x04004727 RID: 18215
		private float tempOffset;

		// Token: 0x04004728 RID: 18216
		public float heaterTargetTemp = 20f;
	}
}
