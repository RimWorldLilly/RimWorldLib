using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200190D RID: 6413
	public class CompMilkable : CompHasGatherableBodyResource
	{
		// Token: 0x17001BB3 RID: 7091
		// (get) Token: 0x0600A0C1 RID: 41153 RVA: 0x00397CD3 File Offset: 0x00395ED3
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.milkIntervalDays;
			}
		}

		// Token: 0x17001BB4 RID: 7092
		// (get) Token: 0x0600A0C2 RID: 41154 RVA: 0x00397CE0 File Offset: 0x00395EE0
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.milkAmount;
			}
		}

		// Token: 0x17001BB5 RID: 7093
		// (get) Token: 0x0600A0C3 RID: 41155 RVA: 0x00397CED File Offset: 0x00395EED
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.milkDef;
			}
		}

		// Token: 0x17001BB6 RID: 7094
		// (get) Token: 0x0600A0C4 RID: 41156 RVA: 0x00397CFA File Offset: 0x00395EFA
		protected override string SaveKey
		{
			get
			{
				return "milkFullness";
			}
		}

		// Token: 0x17001BB7 RID: 7095
		// (get) Token: 0x0600A0C5 RID: 41157 RVA: 0x00397D01 File Offset: 0x00395F01
		public CompProperties_Milkable Props
		{
			get
			{
				return (CompProperties_Milkable)this.props;
			}
		}

		// Token: 0x17001BB8 RID: 7096
		// (get) Token: 0x0600A0C6 RID: 41158 RVA: 0x00397D10 File Offset: 0x00395F10
		protected override bool Active
		{
			get
			{
				if (!base.Active)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				return (!this.Props.milkFemaleOnly || pawn == null || pawn.gender == Gender.Female) && (pawn == null || pawn.ageTracker.CurLifeStage.milkable) && (!ModsConfig.AnomalyActive || !pawn.IsShambler);
			}
		}

		// Token: 0x0600A0C7 RID: 41159 RVA: 0x00397D77 File Offset: 0x00395F77
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "MilkFullness".Translate() + ": " + base.Fullness.ToStringPercent();
		}
	}
}
