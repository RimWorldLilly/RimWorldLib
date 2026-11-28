using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E48 RID: 3656
	public abstract class ThoughtWorker_RoomRequirementsNotMet : ThoughtWorker
	{
		// Token: 0x0600618E RID: 24974
		protected abstract IEnumerable<string> UnmetRequirements(Pawn p);

		// Token: 0x0600618F RID: 24975 RVA: 0x00233637 File Offset: 0x00231837
		protected bool Active(Pawn p)
		{
			return p.royalty != null && p.MapHeld != null && p.MapHeld.IsPlayerHome && this.UnmetRequirements(p).Any<string>();
		}

		// Token: 0x06006190 RID: 24976 RVA: 0x00233664 File Offset: 0x00231864
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!this.Active(p))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
