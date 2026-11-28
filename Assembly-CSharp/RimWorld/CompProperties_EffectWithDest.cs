using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200146A RID: 5226
	public class CompProperties_EffectWithDest : CompProperties_AbilityEffect
	{
		// Token: 0x04005013 RID: 20499
		public AbilityEffectDestination destination;

		// Token: 0x04005014 RID: 20500
		public bool requiresLineOfSight;

		// Token: 0x04005015 RID: 20501
		public float range;

		// Token: 0x04005016 RID: 20502
		public FloatRange randomRange;

		// Token: 0x04005017 RID: 20503
		public ClamorDef destClamorType;

		// Token: 0x04005018 RID: 20504
		public int destClamorRadius;
	}
}
