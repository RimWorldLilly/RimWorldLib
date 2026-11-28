using System;

namespace Verse
{
	// Token: 0x020005ED RID: 1517
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class IgnoreSavedElementAttribute : Attribute
	{
		// Token: 0x06002FB3 RID: 12211 RVA: 0x00131AE8 File Offset: 0x0012FCE8
		public IgnoreSavedElementAttribute(string elementToIgnore)
		{
			this.elementToIgnore = elementToIgnore;
		}

		// Token: 0x04001E56 RID: 7766
		public string elementToIgnore;
	}
}
