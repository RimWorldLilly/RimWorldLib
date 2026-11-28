using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001C9B RID: 7323
	public class Dialog_ChooseIdeoSymbols : Window
	{
		// Token: 0x17002202 RID: 8706
		// (get) Token: 0x0600BB38 RID: 47928 RVA: 0x00423A9C File Offset: 0x00421C9C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(740f, 700f);
			}
		}

		// Token: 0x17002203 RID: 8707
		// (get) Token: 0x0600BB39 RID: 47929 RVA: 0x00423AB0 File Offset: 0x00421CB0
		private static List<ColorDef> IdeoColorsSorted
		{
			get
			{
				if (Dialog_ChooseIdeoSymbols.allColors == null)
				{
					Dialog_ChooseIdeoSymbols.allColors = new List<ColorDef>();
					Dialog_ChooseIdeoSymbols.allColors.AddRange(from x in DefDatabase<ColorDef>.AllDefsListForReading
					where x.colorType == ColorType.Ideo
					select x);
					Dialog_ChooseIdeoSymbols.allColors.SortByColor((ColorDef x) => x.color);
				}
				return Dialog_ChooseIdeoSymbols.allColors;
			}
		}

		// Token: 0x0600BB3A RID: 47930 RVA: 0x00423B30 File Offset: 0x00421D30
		public Dialog_ChooseIdeoSymbols(Ideo ideo) : base(null)
		{
			this.ideo = ideo;
			this.absorbInputAroundWindow = true;
			this.newName = ideo.name;
			this.newAdjective = ideo.adjective;
			this.newMemberName = ideo.memberName;
			this.newWorshipRoomLabel = ideo.WorshipRoomLabel;
			this.newIconDef = ideo.iconDef;
			this.newColorDef = ideo.colorDef;
		}

		// Token: 0x0600BB3B RID: 47931 RVA: 0x00423B9A File Offset: 0x00421D9A
		public override void OnAcceptKeyPressed()
		{
			this.TryAccept();
			Event.current.Use();
		}

		// Token: 0x0600BB3C RID: 47932 RVA: 0x00423BAC File Offset: 0x00421DAC
		public override void DoWindowContents(Rect rect)
		{
			Rect rect2 = rect;
			rect2.height -= Window.CloseButSize.y;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect2.x, rect2.y, rect.width, 35f), "EditSymbols".Translate());
			Text.Font = GameFont.Small;
			rect2.yMin += 45f;
			float num = rect2.y;
			float num2 = rect2.x + rect2.width / 3f;
			float width = rect2.xMax - num2 - Dialog_ChooseIdeoSymbols.ResetButtonWidth - 10f;
			float num3 = num;
			Widgets.Label(rect2.x, ref num3, rect2.width, "Name".Translate(), default(TipSignal));
			this.newName = Widgets.TextField(new Rect(num2, num, width, Dialog_ChooseIdeoSymbols.EditFieldHeight), this.newName, 40, Dialog_ChooseIdeoSymbols.ValidSymbolRegex);
			num += Dialog_ChooseIdeoSymbols.EditFieldHeight + 10f;
			float num4 = num;
			Widgets.Label(rect2.x, ref num4, rect2.width, "Adjective".Translate(), default(TipSignal));
			this.newAdjective = Widgets.TextField(new Rect(num2, num, width, Dialog_ChooseIdeoSymbols.EditFieldHeight), this.newAdjective, 40, Dialog_ChooseIdeoSymbols.ValidSymbolRegex);
			num += Dialog_ChooseIdeoSymbols.EditFieldHeight + 10f;
			float num5 = num;
			Widgets.Label(rect2.x, ref num5, rect2.width, "IdeoMembers".Translate(), default(TipSignal));
			this.newMemberName = Widgets.TextField(new Rect(num2, num, width, Dialog_ChooseIdeoSymbols.EditFieldHeight), this.newMemberName, 40, Dialog_ChooseIdeoSymbols.ValidSymbolRegex);
			num += Dialog_ChooseIdeoSymbols.EditFieldHeight + 10f;
			float num6 = num;
			Widgets.Label(rect2.x, ref num6, rect2.width, "WorshipRoom".Translate(), default(TipSignal));
			Rect rect3 = new Rect(num2, num, width, Dialog_ChooseIdeoSymbols.EditFieldHeight);
			Rect rect4 = new Rect(rect3.xMax + 10f, num, Dialog_ChooseIdeoSymbols.ResetButtonWidth, Dialog_ChooseIdeoSymbols.EditFieldHeight);
			this.newWorshipRoomLabel = Widgets.TextField(rect3, this.newWorshipRoomLabel, 40, Dialog_ChooseIdeoSymbols.ValidSymbolRegex);
			if (Widgets.ButtonText(rect4, "Reset".Translate(), true, true, true, null))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				this.ideo.WorshipRoomLabel = null;
				this.newWorshipRoomLabel = this.ideo.WorshipRoomLabel;
			}
			num += Dialog_ChooseIdeoSymbols.EditFieldHeight + 10f;
			Rect mainRect = rect2;
			mainRect.yMax -= 4f;
			Widgets.Label(mainRect.x, ref num, mainRect.width, "Icon".Translate(), default(TipSignal));
			mainRect.yMin = num;
			this.DoColorSelector(mainRect, ref num);
			mainRect.yMin = num;
			this.DoIconSelector(mainRect);
			if (Widgets.ButtonText(new Rect(0f, rect.height - Dialog_ChooseIdeoSymbols.ButSize.y, Dialog_ChooseIdeoSymbols.ButSize.x, Dialog_ChooseIdeoSymbols.ButSize.y), "Back".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
			if (Widgets.ButtonText(new Rect(rect.width - Dialog_ChooseIdeoSymbols.ButSize.x, rect.height - Dialog_ChooseIdeoSymbols.ButSize.y, Dialog_ChooseIdeoSymbols.ButSize.x, Dialog_ChooseIdeoSymbols.ButSize.y), "DoneButton".Translate(), true, true, true, null))
			{
				this.TryAccept();
			}
		}

		// Token: 0x0600BB3D RID: 47933 RVA: 0x00423F78 File Offset: 0x00422178
		private void DoIconSelector(Rect mainRect)
		{
			int num = 50;
			Rect viewRect = new Rect(0f, 0f, mainRect.width - 16f, this.viewHeight);
			Widgets.BeginScrollView(mainRect, ref this.scrollPos, viewRect, true);
			IEnumerable<IdeoIconDef> allDefs = DefDatabase<IdeoIconDef>.AllDefs;
			int num2 = Mathf.FloorToInt(viewRect.width / (float)(num + 5));
			int num3 = allDefs.Count<IdeoIconDef>();
			int num4 = 0;
			foreach (IdeoIconDef ideoIconDef in allDefs)
			{
				int num5 = num4 / num2;
				int num6 = num4 % num2;
				int num7 = (num4 >= num3 - num3 % num2) ? (num3 % num2) : num2;
				float num8 = (viewRect.width - (float)(num7 * num) - (float)((num7 - 1) * 5)) / 2f;
				Rect rect = new Rect(num8 + (float)(num6 * num) + (float)(num6 * 5), (float)(num5 * num + num5 * 5), (float)num, (float)num);
				Widgets.DrawLightHighlight(rect);
				Widgets.DrawHighlightIfMouseover(rect);
				if (ideoIconDef == this.newIconDef)
				{
					Widgets.DrawBox(rect, 1, null);
				}
				GUI.color = this.newColorDef.color;
				GUI.DrawTexture(new Rect(rect.x + 5f, rect.y + 5f, 40f, 40f), ideoIconDef.Icon);
				GUI.color = Color.white;
				if (Widgets.ButtonInvisible(rect, true))
				{
					this.newIconDef = ideoIconDef;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				this.viewHeight = Mathf.Max(this.viewHeight, rect.yMax);
				num4++;
			}
			GUI.color = Color.white;
			Widgets.EndScrollView();
		}

		// Token: 0x0600BB3E RID: 47934 RVA: 0x00424140 File Offset: 0x00422340
		private void DoColorSelector(Rect mainRect, ref float curY)
		{
			int num = 26;
			float num2 = 98f;
			int num3 = Mathf.FloorToInt((mainRect.width - num2) / (float)(num + 2));
			int num4 = Mathf.CeilToInt((float)Dialog_ChooseIdeoSymbols.IdeoColorsSorted.Count / (float)num3);
			Widgets.BeginGroup(mainRect);
			GUI.color = this.newColorDef.color;
			GUI.DrawTexture(new Rect(5f, 5f, 88f, 88f), this.newIconDef.Icon);
			GUI.color = Color.white;
			curY += num2;
			int num5 = 0;
			foreach (ColorDef colorDef in Dialog_ChooseIdeoSymbols.IdeoColorsSorted)
			{
				int num6 = num5 / num3;
				int num7 = num5 % num3;
				float num8 = (num2 - (float)(num * num4) - 2f) / 2f;
				Rect rect = new Rect(num2 + (float)(num7 * num) + (float)(num7 * 2), num8 + (float)(num6 * num) + (float)(num6 * 2), (float)num, (float)num);
				Widgets.DrawLightHighlight(rect);
				Widgets.DrawHighlightIfMouseover(rect);
				if (this.newColorDef == colorDef)
				{
					Widgets.DrawBox(rect, 1, null);
				}
				Widgets.DrawBoxSolid(new Rect(rect.x + 2f, rect.y + 2f, 22f, 22f), colorDef.color);
				if (Widgets.ButtonInvisible(rect, true))
				{
					this.newColorDef = colorDef;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				curY = Mathf.Max(curY, mainRect.yMin + rect.yMax);
				num5++;
			}
			Widgets.EndGroup();
			curY += 4f;
		}

		// Token: 0x0600BB3F RID: 47935 RVA: 0x004242FC File Offset: 0x004224FC
		private void TryAccept()
		{
			if (!this.newName.NullOrEmpty())
			{
				this.newName = this.newName.Trim();
			}
			if (!this.newAdjective.NullOrEmpty())
			{
				this.newAdjective = this.newAdjective.Trim();
			}
			if (!this.newMemberName.NullOrEmpty())
			{
				this.newMemberName = this.newMemberName.Trim();
			}
			if (!this.newWorshipRoomLabel.NullOrEmpty())
			{
				this.newWorshipRoomLabel = this.newWorshipRoomLabel.Trim();
			}
			bool flag = this.ideo.name != this.newName || this.ideo.adjective != this.newAdjective || this.ideo.memberName != this.newMemberName;
			if (!this.newName.NullOrEmpty())
			{
				this.ideo.name = this.newName;
			}
			if (!this.newAdjective.NullOrEmpty())
			{
				this.ideo.adjective = this.newAdjective;
			}
			if (!this.newMemberName.NullOrEmpty())
			{
				this.ideo.memberName = this.newMemberName;
			}
			if (this.ideo.WorshipRoomLabel != this.newWorshipRoomLabel && !this.newWorshipRoomLabel.NullOrEmpty())
			{
				this.ideo.WorshipRoomLabel = this.newWorshipRoomLabel;
			}
			this.ideo.SetIcon(this.newIconDef, this.newColorDef, this.newColorDef != this.ideo.colorDef);
			if (flag)
			{
				this.ideo.MakeMemeberNamePluralDirty();
				this.ideo.RegenerateAllPreceptNames();
			}
			this.Close(true);
		}

		// Token: 0x04006A73 RID: 27251
		private Ideo ideo;

		// Token: 0x04006A74 RID: 27252
		private string newName;

		// Token: 0x04006A75 RID: 27253
		private string newAdjective;

		// Token: 0x04006A76 RID: 27254
		private string newMemberName;

		// Token: 0x04006A77 RID: 27255
		private string newWorshipRoomLabel;

		// Token: 0x04006A78 RID: 27256
		private IdeoIconDef newIconDef;

		// Token: 0x04006A79 RID: 27257
		private ColorDef newColorDef;

		// Token: 0x04006A7A RID: 27258
		private Vector2 scrollPos;

		// Token: 0x04006A7B RID: 27259
		private float viewHeight;

		// Token: 0x04006A7C RID: 27260
		private static List<ColorDef> allColors;

		// Token: 0x04006A7D RID: 27261
		private const int IconSize = 40;

		// Token: 0x04006A7E RID: 27262
		private const int IconPadding = 5;

		// Token: 0x04006A7F RID: 27263
		private const int IconMargin = 5;

		// Token: 0x04006A80 RID: 27264
		private const int ColorSize = 22;

		// Token: 0x04006A81 RID: 27265
		private const int ColorPadding = 2;

		// Token: 0x04006A82 RID: 27266
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04006A83 RID: 27267
		private static readonly Color IconColor = new Color(0.95f, 0.95f, 0.95f);

		// Token: 0x04006A84 RID: 27268
		private static readonly float EditFieldHeight = 30f;

		// Token: 0x04006A85 RID: 27269
		private static readonly float ResetButtonWidth = 120f;

		// Token: 0x04006A86 RID: 27270
		private static readonly Regex ValidSymbolRegex = new Regex("^[\\p{L}0-9 '\\-]*$");

		// Token: 0x04006A87 RID: 27271
		private const int MaxSymbolLength = 40;
	}
}
