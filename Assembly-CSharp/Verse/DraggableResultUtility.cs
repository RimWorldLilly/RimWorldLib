using System;

namespace Verse
{
	// Token: 0x0200065E RID: 1630
	internal static class DraggableResultUtility
	{
		// Token: 0x060033B1 RID: 13233 RVA: 0x0014CB57 File Offset: 0x0014AD57
		public static bool AnyPressed(this Widgets.DraggableResult result)
		{
			return result == Widgets.DraggableResult.Pressed || result == Widgets.DraggableResult.DraggedThenPressed;
		}
	}
}
