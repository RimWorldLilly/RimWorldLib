using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D0A RID: 7434
	public class Page_SelectStoryteller : Page
	{
		// Token: 0x170022EA RID: 8938
		// (get) Token: 0x0600C030 RID: 49200 RVA: 0x00460BF3 File Offset: 0x0045EDF3
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x0600C031 RID: 49201 RVA: 0x00460C04 File Offset: 0x0045EE04
		public override void PreOpen()
		{
			base.PreOpen();
			if (this.storyteller == null)
			{
				this.storyteller = (from d in DefDatabase<StorytellerDef>.AllDefs
				where d.listVisible
				orderby d.listOrder
				select d).First<StorytellerDef>();
			}
			StorytellerUI.ResetStorytellerSelectionInterface();
		}

		// Token: 0x0600C032 RID: 49202 RVA: 0x00460C7C File Offset: 0x0045EE7C
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			StorytellerUI.DrawStorytellerSelectionInterface(base.GetMainRect(rect, 0f, false), ref this.storyteller, ref this.difficulty, ref this.difficultyValues, this.selectedStorytellerInfoListing);
			base.DoBottomButtons(rect, null, null, null, true, true);
			Rect rect2 = new Rect(rect.xMax - Page.BottomButSize.x - 200f - 6f, rect.yMax - Page.BottomButSize.y, 200f, Page.BottomButSize.y);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, "CanChangeStorytellerSettingsDuringPlay".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600C033 RID: 49203 RVA: 0x00460D2C File Offset: 0x0045EF2C
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.difficulty == null)
			{
				if (!Prefs.DevMode)
				{
					Messages.Message("MustChooseDifficulty".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("Difficulty has been automatically selected (debug mode only)", MessageTypeDefOf.SilentInput, false);
				this.difficulty = DifficultyDefOf.Rough;
				this.difficultyValues = new Difficulty(this.difficulty);
			}
			if (!Find.GameInitData.permadeathChosen)
			{
				if (!Prefs.DevMode)
				{
					Messages.Message("MustChoosePermadeath".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("Reload anytime mode has been automatically selected (debug mode only)", MessageTypeDefOf.SilentInput, false);
				Find.GameInitData.permadeathChosen = true;
				Find.GameInitData.permadeath = false;
			}
			Current.Game.storyteller = new Storyteller(this.storyteller, this.difficulty, this.difficultyValues);
			return true;
		}

		// Token: 0x04006E9F RID: 28319
		private StorytellerDef storyteller;

		// Token: 0x04006EA0 RID: 28320
		private DifficultyDef difficulty;

		// Token: 0x04006EA1 RID: 28321
		private Difficulty difficultyValues = new Difficulty();

		// Token: 0x04006EA2 RID: 28322
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
