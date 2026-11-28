using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E9D RID: 7837
	[DefOf]
	public static class RoomStatDefOf
	{
		// Token: 0x0600CA58 RID: 51800 RVA: 0x004A5E9A File Offset: 0x004A409A
		static RoomStatDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomStatDefOf));
		}

		// Token: 0x04007CDB RID: 31963
		public static RoomStatDef Cleanliness;

		// Token: 0x04007CDC RID: 31964
		public static RoomStatDef Wealth;

		// Token: 0x04007CDD RID: 31965
		public static RoomStatDef Space;

		// Token: 0x04007CDE RID: 31966
		public static RoomStatDef Beauty;

		// Token: 0x04007CDF RID: 31967
		public static RoomStatDef Impressiveness;

		// Token: 0x04007CE0 RID: 31968
		public static RoomStatDef ReadingBonus;

		// Token: 0x04007CE1 RID: 31969
		public static RoomStatDef InfectionChanceFactor;

		// Token: 0x04007CE2 RID: 31970
		public static RoomStatDef GraveVisitingJoyGainFactor;

		// Token: 0x04007CE3 RID: 31971
		public static RoomStatDef FoodPoisonChance;
	}
}
