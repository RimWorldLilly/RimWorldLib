using System;

namespace Verse.Noise
{
	// Token: 0x0200077D RID: 1917
	public class ScaleBias : ModuleBase
	{
		// Token: 0x06003C57 RID: 15447 RVA: 0x0017D63A File Offset: 0x0017B83A
		public ScaleBias() : base(1)
		{
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x0017D652 File Offset: 0x0017B852
		public ScaleBias(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x0017D673 File Offset: 0x0017B873
		public ScaleBias(double scale, double bias, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Bias = bias;
			this.Scale = scale;
		}

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06003C5A RID: 15450 RVA: 0x0017D6A2 File Offset: 0x0017B8A2
		// (set) Token: 0x06003C5B RID: 15451 RVA: 0x0017D6AA File Offset: 0x0017B8AA
		public double Bias
		{
			get
			{
				return this.bias;
			}
			set
			{
				this.bias = value;
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06003C5C RID: 15452 RVA: 0x0017D6B3 File Offset: 0x0017B8B3
		// (set) Token: 0x06003C5D RID: 15453 RVA: 0x0017D6BB File Offset: 0x0017B8BB
		public double Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		// Token: 0x06003C5E RID: 15454 RVA: 0x0017D6C4 File Offset: 0x0017B8C4
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) * this.scale + this.bias;
		}

		// Token: 0x04002748 RID: 10056
		private double scale = 1.0;

		// Token: 0x04002749 RID: 10057
		private double bias;
	}
}
