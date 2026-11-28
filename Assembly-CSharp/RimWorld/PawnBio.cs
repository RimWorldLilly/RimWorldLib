using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200154B RID: 5451
	[CaseInsensitiveXMLParsing]
	public class PawnBio
	{
		// Token: 0x17001684 RID: 5764
		// (get) Token: 0x06008534 RID: 34100 RVA: 0x00300558 File Offset: 0x002FE758
		public PawnBioType BioType
		{
			get
			{
				if (this.pirateKing)
				{
					return PawnBioType.PirateKing;
				}
				if (this.adulthood != null)
				{
					return PawnBioType.BackstoryInGame;
				}
				return PawnBioType.Undefined;
			}
		}

		// Token: 0x06008535 RID: 34101 RVA: 0x00300570 File Offset: 0x002FE770
		public void ResolveReferences()
		{
			if (this.adulthood.spawnCategories.Count == 1 && this.adulthood.spawnCategories[0] == "Trader")
			{
				this.adulthood.spawnCategories.Add("Civil");
			}
		}

		// Token: 0x06008536 RID: 34102 RVA: 0x003005C2 File Offset: 0x002FE7C2
		public override string ToString()
		{
			string str = "PawnBio(";
			NameTriple nameTriple = this.name;
			return str + ((nameTriple != null) ? nameTriple.ToString() : null) + ")";
		}

		// Token: 0x040051FC RID: 20988
		public GenderPossibility gender;

		// Token: 0x040051FD RID: 20989
		public NameTriple name;

		// Token: 0x040051FE RID: 20990
		public BackstoryDef childhood;

		// Token: 0x040051FF RID: 20991
		public BackstoryDef adulthood;

		// Token: 0x04005200 RID: 20992
		public bool pirateKing;

		// Token: 0x04005201 RID: 20993
		public bool rare;
	}
}
