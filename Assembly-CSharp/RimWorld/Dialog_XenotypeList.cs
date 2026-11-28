using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CDC RID: 7388
	public abstract class Dialog_XenotypeList : Dialog_FileList
	{
		// Token: 0x0600BDE1 RID: 48609 RVA: 0x004437C4 File Offset: 0x004419C4
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCustomXenotypeFiles)
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
			CharacterCardUtility.cachedCustomXenotypes = null;
		}
	}
}
