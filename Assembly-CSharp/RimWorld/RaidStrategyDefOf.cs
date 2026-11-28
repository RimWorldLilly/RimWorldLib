using System;

namespace RimWorld
{
	// Token: 0x02001E90 RID: 7824
	[DefOf]
	public static class RaidStrategyDefOf
	{
		// Token: 0x0600CA4B RID: 51787 RVA: 0x004A5DBD File Offset: 0x004A3FBD
		static RaidStrategyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RaidStrategyDefOf));
		}

		// Token: 0x04007AC1 RID: 31425
		public static RaidStrategyDef ImmediateAttack;

		// Token: 0x04007AC2 RID: 31426
		public static RaidStrategyDef ImmediateAttackFriendly;

		// Token: 0x04007AC3 RID: 31427
		[MayRequireAnomaly]
		public static RaidStrategyDef PsychicRitualSiege;

		// Token: 0x04007AC4 RID: 31428
		[MayRequireAnomaly]
		public static RaidStrategyDef ShamblerAssault;
	}
}
