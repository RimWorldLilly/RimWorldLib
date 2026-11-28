using System;

namespace Verse.Noise
{
	// Token: 0x02000755 RID: 1877
	public class AxisAsValueX : ModuleBase
	{
		// Token: 0x06003B77 RID: 15223 RVA: 0x0017ABB9 File Offset: 0x00178DB9
		public AxisAsValueX() : base(0)
		{
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x0017ABC2 File Offset: 0x00178DC2
		public override double GetValue(double x, double y, double z)
		{
			return x;
		}
	}
}
