using System;

namespace Verse.Noise
{
	// Token: 0x02000758 RID: 1880
	public class CutOff : ModuleBase
	{
		// Token: 0x06003B7E RID: 15230 RVA: 0x0017ACD5 File Offset: 0x00178ED5
		public CutOff() : base(0)
		{
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x0017ACDE File Offset: 0x00178EDE
		public CutOff(bool invert, bool zAxis = false) : base(0)
		{
			this.inverted = invert;
			this.zAxis = zAxis;
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x0017ACF8 File Offset: 0x00178EF8
		public override double GetValue(double x, double y, double z)
		{
			double num = this.zAxis ? z : x;
			if (this.inverted)
			{
				return num < 0.0;
			}
			return num >= 0.0;
		}

		// Token: 0x040026EA RID: 9962
		private bool inverted;

		// Token: 0x040026EB RID: 9963
		private bool zAxis;
	}
}
