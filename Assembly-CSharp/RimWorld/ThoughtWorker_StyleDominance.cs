using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD0 RID: 3536
	public class ThoughtWorker_StyleDominance : ThoughtWorker_Precept
	{
		// Token: 0x06006071 RID: 24689 RVA: 0x0022F488 File Offset: 0x0022D688
		public override string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.Ideo.adjective.Named("ADJECTIVE")).CapitalizeFirst();
		}

		// Token: 0x06006072 RID: 24690 RVA: 0x0022F4C0 File Offset: 0x0022D6C0
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.Ideo.adjective.Named("ADJECTIVE"), (from s in p.Ideo.thingStyleCategories
			select s.category.LabelCap.Resolve()).ToCommaList(true, false).Named("STYLES")).CapitalizeFirst();
		}

		// Token: 0x06006073 RID: 24691 RVA: 0x0022F538 File Offset: 0x0022D738
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			int styleDominanceThoughtIndex = p.styleObserver.StyleDominanceThoughtIndex;
			if (styleDominanceThoughtIndex >= 0)
			{
				return ThoughtState.ActiveAtStage(styleDominanceThoughtIndex);
			}
			return false;
		}
	}
}
