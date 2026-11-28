using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200127A RID: 4730
	public struct CellWithRadius : IEquatable<CellWithRadius>
	{
		// Token: 0x0600752A RID: 29994 RVA: 0x00292417 File Offset: 0x00290617
		public CellWithRadius(IntVec3 c, float r)
		{
			this.cell = c;
			this.radius = r;
		}

		// Token: 0x0600752B RID: 29995 RVA: 0x00292428 File Offset: 0x00290628
		public bool Equals(CellWithRadius other)
		{
			return this.cell.Equals(other.cell) && this.radius.Equals(other.radius);
		}

		// Token: 0x0600752C RID: 29996 RVA: 0x00292460 File Offset: 0x00290660
		public override bool Equals(object obj)
		{
			if (obj is CellWithRadius)
			{
				CellWithRadius other = (CellWithRadius)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600752D RID: 29997 RVA: 0x00292488 File Offset: 0x00290688
		public override int GetHashCode()
		{
			return this.cell.GetHashCode() * 397 ^ this.radius.GetHashCode();
		}

		// Token: 0x04004970 RID: 18800
		public readonly IntVec3 cell;

		// Token: 0x04004971 RID: 18801
		public readonly float radius;
	}
}
