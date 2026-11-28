using System;

namespace LudeonTK
{
	// Token: 0x02000057 RID: 87
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class TweakValue : Attribute
	{
		// Token: 0x0600025D RID: 605 RVA: 0x00012CCA File Offset: 0x00010ECA
		public TweakValue(string category, float min = 0f, float max = 100f)
		{
			this.category = category;
			this.min = min;
			this.max = max;
		}

		// Token: 0x04000153 RID: 339
		public string category;

		// Token: 0x04000154 RID: 340
		public float min;

		// Token: 0x04000155 RID: 341
		public float max;
	}
}
