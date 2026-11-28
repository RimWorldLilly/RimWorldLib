using System;

namespace Verse
{
	// Token: 0x02000255 RID: 597
	public class DrawStyle_EmptyOval : DrawStyle_FilledOval
	{
		// Token: 0x06001189 RID: 4489 RVA: 0x00064964 File Offset: 0x00062B64
		protected override bool Filled(float x, float z, float radius, float ratio)
		{
			return base.Inside(x, z, radius, ratio) && (!base.Inside(x + 1f, z, radius, ratio) || !base.Inside(x - 1f, z, radius, ratio) || !base.Inside(x, z + 1f, radius, ratio) || !base.Inside(x, z - 1f, radius, ratio) || !base.Inside(x + 1f, z + 1f, radius, ratio) || !base.Inside(x + 1f, z - 1f, radius, ratio) || !base.Inside(x - 1f, z - 1f, radius, ratio) || !base.Inside(x - 1f, z + 1f, radius, ratio));
		}
	}
}
