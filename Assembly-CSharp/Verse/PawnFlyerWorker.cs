using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000145 RID: 325
	public class PawnFlyerWorker
	{
		// Token: 0x06000A1B RID: 2587 RVA: 0x00038652 File Offset: 0x00036852
		public PawnFlyerWorker(PawnFlyerProperties properties)
		{
			this.properties = properties;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00038664 File Offset: 0x00036864
		public virtual float AdjustedProgress(float t)
		{
			AnimationCurve progressCurve = this.properties.ProgressCurve;
			if (progressCurve == null || progressCurve.length == 0)
			{
				return t;
			}
			return progressCurve.Evaluate(t);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00038691 File Offset: 0x00036891
		public virtual float GetHeight(float t)
		{
			return GenMath.InverseParabola(t);
		}

		// Token: 0x04000630 RID: 1584
		public PawnFlyerProperties properties;
	}
}
