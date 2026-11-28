using System;

namespace Verse.Noise
{
	// Token: 0x02000769 RID: 1897
	public class Abs : ModuleBase
	{
		// Token: 0x06003BF5 RID: 15349 RVA: 0x0017C91D File Offset: 0x0017AB1D
		public Abs() : base(1)
		{
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x0017C926 File Offset: 0x0017AB26
		public Abs(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x0017C938 File Offset: 0x0017AB38
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(this.modules[0].GetValue(x, y, z));
		}
	}
}
