using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B4 RID: 5300
	public static class MetalbloodUtility
	{
		// Token: 0x060081B9 RID: 33209 RVA: 0x002E4918 File Offset: 0x002E2B18
		public static bool HasMetalblood(this Pawn pawn)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def == HediffDefOf.Metalblood)
				{
					return true;
				}
			}
			return false;
		}
	}
}
