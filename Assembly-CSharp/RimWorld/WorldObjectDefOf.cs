using System;

namespace RimWorld
{
	// Token: 0x02001EAA RID: 7850
	[DefOf]
	public static class WorldObjectDefOf
	{
		// Token: 0x0600CA65 RID: 51813 RVA: 0x004A5F77 File Offset: 0x004A4177
		static WorldObjectDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorldObjectDefOf));
		}

		// Token: 0x04007E45 RID: 32325
		public static WorldObjectDef Caravan;

		// Token: 0x04007E46 RID: 32326
		public static WorldObjectDef Settlement;

		// Token: 0x04007E47 RID: 32327
		public static WorldObjectDef AbandonedSettlement;

		// Token: 0x04007E48 RID: 32328
		public static WorldObjectDef AbandonedCamp;

		// Token: 0x04007E49 RID: 32329
		public static WorldObjectDef EscapeShip;

		// Token: 0x04007E4A RID: 32330
		public static WorldObjectDef Ambush;

		// Token: 0x04007E4B RID: 32331
		public static WorldObjectDef DestroyedSettlement;

		// Token: 0x04007E4C RID: 32332
		public static WorldObjectDef AttackedNonPlayerCaravan;

		// Token: 0x04007E4D RID: 32333
		public static WorldObjectDef TravellingTransporters;

		// Token: 0x04007E4E RID: 32334
		public static WorldObjectDef RoutePlannerWaypoint;

		// Token: 0x04007E4F RID: 32335
		public static WorldObjectDef Site;

		// Token: 0x04007E50 RID: 32336
		public static WorldObjectDef PocketMap;

		// Token: 0x04007E51 RID: 32337
		public static WorldObjectDef Camp;

		// Token: 0x04007E52 RID: 32338
		[MayRequireRoyalty]
		public static WorldObjectDef TravelingShuttle;

		// Token: 0x04007E53 RID: 32339
		public static WorldObjectDef Debug_Arena;

		// Token: 0x04007E54 RID: 32340
		[MayRequireIdeology]
		public static WorldObjectDef Settlement_SecondArchonexusCycle;

		// Token: 0x04007E55 RID: 32341
		[MayRequireIdeology]
		public static WorldObjectDef Settlement_ThirdArchonexusCycle;

		// Token: 0x04007E56 RID: 32342
		[MayRequireIdeology]
		public static WorldObjectDef AbandonedArchotechStructures;

		// Token: 0x04007E57 RID: 32343
		[MayRequireOdyssey]
		public static WorldObjectDef Gravship;

		// Token: 0x04007E58 RID: 32344
		[MayRequireOdyssey]
		public static WorldObjectDef ClaimableSite;

		// Token: 0x04007E59 RID: 32345
		[MayRequireOdyssey]
		public static WorldObjectDef ClaimableSpaceSite;

		// Token: 0x04007E5A RID: 32346
		[MayRequireOdyssey]
		public static WorldObjectDef Mechhive;

		// Token: 0x04007E5B RID: 32347
		[MayRequireOdyssey]
		public static WorldObjectDef GravshipLaunch;

		// Token: 0x04007E5C RID: 32348
		[MayRequireOdyssey]
		public static WorldObjectDef AbandonedLandmark;
	}
}
