using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020015E6 RID: 5606
	public class TraitSet : IExposable
	{
		// Token: 0x1700182D RID: 6189
		// (get) Token: 0x06008B5C RID: 35676 RVA: 0x00328259 File Offset: 0x00326459
		public bool AnyTraitHasIngestibleOverrides
		{
			get
			{
				return this.anyTraitHasIngestibleOverrides;
			}
		}

		// Token: 0x1700182E RID: 6190
		// (get) Token: 0x06008B5D RID: 35677 RVA: 0x00328264 File Offset: 0x00326464
		public float HungerRateFactor
		{
			get
			{
				float num = 1f;
				foreach (Trait trait in this.allTraits)
				{
					if (!trait.Suppressed)
					{
						num *= trait.CurrentData.hungerRateFactor;
					}
				}
				return num;
			}
		}

		// Token: 0x1700182F RID: 6191
		// (get) Token: 0x06008B5E RID: 35678 RVA: 0x003282D0 File Offset: 0x003264D0
		public List<Trait> TraitsSorted
		{
			get
			{
				this.tmpTraits.Clear();
				if (!this.allTraits.NullOrEmpty<Trait>())
				{
					for (int i = 0; i < this.allTraits.Count; i++)
					{
						if (this.allTraits[i].sourceGene == null || !this.allTraits[i].Suppressed)
						{
							this.tmpTraits.Add(this.allTraits[i]);
						}
					}
					this.tmpTraits.SortBy((Trait y) => y.sourceGene != null, (Trait x) => x.Suppressed);
				}
				return this.tmpTraits;
			}
		}

		// Token: 0x06008B5F RID: 35679 RVA: 0x0032839A File Offset: 0x0032659A
		public TraitSet(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008B60 RID: 35680 RVA: 0x003283C0 File Offset: 0x003265C0
		public void ExposeData()
		{
			Scribe_Collections.Look<Trait>(ref this.allTraits, "allTraits", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.allTraits.RemoveAll((Trait x) => x == null) != 0)
				{
					Log.Error("Some traits were null after loading.");
				}
				if (this.allTraits.RemoveAll((Trait x) => x.def == null) != 0)
				{
					Log.Error("Some traits had null def after loading.");
				}
				for (int i = 0; i < this.allTraits.Count; i++)
				{
					this.allTraits[i].pawn = this.pawn;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheTraits();
				for (int j = this.allTraits.Count - 1; j >= 0; j--)
				{
					Trait t = this.allTraits[j];
					Gene sourceGene = t.sourceGene;
					if (((sourceGene != null) ? sourceGene.def : null) != null && (t.sourceGene.def.forcedTraits == null || !t.sourceGene.def.forcedTraits.Any((GeneticTraitData x) => x.def == t.def && x.degree == t.Degree)))
					{
						this.allTraits.Remove(t);
					}
				}
			}
		}

		// Token: 0x06008B61 RID: 35681 RVA: 0x00328538 File Offset: 0x00326738
		public bool DisableHostilityFrom(Pawn p)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (!this.allTraits[i].Suppressed)
				{
					if (p.Faction != null && this.allTraits[i].def.disableHostilityFromFaction == p.Faction.def)
					{
						return true;
					}
					if (this.allTraits[i].def.disableHostilityFromAnimalType != null)
					{
						AnimalType? disableHostilityFromAnimalType = this.allTraits[i].def.disableHostilityFromAnimalType;
						AnimalType animalType = p.RaceProps.animalType;
						if (disableHostilityFromAnimalType.GetValueOrDefault() == animalType & disableHostilityFromAnimalType != null)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06008B62 RID: 35682 RVA: 0x003285FC File Offset: 0x003267FC
		public bool IsThoughtDisallowed(ThoughtDef thought)
		{
			if (this.pawn.story == null || thought == null)
			{
				return false;
			}
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (!this.allTraits[i].Suppressed)
				{
					TraitDegreeData currentData = this.allTraits[i].CurrentData;
					if (currentData.disallowedThoughts != null)
					{
						for (int j = 0; j < currentData.disallowedThoughts.Count; j++)
						{
							if (currentData.disallowedThoughts[j] == thought)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06008B63 RID: 35683 RVA: 0x00328688 File Offset: 0x00326888
		public bool IsThoughtFromIngestionDisallowed(ThoughtDef thought, ThingDef ingestible, MeatSourceCategory meatSourceCategory)
		{
			if (thought == null || ingestible == null)
			{
				return false;
			}
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (!this.allTraits[i].Suppressed)
				{
					TraitDegreeData currentData = this.allTraits[i].CurrentData;
					if (currentData.disallowedThoughtsFromIngestion != null)
					{
						for (int j = 0; j < currentData.disallowedThoughtsFromIngestion.Count; j++)
						{
							TraitIngestionThoughtsOverride traitIngestionThoughtsOverride = currentData.disallowedThoughtsFromIngestion[j];
							if (!traitIngestionThoughtsOverride.thoughts.NullOrEmpty<ThoughtDef>())
							{
								if (traitIngestionThoughtsOverride.thing != null)
								{
									if (traitIngestionThoughtsOverride.thing != ingestible)
									{
										goto IL_A8;
									}
								}
								else if (meatSourceCategory == MeatSourceCategory.NotMeat || traitIngestionThoughtsOverride.meatSource != meatSourceCategory)
								{
									goto IL_A8;
								}
								for (int k = 0; k < traitIngestionThoughtsOverride.thoughts.Count; k++)
								{
									if (traitIngestionThoughtsOverride.thoughts[k] == thought)
									{
										return true;
									}
								}
							}
							IL_A8:;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06008B64 RID: 35684 RVA: 0x00328768 File Offset: 0x00326968
		public void GetExtraThoughtsFromIngestion(List<ThoughtDef> buffer, ThingDef ingestible, MeatSourceCategory meatSourceCategory, bool direct)
		{
			if (ingestible == null || buffer == null)
			{
				return;
			}
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (!this.allTraits[i].Suppressed)
				{
					TraitDegreeData currentData = this.allTraits[i].CurrentData;
					if (currentData.extraThoughtsFromIngestion != null)
					{
						int j = 0;
						while (j < currentData.extraThoughtsFromIngestion.Count)
						{
							TraitIngestionThoughtsOverride traitIngestionThoughtsOverride = currentData.extraThoughtsFromIngestion[j];
							if (traitIngestionThoughtsOverride.thing != null)
							{
								if (traitIngestionThoughtsOverride.thing == ingestible)
								{
									goto IL_74;
								}
							}
							else if (meatSourceCategory != MeatSourceCategory.NotMeat && traitIngestionThoughtsOverride.meatSource == meatSourceCategory)
							{
								goto IL_74;
							}
							IL_C5:
							j++;
							continue;
							IL_74:
							if (!traitIngestionThoughtsOverride.thoughts.NullOrEmpty<ThoughtDef>())
							{
								buffer.AddRange(traitIngestionThoughtsOverride.thoughts);
							}
							if (direct)
							{
								if (!traitIngestionThoughtsOverride.thoughtsDirect.NullOrEmpty<ThoughtDef>())
								{
									buffer.AddRange(traitIngestionThoughtsOverride.thoughtsDirect);
									goto IL_C5;
								}
								goto IL_C5;
							}
							else
							{
								if (!traitIngestionThoughtsOverride.thoughtsAsIngredient.NullOrEmpty<ThoughtDef>())
								{
									buffer.AddRange(traitIngestionThoughtsOverride.thoughtsAsIngredient);
									goto IL_C5;
								}
								goto IL_C5;
							}
						}
					}
				}
			}
		}

		// Token: 0x06008B65 RID: 35685 RVA: 0x00328864 File Offset: 0x00326A64
		public void GainTrait(Trait trait, bool suppressConflicts = false)
		{
			if (!suppressConflicts && this.HasTrait(trait.def))
			{
				Pawn pawn = this.pawn;
				string str = (pawn != null) ? pawn.ToString() : null;
				string str2 = " already has trait ";
				TraitDef def = trait.def;
				Log.Warning(str + str2 + ((def != null) ? def.ToString() : null));
				return;
			}
			if (this.HasNonSuppressedTrait(trait))
			{
				return;
			}
			this.allTraits.Add(trait);
			trait.pawn = this.pawn;
			if ((ModsConfig.BiotechActive || ModsConfig.AnomalyActive) && suppressConflicts)
			{
				if (this.allTraits.Any((Trait x) => x != trait && trait.def.CanSuppress(x) && !x.def.canBeSuppressed))
				{
					trait.suppressedByTrait = true;
				}
				else
				{
					for (int i = 0; i < this.allTraits.Count; i++)
					{
						if (this.allTraits[i] != trait && trait.def.CanSuppress(this.allTraits[i]))
						{
							this.allTraits[i].suppressedByTrait = true;
						}
					}
				}
				this.RecalculateSuppression();
			}
			this.pawn.Notify_DisabledWorkTypesChanged();
			if (trait.CurrentData.HasDefinedGraphicProperties)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
			if (this.pawn.skills != null)
			{
				this.pawn.skills.Notify_SkillDisablesChanged();
				this.pawn.skills.DirtyAptitudes();
			}
			if (!this.pawn.Dead && this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			List<AbilityDef> abilities = trait.def.DataAtDegree(trait.Degree).abilities;
			if (!abilities.NullOrEmpty<AbilityDef>())
			{
				for (int j = 0; j < abilities.Count; j++)
				{
					this.pawn.abilities.GainAbility(abilities[j]);
				}
			}
			if (trait.def.disableHostilityFromAnimalType != null)
			{
				AnimalType? disableHostilityFromAnimalType = trait.def.disableHostilityFromAnimalType;
				AnimalType animalType = AnimalType.None;
				if (!(disableHostilityFromAnimalType.GetValueOrDefault() == animalType & disableHostilityFromAnimalType != null))
				{
					goto IL_27D;
				}
			}
			if (trait.def.disableHostilityFromFaction == null)
			{
				goto IL_2A5;
			}
			IL_27D:
			if (this.pawn.Map != null)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			IL_2A5:
			this.RecacheTraits();
			if (!trait.CurrentData.enablesNeeds.NullOrEmpty<NeedDef>() || !trait.CurrentData.disablesNeeds.NullOrEmpty<NeedDef>())
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs != null)
				{
					needs.AddOrRemoveNeedsAsAppropriate();
				}
			}
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
		}

		// Token: 0x06008B66 RID: 35686 RVA: 0x00328B6C File Offset: 0x00326D6C
		public void RemoveTrait(Trait trait, bool unsuppressConflicts = false)
		{
			if (!this.HasTrait(trait.def))
			{
				string[] array = new string[5];
				array[0] = "Trying to remove ";
				array[1] = trait.Label;
				array[2] = " but ";
				int num = 3;
				Pawn pawn = this.pawn;
				array[num] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = " doesn't have it.";
				Log.Warning(string.Concat(array));
				return;
			}
			List<AbilityDef> abilities = trait.def.DataAtDegree(trait.Degree).abilities;
			if (!abilities.NullOrEmpty<AbilityDef>())
			{
				for (int i = 0; i < abilities.Count; i++)
				{
					this.pawn.abilities.RemoveAbility(abilities[i]);
				}
			}
			this.allTraits.Remove(trait);
			if (trait.sourceGene != null)
			{
				this.pawn.genes.RemoveGene(trait.sourceGene);
			}
			if ((ModsConfig.BiotechActive || ModsConfig.AnomalyActive) && unsuppressConflicts)
			{
				for (int j = 0; j < this.allTraits.Count; j++)
				{
					if (this.allTraits[j].Suppressed)
					{
						bool flag = false;
						for (int k = 0; k < this.allTraits.Count; k++)
						{
							if (this.allTraits[j] != this.allTraits[k] && (this.allTraits[k].sourceGene == null || !this.allTraits[k].sourceGene.Overridden) && this.allTraits[j].def.CanSuppress(this.allTraits[k]))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.allTraits[j].suppressedByTrait = false;
						}
					}
				}
			}
			this.pawn.Notify_DisabledWorkTypesChanged();
			if (trait.CurrentData.HasDefinedGraphicProperties)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
			if (this.pawn.skills != null)
			{
				this.pawn.skills.Notify_SkillDisablesChanged();
				this.pawn.skills.DirtyAptitudes();
			}
			if (!this.pawn.Dead && this.pawn.RaceProps.Humanlike && this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (trait.def.disableHostilityFromAnimalType != null)
			{
				AnimalType? disableHostilityFromAnimalType = trait.def.disableHostilityFromAnimalType;
				AnimalType animalType = AnimalType.None;
				if (!(disableHostilityFromAnimalType.GetValueOrDefault() == animalType & disableHostilityFromAnimalType != null))
				{
					goto IL_2A2;
				}
			}
			if (trait.def.disableHostilityFromFaction == null)
			{
				goto IL_2CA;
			}
			IL_2A2:
			if (this.pawn.Map != null)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			IL_2CA:
			this.RecacheTraits();
			if (!trait.CurrentData.enablesNeeds.NullOrEmpty<NeedDef>() || !trait.CurrentData.disablesNeeds.NullOrEmpty<NeedDef>())
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs != null)
				{
					needs.AddOrRemoveNeedsAsAppropriate();
				}
			}
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
		}

		// Token: 0x06008B67 RID: 35687 RVA: 0x00328E90 File Offset: 0x00327090
		public void Notify_GeneRemoved(Gene gene)
		{
			if (ModsConfig.BiotechActive)
			{
				for (int i = this.allTraits.Count - 1; i >= 0; i--)
				{
					if (this.allTraits[i].sourceGene == gene)
					{
						this.RemoveTrait(this.allTraits[i], true);
					}
				}
				this.RecalculateSuppression();
			}
		}

		// Token: 0x06008B68 RID: 35688 RVA: 0x00328EEC File Offset: 0x003270EC
		public void RecalculateSuppression()
		{
			if (!ModsConfig.BiotechActive || this.pawn.genes == null)
			{
				return;
			}
			List<Gene> genesListForReading = this.pawn.genes.GenesListForReading;
			int i;
			Predicate<GeneticTraitData> <>9__0;
			int i2;
			for (i = 0; i < this.allTraits.Count; i = i2 + 1)
			{
				bool flag = false;
				if (this.allTraits[i].sourceGene != null && this.allTraits[i].sourceGene.Overridden)
				{
					this.allTraits[i].suppressedByGene = this.allTraits[i].sourceGene.overriddenByGene;
					flag = true;
				}
				else
				{
					for (int j = 0; j < genesListForReading.Count; j++)
					{
						if (genesListForReading[j].Active && genesListForReading[j].def.suppressedTraits != null)
						{
							List<GeneticTraitData> suppressedTraits = genesListForReading[j].def.suppressedTraits;
							Predicate<GeneticTraitData> predicate;
							if ((predicate = <>9__0) == null)
							{
								predicate = (<>9__0 = ((GeneticTraitData x) => x.def == this.allTraits[i].def && x.degree == this.allTraits[i].Degree));
							}
							if (suppressedTraits.Any(predicate))
							{
								this.allTraits[i].suppressedByGene = genesListForReading[j];
								flag = true;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					this.allTraits[i].suppressedByGene = null;
				}
				i2 = i;
			}
		}

		// Token: 0x06008B69 RID: 35689 RVA: 0x00329078 File Offset: 0x00327278
		public bool HasTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef && !this.allTraits[i].Suppressed)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008B6A RID: 35690 RVA: 0x003290C8 File Offset: 0x003272C8
		public bool HasTrait(TraitDef tDef, int degree)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef && this.allTraits[i].Degree == degree && !this.allTraits[i].Suppressed)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008B6B RID: 35691 RVA: 0x0032912C File Offset: 0x0032732C
		public bool HasNonSuppressedTrait(Trait trait)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				Trait trait2 = this.allTraits[i];
				if ((trait.sourceGene == null || trait2.sourceGene == null || !trait.sourceGene.def.Overrides(trait2.sourceGene.def, this.pawn.genes.IsXenogene(trait.sourceGene), this.pawn.genes.IsXenogene(trait2.sourceGene))) && trait2.def == trait.def && trait2.Degree == trait.Degree && !trait2.Suppressed)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008B6C RID: 35692 RVA: 0x003291E2 File Offset: 0x003273E2
		public bool TryGetNeedEnablingTrait(NeedDef def, out Trait trait)
		{
			if (this.cachedEnabledNeeds == null)
			{
				trait = null;
				return false;
			}
			return this.cachedEnabledNeeds.TryGetValue(def, out trait);
		}

		// Token: 0x06008B6D RID: 35693 RVA: 0x003291FE File Offset: 0x003273FE
		public bool TryGetNeedDisablingTrait(NeedDef def, out Trait trait)
		{
			if (this.cachedDisabledNeeds == null)
			{
				trait = null;
				return false;
			}
			return this.cachedDisabledNeeds.TryGetValue(def, out trait);
		}

		// Token: 0x06008B6E RID: 35694 RVA: 0x0032921A File Offset: 0x0032741A
		public bool EnablesNeed(NeedDef def)
		{
			Dictionary<NeedDef, Trait> dictionary = this.cachedEnabledNeeds;
			return dictionary != null && dictionary.ContainsKey(def);
		}

		// Token: 0x06008B6F RID: 35695 RVA: 0x0032922E File Offset: 0x0032742E
		public bool DisablesNeed(NeedDef def)
		{
			Dictionary<NeedDef, Trait> dictionary = this.cachedDisabledNeeds;
			return dictionary != null && dictionary.ContainsKey(def);
		}

		// Token: 0x17001830 RID: 6192
		// (get) Token: 0x06008B70 RID: 35696 RVA: 0x00329242 File Offset: 0x00327442
		public IEnumerable<MentalBreakDef> TheOnlyAllowedMentalBreaks
		{
			get
			{
				TraitSet.<get_TheOnlyAllowedMentalBreaks>d__30 <get_TheOnlyAllowedMentalBreaks>d__ = new TraitSet.<get_TheOnlyAllowedMentalBreaks>d__30(-2);
				<get_TheOnlyAllowedMentalBreaks>d__.<>4__this = this;
				return <get_TheOnlyAllowedMentalBreaks>d__;
			}
		}

		// Token: 0x06008B71 RID: 35697 RVA: 0x00329254 File Offset: 0x00327454
		public Trait GetTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef && !this.allTraits[i].Suppressed)
				{
					return this.allTraits[i];
				}
			}
			return null;
		}

		// Token: 0x06008B72 RID: 35698 RVA: 0x003292AC File Offset: 0x003274AC
		public Trait GetTrait(TraitDef tDef, int degree)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef && this.allTraits[i].Degree == degree && !this.allTraits[i].Suppressed)
				{
					return this.allTraits[i];
				}
			}
			return null;
		}

		// Token: 0x06008B73 RID: 35699 RVA: 0x00329318 File Offset: 0x00327518
		public int DegreeOfTrait(TraitDef tDef)
		{
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				if (this.allTraits[i].def == tDef && !this.allTraits[i].Suppressed)
				{
					return this.allTraits[i].Degree;
				}
			}
			return 0;
		}

		// Token: 0x06008B74 RID: 35700 RVA: 0x00329378 File Offset: 0x00327578
		private void RecacheTraits()
		{
			this.anyTraitHasIngestibleOverrides = false;
			Dictionary<NeedDef, Trait> dictionary = this.cachedDisabledNeeds;
			if (dictionary != null)
			{
				dictionary.Clear();
			}
			Dictionary<NeedDef, Trait> dictionary2 = this.cachedEnabledNeeds;
			if (dictionary2 != null)
			{
				dictionary2.Clear();
			}
			for (int i = 0; i < this.allTraits.Count; i++)
			{
				Trait trait = this.allTraits[i];
				if (!trait.Suppressed && !trait.CurrentData.ingestibleModifiers.NullOrEmpty<IngestibleModifiers>())
				{
					this.anyTraitHasIngestibleOverrides = true;
				}
				if (!trait.Suppressed && !trait.CurrentData.enablesNeeds.NullOrEmpty<NeedDef>())
				{
					for (int j = 0; j < trait.CurrentData.enablesNeeds.Count; j++)
					{
						NeedDef key = trait.CurrentData.enablesNeeds[j];
						if (this.cachedEnabledNeeds == null)
						{
							this.cachedEnabledNeeds = new Dictionary<NeedDef, Trait>();
						}
						this.cachedEnabledNeeds[key] = trait;
					}
				}
				if (!trait.Suppressed && !trait.CurrentData.disablesNeeds.NullOrEmpty<NeedDef>())
				{
					for (int k = 0; k < trait.CurrentData.disablesNeeds.Count; k++)
					{
						NeedDef key2 = trait.CurrentData.disablesNeeds[k];
						if (this.cachedDisabledNeeds == null)
						{
							this.cachedDisabledNeeds = new Dictionary<NeedDef, Trait>();
						}
						this.cachedDisabledNeeds[key2] = trait;
					}
				}
			}
		}

		// Token: 0x0400559E RID: 21918
		protected Pawn pawn;

		// Token: 0x0400559F RID: 21919
		public List<Trait> allTraits = new List<Trait>();

		// Token: 0x040055A0 RID: 21920
		private bool anyTraitHasIngestibleOverrides;

		// Token: 0x040055A1 RID: 21921
		private Dictionary<NeedDef, Trait> cachedEnabledNeeds;

		// Token: 0x040055A2 RID: 21922
		private Dictionary<NeedDef, Trait> cachedDisabledNeeds;

		// Token: 0x040055A3 RID: 21923
		private readonly List<Trait> tmpTraits = new List<Trait>();
	}
}
