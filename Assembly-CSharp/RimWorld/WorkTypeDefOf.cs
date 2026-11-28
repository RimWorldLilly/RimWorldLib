using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E75 RID: 7797
	[DefOf]
	public static class WorkTypeDefOf
	{
		// Token: 0x0600CA30 RID: 51760 RVA: 0x004A5BF2 File Offset: 0x004A3DF2
		static WorkTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkTypeDefOf));
		}

		// Token: 0x04007789 RID: 30601
		public static WorkTypeDef Mining;

		// Token: 0x0400778A RID: 30602
		public static WorkTypeDef Growing;

		// Token: 0x0400778B RID: 30603
		public static WorkTypeDef Construction;

		// Token: 0x0400778C RID: 30604
		public static WorkTypeDef Warden;

		// Token: 0x0400778D RID: 30605
		public static WorkTypeDef Doctor;

		// Token: 0x0400778E RID: 30606
		public static WorkTypeDef Firefighter;

		// Token: 0x0400778F RID: 30607
		public static WorkTypeDef Hunting;

		// Token: 0x04007790 RID: 30608
		public static WorkTypeDef Handling;

		// Token: 0x04007791 RID: 30609
		public static WorkTypeDef Crafting;

		// Token: 0x04007792 RID: 30610
		public static WorkTypeDef Hauling;

		// Token: 0x04007793 RID: 30611
		public static WorkTypeDef Cleaning;

		// Token: 0x04007794 RID: 30612
		public static WorkTypeDef Research;

		// Token: 0x04007795 RID: 30613
		public static WorkTypeDef PlantCutting;

		// Token: 0x04007796 RID: 30614
		public static WorkTypeDef Smithing;

		// Token: 0x04007797 RID: 30615
		[MayRequireBiotech]
		public static WorkTypeDef Childcare;

		// Token: 0x04007798 RID: 30616
		[MayRequireAnomaly]
		public static WorkTypeDef DarkStudy;

		// Token: 0x04007799 RID: 30617
		[MayRequireOdyssey]
		public static WorkTypeDef Fishing;
	}
}
