using System;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000113 RID: 275
	public class Vector3Curve
	{
		// Token: 0x06000964 RID: 2404 RVA: 0x00035960 File Offset: 0x00033B60
		public Vector3 Evaluate(float time, float nullCurveValue = 0f)
		{
			ComplexCurve complexCurve = this.x;
			float num = (complexCurve != null) ? complexCurve.Evaluate(time) : nullCurveValue;
			ComplexCurve complexCurve2 = this.y;
			float num2 = (complexCurve2 != null) ? complexCurve2.Evaluate(time) : nullCurveValue;
			ComplexCurve complexCurve3 = this.z;
			float num3 = (complexCurve3 != null) ? complexCurve3.Evaluate(time) : nullCurveValue;
			return new Vector3(num, num2, num3);
		}

		// Token: 0x04000475 RID: 1141
		public ComplexCurve x;

		// Token: 0x04000476 RID: 1142
		public ComplexCurve y;

		// Token: 0x04000477 RID: 1143
		public ComplexCurve z;
	}
}
