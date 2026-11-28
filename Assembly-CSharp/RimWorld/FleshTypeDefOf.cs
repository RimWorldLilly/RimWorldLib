using System;

namespace RimWorld
{
	// Token: 0x02001EB3 RID: 7859
	[DefOf]
	public static class FleshTypeDefOf
	{
		// Token: 0x0600CA6E RID: 51822 RVA: 0x004A6010 File Offset: 0x004A4210
		static FleshTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FleshTypeDefOf));
		}

		// Token: 0x04007EB9 RID: 32441
		public static FleshTypeDef Normal;

		// Token: 0x04007EBA RID: 32442
		public static FleshTypeDef Mechanoid;

		// Token: 0x04007EBB RID: 32443
		public static FleshTypeDef Insectoid;

		// Token: 0x04007EBC RID: 32444
		[MayRequireAnomaly]
		public static FleshTypeDef EntityMechanical;

		// Token: 0x04007EBD RID: 32445
		[MayRequireAnomaly]
		public static FleshTypeDef EntityFlesh;

		// Token: 0x04007EBE RID: 32446
		[MayRequireAnomaly]
		public static FleshTypeDef Fleshbeast;

		// Token: 0x04007EBF RID: 32447
		[MayRequireOdyssey]
		public static FleshTypeDef Drone;
	}
}
