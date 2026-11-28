using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000199 RID: 409
	public class PathGridDef : Def
	{
		// Token: 0x06000B69 RID: 2921 RVA: 0x0003D8F8 File Offset: 0x0003BAF8
		public override IEnumerable<string> ConfigErrors()
		{
			PathGridDef.<ConfigErrors>d__3 <ConfigErrors>d__ = new PathGridDef.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040009EB RID: 2539
		public Type workerType = typeof(PathGrid);

		// Token: 0x040009EC RID: 2540
		public bool fencePassable;

		// Token: 0x040009ED RID: 2541
		public bool flying;
	}
}
