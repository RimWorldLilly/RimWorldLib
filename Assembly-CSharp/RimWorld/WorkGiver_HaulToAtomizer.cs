using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C1D RID: 3101
	public class WorkGiver_HaulToAtomizer : WorkGiver_Scanner
	{
		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06005868 RID: 22632 RVA: 0x0020B0A0 File Offset: 0x002092A0
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Atomizer);
			}
		}

		// Token: 0x06005869 RID: 22633 RVA: 0x0020B0AC File Offset: 0x002092AC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckBiotech("Haul to atomizer"))
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			CompAtomizer compAtomizer = t.TryGetComp<CompAtomizer>();
			if (compAtomizer.Full)
			{
				JobFailReason.Is(HaulAIUtility.ContainerFullLowerTrans, null);
				return false;
			}
			if (!forced && !compAtomizer.AutoLoad)
			{
				return false;
			}
			if (!forced && compAtomizer.FillPercent > 0.5f)
			{
				return false;
			}
			if (HaulAIUtility.FindFixedIngredientCount(pawn, compAtomizer.Props.thingDef, compAtomizer.SpaceLeft).NullOrEmpty<Thing>())
			{
				JobFailReason.Is("NoIngredient".Translate(compAtomizer.Props.thingDef), null);
				return false;
			}
			return true;
		}

		// Token: 0x0600586A RID: 22634 RVA: 0x0020B15C File Offset: 0x0020935C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompAtomizer compAtomizer = t.TryGetComp<CompAtomizer>();
			if (compAtomizer == null)
			{
				return null;
			}
			if (compAtomizer.Full)
			{
				JobFailReason.Is(HaulAIUtility.ContainerFullLowerTrans, null);
				return null;
			}
			if (!forced && !compAtomizer.AutoLoad)
			{
				return null;
			}
			List<Thing> list = HaulAIUtility.FindFixedIngredientCount(pawn, compAtomizer.Props.thingDef, compAtomizer.SpaceLeft);
			if (list.NullOrEmpty<Thing>())
			{
				JobFailReason.Is("NoIngredient".Translate(compAtomizer.Props.thingDef), null);
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.HaulToAtomizer, t);
			job.targetQueueB = (from f in list
			select new LocalTargetInfo(f)).ToList<LocalTargetInfo>();
			job.count = compAtomizer.SpaceLeft;
			return job;
		}

		// Token: 0x0400334E RID: 13134
		private const float MaxFillPercent = 0.5f;
	}
}
