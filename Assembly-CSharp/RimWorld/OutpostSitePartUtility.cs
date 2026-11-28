using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02001779 RID: 6009
	public static class OutpostSitePartUtility
	{
		// Token: 0x0600958E RID: 38286 RVA: 0x003661A3 File Offset: 0x003643A3
		public static int GetPawnGroupMakerSeed(SitePartParams parms)
		{
			return parms.randomValue;
		}
	}
}
