using System;

namespace Verse.Noise
{
	// Token: 0x02000761 RID: 1889
	public class Const : ModuleBase
	{
		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x06003B99 RID: 15257 RVA: 0x0017B0ED File Offset: 0x001792ED
		// (set) Token: 0x06003B9A RID: 15258 RVA: 0x0017B0F5 File Offset: 0x001792F5
		public double Value
		{
			get
			{
				return this.val;
			}
			set
			{
				this.val = value;
			}
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x0017B0FE File Offset: 0x001792FE
		public Const() : base(0)
		{
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x0017B107 File Offset: 0x00179307
		public Const(double value) : base(0)
		{
			this.Value = value;
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x0017B117 File Offset: 0x00179317
		public override double GetValue(double x, double y, double z)
		{
			return this.val;
		}

		// Token: 0x040026F8 RID: 9976
		private double val;
	}
}
