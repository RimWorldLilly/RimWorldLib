using System;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x0200242C RID: 9260
	public interface IEdge
	{
		// Token: 0x1700292E RID: 10542
		// (get) Token: 0x0600E8A3 RID: 59555
		Vector2 P { get; }

		// Token: 0x1700292F RID: 10543
		// (get) Token: 0x0600E8A4 RID: 59556
		Vector2 Q { get; }

		// Token: 0x17002930 RID: 10544
		// (get) Token: 0x0600E8A5 RID: 59557
		int Index { get; }
	}
}
