using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001620 RID: 5664
	public class Ideo : IExposable, ILoadReferenceable
	{
		// Token: 0x1700186C RID: 6252
		// (get) Token: 0x06008D3D RID: 36157 RVA: 0x00337919 File Offset: 0x00335B19
		public List<Precept> PreceptsListForReading
		{
			get
			{
				return this.precepts;
			}
		}

		// Token: 0x1700186D RID: 6253
		// (get) Token: 0x06008D3E RID: 36158 RVA: 0x00337924 File Offset: 0x00335B24
		public Texture2D Icon
		{
			get
			{
				Texture2D result;
				if ((result = this.icon) == null)
				{
					result = (this.icon = ContentFinder<Texture2D>.Get((this.iconDef != null) ? this.iconDef.iconPath : BaseContent.BadTexPath, true));
				}
				return result;
			}
		}

		// Token: 0x1700186E RID: 6254
		// (get) Token: 0x06008D3F RID: 36159 RVA: 0x00337964 File Offset: 0x00335B64
		public List<Precept_Role> RolesListForReading
		{
			get
			{
				return this.cachedPossibleRoles;
			}
		}

		// Token: 0x1700186F RID: 6255
		// (get) Token: 0x06008D40 RID: 36160 RVA: 0x0033796C File Offset: 0x00335B6C
		public bool WarnPlayerOnDesignateChopTree
		{
			get
			{
				return this.warnPlayerOnDesignateChopTree;
			}
		}

		// Token: 0x17001870 RID: 6256
		// (get) Token: 0x06008D41 RID: 36161 RVA: 0x00337974 File Offset: 0x00335B74
		public bool WarnPlayerOnDesignateMine
		{
			get
			{
				return this.warnPlayerOnDesignateMine;
			}
		}

		// Token: 0x17001871 RID: 6257
		// (get) Token: 0x06008D42 RID: 36162 RVA: 0x0033797C File Offset: 0x00335B7C
		public Color Color
		{
			get
			{
				if (this.classicMode)
				{
					return Color.white;
				}
				Color? color = this.primaryFactionColor;
				if (color != null)
				{
					return color.GetValueOrDefault();
				}
				ColorDef colorDef = this.colorDef;
				if (colorDef == null)
				{
					return Color.white;
				}
				return colorDef.color;
			}
		}

		// Token: 0x17001872 RID: 6258
		// (get) Token: 0x06008D43 RID: 36163 RVA: 0x003379C4 File Offset: 0x00335BC4
		public Color ApparelColor
		{
			get
			{
				float h;
				float b;
				float b2;
				Color.RGBToHSV(this.Color, out h, out b, out b2);
				return Color.HSVToRGB(h, Mathf.Min(0.5f, b), Mathf.Min(0.8235294f, b2));
			}
		}

		// Token: 0x17001873 RID: 6259
		// (get) Token: 0x06008D44 RID: 36164 RVA: 0x00337A00 File Offset: 0x00335C00
		public Color TextColor
		{
			get
			{
				float h;
				float s;
				float b;
				Color.RGBToHSV(this.Color, out h, out s, out b);
				return Color.HSVToRGB(h, s, Mathf.Max(0.5f, b));
			}
		}

		// Token: 0x17001874 RID: 6260
		// (get) Token: 0x06008D45 RID: 36165 RVA: 0x00337A30 File Offset: 0x00335C30
		public List<ThingDef> VeneratedAnimals
		{
			get
			{
				if (this.cachedVeneratedAnimals == null)
				{
					this.cachedVeneratedAnimals = new List<ThingDef>();
					for (int i = 0; i < this.precepts.Count; i++)
					{
						Precept_Animal precept_Animal = this.precepts[i] as Precept_Animal;
						if (precept_Animal != null)
						{
							this.cachedVeneratedAnimals.Add(precept_Animal.ThingDef);
						}
					}
				}
				return this.cachedVeneratedAnimals;
			}
		}

		// Token: 0x17001875 RID: 6261
		// (get) Token: 0x06008D46 RID: 36166 RVA: 0x00337A94 File Offset: 0x00335C94
		public List<XenotypeDef> PreferredXenotypes
		{
			get
			{
				if (this.cachedPreferredXenotypes == null)
				{
					this.cachedPreferredXenotypes = new List<XenotypeDef>();
					for (int i = 0; i < this.precepts.Count; i++)
					{
						Precept_Xenotype precept_Xenotype = this.precepts[i] as Precept_Xenotype;
						if (precept_Xenotype != null && precept_Xenotype.xenotype != null)
						{
							this.cachedPreferredXenotypes.Add(precept_Xenotype.xenotype);
						}
					}
				}
				return this.cachedPreferredXenotypes;
			}
		}

		// Token: 0x17001876 RID: 6262
		// (get) Token: 0x06008D47 RID: 36167 RVA: 0x00337B00 File Offset: 0x00335D00
		public List<CustomXenotype> PreferredCustomXenotypes
		{
			get
			{
				if (this.cachedPreferredCustomXenotypes == null)
				{
					this.cachedPreferredCustomXenotypes = new List<CustomXenotype>();
					for (int i = 0; i < this.precepts.Count; i++)
					{
						Precept_Xenotype precept_Xenotype = this.precepts[i] as Precept_Xenotype;
						if (precept_Xenotype != null && precept_Xenotype.customXenotype != null)
						{
							this.cachedPreferredCustomXenotypes.Add(precept_Xenotype.customXenotype);
						}
					}
				}
				return this.cachedPreferredCustomXenotypes;
			}
		}

		// Token: 0x17001877 RID: 6263
		// (get) Token: 0x06008D48 RID: 36168 RVA: 0x00337B6C File Offset: 0x00335D6C
		public IntRange DeityCountRange
		{
			get
			{
				int num = 0;
				int num2 = int.MaxValue;
				for (int i = 0; i < this.memes.Count; i++)
				{
					if (this.memes[i].deityCount.min >= 0)
					{
						num = Mathf.Max(num, this.memes[i].deityCount.min);
					}
					if (this.memes[i].deityCount.max >= 0)
					{
						num2 = Mathf.Min(num2, this.memes[i].deityCount.max);
					}
				}
				return new IntRange(num, num2);
			}
		}

		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x06008D49 RID: 36169 RVA: 0x00337C0C File Offset: 0x00335E0C
		public MemeDef StructureMeme
		{
			get
			{
				for (int i = 0; i < this.memes.Count; i++)
				{
					if (this.memes[i].category == MemeCategory.Structure)
					{
						return this.memes[i];
					}
				}
				return null;
			}
		}

		// Token: 0x17001879 RID: 6265
		// (get) Token: 0x06008D4A RID: 36170 RVA: 0x00337C51 File Offset: 0x00335E51
		public Gender SupremeGender
		{
			get
			{
				if (this.HasMeme(MemeDefOf.MaleSupremacy))
				{
					return Gender.Male;
				}
				if (this.HasMeme(MemeDefOf.FemaleSupremacy))
				{
					return Gender.Female;
				}
				return Gender.None;
			}
		}

		// Token: 0x1700187A RID: 6266
		// (get) Token: 0x06008D4B RID: 36171 RVA: 0x00337C74 File Offset: 0x00335E74
		public int RequiredScars
		{
			get
			{
				if (this.precepts.NullOrEmpty<Precept>())
				{
					return 0;
				}
				if (this.requiredScarsCached == -1)
				{
					this.requiredScarsCached = 0;
					foreach (Precept precept in this.precepts)
					{
						if (precept.def.requiredScars > this.requiredScarsCached)
						{
							this.requiredScarsCached = precept.def.requiredScars;
						}
					}
				}
				return this.requiredScarsCached;
			}
		}

		// Token: 0x1700187B RID: 6267
		// (get) Token: 0x06008D4C RID: 36172 RVA: 0x00337D0C File Offset: 0x00335F0C
		public float BlindPawnChance
		{
			get
			{
				if (this.precepts.NullOrEmpty<Precept>())
				{
					return 0f;
				}
				float num = 0f;
				int num2 = 0;
				foreach (Precept precept in this.precepts)
				{
					if (precept.def.blindPawnChance >= 0f)
					{
						num += precept.def.blindPawnChance;
						num2++;
					}
				}
				if (num2 <= 0)
				{
					return 0f;
				}
				return num / (float)num2;
			}
		}

		// Token: 0x1700187C RID: 6268
		// (get) Token: 0x06008D4D RID: 36173 RVA: 0x00337DA8 File Offset: 0x00335FA8
		public string KeyDeityName
		{
			get
			{
				IdeoFoundation_Deity ideoFoundation_Deity = this.foundation as IdeoFoundation_Deity;
				if (ideoFoundation_Deity != null && ideoFoundation_Deity.DeitiesListForReading.Any<IdeoFoundation_Deity.Deity>())
				{
					return ideoFoundation_Deity.DeitiesListForReading[0].name;
				}
				return null;
			}
		}

		// Token: 0x1700187D RID: 6269
		// (get) Token: 0x06008D4E RID: 36174 RVA: 0x00337DE4 File Offset: 0x00335FE4
		public bool ObligationsActive
		{
			get
			{
				if (this.colonistBelieverCountCached == -1)
				{
					this.RecacheColonistBelieverCount();
				}
				return this.colonistBelieverCountCached >= Ideo.MinBelieversToEnableObligations || Faction.OfPlayer.ideos.IsPrimary(this);
			}
		}

		// Token: 0x1700187E RID: 6270
		// (get) Token: 0x06008D4F RID: 36175 RVA: 0x00337E18 File Offset: 0x00336018
		public SoundDef SoundOngoingRitual
		{
			get
			{
				foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.thingStyleCategories)
				{
					if (thingStyleCategoryWithPriority.category.soundOngoingRitual != null)
					{
						return thingStyleCategoryWithPriority.category.soundOngoingRitual;
					}
				}
				return SoundDefOf.RitualSustainer_Theist;
			}
		}

		// Token: 0x1700187F RID: 6271
		// (get) Token: 0x06008D50 RID: 36176 RVA: 0x00337E88 File Offset: 0x00336088
		public RitualVisualEffectDef RitualEffect
		{
			get
			{
				foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.thingStyleCategories)
				{
					if (thingStyleCategoryWithPriority.category.ritualVisualEffectDef != null)
					{
						return thingStyleCategoryWithPriority.category.ritualVisualEffectDef;
					}
				}
				return RitualVisualEffectDefOf.Basic;
			}
		}

		// Token: 0x17001880 RID: 6272
		// (get) Token: 0x06008D51 RID: 36177 RVA: 0x00337EF8 File Offset: 0x003360F8
		public ThingDef RitualSeatDef
		{
			get
			{
				foreach (Precept precept in this.PreceptsListForReading)
				{
					Precept_RitualSeat precept_RitualSeat = precept as Precept_RitualSeat;
					if (precept_RitualSeat != null)
					{
						return precept_RitualSeat.ThingDef;
					}
				}
				return null;
			}
		}

		// Token: 0x17001881 RID: 6273
		// (get) Token: 0x06008D52 RID: 36178 RVA: 0x00337F58 File Offset: 0x00336158
		// (set) Token: 0x06008D53 RID: 36179 RVA: 0x00337F94 File Offset: 0x00336194
		public string WorshipRoomLabel
		{
			get
			{
				if (this.overriddenWorshipRoomLabel != null)
				{
					return this.overriddenWorshipRoomLabel;
				}
				MemeDef structureMeme = this.StructureMeme;
				if (structureMeme != null)
				{
					return structureMeme.worshipRoomLabel;
				}
				return RoomRoleDefOf.WorshipRoom.label.CapitalizeFirst();
			}
			set
			{
				this.overriddenWorshipRoomLabel = value;
			}
		}

		// Token: 0x17001882 RID: 6274
		// (get) Token: 0x06008D54 RID: 36180 RVA: 0x00337F9D File Offset: 0x0033619D
		public string MemberNamePlural
		{
			get
			{
				if (this.memberNamePluralCached.NullOrEmpty())
				{
					this.memberNamePluralCached = Find.ActiveLanguageWorker.Pluralize(this.memberName, -1);
				}
				return this.memberNamePluralCached;
			}
		}

		// Token: 0x17001883 RID: 6275
		// (get) Token: 0x06008D55 RID: 36181 RVA: 0x00337FC9 File Offset: 0x003361C9
		// (set) Token: 0x06008D56 RID: 36182 RVA: 0x00337FD1 File Offset: 0x003361D1
		public bool Fluid
		{
			get
			{
				return this.fluid;
			}
			set
			{
				if (value && this.development == null)
				{
					this.development = new IdeoDevelopmentTracker(this);
				}
				this.fluid = value;
			}
		}

		// Token: 0x17001884 RID: 6276
		// (get) Token: 0x06008D57 RID: 36183 RVA: 0x00337FF4 File Offset: 0x003361F4
		public bool LikesHumanLeatherApparel
		{
			get
			{
				using (List<Precept>.Enumerator enumerator = this.precepts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.likesHumanLeatherApparel)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06008D58 RID: 36184 RVA: 0x00338054 File Offset: 0x00336254
		public bool IsWorkTypeConsideredDangerous(WorkTypeDef workType)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.opposedWorkTypes.Count; j++)
				{
					if (this.precepts[i].def.opposedWorkTypes[j] == workType)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008D59 RID: 36185 RVA: 0x003380C8 File Offset: 0x003362C8
		public void SortStyleCategories()
		{
			this.thingStyleCategories.SortBy((ThingStyleCategoryWithPriority x) => -x.priority);
			this.RecachePossibleBuildables();
		}

		// Token: 0x06008D5A RID: 36186 RVA: 0x003380FC File Offset: 0x003362FC
		public Ideo()
		{
			this.style = new IdeoStyleTracker(this);
		}

		// Token: 0x06008D5B RID: 36187 RVA: 0x003381CC File Offset: 0x003363CC
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.overriddenWorshipRoomLabel, "overriddenWorshipRoomLabel", null, false);
			Scribe_Values.Look<bool>(ref this.createdFromNoExpansionGame, "createdFromNoExpansionGame", false, false);
			Scribe_Deep.Look<IdeoFoundation>(ref this.foundation, "foundation", Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<string>(ref this.adjective, "adjective", null, false);
			Scribe_Values.Look<string>(ref this.memberName, "memberName", null, false);
			Scribe_Values.Look<string>(ref this.leaderTitleMale, "leaderTitleMale", null, false);
			Scribe_Values.Look<string>(ref this.leaderTitleFemale, "leaderTitleFemale", null, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
			Scribe_Values.Look<string>(ref this.descriptionTemplate, "descriptionTemplate", null, false);
			Scribe_Values.Look<bool>(ref this.descriptionLocked, "descriptionLocked", false, false);
			Scribe_Values.Look<bool>(ref this.classicExtraMode, "classic", false, false);
			Scribe_Values.Look<bool>(ref this.classicMode, "hiddenIdeoMode", false, false);
			Scribe_Defs.Look<CultureDef>(ref this.culture, "culture");
			Scribe_Defs.Look<IdeoIconDef>(ref this.iconDef, "iconDef");
			Scribe_Defs.Look<ColorDef>(ref this.colorDef, "colorDef");
			Scribe_Values.Look<Color?>(ref this.primaryFactionColor, "primaryFactionColor", null, false);
			Scribe_Collections.Look<MemeDef>(ref this.memes, "memes", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<Precept>(ref this.precepts, "precepts", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<ThingStyleCategoryWithPriority>(ref this.thingStyleCategories, "thingStyleCategories", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.usedSymbols, "usedSymbols", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.usedSymbolPacks, "usedSymbolPacks", LookMode.Value, Array.Empty<object>());
			Scribe_Deep.Look<IdeoStyleTracker>(ref this.style, "style", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.fluid, "fluid", false, false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Values.Look<bool>(ref this.solid, "solid", false, false);
			if (!GameDataSaveLoader.IsSavingOrLoadingExternalIdeo)
			{
				Scribe_Values.Look<int>(ref this.id, "id", 0, false);
				Scribe_Values.Look<bool>(ref this.relicsCollected, "relicsCollected", false, false);
				Scribe_Values.Look<bool>(ref this.initialPlayerIdeo, "initialPlayerIdeo", false, false);
				Scribe_Deep.Look<IdeoDevelopmentTracker>(ref this.development, "development", new object[]
				{
					this
				});
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.foundation != null)
				{
					this.foundation.ideo = this;
				}
				if (this.style == null)
				{
					this.style = new IdeoStyleTracker(this);
				}
				if (this.style.ideo == null)
				{
					this.style.ideo = this;
				}
				if (this.development != null && this.development.ideo == null)
				{
					this.development.ideo = this;
				}
				if (this.fluid && this.development == null)
				{
					this.development = new IdeoDevelopmentTracker(this);
				}
				if (this.memes.RemoveAll((MemeDef x) => x == null) != 0)
				{
					Log.Error("Some ideoligion memes were null after loading.");
				}
				if (this.precepts.RemoveAll(delegate(Precept x)
				{
					if (x != null && x.def != null)
					{
						Precept_Ritual precept_Ritual = x as Precept_Ritual;
						return precept_Ritual != null && precept_Ritual.behavior == null;
					}
					return true;
				}) != 0)
				{
					Log.Error("Some ideoligion precepts were null after loading.");
				}
				if (this.thingStyleCategories == null)
				{
					this.thingStyleCategories = new List<ThingStyleCategoryWithPriority>();
				}
				if (this.thingStyleCategories.RemoveAll((ThingStyleCategoryWithPriority x) => x == null || x.category == null) != 0)
				{
					Log.Error("Some thing style categories were null after loading.");
				}
				if (this.culture == null)
				{
					this.culture = DefDatabase<CultureDef>.AllDefsListForReading.RandomElement<CultureDef>();
					Log.Error("Ideoligion had null culture. Assigning random.");
				}
				if (this.usedSymbols == null)
				{
					this.usedSymbols = new List<string>();
				}
				if (this.usedSymbolPacks == null)
				{
					this.usedSymbolPacks = new List<string>();
				}
				for (int i = 0; i < this.precepts.Count; i++)
				{
					if (this.precepts[i] != null)
					{
						this.precepts[i].ideo = this;
					}
				}
				if (this.description == null)
				{
					this.RegenerateDescription(false);
				}
				if (this.createdFromNoExpansionGame && ModsConfig.IdeologyActive && this.iconDef == null)
				{
					this.SetIcon(IdeoFoundation.GetRandomIconDef(this), IdeoFoundation.GetRandomColorDef(this), false);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.memberName.NullOrEmpty())
				{
					this.memberName = FactionDefOf.PlayerColony.basicMemberKind.label;
				}
				if (ModsConfig.IdeologyActive && !this.classicMode && this.RitualSeatDef == null)
				{
					PreceptDef def;
					if ((from x in DefDatabase<PreceptDef>.AllDefs
					where x.issue == IssueDefOf.IdeoRitualSeat
					select x).TryRandomElementByWeight((PreceptDef x) => x.selectionWeight, out def))
					{
						this.AddPrecept(PreceptMaker.MakePrecept(def), true, null, null);
						Log.Warning("Adding missing ritual seat precept " + this.RitualSeatDef.LabelCap);
					}
				}
				using (List<PreceptDef>.Enumerator enumerator = DefDatabase<PreceptDef>.AllDefsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PreceptDef p = enumerator.Current;
						if ((p.takeNameFrom == null || this.precepts.Any((Precept _p) => _p.def == p.takeNameFrom)) && p.preceptClass.SameOrSubclassOf<Precept_Ritual>() && !p.visible && !this.precepts.Any((Precept o) => o.def.ritualPatternBase == p.ritualPatternBase) && (!Find.IdeoManager.classicMode || p.classic))
						{
							RitualPatternDef ritualPatternBase = p.ritualPatternBase;
							Precept precept = PreceptMaker.MakePrecept(p);
							this.AddPrecept(precept, true, null, ritualPatternBase);
							Log.Warning("A hidden ritual precept was missing, adding: " + precept.def.LabelCap);
						}
					}
				}
				List<Precept> list = (from x in this.precepts
				where x.def == PreceptDefOf.FuneralNoCorpse
				select x).ToList<Precept>();
				if (list.Count > 1)
				{
					Log.Warning("Multiple FuneralNoCorpse precepts found, removing extras.");
					for (int j = 1; j < list.Count; j++)
					{
						this.precepts.Remove(list[j]);
					}
				}
				if (this.classicMode && this.createdFromNoExpansionGame)
				{
					this.classicMode = false;
				}
				this.RecachePrecepts();
			}
		}

		// Token: 0x06008D5C RID: 36188 RVA: 0x0033888C File Offset: 0x00336A8C
		public bool HasMeme(MemeDef meme)
		{
			return this.memes.Contains(meme);
		}

		// Token: 0x06008D5D RID: 36189 RVA: 0x0033889A File Offset: 0x00336A9A
		public bool EnablesNeed(NeedDef def)
		{
			return this.cachedEnabledNeeds.Contains(def);
		}

		// Token: 0x06008D5E RID: 36190 RVA: 0x003388A8 File Offset: 0x00336AA8
		public bool DisablesNeed(NeedDef def)
		{
			return this.cachedDisabledNeeds.Contains(def);
		}

		// Token: 0x06008D5F RID: 36191 RVA: 0x003388B8 File Offset: 0x00336AB8
		public void IdeoTick()
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				this.precepts[i].Tick();
			}
			if (this.colonistBelieverCountCached == -1)
			{
				this.RecacheColonistBelieverCount();
			}
		}

		// Token: 0x06008D60 RID: 36192 RVA: 0x003388FC File Offset: 0x00336AFC
		public void Notify_AddBedThoughts(Pawn pawn)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					this.precepts[i].def.comps[j].Notify_AddBedThoughts(pawn, this.precepts[i]);
				}
			}
		}

		// Token: 0x06008D61 RID: 36193 RVA: 0x00338974 File Offset: 0x00336B74
		public void Notify_MemberTookAction(HistoryEvent ev, bool canApplySelfTookThoughts)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					this.precepts[i].def.comps[j].Notify_MemberTookAction(ev, this.precepts[i], canApplySelfTookThoughts);
				}
			}
		}

		// Token: 0x06008D62 RID: 36194 RVA: 0x003389EC File Offset: 0x00336BEC
		public void Notify_MemberKnows(HistoryEvent ev, Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					this.precepts[i].def.comps[j].Notify_MemberWitnessedAction(ev, this.precepts[i], member);
				}
			}
		}

		// Token: 0x06008D63 RID: 36195 RVA: 0x00338A64 File Offset: 0x00336C64
		public void Notify_MemberGuestStatusChanged(Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberGuestStatusChanged(member);
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_MemberGuestStatusChanged(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x06008D64 RID: 36196 RVA: 0x00338ACC File Offset: 0x00336CCC
		public void Notify_MemberDied(Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberDied(member);
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_MemberDied(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			if (Faction.OfPlayerSilentFail != null && member.Faction == Faction.OfPlayerSilentFail)
			{
				this.RecacheColonistBelieverCount();
			}
		}

		// Token: 0x06008D65 RID: 36197 RVA: 0x00338B50 File Offset: 0x00336D50
		public void Notify_MemberLost(Pawn member, Map map)
		{
			if (member.IsColonist && map != null && member.workSettings != null && member.workSettings.WorkIsActive(WorkTypeDefOf.Warden))
			{
				bool flag = false;
				foreach (Pawn pawn in map.mapPawns.FreeColonists)
				{
					if (pawn != member && pawn.Ideo == this && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Warden))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					foreach (Pawn pawn2 in map.mapPawns.PrisonersOfColonySpawned)
					{
						pawn2.guest.Notify_WardensOfIdeoLost(this);
					}
				}
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberLost(member);
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_MemberLost(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x06008D66 RID: 36198 RVA: 0x00338C9C File Offset: 0x00336E9C
		public void Notify_MemberGained(Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberGained(member);
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_MemberGained(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x06008D67 RID: 36199 RVA: 0x00338D04 File Offset: 0x00336F04
		public void Notify_MemberCorpseDestroyed(Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberCorpseDestroyed(member);
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_MemberCorpseDestroyed(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x06008D68 RID: 36200 RVA: 0x00338D6C File Offset: 0x00336F6C
		public void Notify_MemberGenerated(Pawn member, bool newborn, bool ignoreApparel = false)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberGenerated(member, newborn, ignoreApparel);
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_MemberGenerated(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x06008D69 RID: 36201 RVA: 0x00338DD8 File Offset: 0x00336FD8
		public void Notify_GameStarted()
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_GameStarted();
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_GameStarted(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x06008D6A RID: 36202 RVA: 0x00338E40 File Offset: 0x00337040
		public void Notify_RelicSeenByPlayer(Thing relic)
		{
			if (this.AllRelicsNewlyCollected())
			{
				Ideo.tmpPlayerIdeoFollowers.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsAndWorld_Alive)
				{
					if (pawn.Ideo == this && pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.thoughts != null && pawn.needs.mood.thoughts.memories != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RelicsCollected, null, null);
						if (pawn.Faction == Faction.OfPlayer)
						{
							Ideo.tmpPlayerIdeoFollowers.Add(pawn);
						}
					}
				}
				if (Ideo.tmpPlayerIdeoFollowers.Count > 0)
				{
					TaggedString t = this.classicMode ? "LetterTextRelicsCollectedClassic".Translate() : "LetterTextRelicsCollected".Translate(this);
					ChoiceLetter let = LetterMaker.MakeLetter("LetterLabelRelicsCollected".Translate() + ": " + this.name.ApplyTag(this).Resolve(), t + ":\n\n" + (from p in Ideo.tmpPlayerIdeoFollowers
					select p.LabelNoCountColored.Resolve()).ToList<string>().ToLineList("- "), LetterDefOf.PositiveEvent, Ideo.tmpPlayerIdeoFollowers, null, null, null);
					Find.LetterStack.ReceiveLetter(let, null, 0, true);
					Ideo.tmpPlayerIdeoFollowers.Clear();
				}
				this.relicsCollected = true;
			}
		}

		// Token: 0x06008D6B RID: 36203 RVA: 0x00339008 File Offset: 0x00337208
		public void Notify_MemberGainedByConversion()
		{
			if (this.Fluid)
			{
				this.development.Notify_MemberGainedByConversion();
			}
		}

		// Token: 0x06008D6C RID: 36204 RVA: 0x00339020 File Offset: 0x00337220
		private bool AllRelicsNewlyCollected()
		{
			if (this.relicsCollected)
			{
				return false;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept_Relic precept_Relic = this.precepts[i] as Precept_Relic;
				if (precept_Relic != null && !precept_Relic.RelicInPlayerPossession)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008D6D RID: 36205 RVA: 0x00339070 File Offset: 0x00337270
		public bool MemberWillingToDo(HistoryEvent ev)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					if (!this.precepts[i].def.comps[j].MemberWillingToDo(ev))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06008D6E RID: 36206 RVA: 0x003390E0 File Offset: 0x003372E0
		public Pair<Precept, Precept> FirstIncompatiblePreceptPair()
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts.Count; j++)
				{
					if (this.precepts[i] != this.precepts[j] && !this.precepts[i].CompatibleWith(this.precepts[j]))
					{
						return new Pair<Precept, Precept>(this.precepts[i], this.precepts[j]);
					}
				}
			}
			return default(Pair<Precept, Precept>);
		}

		// Token: 0x06008D6F RID: 36207 RVA: 0x0033917C File Offset: 0x0033737C
		public Precept FirstPreceptWithWarning()
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				string text;
				string text2;
				if (this.precepts[i].GetPlayerWarning(out text, out text2))
				{
					return this.precepts[i];
				}
			}
			return null;
		}

		// Token: 0x06008D70 RID: 36208 RVA: 0x003391C4 File Offset: 0x003373C4
		public Tuple<Precept_Ritual, List<string>> FirstRitualMissingTarget()
		{
			foreach (Precept precept in this.precepts)
			{
				Precept_Ritual precept_Ritual = precept as Precept_Ritual;
				if (precept_Ritual != null && precept_Ritual.obligationTargetFilter != null)
				{
					List<string> list = precept_Ritual.obligationTargetFilter.MissingTargetBuilding(this);
					if (!list.NullOrEmpty<string>())
					{
						return new Tuple<Precept_Ritual, List<string>>(precept_Ritual, list);
					}
				}
			}
			return null;
		}

		// Token: 0x06008D71 RID: 36209 RVA: 0x00339244 File Offset: 0x00337444
		public Precept_Building FirstConsumableBuildingMissingRitual()
		{
			foreach (Precept precept in this.precepts)
			{
				Precept_Building precept_Building = precept as Precept_Building;
				if (precept_Building != null && precept_Building.ThingDef.ritualFocus != null && precept_Building.ThingDef.ritualFocus.consumable && !this.HasRequiredRitualForBuilding(precept_Building))
				{
					return precept_Building;
				}
			}
			return null;
		}

		// Token: 0x06008D72 RID: 36210 RVA: 0x003392C8 File Offset: 0x003374C8
		private bool HasRequiredRitualForBuilding(Precept_Building building)
		{
			if (this.cachedRitualTargetFilters == null)
			{
				this.cachedRitualTargetFilters = new List<RitualObligationTargetFilter>();
				foreach (Precept precept in this.precepts)
				{
					Precept_Ritual precept_Ritual = precept as Precept_Ritual;
					if (precept_Ritual != null && precept_Ritual.obligationTargetFilter != null)
					{
						this.cachedRitualTargetFilters.Add(precept_Ritual.obligationTargetFilter.def.GetInstance());
					}
				}
			}
			foreach (RitualObligationTargetFilter ritualObligationTargetFilter in this.cachedRitualTargetFilters)
			{
				if (!ritualObligationTargetFilter.def.thingDefs.NullOrEmpty<ThingDef>() && ritualObligationTargetFilter.def.thingDefs.Contains(building.ThingDef))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008D73 RID: 36211 RVA: 0x003393C0 File Offset: 0x003375C0
		public bool IsPreferredXenotype(Pawn pawn)
		{
			if (!this.PreferredXenotypes.Any<XenotypeDef>() && !this.PreferredCustomXenotypes.Any<CustomXenotype>())
			{
				return false;
			}
			if (pawn.genes == null)
			{
				return false;
			}
			if (pawn.genes.CustomXenotype != null)
			{
				foreach (CustomXenotype custom in this.PreferredCustomXenotypes)
				{
					if (GeneUtility.PawnIsCustomXenotype(pawn, custom))
					{
						return true;
					}
				}
				return false;
			}
			return this.PreferredXenotypes.Contains(pawn.genes.Xenotype);
		}

		// Token: 0x06008D74 RID: 36212 RVA: 0x00339468 File Offset: 0x00337668
		public void ClearPrecepts()
		{
			this.precepts.Clear();
			this.RecachePrecepts();
			this.anyPreceptEdited = false;
		}

		// Token: 0x06008D75 RID: 36213 RVA: 0x00339484 File Offset: 0x00337684
		public void RecachePrecepts()
		{
			this.allPreceptDefs.Clear();
			foreach (Precept precept in this.precepts)
			{
				this.allPreceptDefs.Add(precept.def);
			}
			this.requiredScarsCached = -1;
			this.cachedVeneratedAnimals = null;
			this.cachedRitualTargetFilters = null;
			this.cachedPreferredXenotypes = null;
			this.cachedPreferredCustomXenotypes = null;
			this.RecachePossibleSituationalThoughts();
			this.RecachePossibleGoodwillSituations();
			this.RecachePossibleBuildings();
			this.RecachePossibleBuildables();
			this.RecachePossibleRoles();
			this.RecachePossibleMentalBreaks();
			this.RecacheNeeds();
			this.warnPlayerOnDesignateChopTree = false;
			this.warnPlayerOnDesignateMine = false;
			foreach (Precept precept2 in this.precepts)
			{
				if (precept2.def.warnPlayerOnDesignateChopTree)
				{
					this.warnPlayerOnDesignateChopTree = true;
				}
				if (precept2.def.warnPlayerOnDesignateMine)
				{
					this.warnPlayerOnDesignateMine = true;
				}
			}
			foreach (Precept precept3 in this.precepts)
			{
				precept3.Notify_RecachedPrecepts();
			}
			this.currentCacheId++;
		}

		// Token: 0x06008D76 RID: 36214 RVA: 0x003395F8 File Offset: 0x003377F8
		public void AddPrecept(Precept precept, bool init = false, FactionDef generatingFor = null, RitualPatternDef fillWith = null)
		{
			if (precept == null)
			{
				Log.Error("Tried to add a null PreceptDef.");
				return;
			}
			if (this.precepts.Contains(precept))
			{
				Log.Error("Tried to add the same PreceptDef twice.");
				return;
			}
			precept.ideo = this;
			this.precepts.Add(precept);
			this.precepts.SortBy((Precept x) => -this.GetPreceptImpact(x), (Precept x) => -x.def.displayOrderInImpact, (Precept x) => x.def.defName);
			if (init)
			{
				precept.Init(this, generatingFor);
				if (fillWith != null)
				{
					Precept_Ritual precept_Ritual = precept as Precept_Ritual;
					if (precept_Ritual != null)
					{
						fillWith.Fill(precept_Ritual);
						precept_Ritual.RegenerateName();
					}
				}
			}
			this.RecachePrecepts();
			if (precept.def.alsoAdds != null)
			{
				Precept precept2 = this.precepts.FirstOrDefault((Precept p) => p.def == precept.def.alsoAdds);
				if (precept2 != null)
				{
					precept2.RegenerateName();
					return;
				}
				Precept precept3 = PreceptMaker.MakePrecept(precept.def.alsoAdds);
				this.AddPrecept(precept3, true, generatingFor, (fillWith != null) ? precept3.def.ritualPatternBase : null);
			}
		}

		// Token: 0x06008D77 RID: 36215 RVA: 0x00339758 File Offset: 0x00337958
		private float GetPreceptImpact(Precept precept)
		{
			if (!precept.SortByImpact)
			{
				return 0f;
			}
			return (float)precept.def.impact + 1f;
		}

		// Token: 0x06008D78 RID: 36216 RVA: 0x0033977A File Offset: 0x0033797A
		public bool PreceptIsRequired(PreceptDef precept)
		{
			return this.GetMemeThatRequiresPrecept(precept) != null;
		}

		// Token: 0x06008D79 RID: 36217 RVA: 0x00339788 File Offset: 0x00337988
		public MemeDef GetMemeThatRequiresPrecept(PreceptDef precept)
		{
			Predicate<List<PreceptDef>> <>9__0;
			for (int i = 0; i < this.memes.Count; i++)
			{
				if (this.memes[i].requireOne != null)
				{
					List<List<PreceptDef>> requireOne = this.memes[i].requireOne;
					Predicate<List<PreceptDef>> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((List<PreceptDef> x) => x.Contains(precept)));
					}
					if (requireOne.Any(predicate))
					{
						return this.memes[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06008D7A RID: 36218 RVA: 0x00339810 File Offset: 0x00337A10
		public AcceptanceReport CanAddPreceptAllFactions(PreceptDef preceptDef)
		{
			AcceptanceReport result = this.foundation.CanAdd(preceptDef, false);
			if (!result.Accepted)
			{
				return result;
			}
			if (Find.World != null)
			{
				foreach (Faction faction in Find.FactionManager.AllFactions)
				{
					if (faction.def.humanlikeFaction && faction.ideos != null && (faction.ideos.IsPrimary(this) || faction.ideos.IsMinor(this)) && !this.foundation.CanAddForFaction(preceptDef, faction.def, null, false, false, false, false))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06008D7B RID: 36219 RVA: 0x003398D8 File Offset: 0x00337AD8
		public void RemovePrecept(Precept precept, bool replacing = false)
		{
			this.precepts.Remove(precept);
			for (int i = this.precepts.Count - 1; i >= 0; i--)
			{
				if (this.precepts[i].def.takeNameFrom == precept.def)
				{
					this.RemovePrecept(this.precepts[i], false);
				}
			}
			Precept_Role precept_Role = precept as Precept_Role;
			if (precept_Role != null)
			{
				foreach (Pawn pawn in precept_Role.ChosenPawns())
				{
					Pawn_AbilityTracker abilities = pawn.abilities;
					if (abilities != null)
					{
						abilities.Notify_TemporaryAbilitiesChanged();
					}
				}
			}
			if (!replacing && precept.def.defaultSelectionWeight <= 0f && precept.def.issue.HasDefaultPrecept)
			{
				PreceptDef def;
				if ((from x in DefDatabase<PreceptDef>.AllDefs
				where x.issue == precept.def.issue && this.CanAddPreceptAllFactions(x).Accepted && x.defaultSelectionWeight > 0f
				select x).TryRandomElementByWeight((PreceptDef x) => x.defaultSelectionWeight, out def))
				{
					this.AddPrecept(PreceptMaker.MakePrecept(def), true, null, null);
					return;
				}
			}
			this.RecachePrecepts();
		}

		// Token: 0x06008D7C RID: 36220 RVA: 0x00339A3C File Offset: 0x00337C3C
		public void SetIcon(IdeoIconDef iconDef, ColorDef colorDef, bool clearPrimaryFactionColor = false)
		{
			this.iconDef = iconDef;
			this.colorDef = colorDef;
			this.icon = null;
			if (clearPrimaryFactionColor)
			{
				this.primaryFactionColor = null;
			}
		}

		// Token: 0x06008D7D RID: 36221 RVA: 0x00339A62 File Offset: 0x00337C62
		public void DrawIcon(Rect rect)
		{
			Color color = GUI.color;
			GUI.color = this.Color;
			GUI.DrawTexture(rect, this.Icon);
			GUI.color = color;
		}

		// Token: 0x06008D7E RID: 36222 RVA: 0x00339A88 File Offset: 0x00337C88
		public List<Precept> GetAllPreceptsAllowingSituationalThought(ThoughtDef def)
		{
			Ideo.tmpThoughtPrecepts.Clear();
			if (!this.cachedPossibleSituationalThoughts.Contains(def))
			{
				return Ideo.tmpThoughtPrecepts;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				List<PreceptComp> comps = this.precepts[i].def.comps;
				for (int j = 0; j < comps.Count; j++)
				{
					PreceptComp_SituationalThought preceptComp_SituationalThought = comps[j] as PreceptComp_SituationalThought;
					if (preceptComp_SituationalThought != null && preceptComp_SituationalThought.thought == def)
					{
						Ideo.tmpThoughtPrecepts.Add(this.precepts[i]);
						break;
					}
				}
			}
			return Ideo.tmpThoughtPrecepts;
		}

		// Token: 0x06008D7F RID: 36223 RVA: 0x00339B28 File Offset: 0x00337D28
		public Precept GetFirstPreceptAllowingSituationalThought(ThoughtDef def)
		{
			if (!this.cachedPossibleSituationalThoughts.Contains(def))
			{
				return null;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				List<PreceptComp> comps = this.precepts[i].def.comps;
				for (int j = 0; j < comps.Count; j++)
				{
					PreceptComp_SituationalThought preceptComp_SituationalThought = comps[j] as PreceptComp_SituationalThought;
					if (preceptComp_SituationalThought != null && preceptComp_SituationalThought.thought == def)
					{
						return this.precepts[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06008D80 RID: 36224 RVA: 0x00339BAC File Offset: 0x00337DAC
		public bool HasPreceptForBuilding(ThingDef buildingDef)
		{
			return this.cachedPossibleBuildings.Any((Precept_Building b) => b.ThingDef == buildingDef);
		}

		// Token: 0x06008D81 RID: 36225 RVA: 0x00339BE0 File Offset: 0x00337DE0
		public bool MembersCanBuild(Thing thing)
		{
			if (this.classicMode)
			{
				return true;
			}
			BuildableDef buildableDef = thing.def.entityDefToBuild ?? thing.def;
			if (buildableDef.canGenerateDefaultDesignator)
			{
				return true;
			}
			Precept_ThingStyle styleSourcePrecept = thing.StyleSourcePrecept;
			if (((styleSourcePrecept != null) ? styleSourcePrecept.ideo : null) == this || this.cachedPossibleBuildables.Contains(buildableDef))
			{
				return true;
			}
			ThingDef thingDef = buildableDef as ThingDef;
			if (thingDef != null && this.HasPreceptForBuilding(thingDef))
			{
				return true;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				if (this.precepts[i].def.willingToConstructOtherIdeoBuildables)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008D82 RID: 36226 RVA: 0x00339C82 File Offset: 0x00337E82
		public bool IsVeneratedAnimal(Pawn pawn)
		{
			return this.IsVeneratedAnimal(pawn.def);
		}

		// Token: 0x06008D83 RID: 36227 RVA: 0x00339C90 File Offset: 0x00337E90
		public bool IsVeneratedAnimal(ThingDef thingDef)
		{
			return ModsConfig.IdeologyActive && this.VeneratedAnimals.Contains(thingDef);
		}

		// Token: 0x06008D84 RID: 36228 RVA: 0x00339CA8 File Offset: 0x00337EA8
		public IdeoWeaponDisposition GetDispositionForWeapon(ThingDef td)
		{
			if (!td.IsWeapon || td.weaponClasses.NullOrEmpty<WeaponClassDef>())
			{
				return IdeoWeaponDisposition.None;
			}
			foreach (Precept precept in this.precepts)
			{
				Precept_Weapon precept_Weapon = precept as Precept_Weapon;
				if (precept_Weapon != null)
				{
					IdeoWeaponDisposition dispositionForWeapon = precept_Weapon.GetDispositionForWeapon(td);
					if (dispositionForWeapon != IdeoWeaponDisposition.None)
					{
						return dispositionForWeapon;
					}
				}
			}
			return IdeoWeaponDisposition.None;
		}

		// Token: 0x06008D85 RID: 36229 RVA: 0x00339D28 File Offset: 0x00337F28
		public T GetFirstPreceptOfType<T>() where T : class
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				T t = this.precepts[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06008D86 RID: 36230 RVA: 0x00339D75 File Offset: 0x00337F75
		public IEnumerable<T> GetAllPreceptsOfType<T>() where T : class
		{
			Ideo.<GetAllPreceptsOfType>d__152<T> <GetAllPreceptsOfType>d__ = new Ideo.<GetAllPreceptsOfType>d__152<T>(-2);
			<GetAllPreceptsOfType>d__.<>4__this = this;
			return <GetAllPreceptsOfType>d__;
		}

		// Token: 0x06008D87 RID: 36231 RVA: 0x00339D85 File Offset: 0x00337F85
		public bool HasPrecept(PreceptDef preceptDef)
		{
			return this.allPreceptDefs.Contains(preceptDef);
		}

		// Token: 0x06008D88 RID: 36232 RVA: 0x00339D94 File Offset: 0x00337F94
		public int GetPreceptCountOfDef(PreceptDef preceptDef)
		{
			if (preceptDef == null || !this.allPreceptDefs.Contains(preceptDef))
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.precepts.Count; i++)
			{
				if (this.precepts[i].def == preceptDef)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06008D89 RID: 36233 RVA: 0x00339DE8 File Offset: 0x00337FE8
		public Precept GetPrecept(PreceptDef preceptDef)
		{
			if (preceptDef == null || !this.allPreceptDefs.Contains(preceptDef))
			{
				return null;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				if (this.precepts[i].def == preceptDef)
				{
					return this.precepts[i];
				}
			}
			return null;
		}

		// Token: 0x06008D8A RID: 36234 RVA: 0x00339E40 File Offset: 0x00338040
		public void SortMemesInDisplayOrder()
		{
			this.memes.SortBy((MemeDef x) => x.category == MemeCategory.Normal, (MemeDef x) => x.index);
		}

		// Token: 0x06008D8B RID: 36235 RVA: 0x00339E98 File Offset: 0x00338098
		private void RecachePossibleSituationalThoughts()
		{
			this.cachedPossibleSituationalThoughts.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					PreceptComp_SituationalThought preceptComp_SituationalThought = this.precepts[i].def.comps[j] as PreceptComp_SituationalThought;
					if (preceptComp_SituationalThought != null && !this.cachedPossibleSituationalThoughts.Contains(preceptComp_SituationalThought.thought))
					{
						this.cachedPossibleSituationalThoughts.Add(preceptComp_SituationalThought.thought);
					}
				}
			}
		}

		// Token: 0x06008D8C RID: 36236 RVA: 0x00339F3C File Offset: 0x0033813C
		private void RecachePossibleGoodwillSituations()
		{
			this.cachedPossibleGoodwillSituations.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					PreceptComp_GoodwillSituation preceptComp_GoodwillSituation = this.precepts[i].def.comps[j] as PreceptComp_GoodwillSituation;
					if (preceptComp_GoodwillSituation != null && !this.cachedPossibleGoodwillSituations.Contains(preceptComp_GoodwillSituation.goodwillSituation))
					{
						this.cachedPossibleGoodwillSituations.Add(preceptComp_GoodwillSituation.goodwillSituation);
					}
				}
			}
		}

		// Token: 0x06008D8D RID: 36237 RVA: 0x00339FDC File Offset: 0x003381DC
		public void RecachePossibleBuildings()
		{
			this.cachedPossibleBuildings.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept_Building precept_Building = this.precepts[i] as Precept_Building;
				if (precept_Building != null)
				{
					this.cachedPossibleBuildings.Add(precept_Building);
				}
			}
		}

		// Token: 0x06008D8E RID: 36238 RVA: 0x0033A02C File Offset: 0x0033822C
		public void RecachePossibleBuildables()
		{
			this.cachedPossibleBuildables.Clear();
			foreach (MemeDef memeDef in this.memes)
			{
				this.cachedPossibleBuildables.AddRange(memeDef.AllDesignatorBuildables);
			}
			foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.thingStyleCategories)
			{
				this.cachedPossibleBuildables.AddRange(thingStyleCategoryWithPriority.category.AllDesignatorBuildables);
			}
			foreach (Precept precept in this.precepts)
			{
				Precept_RitualSeat precept_RitualSeat = precept as Precept_RitualSeat;
				if (precept_RitualSeat != null)
				{
					this.cachedPossibleBuildables.Add(precept_RitualSeat.ThingDef);
				}
			}
		}

		// Token: 0x06008D8F RID: 36239 RVA: 0x0033A140 File Offset: 0x00338340
		public void RecachePossibleMentalBreaks()
		{
			this.cachedPossibleMentalBreaks.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					PreceptComp_MentalBreak preceptComp_MentalBreak = this.precepts[i].def.comps[j] as PreceptComp_MentalBreak;
					if (preceptComp_MentalBreak != null)
					{
						this.cachedPossibleMentalBreaks.Add(preceptComp_MentalBreak.mentalBreakDef);
					}
				}
			}
		}

		// Token: 0x06008D90 RID: 36240 RVA: 0x0033A1CC File Offset: 0x003383CC
		public void RecachePossibleRoles()
		{
			this.cachedPossibleRoles.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept_Role precept_Role = this.precepts[i] as Precept_Role;
				if (precept_Role != null)
				{
					this.cachedPossibleRoles.Add(precept_Role);
				}
			}
		}

		// Token: 0x06008D91 RID: 36241 RVA: 0x0033A21C File Offset: 0x0033841C
		public void RecacheNeeds()
		{
			this.cachedEnabledNeeds.Clear();
			this.cachedDisabledNeeds.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept precept = this.precepts[i];
				if (!precept.def.enablesNeeds.NullOrEmpty<NeedDef>())
				{
					this.cachedEnabledNeeds.AddRange(precept.def.enablesNeeds);
				}
				if (!precept.def.disablesNeeds.NullOrEmpty<NeedDef>())
				{
					this.cachedDisabledNeeds.AddRange(precept.def.disablesNeeds);
				}
			}
		}

		// Token: 0x06008D92 RID: 36242 RVA: 0x0033A2B4 File Offset: 0x003384B4
		public void RegenerateAllPreceptNames()
		{
			foreach (Precept precept in this.precepts)
			{
				if (precept.UsesGeneratedName && !precept.nameLocked)
				{
					precept.RegenerateName();
				}
				precept.ClearTipCache();
			}
		}

		// Token: 0x06008D93 RID: 36243 RVA: 0x0033A31C File Offset: 0x0033851C
		public void RegenerateDescription(bool force = false)
		{
			if (this.description == null || !this.descriptionLocked)
			{
				IdeoDescriptionResult newDescription = this.GetNewDescription(force);
				this.description = newDescription.text;
				this.descriptionTemplate = newDescription.template;
			}
		}

		// Token: 0x06008D94 RID: 36244 RVA: 0x0033A35C File Offset: 0x0033855C
		public IdeoDescriptionResult GetNewDescription(bool force = false)
		{
			List<IdeoDescriptionMaker.PatternEntry> list = (from entry in this.memes.Where(delegate(MemeDef meme)
			{
				IdeoDescriptionMaker descriptionMaker = meme.descriptionMaker;
				return ((descriptionMaker != null) ? descriptionMaker.patterns : null) != null;
			}).SelectMany((MemeDef meme) => meme.descriptionMaker.patterns)
			group entry by entry.def into grp
			select grp.MaxBy((IdeoDescriptionMaker.PatternEntry entry) => entry.weight)).ToList<IdeoDescriptionMaker.PatternEntry>();
			if (!list.Any<IdeoDescriptionMaker.PatternEntry>())
			{
				if (ModsConfig.IdeologyActive && this.memes.Any<MemeDef>())
				{
					Log.Error("Memes provided no description patterns");
				}
				return new IdeoDescriptionResult
				{
					text = string.Empty
				};
			}
			IdeoStoryPatternDef def = list.RandomElementByWeight((IdeoDescriptionMaker.PatternEntry entry) => entry.weight).def;
			return IdeoDescriptionUtility.ResolveDescription(this, def, force);
		}

		// Token: 0x17001885 RID: 6277
		// (get) Token: 0x06008D95 RID: 36245 RVA: 0x0033A46F File Offset: 0x0033866F
		public int ColonistBelieverCountCached
		{
			get
			{
				if (this.colonistBelieverCountCached == -1)
				{
					return this.RecacheColonistBelieverCount();
				}
				return this.colonistBelieverCountCached;
			}
		}

		// Token: 0x06008D96 RID: 36246 RVA: 0x0033A488 File Offset: 0x00338688
		public int RecacheColonistBelieverCount()
		{
			if (Current.ProgramState != ProgramState.Playing || Find.WindowStack.IsOpen<Dialog_ConfigureIdeo>())
			{
				return 0;
			}
			int num = this.colonistBelieverCountCached;
			int num2 = 0;
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep)
			{
				if (pawn.Ideo == this && !pawn.IsSlave && !pawn.IsQuestLodger())
				{
					num2++;
				}
			}
			this.colonistBelieverCountCached = num2;
			foreach (Precept precept in this.precepts)
			{
				Precept_Role precept_Role = precept as Precept_Role;
				if (precept_Role != null)
				{
					precept_Role.RecacheActivity();
				}
			}
			if (ModsConfig.IdeologyActive && Faction.OfPlayer.ideos.IsMinor(this))
			{
				if (this.colonistBelieverCountCached < Ideo.MinBelieversToEnableObligations && num != -1 && num >= Ideo.MinBelieversToEnableObligations)
				{
					Find.LetterStack.ReceiveLetter("LetterTitleObligationsDeactivated".Translate(this.name), "LetterTitleObligationsDeactivatedTooFewBelievers".Translate(this.Named("IDEO"), Ideo.MinBelieversToEnableObligations), LetterDefOf.NeutralEvent, null, 0, true);
				}
				if (this.colonistBelieverCountCached >= Ideo.MinBelieversToEnableObligations && num != -1 && num < Ideo.MinBelieversToEnableObligations)
				{
					Find.LetterStack.ReceiveLetter("LetterTitleObligationsActivated".Translate(this.name), "LetterTitleObligationsActivatedEnoughBelievers".Translate(this.Named("IDEO"), Ideo.MinBelieversToEnableObligations), LetterDefOf.NeutralEvent, null, 0, true);
				}
			}
			return num2;
		}

		// Token: 0x06008D97 RID: 36247 RVA: 0x0033A644 File Offset: 0x00338844
		public Precept_Role GetRole(Pawn p)
		{
			foreach (Precept_Role precept_Role in this.RolesListForReading)
			{
				if (precept_Role.IsAssigned(p))
				{
					return precept_Role;
				}
			}
			return null;
		}

		// Token: 0x06008D98 RID: 36248 RVA: 0x0033A6A0 File Offset: 0x003388A0
		public StyleCategoryPair GetStyleAndCategoryFor(ThingDef thingDef)
		{
			Precept precept = null;
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept_ThingDef precept_ThingDef = this.precepts[i] as Precept_ThingDef;
				if (precept_ThingDef != null && precept_ThingDef.ThingDef == thingDef)
				{
					precept = precept_ThingDef;
					break;
				}
			}
			return this.style.StyleForThingDef(thingDef, precept);
		}

		// Token: 0x06008D99 RID: 36249 RVA: 0x0033A6F4 File Offset: 0x003388F4
		public ThingStyleDef GetStyleFor(ThingDef thingDef)
		{
			StyleCategoryPair styleAndCategoryFor = this.GetStyleAndCategoryFor(thingDef);
			if (styleAndCategoryFor == null)
			{
				return null;
			}
			return styleAndCategoryFor.styleDef;
		}

		// Token: 0x06008D9A RID: 36250 RVA: 0x0033A708 File Offset: 0x00338908
		public StyleCategoryDef GetStyleCategoryFor(ThingDef thingDef)
		{
			StyleCategoryPair styleCategoryPair = this.style.StyleForThingDef(thingDef, null);
			if (styleCategoryPair == null)
			{
				return null;
			}
			return styleCategoryPair.category;
		}

		// Token: 0x06008D9B RID: 36251 RVA: 0x0033A724 File Offset: 0x00338924
		public void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
			foreach (Precept precept in this.PreceptsListForReading)
			{
				precept.Notify_MemberChangedFaction(p, oldFaction, newFaction);
			}
			if (oldFaction == Faction.OfPlayer || newFaction == Faction.OfPlayer)
			{
				this.RecacheColonistBelieverCount();
			}
		}

		// Token: 0x06008D9C RID: 36252 RVA: 0x0033A790 File Offset: 0x00338990
		public void Notify_NotPrimaryAnymore(Ideo newIdeo)
		{
			foreach (Precept precept in this.precepts)
			{
				precept.Notify_IdeoNotPrimaryAnymore(newIdeo);
			}
		}

		// Token: 0x06008D9D RID: 36253 RVA: 0x0033A7E4 File Offset: 0x003389E4
		public bool HasMaxPreceptsForIssue(IssueDef issue)
		{
			if (!issue.allowMultiplePrecepts)
			{
				for (int i = 0; i < this.precepts.Count; i++)
				{
					if (this.precepts[i].def.issue == issue)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008D9E RID: 36254 RVA: 0x0033A82B File Offset: 0x00338A2B
		public void MakeMemeberNamePluralDirty()
		{
			this.memberNamePluralCached = null;
		}

		// Token: 0x06008D9F RID: 36255 RVA: 0x0033A834 File Offset: 0x00338A34
		public string GetUniqueLoadID()
		{
			return "Ideo_" + this.id.ToString();
		}

		// Token: 0x06008DA0 RID: 36256 RVA: 0x0033A84B File Offset: 0x00338A4B
		public override string ToString()
		{
			if (this.name != null)
			{
				return this.name;
			}
			return this.GetUniqueLoadID();
		}

		// Token: 0x06008DA1 RID: 36257 RVA: 0x0033A864 File Offset: 0x00338A64
		public void CopyTo(Ideo ideo)
		{
			ideo.foundation = IdeoGenerator.MakeFoundation(this.foundation.def);
			ideo.foundation.ideo = ideo;
			this.foundation.CopyTo(ideo.foundation);
			ideo.culture = this.culture;
			ideo.memes.Clear();
			ideo.memes.AddRange(this.memes);
			ideo.ClearPrecepts();
			foreach (Precept precept in this.precepts)
			{
				Precept precept2 = PreceptMaker.MakePrecept(precept.def);
				precept.CopyTo(precept2);
				precept2.ideo = ideo;
				ideo.precepts.Add(precept2);
			}
			ideo.RecachePrecepts();
			ideo.usedSymbols.Clear();
			ideo.usedSymbols.AddRange(ideo.usedSymbols);
			this.style.CopyTo(ideo.style);
			ideo.createdFromNoExpansionGame = this.createdFromNoExpansionGame;
			ideo.fluid = this.fluid;
			if (ideo.fluid)
			{
				ideo.development = new IdeoDevelopmentTracker(ideo);
				this.development.CopyTo(ideo.development);
			}
			ideo.initialPlayerIdeo = this.initialPlayerIdeo;
			ideo.name = this.name;
			ideo.adjective = this.adjective;
			ideo.memberName = this.memberName;
			ideo.iconDef = this.iconDef;
			ideo.colorDef = this.colorDef;
			ideo.primaryFactionColor = this.primaryFactionColor;
			ideo.leaderTitleMale = this.leaderTitleMale;
			ideo.leaderTitleFemale = this.leaderTitleFemale;
			ideo.usedSymbolPacks.Clear();
			ideo.usedSymbolPacks.AddRange(this.usedSymbolPacks);
			ideo.thingStyleCategories.Clear();
			ideo.thingStyleCategories.AddRange(this.thingStyleCategories);
			ideo.description = this.description;
			ideo.descriptionTemplate = this.descriptionTemplate;
			ideo.descriptionLocked = this.descriptionLocked;
			ideo.overriddenWorshipRoomLabel = this.overriddenWorshipRoomLabel;
			ideo.relicsCollected = this.relicsCollected;
		}

		// Token: 0x0400569F RID: 22175
		public static int MinBelieversToEnableObligations = 3;

		// Token: 0x040056A0 RID: 22176
		public int id = -1;

		// Token: 0x040056A1 RID: 22177
		public IdeoFoundation foundation;

		// Token: 0x040056A2 RID: 22178
		public CultureDef culture;

		// Token: 0x040056A3 RID: 22179
		public List<MemeDef> memes = new List<MemeDef>();

		// Token: 0x040056A4 RID: 22180
		private List<Precept> precepts = new List<Precept>();

		// Token: 0x040056A5 RID: 22181
		public List<string> usedSymbols = new List<string>();

		// Token: 0x040056A6 RID: 22182
		public IdeoStyleTracker style;

		// Token: 0x040056A7 RID: 22183
		public bool createdFromNoExpansionGame;

		// Token: 0x040056A8 RID: 22184
		[LoadAlias("classic")]
		public bool classicExtraMode;

		// Token: 0x040056A9 RID: 22185
		[LoadAlias("hiddenIdeoMode")]
		public bool classicMode;

		// Token: 0x040056AA RID: 22186
		public IdeoDevelopmentTracker development;

		// Token: 0x040056AB RID: 22187
		private bool fluid;

		// Token: 0x040056AC RID: 22188
		public bool hidden;

		// Token: 0x040056AD RID: 22189
		public bool solid;

		// Token: 0x040056AE RID: 22190
		public string fileName;

		// Token: 0x040056AF RID: 22191
		public bool initialPlayerIdeo;

		// Token: 0x040056B0 RID: 22192
		public bool anyPreceptEdited;

		// Token: 0x040056B1 RID: 22193
		public int currentCacheId = 1;

		// Token: 0x040056B2 RID: 22194
		public string name;

		// Token: 0x040056B3 RID: 22195
		public string adjective;

		// Token: 0x040056B4 RID: 22196
		public string memberName;

		// Token: 0x040056B5 RID: 22197
		public IdeoIconDef iconDef;

		// Token: 0x040056B6 RID: 22198
		public ColorDef colorDef;

		// Token: 0x040056B7 RID: 22199
		public Color? primaryFactionColor;

		// Token: 0x040056B8 RID: 22200
		public string leaderTitleMale;

		// Token: 0x040056B9 RID: 22201
		public string leaderTitleFemale;

		// Token: 0x040056BA RID: 22202
		public List<string> usedSymbolPacks = new List<string>();

		// Token: 0x040056BB RID: 22203
		public string description;

		// Token: 0x040056BC RID: 22204
		public string descriptionTemplate;

		// Token: 0x040056BD RID: 22205
		public bool descriptionLocked;

		// Token: 0x040056BE RID: 22206
		private string overriddenWorshipRoomLabel;

		// Token: 0x040056BF RID: 22207
		public bool relicsCollected;

		// Token: 0x040056C0 RID: 22208
		private Texture2D icon;

		// Token: 0x040056C1 RID: 22209
		public HashSet<ThoughtDef> cachedPossibleSituationalThoughts = new HashSet<ThoughtDef>();

		// Token: 0x040056C2 RID: 22210
		public List<GoodwillSituationDef> cachedPossibleGoodwillSituations = new List<GoodwillSituationDef>();

		// Token: 0x040056C3 RID: 22211
		public List<Precept_Role> cachedPossibleRoles = new List<Precept_Role>();

		// Token: 0x040056C4 RID: 22212
		public HashSet<Precept_Building> cachedPossibleBuildings = new HashSet<Precept_Building>();

		// Token: 0x040056C5 RID: 22213
		public HashSet<BuildableDef> cachedPossibleBuildables = new HashSet<BuildableDef>();

		// Token: 0x040056C6 RID: 22214
		private HashSet<NeedDef> cachedEnabledNeeds = new HashSet<NeedDef>();

		// Token: 0x040056C7 RID: 22215
		private HashSet<NeedDef> cachedDisabledNeeds = new HashSet<NeedDef>();

		// Token: 0x040056C8 RID: 22216
		public List<ThingStyleCategoryWithPriority> thingStyleCategories = new List<ThingStyleCategoryWithPriority>();

		// Token: 0x040056C9 RID: 22217
		private List<ThingDef> cachedVeneratedAnimals;

		// Token: 0x040056CA RID: 22218
		private List<XenotypeDef> cachedPreferredXenotypes;

		// Token: 0x040056CB RID: 22219
		private List<CustomXenotype> cachedPreferredCustomXenotypes;

		// Token: 0x040056CC RID: 22220
		private int requiredScarsCached;

		// Token: 0x040056CD RID: 22221
		private bool warnPlayerOnDesignateChopTree;

		// Token: 0x040056CE RID: 22222
		private bool warnPlayerOnDesignateMine;

		// Token: 0x040056CF RID: 22223
		private List<RitualObligationTargetFilter> cachedRitualTargetFilters;

		// Token: 0x040056D0 RID: 22224
		private HashSet<PreceptDef> allPreceptDefs = new HashSet<PreceptDef>();

		// Token: 0x040056D1 RID: 22225
		private string memberNamePluralCached;

		// Token: 0x040056D2 RID: 22226
		public HashSet<MentalBreakDef> cachedPossibleMentalBreaks = new HashSet<MentalBreakDef>();

		// Token: 0x040056D3 RID: 22227
		private static List<Pawn> tmpPlayerIdeoFollowers = new List<Pawn>();

		// Token: 0x040056D4 RID: 22228
		private static List<Precept> tmpThoughtPrecepts = new List<Precept>();

		// Token: 0x040056D5 RID: 22229
		private int colonistBelieverCountCached = -1;
	}
}
