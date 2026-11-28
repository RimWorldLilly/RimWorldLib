using System;

namespace Verse.Noise
{
	// Token: 0x0200077A RID: 1914
	public class PowerKeepSign : ModuleBase
	{
		// Token: 0x06003C3F RID: 15423 RVA: 0x0017D285 File Offset: 0x0017B485
		public PowerKeepSign() : base(2)
		{
		}

		// Token: 0x06003C40 RID: 15424 RVA: 0x0017D28E File Offset: 0x0017B48E
		public PowerKeepSign(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x0017D2AC File Offset: 0x0017B4AC
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			return (double)Math.Sign(value) * Math.Pow(Math.Abs(value), this.modules[1].GetValue(x, y, z));
		}
	}
}
