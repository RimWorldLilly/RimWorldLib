using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C1A RID: 3098
	public class WorkGiver_HaulResourcesToCarrier : WorkGiver_Scanner
	{
		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x06005858 RID: 22616 RVA: 0x0020AC90 File Offset: 0x00208E90
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06005859 RID: 22617 RVA: 0x0020AC99 File Offset: 0x00208E99
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x0600585A RID: 22618 RVA: 0x0020ACB4 File Offset: 0x00208EB4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckBiotech("Haul resources to carrier"))
			{
				return false;
			}
			Pawn pawn2 = (Pawn)t;
			if (!pawn2.IsColonyMech || !pawn2.Spawned || pawn2.Downed)
			{
				return false;
			}
			CompMechCarrier comp = pawn2.GetComp<CompMechCarrier>();
			if (comp == null)
			{
				return false;
			}
			int amountToAutofill = comp.AmountToAutofill;
			return amountToAutofill > 0 && pawn.CanReserve(t, 1, -1, null, forced) && !HaulAIUtility.FindFixedIngredientCount(pawn, comp.Props.fixedIngredient, amountToAutofill).NullOrEmpty<Thing>();
		}

		// Token: 0x0600585B RID: 22619 RVA: 0x0020AD38 File Offset: 0x00208F38
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompMechCarrier compMechCarrier = t.TryGetComp<CompMechCarrier>();
			if (compMechCarrier == null)
			{
				return null;
			}
			int amountToAutofill = compMechCarrier.AmountToAutofill;
			if (amountToAutofill <= 0)
			{
				return null;
			}
			List<Thing> list = HaulAIUtility.FindFixedIngredientCount(pawn, compMechCarrier.Props.fixedIngredient, amountToAutofill);
			if (!list.NullOrEmpty<Thing>())
			{
				Job job = HaulAIUtility.HaulToContainerJob(pawn, list[0], t);
				job.count = Mathf.Min(job.count, amountToAutofill);
				job.targetQueueB = (from i in list.Skip(1)
				select new LocalTargetInfo(i)).ToList<LocalTargetInfo>();
				return job;
			}
			return null;
		}
	}
}
