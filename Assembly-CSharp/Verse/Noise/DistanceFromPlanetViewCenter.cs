using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000759 RID: 1881
	public class DistanceFromPlanetViewCenter : ModuleBase
	{
		// Token: 0x06003B81 RID: 15233 RVA: 0x0017AD38 File Offset: 0x00178F38
		public DistanceFromPlanetViewCenter() : base(0)
		{
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x0017AD41 File Offset: 0x00178F41
		public DistanceFromPlanetViewCenter(Vector3 viewCenter, float viewAngle, bool invert = false) : base(0)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.invert = invert;
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x0017AD60 File Offset: 0x00178F60
		public override double GetValue(double x, double y, double z)
		{
			float valueInt = this.GetValueInt(x, y, z);
			if (this.invert)
			{
				return (double)(1f - valueInt);
			}
			return (double)valueInt;
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x0017AD8A File Offset: 0x00178F8A
		private float GetValueInt(double x, double y, double z)
		{
			if (this.viewAngle >= 180f)
			{
				return 0f;
			}
			return Mathf.Min(Vector3.Angle(this.viewCenter, new Vector3((float)x, (float)y, (float)z)) / this.viewAngle, 1f);
		}

		// Token: 0x040026EC RID: 9964
		public Vector3 viewCenter;

		// Token: 0x040026ED RID: 9965
		public float viewAngle;

		// Token: 0x040026EE RID: 9966
		public bool invert;
	}
}
