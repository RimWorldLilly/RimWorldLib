using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E9E RID: 7838
	[DefOf]
	public static class RoomRoleDefOf
	{
		// Token: 0x0600CA59 RID: 51801 RVA: 0x004A5EAB File Offset: 0x004A40AB
		static RoomRoleDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RoomRoleDefOf));
		}

		// Token: 0x04007CE4 RID: 31972
		public static RoomRoleDef None;

		// Token: 0x04007CE5 RID: 31973
		public static RoomRoleDef Bedroom;

		// Token: 0x04007CE6 RID: 31974
		public static RoomRoleDef Barracks;

		// Token: 0x04007CE7 RID: 31975
		public static RoomRoleDef PrisonCell;

		// Token: 0x04007CE8 RID: 31976
		public static RoomRoleDef PrisonBarracks;

		// Token: 0x04007CE9 RID: 31977
		public static RoomRoleDef Hospital;

		// Token: 0x04007CEA RID: 31978
		public static RoomRoleDef Workshop;

		// Token: 0x04007CEB RID: 31979
		public static RoomRoleDef Laboratory;

		// Token: 0x04007CEC RID: 31980
		[MayRequireRoyalty]
		public static RoomRoleDef ThroneRoom;

		// Token: 0x04007CED RID: 31981
		[MayRequireIdeology]
		public static RoomRoleDef WorshipRoom;
	}
}
