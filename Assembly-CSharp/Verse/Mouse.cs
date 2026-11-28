using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200064B RID: 1611
	public static class Mouse
	{
		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x060032A2 RID: 12962 RVA: 0x001430F0 File Offset: 0x001412F0
		public static bool IsInputBlockedNow
		{
			get
			{
				WindowStack windowStack = Find.WindowStack;
				return (Widgets.mouseOverScrollViewStack.Count > 0 && !Widgets.mouseOverScrollViewStack.Peek()) || windowStack.MouseObscuredNow || !windowStack.CurrentWindowGetsInput;
			}
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x00143133 File Offset: 0x00141333
		public static bool IsOver(Rect rect)
		{
			return rect.Contains(Event.current.mousePosition) && !Mouse.IsInputBlockedNow;
		}
	}
}
