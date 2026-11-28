using System;

namespace Verse
{
	// Token: 0x0200068F RID: 1679
	public struct CellLine
	{
		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x0600350C RID: 13580 RVA: 0x00158ED7 File Offset: 0x001570D7
		public float ZIntercept
		{
			get
			{
				return this.zIntercept;
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x0600350D RID: 13581 RVA: 0x00158EDF File Offset: 0x001570DF
		public float Slope
		{
			get
			{
				return this.slope;
			}
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x00158EE7 File Offset: 0x001570E7
		public CellLine(float zIntercept, float slope)
		{
			this.zIntercept = zIntercept;
			this.slope = slope;
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x00158EF7 File Offset: 0x001570F7
		public CellLine(IntVec3 cell, float slope)
		{
			this.slope = slope;
			this.zIntercept = (float)cell.z - (float)cell.x * slope;
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x00158F18 File Offset: 0x00157118
		public static CellLine Between(IntVec3 a, IntVec3 b)
		{
			float num;
			if (a.x == b.x)
			{
				num = 100000000f;
			}
			else
			{
				num = (float)(b.z - a.z) / (float)(b.x - a.x);
			}
			return new CellLine((float)a.z - (float)a.x * num, num);
		}

		// Token: 0x06003511 RID: 13585 RVA: 0x00158F70 File Offset: 0x00157170
		public bool CellIsAbove(IntVec3 c)
		{
			return (float)c.z > this.slope * (float)c.x + this.zIntercept;
		}

		// Token: 0x04002278 RID: 8824
		private float zIntercept;

		// Token: 0x04002279 RID: 8825
		private float slope;
	}
}
