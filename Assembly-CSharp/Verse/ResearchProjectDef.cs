using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001AD RID: 429
	public class ResearchProjectDef : Def
	{
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x0003F2EC File Offset: 0x0003D4EC
		public float ResearchViewX
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0003F2F4 File Offset: 0x0003D4F4
		public float ResearchViewY
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0003F2FC File Offset: 0x0003D4FC
		public float CostApparent
		{
			get
			{
				return this.Cost * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0003F31A File Offset: 0x0003D51A
		public float ProgressReal
		{
			get
			{
				return Find.ResearchManager.GetProgress(this);
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x0003F327 File Offset: 0x0003D527
		public float ProgressApparent
		{
			get
			{
				return this.ProgressReal * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x0003F348 File Offset: 0x0003D548
		public string ProgressApparentString
		{
			get
			{
				string format = (this.baseCost > 0f) ? "F0" : "F2";
				return this.ProgressApparent.ToString(format);
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x0003F37E File Offset: 0x0003D57E
		public float ProgressPercent
		{
			get
			{
				return Mathf.Clamp01(this.ProgressReal / this.Cost);
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x0003F392 File Offset: 0x0003D592
		public bool IsFinished
		{
			get
			{
				return this.ProgressReal >= this.Cost;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x0003F3A5 File Offset: 0x0003D5A5
		public float Cost
		{
			get
			{
				if (this.baseCost <= 0f)
				{
					return this.knowledgeCost;
				}
				return this.baseCost;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x0003F3C1 File Offset: 0x0003D5C1
		public override TaggedString LabelCap
		{
			get
			{
				if (this.label.NullOrEmpty())
				{
					return null;
				}
				if (this.cachedLabelCap.NullOrEmpty())
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x0003F400 File Offset: 0x0003D600
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x0003F408 File Offset: 0x0003D608
		public bool IsHidden
		{
			get
			{
				return !this.IsFinished && ModsConfig.AnomalyActive && Find.EntityCodex.Hidden(this);
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x0003F428 File Offset: 0x0003D628
		public bool CanStartNow
		{
			get
			{
				return !this.IsFinished && this.PrerequisitesCompleted && this.TechprintRequirementMet && (this.requiredResearchBuilding == null || this.PlayerHasAnyAppropriateResearchBench) && this.PlayerMechanitorRequirementMet && this.AnalyzedThingsRequirementsMet && !this.IsHidden && this.InspectionRequirementsMet;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x0003F480 File Offset: 0x0003D680
		public bool PrerequisitesCompleted
		{
			get
			{
				if (this.prerequisites != null)
				{
					for (int i = 0; i < this.prerequisites.Count; i++)
					{
						if (!this.prerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				if (this.hiddenPrerequisites != null)
				{
					for (int j = 0; j < this.hiddenPrerequisites.Count; j++)
					{
						if (!this.hiddenPrerequisites[j].IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x0003F4F4 File Offset: 0x0003D6F4
		public int TechprintCount
		{
			get
			{
				if (!ModLister.RoyaltyInstalled)
				{
					return 0;
				}
				return this.techprintCount;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x0003F505 File Offset: 0x0003D705
		public int TechprintsApplied
		{
			get
			{
				return Find.ResearchManager.GetTechprints(this);
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x0003F512 File Offset: 0x0003D712
		public bool TechprintRequirementMet
		{
			get
			{
				return this.TechprintCount <= 0 || Find.ResearchManager.GetTechprints(this) >= this.TechprintCount;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0003F534 File Offset: 0x0003D734
		public ThingDef Techprint
		{
			get
			{
				if (this.TechprintCount <= 0)
				{
					return null;
				}
				if (this.cachedTechprint == null)
				{
					this.cachedTechprint = DefDatabase<ThingDef>.AllDefs.FirstOrDefault(delegate(ThingDef x)
					{
						CompProperties_Techprint compProperties = x.GetCompProperties<CompProperties_Techprint>();
						return compProperties != null && compProperties.project == this;
					});
					if (this.cachedTechprint == null)
					{
						Log.ErrorOnce("Could not find techprint for research project " + ((this != null) ? this.ToString() : null), (int)this.shortHash ^ 873231450);
					}
				}
				return this.cachedTechprint;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x0003F5A6 File Offset: 0x0003D7A6
		public int RequiredAnalyzedThingCount
		{
			get
			{
				List<ThingDef> list = this.requiredAnalyzed;
				if (list == null)
				{
					return 0;
				}
				return list.Count;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x0003F5BC File Offset: 0x0003D7BC
		public int AnalyzedThingsCompleted
		{
			get
			{
				if (this.requiredAnalyzed.NullOrEmpty<ThingDef>())
				{
					return 0;
				}
				int num = 0;
				for (int i = 0; i < this.requiredAnalyzed.Count; i++)
				{
					AnalysisManager analysisManager = Find.AnalysisManager;
					CompProperties_CompAnalyzableUnlockResearch compProperties = this.requiredAnalyzed[i].GetCompProperties<CompProperties_CompAnalyzableUnlockResearch>();
					AnalysisDetails analysisDetails;
					analysisManager.TryGetAnalysisProgress((compProperties != null) ? compProperties.analysisID : -1, out analysisDetails);
					if (analysisDetails != null && analysisDetails.Satisfied)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x0003F62A File Offset: 0x0003D82A
		public bool AnalyzedThingsRequirementsMet
		{
			get
			{
				return this.RequiredAnalyzedThingCount <= 0 || this.AnalyzedThingsCompleted >= this.RequiredAnalyzedThingCount;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x0003F648 File Offset: 0x0003D848
		public bool InspectionRequirementsMet
		{
			get
			{
				return !ModsConfig.OdysseyActive || !this.requireGravEngineInspected || Find.ResearchManager.gravEngineInspected;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000BEF RID: 3055 RVA: 0x0003F668 File Offset: 0x0003D868
		public List<Def> UnlockedDefs
		{
			get
			{
				if (this.cachedUnlockedDefs == null)
				{
					this.cachedUnlockedDefs = (from x in (from x in (from x in DefDatabase<RecipeDef>.AllDefs
					where x.researchPrerequisite == this || (x.researchPrerequisites != null && x.researchPrerequisites.Contains(this))
					select x).SelectMany((RecipeDef x) => from y in x.products
					select y.thingDef)
					orderby x.label
					select x).Concat(from x in DefDatabase<ThingDef>.AllDefs
					where x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
					orderby x.label
					select x).Concat(from x in DefDatabase<ThingDef>.AllDefs
					where x.plant != null && x.plant.sowResearchPrerequisites != null && x.plant.sowResearchPrerequisites.Contains(this)
					orderby x.label
					select x).Concat(from x in DefDatabase<TerrainDef>.AllDefs
					where x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
					orderby x.label
					select x).Concat(from x in DefDatabase<RecipeDef>.AllDefs
					where x.IsSurgery && x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
					orderby x.label
					select x).Concat(from x in DefDatabase<PsychicRitualDef>.AllDefs
					where x.researchPrerequisite == this
					select x)
					orderby x.label
					select x).Distinct<Def>().ToList<Def>();
				}
				return this.cachedUnlockedDefs;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x0003F830 File Offset: 0x0003DA30
		public List<Dialog_InfoCard.Hyperlink> InfoCardHyperlinks
		{
			get
			{
				if (this.cachedHyperlinks == null)
				{
					this.cachedHyperlinks = new List<Dialog_InfoCard.Hyperlink>();
					List<Def> unlockedDefs = this.UnlockedDefs;
					if (unlockedDefs != null)
					{
						for (int i = 0; i < unlockedDefs.Count; i++)
						{
							this.cachedHyperlinks.Add(new Dialog_InfoCard.Hyperlink(unlockedDefs[i], -1));
						}
					}
				}
				return this.cachedHyperlinks;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x0003F88C File Offset: 0x0003DA8C
		public bool PlayerHasAnyAppropriateResearchBench
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Building> allBuildingsColonist = maps[i].listerBuildings.allBuildingsColonist;
					for (int j = 0; j < allBuildingsColonist.Count; j++)
					{
						Building_ResearchBench building_ResearchBench = allBuildingsColonist[j] as Building_ResearchBench;
						if (building_ResearchBench != null && this.CanBeResearchedAt(building_ResearchBench, true))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x0003F8F3 File Offset: 0x0003DAF3
		public bool PlayerMechanitorRequirementMet
		{
			get
			{
				return !ModsConfig.BiotechActive || !this.requiresMechanitor || MechanitorUtility.AnyMechanitorInPlayerFaction();
			}
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x0003F90B File Offset: 0x0003DB0B
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.tab == null)
			{
				this.tab = ResearchTabDefOf.Main;
			}
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0003F926 File Offset: 0x0003DB26
		public override IEnumerable<string> ConfigErrors()
		{
			ResearchProjectDef.<ConfigErrors>d__91 <ConfigErrors>d__ = new ResearchProjectDef.<ConfigErrors>d__91(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0003F936 File Offset: 0x0003DB36
		public override void PostLoad()
		{
			base.PostLoad();
			if (!ModLister.RoyaltyInstalled)
			{
				this.techprintCount = 0;
			}
			if (!ModLister.BiotechInstalled)
			{
				this.requiredAnalyzed = null;
			}
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0003F95C File Offset: 0x0003DB5C
		public string GetTip()
		{
			string text = this.LabelCap.Colorize(ColoredText.TipSectionTitleColor);
			if (this.ProgressReal > 0f && !this.IsFinished)
			{
				text = string.Concat(new string[]
				{
					text,
					" (",
					this.ProgressApparentString,
					" / ",
					this.CostApparent.ToString("F0"),
					")"
				});
			}
			if (this.cachedDescription == null)
			{
				this.cachedDescription = this.Description;
				if (this.TechprintCount > 0)
				{
					this.cachedDescription = this.cachedDescription + "\n\n" + ("RequiredTechprintTip".Translate() + ": " + this.Techprint.LabelCap).ToString();
				}
				if (this.RequiredAnalyzedThingCount > 0)
				{
					string[] array = new string[5];
					array[0] = this.cachedDescription;
					array[1] = "\n\n";
					array[2] = "StudyRequirementTip".Translate().ToString();
					array[3] = ": ";
					array[4] = (from t in this.requiredAnalyzed
					select t.label).ToCommaList(true, false).CapitalizeFirst();
					this.cachedDescription = string.Concat(array);
				}
				if (this.modContentPack != null && !this.modContentPack.IsCoreMod)
				{
					Color color = this.modContentPack.IsOfficialMod ? ModLister.GetExpansionWithIdentifier(this.modContentPack.PackageId.ToLower()).primaryColor : ColoredText.SubtleGrayColor;
					this.cachedDescription = this.cachedDescription + "\n\n" + ("Stat_Source_Label".Translate().ToString() + ": " + this.modContentPack.Name).Colorize(color);
				}
			}
			return text + "\n" + this.cachedDescription;
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0003FB68 File Offset: 0x0003DD68
		public float CostFactor(TechLevel researcherTechLevel)
		{
			if (this.techLevel == TechLevel.Undefined)
			{
				return 1f;
			}
			TechLevel techLevel = (TechLevel)Mathf.Min((int)this.techLevel, 4);
			if (researcherTechLevel >= techLevel)
			{
				return 1f;
			}
			int num = (int)(techLevel - researcherTechLevel);
			return 1f + (float)num * 0.5f;
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0003FBAD File Offset: 0x0003DDAD
		public bool HasTag(ResearchProjectTagDef tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0003FBC8 File Offset: 0x0003DDC8
		public bool CanBeResearchedAt(Building_ResearchBench bench, bool ignoreResearchBenchPowerStatus)
		{
			if (this.requiredResearchBuilding != null && bench.def != this.requiredResearchBuilding)
			{
				return false;
			}
			if (!ignoreResearchBenchPowerStatus)
			{
				CompPowerTrader comp = bench.GetComp<CompPowerTrader>();
				if (comp != null && !comp.PowerOn)
				{
					return false;
				}
			}
			if (!this.requiredResearchFacilities.NullOrEmpty<ThingDef>())
			{
				CompAffectedByFacilities affectedByFacilities = bench.TryGetComp<CompAffectedByFacilities>();
				if (affectedByFacilities == null)
				{
					return false;
				}
				List<Thing> linkedFacilitiesListForReading = affectedByFacilities.LinkedFacilitiesListForReading;
				int i;
				Predicate<Thing> <>9__0;
				int j;
				for (i = 0; i < this.requiredResearchFacilities.Count; i = j + 1)
				{
					List<Thing> list = linkedFacilitiesListForReading;
					Predicate<Thing> match;
					if ((match = <>9__0) == null)
					{
						match = (<>9__0 = ((Thing x) => x.def == this.requiredResearchFacilities[i] && affectedByFacilities.IsFacilityActive(x)));
					}
					if (list.Find(match) == null)
					{
						return false;
					}
					j = i;
				}
			}
			return true;
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0003FC98 File Offset: 0x0003DE98
		public bool AnyOtherVisiblePrerequisitesCompleted(ResearchProjectDef prerequisite)
		{
			if (this.prerequisites == null)
			{
				return false;
			}
			for (int i = 0; i < this.prerequisites.Count; i++)
			{
				if (this.prerequisites[i] != prerequisite && this.prerequisites[i].IsFinished)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0003FCEC File Offset: 0x0003DEEC
		public void ReapplyAllMods()
		{
			if (this.researchMods != null)
			{
				for (int i = 0; i < this.researchMods.Count; i++)
				{
					try
					{
						this.researchMods[i].Apply();
					}
					catch (Exception ex)
					{
						string str = "Exception applying research mod for project ";
						string str2 = (this != null) ? this.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0003FD6C File Offset: 0x0003DF6C
		public static ResearchProjectDef Named(string defName)
		{
			return DefDatabase<ResearchProjectDef>.GetNamed(defName, true);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0003FD78 File Offset: 0x0003DF78
		public static void GenerateNonOverlappingCoordinates()
		{
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
			{
				researchProjectDef.x = researchProjectDef.researchViewX;
				researchProjectDef.y = researchProjectDef.researchViewY;
			}
			int num = 0;
			do
			{
				bool flag = false;
				foreach (ResearchProjectDef researchProjectDef2 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
				{
					foreach (ResearchProjectDef researchProjectDef3 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
					{
						if (researchProjectDef2 != researchProjectDef3 && researchProjectDef2.tab == researchProjectDef3.tab)
						{
							bool flag2 = Mathf.Abs(researchProjectDef2.x - researchProjectDef3.x) < 0.5f;
							bool flag3 = Mathf.Abs(researchProjectDef2.y - researchProjectDef3.y) < 0.25f;
							if (flag2 && flag3)
							{
								flag = true;
								if (researchProjectDef2.x <= researchProjectDef3.x)
								{
									researchProjectDef2.x -= 0.1f;
									researchProjectDef3.x += 0.1f;
								}
								else
								{
									researchProjectDef2.x += 0.1f;
									researchProjectDef3.x -= 0.1f;
								}
								if (researchProjectDef2.y <= researchProjectDef3.y)
								{
									researchProjectDef2.y -= 0.1f;
									researchProjectDef3.y += 0.1f;
								}
								else
								{
									researchProjectDef2.y += 0.1f;
									researchProjectDef3.y -= 0.1f;
								}
								researchProjectDef2.x += 0.001f;
								researchProjectDef2.y += 0.001f;
								researchProjectDef3.x -= 0.001f;
								researchProjectDef3.y -= 0.001f;
								ResearchProjectDef.ClampInCoordinateLimits(researchProjectDef2);
								ResearchProjectDef.ClampInCoordinateLimits(researchProjectDef3);
							}
						}
					}
				}
				if (!flag)
				{
					return;
				}
				num++;
			}
			while (num <= 200);
			Log.Error("Couldn't relax research project coordinates apart after " + 200.ToString() + " passes.");
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00040020 File Offset: 0x0003E220
		private static void ClampInCoordinateLimits(ResearchProjectDef rp)
		{
			if (rp.x < 0f)
			{
				rp.x = 0f;
			}
			if (rp.y < 0f)
			{
				rp.y = 0f;
			}
			if (rp.y > 6.5f)
			{
				rp.y = 6.5f;
			}
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00040078 File Offset: 0x0003E278
		public void Debug_ApplyPositionDelta(Vector2 delta)
		{
			bool flag = Mathf.Abs(delta.x) > 0.01f;
			bool flag2 = Mathf.Abs(delta.y) > 0.01f;
			if (flag)
			{
				this.x += delta.x;
			}
			if (flag2)
			{
				this.y += delta.y;
			}
			if (flag || flag2)
			{
				this.positionModified = true;
			}
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x000400E4 File Offset: 0x0003E2E4
		public void Debug_SnapPositionData()
		{
			this.x = Mathf.Round(this.x * 2f) / 2f;
			this.y = Mathf.Round(this.y * 10f) / 10f;
			ResearchProjectDef.ClampInCoordinateLimits(this);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00040131 File Offset: 0x0003E331
		public bool Debug_IsPositionModified()
		{
			return this.positionModified;
		}

		// Token: 0x04000B3E RID: 2878
		public float baseCost;

		// Token: 0x04000B3F RID: 2879
		public List<ResearchProjectDef> prerequisites;

		// Token: 0x04000B40 RID: 2880
		public List<ResearchProjectDef> hiddenPrerequisites;

		// Token: 0x04000B41 RID: 2881
		public TechLevel techLevel;

		// Token: 0x04000B42 RID: 2882
		public List<ResearchProjectDef> requiredByThis;

		// Token: 0x04000B43 RID: 2883
		private List<ResearchMod> researchMods;

		// Token: 0x04000B44 RID: 2884
		public ThingDef requiredResearchBuilding;

		// Token: 0x04000B45 RID: 2885
		public List<ThingDef> requiredResearchFacilities;

		// Token: 0x04000B46 RID: 2886
		public List<ResearchProjectTagDef> tags;

		// Token: 0x04000B47 RID: 2887
		public ResearchTabDef tab;

		// Token: 0x04000B48 RID: 2888
		public float researchViewX = 1f;

		// Token: 0x04000B49 RID: 2889
		public float researchViewY = 1f;

		// Token: 0x04000B4A RID: 2890
		[MustTranslate]
		public string discoveredLetterTitle;

		// Token: 0x04000B4B RID: 2891
		[MustTranslate]
		public string discoveredLetterText;

		// Token: 0x04000B4C RID: 2892
		public DifficultyConditionConfig discoveredLetterDisabledWhen = new DifficultyConditionConfig();

		// Token: 0x04000B4D RID: 2893
		public int techprintCount;

		// Token: 0x04000B4E RID: 2894
		public float techprintCommonality = 1f;

		// Token: 0x04000B4F RID: 2895
		public float techprintMarketValue = 1000f;

		// Token: 0x04000B50 RID: 2896
		[NoTranslate]
		public List<string> heldByFactionCategoryTags;

		// Token: 0x04000B51 RID: 2897
		public DifficultyConditionConfig hideWhen = new DifficultyConditionConfig();

		// Token: 0x04000B52 RID: 2898
		public bool requiresMechanitor;

		// Token: 0x04000B53 RID: 2899
		public List<ThingDef> requiredAnalyzed;

		// Token: 0x04000B54 RID: 2900
		public bool recalculatePower;

		// Token: 0x04000B55 RID: 2901
		public KnowledgeCategoryDef knowledgeCategory;

		// Token: 0x04000B56 RID: 2902
		public float knowledgeCost;

		// Token: 0x04000B57 RID: 2903
		public ConceptDef teachConcept;

		// Token: 0x04000B58 RID: 2904
		public RulePack generalRules;

		// Token: 0x04000B59 RID: 2905
		public bool requireGravEngineInspected;

		// Token: 0x04000B5A RID: 2906
		[MustTranslate]
		public List<string> customUnlockTexts;

		// Token: 0x04000B5B RID: 2907
		[Unsaved(false)]
		private float x = 1f;

		// Token: 0x04000B5C RID: 2908
		[Unsaved(false)]
		private float y = 1f;

		// Token: 0x04000B5D RID: 2909
		[Unsaved(false)]
		private bool positionModified;

		// Token: 0x04000B5E RID: 2910
		[Unsaved(false)]
		private ThingDef cachedTechprint;

		// Token: 0x04000B5F RID: 2911
		[Unsaved(false)]
		private List<Def> cachedUnlockedDefs;

		// Token: 0x04000B60 RID: 2912
		[Unsaved(false)]
		private List<Dialog_InfoCard.Hyperlink> cachedHyperlinks;

		// Token: 0x04000B61 RID: 2913
		[Unsaved(false)]
		private string cachedDescription;

		// Token: 0x04000B62 RID: 2914
		public const TechLevel MaxEffectiveTechLevel = TechLevel.Industrial;

		// Token: 0x04000B63 RID: 2915
		private const float ResearchCostFactorPerTechLevelDiff = 0.5f;
	}
}
