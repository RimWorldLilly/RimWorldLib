using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011BF RID: 4543
	public class IncidentWorker_WanderersSkylanterns : IncidentWorker_VisitorGroup
	{
		// Token: 0x06007180 RID: 29056 RVA: 0x0027C6EF File Offset: 0x0027A8EF
		protected override LordJob_VisitColony CreateLordJob(IncidentParms parms, List<Pawn> pawns)
		{
			LordJob_VisitColony lordJob_VisitColony = base.CreateLordJob(parms, pawns);
			lordJob_VisitColony.gifts = parms.gifts;
			parms.gifts = null;
			return lordJob_VisitColony;
		}

		// Token: 0x06007181 RID: 29057 RVA: 0x0027C70C File Offset: 0x0027A90C
		protected override void SendLetter(IncidentParms parms, List<Pawn> pawns, Pawn leader, bool traderExists)
		{
			base.SendStandardLetter(parms, pawns, Array.Empty<NamedArgument>());
		}
	}
}
