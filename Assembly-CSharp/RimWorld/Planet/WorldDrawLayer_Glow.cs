using System;
using System.Collections;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02002319 RID: 8985
	public class WorldDrawLayer_Glow : WorldDrawLayer
	{
		// Token: 0x0600DDEB RID: 56811 RVA: 0x0052930C File Offset: 0x0052750C
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer_Glow.<Regenerate>d__3 <Regenerate>d__ = new WorldDrawLayer_Glow.<Regenerate>d__3(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x04008E7B RID: 36475
		private Material material;

		// Token: 0x04008E7C RID: 36476
		private const int SubdivisionsCount = 4;

		// Token: 0x04008E7D RID: 36477
		public const float GlowRadius = 16f;
	}
}
