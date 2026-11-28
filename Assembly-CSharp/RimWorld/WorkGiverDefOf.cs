using System;

namespace RimWorld
{
	// Token: 0x02001EC6 RID: 7878
	[DefOf]
	public static class WorkGiverDefOf
	{
		// Token: 0x0600CA81 RID: 51841 RVA: 0x004A6153 File Offset: 0x004A4353
		static WorkGiverDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkGiverDefOf));
		}

		// Token: 0x04007F39 RID: 32569
		public static WorkGiverDef Repair;

		// Token: 0x04007F3A RID: 32570
		public static WorkGiverDef ConstructRemoveFloors;

		// Token: 0x04007F3B RID: 32571
		public static WorkGiverDef FightFires;
	}
}
