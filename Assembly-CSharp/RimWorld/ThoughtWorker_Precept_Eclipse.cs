using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DCF RID: 3535
	public class ThoughtWorker_Precept_Eclipse : ThoughtWorker_Precept
	{
		// Token: 0x0600606F RID: 24687 RVA: 0x0022F418 File Offset: 0x0022D618
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.Spawned)
			{
				return false;
			}
			if (!p.Awake() || PawnUtility.IsBiologicallyOrArtificiallyBlind(p))
			{
				return false;
			}
			Room room = p.GetRoom(RegionType.Set_All);
			if (room != null && !room.PsychologicallyOutdoors)
			{
				return false;
			}
			return p.Map.GameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse);
		}
	}
}
