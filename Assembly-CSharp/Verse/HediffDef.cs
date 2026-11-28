using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000173 RID: 371
	public class HediffDef : Def, IRenderNodePropertiesParent
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x0003BFBA File Offset: 0x0003A1BA
		public bool HasDefinedGraphicProperties
		{
			get
			{
				return !this.renderNodeProperties.NullOrEmpty<PawnRenderNodeProperties>() || this.skinShader != null;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x0003BFD4 File Offset: 0x0003A1D4
		public List<PawnRenderNodeProperties> RenderNodeProperties
		{
			get
			{
				return this.renderNodeProperties ?? PawnRenderUtility.EmptyRenderNodeProperties;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x0003BFE5 File Offset: 0x0003A1E5
		public bool IsAddiction
		{
			get
			{
				return typeof(Hediff_Addiction).IsAssignableFrom(this.hediffClass);
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x0003BFFC File Offset: 0x0003A1FC
		public bool AlwaysAllowMothball
		{
			get
			{
				if (!this.alwaysAllowMothballCached)
				{
					this.alwaysAllowMothball = true;
					if (this.comps != null && this.comps.Count > 0)
					{
						this.alwaysAllowMothball = false;
					}
					if (this.stages != null)
					{
						for (int i = 0; i < this.stages.Count; i++)
						{
							HediffStage hediffStage = this.stages[i];
							if (hediffStage.deathMtbDays > 0f || (hediffStage.hediffGivers != null && hediffStage.hediffGivers.Count > 0))
							{
								this.alwaysAllowMothball = false;
							}
						}
					}
					this.alwaysAllowMothballCached = true;
				}
				return this.alwaysAllowMothball;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x0003C09C File Offset: 0x0003A29C
		public Hediff ConcreteExample
		{
			get
			{
				Hediff result;
				if ((result = this.concreteExampleInt) == null)
				{
					result = (this.concreteExampleInt = HediffMaker.Debug_MakeConcreteExampleHediff(this));
				}
				return result;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x0003C0C2 File Offset: 0x0003A2C2
		public string Description
		{
			get
			{
				if (this.descriptionCached == null)
				{
					if (!this.descriptionShort.NullOrEmpty())
					{
						this.descriptionCached = this.descriptionShort;
					}
					else
					{
						this.descriptionCached = this.description;
					}
				}
				return this.descriptionCached;
			}
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0003C0FC File Offset: 0x0003A2FC
		public bool HasComp(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0003C144 File Offset: 0x0003A344
		public HediffCompProperties CompPropsFor(Type compClass)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].compClass == compClass)
					{
						return this.comps[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0003C198 File Offset: 0x0003A398
		public T CompProps<T>() where T : HediffCompProperties
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0003C1F0 File Offset: 0x0003A3F0
		public bool PossibleToDevelopImmunityNaturally()
		{
			HediffCompProperties_Immunizable hediffCompProperties_Immunizable = this.CompProps<HediffCompProperties_Immunizable>();
			return hediffCompProperties_Immunizable != null && (hediffCompProperties_Immunizable.immunityPerDayNotSick > 0f || hediffCompProperties_Immunizable.immunityPerDaySick > 0f);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0003C224 File Offset: 0x0003A424
		public string PrettyTextForPart(BodyPartRecord bodyPart)
		{
			if (this.labelNounPretty.NullOrEmpty())
			{
				return null;
			}
			return this.labelNounPretty.Formatted(this.label, bodyPart.Label);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0003C25C File Offset: 0x0003A45C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].ResolveReferences(this);
				}
			}
			if (this.renderNodeProperties != null)
			{
				for (int j = 0; j < this.renderNodeProperties.Count; j++)
				{
					this.renderNodeProperties[j].ResolveReferencesRecursive();
				}
			}
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0003C2D0 File Offset: 0x0003A4D0
		public int StageAtSeverity(float severity)
		{
			if (this.stages == null)
			{
				return 0;
			}
			for (int i = this.stages.Count - 1; i >= 0; i--)
			{
				if (severity >= this.stages[i].minSeverity)
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0003C318 File Offset: 0x0003A518
		public int AptitudeFor(SkillDef skill)
		{
			int num = 0;
			if (this.aptitudes.NullOrEmpty<Aptitude>())
			{
				return num;
			}
			for (int i = 0; i < this.aptitudes.Count; i++)
			{
				if (this.aptitudes[i].skill == skill)
				{
					num += this.aptitudes[i].level;
				}
			}
			return num;
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0003C375 File Offset: 0x0003A575
		public override IEnumerable<string> ConfigErrors()
		{
			HediffDef.<ConfigErrors>d__105 <ConfigErrors>d__ = new HediffDef.<ConfigErrors>d__105(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0003C385 File Offset: 0x0003A585
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			HediffDef.<SpecialDisplayStats>d__106 <SpecialDisplayStats>d__ = new HediffDef.<SpecialDisplayStats>d__106(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0003C398 File Offset: 0x0003A598
		public bool TryGetReportStringOverrideFor(JobDef def, out string str)
		{
			if (this.reportStringOverrides.NullOrEmpty<HediffDef.ReportStringOverride>())
			{
				str = null;
				return false;
			}
			if (this.reportStringOverridesDict == null)
			{
				this.reportStringOverridesDict = new Dictionary<JobDef, string>();
				if (this.reportStringOverrides != null)
				{
					foreach (HediffDef.ReportStringOverride reportStringOverride in this.reportStringOverrides)
					{
						this.reportStringOverridesDict[reportStringOverride.jobDef] = reportStringOverride.reportString;
					}
				}
			}
			return this.reportStringOverridesDict.TryGetValue(def, out str);
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0003C438 File Offset: 0x0003A638
		public static HediffDef Named(string defName)
		{
			return DefDatabase<HediffDef>.GetNamed(defName, true);
		}

		// Token: 0x0400087A RID: 2170
		public Type hediffClass = typeof(Hediff);

		// Token: 0x0400087B RID: 2171
		public List<HediffCompProperties> comps;

		// Token: 0x0400087C RID: 2172
		[MustTranslate]
		public string descriptionShort;

		// Token: 0x0400087D RID: 2173
		[NoTranslate]
		public string debugLabelExtra;

		// Token: 0x0400087E RID: 2174
		public float initialSeverity = 0.5f;

		// Token: 0x0400087F RID: 2175
		public float lethalSeverity = -1f;

		// Token: 0x04000880 RID: 2176
		public List<HediffStage> stages;

		// Token: 0x04000881 RID: 2177
		public bool tendable;

		// Token: 0x04000882 RID: 2178
		public bool isBad = true;

		// Token: 0x04000883 RID: 2179
		public ThingDef spawnThingOnRemoved;

		// Token: 0x04000884 RID: 2180
		public float chanceToCauseNoPain;

		// Token: 0x04000885 RID: 2181
		public bool canApplyDodChanceForCapacityChanges;

		// Token: 0x04000886 RID: 2182
		public bool makesSickThought;

		// Token: 0x04000887 RID: 2183
		public bool makesAlert = true;

		// Token: 0x04000888 RID: 2184
		public NeedDef chemicalNeed;

		// Token: 0x04000889 RID: 2185
		public float minSeverity;

		// Token: 0x0400088A RID: 2186
		public float maxSeverity = float.MaxValue;

		// Token: 0x0400088B RID: 2187
		public bool scenarioCanAdd;

		// Token: 0x0400088C RID: 2188
		public List<HediffGiver> hediffGivers;

		// Token: 0x0400088D RID: 2189
		public bool cureAllAtOnceIfCuredByItem;

		// Token: 0x0400088E RID: 2190
		public TaleDef taleOnVisible;

		// Token: 0x0400088F RID: 2191
		public bool recordDownedTale = true;

		// Token: 0x04000890 RID: 2192
		public bool everCurableByItem = true;

		// Token: 0x04000891 RID: 2193
		public List<string> tags;

		// Token: 0x04000892 RID: 2194
		public bool priceImpact;

		// Token: 0x04000893 RID: 2195
		public float priceOffset;

		// Token: 0x04000894 RID: 2196
		public bool chronic;

		// Token: 0x04000895 RID: 2197
		public bool keepOnBodyPartRestoration;

		// Token: 0x04000896 RID: 2198
		public bool countsAsAddedPartOrImplant;

		// Token: 0x04000897 RID: 2199
		public bool blocksSocialInteraction;

		// Token: 0x04000898 RID: 2200
		public bool blocksSleeping;

		// Token: 0x04000899 RID: 2201
		[MustTranslate]
		public string overrideTooltip;

		// Token: 0x0400089A RID: 2202
		[MustTranslate]
		public string extraTooltip;

		// Token: 0x0400089B RID: 2203
		[MustTranslate]
		public string inspectString;

		// Token: 0x0400089C RID: 2204
		public bool levelIsQuantity;

		// Token: 0x0400089D RID: 2205
		public bool removeOnDeathrestStart;

		// Token: 0x0400089E RID: 2206
		public bool preventsCrawling;

		// Token: 0x0400089F RID: 2207
		public bool preventsPregnancy;

		// Token: 0x040008A0 RID: 2208
		public bool preventsLungRot;

		// Token: 0x040008A1 RID: 2209
		public bool pregnant;

		// Token: 0x040008A2 RID: 2210
		public bool allowMothballIfLowPriorityWorldPawn;

		// Token: 0x040008A3 RID: 2211
		public List<string> removeWithTags;

		// Token: 0x040008A4 RID: 2212
		public List<BodyPartDef> onlyLifeThreateningTo;

		// Token: 0x040008A5 RID: 2213
		public bool canAffectBionicOrImplant = true;

		// Token: 0x040008A6 RID: 2214
		public bool alwaysShowSeverity;

		// Token: 0x040008A7 RID: 2215
		public bool showGizmosOnCorpse;

		// Token: 0x040008A8 RID: 2216
		public BodyPartDef defaultInstallPart;

		// Token: 0x040008A9 RID: 2217
		public Color? hairColorOverride;

		// Token: 0x040008AA RID: 2218
		public List<HediffInfectionPathway> possiblePathways;

		// Token: 0x040008AB RID: 2219
		public List<InfectionPathwayDef> givesInfectionPathways;

		// Token: 0x040008AC RID: 2220
		public bool duplicationAllowed = true;

		// Token: 0x040008AD RID: 2221
		public bool preventsDeath;

		// Token: 0x040008AE RID: 2222
		public List<MeditationFocusDef> allowedMeditationFocusTypes;

		// Token: 0x040008AF RID: 2223
		public List<AbilityDef> abilities;

		// Token: 0x040008B0 RID: 2224
		public bool isInfection;

		// Token: 0x040008B1 RID: 2225
		public bool forceRemoveOnResurrection;

		// Token: 0x040008B2 RID: 2226
		public bool organicAddedBodypart;

		// Token: 0x040008B3 RID: 2227
		public bool deprioritizeHealing;

		// Token: 0x040008B4 RID: 2228
		public bool clearsEgo;

		// Token: 0x040008B5 RID: 2229
		public List<Aptitude> aptitudes;

		// Token: 0x040008B6 RID: 2230
		public SimpleCurve removeOnRedressChanceByDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			}
		};

		// Token: 0x040008B7 RID: 2231
		public bool removeOnQuestLodgers;

		// Token: 0x040008B8 RID: 2232
		public List<PawnKindDef> removeOnRedressIfNotOfKind;

		// Token: 0x040008B9 RID: 2233
		public bool displayWound;

		// Token: 0x040008BA RID: 2234
		public float? woundAnchorRange;

		// Token: 0x040008BB RID: 2235
		public Color defaultLabelColor = Color.white;

		// Token: 0x040008BC RID: 2236
		private List<PawnRenderNodeProperties> renderNodeProperties;

		// Token: 0x040008BD RID: 2237
		public Color? skinColorOverride;

		// Token: 0x040008BE RID: 2238
		public Color? skinColorTint;

		// Token: 0x040008BF RID: 2239
		public float skinColorTintStrength = 0.5f;

		// Token: 0x040008C0 RID: 2240
		public ShaderTypeDef skinShader;

		// Token: 0x040008C1 RID: 2241
		public bool forceRenderTreeRecache;

		// Token: 0x040008C2 RID: 2242
		public InjuryProps injuryProps;

		// Token: 0x040008C3 RID: 2243
		public AddedBodyPartProps addedPartProps;

		// Token: 0x040008C4 RID: 2244
		private List<HediffDef.ReportStringOverride> reportStringOverrides;

		// Token: 0x040008C5 RID: 2245
		[MustTranslate]
		public string labelNoun;

		// Token: 0x040008C6 RID: 2246
		[MustTranslate]
		public string battleStateLabel;

		// Token: 0x040008C7 RID: 2247
		[MustTranslate]
		public string labelNounPretty;

		// Token: 0x040008C8 RID: 2248
		[MustTranslate]
		public string targetPrefix;

		// Token: 0x040008C9 RID: 2249
		private bool alwaysAllowMothballCached;

		// Token: 0x040008CA RID: 2250
		private bool alwaysAllowMothball;

		// Token: 0x040008CB RID: 2251
		private string descriptionCached;

		// Token: 0x040008CC RID: 2252
		private Dictionary<JobDef, string> reportStringOverridesDict;

		// Token: 0x040008CD RID: 2253
		private Hediff concreteExampleInt;

		// Token: 0x0200253C RID: 9532
		private class ReportStringOverride
		{
			// Token: 0x0400986D RID: 39021
			public JobDef jobDef;

			// Token: 0x0400986E RID: 39022
			[MustTranslate]
			public string reportString;
		}
	}
}
