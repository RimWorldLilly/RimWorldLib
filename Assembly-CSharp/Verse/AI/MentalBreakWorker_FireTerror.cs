using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000858 RID: 2136
	public class MentalBreakWorker_FireTerror : MentalBreakWorker
	{
		// Token: 0x06004323 RID: 17187 RVA: 0x001A8011 File Offset: 0x001A6211
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.Spawned && base.BreakCanOccur(pawn) && ThoughtWorker_Pyrophobia.NearFire(pawn);
		}
	}
}
