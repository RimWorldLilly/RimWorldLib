using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x02000161 RID: 353
	public class DesignationCategoryDef : Def
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0003B317 File Offset: 0x00039517
		public IEnumerable<Designator> ResolvedAllowedDesignators
		{
			get
			{
				DesignationCategoryDef.<get_ResolvedAllowedDesignators>d__14 <get_ResolvedAllowedDesignators>d__ = new DesignationCategoryDef.<get_ResolvedAllowedDesignators>d__14(-2);
				<get_ResolvedAllowedDesignators>d__.<>4__this = this;
				return <get_ResolvedAllowedDesignators>d__;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0003B327 File Offset: 0x00039527
		public List<Designator> AllResolvedDesignators
		{
			get
			{
				return this.resolvedDesignators;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0003B32F File Offset: 0x0003952F
		public IEnumerable<Designator> AllIdeoDesignators
		{
			get
			{
				DesignationCategoryDef.<get_AllIdeoDesignators>d__18 <get_AllIdeoDesignators>d__ = new DesignationCategoryDef.<get_AllIdeoDesignators>d__18(-2);
				<get_AllIdeoDesignators>d__.<>4__this = this;
				return <get_AllIdeoDesignators>d__;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0003B33F File Offset: 0x0003953F
		public IEnumerable<Designator> AllResolvedAndIdeoDesignators
		{
			get
			{
				DesignationCategoryDef.<get_AllResolvedAndIdeoDesignators>d__20 <get_AllResolvedAndIdeoDesignators>d__ = new DesignationCategoryDef.<get_AllResolvedAndIdeoDesignators>d__20(-2);
				<get_AllResolvedAndIdeoDesignators>d__.<>4__this = this;
				return <get_AllResolvedAndIdeoDesignators>d__;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x0003B350 File Offset: 0x00039550
		public bool Visible
		{
			get
			{
				if (DebugSettings.godMode)
				{
					return true;
				}
				if (ModsConfig.AnomalyActive && Find.Anomaly.HighestLevelReached < this.minMonolithLevel && Find.Anomaly.GenerateMonolith)
				{
					return false;
				}
				if (this.researchPrerequisites == null)
				{
					return true;
				}
				using (List<ResearchProjectDef>.Enumerator enumerator = this.researchPrerequisites.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0003B3E4 File Offset: 0x000395E4
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.ResolveDesignators();
			});
			this.cachedHighlightClosedTag = "DesignationCategoryButton-" + this.defName + "-Closed";
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0003B418 File Offset: 0x00039618
		private void ResolveDesignators()
		{
			this.resolvedDesignators.Clear();
			foreach (Type type in this.specialDesignatorClasses)
			{
				Designator designator = null;
				try
				{
					designator = (Designator)Activator.CreateInstance(type);
					designator.isOrder = true;
				}
				catch (Exception ex)
				{
					string[] array = new string[6];
					array[0] = "DesignationCategoryDef";
					array[1] = this.defName;
					array[2] = " could not instantiate special designator from class ";
					int num = 3;
					Type type2 = type;
					array[num] = ((type2 != null) ? type2.ToString() : null);
					array[4] = ".\n Exception: \n";
					array[5] = ex.ToString();
					Log.Error(string.Concat(array));
				}
				if (designator != null)
				{
					this.resolvedDesignators.Add(designator);
				}
			}
			IEnumerable<BuildableDef> enumerable = from tDef in DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>())
			where tDef.designationCategory == this && tDef.canGenerateDefaultDesignator
			select tDef;
			Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown> dictionary = new Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown>();
			foreach (BuildableDef buildableDef in enumerable)
			{
				if (buildableDef.designatorDropdown != null)
				{
					if (!dictionary.ContainsKey(buildableDef.designatorDropdown))
					{
						dictionary[buildableDef.designatorDropdown] = new Designator_Dropdown();
						dictionary[buildableDef.designatorDropdown].Order = buildableDef.uiOrder;
						this.resolvedDesignators.Add(dictionary[buildableDef.designatorDropdown]);
					}
					dictionary[buildableDef.designatorDropdown].Add(new Designator_Build(buildableDef));
				}
				else
				{
					this.resolvedDesignators.Add(new Designator_Build(buildableDef));
				}
			}
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0003B5E8 File Offset: 0x000397E8
		public void DirtyCache()
		{
			this.ideoBuildingDesignatorsCached.Clear();
			this.ideoDropdownsCached.Clear();
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0003B600 File Offset: 0x00039800
		private static bool RequirementSatisfied(string requirement)
		{
			if (!ModLister.HasActiveModWithName(requirement))
			{
				ResearchProjectDef namedSilentFail = DefDatabase<ResearchProjectDef>.GetNamedSilentFail(requirement);
				return namedSilentFail != null && namedSilentFail.IsFinished;
			}
			return true;
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0003B654 File Offset: 0x00039854
		[CompilerGenerated]
		private Designator <get_AllIdeoDesignators>g__GetCachedDesignator|18_0(BuildableDef def, Precept_Building buildingPrecept = null)
		{
			DesignationCategoryDef.BuildablePreceptBuilding key = new DesignationCategoryDef.BuildablePreceptBuilding(def, buildingPrecept);
			Designator designator;
			if (!this.ideoBuildingDesignatorsCached.TryGetValue(key, out designator))
			{
				Designator_Build designator_Build = new Designator_Build(def);
				designator = designator_Build;
				if (buildingPrecept != null)
				{
					designator_Build.sourcePrecept = buildingPrecept;
				}
				this.ideoBuildingDesignatorsCached[key] = designator;
			}
			return designator;
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0003B69C File Offset: 0x0003989C
		[CompilerGenerated]
		private Designator_Dropdown <get_AllIdeoDesignators>g__GetCachedDropdown|18_1(DesignatorDropdownGroupDef group)
		{
			Designator_Dropdown result;
			if (!this.ideoDropdownsCached.TryGetValue(group, out result))
			{
				IEnumerable<BuildableDef> enumerable = from tDef in DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>())
				where tDef.designationCategory == this && !tDef.canGenerateDefaultDesignator && tDef.designatorDropdown == @group
				select tDef;
				if (!enumerable.Any<BuildableDef>())
				{
					this.ideoDropdownsCached[group] = null;
					return this.ideoDropdownsCached[group];
				}
				foreach (BuildableDef buildableDef in enumerable)
				{
					if (!this.ideoDropdownsCached.ContainsKey(buildableDef.designatorDropdown))
					{
						this.ideoDropdownsCached[buildableDef.designatorDropdown] = new Designator_Dropdown();
						this.ideoDropdownsCached[buildableDef.designatorDropdown].Order = buildableDef.uiOrder;
					}
					this.ideoDropdownsCached[buildableDef.designatorDropdown].Add(new Designator_Build(buildableDef));
				}
				result = this.ideoDropdownsCached[group];
			}
			return result;
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0003B7DC File Offset: 0x000399DC
		[CompilerGenerated]
		private IEnumerable<Designator> <get_AllIdeoDesignators>g__GetDesignatorsFromStyleCategory|18_2(StyleCategoryDef categoryDef)
		{
			DesignationCategoryDef.<<get_AllIdeoDesignators>g__GetDesignatorsFromStyleCategory|18_2>d <<get_AllIdeoDesignators>g__GetDesignatorsFromStyleCategory|18_2>d = new DesignationCategoryDef.<<get_AllIdeoDesignators>g__GetDesignatorsFromStyleCategory|18_2>d(-2);
			<<get_AllIdeoDesignators>g__GetDesignatorsFromStyleCategory|18_2>d.<>4__this = this;
			<<get_AllIdeoDesignators>g__GetDesignatorsFromStyleCategory|18_2>d.<>3__categoryDef = categoryDef;
			return <<get_AllIdeoDesignators>g__GetDesignatorsFromStyleCategory|18_2>d;
		}

		// Token: 0x040007E1 RID: 2017
		public List<Type> specialDesignatorClasses = new List<Type>();

		// Token: 0x040007E2 RID: 2018
		public int order;

		// Token: 0x040007E3 RID: 2019
		public bool showPowerGrid;

		// Token: 0x040007E4 RID: 2020
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x040007E5 RID: 2021
		public int minMonolithLevel;

		// Token: 0x040007E6 RID: 2022
		public int? preferredColumn;

		// Token: 0x040007E7 RID: 2023
		[Unsaved(false)]
		private List<Designator> resolvedDesignators = new List<Designator>();

		// Token: 0x040007E8 RID: 2024
		[Unsaved(false)]
		public KeyBindingCategoryDef bindingCatDef;

		// Token: 0x040007E9 RID: 2025
		[Unsaved(false)]
		public string cachedHighlightClosedTag;

		// Token: 0x040007EA RID: 2026
		[Unsaved(false)]
		private Dictionary<DesignationCategoryDef.BuildablePreceptBuilding, Designator> ideoBuildingDesignatorsCached = new Dictionary<DesignationCategoryDef.BuildablePreceptBuilding, Designator>();

		// Token: 0x040007EB RID: 2027
		[Unsaved(false)]
		private Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown> ideoDropdownsCached = new Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown>();

		// Token: 0x040007EC RID: 2028
		[Unsaved(false)]
		private Faction cachedPlayerFaction;

		// Token: 0x02002533 RID: 9523
		public struct BuildablePreceptBuilding
		{
			// Token: 0x170029FB RID: 10747
			// (get) Token: 0x0600ED8B RID: 60811 RVA: 0x00578FDB File Offset: 0x005771DB
			public BuildableDef Buildable
			{
				get
				{
					return this.buildable;
				}
			}

			// Token: 0x170029FC RID: 10748
			// (get) Token: 0x0600ED8C RID: 60812 RVA: 0x00578FE3 File Offset: 0x005771E3
			public Precept_Building Precept
			{
				get
				{
					return this.precept;
				}
			}

			// Token: 0x0600ED8D RID: 60813 RVA: 0x00578FEB File Offset: 0x005771EB
			public BuildablePreceptBuilding(BuildableDef buildable, Precept_Building precept)
			{
				this.buildable = buildable;
				this.precept = precept;
			}

			// Token: 0x0400983F RID: 38975
			private BuildableDef buildable;

			// Token: 0x04009840 RID: 38976
			private Precept_Building precept;
		}
	}
}
