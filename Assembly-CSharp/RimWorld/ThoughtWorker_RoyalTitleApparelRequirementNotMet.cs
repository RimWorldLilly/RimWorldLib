using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E46 RID: 3654
	public class ThoughtWorker_RoyalTitleApparelRequirementNotMet : ThoughtWorker
	{
		// Token: 0x06006183 RID: 24963 RVA: 0x00233308 File Offset: 0x00231508
		private static RoyalTitle Validate(Pawn p)
		{
			if (p.royalty == null || !p.royalty.allowApparelRequirements)
			{
				return null;
			}
			foreach (RoyalTitle royalTitle in p.royalty.AllTitlesInEffectForReading)
			{
				if (royalTitle.def.requiredApparel != null && royalTitle.def.requiredApparel.Count > 0)
				{
					for (int i = 0; i < royalTitle.def.requiredApparel.Count; i++)
					{
						ApparelRequirement apparelRequirement = royalTitle.def.requiredApparel[i];
						if (apparelRequirement.IsActive(p) && !apparelRequirement.IsMet(p))
						{
							return royalTitle;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06006184 RID: 24964 RVA: 0x002333D8 File Offset: 0x002315D8
		private static IEnumerable<string> GetAllRequiredApparelPerGroup(Pawn p)
		{
			ThoughtWorker_RoyalTitleApparelRequirementNotMet.<GetAllRequiredApparelPerGroup>d__1 <GetAllRequiredApparelPerGroup>d__ = new ThoughtWorker_RoyalTitleApparelRequirementNotMet.<GetAllRequiredApparelPerGroup>d__1(-2);
			<GetAllRequiredApparelPerGroup>d__.<>3__p = p;
			return <GetAllRequiredApparelPerGroup>d__;
		}

		// Token: 0x06006185 RID: 24965 RVA: 0x002333E8 File Offset: 0x002315E8
		public override string PostProcessLabel(Pawn p, string label)
		{
			RoyalTitle royalTitle = ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return label.Formatted(royalTitle.Named("TITLE"), p.Named("PAWN")).CapitalizeFirst();
		}

		// Token: 0x06006186 RID: 24966 RVA: 0x00233430 File Offset: 0x00231630
		public override string PostProcessDescription(Pawn p, string description)
		{
			RoyalTitle royalTitle = ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return description.Formatted(ThoughtWorker_RoyalTitleApparelRequirementNotMet.GetAllRequiredApparelPerGroup(p).Distinct<string>().ToLineList("- ", false), royalTitle.Named("TITLE"), p.Named("PAWN")).CapitalizeFirst();
		}

		// Token: 0x06006187 RID: 24967 RVA: 0x00233491 File Offset: 0x00231691
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p) == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}
	}
}
