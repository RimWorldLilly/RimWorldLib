using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200180D RID: 6157
	public class CompAnimalPenMarker : CompAutoCut, IRenameable
	{
		// Token: 0x17001A1D RID: 6685
		// (get) Token: 0x060099B9 RID: 39353 RVA: 0x003795F8 File Offset: 0x003777F8
		public CompProperties_AnimalPenMarker Props
		{
			get
			{
				return this.props as CompProperties_AnimalPenMarker;
			}
		}

		// Token: 0x17001A1E RID: 6686
		// (get) Token: 0x060099BA RID: 39354 RVA: 0x00379605 File Offset: 0x00377805
		public PenMarkerState PenState
		{
			get
			{
				ThingWithComps parent = this.parent;
				if (parent == null)
				{
					return null;
				}
				Map map = parent.Map;
				if (map == null)
				{
					return null;
				}
				return map.animalPenManager.GetPenMarkerState(this);
			}
		}

		// Token: 0x17001A1F RID: 6687
		// (get) Token: 0x060099BB RID: 39355 RVA: 0x00379629 File Offset: 0x00377829
		public ThingFilter AnimalFilter
		{
			get
			{
				return this.animalFilter;
			}
		}

		// Token: 0x17001A20 RID: 6688
		// (get) Token: 0x060099BC RID: 39356 RVA: 0x00379631 File Offset: 0x00377831
		public List<ThingDef> ForceDisplayedAnimalDefs
		{
			get
			{
				return this.forceDisplayedAnimalDefs;
			}
		}

		// Token: 0x17001A21 RID: 6689
		// (get) Token: 0x060099BD RID: 39357 RVA: 0x0037963C File Offset: 0x0037783C
		public override bool CanDesignatePlants
		{
			get
			{
				PenMarkerState penState = this.PenState;
				return penState != null && !penState.Unenclosed;
			}
		}

		// Token: 0x17001A22 RID: 6690
		// (get) Token: 0x060099BE RID: 39358 RVA: 0x0037965E File Offset: 0x0037785E
		// (set) Token: 0x060099BF RID: 39359 RVA: 0x00379670 File Offset: 0x00377870
		public string RenamableLabel
		{
			get
			{
				return this.label ?? this.BaseLabel;
			}
			set
			{
				this.label = value;
			}
		}

		// Token: 0x17001A23 RID: 6691
		// (get) Token: 0x060099C0 RID: 39360 RVA: 0x00379679 File Offset: 0x00377879
		public string BaseLabel
		{
			get
			{
				return this.parent.def.label.CapitalizeFirst();
			}
		}

		// Token: 0x17001A24 RID: 6692
		// (get) Token: 0x060099C1 RID: 39361 RVA: 0x00379690 File Offset: 0x00377890
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x17001A25 RID: 6693
		// (get) Token: 0x060099C2 RID: 39362 RVA: 0x00379698 File Offset: 0x00377898
		public PenFoodCalculator PenFoodCalculator
		{
			get
			{
				if (this.cachedFoodCalculator == null)
				{
					this.cachedFoodCalculator = new PenFoodCalculator();
					this.cachedFoodCalculator_cachedAtTick = null;
				}
				if (this.cachedFoodCalculator_cachedAtTick != null)
				{
					int ticksGame = Find.TickManager.TicksGame;
					int? num = this.cachedFoodCalculator_cachedAtTick + 20;
					if (!(ticksGame > num.GetValueOrDefault() & num != null))
					{
						goto IL_95;
					}
				}
				this.cachedFoodCalculator.ResetAndProcessPen(this);
				this.cachedFoodCalculator_cachedAtTick = new int?(Find.TickManager.TicksGame);
				IL_95:
				return this.cachedFoodCalculator;
			}
		}

		// Token: 0x060099C3 RID: 39363 RVA: 0x00379740 File Offset: 0x00377940
		public void AddForceDisplayedAnimal(ThingDef animalDef)
		{
			if (!this.forceDisplayedAnimalDefs.Contains(animalDef))
			{
				this.forceDisplayedAnimalDefs.Add(animalDef);
			}
		}

		// Token: 0x060099C4 RID: 39364 RVA: 0x0037975C File Offset: 0x0037795C
		public void RemoveForceDisplayedAnimal(ThingDef animalDef)
		{
			this.forceDisplayedAnimalDefs.Remove(animalDef);
		}

		// Token: 0x060099C5 RID: 39365 RVA: 0x0037976B File Offset: 0x0037796B
		public bool AcceptsToPen(Pawn animal)
		{
			return this.animalFilter.Allows(animal) && AnimalPenUtility.GetFixedAnimalFilter().Allows(animal);
		}

		// Token: 0x060099C6 RID: 39366 RVA: 0x00379788 File Offset: 0x00377988
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship && this.label.NullOrEmpty())
			{
				this.label = (this.parent.Map.animalPenManager.MakeNewAnimalPenName() ?? "AnimalPenMarkerDefaultLabel".Translate(1));
				this.forceDisplayedAnimalDefs.Add(ThingDefOf.Cow);
				this.forceDisplayedAnimalDefs.Add(ThingDefOf.Goat);
				this.forceDisplayedAnimalDefs.Add(ThingDefOf.Chicken);
				base.AutoCutFilter.CopyAllowancesFrom(this.parent.Map.animalPenManager.GetDefaultAutoCutFilter());
				this.animalFilter.CopyAllowancesFrom(AnimalPenUtility.GetDefaultAnimalFilter());
			}
		}

		// Token: 0x060099C7 RID: 39367 RVA: 0x00379856 File Offset: 0x00377A56
		public override IEnumerable<IntVec3> GetAutoCutCells()
		{
			CompAnimalPenMarker.<GetAutoCutCells>d__29 <GetAutoCutCells>d__ = new CompAnimalPenMarker.<GetAutoCutCells>d__29(-2);
			<GetAutoCutCells>d__.<>4__this = this;
			return <GetAutoCutCells>d__;
		}

		// Token: 0x060099C8 RID: 39368 RVA: 0x00379866 File Offset: 0x00377A66
		public override ThingFilter GetDefaultAutoCutFilter()
		{
			return this.parent.Map.animalPenManager.GetDefaultAutoCutFilter();
		}

		// Token: 0x060099C9 RID: 39369 RVA: 0x0037987D File Offset: 0x00377A7D
		public override ThingFilter GetFixedAutoCutFilter()
		{
			return this.parent.Map.animalPenManager.GetDefaultAutoCutFilter();
		}

		// Token: 0x060099CA RID: 39370 RVA: 0x00379894 File Offset: 0x00377A94
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.animalFilter, "animalFilter", Array.Empty<object>());
			Scribe_Collections.Look<ThingDef>(ref this.forceDisplayedAnimalDefs, "forceDisplayedAnimalDefs", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.forceDisplayedAnimalDefs == null)
			{
				this.forceDisplayedAnimalDefs = new List<ThingDef>();
			}
		}

		// Token: 0x060099CB RID: 39371 RVA: 0x00379900 File Offset: 0x00377B00
		public override string CompInspectStringExtra()
		{
			if (!this.parent.Spawned)
			{
				return string.Empty;
			}
			PenFoodCalculator penFoodCalculator = this.PenFoodCalculator;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.label).Append(": ").AppendLine(penFoodCalculator.PenSizeDescription());
			PenMarkerState penState = this.PenState;
			if (penState.Unenclosed)
			{
				stringBuilder.Append("PenUnenclosedLabel".Translate());
				if (penState.PassableDoors)
				{
					stringBuilder.Append(" (" + "PenOpenDoorLabel".Translate() + ")");
				}
				else
				{
					stringBuilder.Append(".");
				}
				stringBuilder.AppendLine();
			}
			else if (!penState.HasOutsideAccess)
			{
				stringBuilder.Append("NoExteriorAccess".Translate());
				stringBuilder.AppendLine();
			}
			bool flag = penFoodCalculator.SumNutritionConsumptionPerDay > penFoodCalculator.NutritionPerDayToday;
			string value = PenFoodCalculator.NutritionPerDayToString(penFoodCalculator.NutritionPerDayToday, false);
			string text = PenFoodCalculator.NutritionPerDayToString(penFoodCalculator.SumNutritionConsumptionPerDay, false);
			if (flag)
			{
				text = text.Colorize(ColorLibrary.RedReadable);
			}
			stringBuilder.Append("PenFoodTab_NaturalNutritionGrowthRate".Translate()).Append(": ").AppendLine(value);
			stringBuilder.Append("PenFoodTab_TotalNutritionConsumptionRate".Translate()).Append(": ").AppendLine(text);
			if (penFoodCalculator.sumStockpiledNutritionAvailableNow > 0f)
			{
				stringBuilder.Append("PenFoodTab_StockpileTotal".Translate()).Append(": ").AppendLine(PenFoodCalculator.NutritionToString(penFoodCalculator.sumStockpiledNutritionAvailableNow, false));
			}
			return stringBuilder.ToString().TrimEnd();
		}

		// Token: 0x04005BA9 RID: 23465
		private const int FoodCalculatorCacheTicks = 20;

		// Token: 0x04005BAA RID: 23466
		public string label = "";

		// Token: 0x04005BAB RID: 23467
		private ThingFilter animalFilter = new ThingFilter();

		// Token: 0x04005BAC RID: 23468
		private List<ThingDef> forceDisplayedAnimalDefs = new List<ThingDef>();

		// Token: 0x04005BAD RID: 23469
		private PenFoodCalculator cachedFoodCalculator;

		// Token: 0x04005BAE RID: 23470
		private int? cachedFoodCalculator_cachedAtTick;
	}
}
