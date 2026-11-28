using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C3D RID: 3133
	public class WorkGiver_Researcher : WorkGiver_Scanner
	{
		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x06005925 RID: 22821 RVA: 0x0020E066 File Offset: 0x0020C266
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				if (Find.ResearchManager.GetProject(null) == null)
				{
					return ThingRequest.ForGroup(ThingRequestGroup.Nothing);
				}
				return ThingRequest.ForGroup(ThingRequestGroup.ResearchBench);
			}
		}

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x06005926 RID: 22822 RVA: 0x0020E083 File Offset: 0x0020C283
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005927 RID: 22823 RVA: 0x0020E086 File Offset: 0x0020C286
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return Find.ResearchManager.GetProject(null) == null;
		}

		// Token: 0x06005928 RID: 22824 RVA: 0x0020E098 File Offset: 0x0020C298
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			ResearchProjectDef project = Find.ResearchManager.GetProject(null);
			if (project == null)
			{
				return false;
			}
			Building_ResearchBench building_ResearchBench = t as Building_ResearchBench;
			return building_ResearchBench != null && project.CanBeResearchedAt(building_ResearchBench, false) && pawn.CanReserve(t, 1, -1, null, forced) && (!t.def.hasInteractionCell || pawn.CanReserveSittableOrSpot(t.InteractionCell, forced)) && new HistoryEvent(HistoryEventDefOf.Researching, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job();
		}

		// Token: 0x06005929 RID: 22825 RVA: 0x0020E11D File Offset: 0x0020C31D
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Research, t);
		}

		// Token: 0x0600592A RID: 22826 RVA: 0x0020E12F File Offset: 0x0020C32F
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			return t.Thing.GetStatValue(StatDefOf.ResearchSpeedFactor, true, -1);
		}
	}
}
