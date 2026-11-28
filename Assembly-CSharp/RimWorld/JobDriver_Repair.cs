using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009D2 RID: 2514
	public class JobDriver_Repair : JobDriver
	{
		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x06004C54 RID: 19540 RVA: 0x001D6CC8 File Offset: 0x001D4EC8
		private Building Building
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06004C55 RID: 19541 RVA: 0x001D6CF0 File Offset: 0x001D4EF0
		private bool IsBuildingAttachment
		{
			get
			{
				ThingDef thingDef = GenConstruct.BuiltDefOf(this.Building.def) as ThingDef;
				return ((thingDef != null) ? thingDef.building : null) != null && thingDef.building.isAttachment;
			}
		}

		// Token: 0x06004C56 RID: 19542 RVA: 0x001D6D2E File Offset: 0x001D4F2E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004C57 RID: 19543 RVA: 0x001D6D51 File Offset: 0x001D4F51
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Repair.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_Repair.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003007 RID: 12295
		protected float ticksToNextRepair;

		// Token: 0x04003008 RID: 12296
		private const float WarmupTicks = 80f;

		// Token: 0x04003009 RID: 12297
		private const float TicksBetweenRepairs = 20f;
	}
}
