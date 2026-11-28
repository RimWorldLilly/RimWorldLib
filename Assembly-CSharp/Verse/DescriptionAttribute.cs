using System;

namespace Verse
{
	// Token: 0x020005EB RID: 1515
	[AttributeUsage(AttributeTargets.Field)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x06002FB1 RID: 12209 RVA: 0x00131AD1 File Offset: 0x0012FCD1
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}

		// Token: 0x04001E55 RID: 7765
		public string description;
	}
}
