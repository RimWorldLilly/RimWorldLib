using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD4 RID: 7380
	public class Dialog_ModList_Load : Dialog_ModList
	{
		// Token: 0x0600BDC3 RID: 48579 RVA: 0x00442C5B File Offset: 0x00440E5B
		public Dialog_ModList_Load(Action<ModList> modListReturner)
		{
			this.interactButLabel = "Load".Translate();
			this.modListReturner = modListReturner;
		}

		// Token: 0x0600BDC4 RID: 48580 RVA: 0x00442C80 File Offset: 0x00440E80
		protected override void DoFileInteraction(string fileName)
		{
			string text = GenFilePaths.AbsFilePathForModList(fileName);
			try
			{
				Scribe.loader.InitLoadingMetaHeaderOnly(text);
				ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.ModList, false);
				Scribe.loader.FinalizeLoading();
				ModList obj = null;
				if (GameDataSaveLoader.TryLoadModList(text, out obj))
				{
					this.modListReturner(obj);
				}
				this.Close(true);
			}
			catch (Exception ex)
			{
				string str = "Exception loading ";
				string str2 = text;
				string str3 = ": ";
				Exception ex2 = ex;
				Log.Warning(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				Scribe.ForceStop();
			}
		}

		// Token: 0x04006CAF RID: 27823
		private Action<ModList> modListReturner;
	}
}
