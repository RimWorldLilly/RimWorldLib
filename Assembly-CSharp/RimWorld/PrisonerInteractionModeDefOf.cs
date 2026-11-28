using System;

namespace RimWorld
{
	// Token: 0x02001EB4 RID: 7860
	[DefOf]
	public static class PrisonerInteractionModeDefOf
	{
		// Token: 0x0600CA6F RID: 51823 RVA: 0x004A6021 File Offset: 0x004A4221
		static PrisonerInteractionModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PrisonerInteractionModeDefOf));
		}

		// Token: 0x04007EC0 RID: 32448
		public static PrisonerInteractionModeDef MaintainOnly;

		// Token: 0x04007EC1 RID: 32449
		public static PrisonerInteractionModeDef AttemptRecruit;

		// Token: 0x04007EC2 RID: 32450
		public static PrisonerInteractionModeDef ReduceResistance;

		// Token: 0x04007EC3 RID: 32451
		public static PrisonerInteractionModeDef Release;

		// Token: 0x04007EC4 RID: 32452
		public static PrisonerInteractionModeDef Execution;

		// Token: 0x04007EC5 RID: 32453
		[MayRequireIdeology]
		public static PrisonerInteractionModeDef Enslave;

		// Token: 0x04007EC6 RID: 32454
		[MayRequireIdeology]
		public static PrisonerInteractionModeDef ReduceWill;

		// Token: 0x04007EC7 RID: 32455
		[MayRequireIdeology]
		public static PrisonerInteractionModeDef Convert;

		// Token: 0x04007EC8 RID: 32456
		[MayRequireBiotech]
		public static PrisonerInteractionModeDef Bloodfeed;

		// Token: 0x04007EC9 RID: 32457
		[MayRequireBiotech]
		public static PrisonerInteractionModeDef HemogenFarm;

		// Token: 0x04007ECA RID: 32458
		[MayRequireAnomaly]
		public static PrisonerInteractionModeDef Study;

		// Token: 0x04007ECB RID: 32459
		[MayRequireAnomaly]
		public static PrisonerInteractionModeDef Interrogate;
	}
}
