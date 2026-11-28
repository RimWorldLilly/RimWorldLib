using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E4B RID: 3659
	public class ThoughtWorker_NoThroneroom : ThoughtWorker
	{
		// Token: 0x06006198 RID: 24984 RVA: 0x00233738 File Offset: 0x00231938
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null || p.MapHeld == null || !p.MapHeld.IsPlayerHome || p.royalty.HighestTitleWithThroneRoomRequirements() == null || (MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive && !p.IsQuestLodger()))
			{
				return false;
			}
			return p.ownership.AssignedThrone == null;
		}
	}
}
