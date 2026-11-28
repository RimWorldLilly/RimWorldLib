using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBE RID: 3518
	public class ThoughtWorker_Precept_AltarSharing : ThoughtWorker_Precept
	{
		// Token: 0x0600603C RID: 24636 RVA: 0x0022EBB8 File Offset: 0x0022CDB8
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return this.SharedAltar(p) != null;
		}

		// Token: 0x0600603D RID: 24637 RVA: 0x0022EBC9 File Offset: 0x0022CDC9
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(this.SharedAltar(p).Named("ALTAR"));
		}

		// Token: 0x0600603E RID: 24638 RVA: 0x0022EBE8 File Offset: 0x0022CDE8
		private Thing SharedAltar(Pawn pawn)
		{
			if (!pawn.Spawned || pawn.Ideo == null)
			{
				return null;
			}
			ThoughtWorker_Precept_AltarSharing.tmpPawnIdeoBuildingRooms.Clear();
			ThoughtWorker_Precept_AltarSharing.tmpDifferentIdeoStyleRooms.Clear();
			foreach (Thing thing in pawn.Map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.Styleable)))
			{
				CompStyleable compStyleable = ((ThingWithComps)thing).compStyleable;
				if (compStyleable.Ideo != null && !thing.DestroyedOrNull() && thing.Map == pawn.Map)
				{
					if (compStyleable.Ideo == pawn.Ideo)
					{
						Building building = thing as Building;
						if (building != null)
						{
							Room room = building.GetRoom(RegionType.Set_All);
							if (room != null && !room.TouchesMapEdge)
							{
								if (ThoughtWorker_Precept_AltarSharing.tmpDifferentIdeoStyleRooms.Contains(room))
								{
									return building;
								}
								ThoughtWorker_Precept_AltarSharing.tmpPawnIdeoBuildingRooms[room] = building;
							}
						}
					}
					else
					{
						Room room2 = thing.GetRoom(RegionType.Set_All);
						if (room2 != null && !room2.TouchesMapEdge)
						{
							Building result;
							if (ThoughtWorker_Precept_AltarSharing.tmpPawnIdeoBuildingRooms.TryGetValue(room2, out result))
							{
								return result;
							}
							ThoughtWorker_Precept_AltarSharing.tmpDifferentIdeoStyleRooms.Add(room2);
						}
					}
				}
			}
			return null;
		}

		// Token: 0x040035F0 RID: 13808
		private static Dictionary<Room, Building> tmpPawnIdeoBuildingRooms = new Dictionary<Room, Building>();

		// Token: 0x040035F1 RID: 13809
		private static HashSet<Room> tmpDifferentIdeoStyleRooms = new HashSet<Room>();
	}
}
