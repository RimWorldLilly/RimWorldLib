using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E72 RID: 7794
	[DefOf]
	public static class GameConditionDefOf
	{
		// Token: 0x0600CA2D RID: 51757 RVA: 0x004A5BBF File Offset: 0x004A3DBF
		static GameConditionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GameConditionDefOf));
		}

		// Token: 0x04007752 RID: 30546
		public static GameConditionDef Eclipse;

		// Token: 0x04007753 RID: 30547
		public static GameConditionDef PsychicDrone;

		// Token: 0x04007754 RID: 30548
		public static GameConditionDef PsychicSoothe;

		// Token: 0x04007755 RID: 30549
		public static GameConditionDef HeatWave;

		// Token: 0x04007756 RID: 30550
		public static GameConditionDef ColdSnap;

		// Token: 0x04007757 RID: 30551
		public static GameConditionDef Flashstorm;

		// Token: 0x04007758 RID: 30552
		public static GameConditionDef VolcanicWinter;

		// Token: 0x04007759 RID: 30553
		public static GameConditionDef ToxicFallout;

		// Token: 0x0400775A RID: 30554
		public static GameConditionDef Aurora;

		// Token: 0x0400775B RID: 30555
		[MayRequireRoyalty]
		public static GameConditionDef PsychicSuppression;

		// Token: 0x0400775C RID: 30556
		[MayRequireRoyalty]
		public static GameConditionDef WeatherController;

		// Token: 0x0400775D RID: 30557
		[MayRequireRoyalty]
		public static GameConditionDef EMIField;

		// Token: 0x0400775E RID: 30558
		[MayRequireBiotech]
		public static GameConditionDef NoxiousHaze;

		// Token: 0x0400775F RID: 30559
		[MayRequireAnomaly]
		public static GameConditionDef DeathPall;

		// Token: 0x04007760 RID: 30560
		[MayRequireAnomaly]
		public static GameConditionDef GrayPall;

		// Token: 0x04007761 RID: 30561
		[MayRequireAnomaly]
		public static GameConditionDef HateChantDrone;

		// Token: 0x04007762 RID: 30562
		[MayRequireAnomaly]
		public static GameConditionDef UnnaturalHeat;

		// Token: 0x04007763 RID: 30563
		[MayRequireAnomaly]
		public static GameConditionDef UnnaturalDarkness;

		// Token: 0x04007764 RID: 30564
		[MayRequireAnomaly]
		public static GameConditionDef BloodRain;

		// Token: 0x04007765 RID: 30565
		[MayRequireOdyssey]
		public static GameConditionDef Drought;

		// Token: 0x04007766 RID: 30566
		[MayRequireOdyssey]
		public static GameConditionDef LavaFlow;

		// Token: 0x04007767 RID: 30567
		[MayRequireOdyssey]
		public static GameConditionDef GillRot;
	}
}
