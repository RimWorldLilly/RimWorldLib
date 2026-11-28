using System;

namespace RimWorld
{
	// Token: 0x02001600 RID: 5632
	public struct FactionGeneratorParms
	{
		// Token: 0x06008C2D RID: 35885 RVA: 0x0032F0A5 File Offset: 0x0032D2A5
		public FactionGeneratorParms(FactionDef factionDef, IdeoGenerationParms ideoGenerationParms = default(IdeoGenerationParms), bool? hidden = null)
		{
			this.factionDef = factionDef;
			this.ideoGenerationParms = ideoGenerationParms;
			this.hidden = hidden;
		}

		// Token: 0x04005618 RID: 22040
		public FactionDef factionDef;

		// Token: 0x04005619 RID: 22041
		public IdeoGenerationParms ideoGenerationParms;

		// Token: 0x0400561A RID: 22042
		public bool? hidden;
	}
}
