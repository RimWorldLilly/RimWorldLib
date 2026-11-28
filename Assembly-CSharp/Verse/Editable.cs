using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200010B RID: 267
	public class Editable
	{
		// Token: 0x06000944 RID: 2372 RVA: 0x000355FF File Offset: 0x000337FF
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00035601 File Offset: 0x00033801
		public virtual void PostLoad()
		{
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00035603 File Offset: 0x00033803
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}
	}
}
