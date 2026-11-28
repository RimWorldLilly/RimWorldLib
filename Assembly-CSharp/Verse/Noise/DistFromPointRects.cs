using System;
using System.Collections.Generic;

namespace Verse.Noise
{
	// Token: 0x0200075F RID: 1887
	public class DistFromPointRects : ModuleBase
	{
		// Token: 0x06003B94 RID: 15252 RVA: 0x0017AFB0 File Offset: 0x001791B0
		public DistFromPointRects() : base(0)
		{
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x0017AFB9 File Offset: 0x001791B9
		public DistFromPointRects(List<CellRect> rects) : base(0)
		{
			this.rects = rects;
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x0017AFCC File Offset: 0x001791CC
		public override double GetValue(double x, double y, double z)
		{
			double num = 0.0;
			double num2 = 0.0;
			for (int i = 0; i < this.rects.Count; i++)
			{
				double num3 = 1.0 - Math.Abs(x - (double)this.rects[i].CenterCell.x) / (double)((float)this.rects[i].Width / 2f);
				double num4 = 1.0 - Math.Abs(z - (double)this.rects[i].CenterCell.z) / (double)((float)this.rects[i].Height / 2f);
				if (num3 > num)
				{
					num = num3;
				}
				if (num4 > num2)
				{
					num2 = num4;
				}
			}
			return Math.Min(num, num2);
		}

		// Token: 0x040026F6 RID: 9974
		public List<CellRect> rects;
	}
}
