using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006F3 RID: 1779
	public static class GenWorker<T>
	{
		// Token: 0x060038A1 RID: 14497 RVA: 0x0016AED5 File Offset: 0x001690D5
		private static T Get<D>() where D : T
		{
			return GenWorker<T>.Get(typeof(D));
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x0016AEE8 File Offset: 0x001690E8
		public static T Get(Type type)
		{
			T result;
			if (!GenWorker<T>.Workers.TryGetValue(type, out result))
			{
				result = (GenWorker<T>.Workers[type] = (T)((object)Activator.CreateInstance(type)));
			}
			return result;
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x0016AF20 File Offset: 0x00169120
		public static T Get(Type type, params object[] args)
		{
			T result;
			if (!GenWorker<T>.Workers.TryGetValue(type, out result))
			{
				result = (GenWorker<T>.Workers[type] = (T)((object)Activator.CreateInstance(type, args)));
			}
			return result;
		}

		// Token: 0x040024E0 RID: 9440
		private static readonly Dictionary<Type, T> Workers = new Dictionary<Type, T>();
	}
}
