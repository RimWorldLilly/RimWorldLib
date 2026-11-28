using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A42 RID: 2626
	public static class HuntJobUtility
	{
		// Token: 0x06004F51 RID: 20305 RVA: 0x001E08B8 File Offset: 0x001DEAB8
		public static bool WasKilledByHunter(Pawn pawn, DamageInfo? dinfo)
		{
			if (dinfo == null)
			{
				return false;
			}
			Pawn pawn2 = dinfo.Value.Instigator as Pawn;
			if (pawn2 == null || pawn2.CurJob == null)
			{
				return false;
			}
			JobDriver_Hunt jobDriver_Hunt = pawn2.jobs.curDriver as JobDriver_Hunt;
			return jobDriver_Hunt != null && jobDriver_Hunt.Victim == pawn;
		}
	}
}
