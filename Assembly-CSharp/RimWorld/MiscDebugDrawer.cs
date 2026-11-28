using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F26 RID: 7974
	public static class MiscDebugDrawer
	{
		// Token: 0x0600CB88 RID: 52104 RVA: 0x004AFF2C File Offset: 0x004AE12C
		public static void DebugDrawInteractionCells()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (DebugViewSettings.drawInteractionCells)
			{
				foreach (object obj in Find.Selector.SelectedObjects)
				{
					Thing thing = obj as Thing;
					if (thing != null)
					{
						CellRenderer.RenderCell(thing.InteractionCell, 0.5f);
					}
				}
			}
		}
	}
}
