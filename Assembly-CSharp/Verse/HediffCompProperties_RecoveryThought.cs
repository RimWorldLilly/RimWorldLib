using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200046D RID: 1133
	public class HediffCompProperties_RecoveryThought : HediffCompProperties
	{
		// Token: 0x06001FEA RID: 8170 RVA: 0x000C0EB2 File Offset: 0x000BF0B2
		public HediffCompProperties_RecoveryThought()
		{
			this.compClass = typeof(HediffComp_RecoveryThought);
		}

		// Token: 0x040018D7 RID: 6359
		public ThoughtDef thought;
	}
}
