using System;

namespace Verse.Noise
{
	// Token: 0x02000773 RID: 1907
	public class InverseLerp : ModuleBase
	{
		// Token: 0x06003C2A RID: 15402 RVA: 0x0017D051 File Offset: 0x0017B251
		public InverseLerp() : base(1)
		{
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x0017D05A File Offset: 0x0017B25A
		public InverseLerp(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x0017D07C File Offset: 0x0017B27C
		public override double GetValue(double x, double y, double z)
		{
			double num = (this.modules[0].GetValue(x, y, z) - (double)this.from) / (double)(this.to - this.from);
			if (num < 0.0)
			{
				return 0.0;
			}
			if (num > 1.0)
			{
				return 1.0;
			}
			return num;
		}

		// Token: 0x04002737 RID: 10039
		private float from;

		// Token: 0x04002738 RID: 10040
		private float to;
	}
}
