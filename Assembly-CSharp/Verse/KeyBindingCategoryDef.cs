using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000180 RID: 384
	public class KeyBindingCategoryDef : Def
	{
		// Token: 0x06000B20 RID: 2848 RVA: 0x0003C974 File Offset: 0x0003AB74
		public static KeyBindingCategoryDef Named(string defName)
		{
			return DefDatabase<KeyBindingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x04000951 RID: 2385
		public bool isGameUniversal;

		// Token: 0x04000952 RID: 2386
		public List<KeyBindingCategoryDef> checkForConflicts = new List<KeyBindingCategoryDef>();

		// Token: 0x04000953 RID: 2387
		public bool selfConflicting = true;
	}
}
