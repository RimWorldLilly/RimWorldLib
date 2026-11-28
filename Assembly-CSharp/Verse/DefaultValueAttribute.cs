using System;

namespace Verse
{
	// Token: 0x020005E4 RID: 1508
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultValueAttribute : Attribute
	{
		// Token: 0x06002FA8 RID: 12200 RVA: 0x001319DE File Offset: 0x0012FBDE
		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x001319ED File Offset: 0x0012FBED
		public virtual bool ObjIsDefault(object obj)
		{
			if (obj == null)
			{
				return this.value == null;
			}
			return this.value != null && this.value.Equals(obj);
		}

		// Token: 0x04001E53 RID: 7763
		public object value;
	}
}
