using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D58 RID: 7512
	public class Dialog_GrowthMomentChoices : Window
	{
		// Token: 0x170023EC RID: 9196
		// (get) Token: 0x0600C272 RID: 49778 RVA: 0x0046DC67 File Offset: 0x0046BE67
		private SkillDef SinglePassionChoice
		{
			get
			{
				if (this.chosenPassions.Count == 1)
				{
					return this.chosenPassions[0];
				}
				return null;
			}
		}

		// Token: 0x0600C273 RID: 49779 RVA: 0x0046DC88 File Offset: 0x0046BE88
		public Dialog_GrowthMomentChoices(TaggedString text, ChoiceLetter_GrowthMoment letter) : base(null)
		{
			this.text = text;
			this.letter = letter;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			if (!this.SelectionsMade())
			{
				this.closeOnAccept = false;
				this.closeOnCancel = false;
			}
		}

		// Token: 0x170023ED RID: 9197
		// (get) Token: 0x0600C274 RID: 49780 RVA: 0x0046DCE0 File Offset: 0x0046BEE0
		private float Height
		{
			get
			{
				return CharacterCardUtility.PawnCardSize(this.letter.pawn).y + Window.CloseButSize.y + 4f + this.Margin * 2f;
			}
		}

		// Token: 0x170023EE RID: 9198
		// (get) Token: 0x0600C275 RID: 49781 RVA: 0x0046DD15 File Offset: 0x0046BF15
		public override Vector2 InitialSize
		{
			get
			{
				if (!this.letter.ShowInfoTabs)
				{
					return new Vector2(480f, this.Height);
				}
				return new Vector2(1000f, this.Height);
			}
		}

		// Token: 0x0600C276 RID: 49782 RVA: 0x0046DD48 File Offset: 0x0046BF48
		public override void DoWindowContents(Rect inRect)
		{
			bool showInfoTabs = this.letter.ShowInfoTabs;
			float width = 446f;
			Rect outRect = showInfoTabs ? inRect.LeftPartPixels(width) : inRect;
			outRect.yMax -= 4f + Window.CloseButSize.y;
			Text.Font = GameFont.Small;
			Rect viewRect = new Rect(outRect.x, outRect.y, outRect.width - 16f, this.scrollHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			Widgets.Label(0f, ref num, viewRect.width, this.text.Resolve(), default(TipSignal));
			num += 14f;
			this.DrawPassionChoices(viewRect.width, ref num);
			this.DrawTraitChoices(viewRect.width, ref num);
			this.DrawBottomText(viewRect.width, ref num);
			if (Event.current.type == EventType.layout)
			{
				this.scrollHeight = Mathf.Max(num, outRect.height);
			}
			Widgets.EndScrollView();
			Rect rect = new Rect(0f, outRect.yMax + 4f, inRect.width, Window.CloseButSize.y);
			AcceptanceReport acceptanceReport = this.CanClose();
			Rect rect2 = new Rect(rect.xMax - Window.CloseButSize.x, rect.y, Window.CloseButSize.x, Window.CloseButSize.y);
			if (this.letter.ArchiveView)
			{
				rect2.x = rect.center.x - Window.CloseButSize.x / 2f;
			}
			else
			{
				if (Widgets.ButtonText(new Rect(rect.x, rect.y, Window.CloseButSize.x, Window.CloseButSize.y), "Later".Translate(), true, true, true, null))
				{
					if (this.letter.ShouldAutomaticallyOpenLetter)
					{
						Messages.Message("MessageCannotPostponeGrowthMoment".Translate(this.letter.pawn.Named("PAWN")), null, MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						this.Close(true);
					}
				}
				if (!acceptanceReport.Accepted)
				{
					TextAnchor anchor = Text.Anchor;
					GameFont font = Text.Font;
					Text.Font = GameFont.Tiny;
					Text.Anchor = TextAnchor.MiddleRight;
					Rect rect3 = rect;
					rect3.xMax = rect2.xMin - 4f;
					Widgets.Label(rect3, acceptanceReport.Reason.Colorize(ColoredText.WarningColor));
					Text.Font = font;
					Text.Anchor = anchor;
				}
			}
			if (Widgets.ButtonText(rect2, "OK".Translate(), true, true, true, null))
			{
				if (acceptanceReport.Accepted)
				{
					this.letter.MakeChoices(this.chosenPassions, this.chosenTrait);
					this.Close(true);
					Find.LetterStack.RemoveLetter(this.letter);
				}
				else
				{
					Messages.Message(acceptanceReport.Reason, null, MessageTypeDefOf.RejectInput, false);
				}
			}
			if (showInfoTabs)
			{
				Rect rect4 = inRect.RightPartPixels(1000f - outRect.width - 34f);
				rect4.xMin += 17f;
				rect4.yMax -= 4f + Window.CloseButSize.y;
				Dialog_GrowthMomentChoices.tmpTabs.Clear();
				Dialog_GrowthMomentChoices.tmpTabs.Add(new TabRecord("TabCharacter".Translate(), delegate()
				{
					this.showBio = true;
				}, this.showBio));
				Dialog_GrowthMomentChoices.tmpTabs.Add(new TabRecord("TabHealth".Translate(), delegate()
				{
					this.showBio = false;
				}, !this.showBio));
				rect4.yMin += 32f;
				Rect rect5 = new Rect(rect4.x + (this.showBio ? 17f : 0f), rect4.y, rect4.width, rect4.height);
				Widgets.DrawMenuSection(rect4);
				if (this.showBio)
				{
					CharacterCardUtility.DrawCharacterCard(rect5, this.letter.pawn, null, default(Rect), false);
				}
				else
				{
					HealthCardUtility.DrawHediffListing(rect5, this.letter.pawn, false, 17f, false);
				}
				TabDrawer.DrawTabs<TabRecord>(rect4, Dialog_GrowthMomentChoices.tmpTabs, 200f);
				Dialog_GrowthMomentChoices.tmpTabs.Clear();
			}
		}

		// Token: 0x0600C277 RID: 49783 RVA: 0x0046E1BC File Offset: 0x0046C3BC
		private bool SelectionsMade()
		{
			return this.letter.ArchiveView || ((this.letter.passionChoices.NullOrEmpty<SkillDef>() || !this.chosenPassions.NullOrEmpty<SkillDef>() || this.letter.passionGainsCount <= 0) && (this.letter.traitChoices.NullOrEmpty<Trait>() || this.chosenTrait != null));
		}

		// Token: 0x0600C278 RID: 49784 RVA: 0x0046E224 File Offset: 0x0046C424
		private AcceptanceReport CanClose()
		{
			if (this.letter.ArchiveView)
			{
				return true;
			}
			if (!this.letter.passionChoices.NullOrEmpty<SkillDef>() && this.chosenPassions.Count != this.letter.passionGainsCount)
			{
				if (this.letter.passionGainsCount == 1)
				{
					return "SelectPassionSingular".Translate();
				}
				return "SelectPassionsPlural".Translate(this.letter.passionGainsCount);
			}
			else
			{
				if (!this.letter.traitChoices.NullOrEmpty<Trait>() && this.chosenTrait == null)
				{
					return "SelectATrait".Translate();
				}
				if (!this.SelectionsMade())
				{
					return "BirthdayMakeChoices".Translate();
				}
				return AcceptanceReport.WasAccepted;
			}
		}

		// Token: 0x0600C279 RID: 49785 RVA: 0x0046E2F4 File Offset: 0x0046C4F4
		private void DrawTraitChoices(float width, ref float curY)
		{
			if (!this.letter.ArchiveView && !this.letter.traitChoices.NullOrEmpty<Trait>())
			{
				Widgets.Label(0f, ref curY, width, "BirthdayPickTrait".Translate(this.letter.pawn).Resolve() + ":", default(TipSignal));
				Listing_Standard listing_Standard = new Listing_Standard();
				Rect rect = new Rect(0f, curY, 230f, 99999f);
				listing_Standard.Begin(rect);
				foreach (Trait trait in this.letter.traitChoices)
				{
					if (listing_Standard.RadioButton(trait.LabelCap, this.chosenTrait == trait, 30f, trait.TipString(this.letter.pawn), null))
					{
						this.chosenTrait = trait;
					}
				}
				if (this.letter.noTraitOptionShown)
				{
					TaggedString taggedString = "BirthdayNoTraitChoice".Translate();
					TaggedString taggedString2 = "BirthdayNoTraitChoiceTooltip".Translate(this.letter.pawn);
					if (listing_Standard.RadioButton(taggedString, this.chosenTrait == ChoiceLetter_GrowthMoment.NoTrait, 30f, taggedString2, null))
					{
						this.chosenTrait = ChoiceLetter_GrowthMoment.NoTrait;
					}
				}
				listing_Standard.End();
				curY += listing_Standard.CurHeight + 10f + 4f;
			}
			if (this.letter.ArchiveView && this.letter.chosenTrait != null)
			{
				Widgets.Label(0f, ref curY, width, "BirthdayTraitArchive".Translate(this.letter.chosenTrait.Label.Colorize(ColorLibrary.LightBlue)), default(TipSignal));
				curY += 14f;
			}
		}

		// Token: 0x0600C27A RID: 49786 RVA: 0x0046E50C File Offset: 0x0046C70C
		private void DrawPassionChoices(float width, ref float curY)
		{
			if (!this.letter.ArchiveView && !this.letter.passionChoices.NullOrEmpty<SkillDef>() && this.letter.passionGainsCount > 0)
			{
				Widgets.Label(0f, ref curY, width, ((this.letter.passionGainsCount == 1) ? "BirthdayPickPassion".Translate(this.letter.pawn) : "BirthdayPickPassions".Translate(this.letter.pawn, this.letter.passionGainsCount)).Resolve() + ":", default(TipSignal));
				Listing_Standard listing_Standard = new Listing_Standard();
				Rect rect = new Rect(0f, curY, 230f, 99999f);
				listing_Standard.Begin(rect);
				foreach (SkillDef skillDef in this.letter.passionChoices)
				{
					SkillRecord skill = this.letter.pawn.skills.GetSkill(skillDef);
					Passion passion = this.chosenPassions.Contains(skillDef) ? skill.passion.IncrementPassion() : skill.passion;
					if (passion > Passion.None)
					{
						Texture2D image = (passion == Passion.Major) ? SkillUI.PassionMajorIcon : SkillUI.PassionMinorIcon;
						GUI.DrawTexture(new Rect(rect.xMax - 55f, listing_Standard.CurHeight, 24f, 24f), image);
					}
					if (this.letter.passionGainsCount > 1)
					{
						bool flag = this.chosenPassions.Contains(skillDef);
						bool flag2 = flag;
						listing_Standard.CheckboxLabeled(skillDef.LabelCap, ref flag, 30f);
						if (flag != flag2)
						{
							if (flag)
							{
								this.chosenPassions.Add(skillDef);
							}
							else
							{
								this.chosenPassions.Remove(skillDef);
							}
						}
					}
					else if (listing_Standard.RadioButton(skillDef.LabelCap, this.SinglePassionChoice == skillDef, 30f, null, null))
					{
						this.chosenPassions.Clear();
						this.chosenPassions.Add(skillDef);
					}
				}
				listing_Standard.End();
				curY += listing_Standard.CurHeight + 10f + 4f;
			}
			if (this.letter.ArchiveView && !this.letter.chosenPassions.NullOrEmpty<SkillDef>())
			{
				Widgets.Label(0f, ref curY, width, "BirthdayPassionArchive".Translate((from x in this.letter.chosenPassions
				select x.label).ToCommaList(true, false).Colorize(ColorLibrary.LightBlue)), default(TipSignal));
				curY += 14f;
			}
		}

		// Token: 0x0600C27B RID: 49787 RVA: 0x0046E824 File Offset: 0x0046CA24
		private void DrawBottomText(float width, ref float curY)
		{
			if (this.letter.growthTier < 0 || (this.letter.passionChoices.NullOrEmpty<SkillDef>() && this.letter.traitChoices.NullOrEmpty<Trait>()))
			{
				return;
			}
			string str = "BirthdayGrowthTier".Translate(this.letter.pawn, this.letter.growthTier).Colorize(ColoredText.SubtleGrayColor);
			if (this.letter.pawn.Name != this.letter.oldName)
			{
				str = "BirthdayNickname".Translate(this.letter.oldName.ToStringFull.Colorize(ColoredText.NameColor), this.letter.pawn.LabelShort.Colorize(ColoredText.NameColor)).Resolve() + "\n\n" + str;
			}
			Widgets.Label(0f, ref curY, width, str, default(TipSignal));
			curY += 10f;
		}

		// Token: 0x04006F2E RID: 28462
		private ChoiceLetter_GrowthMoment letter;

		// Token: 0x04006F2F RID: 28463
		private List<SkillDef> chosenPassions = new List<SkillDef>();

		// Token: 0x04006F30 RID: 28464
		private Trait chosenTrait;

		// Token: 0x04006F31 RID: 28465
		private TaggedString text;

		// Token: 0x04006F32 RID: 28466
		private bool showBio = true;

		// Token: 0x04006F33 RID: 28467
		private float scrollHeight;

		// Token: 0x04006F34 RID: 28468
		private Vector2 scrollPosition;

		// Token: 0x04006F35 RID: 28469
		private const float WidthWithTabs = 1000f;

		// Token: 0x04006F36 RID: 28470
		private const float WidthRegular = 480f;

		// Token: 0x04006F37 RID: 28471
		private const float PassionListingWidth = 230f;

		// Token: 0x04006F38 RID: 28472
		private const float PassionIconSize = 24f;

		// Token: 0x04006F39 RID: 28473
		private const float OptionTabIn = 30f;

		// Token: 0x04006F3A RID: 28474
		private static List<TabRecord> tmpTabs = new List<TabRecord>();
	}
}
