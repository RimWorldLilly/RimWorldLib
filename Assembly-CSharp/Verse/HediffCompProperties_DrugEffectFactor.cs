using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200042A RID: 1066
	public class HediffCompProperties_DrugEffectFactor : HediffCompProperties
	{
		// Token: 0x06001F18 RID: 7960 RVA: 0x000BDC08 File Offset: 0x000BBE08
		public HediffCompProperties_DrugEffectFactor()
		{
			this.compClass = typeof(HediffComp_DrugEffectFactor);
		}

		// Token: 0x0400184F RID: 6223
		public ChemicalDef chemical;
	}
}
