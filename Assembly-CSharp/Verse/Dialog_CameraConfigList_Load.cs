using System;

namespace Verse
{
	// Token: 0x020000FE RID: 254
	public class Dialog_CameraConfigList_Load : Dialog_CameraConfigList
	{
		// Token: 0x060008F6 RID: 2294 RVA: 0x0003385E File Offset: 0x00031A5E
		public Dialog_CameraConfigList_Load(Action<CameraMapConfig> cfgReturner)
		{
			this.configReturner = cfgReturner;
			this.interactButLabel = "Load config";
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00033878 File Offset: 0x00031A78
		protected override void DoFileInteraction(string fileName)
		{
			string filePath = GenFilePaths.AbsFilePathForCameraConfig(fileName);
			PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.CameraConfig, delegate
			{
				CameraMapConfig obj;
				if (GameDataSaveLoader.TryLoadCameraConfig(filePath, out obj))
				{
					this.configReturner(obj);
				}
				this.Close(true);
			}, false);
		}

		// Token: 0x04000428 RID: 1064
		private Action<CameraMapConfig> configReturner;
	}
}
