using System;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x02002431 RID: 9265
	public struct VoronoiCell : IVoronoiCell
	{
		// Token: 0x1700293A RID: 10554
		// (get) Token: 0x0600E8B6 RID: 59574 RVA: 0x0056851F File Offset: 0x0056671F
		// (set) Token: 0x0600E8B7 RID: 59575 RVA: 0x00568527 File Offset: 0x00566727
		public Vector2[] Points { readonly get; set; }

		// Token: 0x1700293B RID: 10555
		// (get) Token: 0x0600E8B8 RID: 59576 RVA: 0x00568530 File Offset: 0x00566730
		// (set) Token: 0x0600E8B9 RID: 59577 RVA: 0x00568538 File Offset: 0x00566738
		public int Index { readonly get; set; }

		// Token: 0x0600E8BA RID: 59578 RVA: 0x00568541 File Offset: 0x00566741
		public VoronoiCell(int triangleIndex, Vector2[] points)
		{
			this.Points = points;
			this.Index = triangleIndex;
		}
	}
}
