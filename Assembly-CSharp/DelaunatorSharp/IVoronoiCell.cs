using System;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x0200242E RID: 9262
	public interface IVoronoiCell
	{
		// Token: 0x17002933 RID: 10547
		// (get) Token: 0x0600E8A8 RID: 59560
		Vector2[] Points { get; }

		// Token: 0x17002934 RID: 10548
		// (get) Token: 0x0600E8A9 RID: 59561
		int Index { get; }
	}
}
