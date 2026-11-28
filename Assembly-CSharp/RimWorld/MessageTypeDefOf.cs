using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EBD RID: 7869
	[DefOf]
	public static class MessageTypeDefOf
	{
		// Token: 0x0600CA78 RID: 51832 RVA: 0x004A60BA File Offset: 0x004A42BA
		static MessageTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MessageTypeDefOf));
		}

		// Token: 0x04007EF9 RID: 32505
		public static MessageTypeDef ThreatBig;

		// Token: 0x04007EFA RID: 32506
		public static MessageTypeDef ThreatSmall;

		// Token: 0x04007EFB RID: 32507
		public static MessageTypeDef PawnDeath;

		// Token: 0x04007EFC RID: 32508
		public static MessageTypeDef NegativeHealthEvent;

		// Token: 0x04007EFD RID: 32509
		public static MessageTypeDef NegativeEvent;

		// Token: 0x04007EFE RID: 32510
		public static MessageTypeDef NeutralEvent;

		// Token: 0x04007EFF RID: 32511
		public static MessageTypeDef TaskCompletion;

		// Token: 0x04007F00 RID: 32512
		public static MessageTypeDef PositiveEvent;

		// Token: 0x04007F01 RID: 32513
		public static MessageTypeDef SituationResolved;

		// Token: 0x04007F02 RID: 32514
		public static MessageTypeDef RejectInput;

		// Token: 0x04007F03 RID: 32515
		public static MessageTypeDef CautionInput;

		// Token: 0x04007F04 RID: 32516
		public static MessageTypeDef SilentInput;
	}
}
