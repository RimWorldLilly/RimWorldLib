using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200124B RID: 4683
	public class RoomContents_Orbital_Corridor : RoomContents_Checkpoint_Corridor
	{
		// Token: 0x06007433 RID: 29747 RVA: 0x0028AC48 File Offset: 0x00288E48
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			int count = room.rects.Count;
			RoomGenUtility.SpawnWallAttatchments(ThingDefOf.LifeSupportUnit, map, room, IntRange.Between(count, count), null, null, null);
		}

		// Token: 0x06007434 RID: 29748 RVA: 0x0028AC82 File Offset: 0x00288E82
		protected override bool IsValidWallAttachmentCell(LayoutWallAttatchmentParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
		{
			return parms.def != ThingDefOf.LifeSupportUnit && base.IsValidWallAttachmentCell(parms, cell, rot, room, map);
		}
	}
}
