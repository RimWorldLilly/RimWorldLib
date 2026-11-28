using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000064 RID: 100
	public struct CurvePoint
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00016EFE File Offset: 0x000150FE
		public Vector2 Loc
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600033E RID: 830 RVA: 0x00016F06 File Offset: 0x00015106
		public float x
		{
			get
			{
				return this.loc.x;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00016F13 File Offset: 0x00015113
		public float y
		{
			get
			{
				return this.loc.y;
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00016F20 File Offset: 0x00015120
		public CurvePoint(float x, float y)
		{
			this.loc = new Vector2(x, y);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00016F2F File Offset: 0x0001512F
		public CurvePoint(Vector2 loc)
		{
			this.loc = loc;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00016F38 File Offset: 0x00015138
		public static CurvePoint FromString(string str)
		{
			return new CurvePoint(ParseHelper.FromString<Vector2>(str));
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00016F45 File Offset: 0x00015145
		public override string ToString()
		{
			return this.loc.ToStringTwoDigits();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00016F52 File Offset: 0x00015152
		public static implicit operator Vector2(CurvePoint pt)
		{
			return pt.loc;
		}

		// Token: 0x04000175 RID: 373
		private Vector2 loc;
	}
}
