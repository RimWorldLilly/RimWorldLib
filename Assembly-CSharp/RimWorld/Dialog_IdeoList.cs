using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CCF RID: 7375
	public abstract class Dialog_IdeoList : Dialog_FileList
	{
		// Token: 0x0600BDB5 RID: 48565 RVA: 0x0044293C File Offset: 0x00440B3C
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCustomIdeoFiles)
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
