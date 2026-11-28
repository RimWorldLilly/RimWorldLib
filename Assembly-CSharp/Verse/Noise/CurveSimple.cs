using System;

namespace Verse.Noise
{
	// Token: 0x02000760 RID: 1888
	public class CurveSimple : ModuleBase
	{
		// Token: 0x06003B97 RID: 15255 RVA: 0x0017B0B5 File Offset: 0x001792B5
		public CurveSimple(ModuleBase input, SimpleCurve curve) : base(1)
		{
			this.modules[0] = input;
			this.curve = curve;
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x0017B0CE File Offset: 0x001792CE
		public override double GetValue(double x, double y, double z)
		{
			return (double)this.curve.Evaluate((float)this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x040026F7 RID: 9975
		private SimpleCurve curve;
	}
}
