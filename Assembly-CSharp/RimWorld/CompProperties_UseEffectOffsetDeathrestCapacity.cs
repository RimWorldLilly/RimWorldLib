using System;

namespace RimWorld
{
	// Token: 0x02001A42 RID: 6722
	public class CompProperties_UseEffectOffsetDeathrestCapacity : CompProperties_UseEffect
	{
		// Token: 0x0600A883 RID: 43139 RVA: 0x003BDDAD File Offset: 0x003BBFAD
		public CompProperties_UseEffectOffsetDeathrestCapacity()
		{
			this.compClass = typeof(CompUseEffect_OffsetDeathrestCapacity);
		}

		// Token: 0x040062FE RID: 25342
		public int offset;
	}
}
