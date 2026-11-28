using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E81 RID: 7809
	[DefOf]
	public static class RecipeDefOf
	{
		// Token: 0x0600CA3C RID: 51772 RVA: 0x004A5CBE File Offset: 0x004A3EBE
		static RecipeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RecipeDefOf));
		}

		// Token: 0x04007A17 RID: 31255
		public static RecipeDef RemoveBodyPart;

		// Token: 0x04007A18 RID: 31256
		public static RecipeDef InstallPegLeg;

		// Token: 0x04007A19 RID: 31257
		public static RecipeDef SmeltOrDestroyThing;

		// Token: 0x04007A1A RID: 31258
		public static RecipeDef Sterilize;

		// Token: 0x04007A1B RID: 31259
		[MayRequireBiotech]
		public static RecipeDef ImplantXenogerm;

		// Token: 0x04007A1C RID: 31260
		[MayRequireBiotech]
		public static RecipeDef ImplantEmbryo;

		// Token: 0x04007A1D RID: 31261
		[MayRequireBiotech]
		public static RecipeDef ExtractHemogenPack;
	}
}
