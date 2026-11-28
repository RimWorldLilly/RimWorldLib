using System;

namespace Verse.Noise
{
	// Token: 0x02000778 RID: 1912
	public class OneMinus : ModuleBase
	{
		// Token: 0x06003C39 RID: 15417 RVA: 0x0017D203 File Offset: 0x0017B403
		public OneMinus() : base(1)
		{
		}

		// Token: 0x06003C3A RID: 15418 RVA: 0x0017D20C File Offset: 0x0017B40C
		public OneMinus(ModuleBase module) : base(1)
		{
			this.modules[0] = module;
		}

		// Token: 0x06003C3B RID: 15419 RVA: 0x0017D21E File Offset: 0x0017B41E
		public override double GetValue(double x, double y, double z)
		{
			return 1.0 - this.modules[0].GetValue(x, y, z);
		}
	}
}
