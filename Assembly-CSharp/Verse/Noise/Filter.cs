using System;

namespace Verse.Noise
{
	// Token: 0x02000772 RID: 1906
	public class Filter : ModuleBase
	{
		// Token: 0x06003C27 RID: 15399 RVA: 0x0017CFE1 File Offset: 0x0017B1E1
		public Filter() : base(1)
		{
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x0017CFEA File Offset: 0x0017B1EA
		public Filter(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x0017D00C File Offset: 0x0017B20C
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			if (value >= (double)this.from && value <= (double)this.to)
			{
				return 1.0;
			}
			return 0.0;
		}

		// Token: 0x04002735 RID: 10037
		private float from;

		// Token: 0x04002736 RID: 10038
		private float to;
	}
}
