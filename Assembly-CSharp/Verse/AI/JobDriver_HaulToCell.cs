using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000834 RID: 2100
	public class JobDriver_HaulToCell : JobDriver
	{
		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x060041BB RID: 16827 RVA: 0x001A05C4 File Offset: 0x0019E7C4
		public Thing ToHaul
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x060041BC RID: 16828 RVA: 0x001A05E5 File Offset: 0x0019E7E5
		protected virtual bool DropCarriedThingIfNotTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x001A05E8 File Offset: 0x0019E7E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.forbiddenInitially, "forbiddenInitially", false, false);
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x001A0604 File Offset: 0x0019E804
		public override string GetReport()
		{
			IntVec3 cell = this.job.targetB.Cell;
			Thing thing = null;
			if (this.pawn.CurJob == this.job && this.pawn.carryTracker.CarriedThing != null)
			{
				thing = this.pawn.carryTracker.CarriedThing;
			}
			else if (base.TargetThingA != null && base.TargetThingA.Spawned)
			{
				thing = base.TargetThingA;
			}
			if (thing == null)
			{
				return "ReportHaulingUnknown".Translate();
			}
			string text = null;
			SlotGroup slotGroup = cell.GetSlotGroup(base.Map);
			if (slotGroup != null)
			{
				text = slotGroup.parent.SlotYielderLabel();
			}
			if (text != null)
			{
				return "ReportHaulingTo".Translate(thing.Label, text.Named("DESTINATION"), thing.Named("THING"));
			}
			return "ReportHauling".Translate(thing.Label, thing);
		}

		// Token: 0x060041BF RID: 16831 RVA: 0x001A0700 File Offset: 0x0019E900
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x001A0755 File Offset: 0x0019E955
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.forbiddenInitially = (base.TargetThingA != null && base.TargetThingA.IsForbidden(this.pawn));
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x001A077F File Offset: 0x0019E97F
		protected virtual Toil BeforeDrop()
		{
			return Toils_General.Label();
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x001A0786 File Offset: 0x0019E986
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_HaulToCell.<MakeNewToils>d__13 <MakeNewToils>d__ = new JobDriver_HaulToCell.<MakeNewToils>d__13(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x001A0796 File Offset: 0x0019E996
		private Toil PossiblyDelay()
		{
			Toil toil = ToilMaker.MakeToil("PossiblyDelay");
			toil.atomicWithPrevious = true;
			toil.tickIntervalAction = delegate(int delta)
			{
				if (Find.TickManager.TicksGame >= this.startTick + 30)
				{
					base.ReadyForNextToil();
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x04002B35 RID: 11061
		private bool forbiddenInitially;

		// Token: 0x04002B36 RID: 11062
		private const TargetIndex HaulableInd = TargetIndex.A;

		// Token: 0x04002B37 RID: 11063
		private const TargetIndex StoreCellInd = TargetIndex.B;

		// Token: 0x04002B38 RID: 11064
		private const int MinimumHaulingJobTicks = 30;
	}
}
