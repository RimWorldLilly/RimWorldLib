using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000077 RID: 119
	public static class GenAttribute
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x0001B912 File Offset: 0x00019B12
		public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			return Attribute.IsDefined(memberInfo, typeof(T), true);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001B928 File Offset: 0x00019B28
		public static bool TryGetAttribute<T>(this MemberInfo memberInfo, out T customAttribute) where T : Attribute
		{
			if (!memberInfo.HasAttribute<T>())
			{
				customAttribute = default(T);
				return false;
			}
			Attribute[] customAttributes = Attribute.GetCustomAttributes(memberInfo, typeof(T), true);
			if (customAttributes.Length == 1)
			{
				return (customAttribute = (customAttributes[0] as T)) != null;
			}
			if (customAttributes.Length == 0)
			{
				customAttribute = default(T);
				return false;
			}
			for (int i = 0; i < customAttributes.Length; i++)
			{
				T t = customAttributes[i] as T;
				if (t != null)
				{
					customAttribute = t;
					return true;
				}
			}
			customAttribute = default(T);
			return false;
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001B9C0 File Offset: 0x00019BC0
		public static T TryGetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T result;
			memberInfo.TryGetAttribute(out result);
			return result;
		}
	}
}
