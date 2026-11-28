using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001290 RID: 4752
	public static class ResolveParamsUtility
	{
		// Token: 0x060075D6 RID: 30166 RVA: 0x00297410 File Offset: 0x00295610
		public static void SetCustom<T>(ref Dictionary<string, object> custom, string name, T obj, bool inherit = false)
		{
			if (custom == null)
			{
				custom = new Dictionary<string, object>();
			}
			else
			{
				custom = new Dictionary<string, object>(custom);
			}
			if (!custom.ContainsKey(name))
			{
				custom.Add(name, obj);
				return;
			}
			if (!inherit)
			{
				custom[name] = obj;
			}
		}

		// Token: 0x060075D7 RID: 30167 RVA: 0x0029745D File Offset: 0x0029565D
		public static void RemoveCustom(ref Dictionary<string, object> custom, string name)
		{
			if (custom == null)
			{
				return;
			}
			custom = new Dictionary<string, object>(custom);
			custom.Remove(name);
		}

		// Token: 0x060075D8 RID: 30168 RVA: 0x00297478 File Offset: 0x00295678
		public static bool TryGetCustom<T>(Dictionary<string, object> custom, string name, out T obj)
		{
			object obj2;
			if (custom == null || !custom.TryGetValue(name, out obj2))
			{
				obj = default(T);
				return false;
			}
			obj = (T)((object)obj2);
			return true;
		}

		// Token: 0x060075D9 RID: 30169 RVA: 0x002974AC File Offset: 0x002956AC
		public static T GetCustom<T>(Dictionary<string, object> custom, string name)
		{
			object obj;
			if (custom == null || !custom.TryGetValue(name, out obj))
			{
				return default(T);
			}
			return (T)((object)obj);
		}
	}
}
