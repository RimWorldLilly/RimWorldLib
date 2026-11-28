using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A57 RID: 6743
	public class Blueprint_StorageWithRoomHighlight : Blueprint_Storage
	{
		// Token: 0x0600A92B RID: 43307 RVA: 0x003C00C8 File Offset: 0x003BE2C8
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (Find.Selector.SingleSelectedThing == this)
			{
				Room room = this.GetRoom(RegionType.Set_All);
				if (room != null && room.ProperRoom && !room.PsychologicallyOutdoors)
				{
					room.DrawFieldEdges();
				}
			}
		}
	}
}
