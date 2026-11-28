using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000D5 RID: 213
	public static class InspectTabManager
	{
		// Token: 0x06000839 RID: 2105 RVA: 0x0002F83C File Offset: 0x0002DA3C
		public static InspectTabBase GetSharedInstance(Type tabType)
		{
			InspectTabBase inspectTabBase;
			if (InspectTabManager.sharedInstances.TryGetValue(tabType, out inspectTabBase))
			{
				return inspectTabBase;
			}
			inspectTabBase = (InspectTabBase)Activator.CreateInstance(tabType);
			InspectTabManager.sharedInstances.Add(tabType, inspectTabBase);
			return inspectTabBase;
		}

		// Token: 0x040003B6 RID: 950
		private static Dictionary<Type, InspectTabBase> sharedInstances = new Dictionary<Type, InspectTabBase>();
	}
}
