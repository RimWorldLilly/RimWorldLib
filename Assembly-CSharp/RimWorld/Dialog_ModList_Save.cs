using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD3 RID: 7379
	public class Dialog_ModList_Save : Dialog_ModList
	{
		// Token: 0x1700228A RID: 8842
		// (get) Token: 0x0600BDBF RID: 48575 RVA: 0x00442BA4 File Offset: 0x00440DA4
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600BDC0 RID: 48576 RVA: 0x00442BA7 File Offset: 0x00440DA7
		public Dialog_ModList_Save(ModList modList, Action onClosed = null)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.savingModList = modList;
			this.onClosed = onClosed;
		}

		// Token: 0x0600BDC1 RID: 48577 RVA: 0x00442BD4 File Offset: 0x00440DD4
		protected override void DoFileInteraction(string fileName)
		{
			fileName = GenFile.SanitizedFileName(fileName);
			string absPath = GenFilePaths.AbsFilePathForModList(fileName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveModList(this.savingModList, absPath);
			}, "SavingLongEvent", false, null, true, false, null);
			Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, false);
			this.Close(true);
		}

		// Token: 0x0600BDC2 RID: 48578 RVA: 0x00442C42 File Offset: 0x00440E42
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

		// Token: 0x04006CAD RID: 27821
		private ModList savingModList;

		// Token: 0x04006CAE RID: 27822
		private Action onClosed;
	}
}
