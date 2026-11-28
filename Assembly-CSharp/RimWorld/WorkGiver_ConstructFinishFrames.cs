using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BC8 RID: 3016
	public class WorkGiver_ConstructFinishFrames : WorkGiver_Scanner
	{
		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x060056D9 RID: 22233 RVA: 0x002045CC File Offset: 0x002027CC
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060056DA RID: 22234 RVA: 0x002045CF File Offset: 0x002027CF
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x060056DB RID: 22235 RVA: 0x002045D2 File Offset: 0x002027D2
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		// Token: 0x060056DC RID: 22236 RVA: 0x002045DC File Offset: 0x002027DC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return null;
			}
			Frame frame = t as Frame;
			if (frame == null)
			{
				return null;
			}
			if (!frame.IsCompleted())
			{
				return null;
			}
			if (!GenConstruct.CanTouchTargetFromValidCell(frame, pawn))
			{
				return null;
			}
			if (GenConstruct.FirstBlockingThing(frame, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(frame, pawn, forced);
			}
			if (!GenConstruct.CanConstruct(frame, pawn, true, forced, null))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.FinishFrame, frame);
		}
	}
}
