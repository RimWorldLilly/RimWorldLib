using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000841 RID: 2113
	public static class Toils_General
	{
		// Token: 0x0600427D RID: 17021 RVA: 0x001A3B9C File Offset: 0x001A1D9C
		public static Toil StopDead()
		{
			Toil toil = ToilMaker.MakeToil("StopDead");
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x0600427E RID: 17022 RVA: 0x001A3BE8 File Offset: 0x001A1DE8
		public static Toil Wait(int ticks, TargetIndex face = TargetIndex.None)
		{
			Toil toil = ToilMaker.MakeToil("Wait");
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = ticks;
			if (face != TargetIndex.None)
			{
				toil.handlingFacing = true;
				toil.tickIntervalAction = delegate(int delta)
				{
					toil.actor.rotationTracker.FaceTarget(toil.actor.CurJob.GetTarget(face));
				};
			}
			return toil;
		}

		// Token: 0x0600427F RID: 17023 RVA: 0x001A3C74 File Offset: 0x001A1E74
		public static Toil WaitWith(TargetIndex targetInd, int ticks, bool useProgressBar = false, bool maintainPosture = false, bool maintainSleep = false, TargetIndex face = TargetIndex.None, PathEndMode pathEndMode = PathEndMode.Touch)
		{
			Toil toil = ToilMaker.MakeToil("WaitWith");
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
				Pawn pawn = toil.actor.CurJob.GetTarget(targetInd).Thing as Pawn;
				if (pawn != null)
				{
					if (pawn == toil.actor)
					{
						Log.Warning("Executing WaitWith toil but otherPawn is the same as toil.actor");
						return;
					}
					PawnUtility.ForceWait(pawn, ticks, null, maintainPosture, maintainSleep);
				}
			};
			toil.FailOnDespawnedOrNull(targetInd);
			toil.FailOnCannotTouch(targetInd, pathEndMode);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = ticks;
			if (face != TargetIndex.None)
			{
				toil.handlingFacing = true;
				toil.tickIntervalAction = delegate(int delta)
				{
					toil.actor.rotationTracker.FaceTarget(toil.actor.CurJob.GetTarget(face));
				};
			}
			if (useProgressBar)
			{
				toil.WithProgressBarToilDelay(targetInd, false, -0.5f);
			}
			return toil;
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x001A3D64 File Offset: 0x001A1F64
		public static Toil RemoveDesignationsOnThing(TargetIndex ind, DesignationDef def)
		{
			Toil toil = ToilMaker.MakeToil("RemoveDesignationsOnThing");
			toil.initAction = delegate()
			{
				toil.actor.Map.designationManager.RemoveAllDesignationsOn(toil.actor.jobs.curJob.GetTarget(ind).Thing, false);
			};
			return toil;
		}

		// Token: 0x06004281 RID: 17025 RVA: 0x001A3DAC File Offset: 0x001A1FAC
		public static Toil ClearTarget(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("ClearTarget");
			toil.initAction = delegate()
			{
				toil.GetActor().CurJob.SetTarget(ind, null);
			};
			return toil;
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x001A3DF4 File Offset: 0x001A1FF4
		public static Toil PutCarriedThingInInventory()
		{
			Toil toil = ToilMaker.MakeToil("PutCarriedThingInInventory");
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				if (actor.carryTracker.CarriedThing != null && !actor.carryTracker.innerContainer.TryTransferToContainer(actor.carryTracker.CarriedThing, actor.inventory.innerContainer, true))
				{
					Thing thing;
					actor.carryTracker.TryDropCarriedThing(actor.Position, actor.carryTracker.CarriedThing.stackCount, ThingPlaceMode.Near, out thing, null);
				}
			};
			return toil;
		}

		// Token: 0x06004283 RID: 17027 RVA: 0x001A3E34 File Offset: 0x001A2034
		public static Toil Do(Action action)
		{
			Toil toil = ToilMaker.MakeToil("Do");
			toil.initAction = action;
			return toil;
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x001A3E47 File Offset: 0x001A2047
		public static Toil DoAtomic(Action action)
		{
			Toil toil = ToilMaker.MakeToil("DoAtomic");
			toil.initAction = action;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06004285 RID: 17029 RVA: 0x001A3E64 File Offset: 0x001A2064
		public static Toil Open(TargetIndex openableInd)
		{
			Toil open = ToilMaker.MakeToil("Open");
			open.initAction = delegate()
			{
				Pawn actor = open.actor;
				Thing thing = actor.CurJob.GetTarget(openableInd).Thing;
				Designation designation = actor.Map.designationManager.DesignationOn(thing, DesignationDefOf.Open);
				if (designation != null)
				{
					designation.Delete();
				}
				IOpenable openable = (IOpenable)thing;
				if (openable.CanOpen)
				{
					openable.Open();
					actor.records.Increment(RecordDefOf.ContainersOpened);
				}
			};
			open.defaultCompleteMode = ToilCompleteMode.Instant;
			return open;
		}

		// Token: 0x06004286 RID: 17030 RVA: 0x001A3EB7 File Offset: 0x001A20B7
		public static Toil Label()
		{
			Toil toil = ToilMaker.MakeToil("Label");
			toil.atomicWithPrevious = true;
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x001A3ED4 File Offset: 0x001A20D4
		public static Toil WaitWhileExtractingContents(TargetIndex containerInd, TargetIndex contentsInd, int openTicks)
		{
			Toil extract = Toils_General.Wait(openTicks, containerInd).WithProgressBarToilDelay(containerInd, false, -0.5f).FailOnDespawnedOrNull(containerInd);
			extract.handlingFacing = true;
			extract.AddPreInitAction(delegate
			{
				Thing thing = extract.actor.CurJob.GetTarget(contentsInd).Thing;
				QuestUtility.SendQuestTargetSignals(thing.questTags, "StartedExtractingFromContainer", thing.Named("SUBJECT"));
			});
			return extract;
		}
	}
}
