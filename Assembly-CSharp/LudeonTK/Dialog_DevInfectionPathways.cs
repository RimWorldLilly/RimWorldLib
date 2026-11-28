using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000046 RID: 70
	[StaticConstructorOnStartup]
	public class Dialog_DevInfectionPathways : Window_Dev
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000D1AA File Offset: 0x0000B3AA
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000D1AD File Offset: 0x0000B3AD
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000D1B4 File Offset: 0x0000B3B4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(230f, 330f);
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000D1C8 File Offset: 0x0000B3C8
		public Dialog_DevInfectionPathways()
		{
			this.draggable = true;
			this.focusWhenOpened = false;
			this.drawShadow = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
			this.drawInScreenshotMode = false;
			this.windowPosition = Prefs.DevPalettePosition;
			this.onlyDrawInDevMode = true;
			this.doCloseX = true;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000D228 File Offset: 0x0000B428
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect rect = new Rect(inRect.x, inRect.y, inRect.width, 24f);
			Rect rect2 = new Rect(inRect.x, inRect.height / 2f - 12f, inRect.width, 24f);
			DevGUI.Label(rect, "Infection Pathway Debugger");
			Text.Font = GameFont.Tiny;
			List<Pawn> selectedPawns = Find.Selector.SelectedPawns;
			if (selectedPawns.Count == 0 || !selectedPawns[0].RaceProps.Humanlike)
			{
				DevGUI.Label(rect2, "No valid humanlike selected");
				return;
			}
			Pawn pawn = Find.Selector.SelectedPawns[0];
			if (pawn.infectionVectors.PathwaysCount == 0)
			{
				DevGUI.Label(rect2, "No vectors");
				return;
			}
			Rect outRect = inRect;
			outRect.yMin = rect.height + 6f;
			Rect rect3 = inRect;
			rect3.y = 0f;
			rect3.height = this.lastHeight;
			Widgets.BeginScrollView(outRect, ref this.scroll, rect3, true);
			float num = 0f;
			int num2 = 0;
			this.PrintLabel("Def name", "Age", 1, rect3, ref num);
			foreach (InfectionPathway pathway in pawn.infectionVectors.Pathways)
			{
				this.PrintLabel(pathway, num2++, rect3, ref num);
			}
			this.lastHeight = num;
			Widgets.EndScrollView();
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000D3BC File Offset: 0x0000B5BC
		private void PrintLabel(InfectionPathway pathway, int row, Rect container, ref float y)
		{
			this.PrintLabel(pathway.Def.label, pathway.AgeTicks.ToStringSecondsFromTicks(), row, container, ref y);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000D3E0 File Offset: 0x0000B5E0
		private void PrintLabel(string key, string value, int row, Rect container, ref float y)
		{
			Rect rect = new Rect(container.x, y, container.width, 20f);
			float num = container.width * 0.6f;
			Rect rect2 = new Rect(container.x, y, num, 20f);
			Rect rect3 = new Rect(container.x + num, y, num, 20f);
			if (row % 2 == 0)
			{
				DevGUI.DrawLightHighlight(rect);
			}
			DevGUI.Label(rect2, key);
			DevGUI.Label(rect3, value);
			y += 20f;
		}

		// Token: 0x040000DE RID: 222
		private Vector2 windowPosition;

		// Token: 0x040000DF RID: 223
		private Vector2 scroll;

		// Token: 0x040000E0 RID: 224
		private float lastHeight;

		// Token: 0x040000E1 RID: 225
		private const string Title = "Infection Pathway Debugger";

		// Token: 0x040000E2 RID: 226
		private const float ButtonHeight = 30f;
	}
}
