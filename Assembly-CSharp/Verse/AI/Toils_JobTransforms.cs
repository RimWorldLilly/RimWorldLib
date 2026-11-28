using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000843 RID: 2115
	public static class Toils_JobTransforms
	{
		// Token: 0x06004289 RID: 17033 RVA: 0x001A3F80 File Offset: 0x001A2180
		public static Toil ExtractNextTargetFromQueue(TargetIndex ind, bool failIfCountFromQueueTooBig = true)
		{
			Toil toil = ToilMaker.MakeToil("ExtractNextTargetFromQueue");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
				if (targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					return;
				}
				if (failIfCountFromQueueTooBig && !curJob.countQueue.NullOrEmpty<int>() && targetQueue[0].HasThing && curJob.countQueue[0] > targetQueue[0].Thing.stackCount)
				{
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
					return;
				}
				curJob.SetTarget(ind, targetQueue[0]);
				targetQueue.RemoveAt(0);
				if (!curJob.countQueue.NullOrEmpty<int>())
				{
					curJob.count = curJob.countQueue[0];
					curJob.countQueue.RemoveAt(0);
				}
			};
			return toil;
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x001A3FD0 File Offset: 0x001A21D0
		public static Toil ClearQueue(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("ClearQueue");
			toil.initAction = delegate()
			{
				List<LocalTargetInfo> targetQueue = toil.actor.jobs.curJob.GetTargetQueue(ind);
				if (targetQueue.NullOrEmpty<LocalTargetInfo>())
				{
					return;
				}
				targetQueue.Clear();
			};
			return toil;
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x001A4018 File Offset: 0x001A2218
		public static Toil ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex ind, Func<Thing, bool> validator = null)
		{
			Toil toil = ToilMaker.MakeToil("ClearDespawnedNullOrForbiddenQueuedTargets");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				actor.jobs.curJob.GetTargetQueue(ind).RemoveAll((LocalTargetInfo ta) => !ta.HasThing || !ta.Thing.Spawned || ta.Thing.IsForbidden(actor) || (validator != null && !validator(ta.Thing)));
			};
			return toil;
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x001A4066 File Offset: 0x001A2266
		private static IEnumerable<IntVec3> IngredientPlaceCellsInOrder(Thing destination)
		{
			Toils_JobTransforms.<IngredientPlaceCellsInOrder>d__4 <IngredientPlaceCellsInOrder>d__ = new Toils_JobTransforms.<IngredientPlaceCellsInOrder>d__4(-2);
			<IngredientPlaceCellsInOrder>d__.<>3__destination = destination;
			return <IngredientPlaceCellsInOrder>d__;
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x001A4078 File Offset: 0x001A2278
		public static Toil SetTargetToIngredientPlaceCell(TargetIndex facilityInd, TargetIndex carryItemInd, TargetIndex cellTargetInd)
		{
			Toil toil = ToilMaker.MakeToil("SetTargetToIngredientPlaceCell");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(carryItemInd).Thing;
				IntVec3 c = IntVec3.Invalid;
				foreach (IntVec3 intVec in Toils_JobTransforms.IngredientPlaceCellsInOrder(curJob.GetTarget(facilityInd).Thing))
				{
					if (GenSpawn.CanSpawnAt(thing.def, intVec, actor.Map, null, true))
					{
						if (!c.IsValid)
						{
							c = intVec;
						}
						bool flag = false;
						List<Thing> list = actor.Map.thingGrid.ThingsListAt(intVec);
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i].def.category == ThingCategory.Item && (!list[i].CanStackWith(thing) || list[i].stackCount == list[i].def.stackLimit))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							curJob.SetTarget(cellTargetInd, intVec);
							return;
						}
					}
				}
				curJob.SetTarget(cellTargetInd, c);
			};
			return toil;
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x001A40D0 File Offset: 0x001A22D0
		public static Toil MoveCurrentTargetIntoQueue(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("MoveCurrentTargetIntoQueue");
			toil.initAction = delegate()
			{
				Job curJob = toil.actor.CurJob;
				LocalTargetInfo target = curJob.GetTarget(ind);
				if (target.IsValid)
				{
					List<LocalTargetInfo> targetQueue = curJob.GetTargetQueue(ind);
					if (targetQueue == null)
					{
						curJob.AddQueuedTarget(ind, target);
					}
					else
					{
						targetQueue.Insert(0, target);
					}
					curJob.SetTarget(ind, null);
				}
			};
			return toil;
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x001A4117 File Offset: 0x001A2317
		public static Toil SucceedOnNoTargetInQueue(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("SucceedOnNoTargetInQueue");
			toil.EndOnNoTargetInQueue(ind, JobCondition.Succeeded);
			return toil;
		}

		// Token: 0x04002B69 RID: 11113
		private static List<IntVec3> yieldedIngPlaceCells = new List<IntVec3>();
	}
}
