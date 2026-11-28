using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200119F RID: 4511
	public class IncidentWorker_AnimaTreeSpawn : IncidentWorker_SpecialTreeSpawn
	{
		// Token: 0x17001332 RID: 4914
		// (get) Token: 0x060070D9 RID: 28889 RVA: 0x00277D09 File Offset: 0x00275F09
		protected override bool SendLetter
		{
			get
			{
				return PawnsFinder.HomeMaps_FreeColonistsSpawned.Any((Pawn c) => c.HasPsylink && MeditationFocusDefOf.Natural.CanPawnUse(c));
			}
		}
	}
}
