using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CBA RID: 7354
	public class Dialog_ModSettings : Window
	{
		// Token: 0x17002263 RID: 8803
		// (get) Token: 0x0600BCE2 RID: 48354 RVA: 0x00439DEA File Offset: 0x00437FEA
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

		// Token: 0x0600BCE3 RID: 48355 RVA: 0x00439DFB File Offset: 0x00437FFB
		public Dialog_ModSettings(Mod mod) : base(null)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.mod = mod;
		}

		// Token: 0x0600BCE4 RID: 48356 RVA: 0x00439E2E File Offset: 0x0043802E
		public override void PreClose()
		{
			base.PreClose();
			this.mod.WriteSettings();
		}

		// Token: 0x0600BCE5 RID: 48357 RVA: 0x00439E44 File Offset: 0x00438044
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, inRect.width - 150f - 17f, 35f), this.mod.SettingsCategory());
			Text.Font = GameFont.Small;
			Rect inRect2 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - Window.CloseButSize.y);
			this.mod.DoSettingsWindowContents(inRect2);
		}

		// Token: 0x04006BFC RID: 27644
		private Mod mod;

		// Token: 0x04006BFD RID: 27645
		private const float TopAreaHeight = 40f;

		// Token: 0x04006BFE RID: 27646
		private const float TopButtonHeight = 35f;

		// Token: 0x04006BFF RID: 27647
		private const float TopButtonWidth = 150f;
	}
}
