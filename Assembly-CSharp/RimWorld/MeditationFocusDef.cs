using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA3 RID: 4003
	public class MeditationFocusDef : Def
	{
		// Token: 0x060065F6 RID: 26102 RVA: 0x00244365 File Offset: 0x00242565
		public bool CanPawnUse(Pawn p)
		{
			return MeditationFocusTypeAvailabilityCache.PawnCanUse(p, this);
		}

		// Token: 0x060065F7 RID: 26103 RVA: 0x00244370 File Offset: 0x00242570
		public string EnablingThingsExplanation(Pawn pawn)
		{
			MeditationFocusDef.tmpReasons.Clear();
			if (this.requiresRoyalTitle && pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0)
			{
				RoyalTitle royalTitle = pawn.royalty.AllTitlesInEffectForReading.MaxBy((RoyalTitle t) => t.def.seniority);
				MeditationFocusDef.tmpReasons.Add("MeditationFocusEnabledByTitle".Translate(royalTitle.def.GetLabelCapFor(pawn).Named("TITLE"), royalTitle.faction.Named("FACTION")).Resolve());
			}
			if (pawn.story != null)
			{
				BackstoryDef adulthood = pawn.story.Adulthood;
				BackstoryDef childhood = pawn.story.Childhood;
				if (!this.requiresRoyalTitle && this.requiredBackstoriesAny.Count == 0)
				{
					for (int i = 0; i < this.incompatibleBackstoriesAny.Count; i++)
					{
						BackstoryCategoryAndSlot backstoryCategoryAndSlot = this.incompatibleBackstoriesAny[i];
						BackstoryDef backstoryDef = (backstoryCategoryAndSlot.slot == BackstorySlot.Adulthood) ? adulthood : childhood;
						if (!backstoryDef.spawnCategories.Contains(backstoryCategoryAndSlot.categoryName))
						{
							MeditationFocusDef.<EnablingThingsExplanation>g__AddBackstoryReason|5_0(backstoryCategoryAndSlot.slot, backstoryDef);
						}
					}
					for (int j = 0; j < DefDatabase<TraitDef>.AllDefsListForReading.Count; j++)
					{
						TraitDef traitDef = DefDatabase<TraitDef>.AllDefsListForReading[j];
						List<MeditationFocusDef> disallowedMeditationFocusTypes = traitDef.degreeDatas[0].disallowedMeditationFocusTypes;
						if (disallowedMeditationFocusTypes != null && disallowedMeditationFocusTypes.Contains(this))
						{
							MeditationFocusDef.tmpReasons.Add("MeditationFocusDisabledByTrait".Translate() + ": " + traitDef.degreeDatas[0].GetLabelCapFor(pawn) + ".");
						}
					}
				}
				for (int k = 0; k < this.requiredBackstoriesAny.Count; k++)
				{
					BackstoryCategoryAndSlot backstoryCategoryAndSlot2 = this.requiredBackstoriesAny[k];
					BackstoryDef backstoryDef2 = (backstoryCategoryAndSlot2.slot == BackstorySlot.Adulthood) ? adulthood : childhood;
					if (backstoryDef2.spawnCategories.Contains(backstoryCategoryAndSlot2.categoryName))
					{
						MeditationFocusDef.<EnablingThingsExplanation>g__AddBackstoryReason|5_0(backstoryCategoryAndSlot2.slot, backstoryDef2);
					}
				}
				for (int l = 0; l < pawn.story.traits.allTraits.Count; l++)
				{
					Trait trait = pawn.story.traits.allTraits[l];
					if (!trait.Suppressed)
					{
						List<MeditationFocusDef> allowedMeditationFocusTypes = trait.CurrentData.allowedMeditationFocusTypes;
						if (allowedMeditationFocusTypes != null && allowedMeditationFocusTypes.Contains(this))
						{
							MeditationFocusDef.tmpReasons.Add("MeditationFocusEnabledByTrait".Translate() + ": " + trait.LabelCap + ".");
						}
					}
				}
			}
			for (int m = 0; m < pawn.health.hediffSet.hediffs.Count; m++)
			{
				HediffDef def = pawn.health.hediffSet.hediffs[m].def;
				if (def.allowedMeditationFocusTypes.NotNullAndContains(this))
				{
					MeditationFocusDef.tmpReasons.Add("MeditationFocusEnabledByHediff".Translate() + ": " + def.LabelCap + ".");
				}
			}
			return MeditationFocusDef.tmpReasons.ToLineList("  - ", true);
		}

		// Token: 0x060065FA RID: 26106 RVA: 0x00244710 File Offset: 0x00242910
		[CompilerGenerated]
		internal static void <EnablingThingsExplanation>g__AddBackstoryReason|5_0(BackstorySlot slot, BackstoryDef backstory)
		{
			if (slot == BackstorySlot.Adulthood)
			{
				MeditationFocusDef.tmpReasons.Add("MeditationFocusEnabledByAdulthood".Translate() + ": " + backstory.title.CapitalizeFirst() + ".");
				return;
			}
			MeditationFocusDef.tmpReasons.Add("MeditationFocusEnabledByChildhood".Translate() + ": " + backstory.title.CapitalizeFirst() + ".");
		}

		// Token: 0x04003D67 RID: 15719
		public bool requiresRoyalTitle;

		// Token: 0x04003D68 RID: 15720
		public List<BackstoryCategoryAndSlot> requiredBackstoriesAny = new List<BackstoryCategoryAndSlot>();

		// Token: 0x04003D69 RID: 15721
		public List<BackstoryCategoryAndSlot> incompatibleBackstoriesAny = new List<BackstoryCategoryAndSlot>();

		// Token: 0x04003D6A RID: 15722
		private static List<string> tmpReasons = new List<string>();
	}
}
