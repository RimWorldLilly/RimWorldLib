using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EF9 RID: 7929
	[DefOf]
	public static class AnimationDefOf
	{
		// Token: 0x0600CAB1 RID: 51889 RVA: 0x004A6483 File Offset: 0x004A4683
		static AnimationDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(AnimationDefOf));
		}

		// Token: 0x040080E8 RID: 33000
		[MayRequireAnomaly]
		public static AnimationDef ShamblerRise;

		// Token: 0x040080E9 RID: 33001
		[MayRequireAnomaly]
		public static AnimationDef RevenantSpasm;

		// Token: 0x040080EA RID: 33002
		[MayRequireAnomaly]
		public static AnimationDef RevenantHypnotise;

		// Token: 0x040080EB RID: 33003
		[MayRequireAnomaly]
		public static AnimationDef DevourerDigesting;

		// Token: 0x040080EC RID: 33004
		[MayRequireAnomaly]
		public static AnimationDef UnnaturalCorpseAwokenKilling;

		// Token: 0x040080ED RID: 33005
		[MayRequireAnomaly]
		public static AnimationDef DeathRefusalTwitches;

		// Token: 0x040080EE RID: 33006
		[MayRequireAnomaly]
		public static AnimationDef HoldingPlatformLungeUp;

		// Token: 0x040080EF RID: 33007
		[MayRequireAnomaly]
		public static AnimationDef HoldingPlatformLungeRight;

		// Token: 0x040080F0 RID: 33008
		[MayRequireAnomaly]
		public static AnimationDef HoldingPlatformLungeDown;

		// Token: 0x040080F1 RID: 33009
		[MayRequireAnomaly]
		public static AnimationDef HoldingPlatformLungeLeft;

		// Token: 0x040080F2 RID: 33010
		[MayRequireAnomaly]
		public static AnimationDef HoldingPlatformWiggleIntense;

		// Token: 0x040080F3 RID: 33011
		[MayRequireAnomaly]
		public static AnimationDef HoldingPlatformWiggleLight;
	}
}
