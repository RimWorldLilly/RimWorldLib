using System;

namespace Verse
{
	// Token: 0x0200042C RID: 1068
	public class HediffCompProperties_Effecter : HediffCompProperties
	{
		// Token: 0x06001F1F RID: 7967 RVA: 0x000BDCE8 File Offset: 0x000BBEE8
		public HediffCompProperties_Effecter()
		{
			this.compClass = typeof(HediffComp_Effecter);
		}

		// Token: 0x04001851 RID: 6225
		public EffecterDef stateEffecter;

		// Token: 0x04001852 RID: 6226
		public IntRange severityIndices = new IntRange(-1, -1);
	}
}
