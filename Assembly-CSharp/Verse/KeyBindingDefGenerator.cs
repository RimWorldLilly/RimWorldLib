using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006FF RID: 1791
	public static class KeyBindingDefGenerator
	{
		// Token: 0x060038D0 RID: 14544 RVA: 0x0016CAF9 File Offset: 0x0016ACF9
		public static IEnumerable<KeyBindingCategoryDef> ImpliedKeyBindingCategoryDefs(bool hotReload = false)
		{
			KeyBindingDefGenerator.<ImpliedKeyBindingCategoryDefs>d__0 <ImpliedKeyBindingCategoryDefs>d__ = new KeyBindingDefGenerator.<ImpliedKeyBindingCategoryDefs>d__0(-2);
			<ImpliedKeyBindingCategoryDefs>d__.<>3__hotReload = hotReload;
			return <ImpliedKeyBindingCategoryDefs>d__;
		}

		// Token: 0x060038D1 RID: 14545 RVA: 0x0016CB09 File Offset: 0x0016AD09
		public static IEnumerable<KeyBindingDef> ImpliedKeyBindingDefs(bool hotReload = false)
		{
			KeyBindingDefGenerator.<ImpliedKeyBindingDefs>d__1 <ImpliedKeyBindingDefs>d__ = new KeyBindingDefGenerator.<ImpliedKeyBindingDefs>d__1(-2);
			<ImpliedKeyBindingDefs>d__.<>3__hotReload = hotReload;
			return <ImpliedKeyBindingDefs>d__;
		}
	}
}
