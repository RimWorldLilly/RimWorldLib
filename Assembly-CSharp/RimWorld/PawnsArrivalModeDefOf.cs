using System;

namespace RimWorld
{
	// Token: 0x02001EC7 RID: 7879
	[DefOf]
	public static class PawnsArrivalModeDefOf
	{
		// Token: 0x0600CA82 RID: 51842 RVA: 0x004A6164 File Offset: 0x004A4364
		static PawnsArrivalModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnsArrivalModeDefOf));
		}

		// Token: 0x04007F3C RID: 32572
		public static PawnsArrivalModeDef EdgeWalkIn;

		// Token: 0x04007F3D RID: 32573
		public static PawnsArrivalModeDef EdgeWalkInGroups;

		// Token: 0x04007F3E RID: 32574
		public static PawnsArrivalModeDef EdgeWalkInDistributed;

		// Token: 0x04007F3F RID: 32575
		public static PawnsArrivalModeDef CenterDrop;

		// Token: 0x04007F40 RID: 32576
		public static PawnsArrivalModeDef EdgeDrop;

		// Token: 0x04007F41 RID: 32577
		public static PawnsArrivalModeDef SpecificDropDebug;

		// Token: 0x04007F42 RID: 32578
		public static PawnsArrivalModeDef EmergeFromWater;

		// Token: 0x04007F43 RID: 32579
		public static PawnsArrivalModeDef RandomDrop;

		// Token: 0x04007F44 RID: 32580
		[MayRequireAnomaly]
		public static PawnsArrivalModeDef EdgeWalkInHateChanters;

		// Token: 0x04007F45 RID: 32581
		[MayRequireAnomaly]
		public static PawnsArrivalModeDef EdgeWalkInDistributedGroups;

		// Token: 0x04007F46 RID: 32582
		[MayRequireAnomaly]
		public static PawnsArrivalModeDef EdgeWalkInDarkness;
	}
}
