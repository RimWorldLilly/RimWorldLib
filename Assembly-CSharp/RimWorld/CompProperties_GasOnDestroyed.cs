using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200189B RID: 6299
	public class CompProperties_GasOnDestroyed : CompProperties
	{
		// Token: 0x06009DCA RID: 40394 RVA: 0x0038AFEC File Offset: 0x003891EC
		public CompProperties_GasOnDestroyed()
		{
			this.compClass = typeof(CompGasOnDestroyed);
		}

		// Token: 0x04005D68 RID: 23912
		public GasType gasType;

		// Token: 0x04005D69 RID: 23913
		public int amount;
	}
}
