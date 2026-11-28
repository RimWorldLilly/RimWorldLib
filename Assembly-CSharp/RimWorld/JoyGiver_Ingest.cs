using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B7D RID: 2941
	public class JoyGiver_Ingest : JoyGiver
	{
		// Token: 0x0600556B RID: 21867 RVA: 0x001FC3B7 File Offset: 0x001FA5B7
		public override Job TryGiveJob(Pawn pawn)
		{
			return this.TryGiveJobInternal(pawn, null);
		}

		// Token: 0x0600556C RID: 21868 RVA: 0x001FC3C4 File Offset: 0x001FA5C4
		public override Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatheringSpot, float maxRadius = -1f)
		{
			return this.TryGiveJobInternal(pawn, (Thing x) => !x.Spawned || (GatheringsUtility.InGatheringArea(x.Position, gatheringSpot, pawn.Map) && (maxRadius < 0f || x.Position.InHorDistOf(gatheringSpot, maxRadius))));
		}

		// Token: 0x0600556D RID: 21869 RVA: 0x001FC404 File Offset: 0x001FA604
		private Job TryGiveJobInternal(Pawn pawn, Predicate<Thing> extraValidator)
		{
			Thing thing = this.BestIngestItem(pawn, extraValidator);
			if (thing == null)
			{
				return null;
			}
			return this.CreateIngestJob(thing, pawn);
		}

		// Token: 0x0600556E RID: 21870 RVA: 0x001FC428 File Offset: 0x001FA628
		protected virtual Thing BestIngestItem(Pawn pawn, Predicate<Thing> extraValidator)
		{
			JoyGiver_Ingest.<>c__DisplayClass4_0 CS$<>8__locals1 = new JoyGiver_Ingest.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.extraValidator = extraValidator;
			ThingOwner<Thing> innerContainer = CS$<>8__locals1.pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (this.SearchSetWouldInclude(innerContainer[i]) && CS$<>8__locals1.<BestIngestItem>g__Validator|0(innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			JoyGiver_Ingest.tmpCandidates.Clear();
			this.GetSearchSet(CS$<>8__locals1.pawn, JoyGiver_Ingest.tmpCandidates);
			if (JoyGiver_Ingest.tmpCandidates.Count == 0)
			{
				return null;
			}
			Thing result = GenClosest.ClosestThing_Global_Reachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, JoyGiver_Ingest.tmpCandidates, PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<BestIngestItem>g__Validator|0), null, false);
			JoyGiver_Ingest.tmpCandidates.Clear();
			return result;
		}

		// Token: 0x0600556F RID: 21871 RVA: 0x001FC510 File Offset: 0x001FA710
		protected virtual bool CanIngestForJoy(Pawn pawn, Thing t)
		{
			if (!t.def.IsIngestible || t.def.ingestible.joyKind == null || t.def.ingestible.joy <= 0f || !pawn.WillEat(t, null, true, false))
			{
				return false;
			}
			if (t.Spawned)
			{
				if (t.Fogged())
				{
					return false;
				}
				if (!pawn.CanReserve(t, 1, -1, null, false))
				{
					return false;
				}
				if (t.IsForbidden(pawn))
				{
					return false;
				}
				if (!t.IsSociallyProper(pawn))
				{
					return false;
				}
				if (!t.IsPoliticallyProper(pawn))
				{
					return false;
				}
				if (t.Position.VacuumConcernTo(pawn))
				{
					return false;
				}
			}
			return !t.def.IsDrug || pawn.drugs == null || pawn.drugs.CurrentPolicy[t.def].allowedForJoy || pawn.story == null || pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0 || pawn.InMentalState;
		}

		// Token: 0x06005570 RID: 21872 RVA: 0x001FC615 File Offset: 0x001FA815
		protected virtual bool SearchSetWouldInclude(Thing thing)
		{
			return this.def.thingDefs != null && this.def.thingDefs.Contains(thing.def);
		}

		// Token: 0x06005571 RID: 21873 RVA: 0x001FC63C File Offset: 0x001FA83C
		protected virtual Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, ingestible);
			job.count = ingestible.stackCount;
			if (ingestible.def.ingestible.maxNumToIngestAtOnce > 0)
			{
				job.count = Math.Min(job.count, ingestible.def.ingestible.maxNumToIngestAtOnce);
			}
			return job;
		}

		// Token: 0x040032DD RID: 13021
		private static readonly List<Thing> tmpCandidates = new List<Thing>();
	}
}
