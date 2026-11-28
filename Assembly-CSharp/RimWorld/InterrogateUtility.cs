using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200151D RID: 5405
	public static class InterrogateUtility
	{
		// Token: 0x06008455 RID: 33877 RVA: 0x002F9CB6 File Offset: 0x002F7EB6
		public static float GetChance(Pawn pawn)
		{
			return pawn.GetStatValue(StatDefOf.SocialImpact, true, -1) * 0.15f;
		}

		// Token: 0x040051BA RID: 20922
		private const float SocialFactor = 0.15f;
	}
}
