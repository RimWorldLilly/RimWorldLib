using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EAB RID: 7851
	[DefOf]
	public static class MapGeneratorDefOf
	{
		// Token: 0x0600CA66 RID: 51814 RVA: 0x004A5F88 File Offset: 0x004A4188
		static MapGeneratorDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MapGeneratorDefOf));
		}

		// Token: 0x04007E5D RID: 32349
		public static MapGeneratorDef Encounter;

		// Token: 0x04007E5E RID: 32350
		public static MapGeneratorDef Base_Player;

		// Token: 0x04007E5F RID: 32351
		public static MapGeneratorDef Base_Faction;

		// Token: 0x04007E60 RID: 32352
		[MayRequireAnomaly]
		public static MapGeneratorDef Undercave;

		// Token: 0x04007E61 RID: 32353
		[MayRequireAnomaly]
		public static MapGeneratorDef MetalHell;

		// Token: 0x04007E62 RID: 32354
		[MayRequireAnomaly]
		public static MapGeneratorDef Labyrinth;

		// Token: 0x04007E63 RID: 32355
		[MayRequireOdyssey]
		public static MapGeneratorDef SpacePocket;

		// Token: 0x04007E64 RID: 32356
		[MayRequireOdyssey]
		public static MapGeneratorDef Space;

		// Token: 0x04007E65 RID: 32357
		[MayRequireOdyssey]
		public static MapGeneratorDef OrbitalRelay;

		// Token: 0x04007E66 RID: 32358
		[MayRequireOdyssey]
		public static MapGeneratorDef Mechhive;

		// Token: 0x04007E67 RID: 32359
		[MayRequireOdyssey]
		public static MapGeneratorDef InsectLair;
	}
}
