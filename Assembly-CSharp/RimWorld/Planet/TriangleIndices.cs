using System;

namespace RimWorld.Planet
{
	// Token: 0x02002346 RID: 9030
	public struct TriangleIndices
	{
		// Token: 0x0600DFA0 RID: 57248 RVA: 0x005324D4 File Offset: 0x005306D4
		public TriangleIndices(int v1, int v2, int v3)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}

		// Token: 0x0600DFA1 RID: 57249 RVA: 0x005324EC File Offset: 0x005306EC
		public bool SharesAnyVertexWith(TriangleIndices t, int otherThan)
		{
			return (this.v1 != otherThan && (this.v1 == t.v1 || this.v1 == t.v2 || this.v1 == t.v3)) || (this.v2 != otherThan && (this.v2 == t.v1 || this.v2 == t.v2 || this.v2 == t.v3)) || (this.v3 != otherThan && (this.v3 == t.v1 || this.v3 == t.v2 || this.v3 == t.v3));
		}

		// Token: 0x0600DFA2 RID: 57250 RVA: 0x0053259B File Offset: 0x0053079B
		public int GetNextOrderedVertex(int root)
		{
			if (this.v1 == root)
			{
				return this.v2;
			}
			if (this.v2 == root)
			{
				return this.v3;
			}
			return this.v1;
		}

		// Token: 0x04008F81 RID: 36737
		public int v1;

		// Token: 0x04008F82 RID: 36738
		public int v2;

		// Token: 0x04008F83 RID: 36739
		public int v3;
	}
}
