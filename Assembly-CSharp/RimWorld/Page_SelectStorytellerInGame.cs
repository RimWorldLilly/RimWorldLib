using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D0B RID: 7435
	public class Page_SelectStorytellerInGame : Page
	{
		// Token: 0x170022EB RID: 8939
		// (get) Token: 0x0600C035 RID: 49205 RVA: 0x00460E33 File Offset: 0x0045F033
		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		// Token: 0x0600C036 RID: 49206 RVA: 0x00460E44 File Offset: 0x0045F044
		public Page_SelectStorytellerInGame()
		{
			this.doCloseButton = true;
			this.doCloseX = true;
		}

		// Token: 0x0600C037 RID: 49207 RVA: 0x00460E65 File Offset: 0x0045F065
		public override void PreOpen()
		{
			base.PreOpen();
			StorytellerUI.ResetStorytellerSelectionInterface();
		}

		// Token: 0x0600C038 RID: 49208 RVA: 0x00460E74 File Offset: 0x0045F074
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			Storyteller storyteller = Current.Game.storyteller;
			StorytellerDef def = Current.Game.storyteller.def;
			StorytellerUI.DrawStorytellerSelectionInterface(mainRect, ref storyteller.def, ref storyteller.difficultyDef, ref storyteller.difficulty, this.selectedStorytellerInfoListing);
			if (storyteller.def != def)
			{
				storyteller.Notify_DefChanged();
			}
		}

		// Token: 0x0600C039 RID: 49209 RVA: 0x00460EDC File Offset: 0x0045F0DC
		public override void PreClose()
		{
			foreach (ThingDef thingDef in from x in DefDatabase<ThingDef>.AllDefs
			where x.costListForDifficulty != null
			select x)
			{
				thingDef.costListForDifficulty.RecacheApplies();
			}
			RecipeDefGenerator.ResetRecipeIngredientsForDifficulty();
		}

		// Token: 0x04006EA3 RID: 28323
		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();
	}
}
