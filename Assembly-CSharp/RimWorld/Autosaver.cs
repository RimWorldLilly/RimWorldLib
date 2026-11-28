using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Profile;

namespace RimWorld
{
	// Token: 0x02001E2B RID: 7723
	public sealed class Autosaver
	{
		// Token: 0x1700250C RID: 9484
		// (get) Token: 0x0600C8AB RID: 51371 RVA: 0x00499D90 File Offset: 0x00497F90
		private float AutosaveIntervalDays
		{
			get
			{
				float num = Prefs.AutosaveIntervalDays;
				if (Current.Game.Info.permadeathMode && num > 1f)
				{
					num = 1f;
				}
				return num;
			}
		}

		// Token: 0x1700250D RID: 9485
		// (get) Token: 0x0600C8AC RID: 51372 RVA: 0x00499DC3 File Offset: 0x00497FC3
		private int AutosaveIntervalTicks
		{
			get
			{
				return Mathf.RoundToInt(this.AutosaveIntervalDays * 60000f);
			}
		}

		// Token: 0x0600C8AD RID: 51373 RVA: 0x00499DD8 File Offset: 0x00497FD8
		public void AutosaverTick()
		{
			this.ticksSinceSave++;
			if (this.ticksSinceSave >= this.AutosaveIntervalTicks && !GameDataSaveLoader.SavingIsTemporarilyDisabled)
			{
				LongEventHandler.QueueLongEvent(new Action(this.DoAutosave), "Autosaving", false, null, true, false, null);
				this.ticksSinceSave = 0;
			}
		}

		// Token: 0x0600C8AE RID: 51374 RVA: 0x00499E2C File Offset: 0x0049802C
		public void DoAutosave()
		{
			string fileName;
			if (Current.Game.Info.permadeathMode)
			{
				fileName = Current.Game.Info.permadeathModeUniqueName;
			}
			else
			{
				fileName = this.NewAutosaveFileName();
			}
			GameDataSaveLoader.SaveGame(fileName);
		}

		// Token: 0x0600C8AF RID: 51375 RVA: 0x00499E69 File Offset: 0x00498069
		private void DoMemoryCleanup()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		// Token: 0x0600C8B0 RID: 51376 RVA: 0x00499E70 File Offset: 0x00498070
		private string NewAutosaveFileName()
		{
			string text = this.AutoSaveNames().FirstOrDefault((string name) => !SaveGameFilesUtility.SavedGameNamedExists(name));
			if (text != null)
			{
				return text;
			}
			return this.AutoSaveNames().MinBy((string name) => new FileInfo(GenFilePaths.FilePathForSavedGame(name)).LastWriteTime);
		}

		// Token: 0x0600C8B1 RID: 51377 RVA: 0x00499ED7 File Offset: 0x004980D7
		private IEnumerable<string> AutoSaveNames()
		{
			return new Autosaver.<AutoSaveNames>d__10(-2);
		}

		// Token: 0x04007254 RID: 29268
		private int ticksSinceSave;

		// Token: 0x04007255 RID: 29269
		public const float MaxPermadeathModeAutosaveInterval = 1f;
	}
}
