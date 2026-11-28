using System;

namespace RimWorld
{
	// Token: 0x02001E63 RID: 7779
	[AttributeUsage(AttributeTargets.Field)]
	public class MayRequireAttribute : Attribute
	{
		// Token: 0x0600CA1C RID: 51740 RVA: 0x004A584C File Offset: 0x004A3A4C
		public MayRequireAttribute(string modId)
		{
			this.modId = modId;
		}

		// Token: 0x040072F2 RID: 29426
		public string modId;
	}
}
