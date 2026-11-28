using System;
using UnityEngine;

namespace DelaunatorSharp
{
	// Token: 0x0200242F RID: 9263
	public struct Edge : IEdge
	{
		// Token: 0x17002935 RID: 10549
		// (get) Token: 0x0600E8AA RID: 59562 RVA: 0x005684A3 File Offset: 0x005666A3
		// (set) Token: 0x0600E8AB RID: 59563 RVA: 0x005684AB File Offset: 0x005666AB
		public Vector2 P { readonly get; set; }

		// Token: 0x17002936 RID: 10550
		// (get) Token: 0x0600E8AC RID: 59564 RVA: 0x005684B4 File Offset: 0x005666B4
		// (set) Token: 0x0600E8AD RID: 59565 RVA: 0x005684BC File Offset: 0x005666BC
		public Vector2 Q { readonly get; set; }

		// Token: 0x17002937 RID: 10551
		// (get) Token: 0x0600E8AE RID: 59566 RVA: 0x005684C5 File Offset: 0x005666C5
		// (set) Token: 0x0600E8AF RID: 59567 RVA: 0x005684CD File Offset: 0x005666CD
		public int Index { readonly get; set; }

		// Token: 0x0600E8B0 RID: 59568 RVA: 0x005684D6 File Offset: 0x005666D6
		public Edge(int e, Vector2 p, Vector2 q)
		{
			this.Index = e;
			this.P = p;
			this.Q = q;
		}
	}
}
