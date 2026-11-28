using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D83 RID: 7555
	public class ITab_StudyNotes : ITab
	{
		// Token: 0x1700244B RID: 9291
		// (get) Token: 0x0600C3CA RID: 50122 RVA: 0x0047A349 File Offset: 0x00478549
		protected Thing StudiableThing
		{
			get
			{
				Building_HoldingPlatform building_HoldingPlatform = base.SelThing as Building_HoldingPlatform;
				return ((building_HoldingPlatform != null) ? building_HoldingPlatform.HeldPawn : null) ?? base.SelThing;
			}
		}

		// Token: 0x1700244C RID: 9292
		// (get) Token: 0x0600C3CB RID: 50123 RVA: 0x0047A36C File Offset: 0x0047856C
		protected bool Studiable
		{
			get
			{
				CompStudiable compStudiable = this.StudiableThing.TryGetComp<CompStudiable>();
				return compStudiable != null && compStudiable.StudyUnlocked();
			}
		}

		// Token: 0x1700244D RID: 9293
		// (get) Token: 0x0600C3CC RID: 50124 RVA: 0x0047A384 File Offset: 0x00478584
		public override bool IsVisible
		{
			get
			{
				return this.Studiable && this.StudyUnlocks != null;
			}
		}

		// Token: 0x1700244E RID: 9294
		// (get) Token: 0x0600C3CD RID: 50125 RVA: 0x0047A399 File Offset: 0x00478599
		private CompStudyUnlocks StudyUnlocks
		{
			get
			{
				return this.StudiableThing.TryGetComp<CompStudyUnlocks>();
			}
		}

		// Token: 0x1700244F RID: 9295
		// (get) Token: 0x0600C3CE RID: 50126 RVA: 0x0047A3A6 File Offset: 0x004785A6
		protected virtual IReadOnlyList<ChoiceLetter> Letters
		{
			get
			{
				return this.StudyUnlocks.Letters;
			}
		}

		// Token: 0x17002450 RID: 9296
		// (get) Token: 0x0600C3CF RID: 50127 RVA: 0x0047A3B3 File Offset: 0x004785B3
		protected virtual bool StudyCompleted
		{
			get
			{
				return this.StudyUnlocks.Completed;
			}
		}

		// Token: 0x17002451 RID: 9297
		// (get) Token: 0x0600C3D0 RID: 50128 RVA: 0x0047A3C0 File Offset: 0x004785C0
		protected virtual bool DrawThingIcon
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C3D1 RID: 50129 RVA: 0x0047A3C3 File Offset: 0x004785C3
		public ITab_StudyNotes()
		{
			this.size = new Vector2(Mathf.Min(610f, (float)UI.screenWidth), 350f);
			this.labelKey = "TabStudyNotesContents";
		}

		// Token: 0x0600C3D2 RID: 50130 RVA: 0x0047A3F6 File Offset: 0x004785F6
		public override void OnOpen()
		{
			this.selectedLetter = (this.Letters.EnumerableNullOrEmpty<ChoiceLetter>() ? null : this.Letters.Last<ChoiceLetter>());
		}

		// Token: 0x0600C3D3 RID: 50131 RVA: 0x0047A41C File Offset: 0x0047861C
		protected override void FillTab()
		{
			if (this.previous != this.StudiableThing)
			{
				this.selectedLetter = (this.Letters.EnumerableNullOrEmpty<ChoiceLetter>() ? null : this.Letters.Last<ChoiceLetter>());
				this.previous = this.StudiableThing;
			}
			Rect rect = new Rect(0f, 20f, this.size.x, this.size.y - 20f);
			rect = rect.ContractedBy(10f);
			Rect rect2 = rect;
			rect2.y = 10f;
			rect2.height = 30f;
			Rect rect3 = rect;
			rect3.yMin = rect2.yMax + 17f;
			Rect rect4;
			Rect rect5;
			rect3.SplitVerticallyWithMargin(out rect4, out rect5, 17f);
			rect5.yMin += 17f;
			Rect rect6 = rect5;
			rect6.xMin -= 17f;
			rect6.yMin = rect.yMin;
			this.DrawTitle(rect2);
			this.DrawLetters(rect4);
			if (this.selectedLetter != null)
			{
				Widgets.LabelScrollable(rect5, this.selectedLetter.Text, ref this.rightScroll, false, true, false);
				return;
			}
			using (new TextBlock(new GameFont?(GameFont.Small), new TextAnchor?(TextAnchor.MiddleCenter), new Color?(Color.gray)))
			{
				Widgets.Label(rect6, "StudyNotesTab_NoDiscoveries".Translate());
			}
		}

		// Token: 0x0600C3D4 RID: 50132 RVA: 0x0047A59C File Offset: 0x0047879C
		private void DrawTitle(Rect rect)
		{
			float num = 0f;
			if (this.DrawThingIcon)
			{
				Rect position = rect;
				position.width = position.height;
				num = rect.height + 10f;
				GUI.DrawTexture(position, this.StudiableThing.def.uiIcon);
			}
			Rect rect2 = rect;
			rect2.xMin += num;
			rect2.xMax = rect.x + rect.width / 2f;
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			Rect rect4 = rect3;
			rect4.y = rect2.yMax - 4f;
			using (new TextBlock(GameFont.Medium, TextAnchor.MiddleLeft))
			{
				Widgets.LabelFit(rect2, this.StudiableThing.LabelCap);
			}
			CompStudiable compStudiable = this.StudiableThing.TryGetComp<CompStudiable>();
			if (compStudiable != null)
			{
				Widgets.CheckboxLabeled(rect3, "StudyNotesTab_ToggleStudy".Translate(), ref compStudiable.studyEnabled, false, null, null, true, false);
				string text;
				if (!compStudiable.EverStudiable(out text) && !text.NullOrEmpty())
				{
					using (new TextBlock(ColorLibrary.RedReadable))
					{
						Widgets.Label(rect4, text);
					}
				}
			}
		}

		// Token: 0x0600C3D5 RID: 50133 RVA: 0x0047A6F4 File Offset: 0x004788F4
		private void DrawLetters(Rect rect)
		{
			Rect rect2 = rect;
			rect2.height = Text.LineHeight;
			TaggedString value = this.StudyCompleted ? "StudyNotesTab_StudyProgressCompleted".Translate() : "StudyNotesTab_StudyProgressOngoing".Translate();
			TaggedString value2 = "StudyNotesTab_StudyProgress".Translate();
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				Rect rect3 = rect2;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(value2);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(value);
				Widgets.Label(rect3, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Widgets.DrawLineHorizontal(rect.x, rect2.yMax + 4f, rect.width, Color.gray);
			int num = this.Letters.EnumerableNullOrEmpty<ChoiceLetter>() ? 0 : this.Letters.Count;
			Rect outRect = rect;
			outRect.yMin = rect2.yMax + 10f;
			Rect rect4 = new Rect(0f, 0f, rect.width, 30f * (float)num);
			float num2 = 0f;
			Widgets.BeginScrollView(outRect, ref this.leftScroll, rect4, true);
			for (int i = num - 1; i >= 0; i--)
			{
				this.DoLetterRow(rect4, ref num2, this.Letters[i], i);
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600C3D6 RID: 50134 RVA: 0x0047A854 File Offset: 0x00478A54
		private void DoLetterRow(Rect rect, ref float y, ChoiceLetter letter, int index)
		{
			rect.y = y;
			rect.height = 30f;
			y += 30f;
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlightSelected(rect);
				this.selectedLetter = letter;
			}
			else if (this.selectedLetter == letter)
			{
				Widgets.DrawHighlightSelected(rect);
			}
			else if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Rect rect2 = rect;
			rect2.width = 90f;
			Vector2 location = (Find.CurrentMap != null) ? Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile) : default(Vector2);
			string str = GenDate.DateShortStringAt((long)GenDate.TickGameToAbs(letter.arrivalTick), location);
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax + 4f;
			using (new TextBlock(new GameFont?(GameFont.Small), new TextAnchor?(TextAnchor.MiddleLeft), new bool?(false)))
			{
				Widgets.Label(rect2, str.Truncate(rect2.width, null));
				using (new TextBlock(new Color(0.75f, 0.75f, 0.75f)))
				{
					Widgets.LabelEllipses(rect3, letter.Label);
				}
			}
		}

		// Token: 0x04006FE3 RID: 28643
		private Vector2 leftScroll;

		// Token: 0x04006FE4 RID: 28644
		private Vector2 rightScroll;

		// Token: 0x04006FE5 RID: 28645
		private ChoiceLetter selectedLetter;

		// Token: 0x04006FE6 RID: 28646
		private Thing previous;

		// Token: 0x04006FE7 RID: 28647
		private const float TopPadding = 20f;

		// Token: 0x04006FE8 RID: 28648
		private const float InitialHeight = 350f;

		// Token: 0x04006FE9 RID: 28649
		private const float TitleHeight = 30f;

		// Token: 0x04006FEA RID: 28650
		private const float InitialWidth = 610f;

		// Token: 0x04006FEB RID: 28651
		private const float DateSize = 90f;

		// Token: 0x04006FEC RID: 28652
		private const float RowHeight = 30f;
	}
}
