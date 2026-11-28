using System;

namespace RimWorld
{
	// Token: 0x02001EA1 RID: 7841
	[DefOf]
	public static class InteractionDefOf
	{
		// Token: 0x0600CA5C RID: 51804 RVA: 0x004A5EDE File Offset: 0x004A40DE
		static InteractionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InteractionDefOf));
		}

		// Token: 0x04007D34 RID: 32052
		public static InteractionDef Chitchat;

		// Token: 0x04007D35 RID: 32053
		public static InteractionDef DeepTalk;

		// Token: 0x04007D36 RID: 32054
		public static InteractionDef Insult;

		// Token: 0x04007D37 RID: 32055
		public static InteractionDef RomanceAttempt;

		// Token: 0x04007D38 RID: 32056
		public static InteractionDef MarriageProposal;

		// Token: 0x04007D39 RID: 32057
		public static InteractionDef BuildRapport;

		// Token: 0x04007D3A RID: 32058
		public static InteractionDef RecruitAttempt;

		// Token: 0x04007D3B RID: 32059
		public static InteractionDef SparkJailbreak;

		// Token: 0x04007D3C RID: 32060
		[MayRequireIdeology]
		public static InteractionDef ReduceWill;

		// Token: 0x04007D3D RID: 32061
		[MayRequireIdeology]
		public static InteractionDef EnslaveAttempt;

		// Token: 0x04007D3E RID: 32062
		[MayRequireIdeology]
		public static InteractionDef ConvertIdeoAttempt;

		// Token: 0x04007D3F RID: 32063
		[MayRequireAnomaly]
		public static InteractionDef PrisonerStudyAnomaly;

		// Token: 0x04007D40 RID: 32064
		[MayRequireAnomaly]
		public static InteractionDef InterrogateIdentity;

		// Token: 0x04007D41 RID: 32065
		[MayRequireIdeology]
		public static InteractionDef Suppress;

		// Token: 0x04007D42 RID: 32066
		[MayRequireIdeology]
		public static InteractionDef SparkSlaveRebellion;

		// Token: 0x04007D43 RID: 32067
		public static InteractionDef AnimalChat;

		// Token: 0x04007D44 RID: 32068
		public static InteractionDef TrainAttempt;

		// Token: 0x04007D45 RID: 32069
		public static InteractionDef TameAttempt;

		// Token: 0x04007D46 RID: 32070
		public static InteractionDef Nuzzle;

		// Token: 0x04007D47 RID: 32071
		public static InteractionDef ReleaseToWild;

		// Token: 0x04007D48 RID: 32072
		[MayRequireIdeology]
		public static InteractionDef Counsel_Success;

		// Token: 0x04007D49 RID: 32073
		[MayRequireIdeology]
		public static InteractionDef Counsel_Failure;

		// Token: 0x04007D4A RID: 32074
		[MayRequireIdeology]
		public static InteractionDef Convert_Success;

		// Token: 0x04007D4B RID: 32075
		[MayRequireIdeology]
		public static InteractionDef Convert_Failure;

		// Token: 0x04007D4C RID: 32076
		[MayRequireIdeology]
		public static InteractionDef Reassure;

		// Token: 0x04007D4D RID: 32077
		[MayRequireIdeology]
		public static InteractionDef Trial_Accuse;

		// Token: 0x04007D4E RID: 32078
		[MayRequireIdeology]
		public static InteractionDef Trial_Defend;

		// Token: 0x04007D4F RID: 32079
		[MayRequireIdeology]
		public static InteractionDef Speech_AcceptRole;

		// Token: 0x04007D50 RID: 32080
		[MayRequireIdeology]
		public static InteractionDef Speech_RemoveRole;

		// Token: 0x04007D51 RID: 32081
		[MayRequireBiotech]
		public static InteractionDef BabyPlay;

		// Token: 0x04007D52 RID: 32082
		[MayRequireBiotech]
		public static InteractionDef LessonGeneric;

		// Token: 0x04007D53 RID: 32083
		[MayRequireAnomaly]
		public static InteractionDef InhumanRambling;

		// Token: 0x04007D54 RID: 32084
		[MayRequireAnomaly]
		public static InteractionDef OccultTeaching;

		// Token: 0x04007D55 RID: 32085
		[MayRequireAnomaly]
		public static InteractionDef DisturbingChat;

		// Token: 0x04007D56 RID: 32086
		[MayRequireAnomaly]
		public static InteractionDef CreepyWords;
	}
}
