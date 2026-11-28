using System;
using System.IO;
using RimWorld;

namespace Verse
{
	// Token: 0x020000FC RID: 252
	public abstract class Dialog_CameraConfigList : Dialog_FileList
	{
		// Token: 0x060008F1 RID: 2289 RVA: 0x00033724 File Offset: 0x00031924
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCameraConfigFiles)
			{
				try
				{
					SaveFileInfo saveFileInfo = new SaveFileInfo(fileInfo);
					saveFileInfo.LoadData();
					this.files.Add(saveFileInfo);
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading " + fileInfo.Name + ": " + ex.ToString());
				}
			}
		}
	}
}
