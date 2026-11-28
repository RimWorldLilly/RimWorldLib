using System;

namespace RimWorld
{
	// Token: 0x02001EB2 RID: 7858
	[DefOf]
	public static class ThingSetMakerDefOf
	{
		// Token: 0x0600CA6D RID: 51821 RVA: 0x004A5FFF File Offset: 0x004A41FF
		static ThingSetMakerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThingSetMakerDefOf));
		}

		// Token: 0x04007EA0 RID: 32416
		public static ThingSetMakerDef MapGen_AncientTempleContents;

		// Token: 0x04007EA1 RID: 32417
		public static ThingSetMakerDef MapGen_AncientPodContents;

		// Token: 0x04007EA2 RID: 32418
		public static ThingSetMakerDef MapGen_DefaultStockpile;

		// Token: 0x04007EA3 RID: 32419
		public static ThingSetMakerDef MapGen_PrisonCellStockpile;

		// Token: 0x04007EA4 RID: 32420
		public static ThingSetMakerDef MapGen_AncientComplexRoomLoot_Default;

		// Token: 0x04007EA5 RID: 32421
		[MayRequireIdeology]
		public static ThingSetMakerDef MapGen_AncientComplexRoomLoot_Better;

		// Token: 0x04007EA6 RID: 32422
		[MayRequireIdeology]
		public static ThingSetMakerDef MapGen_AncientComplex_SecurityCrate;

		// Token: 0x04007EA7 RID: 32423
		[MayRequireAnomaly]
		public static ThingSetMakerDef MapGen_FleshSackLoot;

		// Token: 0x04007EA8 RID: 32424
		[MayRequireOdyssey]
		public static ThingSetMakerDef MapGen_ScarlandsAncientPodContents;

		// Token: 0x04007EA9 RID: 32425
		[MayRequireOdyssey]
		public static ThingSetMakerDef MapGen_ScarlandsHermeticCrate;

		// Token: 0x04007EAA RID: 32426
		[MayRequireOdyssey]
		public static ThingSetMakerDef MapGen_HighValueCrate;

		// Token: 0x04007EAB RID: 32427
		[MayRequireOdyssey]
		public static ThingSetMakerDef MapGen_AbandonedColonyStockpile;

		// Token: 0x04007EAC RID: 32428
		public static ThingSetMakerDef Reward_ItemsStandard;

		// Token: 0x04007EAD RID: 32429
		[MayRequireAnomaly]
		public static ThingSetMakerDef Reward_GrayBox;

		// Token: 0x04007EAE RID: 32430
		[MayRequireAnomaly]
		public static ThingSetMakerDef Reward_GrayBoxLowReward;

		// Token: 0x04007EAF RID: 32431
		public static ThingSetMakerDef DebugCaravanInventory;

		// Token: 0x04007EB0 RID: 32432
		public static ThingSetMakerDef DebugQuestDropPodsContents;

		// Token: 0x04007EB1 RID: 32433
		public static ThingSetMakerDef TraderStock;

		// Token: 0x04007EB2 RID: 32434
		public static ThingSetMakerDef ResourcePod;

		// Token: 0x04007EB3 RID: 32435
		public static ThingSetMakerDef RefugeePod;

		// Token: 0x04007EB4 RID: 32436
		public static ThingSetMakerDef Meteorite;

		// Token: 0x04007EB5 RID: 32437
		public static ThingSetMakerDef VisitorGift;

		// Token: 0x04007EB6 RID: 32438
		[MayRequireIdeology]
		public static ThingSetMakerDef Reward_ReliquaryPilgrims;

		// Token: 0x04007EB7 RID: 32439
		[MayRequireOdyssey]
		public static ThingSetMakerDef Reward_UniqueWeapon;

		// Token: 0x04007EB8 RID: 32440
		[MayRequireOdyssey]
		public static ThingSetMakerDef Reward_ArcheanSeed;
	}
}
