using System;

namespace Verse.Noise
{
	// Token: 0x02000774 RID: 1908
	public class Invert : ModuleBase
	{
		// Token: 0x06003C2D RID: 15405 RVA: 0x0017D0DE File Offset: 0x0017B2DE
		public Invert() : base(1)
		{
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x0017D0E7 File Offset: 0x0017B2E7
		public Invert(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x0017D0F9 File Offset: 0x0017B2F9
		public override double GetValue(double x, double y, double z)
		{
			return -this.modules[0].GetValue(x, y, z);
		}
	}
}
