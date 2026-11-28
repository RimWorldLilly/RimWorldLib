using System;

namespace Verse
{
	// Token: 0x0200045B RID: 1115
	public class HediffCompProperties_MessageBase : HediffCompProperties
	{
		// Token: 0x06001FBB RID: 8123 RVA: 0x000C0571 File Offset: 0x000BE771
		public HediffCompProperties_MessageBase()
		{
			this.compClass = typeof(HediffComp_MessageBase);
		}

		// Token: 0x040018B4 RID: 6324
		public MessageTypeDef messageType;

		// Token: 0x040018B5 RID: 6325
		[MustTranslate]
		public string message;

		// Token: 0x040018B6 RID: 6326
		public bool onlyMessageForColonistsOrPrisoners = true;
	}
}
