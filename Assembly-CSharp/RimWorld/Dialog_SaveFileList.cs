using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD5 RID: 7381
	public abstract class Dialog_SaveFileList : Dialog_FileList
	{
		// Token: 0x0600BDC5 RID: 48581 RVA: 0x00442D0C File Offset: 0x00440F0C
		protected override Color FileNameColor(SaveFileInfo sfi)
		{
			if (SaveGameFilesUtility.IsAutoSave(Path.GetFileNameWithoutExtension(sfi.FileName)))
			{
				GUI.color = Dialog_SaveFileList.AutosaveTextColor;
			}
			return base.FileNameColor(sfi);
		}

		// Token: 0x0600BDC6 RID: 48582 RVA: 0x00442D31 File Offset: 0x00440F31
		private void ReloadFilesTask()
		{
			Parallel.ForEach<SaveFileInfo>(this.files, delegate(SaveFileInfo file)
			{
				try
				{
					file.LoadData();
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Exception loading ");
					defaultInterpolatedStringHandler.AppendFormatted(file.FileInfo.Name);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			});
		}

		// Token: 0x0600BDC7 RID: 48583 RVA: 0x00442D60 File Offset: 0x00440F60
		protected override void ReloadFiles()
		{
			if (this.loadSavesTask != null && this.loadSavesTask.Status != TaskStatus.RanToCompletion)
			{
				this.loadSavesTask.Wait();
			}
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllSavedGameFiles)
			{
				try
				{
					SaveFileInfo item = new SaveFileInfo(fileInfo);
					this.files.Add(item);
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
			this.loadSavesTask = Task.Run(new Action(this.ReloadFilesTask));
		}

		// Token: 0x0600BDC8 RID: 48584 RVA: 0x00442E50 File Offset: 0x00441050
		public override void PostClose()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x04006CB0 RID: 27824
		private static readonly Color AutosaveTextColor = new Color(0.75f, 0.75f, 0.75f);

		// Token: 0x04006CB1 RID: 27825
		private Task loadSavesTask;
	}
}
