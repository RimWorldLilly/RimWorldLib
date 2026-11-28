using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001611 RID: 5649
	public static class KidnapUtility
	{
		// Token: 0x06008CC4 RID: 36036 RVA: 0x00332624 File Offset: 0x00330824
		public static bool IsKidnapped(this Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading.Contains(pawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
