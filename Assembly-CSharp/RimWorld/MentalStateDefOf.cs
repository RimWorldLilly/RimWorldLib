using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E85 RID: 7813
	[DefOf]
	public static class MentalStateDefOf
	{
		// Token: 0x0600CA40 RID: 51776 RVA: 0x004A5D02 File Offset: 0x004A3F02
		static MentalStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MentalStateDefOf));
		}

		// Token: 0x04007A2B RID: 31275
		public static MentalStateDef Berserk;

		// Token: 0x04007A2C RID: 31276
		public static MentalStateDef BerserkPermanent;

		// Token: 0x04007A2D RID: 31277
		public static MentalStateDef BerserkMechanoid;

		// Token: 0x04007A2E RID: 31278
		public static MentalStateDef CocoonDisturbed;

		// Token: 0x04007A2F RID: 31279
		[MayRequireBiotech]
		public static MentalStateDef BerserkWarcall;

		// Token: 0x04007A30 RID: 31280
		[MayRequireAnomaly]
		public static MentalStateDef HumanityBreak;

		// Token: 0x04007A31 RID: 31281
		[MayRequireAnomaly]
		public static MentalStateDef EntityKiller;

		// Token: 0x04007A32 RID: 31282
		public static MentalStateDef Wander_Psychotic;

		// Token: 0x04007A33 RID: 31283
		public static MentalStateDef Wander_Sad;

		// Token: 0x04007A34 RID: 31284
		public static MentalStateDef Wander_OwnRoom;

		// Token: 0x04007A35 RID: 31285
		public static MentalStateDef PanicFlee;

		// Token: 0x04007A36 RID: 31286
		public static MentalStateDef Manhunter;

		// Token: 0x04007A37 RID: 31287
		public static MentalStateDef ManhunterPermanent;

		// Token: 0x04007A38 RID: 31288
		public static MentalStateDef SocialFighting;

		// Token: 0x04007A39 RID: 31289
		public static MentalStateDef Roaming;

		// Token: 0x04007A3A RID: 31290
		[MayRequireIdeology]
		public static MentalStateDef Rebellion;

		// Token: 0x04007A3B RID: 31291
		[MayRequireAnomaly]
		public static MentalStateDef ManhunterBloodRain;

		// Token: 0x04007A3C RID: 31292
		[MayRequireAnomaly]
		public static MentalStateDef CubeSculpting;

		// Token: 0x04007A3D RID: 31293
		[MayRequireOdyssey]
		public static MentalStateDef Terror;

		// Token: 0x04007A3E RID: 31294
		[MayRequireBiotech]
		public static MentalStateDef PanicFleeFire;
	}
}
