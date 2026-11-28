using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A6A RID: 2666
	public class JobDriver_RepairMech : JobDriver
	{
		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x0600506E RID: 20590 RVA: 0x001E41DC File Offset: 0x001E23DC
		protected Pawn Mech
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x0600506F RID: 20591 RVA: 0x001E4202 File Offset: 0x001E2402
		protected virtual bool Remote
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x06005070 RID: 20592 RVA: 0x001E4205 File Offset: 0x001E2405
		protected int TicksPerHeal
		{
			get
			{
				return Mathf.RoundToInt(1f / this.pawn.GetStatValue(StatDefOf.MechRepairSpeed, true, -1) * 120f);
			}
		}

		// Token: 0x06005071 RID: 20593 RVA: 0x001E422A File Offset: 0x001E242A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Mech, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005072 RID: 20594 RVA: 0x001E424D File Offset: 0x001E244D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RepairMech.<MakeNewToils>d__10 <MakeNewToils>d__ = new JobDriver_RepairMech.<MakeNewToils>d__10(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06005073 RID: 20595 RVA: 0x001E425D File Offset: 0x001E245D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToNextRepair, "ticksToNextRepair", 0, false);
		}

		// Token: 0x04003145 RID: 12613
		private const TargetIndex MechInd = TargetIndex.A;

		// Token: 0x04003146 RID: 12614
		private const int DefaultTicksPerHeal = 120;

		// Token: 0x04003147 RID: 12615
		protected int ticksToNextRepair;
	}
}
