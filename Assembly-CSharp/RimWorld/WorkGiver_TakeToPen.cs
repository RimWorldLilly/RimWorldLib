using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BBA RID: 3002
	public class WorkGiver_TakeToPen : WorkGiver_InteractAnimal
	{
		// Token: 0x06005690 RID: 22160 RVA: 0x00202A22 File Offset: 0x00200C22
		public WorkGiver_TakeToPen()
		{
			this.canInteractWhileSleeping = true;
		}

		// Token: 0x06005691 RID: 22161 RVA: 0x00202A43 File Offset: 0x00200C43
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x06005692 RID: 22162 RVA: 0x00202A5C File Offset: 0x00200C5C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.IsAnimal)
			{
				return null;
			}
			bool flag = pawn2.MentalStateDef == MentalStateDefOf.Roaming;
			if (this.targetRoamingAnimals && !flag)
			{
				return null;
			}
			if (!this.targetRoamingAnimals && !flag && pawn2.MentalStateDef != null)
			{
				JobFailReason.Is("CantRopeAnimalMentalState".Translate(pawn2, pawn2.MentalStateDef.label), null);
				return null;
			}
			if (pawn2.Position.IsForbidden(pawn))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ForbiddenOutsideAllowedAreaLower".Translate().CapitalizeFirst());
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted(pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				JobFailReason.Is(defaultInterpolatedStringHandler.ToStringAndClear(), null);
				return null;
			}
			if (t.Map.designationManager.DesignationOn(t, DesignationDefOf.ReleaseAnimalToWild) != null)
			{
				return null;
			}
			Map map = pawn2.Map;
			if (this.balanceCalculatorsCachedTick != Find.TickManager.TicksGame || this.balanceCalculatorsCachedForPawn != pawn)
			{
				foreach (KeyValuePair<Map, AnimalPenBalanceCalculator> keyValuePair in this.balanceCalculatorsCached)
				{
					keyValuePair.Value.MarkDirty();
				}
				this.balanceCalculatorsCachedTick = Find.TickManager.TicksGame;
				this.balanceCalculatorsCachedForPawn = pawn;
			}
			AnimalPenBalanceCalculator animalPenBalanceCalculator;
			if (this.balanceCalculatorsCached.ContainsKey(map))
			{
				animalPenBalanceCalculator = this.balanceCalculatorsCached[map];
			}
			else
			{
				animalPenBalanceCalculator = new AnimalPenBalanceCalculator(map, true);
				this.balanceCalculatorsCached.Add(map, animalPenBalanceCalculator);
			}
			string text = null;
			CompAnimalPenMarker penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(pawn, pawn2, out text, forced, this.canInteractWhileSleeping, false, true, this.ropingPriority, animalPenBalanceCalculator);
			if (penAnimalShouldBeTakenTo != null)
			{
				Job job = WorkGiver_TakeToPen.MakeJob(pawn, pawn2, penAnimalShouldBeTakenTo, false, this.ropingPriority, out text);
				if (job != null)
				{
					return job;
				}
			}
			string text2 = null;
			Building hitchingPostAnimalShouldBeTakenTo = AnimalPenUtility.GetHitchingPostAnimalShouldBeTakenTo(pawn, pawn2, out text2, forced);
			if (hitchingPostAnimalShouldBeTakenTo != null)
			{
				return JobMaker.MakeJob(JobDefOf.RopeRoamerToHitchingPost, pawn2, hitchingPostAnimalShouldBeTakenTo);
			}
			if (this.allowUnenclosedPens)
			{
				penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(pawn, pawn2, out text, forced, this.canInteractWhileSleeping, true, true, this.ropingPriority, animalPenBalanceCalculator);
				if (penAnimalShouldBeTakenTo != null)
				{
					Job job2 = WorkGiver_TakeToPen.MakeJob(pawn, pawn2, penAnimalShouldBeTakenTo, true, this.ropingPriority, out text);
					if (job2 != null)
					{
						return job2;
					}
				}
			}
			if (penAnimalShouldBeTakenTo == null && hitchingPostAnimalShouldBeTakenTo == null && AnimalPenUtility.IsUnnecessarilyRoped(pawn2))
			{
				Job job3 = this.MakeUnropeJob(pawn, pawn2, forced, out text);
				if (job3 != null)
				{
					return job3;
				}
			}
			if (text != null)
			{
				JobFailReason.Is(text, null);
			}
			else if (text2 != null)
			{
				JobFailReason.Is(text2, null);
			}
			return null;
		}

		// Token: 0x06005693 RID: 22163 RVA: 0x00202D08 File Offset: 0x00200F08
		private Job MakeUnropeJob(Pawn roper, Pawn animal, bool forced, out string jobFailReason)
		{
			jobFailReason = null;
			if (AnimalPenUtility.RopeAttachmentInteractionCell(roper, animal) == IntVec3.Invalid)
			{
				jobFailReason = "CantRopeAnimalCantTouch".Translate();
				return null;
			}
			if (!forced && !roper.CanReserve(animal, 1, -1, null, false))
			{
				return null;
			}
			if (!roper.CanReach(animal, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Unrope, animal);
		}

		// Token: 0x06005694 RID: 22164 RVA: 0x00202D7C File Offset: 0x00200F7C
		public static Job MakeJob(Pawn pawn, Pawn animal, CompAnimalPenMarker targetPenMarker, bool allowUnenclosedPens, RopingPriority ropingPriority, out string jobFailReason)
		{
			jobFailReason = null;
			IntVec3 c = AnimalPenUtility.FindPlaceInPenToStand(targetPenMarker, pawn);
			if (!c.IsValid)
			{
				jobFailReason = "CantRopeAnimalNoSpace".Translate();
				return null;
			}
			Job job = JobMaker.MakeJob(targetPenMarker.PenState.Enclosed ? JobDefOf.RopeToPen : JobDefOf.RopeRoamerToUnenclosedPen, animal, c, targetPenMarker.parent);
			job.ropingPriority = ropingPriority;
			job.ropeToUnenclosedPens = allowUnenclosedPens;
			return job;
		}

		// Token: 0x0400331A RID: 13082
		protected bool targetRoamingAnimals;

		// Token: 0x0400331B RID: 13083
		protected bool allowUnenclosedPens;

		// Token: 0x0400331C RID: 13084
		protected RopingPriority ropingPriority;

		// Token: 0x0400331D RID: 13085
		private readonly Dictionary<Map, AnimalPenBalanceCalculator> balanceCalculatorsCached = new Dictionary<Map, AnimalPenBalanceCalculator>();

		// Token: 0x0400331E RID: 13086
		private int balanceCalculatorsCachedTick = -1;

		// Token: 0x0400331F RID: 13087
		private Pawn balanceCalculatorsCachedForPawn;
	}
}
