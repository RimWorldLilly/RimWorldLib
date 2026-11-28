using System;

namespace Verse.AI
{
	// Token: 0x020008AF RID: 2223
	public static class PawnLocalAwareness
	{
		// Token: 0x060044ED RID: 17645 RVA: 0x001B0A88 File Offset: 0x001AEC88
		public static bool AnimalAwareOf(this Pawn p, Thing t)
		{
			return p.RaceProps.ToolUser || p.Faction != null || ((float)(p.Position - t.Position).LengthHorizontalSquared <= 900f && p.GetRoom(RegionType.Set_All) == t.GetRoom(RegionType.Set_All) && GenSight.LineOfSight(p.Position, t.Position, p.Map));
		}

		// Token: 0x04002CC4 RID: 11460
		private const float SightRadius = 30f;
	}
}
