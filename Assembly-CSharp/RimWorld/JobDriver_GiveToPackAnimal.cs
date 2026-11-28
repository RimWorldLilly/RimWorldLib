using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009B8 RID: 2488
	public class JobDriver_GiveToPackAnimal : JobDriver
	{
		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06004B4E RID: 19278 RVA: 0x001D31D8 File Offset: 0x001D13D8
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06004B4F RID: 19279 RVA: 0x001D31FC File Offset: 0x001D13FC
		private Pawn Animal
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x001D3222 File Offset: 0x001D1422
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x001D3245 File Offset: 0x001D1445
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_GiveToPackAnimal.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_GiveToPackAnimal.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x001D3255 File Offset: 0x001D1455
		private Toil FindCarrierToil()
		{
			Toil toil = ToilMaker.MakeToil("FindCarrierToil");
			toil.initAction = delegate()
			{
				Pawn pawn = this.FindCarrier();
				if (pawn == null)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				this.job.SetTarget(TargetIndex.B, pawn);
			};
			return toil;
		}

		// Token: 0x06004B53 RID: 19283 RVA: 0x001D3274 File Offset: 0x001D1474
		private Pawn FindCarrier()
		{
			IEnumerable<Pawn> enumerable = GiveToPackAnimalUtility.CarrierCandidatesFor(this.pawn);
			Pawn animal = this.Animal;
			if (animal != null && enumerable.Contains(animal) && animal.RaceProps.packAnimal && this.CanCarryAtLeastOne(animal))
			{
				return animal;
			}
			Pawn pawn = null;
			float num = -1f;
			foreach (Pawn pawn2 in enumerable)
			{
				if (pawn2.RaceProps.packAnimal && this.CanCarryAtLeastOne(pawn2))
				{
					float num2 = (float)pawn2.Position.DistanceToSquared(this.pawn.Position);
					if (pawn == null || num2 < num)
					{
						pawn = pawn2;
						num = num2;
					}
				}
			}
			return pawn;
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x001D333C File Offset: 0x001D153C
		private bool CanCarryAtLeastOne(Pawn carrier)
		{
			return !MassUtility.WillBeOverEncumberedAfterPickingUp(carrier, this.Item, 1);
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x001D334E File Offset: 0x001D154E
		private Toil GiveToCarrierAsMuchAsPossibleToil()
		{
			Toil toil = ToilMaker.MakeToil("GiveToCarrierAsMuchAsPossibleToil");
			toil.initAction = delegate()
			{
				if (this.Item == null)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				int count = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(this.Animal, this.Item), this.Item.stackCount);
				this.pawn.carryTracker.innerContainer.TryTransferToContainer(this.Item, this.Animal.inventory.innerContainer, count, true);
			};
			return toil;
		}

		// Token: 0x04002FC4 RID: 12228
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x04002FC5 RID: 12229
		private const TargetIndex AnimalInd = TargetIndex.B;
	}
}
