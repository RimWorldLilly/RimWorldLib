using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CCD RID: 7373
	public class Dialog_XenogermList_Load : Window
	{
		// Token: 0x0600BDA7 RID: 48551 RVA: 0x00441F49 File Offset: 0x00440149
		public Dialog_XenogermList_Load(Action<CustomXenogerm> loadAction) : base(null)
		{
			this.doCloseButton = true;
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.loadAction = loadAction;
		}

		// Token: 0x0600BDA8 RID: 48552 RVA: 0x00441F88 File Offset: 0x00440188
		public override void DoWindowContents(Rect inRect)
		{
			List<CustomXenogerm> customXenogermsForReading = Find.CustomXenogermDatabase.CustomXenogermsForReading;
			Vector2 vector = new Vector2(inRect.width - 16f, 40f);
			float y = vector.y;
			float height = (float)customXenogermsForReading.Count * y;
			Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, height);
			float height2 = inRect.height - Window.CloseButSize.y - this.bottomAreaHeight - 18f;
			Rect outRect = inRect.TopPartPixels(height2);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			for (int i = customXenogermsForReading.Count - 1; i >= 0; i--)
			{
				CustomXenogerm xenogerm = customXenogermsForReading[i];
				if (num + vector.y >= this.scrollPosition.y && num <= this.scrollPosition.y + outRect.height)
				{
					Rect rect = new Rect(0f, num, vector.x, vector.y);
					if (i % 2 == 0)
					{
						Widgets.DrawAltRect(rect);
					}
					Widgets.BeginGroup(rect);
					Rect rect2 = new Rect(rect.width - 36f, (rect.height - 36f) / 2f, 36f, 36f);
					if (Widgets.ButtonImage(rect2, TexButton.Delete, Color.white, GenUI.SubtleMouseoverColor, true, null))
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDelete".Translate(xenogerm.name), delegate()
						{
							Find.CustomXenogermDatabase.Remove(xenogerm);
						}, true, null, WindowLayer.Dialog));
					}
					TooltipHandler.TipRegionByKey(rect2, "DeleteThisXenogerm");
					Text.Font = GameFont.Small;
					if (Widgets.ButtonText(new Rect(rect2.x - 100f, (rect.height - 36f) / 2f, 100f, 36f), "LoadGameButton".Translate(), true, true, true, null))
					{
						this.loadAction(xenogerm);
						this.Close(true);
					}
					GUI.color = Color.white;
					Rect rect3 = new Rect(8f, 0f, rect.width - 4f, rect.height);
					Text.Anchor = TextAnchor.MiddleLeft;
					Text.Font = GameFont.Small;
					Widgets.Label(rect3, xenogerm.name.Truncate(rect3.width * 1.8f, null));
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.EndGroup();
				}
				num += vector.y;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x04006C8B RID: 27787
		protected float bottomAreaHeight;

		// Token: 0x04006C8C RID: 27788
		protected Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04006C8D RID: 27789
		private Action<CustomXenogerm> loadAction;

		// Token: 0x04006C8E RID: 27790
		protected const float EntryHeight = 40f;

		// Token: 0x04006C8F RID: 27791
		protected const float NameLeftMargin = 8f;

		// Token: 0x04006C90 RID: 27792
		protected const float NameRightMargin = 4f;

		// Token: 0x04006C91 RID: 27793
		protected const float InfoWidth = 94f;

		// Token: 0x04006C92 RID: 27794
		protected const float InteractButWidth = 100f;

		// Token: 0x04006C93 RID: 27795
		protected const float InteractButHeight = 36f;

		// Token: 0x04006C94 RID: 27796
		protected const float DeleteButSize = 36f;
	}
}
