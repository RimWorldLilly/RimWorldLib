using System;

namespace RimWorld
{
	// Token: 0x02001E8C RID: 7820
	[DefOf]
	public static class TrainableDefOf
	{
		// Token: 0x0600CA47 RID: 51783 RVA: 0x004A5D79 File Offset: 0x004A3F79
		static TrainableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TrainableDefOf));
		}

		// Token: 0x04007AB2 RID: 31410
		public static TrainableDef Tameness;

		// Token: 0x04007AB3 RID: 31411
		public static TrainableDef Obedience;

		// Token: 0x04007AB4 RID: 31412
		public static TrainableDef Release;

		// Token: 0x04007AB5 RID: 31413
		[MayRequireOdyssey]
		public static TrainableDef AttackTarget;

		// Token: 0x04007AB6 RID: 31414
		[MayRequireOdyssey]
		public static TrainableDef Comfort;

		// Token: 0x04007AB7 RID: 31415
		[MayRequireOdyssey]
		public static TrainableDef Forage;

		// Token: 0x04007AB8 RID: 31416
		[MayRequireOdyssey]
		public static TrainableDef Dig;

		// Token: 0x04007AB9 RID: 31417
		[MayRequireOdyssey]
		public static TrainableDef EggSpew;

		// Token: 0x04007ABA RID: 31418
		[MayRequireOdyssey]
		public static TrainableDef SludgeSpew;
	}
}
