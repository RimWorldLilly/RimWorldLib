using System;
using System.Collections.Generic;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x0200100F RID: 4111
	public class InstructionDef : Def
	{
		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x060067A0 RID: 26528 RVA: 0x0024A1A1 File Offset: 0x002483A1
		public string Text
		{
			get
			{
				if (!SteamDeck.IsSteamDeckInNonKeyboardMode || this.textController.NullOrEmpty())
				{
					return this.text;
				}
				return this.textController;
			}
		}

		// Token: 0x060067A1 RID: 26529 RVA: 0x0024A1C4 File Offset: 0x002483C4
		public override IEnumerable<string> ConfigErrors()
		{
			InstructionDef.<ConfigErrors>d__24 <ConfigErrors>d__ = new InstructionDef.<ConfigErrors>d__24(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040041CA RID: 16842
		public Type instructionClass = typeof(Instruction_Basic);

		// Token: 0x040041CB RID: 16843
		[MustTranslate]
		public string text;

		// Token: 0x040041CC RID: 16844
		[MustTranslate]
		public string textController;

		// Token: 0x040041CD RID: 16845
		public bool startCentered;

		// Token: 0x040041CE RID: 16846
		public bool tutorialModeOnly = true;

		// Token: 0x040041CF RID: 16847
		[NoTranslate]
		public string eventTagInitiate;

		// Token: 0x040041D0 RID: 16848
		public InstructionDef eventTagInitiateSource;

		// Token: 0x040041D1 RID: 16849
		[NoTranslate]
		public List<string> eventTagsEnd;

		// Token: 0x040041D2 RID: 16850
		[NoTranslate]
		public List<string> actionTagsAllowed;

		// Token: 0x040041D3 RID: 16851
		[MustTranslate]
		public string rejectInputMessage;

		// Token: 0x040041D4 RID: 16852
		public ConceptDef concept;

		// Token: 0x040041D5 RID: 16853
		[NoTranslate]
		public List<string> highlightTags;

		// Token: 0x040041D6 RID: 16854
		[MustTranslate]
		public string onMapInstruction;

		// Token: 0x040041D7 RID: 16855
		public int targetCount;

		// Token: 0x040041D8 RID: 16856
		public ThingDef thingDef;

		// Token: 0x040041D9 RID: 16857
		public RecipeDef recipeDef;

		// Token: 0x040041DA RID: 16858
		public int recipeTargetCount = 1;

		// Token: 0x040041DB RID: 16859
		public ThingDef giveOnActivateDef;

		// Token: 0x040041DC RID: 16860
		public int giveOnActivateCount;

		// Token: 0x040041DD RID: 16861
		public bool endTutorial;

		// Token: 0x040041DE RID: 16862
		public bool resetBuildDesignatorStuffs;

		// Token: 0x040041DF RID: 16863
		private static List<string> tmpParseErrors = new List<string>();
	}
}
