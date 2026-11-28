using System;
using System.Collections.Generic;

namespace Verse.Sound
{
	// Token: 0x020007BB RID: 1979
	public abstract class AudioGrain
	{
		// Token: 0x06003F9F RID: 16287
		public abstract IEnumerable<ResolvedGrain> GetResolvedGrains();

		// Token: 0x04002A28 RID: 10792
		public bool forceToMono = true;
	}
}
