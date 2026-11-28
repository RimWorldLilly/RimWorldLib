using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020009BB RID: 2491
	public class JobDriver_PrepareCaravan_GatherItems : JobDriver
	{
		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06004B5D RID: 19293 RVA: 0x001D3574 File Offset: 0x001D1774
		public Thing ToHaul
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06004B5E RID: 19294 RVA: 0x001D3598 File Offset: 0x001D1798
		public Pawn Carrier
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06004B5F RID: 19295 RVA: 0x001D35BE File Offset: 0x001D17BE
		private List<TransferableOneWay> Transferables
		{
			get
			{
				return ((LordJob_FormAndSendCaravan)this.job.lord.LordJob).transferables;
			}
		}

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06004B60 RID: 19296 RVA: 0x001D35DC File Offset: 0x001D17DC
		private TransferableOneWay Transferable
		{
			get
			{
				TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(this.ToHaul, this.Transferables, TransferAsOneMode.PodsOrCaravanPacking);
				if (transferableOneWay != null)
				{
					return transferableOneWay;
				}
				throw new InvalidOperationException("Could not find any matching transferable.");
			}
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x001D360B File Offset: 0x001D180B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ToHaul, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x001D3630 File Offset: 0x001D1830
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (this.gatherState == PrepareCaravanGatherState.Unset)
			{
				this.gatherState = ((!this.pawn.IsFormingCaravan() || (MassUtility.IsOverEncumbered(this.pawn) && !this.pawn.inventory.HasAnyUnpackedCaravanItems)) ? PrepareCaravanGatherState.Carry : PrepareCaravanGatherState.Haul);
			}
			if (this.gatherState == PrepareCaravanGatherState.Carry)
			{
				return this.MakeNewToilsCarry();
			}
			return this.MakeNewToilsHaulInInventory();
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x001D3691 File Offset: 0x001D1891
		private IEnumerable<Toil> MakeNewToilsCarry()
		{
			JobDriver_PrepareCaravan_GatherItems.<MakeNewToilsCarry>d__17 <MakeNewToilsCarry>d__ = new JobDriver_PrepareCaravan_GatherItems.<MakeNewToilsCarry>d__17(-2);
			<MakeNewToilsCarry>d__.<>4__this = this;
			return <MakeNewToilsCarry>d__;
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x001D36A1 File Offset: 0x001D18A1
		private IEnumerable<Toil> MakeNewToilsHaulInInventory()
		{
			JobDriver_PrepareCaravan_GatherItems.<MakeNewToilsHaulInInventory>d__18 <MakeNewToilsHaulInInventory>d__ = new JobDriver_PrepareCaravan_GatherItems.<MakeNewToilsHaulInInventory>d__18(-2);
			<MakeNewToilsHaulInInventory>d__.<>4__this = this;
			return <MakeNewToilsHaulInInventory>d__;
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x001D36B4 File Offset: 0x001D18B4
		private Toil DetermineNumToHaul(Toil findCarrier = null)
		{
			Toil toil = ToilMaker.MakeToil("DetermineNumToHaul");
			toil.initAction = delegate()
			{
				int num = GatherItemsForCaravanUtility.CountLeftToTransfer(this.pawn, this.Transferable, this.job.lord);
				if (this.pawn.carryTracker.CarriedThing != null)
				{
					num -= this.pawn.carryTracker.CarriedThing.stackCount;
				}
				if (num > 0)
				{
					this.job.count = num;
					return;
				}
				if (findCarrier == null || !this.pawn.inventory.HasAnyUnpackedCaravanItems)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					return;
				}
				this.pawn.jobs.curDriver.JumpToToil(findCarrier);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x001D36FF File Offset: 0x001D18FF
		private Toil AddCarriedThingToTransferables()
		{
			Toil toil = ToilMaker.MakeToil("AddCarriedThingToTransferables");
			toil.initAction = delegate()
			{
				TransferableOneWay transferable = this.Transferable;
				if (!transferable.things.Contains(this.pawn.carryTracker.CarriedThing))
				{
					transferable.things.Add(this.pawn.carryTracker.CarriedThing);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x001D372B File Offset: 0x001D192B
		private Toil FindCarrier()
		{
			Toil toil = ToilMaker.MakeToil("FindCarrier");
			toil.initAction = delegate()
			{
				Pawn pawn = this.FindBestCarrier(true);
				if (pawn == null)
				{
					bool flag = this.pawn.GetLord() == this.job.lord;
					if (flag && !MassUtility.IsOverEncumbered(this.pawn))
					{
						pawn = this.pawn;
					}
					else
					{
						pawn = this.FindBestCarrier(false);
						if (pawn == null)
						{
							if (flag)
							{
								pawn = this.pawn;
							}
							else
							{
								IEnumerable<Pawn> source = from x in this.job.lord.ownedPawns
								where JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(x, this.pawn, true)
								select x;
								if (!source.Any<Pawn>())
								{
									base.EndJobWith(JobCondition.Incompletable);
									return;
								}
								pawn = source.RandomElement<Pawn>();
							}
						}
					}
				}
				this.job.SetTarget(TargetIndex.B, pawn);
			};
			return toil;
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x001D3749 File Offset: 0x001D1949
		private bool IsFinishedCollectingItems()
		{
			return MassUtility.IsOverEncumbered(this.pawn) || (this.pickedUpFirstItemTicks > -1 && Find.TickManager.TicksGame > this.pickedUpFirstItemTicks + 7500);
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x001D3780 File Offset: 0x001D1980
		private Toil HaulCaravanItemInInventory(Toil reserve)
		{
			Toil toil = ToilMaker.MakeToil("HaulCaravanItemInInventory");
			toil.initAction = delegate()
			{
				if (this.pickedUpFirstItemTicks == -1)
				{
					this.pickedUpFirstItemTicks = Find.TickManager.TicksGame;
				}
				this.Transferable.AdjustTo(Mathf.Max(this.Transferable.CountToTransfer - this.pawn.carryTracker.CarriedThing.stackCount, 0));
				this.pawn.inventory.AddHauledCaravanItem(this.pawn.carryTracker.CarriedThing);
				if (!this.IsFinishedCollectingItems())
				{
					this.SetNewHaulTargetAndJumpToReserve(reserve);
				}
			};
			return toil;
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x001D37C0 File Offset: 0x001D19C0
		private void SetNewHaulTargetAndJumpToReserve(Toil reserve)
		{
			if (this.CheckToilLoopBackstop())
			{
				Thing thing = GatherItemsForCaravanUtility.FindThingToHaul(this.pawn, this.pawn.GetLord());
				if (thing != null)
				{
					this.job.SetTarget(TargetIndex.A, thing);
					this.pawn.jobs.curDriver.JumpToToil(reserve);
				}
			}
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x001D3818 File Offset: 0x001D1A18
		private Toil AddHauledItemsToCarrier(Toil findCarrier)
		{
			Toil toil = ToilMaker.MakeToil("AddHauledItemsToCarrier");
			toil.initAction = delegate()
			{
				if (this.Carrier == this.pawn)
				{
					this.pawn.inventory.ClearHaulingCaravanCache();
					return;
				}
				this.pawn.inventory.TransferCaravanItemsToCarrier(this.Carrier.inventory);
				if (this.pawn.inventory.HasAnyUnpackedCaravanItems && this.CheckToilLoopBackstop())
				{
					this.pawn.jobs.curDriver.JumpToToil(findCarrier);
				}
			};
			return toil;
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x001D3858 File Offset: 0x001D1A58
		private bool CheckToilLoopBackstop()
		{
			int num = this.toilLoops + 1;
			this.toilLoops = num;
			if (num > 500)
			{
				Log.Error("Prepare caravan gather items job for pawn " + this.pawn.Label + " looped through toils too many times");
				base.EndJobWith(JobCondition.Errored);
				return false;
			}
			return true;
		}

		// Token: 0x06004B6D RID: 19309 RVA: 0x001D38A7 File Offset: 0x001D1AA7
		private Toil PlaceTargetInCarrierInventory()
		{
			Toil toil = ToilMaker.MakeToil("PlaceTargetInCarrierInventory");
			toil.initAction = delegate()
			{
				Pawn_CarryTracker carryTracker = this.pawn.carryTracker;
				Thing carriedThing = carryTracker.CarriedThing;
				if (carryTracker.innerContainer.Count == 0)
				{
					carryTracker.pawn.Drawer.renderer.SetAllGraphicsDirty();
				}
				this.Transferable.AdjustTo(Mathf.Max(this.Transferable.CountToTransfer - carriedThing.stackCount, 0));
				Thing thing;
				carryTracker.innerContainer.TryTransferToContainer(carriedThing, this.Carrier.inventory.innerContainer, carriedThing.stackCount, out thing, true);
				CompForbiddable compForbiddable = (thing != null) ? thing.TryGetComp<CompForbiddable>() : null;
				if (compForbiddable != null)
				{
					compForbiddable.Forbidden = false;
				}
			};
			return toil;
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x001D38C8 File Offset: 0x001D1AC8
		public static bool IsUsableCarrier(Pawn p, Pawn forPawn, bool allowColonists)
		{
			return p.IsFormingCaravan() && (p == forPawn || (!p.DestroyedOrNull() && p.Spawned && !p.inventory.UnloadEverything && forPawn.CanReach(p, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && ((allowColonists && p.IsColonist) || ((p.RaceProps.packAnimal || p.HostFaction == Faction.OfPlayer) && !p.IsBurning() && !p.Downed && !MassUtility.IsOverEncumbered(p)))));
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x001D395C File Offset: 0x001D1B5C
		private float GetCarrierScore(Pawn p)
		{
			float lengthHorizontal = (p.Position - this.pawn.Position).LengthHorizontal;
			float num = MassUtility.EncumbrancePercent(p);
			return 1f - num - lengthHorizontal / 10f * 0.2f;
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x001D39A4 File Offset: 0x001D1BA4
		private Pawn FindBestCarrier(bool onlyAnimals)
		{
			Lord lord = this.job.lord;
			Pawn pawn = null;
			float num = 0f;
			if (lord != null)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn2 = lord.ownedPawns[i];
					if (pawn2 != this.pawn && (!onlyAnimals || pawn2.RaceProps.Animal) && JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(pawn2, this.pawn, false))
					{
						float carrierScore = this.GetCarrierScore(pawn2);
						if (pawn == null || carrierScore > num)
						{
							pawn = pawn2;
							num = carrierScore;
						}
					}
				}
			}
			return pawn;
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x001D3A30 File Offset: 0x001D1C30
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.pickedUpFirstItemTicks, "pickedUpFirstItemTicks", 0, false);
			Scribe_Values.Look<int>(ref this.toilLoops, "toilLoops", 0, false);
			Scribe_Values.Look<PrepareCaravanGatherState>(ref this.gatherState, "gatherState", PrepareCaravanGatherState.Unset, false);
		}

		// Token: 0x04002FCA RID: 12234
		private int pickedUpFirstItemTicks = -1;

		// Token: 0x04002FCB RID: 12235
		private int toilLoops;

		// Token: 0x04002FCC RID: 12236
		private PrepareCaravanGatherState gatherState;

		// Token: 0x04002FCD RID: 12237
		private const TargetIndex ToHaulInd = TargetIndex.A;

		// Token: 0x04002FCE RID: 12238
		private const TargetIndex CarrierInd = TargetIndex.B;

		// Token: 0x04002FCF RID: 12239
		private const int MaxTicksGatherItems = 7500;

		// Token: 0x04002FD0 RID: 12240
		private const int LoopBackstop = 500;
	}
}
