using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CC9 RID: 7369
	public class Dialog_SelectXenotypeIcon : Window
	{
		// Token: 0x1700227C RID: 8828
		// (get) Token: 0x0600BD5E RID: 48478 RVA: 0x0043F2CE File Offset: 0x0043D4CE
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(334f + this.Margin * 2f + 16f, 400f);
			}
		}

		// Token: 0x0600BD5F RID: 48479 RVA: 0x0043F2F2 File Offset: 0x0043D4F2
		public Dialog_SelectXenotypeIcon(XenotypeIconDef selected, Action<XenotypeIconDef> iconSelector) : base(null)
		{
			this.selected = selected;
			this.iconSelector = iconSelector;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x0600BD60 RID: 48480 RVA: 0x0043F310 File Offset: 0x0043D510
		public override void PostOpen()
		{
			if (!ModLister.CheckBiotech("xenotype icon"))
			{
				this.Close(false);
				return;
			}
			base.PostOpen();
		}

		// Token: 0x0600BD61 RID: 48481 RVA: 0x0043F32C File Offset: 0x0043D52C
		public override void DoWindowContents(Rect rect)
		{
			Rect rect2 = rect;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, "SelectIcon".Translate());
			Text.Font = GameFont.Small;
			rect2.yMin += 39f;
			rect2.yMax -= Window.CloseButSize.y + 4f;
			Rect outRect = rect2;
			outRect.yMax -= 4f;
			Rect rect3 = new Rect(outRect.x, outRect.y, outRect.width - 16f, this.scrollHeight);
			Widgets.DrawLightHighlight(rect3);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect3, true);
			float num = outRect.x + 6f;
			float num2 = outRect.y + 6f;
			foreach (XenotypeIconDef xenotypeIconDef in DefDatabase<XenotypeIconDef>.AllDefs)
			{
				if (num + 35f + 6f > rect3.width)
				{
					num = outRect.x + 6f;
					num2 += 41f;
					this.scrollHeight = Mathf.Max(this.scrollHeight, num2);
				}
				Rect rect4 = new Rect(num, num2, 35f, 35f);
				Widgets.DrawHighlight(rect4);
				if (this.selected == xenotypeIconDef)
				{
					GUI.color = Dialog_SelectXenotypeIcon.OutlineColorSelected;
					Widgets.DrawHighlight(rect4);
					Widgets.DrawBox(rect4.ExpandedBy(2f), 2, null);
				}
				else
				{
					GUI.color = Dialog_SelectXenotypeIcon.OutlineColorUnselected;
					Widgets.DrawBox(rect4, 1, null);
				}
				GUI.color = Color.white;
				if (Widgets.ButtonImage(rect4, xenotypeIconDef.Icon, XenotypeDef.IconColor, true, null))
				{
					this.selected = xenotypeIconDef;
				}
				num += 41f;
			}
			Widgets.EndScrollView();
			if (Widgets.ButtonText(new Rect((rect.width - Window.CloseButSize.x) / 2f, rect2.yMax, Window.CloseButSize.x, Window.CloseButSize.y), "Accept".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
		}

		// Token: 0x0600BD62 RID: 48482 RVA: 0x0043F574 File Offset: 0x0043D774
		public override void PreClose()
		{
			this.iconSelector(this.selected);
		}

		// Token: 0x04006C50 RID: 27728
		private Vector2 scrollPosition;

		// Token: 0x04006C51 RID: 27729
		private float scrollHeight;

		// Token: 0x04006C52 RID: 27730
		private XenotypeIconDef selected;

		// Token: 0x04006C53 RID: 27731
		private Action<XenotypeIconDef> iconSelector;

		// Token: 0x04006C54 RID: 27732
		private const float HeaderHeight = 35f;

		// Token: 0x04006C55 RID: 27733
		private const float IconSize = 35f;

		// Token: 0x04006C56 RID: 27734
		private const float IconGap = 6f;

		// Token: 0x04006C57 RID: 27735
		private const int IconsPerRow = 8;

		// Token: 0x04006C58 RID: 27736
		private static readonly Color OutlineColorSelected = new Color(1f, 1f, 0.7f, 1f);

		// Token: 0x04006C59 RID: 27737
		private static readonly Color OutlineColorUnselected = new Color(1f, 1f, 1f, 0.1f);
	}
}
