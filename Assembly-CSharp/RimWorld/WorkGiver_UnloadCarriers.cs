using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C53 RID: 3155
	public class WorkGiver_UnloadCarriers : WorkGiver_Scanner
	{
		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x0600598F RID: 22927 RVA: 0x0020EF74 File Offset: 0x0020D174
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06005990 RID: 22928 RVA: 0x0020EF7D File Offset: 0x0020D17D
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06005991 RID: 22929 RVA: 0x0020EF80 File Offset: 0x0020D180
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			IReadOnlyList<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].inventory.UnloadEverything)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005992 RID: 22930 RVA: 0x0020EFC5 File Offset: 0x0020D1C5
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWhoShouldHaveInventoryUnloaded;
		}

		// Token: 0x06005993 RID: 22931 RVA: 0x0020EFD7 File Offset: 0x0020D1D7
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return UnloadCarriersJobGiverUtility.HasJobOnThing(pawn, t, forced);
		}

		// Token: 0x06005994 RID: 22932 RVA: 0x0020EFE1 File Offset: 0x0020D1E1
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.UnloadInventory, t);
		}
	}
}
