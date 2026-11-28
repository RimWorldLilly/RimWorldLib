using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000754 RID: 1876
	public class AbsLatitudeCurve : ModuleBase
	{
		// Token: 0x06003B74 RID: 15220 RVA: 0x0017AB64 File Offset: 0x00178D64
		public AbsLatitudeCurve() : base(0)
		{
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x0017AB6D File Offset: 0x00178D6D
		public AbsLatitudeCurve(SimpleCurve curve, float planetRadius) : base(0)
		{
			this.curve = curve;
			this.planetRadius = planetRadius;
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x0017AB84 File Offset: 0x00178D84
		public override double GetValue(double x, double y, double z)
		{
			float f = Mathf.Asin((float)(y / (double)this.planetRadius)) * 57.29578f;
			return (double)this.curve.Evaluate(Mathf.Abs(f));
		}

		// Token: 0x040026E5 RID: 9957
		public SimpleCurve curve;

		// Token: 0x040026E6 RID: 9958
		public float planetRadius;
	}
}
