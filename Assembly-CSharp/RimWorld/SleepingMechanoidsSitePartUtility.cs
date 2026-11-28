using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200177C RID: 6012
	public static class SleepingMechanoidsSitePartUtility
	{
		// Token: 0x0600959A RID: 38298 RVA: 0x003664BF File Offset: 0x003646BF
		public static int GetPawnGroupMakerSeed(SitePartParams parms)
		{
			return parms.randomValue;
		}
	}
}
