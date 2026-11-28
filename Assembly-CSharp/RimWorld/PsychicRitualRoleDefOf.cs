using System;

namespace RimWorld
{
	// Token: 0x02001EF8 RID: 7928
	[DefOf]
	public static class PsychicRitualRoleDefOf
	{
		// Token: 0x0600CAB0 RID: 51888 RVA: 0x004A6472 File Offset: 0x004A4672
		static PsychicRitualRoleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PsychicRitualRoleDefOf));
		}

		// Token: 0x040080E4 RID: 32996
		[MayRequireAnomaly]
		public static PsychicRitualRoleDef Invoker;

		// Token: 0x040080E5 RID: 32997
		[MayRequireAnomaly]
		public static PsychicRitualRoleDef Chanter;

		// Token: 0x040080E6 RID: 32998
		[MayRequireAnomaly]
		public static PsychicRitualRoleDef ChanterAdvanced;

		// Token: 0x040080E7 RID: 32999
		[MayRequireAnomaly]
		public static PsychicRitualRoleDef Defender;
	}
}
