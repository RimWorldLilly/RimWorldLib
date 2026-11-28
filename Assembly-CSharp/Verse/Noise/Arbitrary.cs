using System;

namespace Verse.Noise
{
	// Token: 0x0200076B RID: 1899
	public class Arbitrary : ModuleBase
	{
		// Token: 0x06003BFB RID: 15355 RVA: 0x0017C996 File Offset: 0x0017AB96
		public Arbitrary() : base(1)
		{
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x0017C99F File Offset: 0x0017AB9F
		public Arbitrary(ModuleBase source, Func<double, double> processor) : base(1)
		{
			this.modules[0] = source;
			this.processor = processor;
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x0017C9B8 File Offset: 0x0017ABB8
		public override double GetValue(double x, double y, double z)
		{
			return this.processor(this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x0400272B RID: 10027
		private Func<double, double> processor;
	}
}
