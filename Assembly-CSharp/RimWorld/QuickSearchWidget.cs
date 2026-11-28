using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DE4 RID: 7652
	public class QuickSearchWidget
	{
		// Token: 0x0600C6F8 RID: 50936 RVA: 0x0048DC80 File Offset: 0x0048BE80
		public QuickSearchWidget()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
			defaultInterpolatedStringHandler.AppendLiteral("QuickSearchWidget_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(QuickSearchWidget.instanceCounter++);
			this.controlName = defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600C6F9 RID: 50937 RVA: 0x0048DCE8 File Offset: 0x0048BEE8
		public void OnGUI(Rect rect, Action onFilterChange = null, Action onClear = null)
		{
			if (this.CurrentlyFocused() && Event.current.type == EventType.keyDown && Event.current.keyCode == KeyCode.Escape)
			{
				this.Unfocus();
				Event.current.Use();
			}
			if (OriginalEventUtility.EventType == EventType.mouseDown && !rect.Contains(Event.current.mousePosition))
			{
				this.Unfocus();
			}
			Color color = GUI.color;
			GUI.color = Color.white;
			float num = Mathf.Min(18f, rect.height);
			float num2 = num + 8f;
			float y = rect.y + (rect.height - num2) / 2f + 4f;
			Rect position = new Rect(rect.x + 4f, y, num, num);
			GUI.DrawTexture(position, TexButton.Search);
			GUI.SetNextControlName(this.controlName);
			Rect rect2 = new Rect(position.xMax + 4f, rect.y, rect.width - num2, rect.height);
			if (this.filter.Active)
			{
				rect2.xMax -= num2;
			}
			using (new TextBlock(GameFont.Small))
			{
				if (this.noResultsMatched && this.filter.Active)
				{
					GUI.color = ColorLibrary.RedReadable;
				}
				else if (!this.filter.Active && !this.CurrentlyFocused())
				{
					GUI.color = this.inactiveTextColor;
				}
				string text = Widgets.TextField(rect2, this.filter.Text, this.maxSearchTextLength, null);
				GUI.color = Color.white;
				if (text != this.filter.Text)
				{
					this.filter.Text = text;
					if (onFilterChange != null)
					{
						onFilterChange();
					}
				}
			}
			if (this.filter.Active && Widgets.ButtonImage(new Rect(rect2.xMax + 4f, y, num, num), TexButton.CloseXSmall, true, null))
			{
				this.filter.Text = "";
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				if (onFilterChange != null)
				{
					onFilterChange();
				}
				if (onClear != null)
				{
					onClear();
				}
			}
			GUI.color = color;
		}

		// Token: 0x0600C6FA RID: 50938 RVA: 0x0048DF24 File Offset: 0x0048C124
		public void Unfocus()
		{
			if (this.CurrentlyFocused())
			{
				UI.UnfocusCurrentControl();
			}
		}

		// Token: 0x0600C6FB RID: 50939 RVA: 0x0048DF33 File Offset: 0x0048C133
		public void Focus()
		{
			GUI.FocusControl(this.controlName);
		}

		// Token: 0x0600C6FC RID: 50940 RVA: 0x0048DF40 File Offset: 0x0048C140
		public bool CurrentlyFocused()
		{
			return GUI.GetNameOfFocusedControl() == this.controlName;
		}

		// Token: 0x0600C6FD RID: 50941 RVA: 0x0048DF52 File Offset: 0x0048C152
		public void Reset()
		{
			this.filter.Text = "";
			this.noResultsMatched = false;
		}

		// Token: 0x0400711F RID: 28959
		public QuickSearchFilter filter = new QuickSearchFilter();

		// Token: 0x04007120 RID: 28960
		public bool noResultsMatched;

		// Token: 0x04007121 RID: 28961
		public Color inactiveTextColor = Color.white;

		// Token: 0x04007122 RID: 28962
		public int maxSearchTextLength = 30;

		// Token: 0x04007123 RID: 28963
		private readonly string controlName;

		// Token: 0x04007124 RID: 28964
		public const float WidgetHeight = 24f;

		// Token: 0x04007125 RID: 28965
		public const float IconSize = 18f;

		// Token: 0x04007126 RID: 28966
		public const float IconMargin = 4f;

		// Token: 0x04007127 RID: 28967
		private const int BaseMaxSearchTextLength = 30;

		// Token: 0x04007128 RID: 28968
		private static int instanceCounter;
	}
}
