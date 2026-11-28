using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E49 RID: 3657
	public class ThoughtWorker_ThroneRoomRequirementsNotMet : ThoughtWorker_RoomRequirementsNotMet
	{
		// Token: 0x06006192 RID: 24978 RVA: 0x00233683 File Offset: 0x00231883
		protected override IEnumerable<string> UnmetRequirements(Pawn p)
		{
			return p.royalty.GetUnmetThroneroomRequirements(false, false);
		}

		// Token: 0x06006193 RID: 24979 RVA: 0x00233692 File Offset: 0x00231892
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(this.UnmetRequirements(p).ToLineList("- ", false), p.royalty.HighestTitleWithThroneRoomRequirements().Named("TITLE"));
		}
	}
}
