using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005E6 RID: 1510
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultEmptyListAttribute : DefaultValueAttribute
	{
		// Token: 0x06002FAB RID: 12203 RVA: 0x00131A26 File Offset: 0x0012FC26
		public DefaultEmptyListAttribute(Type type) : base(type)
		{
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x00131A30 File Offset: 0x0012FC30
		public override bool ObjIsDefault(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType().GetGenericTypeDefinition() != typeof(List<>))
			{
				return false;
			}
			Type[] genericArguments = obj.GetType().GetGenericArguments();
			return genericArguments.Length == 1 && !(genericArguments[0] != (Type)this.value) && (int)obj.GetType().GetProperty("Count").GetValue(obj, null) == 0;
		}
	}
}
