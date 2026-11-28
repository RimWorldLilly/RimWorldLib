using System;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200005B RID: 91
	public static class UIScaling
	{
		// Token: 0x0600026B RID: 619 RVA: 0x00013534 File Offset: 0x00011734
		public static Rect AdjustRectToUIScaling(Rect rect)
		{
			Rect result = rect;
			result.xMin = UIScaling.AdjustCoordToUIScalingFloor(rect.xMin);
			result.yMin = UIScaling.AdjustCoordToUIScalingFloor(rect.yMin);
			result.xMax = UIScaling.AdjustCoordToUIScalingCeil(rect.xMax);
			result.yMax = UIScaling.AdjustCoordToUIScalingCeil(rect.yMax);
			return result;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00013590 File Offset: 0x00011790
		public static float AdjustCoordToUIScalingFloor(float coord)
		{
			double num = (double)(Prefs.UIScale * coord);
			float num2 = (float)(num - Math.Floor(num)) / Prefs.UIScale;
			return coord - num2;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x000135B8 File Offset: 0x000117B8
		public static float AdjustCoordToUIScalingCeil(float coord)
		{
			double num = (double)(Prefs.UIScale * coord);
			float num2 = (float)(num - Math.Ceiling(num)) / Prefs.UIScale;
			return coord - num2;
		}
	}
}
