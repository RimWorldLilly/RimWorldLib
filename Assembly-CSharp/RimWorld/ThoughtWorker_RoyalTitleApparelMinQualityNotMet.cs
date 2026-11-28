using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E47 RID: 3655
	public class ThoughtWorker_RoyalTitleApparelMinQualityNotMet : ThoughtWorker
	{
		// Token: 0x06006189 RID: 24969 RVA: 0x002334B0 File Offset: 0x002316B0
		private RoyalTitle Validate(Pawn p, out QualityCategory minQuality)
		{
			minQuality = QualityCategory.Awful;
			foreach (RoyalTitle royalTitle in p.royalty.AllTitlesInEffectForReading)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				minQuality = royalTitle.def.requiredMinimumApparelQuality;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					QualityCategory qualityCategory;
					if (wornApparel[i].TryGetQuality(out qualityCategory) && qualityCategory < royalTitle.def.requiredMinimumApparelQuality)
					{
						return royalTitle;
					}
				}
			}
			return null;
		}

		// Token: 0x0600618A RID: 24970 RVA: 0x0023355C File Offset: 0x0023175C
		public override string PostProcessLabel(Pawn p, string label)
		{
			QualityCategory qualityCategory;
			RoyalTitle royalTitle = this.Validate(p, out qualityCategory);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return label.Formatted(royalTitle.Named("TITLE"), p.Named("PAWN"));
		}

		// Token: 0x0600618B RID: 24971 RVA: 0x002335A0 File Offset: 0x002317A0
		public override string PostProcessDescription(Pawn p, string description)
		{
			QualityCategory cat;
			RoyalTitle royalTitle = this.Validate(p, out cat);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return description.Formatted(royalTitle.Named("TITLE"), cat.GetLabel().Named("QUALITY"), p.Named("PAWN")).CapitalizeFirst();
		}

		// Token: 0x0600618C RID: 24972 RVA: 0x002335FC File Offset: 0x002317FC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null)
			{
				return false;
			}
			QualityCategory qualityCategory;
			if (this.Validate(p, out qualityCategory) == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
