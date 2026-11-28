using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D57 RID: 7511
	public class ChoiceLetter_GrowthMoment : LetterWithTimeout
	{
		// Token: 0x170023E8 RID: 9192
		// (get) Token: 0x0600C263 RID: 49763 RVA: 0x0046D3B4 File Offset: 0x0046B5B4
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023E9 RID: 9193
		// (get) Token: 0x0600C264 RID: 49764 RVA: 0x0046D3B7 File Offset: 0x0046B5B7
		public override bool CanShowInLetterStack
		{
			get
			{
				return !this.ArchiveView && !base.TimeoutPassed;
			}
		}

		// Token: 0x170023EA RID: 9194
		// (get) Token: 0x0600C265 RID: 49765 RVA: 0x0046D3CC File Offset: 0x0046B5CC
		public bool ArchiveView
		{
			get
			{
				return this.choiceMade || this.pawn.DestroyedOrNull();
			}
		}

		// Token: 0x170023EB RID: 9195
		// (get) Token: 0x0600C266 RID: 49766 RVA: 0x0046D3E3 File Offset: 0x0046B5E3
		public bool ShowInfoTabs
		{
			get
			{
				return !this.ArchiveView && (!this.passionChoices.NullOrEmpty<SkillDef>() || !this.traitChoices.NullOrEmpty<Trait>());
			}
		}

		// Token: 0x0600C267 RID: 49767 RVA: 0x0046D40C File Offset: 0x0046B60C
		public void ConfigureGrowthLetter(Pawn pawn, int passionChoiceCount, int traitChoiceCount, int passionGainsCount, List<string> enabledWorkTypes, Name oldName)
		{
			this.pawn = pawn;
			this.passionChoiceCount = passionChoiceCount;
			this.passionGainsCount = passionGainsCount;
			this.traitChoiceCount = traitChoiceCount;
			this.enabledWorkTypes = enabledWorkTypes;
			this.oldName = oldName;
			this.growthTier = pawn.ageTracker.GrowthTier;
			if (passionGainsCount > passionChoiceCount)
			{
				Log.Error("ConfigureGrowthLetter: passionGainsCount > passionChoiceCount.");
				passionGainsCount = passionChoiceCount;
			}
			this.CacheLetterText();
		}

		// Token: 0x0600C268 RID: 49768 RVA: 0x0046D470 File Offset: 0x0046B670
		private void CacheLetterText()
		{
			this.text = "BirthdayBiologicalGrowthMoment".Translate(this.pawn, this.pawn.ageTracker.AgeBiologicalYears);
			if (this.pawn.ageTracker.AgeBiologicalYears >= GrowthUtility.GrowthMomentAges[0])
			{
				this.text += "\n\n" + GrowthUtility.GrowthFlavorForTier(this.pawn, this.growthTier);
			}
			if (!this.enabledWorkTypes.NullOrEmpty<string>())
			{
				this.text += "\n\n" + "BirthdayBiologicalAgeWorkTypes".Translate(this.pawn, this.pawn.ageTracker.AgeBiologicalYears) + ":\n" + this.enabledWorkTypes.ToLineList("  - ");
			}
			this.mouseoverText = this.text;
			if (this.pawn.Name != this.oldName)
			{
				TaggedString t = "BirthdayNickname".Translate(this.oldName.ToStringFull.Colorize(ColoredText.NameColor), this.pawn.LabelShort.Colorize(ColoredText.NameColor));
				this.mouseoverText += "\n\n" + t;
			}
			if (this.passionChoiceCount > 0 || this.traitChoiceCount > 0)
			{
				this.mouseoverText += "\n\n" + "BirthdayChooseHowPawnWillGrow".Translate(this.pawn);
			}
		}

		// Token: 0x0600C269 RID: 49769 RVA: 0x0046D620 File Offset: 0x0046B820
		public override void OpenLetter()
		{
			this.TrySetChoices();
			Find.MainTabsRoot.EscapeCurrentTab(false);
			Dialog_GrowthMomentChoices window = new Dialog_GrowthMomentChoices(this.text, this);
			Find.WindowStack.Add(window);
		}

		// Token: 0x0600C26A RID: 49770 RVA: 0x0046D656 File Offset: 0x0046B856
		protected override string GetMouseoverText()
		{
			return this.mouseoverText.Resolve();
		}

		// Token: 0x0600C26B RID: 49771 RVA: 0x0046D664 File Offset: 0x0046B864
		public void MakeChoices(List<SkillDef> skills, Trait trait)
		{
			if (this.ArchiveView)
			{
				return;
			}
			this.choiceMade = true;
			this.chosenPassions = skills;
			this.chosenTrait = trait;
			if (!skills.NullOrEmpty<SkillDef>())
			{
				foreach (SkillDef skillDef in skills)
				{
					SkillRecord skill = this.pawn.skills.GetSkill(skillDef);
					if (skill.passion == Passion.Major)
					{
						string format = "{0} Tried to upgrade a passion for {1} but it's already major";
						Pawn pawn = this.pawn;
						Log.Warning(string.Format(format, (pawn != null) ? pawn.LabelShort : null, skillDef));
						return;
					}
					skill.passion = skill.passion.IncrementPassion();
					if (ModsConfig.BiotechActive && this.pawn.genes != null)
					{
						foreach (Gene gene in this.pawn.genes.GenesListForReading)
						{
							if (gene.Active && gene.def.passionMod != null && gene.def.passionMod.skill == skill.def && gene.def.passionMod.modType == PassionMod.PassionModType.DropAll)
							{
								Passion? passionPreAdd = gene.passionPreAdd;
								if (passionPreAdd != null)
								{
									if (passionPreAdd.Value == Passion.None)
									{
										gene.passionPreAdd = new Passion?(Passion.Minor);
									}
									else if (passionPreAdd.Value == Passion.Minor)
									{
										gene.passionPreAdd = new Passion?(Passion.Major);
									}
								}
								else
								{
									gene.passionPreAdd = new Passion?(skill.passion);
								}
							}
						}
					}
				}
			}
			if (trait != ChoiceLetter_GrowthMoment.NoTrait && trait != null)
			{
				this.pawn.story.traits.GainTrait(trait, false);
				TraitUtility.ApplySkillGainFromTrait(this.pawn, trait);
			}
			if (this.pawn.ageTracker.AgeBiologicalYears == 13)
			{
				PawnGenerator.TryGenerateSexualityTraitFor(this.pawn, true);
			}
			this.pawn.ageTracker.growthPoints = 0f;
			this.pawn.ageTracker.canGainGrowthPoints = true;
		}

		// Token: 0x0600C26C RID: 49772 RVA: 0x0046D8C0 File Offset: 0x0046BAC0
		public static List<SkillDef> PassionOptions(Pawn pawn, int count, bool checkGenes)
		{
			return (from s in DefDatabase<SkillDef>.AllDefsListForReading
			where ChoiceLetter_GrowthMoment.IsValidGrowthPassionOption(pawn, s, checkGenes)
			select s).InRandomOrder(null).Take(count).ToList<SkillDef>();
		}

		// Token: 0x0600C26D RID: 49773 RVA: 0x0046D908 File Offset: 0x0046BB08
		private void TrySetChoices()
		{
			if (this.choiceMade || this.pawn.DestroyedOrNull())
			{
				return;
			}
			if (this.passionChoiceCount > 0 && this.passionChoices == null)
			{
				this.passionChoices = ChoiceLetter_GrowthMoment.PassionOptions(this.pawn, this.passionChoiceCount, false);
			}
			if (this.traitChoiceCount > 0 && this.traitChoices == null)
			{
				if (this.traitChoiceCount > 2 && Rand.Value < 0.35f)
				{
					this.traitChoiceCount--;
					this.noTraitOptionShown = true;
				}
				this.traitChoices = PawnGenerator.GenerateTraitsFor(this.pawn, this.traitChoiceCount, null, true);
			}
		}

		// Token: 0x0600C26E RID: 49774 RVA: 0x0046D9B0 File Offset: 0x0046BBB0
		private static bool IsValidGrowthPassionOption(Pawn pawn, SkillDef skill, bool checkGenes)
		{
			SkillRecord skill2 = pawn.skills.GetSkill(skill);
			if (checkGenes && pawn.genes != null)
			{
				foreach (Gene gene in pawn.genes.GenesListForReading)
				{
					if (gene.Active && gene.def.passionMod != null && gene.def.passionMod.modType == PassionMod.PassionModType.DropAll && gene.def.passionMod.skill == skill)
					{
						return false;
					}
				}
			}
			return !skill2.PermanentlyDisabled && skill2.passion != Passion.Major;
		}

		// Token: 0x0600C26F RID: 49775 RVA: 0x0046DA70 File Offset: 0x0046BC70
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Collections.Look<Trait>(ref this.traitChoices, "traitChoices", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.growthTier, "growthTier", 0, false);
			Scribe_Collections.Look<SkillDef>(ref this.passionChoices, "passionChoices", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.enabledWorkTypes, "enabledWorkTypes", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<SkillDef>(ref this.chosenPassions, "chosenPassions", LookMode.Def, Array.Empty<object>());
			Scribe_Deep.Look<Trait>(ref this.chosenTrait, "chosenTrait", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.passionChoiceCount, "passionChoiceCount", 0, false);
			Scribe_Values.Look<int>(ref this.passionGainsCount, "passionGainsCount", 0, false);
			Scribe_Values.Look<int>(ref this.traitChoiceCount, "traitChoiceCount", 0, false);
			Scribe_Values.Look<TaggedString>(ref this.text, "text", default(TaggedString), false);
			Scribe_Values.Look<TaggedString>(ref this.mouseoverText, "mouseoverText", default(TaggedString), false);
			Scribe_Values.Look<bool>(ref this.choiceMade, "choiceMade", false, false);
			Scribe_Deep.Look<Name>(ref this.oldName, "oldName", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.noTraitOptionShown, "noTraitOptionShown", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Trait> list = this.traitChoices;
				if (list != null)
				{
					list.RemoveAll((Trait x) => x == null);
				}
				List<Trait> list2 = this.traitChoices;
				if (list2 != null)
				{
					list2.RemoveAll((Trait x) => x.def == null);
				}
				List<SkillDef> list3 = this.passionChoices;
				if (list3 == null)
				{
					return;
				}
				list3.RemoveAll((SkillDef x) => x == null);
			}
		}

		// Token: 0x04006F1D RID: 28445
		public const float ChanceForNoTraitOption = 0.35f;

		// Token: 0x04006F1E RID: 28446
		public static Trait NoTrait = new Trait();

		// Token: 0x04006F1F RID: 28447
		public Pawn pawn;

		// Token: 0x04006F20 RID: 28448
		public TaggedString text;

		// Token: 0x04006F21 RID: 28449
		public TaggedString mouseoverText;

		// Token: 0x04006F22 RID: 28450
		private int passionChoiceCount;

		// Token: 0x04006F23 RID: 28451
		public int traitChoiceCount;

		// Token: 0x04006F24 RID: 28452
		public int passionGainsCount;

		// Token: 0x04006F25 RID: 28453
		public List<SkillDef> passionChoices;

		// Token: 0x04006F26 RID: 28454
		public List<Trait> traitChoices;

		// Token: 0x04006F27 RID: 28455
		public bool noTraitOptionShown;

		// Token: 0x04006F28 RID: 28456
		public Trait chosenTrait;

		// Token: 0x04006F29 RID: 28457
		public List<SkillDef> chosenPassions;

		// Token: 0x04006F2A RID: 28458
		public List<string> enabledWorkTypes;

		// Token: 0x04006F2B RID: 28459
		public bool choiceMade;

		// Token: 0x04006F2C RID: 28460
		public Name oldName;

		// Token: 0x04006F2D RID: 28461
		public int growthTier;
	}
}
