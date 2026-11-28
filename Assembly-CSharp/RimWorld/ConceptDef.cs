using System;
using System.Collections.Generic;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x0200100E RID: 4110
	public class ConceptDef : Def
	{
		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x06006798 RID: 26520 RVA: 0x0024A0D3 File Offset: 0x002482D3
		public bool TriggeredDirect
		{
			get
			{
				return this.priority <= 0f;
			}
		}

		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x06006799 RID: 26521 RVA: 0x0024A0E5 File Offset: 0x002482E5
		public string HelpTextAdjusted
		{
			get
			{
				if (!SteamDeck.IsSteamDeckInNonKeyboardMode || this.helpTextController.NullOrEmpty())
				{
					return this.helpText.AdjustedForKeys(null, true);
				}
				return this.helpTextController.AdjustedForKeys(null, true);
			}
		}

		// Token: 0x0600679A RID: 26522 RVA: 0x0024A116 File Offset: 0x00248316
		public override IEnumerable<string> ConfigErrors()
		{
			ConceptDef.<ConfigErrors>d__13 <ConfigErrors>d__ = new ConceptDef.<ConfigErrors>d__13(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600679B RID: 26523 RVA: 0x0024A126 File Offset: 0x00248326
		public static ConceptDef Named(string defName)
		{
			return DefDatabase<ConceptDef>.GetNamed(defName, true);
		}

		// Token: 0x0600679C RID: 26524 RVA: 0x0024A130 File Offset: 0x00248330
		public void HighlightAllTags()
		{
			if (this.highlightTags != null)
			{
				for (int i = 0; i < this.highlightTags.Count; i++)
				{
					UIHighlighter.HighlightTag(this.highlightTags[i]);
				}
			}
		}

		// Token: 0x040041C1 RID: 16833
		public float priority = float.MaxValue;

		// Token: 0x040041C2 RID: 16834
		public bool noteTeaches;

		// Token: 0x040041C3 RID: 16835
		public bool needsOpportunity;

		// Token: 0x040041C4 RID: 16836
		public bool opportunityDecays = true;

		// Token: 0x040041C5 RID: 16837
		public ProgramState gameMode = ProgramState.Playing;

		// Token: 0x040041C6 RID: 16838
		[MustTranslate]
		private string helpText;

		// Token: 0x040041C7 RID: 16839
		[MustTranslate]
		private string helpTextController;

		// Token: 0x040041C8 RID: 16840
		[NoTranslate]
		public List<string> highlightTags;

		// Token: 0x040041C9 RID: 16841
		private static List<string> tmpParseErrors = new List<string>();
	}
}
