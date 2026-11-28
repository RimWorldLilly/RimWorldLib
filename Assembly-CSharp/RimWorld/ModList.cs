using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D05 RID: 7429
	public class ModList : IExposable
	{
		// Token: 0x0600BFFA RID: 49146 RVA: 0x0045EF06 File Offset: 0x0045D106
		public void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.ids, "ids", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.names, "names", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x04006E89 RID: 28297
		public string fileName;

		// Token: 0x04006E8A RID: 28298
		public List<string> ids;

		// Token: 0x04006E8B RID: 28299
		public List<string> names;
	}
}
