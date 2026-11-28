using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200154F RID: 5455
	public class Pawn_DraftController : IExposable
	{
		// Token: 0x17001685 RID: 5765
		// (get) Token: 0x06008549 RID: 34121 RVA: 0x00300CC4 File Offset: 0x002FEEC4
		// (set) Token: 0x0600854A RID: 34122 RVA: 0x00300CCC File Offset: 0x002FEECC
		public bool Drafted
		{
			get
			{
				return this.draftedInt;
			}
			set
			{
				if (value == this.draftedInt)
				{
					return;
				}
				this.pawn.mindState.priorityWork.ClearPrioritizedWorkAndJobQueue();
				this.fireAtWillInt = true;
				this.draftedInt = value;
				if (!value && this.pawn.Spawned)
				{
					this.pawn.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(this.pawn);
				}
				this.pawn.jobs.ClearQueuedJobs(true);
				if (this.pawn.jobs.curJob != null && this.pawn.jobs.IsCurrentJobPlayerInterruptible())
				{
					JobDriver_Ingest jobDriver_Ingest = this.pawn.jobs.curDriver as JobDriver_Ingest;
					if (jobDriver_Ingest != null && jobDriver_Ingest.EatingFromInventory)
					{
						this.pawn.inventory.innerContainer.TryAddRangeOrTransfer(this.pawn.carryTracker.innerContainer, true, false);
					}
					if (!value)
					{
						Pawn pawn = this.pawn.carryTracker.CarriedThing as Pawn;
						ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
						if (pawn != null && ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
						{
							Job job = ChildcareUtility.MakeBringBabyToSafetyJob(this.pawn, pawn);
							Pawn_JobTracker jobs = this.pawn.jobs;
							Job newJob = job;
							JobCondition lastJobEndCondition = JobCondition.InterruptForced;
							ThinkNode jobGiver = null;
							bool resumeCurJobAfterwards = false;
							bool cancelBusyStances = true;
							ThinkTreeDef thinkTree = null;
							bool? keepCarryingThingOverride = new bool?(true);
							jobs.StartJob(newJob, lastJobEndCondition, jobGiver, resumeCurJobAfterwards, cancelBusyStances, thinkTree, null, false, false, keepCarryingThingOverride, false, true, false);
						}
						else if (this.pawn.IsCarrying())
						{
							Thing thing;
							this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
						else
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
					}
					else
					{
						Pawn pawn2 = this.pawn.carryTracker.CarriedThing as Pawn;
						ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
						if (pawn2 != null && ChildcareUtility.CanSuckle(pawn2, out breastfeedFailReason))
						{
							Messages.Message("MessageDraftedPawnCarryingBaby".Translate(this.pawn), this.pawn, MessageTypeDefOf.NeutralEvent, true);
						}
						this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
				if (this.draftedInt)
				{
					Lord lord = this.pawn.GetLord();
					if (lord != null && lord.LordJob is LordJob_VoluntarilyJoinable)
					{
						lord.Notify_PawnLost(this.pawn, PawnLostCondition.Drafted, null);
					}
					this.autoUndrafter.Notify_Drafted();
					Pawn_JobTracker jobs2 = this.pawn.jobs;
					if (jobs2 != null)
					{
						jobs2.SetFormingCaravanTick(true);
					}
					this.pawn.inventory.DropAllPackingCaravanThings();
					CompCanBeDormant compCanBeDormant = this.pawn.TryGetComp<CompCanBeDormant>();
					if (compCanBeDormant != null)
					{
						compCanBeDormant.WakeUp();
					}
				}
				else
				{
					if (this.pawn.playerSettings != null)
					{
						this.pawn.playerSettings.animalsReleased = false;
					}
					if (this.pawn.IsFormingCaravan())
					{
						Pawn_JobTracker jobs3 = this.pawn.jobs;
						if (jobs3 != null)
						{
							jobs3.SetFormingCaravanTick(false);
						}
					}
				}
				foreach (Pawn pawn3 in PawnUtility.SpawnedMasteredPawns(this.pawn))
				{
					pawn3.jobs.Notify_MasterDraftedOrUndrafted();
				}
			}
		}

		// Token: 0x17001686 RID: 5766
		// (get) Token: 0x0600854B RID: 34123 RVA: 0x00301000 File Offset: 0x002FF200
		// (set) Token: 0x0600854C RID: 34124 RVA: 0x00301008 File Offset: 0x002FF208
		public bool FireAtWill
		{
			get
			{
				return this.fireAtWillInt;
			}
			set
			{
				this.fireAtWillInt = value;
				if (this.fireAtWillInt)
				{
					return;
				}
				if (this.pawn.stances.curStance is Stance_Warmup)
				{
					this.pawn.stances.CancelBusyStanceSoft();
				}
			}
		}

		// Token: 0x17001687 RID: 5767
		// (get) Token: 0x0600854D RID: 34125 RVA: 0x00301044 File Offset: 0x002FF244
		public bool ShowDraftGizmo
		{
			get
			{
				return (!ModsConfig.BiotechActive || !this.pawn.IsColonyMech || this.pawn.GetMechControlGroup() != null) && (!this.pawn.IsSubhuman || this.pawn.IsColonySubhumanPlayerControlled);
			}
		}

		// Token: 0x0600854E RID: 34126 RVA: 0x00301091 File Offset: 0x002FF291
		public Pawn_DraftController(Pawn pawn)
		{
			this.pawn = pawn;
			this.autoUndrafter = new AutoUndrafter(pawn);
		}

		// Token: 0x0600854F RID: 34127 RVA: 0x003010B4 File Offset: 0x002FF2B4
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.draftedInt, "drafted", false, false);
			Scribe_Values.Look<bool>(ref this.fireAtWillInt, "fireAtWill", true, false);
			Scribe_Deep.Look<AutoUndrafter>(ref this.autoUndrafter, "autoUndrafter", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x06008550 RID: 34128 RVA: 0x00301104 File Offset: 0x002FF304
		public void DraftControllerTickInterval(int delta)
		{
			this.autoUndrafter.AutoUndraftTickInterval(delta);
		}

		// Token: 0x06008551 RID: 34129 RVA: 0x00301112 File Offset: 0x002FF312
		internal IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_DraftController.<GetGizmos>d__15 <GetGizmos>d__ = new Pawn_DraftController.<GetGizmos>d__15(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06008552 RID: 34130 RVA: 0x00301122 File Offset: 0x002FF322
		internal void Notify_PrimaryWeaponChanged()
		{
			this.fireAtWillInt = true;
		}

		// Token: 0x04005209 RID: 21001
		public Pawn pawn;

		// Token: 0x0400520A RID: 21002
		private bool draftedInt;

		// Token: 0x0400520B RID: 21003
		private bool fireAtWillInt = true;

		// Token: 0x0400520C RID: 21004
		private AutoUndrafter autoUndrafter;
	}
}
