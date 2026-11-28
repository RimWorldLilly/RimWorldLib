using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DEF RID: 3567
	public class ThoughtWorker_NeedRoomSize : ThoughtWorker
	{
		// Token: 0x060060C4 RID: 24772 RVA: 0x00230674 File Offset: 0x0022E874
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.roomsize == null)
			{
				return ThoughtState.Inactive;
			}
			if (!p.Awake())
			{
				return ThoughtState.Inactive;
			}
			Room room = p.GetRoom(RegionType.Set_All);
			if (room == null || room.PsychologicallyOutdoors)
			{
				return ThoughtState.Inactive;
			}
			RoomSizeCategory curCategory = p.needs.roomsize.CurCategory;
			if (p.Ideo != null && curCategory < RoomSizeCategory.Normal && p.needs.PrefersIndoors)
			{
				return ThoughtState.Inactive;
			}
			switch (curCategory)
			{
			case RoomSizeCategory.VeryCramped:
				return ThoughtState.ActiveAtStage(0);
			case RoomSizeCategory.Cramped:
				return ThoughtState.ActiveAtStage(1);
			case RoomSizeCategory.Normal:
				return ThoughtState.Inactive;
			case RoomSizeCategory.Spacious:
				return ThoughtState.ActiveAtStage(2);
			default:
				throw new InvalidOperationException("Unknown RoomSizeCategory");
			}
		}
	}
}
