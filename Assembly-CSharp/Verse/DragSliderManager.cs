using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000647 RID: 1607
	public static class DragSliderManager
	{
		// Token: 0x06003256 RID: 12886 RVA: 0x00140F3B File Offset: 0x0013F13B
		public static void ForceStop()
		{
			DragSliderManager.dragging = false;
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x00140F43 File Offset: 0x0013F143
		public static bool DragSlider(Rect rect, float rateFactor, DragSliderCallback newStartMethod, DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			if (Event.current.type == EventType.mouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				DragSliderManager.lastRateFactor = rateFactor;
				newStartMethod(0f, rateFactor);
				DragSliderManager.StartDragSliding(newDraggingUpdateMethod, newCompletedMethod);
				return true;
			}
			return false;
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x00140F82 File Offset: 0x0013F182
		private static void StartDragSliding(DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
		{
			DragSliderManager.dragging = true;
			DragSliderManager.draggingUpdateMethod = newDraggingUpdateMethod;
			DragSliderManager.completedMethod = newCompletedMethod;
			DragSliderManager.rootX = UI.MousePositionOnUI.x;
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x00140FA5 File Offset: 0x0013F1A5
		private static float CurMouseOffset()
		{
			return UI.MousePositionOnUI.x - DragSliderManager.rootX;
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x00140FB8 File Offset: 0x0013F1B8
		public static void DragSlidersOnGUI()
		{
			if (DragSliderManager.dragging && Event.current.type == EventType.mouseUp && Event.current.button == 0)
			{
				DragSliderManager.dragging = false;
				if (DragSliderManager.completedMethod != null)
				{
					DragSliderManager.completedMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
				}
			}
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x00141006 File Offset: 0x0013F206
		public static void DragSlidersUpdate()
		{
			if (DragSliderManager.dragging && DragSliderManager.draggingUpdateMethod != null)
			{
				DragSliderManager.draggingUpdateMethod(DragSliderManager.CurMouseOffset(), DragSliderManager.lastRateFactor);
			}
		}

		// Token: 0x04002034 RID: 8244
		private static bool dragging = false;

		// Token: 0x04002035 RID: 8245
		private static float rootX;

		// Token: 0x04002036 RID: 8246
		private static float lastRateFactor = 1f;

		// Token: 0x04002037 RID: 8247
		private static DragSliderCallback draggingUpdateMethod;

		// Token: 0x04002038 RID: 8248
		private static DragSliderCallback completedMethod;
	}
}
