using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200196D RID: 6509
	public class CompShearable : CompHasGatherableBodyResource
	{
		// Token: 0x17001C49 RID: 7241
		// (get) Token: 0x0600A35A RID: 41818 RVA: 0x003A52D8 File Offset: 0x003A34D8
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.shearIntervalDays;
			}
		}

		// Token: 0x17001C4A RID: 7242
		// (get) Token: 0x0600A35B RID: 41819 RVA: 0x003A52E5 File Offset: 0x003A34E5
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.woolAmount;
			}
		}

		// Token: 0x17001C4B RID: 7243
		// (get) Token: 0x0600A35C RID: 41820 RVA: 0x003A52F2 File Offset: 0x003A34F2
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.woolDef;
			}
		}

		// Token: 0x17001C4C RID: 7244
		// (get) Token: 0x0600A35D RID: 41821 RVA: 0x003A52FF File Offset: 0x003A34FF
		protected override string SaveKey
		{
			get
			{
				return "woolGrowth";
			}
		}

		// Token: 0x17001C4D RID: 7245
		// (get) Token: 0x0600A35E RID: 41822 RVA: 0x003A5306 File Offset: 0x003A3506
		public CompProperties_Shearable Props
		{
			get
			{
				return (CompProperties_Shearable)this.props;
			}
		}

		// Token: 0x17001C4E RID: 7246
		// (get) Token: 0x0600A35F RID: 41823 RVA: 0x003A5314 File Offset: 0x003A3514
		protected override bool Active
		{
			get
			{
				if (!base.Active)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				return (pawn == null || pawn.ageTracker.CurLifeStage.shearable) && (!ModsConfig.AnomalyActive || !pawn.IsShambler);
			}
		}

		// Token: 0x0600A360 RID: 41824 RVA: 0x003A5360 File Offset: 0x003A3560
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "WoolGrowth".Translate() + ": " + base.Fullness.ToStringPercent();
		}
	}
}
