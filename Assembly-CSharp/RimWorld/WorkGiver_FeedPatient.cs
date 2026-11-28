using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C0A RID: 3082
	public class WorkGiver_FeedPatient : WorkGiver_Scanner
	{
		// Token: 0x17000F62 RID: 3938
		// (get) Token: 0x06005808 RID: 22536 RVA: 0x002094BC File Offset: 0x002076BC
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06005809 RID: 22537 RVA: 0x002094C5 File Offset: 0x002076C5
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x0600580A RID: 22538 RVA: 0x002094C8 File Offset: 0x002076C8
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600580B RID: 22539 RVA: 0x002094CB File Offset: 0x002076CB
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedHungryPawns;
		}

		// Token: 0x0600580C RID: 22540 RVA: 0x002094E0 File Offset: 0x002076E0
		private bool TryFindBestFoodSourceFor(Pawn pawn, Pawn patient, out Thing foodSource, out ThingDef foodDef)
		{
			return FoodUtility.TryFindBestFoodSourceFor(pawn, patient, patient.needs.food.CurCategory == HungerCategory.Starving, out foodSource, out foodDef, false, true, true, false, true, false, false, false, false, false, true, FoodPreferability.Undefined);
		}

		// Token: 0x0600580D RID: 22541 RVA: 0x00209518 File Offset: 0x00207718
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn2 == pawn)
			{
				return false;
			}
			if (this.def.feedHumanlikesOnly && !pawn2.RaceProps.Humanlike)
			{
				return false;
			}
			if (this.def.feedAnimalsOnly && !pawn2.IsAnimal)
			{
				return false;
			}
			if (pawn2.DevelopmentalStage.Baby())
			{
				return false;
			}
			if (!FeedPatientUtility.IsHungry(pawn2))
			{
				return false;
			}
			if (!FeedPatientUtility.ShouldBeFed(pawn2))
			{
				return false;
			}
			if (WardenFeedUtility.ShouldBeFed(pawn2) && !pawn.IsColonyMech)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn2.foodRestriction != null)
			{
				FoodPolicy currentRespectedRestriction = pawn2.foodRestriction.GetCurrentRespectedRestriction(pawn);
				if (currentRespectedRestriction != null && currentRespectedRestriction.filter.AllowedDefCount == 0)
				{
					JobFailReason.Is("NoFoodMatchingRestrictions".Translate(), null);
					return false;
				}
			}
			Thing thing;
			ThingDef thingDef;
			if (!this.TryFindBestFoodSourceFor(pawn, pawn2, out thing, out thingDef))
			{
				JobFailReason.Is("NoFood".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x0600580E RID: 22542 RVA: 0x00209614 File Offset: 0x00207814
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Thing thing;
			ThingDef thingDef;
			if (this.TryFindBestFoodSourceFor(pawn, pawn2, out thing, out thingDef))
			{
				float nutrition = FoodUtility.GetNutrition(pawn2, thing, thingDef);
				Job job = JobMaker.MakeJob(JobDefOf.FeedPatient);
				job.targetA = thing;
				job.targetB = pawn2;
				job.count = FoodUtility.WillIngestStackCountOf(pawn2, thingDef, nutrition);
				return job;
			}
			return null;
		}

		// Token: 0x0600580F RID: 22543 RVA: 0x00209674 File Offset: 0x00207874
		public override string JobInfo(Pawn pawn, Job job)
		{
			if (FoodUtility.MoodFromIngesting((Pawn)((Thing)job.targetB), job.targetA.Thing, FoodUtility.GetFinalIngestibleDef(job.targetA.Thing, false)) < 0f)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("WarningFoodDisliked".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(")");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			return string.Empty;
		}
	}
}
