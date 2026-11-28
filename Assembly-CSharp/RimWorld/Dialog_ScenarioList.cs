using System;
using System.IO;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD8 RID: 7384
	public abstract class Dialog_ScenarioList : Dialog_FileList
	{
		// Token: 0x0600BDD1 RID: 48593 RVA: 0x00442F94 File Offset: 0x00441194
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCustomScenarioFiles)
			{
				try
				{
					SaveFileInfo saveFileInfo = new SaveFileInfo(fileInfo);
					saveFileInfo.LoadData();
					this.files.Add(saveFileInfo);
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Exception loading ");
					defaultInterpolatedStringHandler.AppendFormatted(fileInfo.Name);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}
	}
}
