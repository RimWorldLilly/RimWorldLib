using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020A7 RID: 8359
	public struct GeneDefWithType
	{
		// Token: 0x170025D5 RID: 9685
		// (get) Token: 0x0600D2C0 RID: 53952 RVA: 0x004D854E File Offset: 0x004D674E
		public bool RandomChosen
		{
			get
			{
				return this.geneDef.RandomChosen;
			}
		}

		// Token: 0x0600D2C1 RID: 53953 RVA: 0x004D855B File Offset: 0x004D675B
		public GeneDefWithType(GeneDef geneDef, bool xenogene)
		{
			this.geneDef = geneDef;
			this.isXenogene = xenogene;
		}

		// Token: 0x0600D2C2 RID: 53954 RVA: 0x004D856B File Offset: 0x004D676B
		public bool ConflictsWith(GeneDefWithType other)
		{
			return this.geneDef.ConflictsWith(other.geneDef);
		}

		// Token: 0x0600D2C3 RID: 53955 RVA: 0x004D857E File Offset: 0x004D677E
		public bool Overrides(GeneDefWithType other)
		{
			return this.geneDef.Overrides(other.geneDef, this.isXenogene, other.isXenogene);
		}

		// Token: 0x040085A5 RID: 34213
		public GeneDef geneDef;

		// Token: 0x040085A6 RID: 34214
		public bool isXenogene;
	}
}
