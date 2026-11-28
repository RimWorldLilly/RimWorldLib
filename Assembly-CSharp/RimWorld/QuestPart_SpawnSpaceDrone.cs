using System;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FA RID: 4346
	public class QuestPart_SpawnSpaceDrone : QuestPart_SpawnThing
	{
		// Token: 0x06006C72 RID: 27762 RVA: 0x002606AC File Offset: 0x0025E8AC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				Map map = QuestGen_Get.GetMap(false, null, false);
				this.mapParent = ((map != null) ? map.Parent : null);
				if (this.mapParent != null)
				{
					QuestPart_SpawnSpaceDrone.TryFindSpacedronePosition(this.mapParent.Map, out this.cell);
				}
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x06006C73 RID: 27763 RVA: 0x00260718 File Offset: 0x0025E918
		public static bool TryFindSpacedronePosition(Map map, out IntVec3 spot)
		{
			IntVec2 size = ThingDefOf.Spacedrone.size;
			CellRect rect = GenAdj.OccupiedRect(IntVec3.Zero, ThingDefOf.Spacedrone.defaultPlacingRot, ThingDefOf.Spacedrone.size);
			IntVec3 position = rect.CenterCell + ThingDefOf.Spacedrone.interactionCellOffset;
			rect = rect.ExpandToFit(position);
			return DropCellFinder.FindSafeLandingSpot(out spot, null, map, 35, 15, 25, new IntVec2?(new IntVec2(rect.Width, rect.Height)), new IntVec3?(ThingDefOf.Spacedrone.interactionCellOffset));
		}
	}
}
