using System;

namespace Verse
{
	// Token: 0x020005E2 RID: 1506
	[AttributeUsage(AttributeTargets.Field)]
	public class EditSliderRangeAttribute : Attribute
	{
		// Token: 0x06002FA6 RID: 12198 RVA: 0x001319AE File Offset: 0x0012FBAE
		public EditSliderRangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x04001E50 RID: 7760
		public float min;

		// Token: 0x04001E51 RID: 7761
		public float max = 1f;
	}
}
