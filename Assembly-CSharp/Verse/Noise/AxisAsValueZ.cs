using System;

namespace Verse.Noise
{
	// Token: 0x02000756 RID: 1878
	public class AxisAsValueZ : ModuleBase
	{
		// Token: 0x06003B79 RID: 15225 RVA: 0x0017ABC5 File Offset: 0x00178DC5
		public AxisAsValueZ() : base(0)
		{
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x0017ABCE File Offset: 0x00178DCE
		public override double GetValue(double x, double y, double z)
		{
			return z;
		}
	}
}
