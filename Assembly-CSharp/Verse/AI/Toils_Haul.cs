using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000837 RID: 2103
	public class Toils_Haul
	{
		// Token: 0x060041E2 RID: 16866 RVA: 0x001A0E48 File Offset: 0x0019F048
		public static bool ErrorCheckForCarry(Pawn pawn, Thing haulThing, bool canTakeFromInventory = false)
		{
			if (!haulThing.SpawnedOrAnyParentSpawned || (!canTakeFromInventory && !haulThing.Spawned))
			{
				Log.Message(((pawn != null) ? pawn.ToString() : null) + " tried to start carry " + ((haulThing != null) ? haulThing.ToString() : null) + " which isn't spawned.");
				pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				return true;
			}
			if (haulThing.stackCount == 0)
			{
				Log.Message(((pawn != null) ? pawn.ToString() : null) + " tried to start carry " + ((haulThing != null) ? haulThing.ToString() : null) + " which had stackcount 0.");
				pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				return true;
			}
			if (pawn.jobs.curJob.count <= 0)
			{
				string str = "Invalid count: ";
				string str2 = pawn.jobs.curJob.count.ToString();
				string str3 = ", setting to 1. Job was ";
				Job curJob = pawn.jobs.curJob;
				Log.Error(str + str2 + str3 + ((curJob != null) ? curJob.ToString() : null));
				pawn.jobs.curJob.count = 1;
			}
			return false;
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x001A0F54 File Offset: 0x0019F154
		public static Toil StartCarryThing(TargetIndex haulableInd, bool putRemainderInQueue = false, bool subtractNumTakenFromJobCount = false, bool failIfStackCountLessThanJobCount = false, bool reserve = true, bool canTakeFromInventory = false)
		{
			Toil toil = ToilMaker.MakeToil("StartCarryThing");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(haulableInd).Thing;
				if (Toils_Haul.ErrorCheckForCarry(actor, thing, canTakeFromInventory))
				{
					return;
				}
				if (curJob.count == 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 2);
					defaultInterpolatedStringHandler.AppendLiteral("StartCarryThing job had count = ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(curJob.count);
					defaultInterpolatedStringHandler.AppendLiteral(". Job: ");
					defaultInterpolatedStringHandler.AppendFormatted<Job>(curJob);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int num = actor.carryTracker.AvailableStackSpace(thing.def);
				if (num <= 0)
				{
					int value = actor.carryTracker.MaxStackSpaceEver(thing.def);
					int value2 = 0;
					if (actor.carryTracker.CarriedThing != null)
					{
						value2 = actor.carryTracker.CarriedThing.stackCount;
					}
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(82, 5);
					defaultInterpolatedStringHandler2.AppendLiteral("StartCarryThing got availableStackSpace ");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler2.AppendLiteral(" (haulTarg ");
					defaultInterpolatedStringHandler2.AppendFormatted<Thing>(thing);
					defaultInterpolatedStringHandler2.AppendLiteral(", Job: ");
					defaultInterpolatedStringHandler2.AppendFormatted<Job>(curJob);
					defaultInterpolatedStringHandler2.AppendLiteral(", maximum: ");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(value);
					defaultInterpolatedStringHandler2.AppendLiteral(", carrying: ");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(value2);
					defaultInterpolatedStringHandler2.AppendLiteral(")");
					throw new Exception(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
				if (failIfStackCountLessThanJobCount && thing.stackCount < curJob.count)
				{
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
					return;
				}
				int num2 = Mathf.Min(new int[]
				{
					curJob.count,
					num,
					thing.stackCount
				});
				if (num2 <= 0)
				{
					int value3 = actor.carryTracker.MaxStackSpaceEver(thing.def);
					int value4 = 0;
					if (actor.carryTracker.CarriedThing != null)
					{
						value4 = actor.carryTracker.CarriedThing.stackCount;
					}
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(147, 7);
					defaultInterpolatedStringHandler3.AppendLiteral("StartCarryThing zero or negative desiredNumToTake (");
					defaultInterpolatedStringHandler3.AppendFormatted<int>(num2);
					defaultInterpolatedStringHandler3.AppendLiteral("), actor: ");
					defaultInterpolatedStringHandler3.AppendFormatted<Pawn>(actor);
					defaultInterpolatedStringHandler3.AppendLiteral(", curJob.count: ");
					defaultInterpolatedStringHandler3.AppendFormatted<int>(curJob.count);
					defaultInterpolatedStringHandler3.AppendLiteral(", availableStackSpace: ");
					defaultInterpolatedStringHandler3.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler3.AppendLiteral(" (maximum: ");
					defaultInterpolatedStringHandler3.AppendFormatted<int>(value3);
					defaultInterpolatedStringHandler3.AppendLiteral(", carrying: ");
					defaultInterpolatedStringHandler3.AppendFormatted<int>(value4);
					defaultInterpolatedStringHandler3.AppendLiteral("), haulTarg.stackCount: ");
					defaultInterpolatedStringHandler3.AppendFormatted<int>(thing.stackCount);
					throw new Exception(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
				int stackCount = thing.stackCount;
				int num3 = actor.carryTracker.TryStartCarry(thing, num2, reserve);
				if (num3 == 0)
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
				if (num3 < stackCount)
				{
					int num4 = curJob.count - num3;
					if (putRemainderInQueue && num4 > 0)
					{
						curJob.GetTargetQueue(haulableInd).Insert(0, thing);
						Job job = curJob;
						if (job.countQueue == null)
						{
							job.countQueue = new List<int>();
						}
						curJob.countQueue.Insert(0, num4);
					}
					else if (actor.Map.reservationManager.ReservedBy(thing, actor, curJob))
					{
						actor.Map.reservationManager.Release(thing, actor, curJob);
					}
				}
				if (subtractNumTakenFromJobCount)
				{
					curJob.count -= num3;
				}
				curJob.SetTarget(haulableInd, actor.carryTracker.CarriedThing);
				actor.records.Increment(RecordDefOf.ThingsHauled);
			};
			return toil;
		}

		// Token: 0x060041E4 RID: 16868 RVA: 0x001A0FC0 File Offset: 0x0019F1C0
		public static Toil StoreThingJob(TargetIndex thingIndex)
		{
			Toil toil = ToilMaker.MakeToil("StoreThingJob");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.CurJob;
				Thing thing = curJob.GetTarget(thingIndex).Thing;
				Job job = HaulAIUtility.HaulToStorageJob(actor, thing, curJob.playerForced);
				if (job != null)
				{
					actor.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}
			};
			return toil;
		}

		// Token: 0x060041E5 RID: 16869 RVA: 0x001A1008 File Offset: 0x0019F208
		public static Toil DropCarriedThing()
		{
			Toil toil = ToilMaker.MakeToil("DropCarriedThing");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (actor.carryTracker.CarriedThing == null)
				{
					Pawn pawn = actor;
					Log.Error(((pawn != null) ? pawn.ToString() : null) + " tried to drop carried thing but is not carrying anything.");
					return;
				}
				Thing thing;
				if (!actor.carryTracker.TryDropCarriedThing(actor.Position, ThingPlaceMode.Direct, out thing, null))
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			};
			return toil;
		}

		// Token: 0x060041E6 RID: 16870 RVA: 0x001A1048 File Offset: 0x0019F248
		public static Toil JumpIfAlsoCollectingNextTargetInQueue(Toil gotoGetTargetToil, TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("JumpIfAlsoCollectingNextTargetInQueue");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					return;
				}
				if (curJob.count <= 0)
				{
					return;
				}
				if (actor.carryTracker.CarriedThing == null)
				{
					string str = "JumpToAlsoCollectTargetInQueue run on ";
					Pawn pawn = actor;
					Log.Error(str + ((pawn != null) ? pawn.ToString() : null) + " who is not carrying something.");
					return;
				}
				if (actor.carryTracker.AvailableStackSpace(actor.carryTracker.CarriedThing.def) <= 0)
				{
					return;
				}
				for (int i = 0; i < targetQueue.Count; i++)
				{
					if (!GenAI.CanUseItemForWork(actor, targetQueue[i].Thing))
					{
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
					if (targetQueue[i].Thing.def == actor.carryTracker.CarriedThing.def)
					{
						curJob.SetTarget(ind, targetQueue[i].Thing);
						targetQueue.RemoveAt(i);
						actor.jobs.curDriver.JumpToToil(gotoGetTargetToil);
						return;
					}
				}
			};
			return toil;
		}

		// Token: 0x060041E7 RID: 16871 RVA: 0x001A1098 File Offset: 0x0019F298
		public static Toil CheckForGetOpportunityDuplicate(Toil getHaulTargetToil, TargetIndex haulableInd, TargetIndex storeCellInd, bool takeFromValidStorage = false, Predicate<Thing> extraValidator = null)
		{
			Toils_Haul.<>c__DisplayClass5_0 CS$<>8__locals1 = new Toils_Haul.<>c__DisplayClass5_0();
			CS$<>8__locals1.storeCellInd = storeCellInd;
			CS$<>8__locals1.takeFromValidStorage = takeFromValidStorage;
			CS$<>8__locals1.extraValidator = extraValidator;
			CS$<>8__locals1.haulableInd = haulableInd;
			CS$<>8__locals1.getHaulTargetToil = getHaulTargetToil;
			CS$<>8__locals1.toil = ToilMaker.MakeToil("CheckForGetOpportunityDuplicate");
			CS$<>8__locals1.toil.initAction = delegate()
			{
				Toils_Haul.<>c__DisplayClass5_1 CS$<>8__locals2 = new Toils_Haul.<>c__DisplayClass5_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.actor = CS$<>8__locals1.toil.actor;
				CS$<>8__locals2.curJob = CS$<>8__locals2.actor.jobs.curJob;
				if (CS$<>8__locals2.actor.carryTracker.CarriedThing.def.stackLimit == 1)
				{
					return;
				}
				if (CS$<>8__locals2.actor.carryTracker.Full)
				{
					return;
				}
				if (CS$<>8__locals2.curJob.count <= 0)
				{
					return;
				}
				Thing thing = GenClosest.ClosestThingReachable(CS$<>8__locals2.actor.Position, CS$<>8__locals2.actor.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.ClosestTouch, TraverseParms.For(CS$<>8__locals2.actor, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 8f, new Predicate<Thing>(CS$<>8__locals2.<CheckForGetOpportunityDuplicate>g__DupeValidator|1), null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (thing != null)
				{
					CS$<>8__locals2.curJob.SetTarget(CS$<>8__locals1.haulableInd, thing);
					CS$<>8__locals2.actor.jobs.curDriver.JumpToToil(CS$<>8__locals1.getHaulTargetToil);
				}
			};
			return CS$<>8__locals1.toil;
		}

		// Token: 0x060041E8 RID: 16872 RVA: 0x001A10FC File Offset: 0x0019F2FC
		public static Toil CarryHauledThingToCell(TargetIndex squareIndex, PathEndMode pathEndMode = PathEndMode.ClosestTouch)
		{
			Toil toil = ToilMaker.MakeToil("CarryHauledThingToCell");
			toil.initAction = delegate()
			{
				IntVec3 cell = toil.actor.jobs.curJob.GetTarget(squareIndex).Cell;
				toil.actor.pather.StartPath(cell, pathEndMode);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.AddEndCondition(delegate
			{
				Pawn actor = toil.actor;
				IntVec3 cell = actor.jobs.curJob.GetTarget(squareIndex).Cell;
				CompPushable compPushable = actor.carryTracker.CarriedThing.TryGetComp<CompPushable>();
				if (compPushable != null)
				{
					Vector3 v = actor.Position.ToVector3() + compPushable.drawPos;
					if (new IntVec3(v) == cell)
					{
						return JobCondition.Succeeded;
					}
				}
				return JobCondition.Ongoing;
			});
			toil.AddFailCondition(delegate
			{
				Pawn actor = toil.actor;
				IntVec3 cell = actor.jobs.curJob.GetTarget(squareIndex).Cell;
				if (actor.carryTracker.CarriedThing == null)
				{
					return true;
				}
				if (actor.jobs.curJob.haulMode == HaulMode.ToCellStorage && !cell.IsValidStorageFor(actor.Map, actor.carryTracker.CarriedThing))
				{
					return true;
				}
				CompPushable compPushable = actor.carryTracker.CarriedThing.TryGetComp<CompPushable>();
				return compPushable != null && !compPushable.canBePushed;
			});
			return toil;
		}

		// Token: 0x060041E9 RID: 16873 RVA: 0x001A1184 File Offset: 0x0019F384
		public static Toil PlaceCarriedThingInCellFacing(TargetIndex facingTargetInd)
		{
			Toil toil = ToilMaker.MakeToil("PlaceCarriedThingInCellFacing");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				if (actor.carryTracker.CarriedThing == null)
				{
					Pawn pawn = actor;
					Log.Error(((pawn != null) ? pawn.ToString() : null) + " tried to place hauled thing in facing cell but is not hauling anything.");
					return;
				}
				LocalTargetInfo target = actor.CurJob.GetTarget(facingTargetInd);
				IntVec3 b;
				if (target.HasThing)
				{
					b = target.Thing.OccupiedRect().ClosestCellTo(actor.Position);
				}
				else
				{
					b = target.Cell;
				}
				IntVec3 dropLoc = actor.Position + Pawn_RotationTracker.RotFromAngleBiased((actor.Position - b).AngleFlat).FacingCell;
				Thing thing;
				if (!actor.carryTracker.TryDropCarriedThing(dropLoc, ThingPlaceMode.Direct, out thing, null))
				{
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			};
			return toil;
		}

		// Token: 0x060041EA RID: 16874 RVA: 0x001A11CC File Offset: 0x0019F3CC
		public static Toil PlaceHauledThingInCell(TargetIndex cellInd, Toil nextToilOnPlaceFailOrIncomplete, bool storageMode, bool tryStoreInSameStorageIfSpotCantHoldWholeStack = false)
		{
			Toil toil = ToilMaker.MakeToil("PlaceHauledThingInCell");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				IntVec3 cell = curJob.GetTarget(cellInd).Cell;
				if (actor.carryTracker.CarriedThing == null)
				{
					Pawn pawn = actor;
					Log.Error(((pawn != null) ? pawn.ToString() : null) + " tried to place hauled thing in cell but is not hauling anything.");
					return;
				}
				SlotGroup slotGroup = actor.Map.haulDestinationManager.SlotGroupAt(cell);
				if (slotGroup != null && slotGroup.Settings.AllowedToAccept(actor.carryTracker.CarriedThing))
				{
					actor.Map.designationManager.TryRemoveDesignationOn(actor.carryTracker.CarriedThing, DesignationDefOf.Haul);
				}
				Action<Thing, int> placedAction = null;
				if (curJob.def == JobDefOf.DoBill || curJob.def == JobDefOf.RecolorApparel || curJob.def == JobDefOf.RefuelAtomic || curJob.def == JobDefOf.RearmTurretAtomic)
				{
					placedAction = delegate(Thing th, int added)
					{
						HaulAIUtility.UpdateJobWithPlacedThings(curJob, th, added);
					};
				}
				Thing thing;
				if (!actor.carryTracker.TryDropCarriedThing(cell, ThingPlaceMode.Direct, out thing, placedAction))
				{
					if (storageMode)
					{
						IntVec3 c;
						if (nextToilOnPlaceFailOrIncomplete != null && ((tryStoreInSameStorageIfSpotCantHoldWholeStack && StoreUtility.TryFindBestBetterStoreCellForIn(actor.carryTracker.CarriedThing, actor, actor.Map, StoragePriority.Unstored, actor.Faction, curJob.bill.GetSlotGroup(), out c, true)) || StoreUtility.TryFindBestBetterStoreCellFor(actor.carryTracker.CarriedThing, actor, actor.Map, StoragePriority.Unstored, actor.Faction, out c, true)))
						{
							if (actor.CanReserve(c, 1, -1, null, false))
							{
								actor.Reserve(c, actor.CurJob, 1, -1, null, true, false);
							}
							actor.CurJob.SetTarget(cellInd, c);
							actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
							return;
						}
						IntVec3 c2;
						if (!HaulAIUtility.CanHaulAside(actor, actor.carryTracker.CarriedThing, out c2))
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(102, 3);
							defaultInterpolatedStringHandler.AppendLiteral("Incomplete haul for ");
							defaultInterpolatedStringHandler.AppendFormatted<Pawn>(actor);
							defaultInterpolatedStringHandler.AppendLiteral(": Could not find anywhere to put ");
							defaultInterpolatedStringHandler.AppendFormatted<Thing>(actor.carryTracker.CarriedThing);
							defaultInterpolatedStringHandler.AppendLiteral(" near ");
							defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(actor.Position);
							defaultInterpolatedStringHandler.AppendLiteral(". Destroying. This should be very uncommon!");
							Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
							actor.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
							return;
						}
						curJob.SetTarget(cellInd, c2);
						curJob.count = int.MaxValue;
						curJob.haulOpportunisticDuplicates = false;
						curJob.haulMode = HaulMode.ToCellNonStorage;
						if (nextToilOnPlaceFailOrIncomplete != null)
						{
							actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
							return;
						}
					}
					else if (nextToilOnPlaceFailOrIncomplete != null)
					{
						actor.jobs.curDriver.JumpToToil(nextToilOnPlaceFailOrIncomplete);
						return;
					}
				}
			};
			return toil;
		}

		// Token: 0x060041EB RID: 16875 RVA: 0x001A1228 File Offset: 0x0019F428
		public static Toil CarryHauledThingToContainer()
		{
			Toil gotoDest = ToilMaker.MakeToil("CarryHauledThingToContainer");
			gotoDest.initAction = delegate()
			{
				gotoDest.actor.pather.StartPath(gotoDest.actor.jobs.curJob.targetB.Thing, PathEndMode.Touch);
			};
			gotoDest.AddFailCondition(delegate
			{
				Thing thing = gotoDest.actor.jobs.curJob.targetB.Thing;
				if (thing.Destroyed || (!gotoDest.actor.jobs.curJob.ignoreForbidden && thing.IsForbidden(gotoDest.actor)))
				{
					return true;
				}
				ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
				return thingOwner != null && !thingOwner.CanAcceptAnyOf(gotoDest.actor.carryTracker.CarriedThing, true);
			});
			gotoDest.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return gotoDest;
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x001A128C File Offset: 0x0019F48C
		public static Toil DepositHauledThingInContainer(TargetIndex containerInd, TargetIndex reserveForContainerInd, Action onDeposited = null)
		{
			Toil toil = ToilMaker.MakeToil("DepositHauledThingInContainer");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				if (actor.carryTracker.CarriedThing == null)
				{
					Pawn pawn = actor;
					Log.Error(((pawn != null) ? pawn.ToString() : null) + " tried to place hauled thing in container but is not hauling anything.");
					return;
				}
				Thing thing = curJob.GetTarget(containerInd).Thing;
				ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
				if (thingOwner != null)
				{
					int num = actor.carryTracker.CarriedThing.stackCount;
					IHaulEnroute haulEnroute = thing as IHaulEnroute;
					if (haulEnroute != null)
					{
						ThingDef def = actor.carryTracker.CarriedThing.def;
						num = Mathf.Min(haulEnroute.GetSpaceRemainingWithEnroute(def, actor), num);
						if (reserveForContainerInd != TargetIndex.None)
						{
							Thing thing2 = curJob.GetTarget(reserveForContainerInd).Thing;
							if (!thing2.DestroyedOrNull() && thing2 != haulEnroute)
							{
								IHaulEnroute haulEnroute2 = thing2 as IHaulEnroute;
								if (haulEnroute2 != null)
								{
									int spaceRemainingWithEnroute = haulEnroute2.GetSpaceRemainingWithEnroute(def, actor);
									num = Mathf.Min(num, actor.carryTracker.CarriedThing.stackCount - spaceRemainingWithEnroute);
								}
							}
						}
					}
					Thing carriedThing = actor.carryTracker.CarriedThing;
					int num2 = actor.carryTracker.innerContainer.TryTransferToContainer(carriedThing, thingOwner, num, true);
					if (num2 != 0)
					{
						IHaulEnroute haulEnroute3 = thing as IHaulEnroute;
						if (haulEnroute3 != null)
						{
							thing.Map.enrouteManager.ReleaseFor(haulEnroute3, actor);
						}
						INotifyHauledTo notifyHauledTo = thing as INotifyHauledTo;
						if (notifyHauledTo != null)
						{
							notifyHauledTo.Notify_HauledTo(actor, carriedThing, num2);
						}
						ThingWithComps thingWithComps = thing as ThingWithComps;
						if (thingWithComps != null)
						{
							foreach (ThingComp thingComp in thingWithComps.AllComps)
							{
								INotifyHauledTo notifyHauledTo2 = thingComp as INotifyHauledTo;
								if (notifyHauledTo2 != null)
								{
									notifyHauledTo2.Notify_HauledTo(actor, carriedThing, num2);
								}
							}
						}
						if (curJob.def == JobDefOf.DoBill)
						{
							HaulAIUtility.UpdateJobWithPlacedThings(curJob, carriedThing, num2);
						}
						Action onDeposited2 = onDeposited;
						if (onDeposited2 == null)
						{
							return;
						}
						onDeposited2();
						return;
					}
				}
				else
				{
					if (curJob.GetTarget(containerInd).Thing.def.Minifiable)
					{
						actor.carryTracker.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
						return;
					}
					string str = "Could not deposit hauled thing in container: ";
					Thing thing3 = curJob.GetTarget(containerInd).Thing;
					Log.Error(str + ((thing3 != null) ? thing3.ToString() : null));
				}
			};
			return toil;
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x001A12E4 File Offset: 0x0019F4E4
		public static Toil JumpToCarryToNextContainerIfPossible(Toil carryToContainerToil, TargetIndex primaryTargetInd)
		{
			Toil toil = ToilMaker.MakeToil("JumpToCarryToNextContainerIfPossible");
			toil.debugName = "Jump carry if possible";
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				if (actor.carryTracker.CarriedThing == null)
				{
					return;
				}
				if (curJob.targetQueueB != null && curJob.targetQueueB.Count > 0)
				{
					Thing nextTarget;
					if (!Toils_Haul.TryGetNextDestinationFromQueue(primaryTargetInd, TargetIndex.B, actor.carryTracker.CarriedThing.def, curJob, actor, out nextTarget))
					{
						return;
					}
					curJob.targetQueueB.RemoveAll((LocalTargetInfo target) => target.Thing == nextTarget);
					curJob.targetB = nextTarget;
					curJob.targetC = nextTarget;
					actor.jobs.curDriver.JumpToToil(carryToContainerToil);
				}
			};
			return toil;
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x001A1344 File Offset: 0x0019F544
		public static bool TryGetNextDestinationFromQueue(TargetIndex primaryIndex, TargetIndex destIndex, ThingDef stuff, Job job, Pawn actor, out Thing target)
		{
			Toils_Haul.<>c__DisplayClass12_0 CS$<>8__locals1 = new Toils_Haul.<>c__DisplayClass12_0();
			CS$<>8__locals1.stuff = stuff;
			CS$<>8__locals1.actor = actor;
			CS$<>8__locals1.primaryTarget = job.GetTarget(primaryIndex).Thing;
			target = null;
			Pawn_CarryTracker carryTracker = CS$<>8__locals1.actor.carryTracker;
			if (((carryTracker != null) ? carryTracker.CarriedThing : null) == null)
			{
				return false;
			}
			CS$<>8__locals1.hasSpareItems = (CS$<>8__locals1.actor.carryTracker.CarriedThing.stackCount > 0);
			if (CS$<>8__locals1.primaryTarget != null && CS$<>8__locals1.primaryTarget.Spawned)
			{
				IHaulEnroute haulEnroute = CS$<>8__locals1.primaryTarget as IHaulEnroute;
				if (haulEnroute != null)
				{
					int spaceRemainingWithEnroute = haulEnroute.GetSpaceRemainingWithEnroute(CS$<>8__locals1.stuff, CS$<>8__locals1.actor);
					CS$<>8__locals1.hasSpareItems = (CS$<>8__locals1.actor.carryTracker.CarriedThing.stackCount > spaceRemainingWithEnroute);
				}
			}
			target = GenClosest.ClosestThing_Global_Reachable(CS$<>8__locals1.actor.Position, CS$<>8__locals1.actor.Map, from x in job.GetTargetQueue(destIndex)
			select x.Thing, PathEndMode.Touch, TraverseParms.For(CS$<>8__locals1.actor, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 99999f, new Predicate<Thing>(CS$<>8__locals1.<TryGetNextDestinationFromQueue>g__Validator|0), null, false);
			return target != null;
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x001A1481 File Offset: 0x0019F681
		public static Toil TakeToInventory(TargetIndex ind, int count)
		{
			return Toils_Haul.TakeToInventory(ind, new int?(count), null, null);
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x001A1494 File Offset: 0x0019F694
		private static Toil TakeToInventory(TargetIndex ind, int? count, Func<int> countGetter, Func<Thing, int> countGetterPassingThing)
		{
			Toil takeThing = ToilMaker.MakeToil("TakeToInventory");
			takeThing.initAction = delegate()
			{
				Pawn actor = takeThing.actor;
				Thing thing = actor.CurJob.GetTarget(ind).Thing;
				if (Toils_Haul.ErrorCheckForCarry(actor, thing, false))
				{
					return;
				}
				int? count2 = count;
				int a;
				if (count2 == null)
				{
					Func<Thing, int> countGetterPassingThing2 = countGetterPassingThing;
					a = ((countGetterPassingThing2 != null) ? countGetterPassingThing2(thing) : countGetter());
				}
				else
				{
					a = count2.GetValueOrDefault();
				}
				int num = Mathf.Min(a, thing.stackCount);
				if (actor.CurJob.checkEncumbrance)
				{
					num = Math.Min(num, MassUtility.CountToPickUpUntilOverEncumbered(actor, thing));
				}
				if (num <= 0)
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.inventory.GetDirectlyHeldThings().TryAdd(thing.SplitOff(num), true);
				if (thing.def.ingestible != null && thing.def.ingestible.preferability <= FoodPreferability.RawTasty)
				{
					actor.mindState.lastInventoryRawFoodUseTick = Find.TickManager.TicksGame;
				}
				thing.def.soundPickup.PlayOneShot(new TargetInfo(actor.Position, actor.Map, false));
			};
			return takeThing;
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x001A14F0 File Offset: 0x0019F6F0
		public static Toil TakeToInventory(TargetIndex ind, Func<int> countGetter)
		{
			return Toils_Haul.TakeToInventory(ind, null, countGetter, null);
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x001A1510 File Offset: 0x0019F710
		public static Toil TakeToInventory(TargetIndex ind, Func<Thing, int> countGetter)
		{
			return Toils_Haul.TakeToInventory(ind, null, null, countGetter);
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x001A1530 File Offset: 0x0019F730
		public static Toil TakeFromOtherInventory(Thing item, ThingOwner taker, ThingOwner holder, int count = -1, TargetIndex indexToSet = TargetIndex.None)
		{
			Toil toil = ToilMaker.MakeToil("TakeFromOtherInventory");
			toil.initAction = delegate()
			{
				if (!holder.Contains(item))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				count = ((count < 0) ? toil.actor.jobs.curJob.count : count);
				Thing thing;
				holder.TryTransferToContainer(item, taker, Mathf.Min(item.stackCount, count), out thing, true);
				if (thing == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Taker ");
					defaultInterpolatedStringHandler.AppendFormatted(toil.actor.Label);
					defaultInterpolatedStringHandler.AppendLiteral(" unable to take count ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(count);
					defaultInterpolatedStringHandler.AppendLiteral(" of thing ");
					defaultInterpolatedStringHandler.AppendFormatted(item.Label);
					defaultInterpolatedStringHandler.AppendLiteral(" from holder's inventory");
					Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				if (indexToSet != TargetIndex.None)
				{
					toil.actor.jobs.curJob.SetTarget(indexToSet, thing);
				}
			};
			return toil;
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x001A1594 File Offset: 0x0019F794
		public static Toil CheckItemCarriedByOtherPawn(Thing item, TargetIndex targetPawnIfCarried = TargetIndex.None, Toil jumpIfCarriedByOther = null)
		{
			Toil toil = ToilMaker.MakeToil("CheckItemCarriedByOtherPawn");
			toil.initAction = delegate()
			{
				Thing item2 = item;
				Pawn_InventoryTracker pawn_InventoryTracker = ((item2 != null) ? item2.ParentHolder : null) as Pawn_InventoryTracker;
				Pawn pawn = (pawn_InventoryTracker != null) ? pawn_InventoryTracker.pawn : null;
				if (pawn != null && pawn != toil.actor)
				{
					if (targetPawnIfCarried != TargetIndex.None)
					{
						toil.actor.jobs.curJob.SetTarget(targetPawnIfCarried, pawn);
					}
					if (jumpIfCarriedByOther != null)
					{
						toil.actor.jobs.curDriver.JumpToToil(jumpIfCarriedByOther);
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}
	}
}
