using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BC7 RID: 3015
	public class WorkGiver_ConstructDeliverResourcesToFrames : WorkGiver_ConstructDeliverResources
	{
		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x060056D5 RID: 22229 RVA: 0x002044CD File Offset: 0x002026CD
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingFrame);
			}
		}

		// Token: 0x060056D6 RID: 22230 RVA: 0x002044D8 File Offset: 0x002026D8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			Frame frame = t as Frame;
			if (frame == null)
			{
				return false;
			}
			if (!GenConstruct.CanTouchTargetFromValidCell(frame, pawn))
			{
				return false;
			}
			if (GenConstruct.FirstBlockingThing(frame, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(frame, pawn, forced) != null;
			}
			return GenConstruct.CanConstruct(frame, pawn, this.def.workType, forced, JobDefOf.HaulToContainer) && GenConstruct.CanGetResources_NewTemp(frame, pawn, forced) && base.ResourceDeliverJobFor(pawn, frame, true, forced) != null;
		}

		// Token: 0x060056D7 RID: 22231 RVA: 0x00204558 File Offset: 0x00202758
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
			if (!GenConstruct.CanTouchTargetFromValidCell(frame, pawn))
			{
				return null;
			}
			if (GenConstruct.FirstBlockingThing(frame, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(frame, pawn, forced);
			}
			if (!GenConstruct.CanConstruct(frame, pawn, this.def.workType, forced, JobDefOf.HaulToContainer))
			{
				return null;
			}
			return base.ResourceDeliverJobFor(pawn, frame, true, forced);
		}
	}
}
