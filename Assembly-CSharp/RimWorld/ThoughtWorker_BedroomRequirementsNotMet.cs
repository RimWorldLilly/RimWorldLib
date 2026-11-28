using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E4A RID: 3658
	public class ThoughtWorker_BedroomRequirementsNotMet : ThoughtWorker_RoomRequirementsNotMet
	{
		// Token: 0x06006195 RID: 24981 RVA: 0x002336D3 File Offset: 0x002318D3
		protected override IEnumerable<string> UnmetRequirements(Pawn p)
		{
			return p.royalty.GetUnmetBedroomRequirements(false, false);
		}

		// Token: 0x06006196 RID: 24982 RVA: 0x002336E4 File Offset: 0x002318E4
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(this.UnmetRequirements(p).ToLineList("- ", false), p.royalty.HighestTitleWithBedroomRequirements().Named("TITLE")).CapitalizeFirst();
		}
	}
}
