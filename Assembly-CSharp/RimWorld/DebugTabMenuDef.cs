using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F4C RID: 3916
	public class DebugTabMenuDef : Def
	{
		// Token: 0x060064DC RID: 25820 RVA: 0x0023FF0E File Offset: 0x0023E10E
		public override IEnumerable<string> ConfigErrors()
		{
			DebugTabMenuDef.<ConfigErrors>d__2 <ConfigErrors>d__ = new DebugTabMenuDef.<ConfigErrors>d__2(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003A85 RID: 14981
		public Type menuClass;

		// Token: 0x04003A86 RID: 14982
		public int displayOrder = 99999;
	}
}
