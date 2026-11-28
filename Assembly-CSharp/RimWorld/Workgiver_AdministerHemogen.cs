using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BEE RID: 3054
	public class Workgiver_AdministerHemogen : WorkGiver_Scanner
	{
		// Token: 0x17000F46 RID: 3910
		// (get) Token: 0x06005774 RID: 22388 RVA: 0x00206AA9 File Offset: 0x00204CA9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17000F47 RID: 3911
		// (get) Token: 0x06005775 RID: 22389 RVA: 0x00206AB2 File Offset: 0x00204CB2
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x00206AB5 File Offset: 0x00204CB5
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x00206AB8 File Offset: 0x00204CB8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn2 == pawn)
			{
				return false;
			}
			Pawn_GeneTracker genes = pawn2.genes;
			Gene_Hemogen gene_Hemogen = (genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null;
			if (gene_Hemogen == null)
			{
				return false;
			}
			if (gene_Hemogen.ValuePercent >= 0.95f)
			{
				return false;
			}
			if (!forced && gene_Hemogen.Value >= 0.25f)
			{
				return false;
			}
			if (!FeedPatientUtility.ShouldBeFed(pawn2))
			{
				return false;
			}
			if (!gene_Hemogen.hemogenPacksAllowed)
			{
				return false;
			}
			if (!gene_Hemogen.ShouldConsumeHemogenNow())
			{
				JobFailReason.Is("NotAllowedHemogen".Translate(), null);
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.HemogenPack), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing pack) => !pack.IsForbidden(pawn) && pawn.CanReserve(pack, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false) == null)
			{
				JobFailReason.Is("NoIngredient".Translate(ThingDefOf.HemogenPack), null);
				return false;
			}
			return true;
		}

		// Token: 0x06005778 RID: 22392 RVA: 0x00206BE0 File Offset: 0x00204DE0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn t2 = (Pawn)t;
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.HemogenPack), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing pack) => !pack.IsForbidden(pawn) && pawn.CanReserve(pack, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.FeedPatient, thing, t2);
				job.count = 1;
				return job;
			}
			return null;
		}

		// Token: 0x04003333 RID: 13107
		private const float MinLevelForFeedingHemogenUnforced = 0.25f;

		// Token: 0x04003334 RID: 13108
		private const float HemogenPctMax = 0.95f;
	}
}
