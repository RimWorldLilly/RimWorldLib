using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B48 RID: 2888
	public class JobGiver_GetHemogen : ThinkNode_JobGiver
	{
		// Token: 0x17000F08 RID: 3848
		// (get) Token: 0x06005492 RID: 21650 RVA: 0x001F6D38 File Offset: 0x001F4F38
		public static float HemogenPackHemogenGain
		{
			get
			{
				if (JobGiver_GetHemogen.cachedHemogenPackHemogenGain == null)
				{
					if (!ModsConfig.BiotechActive)
					{
						JobGiver_GetHemogen.cachedHemogenPackHemogenGain = new float?(0f);
					}
					else
					{
						IngestibleProperties ingestible = ThingDefOf.HemogenPack.ingestible;
						object obj;
						if (ingestible == null)
						{
							obj = null;
						}
						else
						{
							List<IngestionOutcomeDoer> outcomeDoers = ingestible.outcomeDoers;
							if (outcomeDoers == null)
							{
								obj = null;
							}
							else
							{
								obj = outcomeDoers.FirstOrDefault((IngestionOutcomeDoer x) => x is IngestionOutcomeDoer_OffsetHemogen);
							}
						}
						IngestionOutcomeDoer_OffsetHemogen ingestionOutcomeDoer_OffsetHemogen = obj as IngestionOutcomeDoer_OffsetHemogen;
						if (ingestionOutcomeDoer_OffsetHemogen == null)
						{
							JobGiver_GetHemogen.cachedHemogenPackHemogenGain = new float?(0f);
						}
						else
						{
							JobGiver_GetHemogen.cachedHemogenPackHemogenGain = new float?(ingestionOutcomeDoer_OffsetHemogen.offset);
						}
					}
				}
				return JobGiver_GetHemogen.cachedHemogenPackHemogenGain.Value;
			}
		}

		// Token: 0x06005493 RID: 21651 RVA: 0x001F6DE1 File Offset: 0x001F4FE1
		public static void ResetStaticData()
		{
			JobGiver_GetHemogen.cachedHemogenPackHemogenGain = null;
		}

		// Token: 0x06005494 RID: 21652 RVA: 0x001F6DEE File Offset: 0x001F4FEE
		public override float GetPriority(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			Pawn_GeneTracker genes = pawn.genes;
			if (((genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null) == null)
			{
				return 0f;
			}
			return 9.1f;
		}

		// Token: 0x06005495 RID: 21653 RVA: 0x001F6E1C File Offset: 0x001F501C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return null;
			}
			Pawn_GeneTracker genes = pawn.genes;
			Gene_Hemogen gene_Hemogen = (genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null;
			if (gene_Hemogen == null)
			{
				return null;
			}
			if (!gene_Hemogen.ShouldConsumeHemogenNow())
			{
				return null;
			}
			if (pawn.IsBloodfeeder())
			{
				Pawn prisoner = this.GetPrisoner(pawn);
				if (prisoner != null)
				{
					return JobMaker.MakeJob(JobDefOf.PrisonerBloodfeed, prisoner);
				}
			}
			if (gene_Hemogen.hemogenPacksAllowed)
			{
				int num = Mathf.FloorToInt((gene_Hemogen.Max - gene_Hemogen.Value) / JobGiver_GetHemogen.HemogenPackHemogenGain);
				if (num > 0)
				{
					Thing hemogenPack = this.GetHemogenPack(pawn);
					if (hemogenPack != null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.Ingest, hemogenPack);
						job.count = Mathf.Min(hemogenPack.stackCount, num);
						job.ingestTotalCount = true;
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x06005496 RID: 21654 RVA: 0x001F6ED4 File Offset: 0x001F50D4
		private Thing GetHemogenPack(Pawn pawn)
		{
			Thing carriedThing = pawn.carryTracker.CarriedThing;
			if (carriedThing != null && carriedThing.def == ThingDefOf.HemogenPack)
			{
				return carriedThing;
			}
			for (int i = 0; i < pawn.inventory.innerContainer.Count; i++)
			{
				if (pawn.inventory.innerContainer[i].def == ThingDefOf.HemogenPack)
				{
					return pawn.inventory.innerContainer[i];
				}
			}
			return GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerThings.ThingsOfDef(ThingDefOf.HemogenPack), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing t) => pawn.CanReserve(t, 1, -1, null, false) && !t.IsForbidden(pawn), null, false);
		}

		// Token: 0x06005497 RID: 21655 RVA: 0x001F6FC4 File Offset: 0x001F51C4
		public static AcceptanceReport CanFeedOnPrisoner(Pawn bloodfeeder, Pawn prisoner)
		{
			if (prisoner.WouldDieFromAdditionalBloodLoss(0.4499f))
			{
				return "CannotFeedOnWouldKill".Translate(prisoner.Named("PAWN"));
			}
			if (!prisoner.IsPrisonerOfColony || !prisoner.guest.PrisonerIsSecure || prisoner.guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.Bloodfeed) || prisoner.IsForbidden(bloodfeeder) || !bloodfeeder.CanReserveAndReach(prisoner, PathEndMode.OnCell, bloodfeeder.NormalMaxDanger(), 1, -1, null, false) || prisoner.InAggroMentalState)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06005498 RID: 21656 RVA: 0x001F7058 File Offset: 0x001F5258
		private Pawn GetPrisoner(Pawn pawn)
		{
			return (Pawn)GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.mapPawns.PrisonersOfColonySpawned, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, delegate(Thing t)
			{
				Pawn pawn2 = t as Pawn;
				return pawn2 != null && JobGiver_GetHemogen.CanFeedOnPrisoner(pawn, pawn2).Accepted;
			}, null, false);
		}

		// Token: 0x0400328B RID: 12939
		private static float? cachedHemogenPackHemogenGain;
	}
}
