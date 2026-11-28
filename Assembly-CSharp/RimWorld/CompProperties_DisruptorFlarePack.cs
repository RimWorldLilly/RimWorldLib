using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001866 RID: 6246
	public class CompProperties_DisruptorFlarePack : CompProperties_AIUSablePack
	{
		// Token: 0x06009C4C RID: 40012 RVA: 0x0038466F File Offset: 0x0038286F
		public CompProperties_DisruptorFlarePack()
		{
			this.compClass = typeof(CompDisruptorFlarePack);
		}

		// Token: 0x04005CDD RID: 23773
		public float fieldRadius;

		// Token: 0x04005CDE RID: 23774
		public float durationSeconds;
	}
}
