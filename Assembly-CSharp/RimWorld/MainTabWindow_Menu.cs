using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D9D RID: 7581
	public class MainTabWindow_Menu : MainTabWindow
	{
		// Token: 0x17002495 RID: 9365
		// (get) Token: 0x0600C4C1 RID: 50369 RVA: 0x00480B6E File Offset: 0x0047ED6E
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(450f, 390f);
			}
		}

		// Token: 0x17002496 RID: 9366
		// (get) Token: 0x0600C4C2 RID: 50370 RVA: 0x00480B7F File Offset: 0x0047ED7F
		public override MainTabWindowAnchor Anchor
		{
			get
			{
				return MainTabWindowAnchor.Right;
			}
		}

		// Token: 0x0600C4C3 RID: 50371 RVA: 0x00480B82 File Offset: 0x0047ED82
		public MainTabWindow_Menu()
		{
			this.forcePause = true;
			this.layer = WindowLayer.Super;
		}

		// Token: 0x0600C4C4 RID: 50372 RVA: 0x00480B98 File Offset: 0x0047ED98
		public override void PreOpen()
		{
			base.PreOpen();
			PlayerKnowledgeDatabase.Save();
			ShipCountdown.CancelCountdown();
			if (ModsConfig.IdeologyActive)
			{
				ArchonexusCountdown.CancelCountdown();
			}
			this.anyGameFiles = GenFilePaths.AllSavedGameFiles.Any<FileInfo>();
		}

		// Token: 0x0600C4C5 RID: 50373 RVA: 0x00480BC6 File Offset: 0x0047EDC6
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			VersionControl.DrawInfoInCorner();
		}

		// Token: 0x0600C4C6 RID: 50374 RVA: 0x00480BD3 File Offset: 0x0047EDD3
		public override void DoWindowContents(Rect rect)
		{
			MainMenuDrawer.DoMainMenuControls(rect, this.anyGameFiles);
		}

		// Token: 0x04007067 RID: 28775
		private bool anyGameFiles;
	}
}
