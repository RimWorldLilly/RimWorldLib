using System;

namespace RimWorld
{
	// Token: 0x02001E74 RID: 7796
	[DefOf]
	public static class FactionDefOf
	{
		// Token: 0x0600CA2F RID: 51759 RVA: 0x004A5BE1 File Offset: 0x004A3DE1
		static FactionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FactionDefOf));
		}

		// Token: 0x04007774 RID: 30580
		public static FactionDef PlayerColony;

		// Token: 0x04007775 RID: 30581
		public static FactionDef PlayerTribe;

		// Token: 0x04007776 RID: 30582
		public static FactionDef Ancients;

		// Token: 0x04007777 RID: 30583
		public static FactionDef AncientsHostile;

		// Token: 0x04007778 RID: 30584
		public static FactionDef Mechanoid;

		// Token: 0x04007779 RID: 30585
		public static FactionDef Insect;

		// Token: 0x0400777A RID: 30586
		public static FactionDef Pirate;

		// Token: 0x0400777B RID: 30587
		public static FactionDef OutlanderCivil;

		// Token: 0x0400777C RID: 30588
		public static FactionDef TribeCivil;

		// Token: 0x0400777D RID: 30589
		public static FactionDef OutlanderRough;

		// Token: 0x0400777E RID: 30590
		public static FactionDef TribeRough;

		// Token: 0x0400777F RID: 30591
		[MayRequireRoyalty]
		public static FactionDef Empire;

		// Token: 0x04007780 RID: 30592
		[MayRequireRoyalty]
		public static FactionDef OutlanderRefugee;

		// Token: 0x04007781 RID: 30593
		[MayRequireIdeology]
		public static FactionDef Beggars;

		// Token: 0x04007782 RID: 30594
		[MayRequireIdeology]
		public static FactionDef Pilgrims;

		// Token: 0x04007783 RID: 30595
		[MayRequireBiotech]
		public static FactionDef PirateWaster;

		// Token: 0x04007784 RID: 30596
		[MayRequireBiotech]
		public static FactionDef Sanguophages;

		// Token: 0x04007785 RID: 30597
		[MayRequireAnomaly]
		public static FactionDef HoraxCult;

		// Token: 0x04007786 RID: 30598
		[MayRequireAnomaly]
		public static FactionDef Entities;

		// Token: 0x04007787 RID: 30599
		[MayRequireOdyssey]
		public static FactionDef TradersGuild;

		// Token: 0x04007788 RID: 30600
		[MayRequireOdyssey]
		public static FactionDef Salvagers;
	}
}
