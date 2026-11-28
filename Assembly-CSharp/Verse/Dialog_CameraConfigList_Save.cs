using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000FD RID: 253
	public class Dialog_CameraConfigList_Save : Dialog_CameraConfigList
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x000337C8 File Offset: 0x000319C8
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x000337CB File Offset: 0x000319CB
		public Dialog_CameraConfigList_Save(CameraMapConfig config)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.config = config;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x000337F0 File Offset: 0x000319F0
		protected override void DoFileInteraction(string fileName)
		{
			fileName = GenFile.SanitizedFileName(fileName);
			string absPath = GenFilePaths.AbsFilePathForCameraConfig(fileName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveCameraConfig(this.config, absPath);
			}, "SavingLongEvent", false, null, true, false, null);
			Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, false);
			this.Close(true);
		}

		// Token: 0x04000427 RID: 1063
		private CameraMapConfig config;
	}
}
