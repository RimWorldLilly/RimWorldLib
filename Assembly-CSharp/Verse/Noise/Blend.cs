using System;

namespace Verse.Noise
{
	// Token: 0x0200076C RID: 1900
	public class Blend : ModuleBase
	{
		// Token: 0x06003BFE RID: 15358 RVA: 0x0017C9D5 File Offset: 0x0017ABD5
		public Blend() : base(3)
		{
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x0017C9DE File Offset: 0x0017ABDE
		public Blend(ModuleBase lhs, ModuleBase rhs, ModuleBase controller) : base(3)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
			this.modules[2] = controller;
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06003C00 RID: 15360 RVA: 0x0017CA02 File Offset: 0x0017AC02
		// (set) Token: 0x06003C01 RID: 15361 RVA: 0x0017CA0C File Offset: 0x0017AC0C
		public ModuleBase Controller
		{
			get
			{
				return this.modules[2];
			}
			set
			{
				this.modules[2] = value;
			}
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x0017CA18 File Offset: 0x0017AC18
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			double position = (this.modules[2].GetValue(x, y, z) + 1.0) / 2.0;
			return Utils.InterpolateLinear(value, value2, position);
		}
	}
}
