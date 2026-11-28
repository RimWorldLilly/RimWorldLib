using System;

namespace RimWorld
{
	// Token: 0x02001EAC RID: 7852
	[DefOf]
	public static class PawnGroupKindDefOf
	{
		// Token: 0x0600CA67 RID: 51815 RVA: 0x004A5F99 File Offset: 0x004A4199
		static PawnGroupKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnGroupKindDefOf));
		}

		// Token: 0x04007E68 RID: 32360
		public static PawnGroupKindDef Combat;

		// Token: 0x04007E69 RID: 32361
		public static PawnGroupKindDef Trader;

		// Token: 0x04007E6A RID: 32362
		public static PawnGroupKindDef Peaceful;

		// Token: 0x04007E6B RID: 32363
		public static PawnGroupKindDef Settlement;

		// Token: 0x04007E6C RID: 32364
		public static PawnGroupKindDef Settlement_RangedOnly;

		// Token: 0x04007E6D RID: 32365
		[MayRequireIdeology]
		public static PawnGroupKindDef Miners;

		// Token: 0x04007E6E RID: 32366
		[MayRequireIdeology]
		public static PawnGroupKindDef Farmers;

		// Token: 0x04007E6F RID: 32367
		[MayRequireIdeology]
		public static PawnGroupKindDef Loggers;

		// Token: 0x04007E70 RID: 32368
		[MayRequireIdeology]
		public static PawnGroupKindDef Hunters;

		// Token: 0x04007E71 RID: 32369
		[MayRequireAnomaly]
		public static PawnGroupKindDef Shamblers;

		// Token: 0x04007E72 RID: 32370
		[MayRequireAnomaly]
		public static PawnGroupKindDef Fleshbeasts;

		// Token: 0x04007E73 RID: 32371
		[MayRequireAnomaly]
		public static PawnGroupKindDef FleshbeastsWithDreadmeld;

		// Token: 0x04007E74 RID: 32372
		[MayRequireAnomaly]
		public static PawnGroupKindDef Sightstealers;

		// Token: 0x04007E75 RID: 32373
		[MayRequireAnomaly]
		public static PawnGroupKindDef Metalhorrors;

		// Token: 0x04007E76 RID: 32374
		[MayRequireAnomaly]
		public static PawnGroupKindDef PsychicRitualSiege;

		// Token: 0x04007E77 RID: 32375
		[MayRequireAnomaly]
		public static PawnGroupKindDef Gorehulks;

		// Token: 0x04007E78 RID: 32376
		[MayRequireAnomaly]
		public static PawnGroupKindDef Devourers;

		// Token: 0x04007E79 RID: 32377
		[MayRequireAnomaly]
		public static PawnGroupKindDef Noctols;

		// Token: 0x04007E7A RID: 32378
		[MayRequireAnomaly]
		public static PawnGroupKindDef SightstealersNoctols;

		// Token: 0x04007E7B RID: 32379
		[MayRequireAnomaly]
		public static PawnGroupKindDef Chimeras;
	}
}
