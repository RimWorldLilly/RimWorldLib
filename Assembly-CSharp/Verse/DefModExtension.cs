using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000334 RID: 820
	public abstract class DefModExtension
	{
		// Token: 0x06001932 RID: 6450 RVA: 0x000972DE File Offset: 0x000954DE
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x000972E5 File Offset: 0x000954E5
		public virtual void ResolveReferences(Def parentDef)
		{
		}
	}
}
