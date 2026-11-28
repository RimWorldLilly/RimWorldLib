using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CC7 RID: 7367
	public abstract class Dialog_Search<T> : Window where T : class
	{
		// Token: 0x17002274 RID: 8820
		// (get) Token: 0x0600BD3E RID: 48446 RVA: 0x0043E59F File Offset: 0x0043C79F
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(350f, 100f);
			}
		}

		// Token: 0x17002275 RID: 8821
		// (get) Token: 0x0600BD3F RID: 48447 RVA: 0x0043E5B0 File Offset: 0x0043C7B0
		public override QuickSearchWidget CommonSearchWidget
		{
			get
			{
				return this.quickSearchWidget;
			}
		}

		// Token: 0x17002276 RID: 8822
		// (get) Token: 0x0600BD40 RID: 48448 RVA: 0x0043E5B8 File Offset: 0x0043C7B8
		protected bool Searching
		{
			get
			{
				return !this.quickSearchWidget.filter.Text.NullOrEmpty() && this.allElements.Any<T>() && this.searchIndex < this.allElements.Count;
			}
		}

		// Token: 0x17002277 RID: 8823
		// (get) Token: 0x0600BD41 RID: 48449
		protected abstract List<T> SearchSet { get; }

		// Token: 0x17002278 RID: 8824
		// (get) Token: 0x0600BD42 RID: 48450
		protected abstract bool ShouldClose { get; }

		// Token: 0x17002279 RID: 8825
		// (get) Token: 0x0600BD43 RID: 48451
		protected abstract TaggedString SearchLabel { get; }

		// Token: 0x1700227A RID: 8826
		// (get) Token: 0x0600BD44 RID: 48452 RVA: 0x0043E5F3 File Offset: 0x0043C7F3
		public T Highlighted
		{
			get
			{
				return this.highlightedElement;
			}
		}

		// Token: 0x0600BD45 RID: 48453
		protected abstract void TryAddElement(T element);

		// Token: 0x0600BD46 RID: 48454
		protected abstract void TryRemoveElement(T element);

		// Token: 0x0600BD47 RID: 48455
		protected abstract void DoIcon(T element, Rect iconRect);

		// Token: 0x0600BD48 RID: 48456
		protected abstract void DoLabel(T element, Rect labelRect);

		// Token: 0x0600BD49 RID: 48457 RVA: 0x0043E5FB File Offset: 0x0043C7FB
		protected virtual void DoExtraIcon(T element, Rect iconRect)
		{
		}

		// Token: 0x0600BD4A RID: 48458
		protected abstract void ClikedOnElement(T element);

		// Token: 0x0600BD4B RID: 48459
		protected abstract bool ShouldSkipElement(T element);

		// Token: 0x0600BD4C RID: 48460
		protected abstract void OnHighlightUpdate(T element);

		// Token: 0x0600BD4D RID: 48461 RVA: 0x0043E5FD File Offset: 0x0043C7FD
		protected virtual void CheckAnyElementRemoved()
		{
		}

		// Token: 0x0600BD4E RID: 48462 RVA: 0x0043E5FF File Offset: 0x0043C7FF
		public bool IsListed(T element)
		{
			return this.searchResultsSet.Contains(element);
		}

		// Token: 0x0600BD4F RID: 48463 RVA: 0x0043E60D File Offset: 0x0043C80D
		protected override Rect QuickSearchWidgetRect(Rect winRect, Rect inRect)
		{
			return new Rect(inRect.x, inRect.yMax - 24f, inRect.width, 24f);
		}

		// Token: 0x0600BD50 RID: 48464 RVA: 0x0043E634 File Offset: 0x0043C834
		public Dialog_Search() : base(null)
		{
			this.doCloseX = true;
			this.closeOnAccept = false;
			this.preventCameraMotion = false;
			this.quickSearchWidget = new QuickSearchWidget();
			this.searchResults = new SortedList<string, T>(new DuplicateKeyComparer<string>());
			this.searchResultsSet = new HashSet<T>();
			this.allElements = new List<T>();
		}

		// Token: 0x0600BD51 RID: 48465 RVA: 0x0043E690 File Offset: 0x0043C890
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			this.highlightedElement = default(T);
			float num = Text.CalcHeight(this.SearchLabel, inRect.width);
			Rect rect = new Rect(0f, inRect.yMax - 24f - num, inRect.width, num);
			using (new TextBlock(ColoredText.SubtleGrayColor, TextAnchor.MiddleLeft, true))
			{
				string label;
				if (this.Searching)
				{
					label = QuickSearchUtility.CurrentSearchText;
				}
				else if (this.quickSearchWidget.filter.Text.Length > 0)
				{
					label = ((this.searchResults.Count == 1) ? "MapSearchResultSingular".Translate() : "MapSearchResults".Translate(this.searchResults.Count));
				}
				else
				{
					label = this.SearchLabel;
				}
				Widgets.Label(rect, label);
			}
			if (this.searchResults.Count > 0)
			{
				Rect outRect = new Rect(0f, 0f, inRect.width, inRect.height);
				outRect.yMax = rect.yMin;
				Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, this.scrollHeight);
				Rect rect2 = new Rect(0f, this.scrollPos.y, outRect.width, outRect.height);
				bool flag = this.scrollHeight >= outRect.height;
				Widgets.BeginScrollView(outRect, ref this.scrollPos, viewRect, true);
				using (new ProfilerBlock("DrawSearchResults"))
				{
					for (int i = 0; i < this.searchResults.Count; i++)
					{
						Rect rect3 = new Rect(0f, 26f * (float)i, inRect.width, 26f);
						if (rect2.Overlaps(rect3))
						{
							if (i % 2 == 1)
							{
								Widgets.DrawLightHighlight(rect3);
							}
							T t = this.searchResults.Values[i];
							if (t != null && !this.ShouldSkipElement(t))
							{
								Rect iconRect = rect3;
								iconRect.xMax = 26f;
								this.DoIcon(t, iconRect);
								Rect iconRect2 = rect3;
								if (flag)
								{
									iconRect2.xMin = rect3.xMax - 26f - 16f;
									iconRect2.width = 26f;
								}
								else
								{
									iconRect2.xMin = rect3.xMax - 26f;
								}
								this.DoExtraIcon(t, iconRect2);
								Rect labelRect = rect3;
								labelRect.xMin = iconRect.xMax + 4f;
								labelRect.xMax = iconRect2.xMin - 4f;
								this.DoLabel(t, labelRect);
								if (Mouse.IsOver(rect3))
								{
									Widgets.DrawHighlight(rect3);
									this.highlightedElement = t;
								}
								if (Widgets.ButtonInvisible(rect3, true))
								{
									this.ClikedOnElement(t);
								}
							}
						}
					}
				}
				Widgets.EndScrollView();
			}
			if (!this.triedToFocus && this.openFrames == 2)
			{
				this.quickSearchWidget.Focus();
				this.triedToFocus = true;
			}
		}

		// Token: 0x0600BD52 RID: 48466 RVA: 0x0043E9F4 File Offset: 0x0043CBF4
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			this.openFrames++;
			if (this.ShouldClose)
			{
				this.Close(true);
				return;
			}
			if (this.highlightedElement != null)
			{
				this.OnHighlightUpdate(this.highlightedElement);
			}
			if (this.Searching)
			{
				using (new ProfilerBlock("Searching"))
				{
					for (int i = 0; i < 500; i++)
					{
						this.searchIndex++;
						if (this.searchIndex >= this.allElements.Count)
						{
							this.allElements.Clear();
							break;
						}
						this.TryAddElement(this.allElements[this.searchIndex]);
					}
					return;
				}
			}
			if (Time.frameCount % 20 == 0)
			{
				this.CheckAnyElementRemoved();
			}
		}

		// Token: 0x0600BD53 RID: 48467 RVA: 0x0043EAD8 File Offset: 0x0043CCD8
		protected override void SetInitialSizeAndPosition()
		{
			this.scrollHeight = (float)this.searchResults.Count * 26f;
			Vector2 initialSize = this.InitialSize;
			initialSize.y = Mathf.Clamp(initialSize.y + this.scrollHeight, this.InitialSize.y, (float)UI.screenHeight / 2f);
			this.windowRect = new Rect((float)UI.screenWidth - initialSize.x, (float)UI.screenHeight - initialSize.y - 35f, initialSize.x, initialSize.y).Rounded();
		}

		// Token: 0x0600BD54 RID: 48468 RVA: 0x0043EB70 File Offset: 0x0043CD70
		public override void Notify_CommonSearchChanged()
		{
			this.scrollPos = Vector2.zero;
			this.searchIndex = 0;
			this.searchResults.Clear();
			this.searchResultsSet.Clear();
			this.allElements.Clear();
			this.allElements.AddRange(this.SearchSet);
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x0600BD55 RID: 48469 RVA: 0x0043EBC7 File Offset: 0x0043CDC7
		protected bool TextMatch(string text)
		{
			return !text.NullOrEmpty() && text.IndexOf(this.quickSearchWidget.filter.Text, StringComparison.InvariantCultureIgnoreCase) >= 0;
		}

		// Token: 0x04006C38 RID: 27704
		protected QuickSearchWidget quickSearchWidget;

		// Token: 0x04006C39 RID: 27705
		protected SortedList<string, T> searchResults;

		// Token: 0x04006C3A RID: 27706
		protected HashSet<T> searchResultsSet;

		// Token: 0x04006C3B RID: 27707
		protected T highlightedElement;

		// Token: 0x04006C3C RID: 27708
		protected Vector2 scrollPos;

		// Token: 0x04006C3D RID: 27709
		protected float scrollHeight;

		// Token: 0x04006C3E RID: 27710
		protected List<T> allElements;

		// Token: 0x04006C3F RID: 27711
		protected int searchIndex;

		// Token: 0x04006C40 RID: 27712
		protected bool triedToFocus;

		// Token: 0x04006C41 RID: 27713
		private int openFrames;

		// Token: 0x04006C42 RID: 27714
		protected const float ElementHeight = 26f;

		// Token: 0x04006C43 RID: 27715
		private const int MaxSearchesPerFrame = 500;
	}
}
