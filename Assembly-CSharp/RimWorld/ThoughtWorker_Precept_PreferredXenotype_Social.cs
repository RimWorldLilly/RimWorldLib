using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE1 RID: 3553
	public class ThoughtWorker_Precept_PreferredXenotype_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x060060A2 RID: 24738 RVA: 0x0022FF40 File Offset: 0x0022E140
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || otherPawn.genes == null)
			{
				return ThoughtState.Inactive;
			}
			if (!p.Ideo.PreferredXenotypes.Any<XenotypeDef>() && !p.Ideo.PreferredCustomXenotypes.Any<CustomXenotype>())
			{
				return ThoughtState.Inactive;
			}
			if (p.Ideo.IsPreferredXenotype(otherPawn))
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.ActiveAtStage(1);
		}
	}
}
