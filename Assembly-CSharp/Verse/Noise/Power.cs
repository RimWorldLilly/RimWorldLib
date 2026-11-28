using System;

namespace Verse.Noise
{
	// Token: 0x02000779 RID: 1913
	public class Power : ModuleBase
	{
		// Token: 0x06003C3C RID: 15420 RVA: 0x0017D23A File Offset: 0x0017B43A
		public Power() : base(2)
		{
		}

		// Token: 0x06003C3D RID: 15421 RVA: 0x0017D243 File Offset: 0x0017B443
		public Power(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x0017D25E File Offset: 0x0017B45E
		public override double GetValue(double x, double y, double z)
		{
			return Math.Pow(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z));
		}
	}
}
