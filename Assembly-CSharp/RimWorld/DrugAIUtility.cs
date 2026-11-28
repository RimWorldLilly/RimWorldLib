using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AC8 RID: 2760
	public static class DrugAIUtility
	{
		// Token: 0x060052D0 RID: 21200 RVA: 0x001EDD9C File Offset: 0x001EBF9C
		public static Job IngestAndTakeToInventoryJob(Thing drug, Pawn pawn, int maxNumToCarry = 9999)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, drug);
			job.count = Mathf.Min(new int[]
			{
				drug.stackCount,
				drug.def.ingestible.defaultNumToIngestAtOnce,
				maxNumToCarry
			});
			Pawn_DrugPolicyTracker drugs = pawn.drugs;
			DrugPolicy drugPolicy = (drugs != null) ? drugs.CurrentPolicy : null;
			if (drugPolicy != null)
			{
				DrugPolicyEntry drugPolicyEntry = drugPolicy[drug.def];
				int num = pawn.inventory.innerContainer.TotalStackCountOfDef(drug.def) - job.count;
				if (drugPolicyEntry.allowScheduled && num <= 0)
				{
					job.takeExtraIngestibles = drugPolicyEntry.takeToInventory;
				}
			}
			return job;
		}
	}
}
