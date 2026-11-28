using System;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000814 RID: 2068
	public static class EnrouteUtility
	{
		// Token: 0x06004118 RID: 16664 RVA: 0x0019DA00 File Offset: 0x0019BC00
		public static int GetSpaceRemainingWithEnroute(this IHaulEnroute enroute, ThingDef stuff, Pawn excludeEnrouteFor = null)
		{
			int enroute2 = enroute.Map.enrouteManager.GetEnroute(enroute, stuff, excludeEnrouteFor);
			return Mathf.Max(enroute.SpaceRemainingFor(stuff) - enroute2, 0);
		}
	}
}
