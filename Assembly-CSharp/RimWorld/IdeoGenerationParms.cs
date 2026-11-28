using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001625 RID: 5669
	public struct IdeoGenerationParms
	{
		// Token: 0x06008DCA RID: 36298 RVA: 0x0033C094 File Offset: 0x0033A294
		public IdeoGenerationParms(FactionDef forFaction, bool forceNoExpansionIdeo = false, List<PreceptDef> disallowedPrecepts = null, List<MemeDef> disallowedMemes = null, List<MemeDef> forcedMemes = null, bool classicExtra = false, bool forceNoWeaponPreference = false, bool forNewFluidIdeo = false, bool fixedIdeo = false, string name = "", List<StyleCategoryDef> styles = null, List<DeityPreset> deities = null, bool hidden = false, string description = "", bool requiredPreceptsOnly = false)
		{
			this.forFaction = forFaction;
			this.forceNoExpansionIdeo = forceNoExpansionIdeo;
			this.disallowedPrecepts = disallowedPrecepts;
			this.disallowedMemes = disallowedMemes;
			this.forcedMemes = forcedMemes;
			this.classicExtra = classicExtra;
			this.forceNoWeaponPreference = forceNoWeaponPreference;
			this.forNewFluidIdeo = forNewFluidIdeo;
			this.fixedIdeo = fixedIdeo;
			this.name = name;
			this.styles = styles;
			this.deities = deities;
			this.hidden = hidden;
			this.description = description;
			this.requiredPreceptsOnly = requiredPreceptsOnly;
		}

		// Token: 0x040056E3 RID: 22243
		public FactionDef forFaction;

		// Token: 0x040056E4 RID: 22244
		public bool forceNoExpansionIdeo;

		// Token: 0x040056E5 RID: 22245
		public bool classicExtra;

		// Token: 0x040056E6 RID: 22246
		public List<PreceptDef> disallowedPrecepts;

		// Token: 0x040056E7 RID: 22247
		public List<MemeDef> disallowedMemes;

		// Token: 0x040056E8 RID: 22248
		public List<MemeDef> forcedMemes;

		// Token: 0x040056E9 RID: 22249
		public bool forceNoWeaponPreference;

		// Token: 0x040056EA RID: 22250
		public bool forNewFluidIdeo;

		// Token: 0x040056EB RID: 22251
		public bool fixedIdeo;

		// Token: 0x040056EC RID: 22252
		public string name;

		// Token: 0x040056ED RID: 22253
		public List<StyleCategoryDef> styles;

		// Token: 0x040056EE RID: 22254
		public List<DeityPreset> deities;

		// Token: 0x040056EF RID: 22255
		public bool hidden;

		// Token: 0x040056F0 RID: 22256
		public string description;

		// Token: 0x040056F1 RID: 22257
		public bool requiredPreceptsOnly;
	}
}
