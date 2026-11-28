using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE3 RID: 3555
	public class ThoughtWorker_Precept_SelfDislikedXenotype : ThoughtWorker_Precept
	{
		// Token: 0x060060A6 RID: 24742 RVA: 0x002300E8 File Offset: 0x0022E2E8
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.genes.XenotypeLabel.Named("XENOTYPENAME"));
		}

		// Token: 0x060060A7 RID: 24743 RVA: 0x0023010A File Offset: 0x0022E30A
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || p.genes == null)
			{
				return ThoughtState.Inactive;
			}
			return !p.Ideo.IsPreferredXenotype(p);
		}
	}
}
