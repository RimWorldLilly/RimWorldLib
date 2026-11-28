using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x0200075C RID: 1884
	public class DistFromCone : ModuleBase
	{
		// Token: 0x06003B8B RID: 15243 RVA: 0x0017AE13 File Offset: 0x00179013
		public DistFromCone() : base(0)
		{
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x0017AE1C File Offset: 0x0017901C
		public DistFromCone(float gradient, float span) : base(0)
		{
			ModuleBase moduleBase = new AxisAsValueX();
			moduleBase = new Rotate(0.0, (double)(Mathf.Atan2(1f, gradient) * 57.29578f), 0.0, moduleBase);
			ModuleBase moduleBase2 = new AxisAsValueX();
			moduleBase2 = new Rotate(0.0, (double)(Mathf.Atan2(1f, -gradient) * 57.29578f), 0.0, moduleBase2);
			this.shape = new Min(moduleBase, moduleBase2);
			this.shape = new ScaleBias(-1.0, 1.0, this.shape);
			this.span = span;
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x0017AECA File Offset: 0x001790CA
		public override double GetValue(double x, double y, double z)
		{
			return this.shape.GetValue(x / (double)this.span, y / (double)this.span, z / (double)this.span);
		}

		// Token: 0x040026F1 RID: 9969
		private ModuleBase shape;

		// Token: 0x040026F2 RID: 9970
		private float span;
	}
}
