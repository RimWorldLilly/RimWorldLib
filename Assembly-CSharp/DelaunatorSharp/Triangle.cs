using System;
using System.Collections.Generic;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x02002430 RID: 9264
	public struct Triangle : ITriangle
	{
		// Token: 0x17002938 RID: 10552
		// (get) Token: 0x0600E8B1 RID: 59569 RVA: 0x005684ED File Offset: 0x005666ED
		// (set) Token: 0x0600E8B2 RID: 59570 RVA: 0x005684F5 File Offset: 0x005666F5
		public int Index { readonly get; set; }

		// Token: 0x17002939 RID: 10553
		// (get) Token: 0x0600E8B3 RID: 59571 RVA: 0x005684FE File Offset: 0x005666FE
		// (set) Token: 0x0600E8B4 RID: 59572 RVA: 0x00568506 File Offset: 0x00566706
		public IEnumerable<Vector2> Points { readonly get; set; }

		// Token: 0x0600E8B5 RID: 59573 RVA: 0x0056850F File Offset: 0x0056670F
		public Triangle(int t, IEnumerable<Vector2> points)
		{
			this.Points = points;
			this.Index = t;
		}
	}
}
