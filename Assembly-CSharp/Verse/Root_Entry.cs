using System;
using System.IO;
using RimWorld;

namespace Verse
{
	// Token: 0x02000203 RID: 515
	public class Root_Entry : Root
	{
		// Token: 0x06000ECE RID: 3790 RVA: 0x0004C4D0 File Offset: 0x0004A6D0
		public override void Start()
		{
			base.Start();
			try
			{
				Current.Game = null;
				this.musicManagerEntry = new MusicManagerEntry();
				FileInfo fileInfo = Root.checkedAutostartSaveFile ? null : SaveGameFilesUtility.GetAutostartSaveFile();
				Root.checkedAutostartSaveFile = true;
				if (fileInfo != null)
				{
					GameDataSaveLoader.LoadGame(fileInfo);
				}
			}
			catch (Exception ex)
			{
				string str = "Critical error in root Start(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0004C544 File Offset: 0x0004A744
		public override void Update()
		{
			base.Update();
			if (LongEventHandler.ShouldWaitForEvent || this.destroyed)
			{
				return;
			}
			try
			{
				this.musicManagerEntry.MusicManagerEntryUpdate();
				if (Find.World != null)
				{
					Find.World.WorldUpdate();
				}
				if (Current.Game != null)
				{
					Current.Game.UpdateEntry();
				}
			}
			catch (Exception ex)
			{
				string str = "Root level exception in Update(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x04000E7F RID: 3711
		public MusicManagerEntry musicManagerEntry;
	}
}
