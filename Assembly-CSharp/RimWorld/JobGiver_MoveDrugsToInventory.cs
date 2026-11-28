using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AC7 RID: 2759
	public class JobGiver_MoveDrugsToInventory : ThinkNode_JobGiver
	{
		// Token: 0x060052CB RID: 21195 RVA: 0x001EDBB4 File Offset: 0x001EBDB4
		public override float GetPriority(Pawn pawn)
		{
			Pawn_DrugPolicyTracker drugs = pawn.drugs;
			DrugPolicy drugPolicy = (drugs != null) ? drugs.CurrentPolicy : null;
			if (drugPolicy == null)
			{
				return 0f;
			}
			for (int i = 0; i < drugPolicy.Count; i++)
			{
				if (pawn.drugs.AllowedToTakeToInventory(drugPolicy[i].drug))
				{
					return 7.5f;
				}
			}
			return 0f;
		}

		// Token: 0x060052CC RID: 21196 RVA: 0x001EDC14 File Offset: 0x001EBE14
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn_DrugPolicyTracker drugs = pawn.drugs;
			DrugPolicy drugPolicy = (drugs != null) ? drugs.CurrentPolicy : null;
			if (drugPolicy == null)
			{
				return null;
			}
			for (int i = 0; i < drugPolicy.Count; i++)
			{
				if (pawn.drugs.AllowedToTakeToInventory(drugPolicy[i].drug))
				{
					int num = drugPolicy[i].takeToInventory - pawn.inventory.innerContainer.TotalStackCountOfDef(drugPolicy[i].drug);
					Thing thing = this.FindDrugFor(pawn, drugPolicy[i].drug, num);
					if (thing != null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, thing);
						job.count = Mathf.Min(num, thing.stackCount);
						return job;
					}
				}
			}
			return null;
		}

		// Token: 0x060052CD RID: 21197 RVA: 0x001EDCD0 File Offset: 0x001EBED0
		private Thing FindDrugFor(Pawn pawn, ThingDef drugDef, int desired)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drugDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => this.DrugValidator(pawn, x, desired), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x060052CE RID: 21198 RVA: 0x001EDD44 File Offset: 0x001EBF44
		private bool DrugValidator(Pawn pawn, Thing drug, int desired)
		{
			if (!drug.def.IsDrug)
			{
				return false;
			}
			if (drug.Spawned)
			{
				if (drug.IsForbidden(pawn))
				{
					return false;
				}
				if (!pawn.CanReserve(drug, 10, Mathf.Min(drug.stackCount, desired), null, false))
				{
					return false;
				}
			}
			return true;
		}
	}
}
