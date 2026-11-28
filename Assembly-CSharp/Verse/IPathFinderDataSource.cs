using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200035F RID: 863
	public interface IPathFinderDataSource : IDisposable
	{
		// Token: 0x06001A76 RID: 6774
		void ComputeAll(IEnumerable<PathRequest> requests);

		// Token: 0x06001A77 RID: 6775
		bool UpdateIncrementally(IEnumerable<PathRequest> requests, List<IntVec3> cellDeltas);
	}
}
