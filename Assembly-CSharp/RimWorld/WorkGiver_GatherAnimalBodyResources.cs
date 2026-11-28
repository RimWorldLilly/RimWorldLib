using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BB4 RID: 2996
	public abstract class WorkGiver_GatherAnimalBodyResources : WorkGiver_Scanner
	{
		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x0600566F RID: 22127
		protected abstract JobDef JobDef { get; }

		// Token: 0x06005670 RID: 22128
		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		// Token: 0x06005671 RID: 22129 RVA: 0x0020240A File Offset: 0x0020060A
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x06005672 RID: 22130 RVA: 0x00202424 File Offset: 0x00200624
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Pawn> list = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsAnimal)
				{
					CompHasGatherableBodyResource comp = this.GetComp(list[i]);
					if (comp != null && comp.ActiveAndFull)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x06005673 RID: 22131 RVA: 0x00202483 File Offset: 0x00200683
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06005674 RID: 22132 RVA: 0x00202488 File Offset: 0x00200688
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.IsAnimal)
			{
				return false;
			}
			CompHasGatherableBodyResource comp = this.GetComp(pawn2);
			return comp != null && comp.ActiveAndFull && !pawn2.Downed && (pawn2.roping == null || !pawn2.roping.IsRopedByPawn) && pawn2.CanCasuallyInteractNow(false, false, false, false) && pawn.CanReserve(pawn2, 1, -1, null, forced);
		}

		// Token: 0x06005675 RID: 22133 RVA: 0x002024FA File Offset: 0x002006FA
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(this.JobDef, t);
		}
	}
}
