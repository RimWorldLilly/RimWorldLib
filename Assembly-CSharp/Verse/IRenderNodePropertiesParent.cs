using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000390 RID: 912
	public interface IRenderNodePropertiesParent
	{
		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001BD2 RID: 7122
		bool HasDefinedGraphicProperties { get; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001BD3 RID: 7123
		List<PawnRenderNodeProperties> RenderNodeProperties { get; }
	}
}
