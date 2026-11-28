using System;

namespace RimWorld
{
	// Token: 0x02001EB1 RID: 7857
	[DefOf]
	public static class RoadPathingDefOf
	{
		// Token: 0x0600CA6C RID: 51820 RVA: 0x004A5FEE File Offset: 0x004A41EE
		static RoadPathingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoadPathingDefOf));
		}

		// Token: 0x04007E9E RID: 32414
		public static RoadPathingDef Avoid;

		// Token: 0x04007E9F RID: 32415
		public static RoadPathingDef Bulldoze;
	}
}
