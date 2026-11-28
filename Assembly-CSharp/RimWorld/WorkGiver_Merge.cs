using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C26 RID: 3110
	public class WorkGiver_Merge : WorkGiver_Scanner
	{
		// Token: 0x0600589D RID: 22685 RVA: 0x0020C02A File Offset: 0x0020A22A
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600589E RID: 22686 RVA: 0x0020C02D File Offset: 0x0020A22D
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerMergeables.ThingsPotentiallyNeedingMerging();
		}

		// Token: 0x0600589F RID: 22687 RVA: 0x0020C03F File Offset: 0x0020A23F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerMergeables.ThingsPotentiallyNeedingMerging().Count == 0;
		}

		// Token: 0x060058A0 RID: 22688 RVA: 0x0020C05C File Offset: 0x0020A25C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.stackCount == t.def.stackLimit)
			{
				return null;
			}
			if (!HaulAIUtility.PawnCanAutomaticallyHaul(pawn, t, forced))
			{
				return null;
			}
			ISlotGroup slotGroup = t.GetSlotGroup();
			ISlotGroup storageGroup = slotGroup.StorageGroup;
			slotGroup = (storageGroup ?? slotGroup);
			if (!pawn.CanReserve(t.Position, 1, -1, null, forced))
			{
				return null;
			}
			foreach (Thing thing in slotGroup.HeldThings)
			{
				if (thing != t && thing.CanStackWith(t) && (forced || thing.stackCount >= t.stackCount) && thing.stackCount < thing.def.stackLimit && !thing.IsForbidden(pawn.Faction) && thing.Position.InAllowedArea(pawn) && pawn.CanReserve(thing.Position, 1, -1, null, forced) && pawn.CanReserve(thing, 1, -1, null, false) && thing.Position.IsValidStorageFor(thing.Map, t) && !thing.Position.ContainsStaticFire(thing.Map))
				{
					Job job = JobMaker.MakeJob(JobDefOf.HaulToCell, t, thing.Position);
					job.count = Mathf.Min(thing.def.stackLimit - thing.stackCount, t.stackCount);
					job.haulMode = HaulMode.ToCellStorage;
					return job;
				}
			}
			JobFailReason.Is("NoMergeTarget".Translate(), null);
			return null;
		}
	}
}
