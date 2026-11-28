using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016EF RID: 5871
	public class RitualPatternDef : Def
	{
		// Token: 0x17001935 RID: 6453
		// (get) Token: 0x060092CF RID: 37583 RVA: 0x0035A548 File Offset: 0x00358748
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					if (this.iconPathOverride != null)
					{
						this.icon = ContentFinder<Texture2D>.Get(this.iconPathOverride, true);
					}
					else
					{
						this.icon = null;
					}
				}
				return this.icon;
			}
		}

		// Token: 0x060092D0 RID: 37584 RVA: 0x0035A584 File Offset: 0x00358784
		public void Fill(Precept_Ritual ritual)
		{
			ritual.nameMaker = this.nameMaker;
			if (!this.ritualObligationTriggers.NullOrEmpty<RitualObligationTriggerProperties>())
			{
				ritual.obligationTriggers.Clear();
				foreach (RitualObligationTriggerProperties ritualObligationTriggerProperties in this.ritualObligationTriggers)
				{
					RitualObligationTrigger instance = ritualObligationTriggerProperties.GetInstance(ritual);
					ritual.obligationTriggers.Add(instance);
					instance.Init(ritualObligationTriggerProperties);
				}
			}
			if (this.ritualOutcomeEffect != null)
			{
				ritual.outcomeEffect = this.ritualOutcomeEffect.GetInstance();
			}
			Precept_Ritual ritual2 = ritual;
			RitualObligationTargetFilterDef ritualObligationTargetFilterDef = this.ritualObligationTargetFilter;
			ritual2.obligationTargetFilter = ((ritualObligationTargetFilterDef != null) ? ritualObligationTargetFilterDef.GetInstance() : null);
			if (ritual.obligationTargetFilter != null)
			{
				ritual.obligationTargetFilter.parent = ritual;
			}
			Precept_Ritual ritual3 = ritual;
			RitualTargetFilterDef ritualTargetFilterDef = this.ritualTargetFilter;
			ritual3.targetFilter = ((ritualTargetFilterDef != null) ? ritualTargetFilterDef.GetInstance() : null);
			ritual.behavior = this.ritualBehavior.GetInstance();
			ritual.ritualOnlyForIdeoMembers = this.ritualOnlyForIdeoMembers;
			ritual.ritualExpectedDesc = this.ritualExpectedDesc;
			ritual.ritualExpectedDescNoAdjective = this.ritualExpectedDescNoAdjective;
			ritual.descOverride = this.descOverride;
			ritual.shortDescOverride = this.shortDescOverride;
			ritual.iconPathOverride = this.iconPathOverride;
			ritual.cancelIconPathOverride = this.cancelIconPathOverride;
			ritual.patternGroupTag = this.patternGroupTag;
			ritual.minTechLevel = this.minTechLevel;
			ritual.maxTechLevel = this.maxTechLevel;
			ritual.showIdeoIconsInDialog = this.showIdeoIconsInDialog;
			ritual.playsIdeoMusic = this.playsIdeoMusic;
			ritual.ritualExplanation = this.ritualExplanation;
			ritual.canBeAnytime = this.canStartAnytime;
			ritual.allowOtherInstances = this.allowOtherInstances;
			ritual.mergeGizmosForObligations = this.mergeGizmosForObligations;
			ritual.canMergeGizmosFromDifferentIdeos = this.canMergeGizmosFromDifferentIdeos;
			ritual.layerWhitelist = this.layerWhitelist;
			ritual.layerBlacklist = this.layerBlacklist;
			ritual.ignoreExtremeTemperatures = this.ignoreExtremeTemperatures;
			if (this.alwaysStartAnytime)
			{
				ritual.isAnytime = true;
			}
			else if (this.canStartAnytime)
			{
				Precept_Ritual precept_Ritual = ritual.ideo.PreceptsListForReading.OfType<Precept_Ritual>().FirstOrDefault((Precept_Ritual p) => p != ritual && p.behavior != null && p.behavior.def == this.ritualBehavior);
				int num = ritual.ideo.PreceptsListForReading.OfType<Precept_Ritual>().Count((Precept_Ritual r) => r != ritual && r.isAnytime && r.def.ritualPatternBase != null && r.def.ritualPatternBase.canStartAnytime);
				int num2 = ritual.ideo.PreceptsListForReading.OfType<Precept_Ritual>().Count((Precept_Ritual r) => r != ritual && !r.isAnytime && r.def.ritualPatternBase != null && r.def.ritualPatternBase.canStartAnytime);
				if (precept_Ritual != null)
				{
					ritual.isAnytime = false;
					precept_Ritual.isAnytime = false;
				}
				else if (num != num2)
				{
					ritual.isAnytime = (num2 > num);
				}
				else
				{
					ritual.isAnytime = Rand.Bool;
				}
			}
			else
			{
				ritual.isAnytime = false;
			}
			if (ritual.SupportsAttachableOutcomeEffect)
			{
				ritual.attachableOutcomeEffect = (from d in DefDatabase<RitualAttachableOutcomeEffectDef>.AllDefs
				where base.<Fill>g__ValidateAttachedOutcome|0(d, false)
				select d).RandomElementWithFallback(null);
				if (ritual.attachableOutcomeEffect == null)
				{
					ritual.attachableOutcomeEffect = (from d in DefDatabase<RitualAttachableOutcomeEffectDef>.AllDefs
					where base.<Fill>g__ValidateAttachedOutcome|0(d, true)
					select d).RandomElementWithFallback(null);
				}
			}
			ritual.generatedAttachedReward = true;
			ritual.sourcePattern = this;
		}

		// Token: 0x060092D1 RID: 37585 RVA: 0x0035A98C File Offset: 0x00358B8C
		public bool CanFactionUse(FactionDef faction)
		{
			return faction == null || (Find.IdeoManager != null && Find.IdeoManager.classicMode) || RitualPatternDef.CanUseWithTechLevel(faction.techLevel, this.minTechLevel, this.maxTechLevel);
		}

		// Token: 0x060092D2 RID: 37586 RVA: 0x0035A9BD File Offset: 0x00358BBD
		public static bool CanUseWithTechLevel(TechLevel level, TechLevel min, TechLevel max)
		{
			return min <= level && (max == TechLevel.Undefined || max >= level);
		}

		// Token: 0x040058D6 RID: 22742
		public RulePackDef nameMaker;

		// Token: 0x040058D7 RID: 22743
		public FloatRange ritualFreeStartIntervalDaysRange;

		// Token: 0x040058D8 RID: 22744
		public List<RitualObligationTriggerProperties> ritualObligationTriggers;

		// Token: 0x040058D9 RID: 22745
		public RitualObligationTargetFilterDef ritualObligationTargetFilter;

		// Token: 0x040058DA RID: 22746
		public RitualTargetFilterDef ritualTargetFilter;

		// Token: 0x040058DB RID: 22747
		public RitualBehaviorDef ritualBehavior;

		// Token: 0x040058DC RID: 22748
		public RitualOutcomeEffectDef ritualOutcomeEffect;

		// Token: 0x040058DD RID: 22749
		public bool ritualOnlyForIdeoMembers = true;

		// Token: 0x040058DE RID: 22750
		public bool canStartAnytime;

		// Token: 0x040058DF RID: 22751
		public bool alwaysStartAnytime;

		// Token: 0x040058E0 RID: 22752
		public bool allowOtherInstances;

		// Token: 0x040058E1 RID: 22753
		public bool playsIdeoMusic = true;

		// Token: 0x040058E2 RID: 22754
		public bool ignoreConsumableBuildingRequirement;

		// Token: 0x040058E3 RID: 22755
		public bool mergeGizmosForObligations;

		// Token: 0x040058E4 RID: 22756
		public bool canMergeGizmosFromDifferentIdeos = true;

		// Token: 0x040058E5 RID: 22757
		public bool ignoreExtremeTemperatures;

		// Token: 0x040058E6 RID: 22758
		public TechLevel minTechLevel;

		// Token: 0x040058E7 RID: 22759
		public TechLevel maxTechLevel;

		// Token: 0x040058E8 RID: 22760
		public bool showIdeoIconsInDialog = true;

		// Token: 0x040058E9 RID: 22761
		[MustTranslate]
		public string ritualExpectedDesc;

		// Token: 0x040058EA RID: 22762
		[MustTranslate]
		public string ritualExpectedDescNoAdjective;

		// Token: 0x040058EB RID: 22763
		[MustTranslate]
		public string shortDescOverride;

		// Token: 0x040058EC RID: 22764
		[MustTranslate]
		public string descOverride;

		// Token: 0x040058ED RID: 22765
		[MustTranslate]
		public string ritualExplanation;

		// Token: 0x040058EE RID: 22766
		[MustTranslate]
		public string beginRitualOverride;

		// Token: 0x040058EF RID: 22767
		[NoTranslate]
		public string iconPathOverride;

		// Token: 0x040058F0 RID: 22768
		[NoTranslate]
		public string cancelIconPathOverride;

		// Token: 0x040058F1 RID: 22769
		[NoTranslate]
		public List<string> tags;

		// Token: 0x040058F2 RID: 22770
		[NoTranslate]
		public string patternGroupTag;

		// Token: 0x040058F3 RID: 22771
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x040058F4 RID: 22772
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x040058F5 RID: 22773
		private Texture2D icon;
	}
}
