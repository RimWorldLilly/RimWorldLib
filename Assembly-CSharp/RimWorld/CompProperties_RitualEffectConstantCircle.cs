using System;

namespace RimWorld
{
	// Token: 0x02001735 RID: 5941
	public class CompProperties_RitualEffectConstantCircle : CompProperties_RitualVisualEffect
	{
		// Token: 0x06009420 RID: 37920 RVA: 0x003601B4 File Offset: 0x0035E3B4
		public CompProperties_RitualEffectConstantCircle()
		{
			this.compClass = typeof(CompRitualEffect_ConstantCircle);
		}

		// Token: 0x04005978 RID: 22904
		public float radius = 5f;

		// Token: 0x04005979 RID: 22905
		public int numCopies = 5;
	}
}
