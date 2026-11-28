using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD2 RID: 7378
	public abstract class Dialog_ModList : Dialog_FileList
	{
		// Token: 0x0600BDBD RID: 48573 RVA: 0x00442B00 File Offset: 0x00440D00
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllModListFiles)
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
