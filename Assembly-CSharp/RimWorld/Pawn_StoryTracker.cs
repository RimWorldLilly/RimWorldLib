using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001548 RID: 5448
	public class Pawn_StoryTracker : IExposable
	{
		// Token: 0x17001672 RID: 5746
		// (get) Token: 0x06008517 RID: 34071 RVA: 0x002FF908 File Offset: 0x002FDB08
		// (set) Token: 0x06008518 RID: 34072 RVA: 0x002FF910 File Offset: 0x002FDB10
		public BackstoryDef Childhood
		{
			get
			{
				return this.childhood;
			}
			set
			{
				if (this.pawn.DevelopmentalStage.Adult() && value.spawnCategories.Contains("Child"))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Assigning active child backstory to adult pawn ");
					defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.backstoriesCache = null;
				this.childhood = value;
			}
		}

		// Token: 0x17001673 RID: 5747
		// (get) Token: 0x06008519 RID: 34073 RVA: 0x002FF98A File Offset: 0x002FDB8A
		// (set) Token: 0x0600851A RID: 34074 RVA: 0x002FF992 File Offset: 0x002FDB92
		public BackstoryDef Adulthood
		{
			get
			{
				return this.adulthood;
			}
			set
			{
				this.backstoriesCache = null;
				this.adulthood = value;
			}
		}

		// Token: 0x17001674 RID: 5748
		// (get) Token: 0x0600851B RID: 34075 RVA: 0x002FF9A2 File Offset: 0x002FDBA2
		// (set) Token: 0x0600851C RID: 34076 RVA: 0x002FF9B9 File Offset: 0x002FDBB9
		public string Title
		{
			get
			{
				if (this.title != null)
				{
					return this.title;
				}
				return this.TitleDefault;
			}
			set
			{
				this.title = null;
				if (value != this.Title && !value.NullOrEmpty())
				{
					this.title = value;
				}
			}
		}

		// Token: 0x17001675 RID: 5749
		// (get) Token: 0x0600851D RID: 34077 RVA: 0x002FF9DF File Offset: 0x002FDBDF
		public string TitleCap
		{
			get
			{
				return this.Title.CapitalizeFirst();
			}
		}

		// Token: 0x17001676 RID: 5750
		// (get) Token: 0x0600851E RID: 34078 RVA: 0x002FF9EC File Offset: 0x002FDBEC
		public string TitleDefault
		{
			get
			{
				if (this.adulthood != null)
				{
					return this.adulthood.TitleFor(this.pawn.gender);
				}
				if (this.childhood != null)
				{
					return this.childhood.TitleFor(this.pawn.gender);
				}
				return "";
			}
		}

		// Token: 0x17001677 RID: 5751
		// (get) Token: 0x0600851F RID: 34079 RVA: 0x002FFA3C File Offset: 0x002FDC3C
		public string TitleDefaultCap
		{
			get
			{
				return this.TitleDefault.CapitalizeFirst();
			}
		}

		// Token: 0x17001678 RID: 5752
		// (get) Token: 0x06008520 RID: 34080 RVA: 0x002FFA4C File Offset: 0x002FDC4C
		public string TitleShort
		{
			get
			{
				if (this.title != null)
				{
					return this.title;
				}
				if (this.adulthood != null)
				{
					return this.adulthood.TitleShortFor(this.pawn.gender);
				}
				if (this.childhood != null)
				{
					return this.childhood.TitleShortFor(this.pawn.gender);
				}
				return "";
			}
		}

		// Token: 0x17001679 RID: 5753
		// (get) Token: 0x06008521 RID: 34081 RVA: 0x002FFAAB File Offset: 0x002FDCAB
		public string TitleShortCap
		{
			get
			{
				return this.TitleShort.CapitalizeFirst();
			}
		}

		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x06008522 RID: 34082 RVA: 0x002FFAB8 File Offset: 0x002FDCB8
		// (set) Token: 0x06008523 RID: 34083 RVA: 0x002FFB2E File Offset: 0x002FDD2E
		public Color SkinColorBase
		{
			get
			{
				if (this.skinColorBase == null && this.pawn.genes != null)
				{
					GeneDef melaninGene = this.pawn.genes.GetMelaninGene();
					if (melaninGene != null)
					{
						this.skinColorBase = melaninGene.skinColorBase;
					}
					else
					{
						GeneDef geneDef = PawnSkinColors.RandomSkinColorGene(this.pawn);
						if (geneDef != null)
						{
							this.pawn.genes.AddGene(geneDef, false);
						}
					}
				}
				return this.skinColorBase.Value;
			}
			set
			{
				this.skinColorBase = new Color?(value);
			}
		}

		// Token: 0x1700167B RID: 5755
		// (get) Token: 0x06008524 RID: 34084 RVA: 0x002FFB3C File Offset: 0x002FDD3C
		public Color SkinColor
		{
			get
			{
				Color baseColor = this.skinColorOverride ?? this.SkinColorBase;
				if (this.pawn.IsMutant)
				{
					baseColor = MutantUtility.GetMutantSkinColor(this.pawn, baseColor);
				}
				return this.pawn.health.hediffSet.GetSkinColor(baseColor);
			}
		}

		// Token: 0x1700167C RID: 5756
		// (get) Token: 0x06008525 RID: 34085 RVA: 0x002FFB9B File Offset: 0x002FDD9B
		public bool SkinColorOverriden
		{
			get
			{
				return this.skinColorOverride != null || (this.pawn.IsMutant && this.pawn.mutant.Def.skinColorOverride != null);
			}
		}

		// Token: 0x1700167D RID: 5757
		// (get) Token: 0x06008526 RID: 34086 RVA: 0x002FFBD5 File Offset: 0x002FDDD5
		public bool CaresAboutOthersAppearance
		{
			get
			{
				return !this.pawn.Inhumanized() && !this.traits.HasTrait(TraitDefOf.Kind) && !this.traits.HasTrait(TraitDefOf.Ascetic);
			}
		}

		// Token: 0x1700167E RID: 5758
		// (get) Token: 0x06008527 RID: 34087 RVA: 0x002FFC0C File Offset: 0x002FDE0C
		// (set) Token: 0x06008528 RID: 34088 RVA: 0x002FFCB8 File Offset: 0x002FDEB8
		public Color HairColor
		{
			get
			{
				if (this.pawn.Corpse != null && this.pawn.Corpse.CurRotDrawMode == RotDrawMode.Rotting)
				{
					return PawnRenderUtility.GetRottenColor(this.hairColor);
				}
				if (ModsConfig.AnomalyActive && this.pawn.IsMutant && this.pawn.mutant.Def.useCorpseGraphics && this.pawn.mutant.rotStage == RotStage.Rotting)
				{
					return PawnRenderUtility.GetRottenColor(this.hairColor);
				}
				if (ModsConfig.AnomalyActive && this.pawn.IsShambler)
				{
					return MutantUtility.GetShamblerColor(this.hairColor);
				}
				return this.hairColor;
			}
			set
			{
				this.hairColor = value;
				if (ModsConfig.BiotechActive && this.pawn.genes != null)
				{
					foreach (Gene gene in this.pawn.genes.GenesListForReading)
					{
						if (gene.def.skinIsHairColor && gene.Active)
						{
							this.skinColorOverride = new Color?(this.hairColor);
							break;
						}
					}
				}
			}
		}

		// Token: 0x1700167F RID: 5759
		// (get) Token: 0x06008529 RID: 34089 RVA: 0x002FFD54 File Offset: 0x002FDF54
		public List<BackstoryDef> AllBackstories
		{
			get
			{
				if (this.backstoriesCache == null)
				{
					this.backstoriesCache = new List<BackstoryDef>();
					if (this.childhood != null)
					{
						this.backstoriesCache.Add(this.childhood);
					}
					if (this.adulthood != null)
					{
						this.backstoriesCache.Add(this.adulthood);
					}
				}
				return this.backstoriesCache;
			}
		}

		// Token: 0x17001680 RID: 5760
		// (get) Token: 0x0600852A RID: 34090 RVA: 0x002FFDAC File Offset: 0x002FDFAC
		public WorkTags DisabledWorkTagsBackstoryAndTraits
		{
			get
			{
				WorkTags workTags = WorkTags.None;
				if (this.childhood != null)
				{
					workTags |= this.childhood.workDisables;
				}
				if (this.adulthood != null)
				{
					workTags |= this.adulthood.workDisables;
				}
				for (int i = 0; i < this.traits.allTraits.Count; i++)
				{
					if (!this.traits.allTraits[i].Suppressed)
					{
						workTags |= this.traits.allTraits[i].def.disabledWorkTags;
					}
				}
				return workTags;
			}
		}

		// Token: 0x17001681 RID: 5761
		// (get) Token: 0x0600852B RID: 34091 RVA: 0x002FFE3C File Offset: 0x002FE03C
		public WorkTags DisabledWorkTagsBackstoryTraitsAndGenes
		{
			get
			{
				WorkTags workTags = this.DisabledWorkTagsBackstoryAndTraits;
				if (this.pawn.genes != null)
				{
					workTags |= this.pawn.genes.DisabledWorkTags;
				}
				return workTags;
			}
		}

		// Token: 0x17001682 RID: 5762
		// (get) Token: 0x0600852C RID: 34092 RVA: 0x002FFE74 File Offset: 0x002FE074
		public float VoicePitchFactor
		{
			get
			{
				return Pawn_StoryTracker.VoicePitchFactorRange.RandomInRangeSeeded(this.pawn.thingIDNumber);
			}
		}

		// Token: 0x17001683 RID: 5763
		// (get) Token: 0x0600852D RID: 34093 RVA: 0x002FFE99 File Offset: 0x002FE099
		public bool IsDisturbing
		{
			get
			{
				return (ModsConfig.AnomalyActive && this.traits.HasTrait(TraitDefOf.Disturbing)) || this.pawn.health.hediffSet.HasHediff(HediffDefOf.VoidTouched, false);
			}
		}

		// Token: 0x0600852E RID: 34094 RVA: 0x002FFED1 File Offset: 0x002FE0D1
		public Pawn_StoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.traits = new TraitSet(pawn);
		}

		// Token: 0x0600852F RID: 34095 RVA: 0x002FFF04 File Offset: 0x002FE104
		public void ExposeData()
		{
			Scribe_Defs.Look<BodyTypeDef>(ref this.bodyType, "bodyType");
			Scribe_Defs.Look<HairDef>(ref this.hairDef, "hairDef");
			Scribe_Values.Look<Color>(ref this.hairColor, "hairColor", default(Color), false);
			Scribe_Values.Look<Color?>(ref this.skinColorOverride, "skinColorOverride", null, false);
			Scribe_Deep.Look<TraitSet>(ref this.traits, "traits", new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<string>(ref this.birthLastName, "birthLastName", null, false);
			Scribe_Defs.Look<ColorDef>(ref this.favoriteColor, "favoriteColorDef");
			Scribe_Defs.Look<FurDef>(ref this.furDef, "furDef");
			Scribe_Defs.Look<HeadTypeDef>(ref this.headType, "headType");
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				Scribe_Values.Look<float>(ref this.melanin, "melanin", -1f, false);
				if (Scribe.mode == LoadSaveMode.PostLoadInit && this.melanin >= 0f)
				{
					this.pawn.genes.InitializeGenesFromOldSave(this.melanin);
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				string headGraphicPath = null;
				Scribe_Values.Look<string>(ref headGraphicPath, "headGraphicPath", null, false);
				if (!headGraphicPath.NullOrEmpty())
				{
					this.TryGetRandomHeadFromSet(from x in DefDatabase<HeadTypeDef>.AllDefs
					where x.graphicPath == headGraphicPath
					select x);
				}
				if (ModsConfig.IdeologyActive && this.favoriteColor == null)
				{
					Color? color2 = null;
					Scribe_Values.Look<Color?>(ref color2, "favoriteColor", null, false);
					if (color2 != null)
					{
						Color color = color2.Value;
						this.favoriteColor = DefDatabase<ColorDef>.AllDefsListForReading.FirstOrDefault((ColorDef cd) => cd.color.IndistinguishableFrom(color));
						if (this.favoriteColor == null)
						{
							this.favoriteColor = DefDatabase<ColorDef>.AllDefs.Where(delegate(ColorDef x)
							{
								ColorType colorType = x.colorType;
								return colorType == ColorType.Misc || colorType == ColorType.Ideo;
							}).RandomElement<ColorDef>();
						}
					}
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.birthLastName == null)
				{
					NameTriple nameTriple = this.pawn.Name as NameTriple;
					if (nameTriple != null)
					{
						this.birthLastName = nameTriple.Last;
					}
				}
				if (this.hairDef == null)
				{
					this.hairDef = DefDatabase<HairDef>.AllDefs.RandomElement<HairDef>();
				}
				if (this.headType == null)
				{
					this.TryGetRandomHeadFromSet(from x in DefDatabase<HeadTypeDef>.AllDefs
					where x.randomChosen
					select x);
				}
				if (ModsConfig.BiotechActive && this.pawn.DevelopmentalStage.Child() && this.bodyType != BodyTypeDefOf.Child)
				{
					Pawn pawn = this.pawn;
					string str = (pawn != null) ? pawn.ToString() : null;
					string str2 = " had body type set to ";
					BodyTypeDef bodyTypeDef = this.bodyType;
					Log.Warning(str + str2 + ((bodyTypeDef != null) ? bodyTypeDef.label : null) + " as a child.");
					this.bodyType = BodyTypeDefOf.Child;
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				string childDefName = null;
				Scribe_Values.Look<string>(ref childDefName, "childhood", null, false);
				if (!childDefName.NullOrEmpty())
				{
					this.childhood = DefDatabase<BackstoryDef>.AllDefs.FirstOrDefault((BackstoryDef b) => b.defName == childDefName);
					if (this.childhood == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Couldn't load child backstory ");
						defaultInterpolatedStringHandler.AppendFormatted(childDefName);
						defaultInterpolatedStringHandler.AppendLiteral(" for ");
						defaultInterpolatedStringHandler.AppendFormatted<Name>(this.pawn.Name);
						defaultInterpolatedStringHandler.AppendLiteral(". Giving random.");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
						this.childhood = (from bs in DefDatabase<BackstoryDef>.AllDefs
						where bs.slot == BackstorySlot.Childhood
						select bs).RandomElement<BackstoryDef>();
					}
				}
				string adultDefName = null;
				Scribe_Values.Look<string>(ref adultDefName, "adulthood", null, false);
				if (!adultDefName.NullOrEmpty())
				{
					this.adulthood = DefDatabase<BackstoryDef>.AllDefs.FirstOrDefault((BackstoryDef b) => b.defName == adultDefName);
					if (this.adulthood == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(51, 2);
						defaultInterpolatedStringHandler2.AppendLiteral("Couldn't load adult backstory ");
						defaultInterpolatedStringHandler2.AppendFormatted(adultDefName);
						defaultInterpolatedStringHandler2.AppendLiteral(" for ");
						defaultInterpolatedStringHandler2.AppendFormatted<Name>(this.pawn.Name);
						defaultInterpolatedStringHandler2.AppendLiteral(". Giving random.");
						Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
						this.adulthood = (from bs in DefDatabase<BackstoryDef>.AllDefs
						where bs.slot == BackstorySlot.Adulthood
						select bs).RandomElement<BackstoryDef>();
						return;
					}
				}
			}
			else if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe_Defs.Look<BackstoryDef>(ref this.childhood, "childhood");
				Scribe_Defs.Look<BackstoryDef>(ref this.adulthood, "adulthood");
			}
		}

		// Token: 0x06008530 RID: 34096 RVA: 0x0030040D File Offset: 0x002FE60D
		public BackstoryDef GetBackstory(BackstorySlot slot)
		{
			if (slot == BackstorySlot.Childhood)
			{
				return this.childhood;
			}
			return this.adulthood;
		}

		// Token: 0x06008531 RID: 34097 RVA: 0x00300420 File Offset: 0x002FE620
		public bool TryGetRandomHeadFromSet(IEnumerable<HeadTypeDef> options)
		{
			Rand.PushState(this.pawn.thingIDNumber);
			bool result = options.Where(new Func<HeadTypeDef, bool>(this.<TryGetRandomHeadFromSet>g__CanUseHeadType|61_0)).TryRandomElementByWeight((HeadTypeDef x) => x.selectionWeight, out this.headType);
			Rand.PopState();
			return result;
		}

		// Token: 0x06008533 RID: 34099 RVA: 0x00300494 File Offset: 0x002FE694
		[CompilerGenerated]
		private bool <TryGetRandomHeadFromSet>g__CanUseHeadType|61_0(HeadTypeDef head)
		{
			if (!ModsConfig.BiotechActive || head.requiredGenes.NullOrEmpty<GeneDef>())
			{
				return head.gender == Gender.None || head.gender == this.pawn.gender;
			}
			if (this.pawn.genes == null)
			{
				return false;
			}
			foreach (GeneDef geneDef in head.requiredGenes)
			{
				if (!this.pawn.genes.HasActiveGene(geneDef))
				{
					return false;
				}
			}
			return head.gender == Gender.None || head.gender == this.pawn.gender;
		}

		// Token: 0x040051E3 RID: 20963
		private Pawn pawn;

		// Token: 0x040051E4 RID: 20964
		private BackstoryDef childhood;

		// Token: 0x040051E5 RID: 20965
		private BackstoryDef adulthood;

		// Token: 0x040051E6 RID: 20966
		private Color hairColor = Color.white;

		// Token: 0x040051E7 RID: 20967
		public Color? skinColorOverride;

		// Token: 0x040051E8 RID: 20968
		public HeadTypeDef headType;

		// Token: 0x040051E9 RID: 20969
		public BodyTypeDef bodyType;

		// Token: 0x040051EA RID: 20970
		public HairDef hairDef;

		// Token: 0x040051EB RID: 20971
		public TraitSet traits;

		// Token: 0x040051EC RID: 20972
		public string title;

		// Token: 0x040051ED RID: 20973
		public string birthLastName;

		// Token: 0x040051EE RID: 20974
		public ColorDef favoriteColor;

		// Token: 0x040051EF RID: 20975
		public FurDef furDef;

		// Token: 0x040051F0 RID: 20976
		[Unsaved(false)]
		private Color? skinColorBase;

		// Token: 0x040051F1 RID: 20977
		[Unsaved(false)]
		private float melanin = -1f;

		// Token: 0x040051F2 RID: 20978
		[Unsaved(false)]
		private List<BackstoryDef> backstoriesCache;

		// Token: 0x040051F3 RID: 20979
		private static readonly FloatRange VoicePitchFactorRange = new FloatRange(0.85f, 1.15f);
	}
}
