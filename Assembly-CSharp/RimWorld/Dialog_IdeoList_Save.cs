using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD0 RID: 7376
	public class Dialog_IdeoList_Save : Dialog_IdeoList
	{
		// Token: 0x17002289 RID: 8841
		// (get) Token: 0x0600BDB7 RID: 48567 RVA: 0x004429E0 File Offset: 0x00440BE0
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600BDB8 RID: 48568 RVA: 0x004429E3 File Offset: 0x00440BE3
		public Dialog_IdeoList_Save(Ideo ideo)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.typingName = ideo.name;
			this.savingIdeo = ideo;
		}

		// Token: 0x0600BDB9 RID: 48569 RVA: 0x00442A14 File Offset: 0x00440C14
		protected override void DoFileInteraction(string fileName)
		{
			fileName = GenFile.SanitizedFileName(fileName);
			string absPath = GenFilePaths.AbsPathForIdeo(fileName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveIdeo(this.savingIdeo, absPath);
			}, "SavingLongEvent", false, null, true, false, null);
			Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, false);
			this.Close(true);
		}

		// Token: 0x04006CA9 RID: 27817
		private Ideo savingIdeo;
	}
}
