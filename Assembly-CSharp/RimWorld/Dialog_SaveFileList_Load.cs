using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CD6 RID: 7382
	public class Dialog_SaveFileList_Load : Dialog_SaveFileList
	{
		// Token: 0x1700228B RID: 8843
		// (get) Token: 0x0600BDCB RID: 48587 RVA: 0x00442E8D File Offset: 0x0044108D
		protected override bool FocusSearchField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600BDCC RID: 48588 RVA: 0x00442E90 File Offset: 0x00441090
		public Dialog_SaveFileList_Load()
		{
			this.interactButLabel = "LoadGameButton".Translate();
		}

		// Token: 0x0600BDCD RID: 48589 RVA: 0x00442EAD File Offset: 0x004410AD
		protected override void DoFileInteraction(string saveFileName)
		{
			GameDataSaveLoader.CheckVersionAndLoadGame(saveFileName);
		}
	}
}
