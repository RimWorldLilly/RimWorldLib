using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001629 RID: 5673
	public class IdeoStyleTracker : IExposable
	{
		// Token: 0x1700188D RID: 6285
		// (get) Token: 0x06008DEC RID: 36332 RVA: 0x0033CEC5 File Offset: 0x0033B0C5
		public int NumHairAndBeardStylesAvailable
		{
			get
			{
				if (this.hairAndBeardStylesAvailable < 0)
				{
					this.RecacheStyleItemCounts();
				}
				return this.hairAndBeardStylesAvailable;
			}
		}

		// Token: 0x1700188E RID: 6286
		// (get) Token: 0x06008DED RID: 36333 RVA: 0x0033CEDC File Offset: 0x0033B0DC
		public int NumTattooStylesAvailable
		{
			get
			{
				if (this.tattooStylesAvailable < 0)
				{
					this.RecacheStyleItemCounts();
				}
				return this.tattooStylesAvailable;
			}
		}

		// Token: 0x1700188F RID: 6287
		// (get) Token: 0x06008DEE RID: 36334 RVA: 0x0033CEF4 File Offset: 0x0033B0F4
		public HairDef DisplayedHairDef
		{
			get
			{
				if (this.cachedDisplayHairDef == null)
				{
					if (this.hairFrequencies == null)
					{
						return null;
					}
					Rand.PushState(this.ideo.id);
					this.cachedDisplayHairDef = this.hairFrequencies.RandomElementByWeight((KeyValuePair<HairDef, StyleItemSpawningProperties> x) => (float)x.Value.frequency).Key;
					Rand.PopState();
				}
				return this.cachedDisplayHairDef;
			}
		}

		// Token: 0x17001890 RID: 6288
		// (get) Token: 0x06008DEF RID: 36335 RVA: 0x0033CF68 File Offset: 0x0033B168
		public TattooDef DisplayedTattooDef
		{
			get
			{
				if (this.cachedDisplayTattooDef == null)
				{
					if (this.tattooFrequencies == null)
					{
						return null;
					}
					Rand.PushState(this.ideo.id);
					KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair;
					if (!this.tattooFrequencies.TryRandomElementByWeight((KeyValuePair<TattooDef, StyleItemSpawningProperties> x) => (float)IdeoStyleTracker.<get_DisplayedTattooDef>g__GetWeight|16_0(x, false), out keyValuePair))
					{
						this.tattooFrequencies.TryRandomElementByWeight((KeyValuePair<TattooDef, StyleItemSpawningProperties> x) => (float)IdeoStyleTracker.<get_DisplayedTattooDef>g__GetWeight|16_0(x, true), out keyValuePair);
					}
					Rand.PopState();
					this.cachedDisplayTattooDef = keyValuePair.Key;
				}
				return this.cachedDisplayTattooDef;
			}
		}

		// Token: 0x06008DF0 RID: 36336 RVA: 0x0033D00D File Offset: 0x0033B20D
		public IdeoStyleTracker()
		{
		}

		// Token: 0x06008DF1 RID: 36337 RVA: 0x0033D039 File Offset: 0x0033B239
		public IdeoStyleTracker(Ideo ideo)
		{
			this.ideo = ideo;
		}

		// Token: 0x06008DF2 RID: 36338 RVA: 0x0033D06C File Offset: 0x0033B26C
		private void InitializeDefMaps()
		{
			this.hairFrequencies = new DefMap<HairDef, StyleItemSpawningProperties>();
			this.beardFrequencies = new DefMap<BeardDef, StyleItemSpawningProperties>();
			this.tattooFrequencies = new DefMap<TattooDef, StyleItemSpawningProperties>();
		}

		// Token: 0x06008DF3 RID: 36339 RVA: 0x0033D090 File Offset: 0x0033B290
		public StyleItemFrequency GetFrequency(StyleItemDef def)
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				return this.hairFrequencies[hairDef].frequency;
			}
			BeardDef beardDef = def as BeardDef;
			if (beardDef != null)
			{
				return this.beardFrequencies[beardDef].frequency;
			}
			TattooDef tattooDef = def as TattooDef;
			if (tattooDef != null)
			{
				return this.tattooFrequencies[tattooDef].frequency;
			}
			return StyleItemFrequency.Never;
		}

		// Token: 0x06008DF4 RID: 36340 RVA: 0x0033D110 File Offset: 0x0033B310
		public StyleGender GetGender(StyleItemDef def)
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				return this.hairFrequencies[hairDef].gender;
			}
			BeardDef beardDef = def as BeardDef;
			if (beardDef != null)
			{
				return this.beardFrequencies[beardDef].gender;
			}
			TattooDef tattooDef = def as TattooDef;
			if (tattooDef != null)
			{
				return this.tattooFrequencies[tattooDef].gender;
			}
			return StyleGender.Any;
		}

		// Token: 0x06008DF5 RID: 36341 RVA: 0x0033D190 File Offset: 0x0033B390
		public void SetFrequency(StyleItemDef def, StyleItemFrequency freq)
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				this.hairFrequencies[hairDef].frequency = freq;
			}
			else
			{
				BeardDef beardDef = def as BeardDef;
				if (beardDef != null)
				{
					this.beardFrequencies[beardDef].frequency = freq;
				}
				else
				{
					TattooDef tattooDef = def as TattooDef;
					if (tattooDef != null)
					{
						this.tattooFrequencies[tattooDef].frequency = freq;
					}
				}
			}
			this.hairAndBeardStylesAvailable = -1;
			this.tattooStylesAvailable = -1;
			this.cachedDisplayHairDef = null;
			this.cachedDisplayTattooDef = null;
		}

		// Token: 0x06008DF6 RID: 36342 RVA: 0x0033D230 File Offset: 0x0033B430
		public void SetGender(StyleItemDef def, StyleGender gender)
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				this.hairFrequencies[hairDef].gender = gender;
				return;
			}
			BeardDef beardDef = def as BeardDef;
			if (beardDef != null)
			{
				this.beardFrequencies[beardDef].gender = gender;
				return;
			}
			TattooDef tattooDef = def as TattooDef;
			if (tattooDef != null)
			{
				this.tattooFrequencies[tattooDef].gender = gender;
			}
		}

		// Token: 0x06008DF7 RID: 36343 RVA: 0x0033D2B4 File Offset: 0x0033B4B4
		public void EnsureAtLeastOneStyleItemAvailable()
		{
			bool flag = false;
			foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
			{
				if (keyValuePair.Value.frequency > StyleItemFrequency.Never)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.hairFrequencies[HairDefOf.Bald].frequency = StyleItemFrequency.Normal;
				this.hairFrequencies[HairDefOf.Bald].gender = StyleGender.Any;
			}
			flag = false;
			foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
			{
				if (keyValuePair2.Value.frequency != StyleItemFrequency.Never)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.beardFrequencies[BeardDefOf.NoBeard].frequency = StyleItemFrequency.Normal;
				this.beardFrequencies[BeardDefOf.NoBeard].gender = StyleGender.Any;
			}
			bool flag2 = false;
			bool flag3 = false;
			foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
			{
				if (keyValuePair3.Value.frequency != StyleItemFrequency.Never)
				{
					if (keyValuePair3.Key.tattooType == TattooType.Face)
					{
						flag2 = true;
					}
					else if (keyValuePair3.Key.tattooType == TattooType.Body)
					{
						flag3 = true;
					}
				}
				if (flag2 && flag3)
				{
					break;
				}
			}
			if (!flag2)
			{
				this.tattooFrequencies[TattooDefOf.NoTattoo_Face].frequency = StyleItemFrequency.Normal;
				this.tattooFrequencies[TattooDefOf.NoTattoo_Face].gender = StyleGender.Any;
			}
			if (!flag3)
			{
				this.tattooFrequencies[TattooDefOf.NoTattoo_Body].frequency = StyleItemFrequency.Normal;
				this.tattooFrequencies[TattooDefOf.NoTattoo_Face].gender = StyleGender.Any;
			}
			this.hairAndBeardStylesAvailable = -1;
			this.tattooStylesAvailable = -1;
		}

		// Token: 0x06008DF8 RID: 36344 RVA: 0x0033D4A0 File Offset: 0x0033B6A0
		private void RecacheStyleItemCounts()
		{
			if (this.ideo.culture == null)
			{
				return;
			}
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				this.RecalculateAvailableStyleItems();
			}
			this.hairAndBeardStylesAvailable = 0;
			foreach (HairDef def in DefDatabase<HairDef>.AllDefs)
			{
				if (this.hairFrequencies[def].frequency != StyleItemFrequency.Never)
				{
					this.hairAndBeardStylesAvailable++;
				}
			}
			foreach (BeardDef def2 in DefDatabase<BeardDef>.AllDefs)
			{
				if (this.beardFrequencies[def2].frequency != StyleItemFrequency.Never)
				{
					this.hairAndBeardStylesAvailable++;
				}
			}
			this.tattooStylesAvailable = 0;
			foreach (TattooDef tattooDef in DefDatabase<TattooDef>.AllDefs)
			{
				if (tattooDef != TattooDefOf.NoTattoo_Face && tattooDef != TattooDefOf.NoTattoo_Body && this.tattooFrequencies[tattooDef].frequency != StyleItemFrequency.Never)
				{
					this.tattooStylesAvailable++;
				}
			}
		}

		// Token: 0x06008DF9 RID: 36345 RVA: 0x0033D604 File Offset: 0x0033B804
		public void RecalculateAvailableStyleItems()
		{
			if (!this.ideo.culture.styleItemTags.NullOrEmpty<StyleItemTagWeighted>())
			{
				this.memberStyleTags.AddRange(this.ideo.culture.styleItemTags);
			}
			int i;
			int num;
			for (i = 0; i < this.ideo.memes.Count; i = num + 1)
			{
				if (!this.ideo.memes[i].styleItemTags.NullOrEmpty<StyleItemTagWeighted>())
				{
					int j;
					for (j = 0; j < this.ideo.memes[i].styleItemTags.Count; j = num + 1)
					{
						StyleItemTagWeighted styleItemTagWeighted = this.memberStyleTags.Find((StyleItemTagWeighted x) => x.Tag == this.ideo.memes[i].styleItemTags[j].Tag);
						if (styleItemTagWeighted == null)
						{
							this.memberStyleTags.Add(this.ideo.memes[i].styleItemTags[j]);
						}
						else
						{
							styleItemTagWeighted.Add(this.ideo.memes[i].styleItemTags[j]);
						}
						num = j;
					}
				}
				num = i;
			}
			this.InitializeDefMaps();
			this.<RecalculateAvailableStyleItems>g__SetupFrequency|27_0<HairDef>();
			this.<RecalculateAvailableStyleItems>g__SetupFrequency|27_0<BeardDef>();
			this.<RecalculateAvailableStyleItems>g__SetupFrequency|27_0<TattooDef>();
			this.EnsureAtLeastOneStyleItemAvailable();
			this.memberStyleTags.Clear();
		}

		// Token: 0x06008DFA RID: 36346 RVA: 0x0033D7B4 File Offset: 0x0033B9B4
		private bool NeedToRecacheStyleItems()
		{
			if (this.hairFrequencies == null || this.beardFrequencies == null || this.tattooFrequencies == null)
			{
				return true;
			}
			foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
			{
				if (keyValuePair.Value.frequency != StyleItemFrequency.Never)
				{
					return false;
				}
			}
			foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
			{
				if (keyValuePair2.Value.frequency != StyleItemFrequency.Never)
				{
					return false;
				}
			}
			foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
			{
				if (keyValuePair3.Value.frequency != StyleItemFrequency.Never)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008DFB RID: 36347 RVA: 0x0033D8C4 File Offset: 0x0033BAC4
		public StyleCategoryPair StyleForThingDef(ThingDef thing, Precept precept = null)
		{
			if (thing == null)
			{
				return null;
			}
			if (this.styleForThingDef.ContainsKey(thing))
			{
				return this.styleForThingDef[thing];
			}
			if (Find.IdeoManager.classicMode)
			{
				foreach (StyleCategoryDef styleCategoryDef in Find.IdeoManager.selectedStyleCategories)
				{
					ThingStyleDef thingStyleDef = styleCategoryDef.GetStyleForThingDef(thing, precept);
					if (thingStyleDef != null)
					{
						StyleCategoryPair styleCategoryPair = new StyleCategoryPair
						{
							styleDef = thingStyleDef,
							category = styleCategoryDef
						};
						this.styleForThingDef.Add(thing, styleCategoryPair);
						return styleCategoryPair;
					}
				}
			}
			foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.ideo.thingStyleCategories)
			{
				ThingStyleDef thingStyleDef2 = thingStyleCategoryWithPriority.category.GetStyleForThingDef(thing, precept);
				if (thingStyleDef2 != null)
				{
					StyleCategoryPair styleCategoryPair2 = new StyleCategoryPair
					{
						styleDef = thingStyleDef2,
						category = thingStyleCategoryWithPriority.category
					};
					this.styleForThingDef.Add(thing, styleCategoryPair2);
					return styleCategoryPair2;
				}
			}
			return null;
		}

		// Token: 0x06008DFC RID: 36348 RVA: 0x0033DA00 File Offset: 0x0033BC00
		public void SetStyleForThingDef(ThingDef thing, StyleCategoryPair styleAndCat)
		{
			this.styleForThingDef.SetOrAdd(thing, styleAndCat);
		}

		// Token: 0x06008DFD RID: 36349 RVA: 0x0033DA0F File Offset: 0x0033BC0F
		public void ResetStylesForThingDef()
		{
			this.styleForThingDef.RemoveAll((KeyValuePair<ThingDef, StyleCategoryPair> kvp) => !this.ideo.PreceptsListForReading.Any(delegate(Precept p)
			{
				Precept_ThingDef precept_ThingDef = p as Precept_ThingDef;
				return precept_ThingDef != null && precept_ThingDef.ThingDef == kvp.Key;
			}) || !kvp.Key.canEditAnyStyle);
		}

		// Token: 0x06008DFE RID: 36350 RVA: 0x0033DA29 File Offset: 0x0033BC29
		public void ResetStyleForThing(ThingDef thingDef)
		{
			this.styleForThingDef.Remove(thingDef);
		}

		// Token: 0x06008DFF RID: 36351 RVA: 0x0033DA38 File Offset: 0x0033BC38
		public virtual void CopyTo(IdeoStyleTracker other)
		{
			other.InitializeDefMaps();
			foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
			{
				other.hairFrequencies[keyValuePair.Key] = keyValuePair.Value;
			}
			foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
			{
				other.beardFrequencies[keyValuePair2.Key] = keyValuePair2.Value;
			}
			foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
			{
				other.tattooFrequencies[keyValuePair3.Key] = keyValuePair3.Value;
			}
			other.styleForThingDef.Clear();
			other.styleForThingDef.AddRange(this.styleForThingDef);
			other.cachedDisplayHairDef = this.cachedDisplayHairDef;
			other.cachedDisplayTattooDef = this.cachedDisplayTattooDef;
			other.RecacheStyleItemCounts();
		}

		// Token: 0x06008E00 RID: 36352 RVA: 0x0033DB7C File Offset: 0x0033BD7C
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<HairDef, StyleItemSpawningProperties>>(ref this.hairFrequencies, "hairFrequencies", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<BeardDef, StyleItemSpawningProperties>>(ref this.beardFrequencies, "beardFrequencies", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<TattooDef, StyleItemSpawningProperties>>(ref this.tattooFrequencies, "tattooFrequencies", Array.Empty<object>());
			Scribe_Collections.Look<ThingDef, StyleCategoryPair>(ref this.styleForThingDef, "styleForThingDef", LookMode.Def, LookMode.Deep);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.NeedToRecacheStyleItems())
			{
				this.RecalculateAvailableStyleItems();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.styleForThingDef == null)
				{
					this.styleForThingDef = new Dictionary<ThingDef, StyleCategoryPair>();
					return;
				}
				this.styleForThingDef.RemoveAll((KeyValuePair<ThingDef, StyleCategoryPair> x) => x.Key == null || x.Value == null);
			}
		}

		// Token: 0x06008E01 RID: 36353 RVA: 0x0033DC37 File Offset: 0x0033BE37
		[CompilerGenerated]
		internal static int <get_DisplayedTattooDef>g__GetWeight|16_0(KeyValuePair<TattooDef, StyleItemSpawningProperties> pair, bool ignoreFrequency)
		{
			if (pair.Key == TattooDefOf.NoTattoo_Body || pair.Key == TattooDefOf.NoTattoo_Face)
			{
				return 0;
			}
			if (ignoreFrequency)
			{
				return 1;
			}
			return (int)pair.Value.frequency;
		}

		// Token: 0x06008E02 RID: 36354 RVA: 0x0033DC68 File Offset: 0x0033BE68
		[CompilerGenerated]
		private void <RecalculateAvailableStyleItems>g__SetupFrequency|27_0<T>() where T : StyleItemDef
		{
			foreach (T t in DefDatabase<T>.AllDefs)
			{
				float freq = PawnStyleItemChooser.StyleItemChoiceLikelihoodFromTags(t, this.memberStyleTags);
				bool flag = t is TattooDef && this.ideo.classicMode;
				this.SetFrequency(t, flag ? StyleItemFrequency.Never : PawnStyleItemChooser.GetStyleItemFrequency(freq));
				this.SetGender(t, t.styleGender);
			}
		}

		// Token: 0x04005700 RID: 22272
		public Ideo ideo;

		// Token: 0x04005701 RID: 22273
		private DefMap<HairDef, StyleItemSpawningProperties> hairFrequencies;

		// Token: 0x04005702 RID: 22274
		private DefMap<BeardDef, StyleItemSpawningProperties> beardFrequencies;

		// Token: 0x04005703 RID: 22275
		private DefMap<TattooDef, StyleItemSpawningProperties> tattooFrequencies;

		// Token: 0x04005704 RID: 22276
		private Dictionary<ThingDef, StyleCategoryPair> styleForThingDef = new Dictionary<ThingDef, StyleCategoryPair>();

		// Token: 0x04005705 RID: 22277
		private int hairAndBeardStylesAvailable = -1;

		// Token: 0x04005706 RID: 22278
		private int tattooStylesAvailable = -1;

		// Token: 0x04005707 RID: 22279
		private HairDef cachedDisplayHairDef;

		// Token: 0x04005708 RID: 22280
		private TattooDef cachedDisplayTattooDef;

		// Token: 0x04005709 RID: 22281
		private List<StyleItemTagWeighted> memberStyleTags = new List<StyleItemTagWeighted>();
	}
}
