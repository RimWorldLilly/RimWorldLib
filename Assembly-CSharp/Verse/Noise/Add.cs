using System;

namespace Verse.Noise
{
	// Token: 0x0200076A RID: 1898
	public class Add : ModuleBase
	{
		// Token: 0x06003BF8 RID: 15352 RVA: 0x0017C94F File Offset: 0x0017AB4F
		public Add() : base(2)
		{
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x0017C958 File Offset: 0x0017AB58
		public Add(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x0017C973 File Offset: 0x0017AB73
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) + this.modules[1].GetValue(x, y, z);
		}
	}
}
