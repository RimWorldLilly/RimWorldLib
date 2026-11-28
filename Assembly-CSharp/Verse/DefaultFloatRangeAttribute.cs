using System;

namespace Verse
{
	// Token: 0x020005E5 RID: 1509
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultFloatRangeAttribute : DefaultValueAttribute
	{
		// Token: 0x06002FAA RID: 12202 RVA: 0x00131A12 File Offset: 0x0012FC12
		public DefaultFloatRangeAttribute(float min, float max) : base(new FloatRange(min, max))
		{
		}
	}
}
