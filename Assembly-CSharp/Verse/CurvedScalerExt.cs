using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200025C RID: 604
	public static class CurvedScalerExt
	{
		// Token: 0x060011A2 RID: 4514 RVA: 0x00065470 File Offset: 0x00063670
		public static Vector3 ScaleAtTime(this List<CurvedScaler> scalers, float ageSecs)
		{
			Vector3 vector = Vector3.one;
			foreach (CurvedScaler curvedScaler in scalers)
			{
				Rand.PushState(curvedScaler.GetHashCode());
				float randomInRange = curvedScaler.scaleTime.RandomInRange;
				float randomInRange2 = curvedScaler.scaleAmt.RandomInRange;
				float x = Mathf.Clamp01(ageSecs / randomInRange);
				vector += randomInRange2 * curvedScaler.curve.Evaluate(x) * curvedScaler.axisMask;
				Rand.PopState();
			}
			return vector;
		}
	}
}
