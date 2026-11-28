using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC9 RID: 4041
	public class PreceptDef : Def
	{
		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x0600668C RID: 26252 RVA: 0x00246800 File Offset: 0x00244A00
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					if (this.iconPath != null)
					{
						this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
					}
					else if (!this.issue.iconPath.NullOrEmpty())
					{
						this.icon = this.issue.Icon;
					}
				}
				return this.icon;
			}
		}

		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x0600668D RID: 26253 RVA: 0x00246860 File Offset: 0x00244A60
		public PreceptWorker Worker
		{
			get
			{
				if (this.worker == null)
				{
					this.worker = (PreceptWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x0600668E RID: 26254 RVA: 0x00246894 File Offset: 0x00244A94
		public List<TraitRequirement> TraitsAffecting
		{
			get
			{
				if (this.traitsAffectingCached == null)
				{
					this.traitsAffectingCached = new List<TraitRequirement>();
					for (int i = 0; i < this.comps.Count; i++)
					{
						this.traitsAffectingCached.AddRange(this.comps[i].TraitsAffecting);
					}
					this.traitsAffectingCached.RemoveDuplicates((TraitRequirement a, TraitRequirement b) => a.def == b.def && ((a.degree != null) ? a.degree.Value : 0) == ((b.degree != null) ? b.degree.Value : 0));
				}
				return this.traitsAffectingCached;
			}
		}

		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x0600668F RID: 26255 RVA: 0x00246918 File Offset: 0x00244B18
		public List<string> RequiredMemeLabels
		{
			get
			{
				if (this.requiredMemeLabels == null && !this.requiredMemes.NullOrEmpty<MemeDef>())
				{
					this.requiredMemeLabels = new List<string>();
					foreach (MemeDef memeDef in this.requiredMemes)
					{
						this.requiredMemeLabels.Add(memeDef.label);
					}
				}
				return this.requiredMemeLabels;
			}
		}

		// Token: 0x06006690 RID: 26256 RVA: 0x0024699C File Offset: 0x00244B9C
		public override IEnumerable<string> ConfigErrors()
		{
			PreceptDef.<ConfigErrors>d__119 <ConfigErrors>d__ = new PreceptDef.<ConfigErrors>d__119(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06006691 RID: 26257 RVA: 0x002469AC File Offset: 0x00244BAC
		public override void PostLoad()
		{
			base.PostLoad();
			foreach (PreceptComp preceptComp in this.comps)
			{
				preceptComp.preceptDef = this;
			}
		}

		// Token: 0x04003F13 RID: 16147
		public Type preceptClass = typeof(Precept);

		// Token: 0x04003F14 RID: 16148
		public IssueDef issue;

		// Token: 0x04003F15 RID: 16149
		public List<PreceptComp> comps = new List<PreceptComp>();

		// Token: 0x04003F16 RID: 16150
		public List<AbilityStatModifiers> abilityStatFactors;

		// Token: 0x04003F17 RID: 16151
		public List<StatModifier> statOffsets;

		// Token: 0x04003F18 RID: 16152
		public List<StatModifier> statFactors;

		// Token: 0x04003F19 RID: 16153
		public List<ConditionalStatAffecter> conditionalStatAffecters;

		// Token: 0x04003F1A RID: 16154
		public float selectionWeight = 1f;

		// Token: 0x04003F1B RID: 16155
		public List<WorkTypeDef> opposedWorkTypes = new List<WorkTypeDef>();

		// Token: 0x04003F1C RID: 16156
		public PreceptImpact impact;

		// Token: 0x04003F1D RID: 16157
		public List<MemeDef> associatedMemes = new List<MemeDef>();

		// Token: 0x04003F1E RID: 16158
		public List<MemeDef> conflictingMemes = new List<MemeDef>();

		// Token: 0x04003F1F RID: 16159
		public List<MemeDef> requiredMemes = new List<MemeDef>();

		// Token: 0x04003F20 RID: 16160
		public bool visible = true;

		// Token: 0x04003F21 RID: 16161
		public bool visibleOnAddFloatMenu = true;

		// Token: 0x04003F22 RID: 16162
		public bool listedForRoles = true;

		// Token: 0x04003F23 RID: 16163
		public PreceptDef takeNameFrom;

		// Token: 0x04003F24 RID: 16164
		public PreceptDef alsoAdds;

		// Token: 0x04003F25 RID: 16165
		public int maxCount = 1;

		// Token: 0x04003F26 RID: 16166
		public bool isNonIdeoRitual;

		// Token: 0x04003F27 RID: 16167
		[NoTranslate]
		public List<string> exclusionTags = new List<string>();

		// Token: 0x04003F28 RID: 16168
		public bool allowDuplicates;

		// Token: 0x04003F29 RID: 16169
		public bool ignoreLimitsInEditMode;

		// Token: 0x04003F2A RID: 16170
		public bool canUseAlreadyUsedThingDef;

		// Token: 0x04003F2B RID: 16171
		public bool classic;

		// Token: 0x04003F2C RID: 16172
		public bool classicExtra;

		// Token: 0x04003F2D RID: 16173
		public float defaultSelectionWeight;

		// Token: 0x04003F2E RID: 16174
		public bool enabledForNPCFactions = true;

		// Token: 0x04003F2F RID: 16175
		public bool countsTowardsPreceptLimit = true;

		// Token: 0x04003F30 RID: 16176
		public bool canGenerateAsSpecialPrecept = true;

		// Token: 0x04003F31 RID: 16177
		public RulePackDef nameMaker;

		// Token: 0x04003F32 RID: 16178
		public int nameMaxLength = 16;

		// Token: 0x04003F33 RID: 16179
		public SimpleCurve preceptInstanceCountCurve;

		// Token: 0x04003F34 RID: 16180
		public RitualPatternDef ritualPatternBase;

		// Token: 0x04003F35 RID: 16181
		public bool receivesExpectationsQualityOffset;

		// Token: 0x04003F36 RID: 16182
		public bool usesIdeoVisualEffects = true;

		// Token: 0x04003F37 RID: 16183
		public bool mergeRitualGizmosFromAllIdeos;

		// Token: 0x04003F38 RID: 16184
		public bool allowSpectatorsFromOtherIdeos = true;

		// Token: 0x04003F39 RID: 16185
		public bool allowOptionalRitualObligations;

		// Token: 0x04003F3A RID: 16186
		public bool classicModeOnly;

		// Token: 0x04003F3B RID: 16187
		public List<PreceptThingChanceClass> buildingDefChances;

		// Token: 0x04003F3C RID: 16188
		public List<ExpectationDef> buildingMinExpectations;

		// Token: 0x04003F3D RID: 16189
		public List<RoomRequirement> buildingRoomRequirements;

		// Token: 0x04003F3E RID: 16190
		public List<RoomRequirement> buildingRoomRequirementsFixed;

		// Token: 0x04003F3F RID: 16191
		public SimpleCurve roomRequirementCountCurve;

		// Token: 0x04003F40 RID: 16192
		public bool leaderRole;

		// Token: 0x04003F41 RID: 16193
		public int activationBelieverCount = -1;

		// Token: 0x04003F42 RID: 16194
		public int deactivationBelieverCount = -1;

		// Token: 0x04003F43 RID: 16195
		public List<RoleRequirement> roleRequirements;

		// Token: 0x04003F44 RID: 16196
		public WorkTags roleDisabledWorkTags;

		// Token: 0x04003F45 RID: 16197
		public WorkTags roleRequiredWorkTags;

		// Token: 0x04003F46 RID: 16198
		public WorkTags roleRequiredWorkTagAny;

		// Token: 0x04003F47 RID: 16199
		public List<PreceptApparelRequirement> roleApparelRequirements;

		// Token: 0x04003F48 RID: 16200
		public SimpleCurve roleApparelRequirementCountCurve;

		// Token: 0x04003F49 RID: 16201
		public List<AbilityDef> grantedAbilities;

		// Token: 0x04003F4A RID: 16202
		public List<RoleEffect> roleEffects;

		// Token: 0x04003F4B RID: 16203
		public List<string> roleTags;

		// Token: 0x04003F4C RID: 16204
		public string iconPath;

		// Token: 0x04003F4D RID: 16205
		public float restrictToSupremeGenderChance;

		// Token: 0x04003F4E RID: 16206
		public float certaintyLossFactor = 1f;

		// Token: 0x04003F4F RID: 16207
		public float convertPowerFactor = 1f;

		// Token: 0x04003F50 RID: 16208
		public int expectationsOffset;

		// Token: 0x04003F51 RID: 16209
		public bool createsRoleEmptyThought = true;

		// Token: 0x04003F52 RID: 16210
		public bool roleCanBeChild;

		// Token: 0x04003F53 RID: 16211
		public bool disallowLoggingCamps;

		// Token: 0x04003F54 RID: 16212
		public bool disallowMiningCamps;

		// Token: 0x04003F55 RID: 16213
		public bool disallowHuntingCamps;

		// Token: 0x04003F56 RID: 16214
		public bool disallowFarmingCamps;

		// Token: 0x04003F57 RID: 16215
		public bool approvesOfSlavery;

		// Token: 0x04003F58 RID: 16216
		public bool prefersNudity;

		// Token: 0x04003F59 RID: 16217
		public Gender genderPrefersNudity;

		// Token: 0x04003F5A RID: 16218
		public bool useChoicesFromBuildingDefs;

		// Token: 0x04003F5B RID: 16219
		public int displayOrderInImpact;

		// Token: 0x04003F5C RID: 16220
		public int displayOrderInIssue;

		// Token: 0x04003F5D RID: 16221
		public bool proselytizes;

		// Token: 0x04003F5E RID: 16222
		public int requiredScars;

		// Token: 0x04003F5F RID: 16223
		public bool approvesOfCharity;

		// Token: 0x04003F60 RID: 16224
		public float blindPawnChance = -1f;

		// Token: 0x04003F61 RID: 16225
		public bool approvesOfBlindness;

		// Token: 0x04003F62 RID: 16226
		public bool canRemoveInUI = true;

		// Token: 0x04003F63 RID: 16227
		public bool prefersDarkness;

		// Token: 0x04003F64 RID: 16228
		public List<NeedDef> enablesNeeds;

		// Token: 0x04003F65 RID: 16229
		public List<NeedDef> disablesNeeds;

		// Token: 0x04003F66 RID: 16230
		public float blindPsychicSensitivityOffset;

		// Token: 0x04003F67 RID: 16231
		public DrugPolicyDef defaultDrugPolicyOverride;

		// Token: 0x04003F68 RID: 16232
		public bool warnPlayerOnDesignateChopTree;

		// Token: 0x04003F69 RID: 16233
		public bool warnPlayerOnDesignateMine;

		// Token: 0x04003F6A RID: 16234
		public bool willingToConstructOtherIdeoBuildables;

		// Token: 0x04003F6B RID: 16235
		public float biosculpterPodCycleSpeedFactor = 1f;

		// Token: 0x04003F6C RID: 16236
		public float growthVatSpeedFactor = 1f;

		// Token: 0x04003F6D RID: 16237
		public bool prefersSlabBed;

		// Token: 0x04003F6E RID: 16238
		public bool useRepeatPenalty = true;

		// Token: 0x04003F6F RID: 16239
		public bool showRitualFloatMenuOption = true;

		// Token: 0x04003F70 RID: 16240
		[MustTranslate]
		public string tipLabelOverride;

		// Token: 0x04003F71 RID: 16241
		public bool capitalizeAsTitle = true;

		// Token: 0x04003F72 RID: 16242
		public bool ignoreNameUniqueness;

		// Token: 0x04003F73 RID: 16243
		[MustTranslate]
		public string extraTextPawnDeathLetter;

		// Token: 0x04003F74 RID: 16244
		public PreceptDef apparelPreceptSwapDef;

		// Token: 0x04003F75 RID: 16245
		public AbilityGroupDef useCooldownFromAbilityGroupDef;

		// Token: 0x04003F76 RID: 16246
		public bool iconIgnoresIdeoColor;

		// Token: 0x04003F77 RID: 16247
		public PreceptDef sourcePawnRoleDef;

		// Token: 0x04003F78 RID: 16248
		public AbilityDef sourceAbilityDef;

		// Token: 0x04003F79 RID: 16249
		public bool likesHumanLeatherApparel;

		// Token: 0x04003F7A RID: 16250
		public bool approvesOfRaiding;

		// Token: 0x04003F7B RID: 16251
		public float skipOpportunityLettersBeforeDay = 10f;

		// Token: 0x04003F7C RID: 16252
		public bool notifyPlayerOnOpportunity = true;

		// Token: 0x04003F7D RID: 16253
		public Type workerClass = typeof(PreceptWorker);

		// Token: 0x04003F7E RID: 16254
		private PreceptWorker worker;

		// Token: 0x04003F7F RID: 16255
		[Unsaved(false)]
		private List<TraitRequirement> traitsAffectingCached;

		// Token: 0x04003F80 RID: 16256
		[Unsaved(false)]
		private List<string> requiredMemeLabels;

		// Token: 0x04003F81 RID: 16257
		private Texture2D icon;
	}
}
