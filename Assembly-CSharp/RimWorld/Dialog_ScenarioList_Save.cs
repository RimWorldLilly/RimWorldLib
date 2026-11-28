using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD9 RID: 7385
	public class Dialog_ScenarioList_Save : Dialog_ScenarioList
	{
		// Token: 0x1700228D RID: 8845
		// (get) Token: 0x0600BDD3 RID: 48595 RVA: 0x0044305C File Offset: 0x0044125C
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600BDD4 RID: 48596 RVA: 0x00443060 File Offset: 0x00441260
		public Dialog_ScenarioList_Save(Scenario scen, Action onClosed = null)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			string name = scen.name;
			this.typingName = ((name != null) ? name.Trim() : null);
			this.savingScen = scen;
			this.onClosed = onClosed;
		}

		// Token: 0x0600BDD5 RID: 48597 RVA: 0x004430B0 File Offset: 0x004412B0
		protected override void DoFileInteraction(string fileName)
		{
			fileName = GenFile.SanitizedFileName(fileName);
			string absPath = GenFilePaths.AbsPathForScenario(fileName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveScenario(this.savingScen, absPath);
			}, "SavingLongEvent", false, null, true, false, null);
			Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, false);
			this.Close(true);
		}

		// Token: 0x0600BDD6 RID: 48598 RVA: 0x0044311E File Offset: 0x0044131E
		public override void Close(bool doCloseSound = true)
		{
			base.Close(doCloseSound);
			Action action = this.onClosed;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04006CB2 RID: 27826
		private Scenario savingScen;

		// Token: 0x04006CB3 RID: 27827
		private Action onClosed;
	}
}
