using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB3 RID: 4019
	public class MutantDef : Def, IRenderNodePropertiesParent
	{
		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x06006619 RID: 26137 RVA: 0x00244CB8 File Offset: 0x00242EB8
		public bool HasDefinedGraphicProperties
		{
			get
			{
				return !this.renderNodeProperties.NullOrEmpty<PawnRenderNodeProperties>();
			}
		}

		// Token: 0x1700117C RID: 4476
		// (get) Token: 0x0600661A RID: 26138 RVA: 0x00244CC8 File Offset: 0x00242EC8
		public List<PawnRenderNodeProperties> RenderNodeProperties
		{
			get
			{
				return this.renderNodeProperties;
			}
		}

		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x0600661B RID: 26139 RVA: 0x00244CD0 File Offset: 0x00242ED0
		public List<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				if (this.cachedDisabledWorkTypes == null)
				{
					this.cachedDisabledWorkTypes = new List<WorkTypeDef>();
					List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (!this.AllowsWorkType(allDefsListForReading[i]))
						{
							this.cachedDisabledWorkTypes.Add(allDefsListForReading[i]);
						}
					}
				}
				return this.cachedDisabledWorkTypes;
			}
		}

		// Token: 0x0600661C RID: 26140 RVA: 0x00244D30 File Offset: 0x00242F30
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
		}

		// Token: 0x0600661D RID: 26141 RVA: 0x00244D7C File Offset: 0x00242F7C
		public string GetBodyGraphicPath(Pawn pawn)
		{
			for (int i = 0; i < this.bodyTypeGraphicPaths.Count; i++)
			{
				if (this.bodyTypeGraphicPaths[i].bodyType == pawn.story.bodyType)
				{
					return this.bodyTypeGraphicPaths[i].texturePath;
				}
			}
			return null;
		}

		// Token: 0x0600661E RID: 26142 RVA: 0x00244DD0 File Offset: 0x00242FD0
		public bool StyleItemAllowed(StyleItemDef styleItem)
		{
			if (!ModLister.AnomalyInstalled)
			{
				return true;
			}
			bool flag = styleItem is HairDef;
			bool flag2 = styleItem is BeardDef;
			if (!flag && !flag2)
			{
				return true;
			}
			if (flag)
			{
				if (this.hairTagFilter != null && !this.hairTagFilter.Allows(styleItem.styleTags))
				{
					return false;
				}
			}
			else if (flag2 && this.beardTagFilter != null && !this.beardTagFilter.Allows(styleItem.styleTags))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600661F RID: 26143 RVA: 0x00244E43 File Offset: 0x00243043
		private bool AllowsWorkType(WorkTypeDef workType)
		{
			return (this.workDisables & workType.workTags) == WorkTags.None;
		}

		// Token: 0x06006620 RID: 26144 RVA: 0x00244E58 File Offset: 0x00243058
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.renderNodeProperties != null)
			{
				for (int i = 0; i < this.renderNodeProperties.Count; i++)
				{
					this.renderNodeProperties[i].ResolveReferencesRecursive();
				}
			}
		}

		// Token: 0x04003DF8 RID: 15864
		public HediffDef hediff;

		// Token: 0x04003DF9 RID: 15865
		public ThinkTreeDef thinkTree;

		// Token: 0x04003DFA RID: 15866
		public ThinkTreeDef thinkTreeConstant;

		// Token: 0x04003DFB RID: 15867
		public DevelopmentalStage allowedDevelopmentalStages = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04003DFC RID: 15868
		public bool clearMutantStatusOnDeath;

		// Token: 0x04003DFD RID: 15869
		public bool consideredSubhuman;

		// Token: 0x04003DFE RID: 15870
		public WorkTags workDisables;

		// Token: 0x04003DFF RID: 15871
		public bool disablesIdeo;

		// Token: 0x04003E00 RID: 15872
		public bool disableAging;

		// Token: 0x04003E01 RID: 15873
		public bool disableApparel;

		// Token: 0x04003E02 RID: 15874
		public List<GeneDef> disablesGenes = new List<GeneDef>();

		// Token: 0x04003E03 RID: 15875
		public bool preventsMentalBreaks;

		// Token: 0x04003E04 RID: 15876
		public bool canOpenDoors = true;

		// Token: 0x04003E05 RID: 15877
		public bool canOpenAnyDoor;

		// Token: 0x04003E06 RID: 15878
		public bool canCarryPawns = true;

		// Token: 0x04003E07 RID: 15879
		public bool canTend = true;

		// Token: 0x04003E08 RID: 15880
		public bool canTravelInCaravan = true;

		// Token: 0x04003E09 RID: 15881
		public bool canOwnCaravan = true;

		// Token: 0x04003E0A RID: 15882
		public bool respectsAllowedArea = true;

		// Token: 0x04003E0B RID: 15883
		public bool canGainXP = true;

		// Token: 0x04003E0C RID: 15884
		public bool disableEatingAtTable;

		// Token: 0x04003E0D RID: 15885
		public bool disableOwnership;

		// Token: 0x04003E0E RID: 15886
		public bool disablePolicies;

		// Token: 0x04003E0F RID: 15887
		public bool disableTitles;

		// Token: 0x04003E10 RID: 15888
		public bool disableFlying;

		// Token: 0x04003E11 RID: 15889
		[MustTranslate]
		public string namePrefix = "";

		// Token: 0x04003E12 RID: 15890
		public bool hideLabel;

		// Token: 0x04003E13 RID: 15891
		public EntityCodexEntryDef codexEntry;

		// Token: 0x04003E14 RID: 15892
		public bool showInScenarioEditor = true;

		// Token: 0x04003E15 RID: 15893
		public bool overrideLabel;

		// Token: 0x04003E16 RID: 15894
		public bool overrideInspectString;

		// Token: 0x04003E17 RID: 15895
		public bool showJobReport = true;

		// Token: 0x04003E18 RID: 15896
		public List<Type> whitelistedFloatMenuProviders;

		// Token: 0x04003E19 RID: 15897
		public bool disableNeeds;

		// Token: 0x04003E1A RID: 15898
		public List<NeedDef> needWhitelist;

		// Token: 0x04003E1B RID: 15899
		public bool overrideFoodType;

		// Token: 0x04003E1C RID: 15900
		public FoodTypeFlags foodType;

		// Token: 0x04003E1D RID: 15901
		public bool allowEatingCorpses;

		// Token: 0x04003E1E RID: 15902
		public bool useCorpseGraphics;

		// Token: 0x04003E1F RID: 15903
		public Color? skinColorOverride;

		// Token: 0x04003E20 RID: 15904
		public Color? skinColorTint;

		// Token: 0x04003E21 RID: 15905
		public float skinColorTintStrength = 0.5f;

		// Token: 0x04003E22 RID: 15906
		public Color? hairColorOverride;

		// Token: 0x04003E23 RID: 15907
		public List<BodyTypeGraphicData> bodyTypeGraphicPaths = new List<BodyTypeGraphicData>();

		// Token: 0x04003E24 RID: 15908
		public List<HeadTypeDef> forcedHeadTypes;

		// Token: 0x04003E25 RID: 15909
		public TagFilter hairTagFilter;

		// Token: 0x04003E26 RID: 15910
		public TagFilter beardTagFilter;

		// Token: 0x04003E27 RID: 15911
		private List<PawnRenderNodeProperties> renderNodeProperties;

		// Token: 0x04003E28 RID: 15912
		public AnimationDef standingAnimation;

		// Token: 0x04003E29 RID: 15913
		public bool makesFootprints = true;

		// Token: 0x04003E2A RID: 15914
		public bool canBleed = true;

		// Token: 0x04003E2B RID: 15915
		public ThingDef bloodDef;

		// Token: 0x04003E2C RID: 15916
		public ThingDef bloodSmearDef;

		// Token: 0x04003E2D RID: 15917
		public bool entitledToMedicalCare = true;

		// Token: 0x04003E2E RID: 15918
		public bool isImmuneToInfections;

		// Token: 0x04003E2F RID: 15919
		public bool canUseDrugs;

		// Token: 0x04003E30 RID: 15920
		public List<ThingDef> drugWhitelist = new List<ThingDef>();

		// Token: 0x04003E31 RID: 15921
		public Color? woundColor;

		// Token: 0x04003E32 RID: 15922
		public bool preventIllnesses = true;

		// Token: 0x04003E33 RID: 15923
		public bool removeChronicIllnesses;

		// Token: 0x04003E34 RID: 15924
		public bool removePermanentInjuries;

		// Token: 0x04003E35 RID: 15925
		public bool removeAddictions;

		// Token: 0x04003E36 RID: 15926
		public bool removeAllInjuries;

		// Token: 0x04003E37 RID: 15927
		public bool restoreLegs;

		// Token: 0x04003E38 RID: 15928
		public bool terminatePregnancy;

		// Token: 0x04003E39 RID: 15929
		public bool partsCleanAndDroppable = true;

		// Token: 0x04003E3A RID: 15930
		public bool clearsEgo;

		// Token: 0x04003E3B RID: 15931
		public bool breathesAir = true;

		// Token: 0x04003E3C RID: 15932
		public List<StartingHediff> givesHediffs = new List<StartingHediff>();

		// Token: 0x04003E3D RID: 15933
		public List<HediffDef> removesHediffs = new List<HediffDef>();

		// Token: 0x04003E3E RID: 15934
		public List<HediffGiver> hediffGivers;

		// Token: 0x04003E3F RID: 15935
		[MustTranslate]
		public string deathLetter;

		// Token: 0x04003E40 RID: 15936
		[MustTranslate]
		public string deathLetterExtra;

		// Token: 0x04003E41 RID: 15937
		public SoundDef soundCall;

		// Token: 0x04003E42 RID: 15938
		public SoundDef soundAttack;

		// Token: 0x04003E43 RID: 15939
		public SoundDef soundWounded;

		// Token: 0x04003E44 RID: 15940
		public SoundDef soundDeath;

		// Token: 0x04003E45 RID: 15941
		public SoundDef soundAngry;

		// Token: 0x04003E46 RID: 15942
		public List<VerbProperties> verbs;

		// Token: 0x04003E47 RID: 15943
		public List<Tool> tools;

		// Token: 0x04003E48 RID: 15944
		public List<AbilityDef> abilities = new List<AbilityDef>();

		// Token: 0x04003E49 RID: 15945
		public List<AbilityDef> abilityWhitelist = new List<AbilityDef>();

		// Token: 0x04003E4A RID: 15946
		public bool passive;

		// Token: 0x04003E4B RID: 15947
		public bool canBeDrafted = true;

		// Token: 0x04003E4C RID: 15948
		public bool canAttackWhileCrawling;

		// Token: 0x04003E4D RID: 15949
		public float deathOnDownedChance = -1f;

		// Token: 0x04003E4E RID: 15950
		public float soundAttackChance = -1f;

		// Token: 0x04003E4F RID: 15951
		public List<ThingDefCountClass> killedLeavings;

		// Token: 0x04003E50 RID: 15952
		public bool psychicShockUntargetable;

		// Token: 0x04003E51 RID: 15953
		public bool disableHostilityResponse;

		// Token: 0x04003E52 RID: 15954
		public FactionDef defaultFaction;

		// Token: 0x04003E53 RID: 15955
		public bool isConsideredCorpse;

		// Token: 0x04003E54 RID: 15956
		public ThoughtDef relativeTurnedThought;

		// Token: 0x04003E55 RID: 15957
		public bool incapableOfSocialInteractions;

		// Token: 0x04003E56 RID: 15958
		public bool tameable = true;

		// Token: 0x04003E57 RID: 15959
		public bool canBeCapturedToHoldingPlatform;

		// Token: 0x04003E58 RID: 15960
		public bool producesBioferrite;

		// Token: 0x04003E59 RID: 15961
		public float anomalyKnowledgeOffset;

		// Token: 0x04003E5A RID: 15962
		public KnowledgeCategoryDef knowledgeCategory;

		// Token: 0x04003E5B RID: 15963
		private List<WorkTypeDef> cachedDisabledWorkTypes;
	}
}
