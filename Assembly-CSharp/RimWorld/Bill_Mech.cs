using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001AD7 RID: 6871
	public abstract class Bill_Mech : Bill_Autonomous
	{
		// Token: 0x17001F8B RID: 8075
		// (get) Token: 0x0600AF2B RID: 44843 RVA: 0x003DAC21 File Offset: 0x003D8E21
		public Pawn BoundPawn
		{
			get
			{
				return this.boundPawn;
			}
		}

		// Token: 0x17001F8C RID: 8076
		// (get) Token: 0x0600AF2C RID: 44844 RVA: 0x003DAC29 File Offset: 0x003D8E29
		public int GestationCyclesCompleted
		{
			get
			{
				return this.gestationCycles;
			}
		}

		// Token: 0x17001F8D RID: 8077
		// (get) Token: 0x0600AF2D RID: 44845 RVA: 0x003DAC31 File Offset: 0x003D8E31
		public int StartedTick
		{
			get
			{
				return this.startedTick;
			}
		}

		// Token: 0x17001F8E RID: 8078
		// (get) Token: 0x0600AF2E RID: 44846 RVA: 0x003DAC39 File Offset: 0x003D8E39
		public float WorkSpeedMultiplier
		{
			get
			{
				if (this.recipe.workSpeedStat != null)
				{
					return this.boundPawn.GetStatValue(this.recipe.workSpeedStat, true, -1);
				}
				return 1f;
			}
		}

		// Token: 0x17001F8F RID: 8079
		// (get) Token: 0x0600AF2F RID: 44847 RVA: 0x003DAC66 File Offset: 0x003D8E66
		public Building_MechGestator Gestator
		{
			get
			{
				return (Building_MechGestator)this.billStack.billGiver;
			}
		}

		// Token: 0x17001F90 RID: 8080
		// (get) Token: 0x0600AF30 RID: 44848
		public abstract float BandwidthCost { get; }

		// Token: 0x17001F91 RID: 8081
		// (get) Token: 0x0600AF31 RID: 44849 RVA: 0x003DAC78 File Offset: 0x003D8E78
		protected override string StatusString
		{
			get
			{
				switch (base.State)
				{
				case FormingState.Gathering:
				case FormingState.Preparing:
					if (this.BoundPawn != null)
					{
						return "Worker".Translate() + ": " + this.BoundPawn.LabelShortCap;
					}
					break;
				case FormingState.Forming:
					return "Gestating".Translate();
				case FormingState.Formed:
					if (this.BoundPawn != null)
					{
						return "WaitingFor".Translate() + ": " + this.BoundPawn.LabelShortCap;
					}
					break;
				}
				return null;
			}
		}

		// Token: 0x17001F92 RID: 8082
		// (get) Token: 0x0600AF32 RID: 44850 RVA: 0x003DAD19 File Offset: 0x003D8F19
		protected override float StatusLineMinHeight
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x17001F93 RID: 8083
		// (get) Token: 0x0600AF33 RID: 44851 RVA: 0x003DAD20 File Offset: 0x003D8F20
		protected override Color BaseColor
		{
			get
			{
				if (this.suspended)
				{
					return base.BaseColor;
				}
				return Color.white;
			}
		}

		// Token: 0x0600AF34 RID: 44852 RVA: 0x003DAD36 File Offset: 0x003D8F36
		public Bill_Mech()
		{
		}

		// Token: 0x0600AF35 RID: 44853 RVA: 0x003DAD3E File Offset: 0x003D8F3E
		public Bill_Mech(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x0600AF36 RID: 44854 RVA: 0x003DAD48 File Offset: 0x003D8F48
		protected override Window GetBillDialog()
		{
			return new Dialog_MechBillConfig(this, ((Thing)this.billStack.billGiver).Position);
		}

		// Token: 0x0600AF37 RID: 44855 RVA: 0x003DAD68 File Offset: 0x003D8F68
		public override bool ShouldDoNow()
		{
			Pawn pawn = this.BoundPawn;
			if (((pawn != null) ? pawn.mechanitor : null) != null && !this.BoundPawn.mechanitor.HasBandwidthForBill(this))
			{
				JobFailReason.Is("NotEnoughBandwidth".Translate(), null);
				return false;
			}
			return base.ShouldDoNow();
		}

		// Token: 0x0600AF38 RID: 44856 RVA: 0x003DADBC File Offset: 0x003D8FBC
		public override bool PawnAllowedToStartAnew(Pawn p)
		{
			if (!ModLister.CheckBiotech("Mech bill"))
			{
				return false;
			}
			if (!base.PawnAllowedToStartAnew(p))
			{
				return false;
			}
			if (this.BoundPawn != null && this.BoundPawn != p)
			{
				JobFailReason.Is("AlreadyAssigned".Translate() + " (" + this.BoundPawn.LabelShort + ")", null);
				return false;
			}
			if (!p.mechanitor.HasBandwidthForBill(this))
			{
				JobFailReason.Is("NotEnoughBandwidth".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x0600AF39 RID: 44857 RVA: 0x003DAE54 File Offset: 0x003D9054
		public override void Notify_DoBillStarted(Pawn billDoer)
		{
			base.Notify_DoBillStarted(billDoer);
			if (this.boundPawn != billDoer)
			{
				this.boundPawn = billDoer;
			}
		}

		// Token: 0x0600AF3A RID: 44858 RVA: 0x003DAE6D File Offset: 0x003D906D
		public override void Reset()
		{
			base.Reset();
			this.gestationCycles = 0;
			this.boundPawn = null;
		}

		// Token: 0x0600AF3B RID: 44859 RVA: 0x003DAE83 File Offset: 0x003D9083
		public void ForceCompleteAllCycles()
		{
			this.gestationCycles = this.recipe.gestationCycles;
			this.formingTicks = 0f;
		}

		// Token: 0x0600AF3C RID: 44860 RVA: 0x003DAEA4 File Offset: 0x003D90A4
		public override void BillTick()
		{
			if (this.suspended || this.state != FormingState.Forming)
			{
				return;
			}
			this.formingTicks -= 1f * this.WorkSpeedMultiplier;
			if (this.formingTicks <= 0f)
			{
				this.gestationCycles++;
				if (this.gestationCycles >= this.recipe.gestationCycles)
				{
					this.state = FormingState.Formed;
					this.Gestator.Notify_FormingCompleted();
					return;
				}
				this.formingTicks = (float)this.recipe.formingTicks;
				this.state = FormingState.Preparing;
			}
		}

		// Token: 0x0600AF3D RID: 44861 RVA: 0x003DAF38 File Offset: 0x003D9138
		public override void AppendInspectionData(StringBuilder sb)
		{
			if (base.State == FormingState.Forming || base.State == FormingState.Preparing)
			{
				sb.AppendLine("CurrentGestationCycle".Translate() + ": " + ((int)(this.formingTicks * (1f / this.WorkSpeedMultiplier))).ToStringTicksToPeriod(true, false, true, true, false));
				sb.AppendLine("RemainingGestationCycles".Translate() + ": " + (this.recipe.gestationCycles - this.GestationCyclesCompleted).ToString() + " (" + "OfLower".Translate() + " " + this.recipe.gestationCycles.ToString() + ")");
			}
		}

		// Token: 0x0600AF3E RID: 44862 RVA: 0x003DB019 File Offset: 0x003D9219
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.boundPawn, "boundPawn", false);
			Scribe_Values.Look<int>(ref this.gestationCycles, "gestationCycles", 0, false);
		}

		// Token: 0x0400653F RID: 25919
		private const float StatusStringLineHeight = 20f;

		// Token: 0x04006540 RID: 25920
		private Pawn boundPawn;

		// Token: 0x04006541 RID: 25921
		private int gestationCycles;
	}
}
