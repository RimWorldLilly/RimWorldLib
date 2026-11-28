using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A8A RID: 2698
	public class JobDriver_UseItem : JobDriver
	{
		// Token: 0x0600514A RID: 20810 RVA: 0x001E695C File Offset: 0x001E4B5C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.useDuration, "useDuration", 0, false);
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x001E6978 File Offset: 0x001E4B78
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.useDuration = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable>().Props.useDuration;
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x001E69B4 File Offset: 0x001E4BB4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false) && (!this.job.targetB.IsValid || this.pawn.Reserve(this.job.targetB, this.job, 1, -1, null, errorOnFailed, false));
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x001E6A1E File Offset: 0x001E4C1E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_UseItem.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_UseItem.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x001E6A30 File Offset: 0x001E4C30
		protected Toil PrepareToUse()
		{
			Toil toil = Toils_General.Wait(this.useDuration, TargetIndex.A);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, base.TargetThingA.def.hasInteractionCell ? PathEndMode.InteractionCell : PathEndMode.Touch);
			toil.handlingFacing = true;
			toil.tickAction = delegate()
			{
				ThingWithComps thingWithComps = this.job.GetTarget(TargetIndex.A).Thing as ThingWithComps;
				if (thingWithComps != null)
				{
					using (IEnumerator<CompUseEffect> enumerator = thingWithComps.GetComps<CompUseEffect>().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CompUseEffect compUseEffect = enumerator.Current;
							if (compUseEffect != null)
							{
								compUseEffect.PrepareTick();
							}
						}
						goto IL_74;
					}
				}
				CompUseEffect compUseEffect2 = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUseEffect>();
				if (compUseEffect2 != null)
				{
					compUseEffect2.PrepareTick();
				}
				IL_74:
				CompUsable compUsable = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable>();
				if (compUsable != null && this.warmupMote == null && compUsable.Props.warmupMote != null)
				{
					this.warmupMote = MoteMaker.MakeAttachedOverlay(this.job.GetTarget(TargetIndex.B).Thing, compUsable.Props.warmupMote, Vector3.zero, 1f, -1f);
				}
				Mote mote = this.warmupMote;
				if (mote != null)
				{
					mote.Maintain();
				}
				this.pawn.rotationTracker.FaceTarget(base.TargetA);
			};
			if (this.job.targetB.IsValid)
			{
				toil.FailOnDespawnedOrNull(TargetIndex.B);
				CompTargetable compTargetable = this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompTargetable>();
				if (compTargetable != null && compTargetable.Props.nonDownedPawnOnly)
				{
					toil.FailOnDestroyedOrNull(TargetIndex.B);
					toil.FailOnDowned(TargetIndex.B);
				}
			}
			return toil;
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x001E6AEC File Offset: 0x001E4CEC
		protected Toil Use()
		{
			Toil use = ToilMaker.MakeToil("Use");
			use.initAction = delegate()
			{
				Pawn actor = use.actor;
				CompUsable compUsable = actor.CurJob.targetA.Thing.TryGetComp<CompUsable>();
				compUsable.UsedBy(actor);
				if (compUsable.Props.finalizeMote != null)
				{
					MoteMaker.MakeAttachedOverlay(actor.CurJob.GetTarget(TargetIndex.A).Thing, compUsable.Props.finalizeMote, Vector3.zero, 1f, -1f);
				}
			};
			use.defaultCompleteMode = ToilCompleteMode.Instant;
			return use;
		}

		// Token: 0x04003186 RID: 12678
		private int useDuration = -1;

		// Token: 0x04003187 RID: 12679
		private Mote warmupMote;
	}
}
