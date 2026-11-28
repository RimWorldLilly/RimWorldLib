using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D2 RID: 5074
	public class CompProperties_AbilityEffect : AbilityCompProperties
	{
		// Token: 0x04004F40 RID: 20288
		public int goodwillImpact;

		// Token: 0x04004F41 RID: 20289
		public bool psychic;

		// Token: 0x04004F42 RID: 20290
		public bool applicableToMechs = true;

		// Token: 0x04004F43 RID: 20291
		public bool applyGoodwillImpactToLodgers = true;

		// Token: 0x04004F44 RID: 20292
		public bool canTargetBosses = true;

		// Token: 0x04004F45 RID: 20293
		public ClamorDef clamorType;

		// Token: 0x04004F46 RID: 20294
		public int clamorRadius;

		// Token: 0x04004F47 RID: 20295
		public float screenShakeIntensity;

		// Token: 0x04004F48 RID: 20296
		public SoundDef sound;

		// Token: 0x04004F49 RID: 20297
		public SoundDef soundMale;

		// Token: 0x04004F4A RID: 20298
		public SoundDef soundFemale;

		// Token: 0x04004F4B RID: 20299
		public string customLetterLabel;

		// Token: 0x04004F4C RID: 20300
		public string customLetterText;

		// Token: 0x04004F4D RID: 20301
		public bool sendLetter = true;

		// Token: 0x04004F4E RID: 20302
		public string message;

		// Token: 0x04004F4F RID: 20303
		public MessageTypeDef messageType;

		// Token: 0x04004F50 RID: 20304
		public float weight = 1f;

		// Token: 0x04004F51 RID: 20305
		public bool availableWhenTargetIsWounded = true;

		// Token: 0x04004F52 RID: 20306
		public bool canTargetBaby = true;
	}
}
