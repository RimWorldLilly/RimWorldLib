using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A92 RID: 2706
	public abstract class JobDriver_PlantWork : JobDriver
	{
		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06005181 RID: 20865 RVA: 0x001E74E3 File Offset: 0x001E56E3
		protected Plant Plant
		{
			get
			{
				return (Plant)this.job.targetA.Thing;
			}
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06005182 RID: 20866 RVA: 0x001E74FA File Offset: 0x001E56FA
		protected virtual DesignationDef RequiredDesignation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06005183 RID: 20867 RVA: 0x001E74FD File Offset: 0x001E56FD
		protected virtual PlantDestructionMode PlantDestructionMode
		{
			get
			{
				return PlantDestructionMode.Smash;
			}
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x001E7500 File Offset: 0x001E5700
		public static float WorkDonePerTick(Pawn actor, Plant plant)
		{
			return actor.GetStatValue(StatDefOf.PlantWorkSpeed, true, -1) * Mathf.Lerp(3.3f, 1f, plant.Growth);
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x001E7528 File Offset: 0x001E5728
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
			if (target.IsValid && !this.pawn.Reserve(target, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06005186 RID: 20870 RVA: 0x001E7587 File Offset: 0x001E5787
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PlantWork.<MakeNewToils>d__11 <MakeNewToils>d__ = new JobDriver_PlantWork.<MakeNewToils>d__11(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06005187 RID: 20871 RVA: 0x001E7597 File Offset: 0x001E5797
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		// Token: 0x06005188 RID: 20872 RVA: 0x001E75B5 File Offset: 0x001E57B5
		protected virtual void Init()
		{
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x001E75B7 File Offset: 0x001E57B7
		protected virtual Toil PlantWorkDoneToil()
		{
			return null;
		}

		// Token: 0x04003193 RID: 12691
		private float workDone;

		// Token: 0x04003194 RID: 12692
		protected float xpPerTick;

		// Token: 0x04003195 RID: 12693
		protected const TargetIndex PlantInd = TargetIndex.A;
	}
}
