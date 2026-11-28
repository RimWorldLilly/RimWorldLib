using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x0200080C RID: 2060
	public class JobDriver_PlayToys : JobDriver_BabyPlay
	{
		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x060040D7 RID: 16599 RVA: 0x0019C14A File Offset: 0x0019A34A
		private Thing ToyBox
		{
			get
			{
				return base.TargetThingB;
			}
		}

		// Token: 0x17000C83 RID: 3203
		// (get) Token: 0x060040D8 RID: 16600 RVA: 0x0019C152 File Offset: 0x0019A352
		private Building_Bed Bed
		{
			get
			{
				return (Building_Bed)base.TargetThingC;
			}
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x060040D9 RID: 16601 RVA: 0x0019C15F File Offset: 0x0019A35F
		protected override JobDriver_BabyPlay.StartingConditions StartingCondition
		{
			get
			{
				return JobDriver_BabyPlay.StartingConditions.PickupBaby;
			}
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x0019C164 File Offset: 0x0019A364
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return base.TryMakePreToilReservations(errorOnFailed) && (this.Bed == null || this.pawn.Reserve(this.Bed, this.job, 1, -1, null, errorOnFailed, false)) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x0019C1F1 File Offset: 0x0019A3F1
		protected override IEnumerable<Toil> Play()
		{
			JobDriver_PlayToys.<Play>d__13 <Play>d__ = new JobDriver_PlayToys.<Play>d__13(-2);
			<Play>d__.<>4__this = this;
			return <Play>d__;
		}

		// Token: 0x060040DC RID: 16604 RVA: 0x0019C204 File Offset: 0x0019A404
		private Toil PlayToil()
		{
			Toil toil = ToilMaker.MakeToil("PlayToil");
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.handlingFacing = true;
			toil.WithEffect(EffecterDefOf.PlayStatic, TargetIndex.A, null);
			toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				if (this.motesToMaintain.NullOrEmpty<Mote>())
				{
					Vector3 a = base.Baby.TrueCenter();
					Vector3 v = IntVec3.North.ToVector3();
					float num = 72f;
					this.motesToMaintain = new Mote[5];
					for (int i = 0; i < 5; i++)
					{
						Vector3 loc = a + v.RotatedBy(num * (float)i + JobDriver_PlayToys.ToyRandomAngleOffset.RandomInRange) * 0.5f;
						this.motesToMaintain[i] = MoteMaker.MakeStaticMote(loc, base.Map, ThingDefOf.Mote_Toy, 1f, false, 0f);
					}
				}
				this.pawn.rotationTracker.FaceTarget(base.Baby);
				for (int j = 0; j < this.motesToMaintain.Length; j++)
				{
					Mote mote = this.motesToMaintain[j];
					if (mote != null)
					{
						mote.Maintain();
					}
				}
				if (this.pawn.IsHashIntervalTick(600, delta))
				{
					this.pawn.interactions.TryInteractWith(base.Baby, InteractionDefOf.BabyPlay);
				}
				if (this.roomPlayGainFactor < 0f)
				{
					this.roomPlayGainFactor = BabyPlayUtility.GetRoomPlayGainFactors(base.Baby);
				}
				if (BabyPlayUtility.PlayTickCheckEnd(base.Baby, this.pawn, this.roomPlayGainFactor, delta, this.ToyBox))
				{
					this.pawn.jobs.curDriver.ReadyForNextToil();
				}
			}));
			ChildcareUtility.MakeBabyPlayAsLongAsToilIsActive(toil, TargetIndex.A);
			return toil;
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x0019C269 File Offset: 0x0019A469
		private Toil DropBaby()
		{
			Toil toil = ToilMaker.MakeToil("DropBaby");
			toil.initAction = delegate()
			{
				Thing thing;
				this.pawn.carryTracker.innerContainer.TryDrop(base.Baby, ThingPlaceMode.Near, 1, out thing, null, (IntVec3 c) => !this.ToyBox.OccupiedRect().Contains(c));
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x04002AFD RID: 11005
		private const TargetIndex ToyBoxInd = TargetIndex.B;

		// Token: 0x04002AFE RID: 11006
		private const int ToysCount = 5;

		// Token: 0x04002AFF RID: 11007
		private const float ToyDistanceFactor = 0.5f;

		// Token: 0x04002B00 RID: 11008
		private static readonly FloatRange ToyRandomAngleOffset = new FloatRange(-5f, 5f);

		// Token: 0x04002B01 RID: 11009
		private const int InteractionTicks = 600;

		// Token: 0x04002B02 RID: 11010
		private Mote[] motesToMaintain;
	}
}
