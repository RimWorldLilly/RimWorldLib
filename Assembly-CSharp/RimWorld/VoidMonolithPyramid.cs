using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B10 RID: 6928
	public class VoidMonolithPyramid : ThingWithComps
	{
		// Token: 0x17001FEB RID: 8171
		// (get) Token: 0x0600B117 RID: 45335 RVA: 0x003E66F8 File Offset: 0x003E48F8
		public override Vector3 DrawPos
		{
			get
			{
				Vector3 drawPos = base.DrawPos;
				float num = Mathf.Sin(GenTicks.TicksGame.TicksToSeconds() / 2f) * 0.1f;
				drawPos.y = AltitudeLayer.MoteOverhead.AltitudeFor() - 0.4f;
				drawPos.z += num;
				return drawPos;
			}
		}

		// Token: 0x04006658 RID: 26200
		private const float PyramidMaxOffset = 0.1f;

		// Token: 0x04006659 RID: 26201
		private const float PyramidOffsetTimeFactor = 2f;
	}
}
