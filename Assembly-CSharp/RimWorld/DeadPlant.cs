using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B36 RID: 6966
	public class DeadPlant : Plant
	{
		// Token: 0x1700203F RID: 8255
		// (get) Token: 0x0600B2B6 RID: 45750 RVA: 0x003EECC1 File Offset: 0x003ECEC1
		protected override bool Resting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002040 RID: 8256
		// (get) Token: 0x0600B2B7 RID: 45751 RVA: 0x003EECC4 File Offset: 0x003ECEC4
		public override float GrowthRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17002041 RID: 8257
		// (get) Token: 0x0600B2B8 RID: 45752 RVA: 0x003EECCB File Offset: 0x003ECECB
		public override float CurrentDyingDamagePerTick
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17002042 RID: 8258
		// (get) Token: 0x0600B2B9 RID: 45753 RVA: 0x003EECD2 File Offset: 0x003ECED2
		public override string LabelMouseover
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17002043 RID: 8259
		// (get) Token: 0x0600B2BA RID: 45754 RVA: 0x003EECDA File Offset: 0x003ECEDA
		public override PlantLifeStage LifeStage
		{
			get
			{
				return PlantLifeStage.Mature;
			}
		}

		// Token: 0x0600B2BB RID: 45755 RVA: 0x003EECDD File Offset: 0x003ECEDD
		public override string GetInspectStringLowPriority()
		{
			return null;
		}

		// Token: 0x0600B2BC RID: 45756 RVA: 0x003EECE0 File Offset: 0x003ECEE0
		public override string GetInspectString()
		{
			float statValue = this.GetStatValue(StatDefOf.DeteriorationRate, true, -1);
			if (statValue > 0f)
			{
				int numTicks = Mathf.RoundToInt((float)this.HitPoints / statValue * 60000f);
				return "DeteriorateInDuration".Translate(numTicks.ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
			}
			return "";
		}

		// Token: 0x0600B2BD RID: 45757 RVA: 0x003EED40 File Offset: 0x003ECF40
		public override void CropBlighted()
		{
		}
	}
}
