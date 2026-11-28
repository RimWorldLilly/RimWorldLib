using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED1 RID: 3793
	public class CompProperties_EffecterOnDeath : CompProperties
	{
		// Token: 0x0600638A RID: 25482 RVA: 0x0023BB88 File Offset: 0x00239D88
		public CompProperties_EffecterOnDeath()
		{
			this.compClass = typeof(EffecterOnDeath);
		}

		// Token: 0x040037AB RID: 14251
		public EffecterDef effecterDef;
	}
}
