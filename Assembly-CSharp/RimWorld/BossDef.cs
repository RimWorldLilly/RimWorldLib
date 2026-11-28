using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F42 RID: 3906
	public class BossDef : Def
	{
		// Token: 0x060064BF RID: 25791 RVA: 0x0023F9BF File Offset: 0x0023DBBF
		public override IEnumerable<string> ConfigErrors()
		{
			BossDef.<ConfigErrors>d__2 <ConfigErrors>d__ = new BossDef.<ConfigErrors>d__2(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003A4C RID: 14924
		public PawnKindDef kindDef;

		// Token: 0x04003A4D RID: 14925
		public int appearAfterTicks = int.MaxValue;
	}
}
