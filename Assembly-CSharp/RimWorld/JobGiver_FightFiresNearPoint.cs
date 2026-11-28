using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B16 RID: 2838
	internal class JobGiver_FightFiresNearPoint : ThinkNode_JobGiver
	{
		// Token: 0x060053F8 RID: 21496 RVA: 0x001F3F1D File Offset: 0x001F211D
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FightFiresNearPoint jobGiver_FightFiresNearPoint = (JobGiver_FightFiresNearPoint)base.DeepCopy(resolve);
			jobGiver_FightFiresNearPoint.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_FightFiresNearPoint;
		}

		// Token: 0x060053F9 RID: 21497 RVA: 0x001F3F38 File Offset: 0x001F2138
		protected override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = GenClosest.ClosestThingReachable(pawn.GetLord().CurLordToil.FlagLoc, pawn.Map, ThingRequest.ForDef(ThingDefOf.Fire), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), this.maxDistFromPoint, JobGiver_FightFiresNearPoint.FireValidator(pawn), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing != null)
			{
				return JobMaker.MakeJob(JobDefOf.BeatFire, thing);
			}
			return null;
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x001F3FA1 File Offset: 0x001F21A1
		public static Predicate<Thing> FireValidator(Pawn pawn)
		{
			return (Thing t) => !(((AttachableThing)t).parent is Pawn) && pawn.CanReserve(t, 1, -1, null, false) && !pawn.WorkTagIsDisabled(WorkTags.Firefighting);
		}

		// Token: 0x04003249 RID: 12873
		public float maxDistFromPoint = -1f;
	}
}
