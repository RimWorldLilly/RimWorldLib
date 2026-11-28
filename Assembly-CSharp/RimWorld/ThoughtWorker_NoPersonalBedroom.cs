using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E4C RID: 3660
	public class ThoughtWorker_NoPersonalBedroom : ThoughtWorker
	{
		// Token: 0x0600619A RID: 24986 RVA: 0x002337A0 File Offset: 0x002319A0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null || p.MapHeld == null || !p.MapHeld.IsPlayerHome || p.royalty.HighestTitleWithBedroomRequirements() == null || (MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive && !p.IsQuestLodger()))
			{
				return false;
			}
			return !p.royalty.HasPersonalBedroom();
		}

		// Token: 0x0600619B RID: 24987 RVA: 0x00233800 File Offset: 0x00231A00
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.royalty.HighestTitleWithBedroomRequirements().Named("TITLE")).CapitalizeFirst();
		}
	}
}
