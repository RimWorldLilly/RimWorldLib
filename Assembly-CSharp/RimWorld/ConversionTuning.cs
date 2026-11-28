using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001512 RID: 5394
	public class ConversionTuning
	{
		// Token: 0x04005198 RID: 20888
		public const float ConvertAttempt_BaseSelectionWeight = 0.04f;

		// Token: 0x04005199 RID: 20889
		public const float ConvertAttempt_Colonist = 1f;

		// Token: 0x0400519A RID: 20890
		public const float ConvertAttempt_Slave = 0.5f;

		// Token: 0x0400519B RID: 20891
		public const float ConvertAttempt_NPCFreeVsColonist = 0.5f;

		// Token: 0x0400519C RID: 20892
		public const float ConvertAttempt_NPCFreeVsNPCFree = 0.25f;

		// Token: 0x0400519D RID: 20893
		public const float ConvertAttempt_NPCFreeVsPrisoner = 0.25f;

		// Token: 0x0400519E RID: 20894
		public const float ConvertAttempt_NPCFreeVsSlave = 0.5f;

		// Token: 0x0400519F RID: 20895
		public const float ConvertAttempt_PrisonerVsColonist = 0.25f;

		// Token: 0x040051A0 RID: 20896
		public const float ConvertAttempt_PrisonerVsNPCFree = 0.25f;

		// Token: 0x040051A1 RID: 20897
		public const float ConvertAttempt_PrisonerVsPrisoner = 0.5f;

		// Token: 0x040051A2 RID: 20898
		public const float ConvertAttempt_PrisonerVsSlave = 0.5f;

		// Token: 0x040051A3 RID: 20899
		public const float ConvertAttempt_FailOutcomeWeight_Nothing = 0.78f;

		// Token: 0x040051A4 RID: 20900
		public const float ConvertAttempt_FailOutcomeWeight_Resentment = 0.2f;

		// Token: 0x040051A5 RID: 20901
		public const float ConvertAttempt_FailOutcomeWeight_SocialFight = 0.02f;

		// Token: 0x040051A6 RID: 20902
		public const float ConvertAttempt_BaseCertaintyReduction = 0.06f;

		// Token: 0x040051A7 RID: 20903
		public static readonly SimpleCurve CertaintyPerDayByMoodCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 0.01f),
				true
			},
			{
				new CurvePoint(0.5f, 0.02f),
				true
			},
			{
				new CurvePoint(0.8f, 0.03f),
				true
			}
		};

		// Token: 0x040051A8 RID: 20904
		public const float PostConversionCertainty = 0.5f;

		// Token: 0x040051A9 RID: 20905
		public static readonly FloatRange InitialCertaintyRange = new FloatRange(0.6f, 1f);

		// Token: 0x040051AA RID: 20906
		public const float ConversionPowerFactor_AgreeWithMeme = 0.2f;

		// Token: 0x040051AB RID: 20907
		public const float ConversionPowerFactor_DisagreeWithMeme = -0.2f;

		// Token: 0x040051AC RID: 20908
		public const float ConversionPowerFactor_Min = -0.4f;

		// Token: 0x040051AD RID: 20909
		public const float ChildCertaintyChangeFactor = 2f;
	}
}
