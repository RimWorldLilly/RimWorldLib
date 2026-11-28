using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.Sound;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x020007A1 RID: 1953
	public sealed class ProfilerThreadFilter
	{
		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06003E89 RID: 16009 RVA: 0x0018DD5B File Offset: 0x0018BF5B
		// (set) Token: 0x06003E8A RID: 16010 RVA: 0x0018DD63 File Offset: 0x0018BF63
		public bool ShowBytes
		{
			get
			{
				return this.showBytes;
			}
			set
			{
				if (this.showBytes == value)
				{
					return;
				}
				this.showBytes = value;
				this.refreshGate.Invalidate();
			}
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x0018DD84 File Offset: 0x0018BF84
		public ProfilerThreadFilter(ProfilerSession session)
		{
			this.session = session;
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x0018DDF0 File Offset: 0x0018BFF0
		public void Refresh()
		{
			if (this.lastRegistryVersion != ProfilerThreadRegistry.Version)
			{
				this.lastRegistryVersion = ProfilerThreadRegistry.Version;
				this.items.Clear();
				IReadOnlyList<ProfilerThreadRegistry.Entry> entries = ProfilerThreadRegistry.Entries;
				for (int i = 0; i < entries.Count; i++)
				{
					this.items.Add(new ProfilerThreadFilter.Item
					{
						threadId = entries[i].threadId,
						label = FrameProfiler.GetLabel(entries[i].nameLabelId),
						isMain = entries[i].isMain,
						color = Color.Lerp(ProfilerPalette.ColorFor(entries[i].nameLabelId), Color.white, 0.15f)
					});
				}
				this.statsCoverItems = false;
			}
			bool flag = this.lastSelectionVersion != ProfilerThreadRegistry.SelectionVersion;
			this.lastSelectionVersion = ProfilerThreadRegistry.SelectionVersion;
			if (this.items.Count <= 1)
			{
				return;
			}
			bool flag2 = (this.open && !this.statsCoverItems) || flag;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (!flag2 && (realtimeSinceStartup - this.lastStatsTime < 0.25f || !this.refreshGate.ShouldRefresh(this.session)))
			{
				return;
			}
			this.lastStatsTime = realtimeSinceStartup;
			this.UpdateStats();
			if (this.open && this.items.Count >= 3)
			{
				this.items.Sort(new Comparison<ProfilerThreadFilter.Item>(ProfilerThreadFilter.CompareItems));
			}
			this.UpdateSummary();
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x0018DF60 File Offset: 0x0018C160
		private static int CompareItems(ProfilerThreadFilter.Item lhs, ProfilerThreadFilter.Item rhs)
		{
			if (lhs.isMain != rhs.isMain)
			{
				if (!lhs.isMain)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				int num = rhs.value.CompareTo(lhs.value);
				if (num == 0)
				{
					return string.CompareOrdinal(lhs.label, rhs.label);
				}
				return num;
			}
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x0018DFB0 File Offset: 0x0018C1B0
		private void UpdateStats()
		{
			this.session.CollectFrames(this.session.Span, this.frameScratch);
			if (this.frameScratch.Count == 0)
			{
				return;
			}
			this.valueByThreadId.Clear();
			float num = 0f;
			for (int i = 0; i < this.frameScratch.Count; i++)
			{
				FrameProfiler.FrameSnapshot frameSnapshot = this.frameScratch[i];
				if (!this.showBytes)
				{
					num += frameSnapshot.totalMs;
				}
				for (int j = 0; j < frameSnapshot.sectionCount; j++)
				{
					float num2 = this.showBytes ? ((float)frameSnapshot.pathEntries[frameSnapshot.sections[j].firstPathEntry].allocBytes) : ((j == 0) ? frameSnapshot.totalMs : ((float)frameSnapshot.sections[j].busyNanoseconds / 1000000f));
					if (this.showBytes)
					{
						num += num2;
					}
					float num3;
					this.valueByThreadId.TryGetValue(frameSnapshot.sections[j].threadId, out num3);
					this.valueByThreadId[frameSnapshot.sections[j].threadId] = num3 + num2;
				}
			}
			int count = this.frameScratch.Count;
			num /= (float)count;
			this.statsCoverItems = this.open;
			float num4 = 0f;
			StringBuilder scratch = ProfilerGui.Scratch;
			for (int k = 0; k < this.items.Count; k++)
			{
				ProfilerThreadFilter.Item item = this.items[k];
				float num5;
				this.valueByThreadId.TryGetValue(item.threadId, out num5);
				num5 /= (float)count;
				if (ProfilerThreadRegistry.IsSelected(item.threadId))
				{
					num4 += num5;
				}
				if (this.open)
				{
					item.value = num5;
					item.fraction = ((num > 0f) ? Mathf.Clamp01(num5 / num) : 0f);
					scratch.Clear();
					this.AppendValue(scratch, num5);
					item.busyText = scratch.ToString();
					item.utilText = Mathf.RoundToInt(item.fraction * 100f).ToString() + (this.showBytes ? "% of alloc" : "% busy");
				}
			}
			scratch.Clear();
			this.AppendValue(scratch, num4);
			this.buttonBusyText = scratch.ToString();
		}

		// Token: 0x06003E8F RID: 16015 RVA: 0x0018E22B File Offset: 0x0018C42B
		private void AppendValue(StringBuilder builder, float value)
		{
			if (this.showBytes)
			{
				ProfilerTreeModel.AppendAllocBytes(builder, (long)value, " ");
				return;
			}
			ProfilerTreeModel.AppendMs(builder, value, "0.0", " ");
		}

		// Token: 0x06003E90 RID: 16016 RVA: 0x0018E254 File Offset: 0x0018C454
		private void UpdateSummary()
		{
			IReadOnlyList<ProfilerThreadRegistry.Entry> entries = ProfilerThreadRegistry.Entries;
			int num;
			int num2;
			ProfilerThreadRegistry.GetSelectionSummary(out num, out num2);
			this.buttonDot = ProfilerThreadFilter.MultiDot;
			if (entries.Count > 1 && num == entries.Count)
			{
				this.buttonLabel = "All threads";
				return;
			}
			if (num == 1 && num2 >= 0)
			{
				this.buttonLabel = (entries[num2].isMain ? "Main" : FrameProfiler.GetLabel(entries[num2].nameLabelId));
				this.buttonDot = Color.Lerp(ProfilerPalette.ColorFor(entries[num2].nameLabelId), Color.white, 0.15f);
				return;
			}
			this.buttonLabel = ((num == 0) ? "No threads" : (num.ToString() + " threads"));
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x0018E318 File Offset: 0x0018C518
		public void HandleInput()
		{
			if (!this.open)
			{
				return;
			}
			ProfilerGui.AddOverlay(this.popupRect);
			Event current = Event.current;
			if (current.type != EventType.mouseDown && current.type != EventType.scrollWheel)
			{
				return;
			}
			if (!this.popupRect.Contains(current.mousePosition))
			{
				if (current.type == EventType.mouseDown && !this.buttonRect.Contains(current.mousePosition))
				{
					this.open = false;
				}
				return;
			}
			if (current.type == EventType.mouseDown && current.button == 0)
			{
				float num = current.mousePosition.y - this.popupRect.y;
				if (num < 26f)
				{
					this.ToggleSelectAll();
				}
				else
				{
					int num2 = Mathf.FloorToInt((num - 26f) / 24f);
					if (num2 >= 0 && num2 < this.items.Count)
					{
						int threadId = this.items[num2].threadId;
						bool flag = !ProfilerThreadRegistry.IsSelected(threadId);
						ProfilerThreadRegistry.SetSelected(threadId, flag);
						(flag ? SoundDefOf.Checkbox_TurnedOn : SoundDefOf.Checkbox_TurnedOff).PlayOneShotOnCamera(null);
						this.session.Invalidate();
					}
				}
			}
			current.Use();
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x0018E434 File Offset: 0x0018C634
		private void ToggleSelectAll()
		{
			IReadOnlyList<ProfilerThreadRegistry.Entry> entries = ProfilerThreadRegistry.Entries;
			bool allSelected = ProfilerThreadRegistry.AllSelected;
			(allSelected ? SoundDefOf.Checkbox_TurnedOff : SoundDefOf.Checkbox_TurnedOn).PlayOneShotOnCamera(null);
			for (int i = 0; i < entries.Count; i++)
			{
				if (!allSelected)
				{
					ProfilerThreadRegistry.SetSelected(entries[i].threadId, true);
				}
				else if (!entries[i].isMain)
				{
					ProfilerThreadRegistry.SetSelected(entries[i].threadId, false);
				}
			}
			this.session.Invalidate();
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x0018E4B8 File Offset: 0x0018C6B8
		public void DrawButton(ref float x, float y)
		{
			if (this.items.Count <= 1)
			{
				this.open = false;
				return;
			}
			float num = ProfilerGui.Width(this.buttonLabel, GameFont.Tiny);
			float num2 = ProfilerGui.Width(this.buttonBusyText, GameFont.Tiny);
			this.buttonRect = new Rect(x, y, 22f + num + 8f + num2 + 22f, 21f);
			x = this.buttonRect.xMax;
			ProfilerGui.Bordered(this.buttonRect, (this.open || ProfilerGui.Hovered(this.buttonRect)) ? ProfilerGui.ChipHover : ProfilerGui.ChipBackground, ProfilerGui.Divider);
			ProfilerGui.Fill(new Rect(this.buttonRect.x + 8f, this.buttonRect.y + 7f, 7f, 7f), this.buttonDot);
			ProfilerGui.Label(new Rect(this.buttonRect.x + 22f, y, num + 4f, 21f), this.buttonLabel, ProfilerGui.TextBright, TextAnchor.MiddleLeft, GameFont.Tiny);
			ProfilerGui.Label(new Rect(this.buttonRect.x + 22f + num + 8f, y, num2 + 4f, 21f), this.buttonBusyText, ProfilerGui.TextFaint, TextAnchor.MiddleLeft, GameFont.Tiny);
			ProfilerGui.Arrow(new Rect(this.buttonRect.xMax - 20f, y, 16f, 21f), true, ProfilerGui.TextDim);
			if (ProfilerGui.Pressed(this.buttonRect, this.open ? SoundDefOf.Click : SoundDefOf.FloatMenu_Open))
			{
				this.open = !this.open;
				this.statsCoverItems = false;
			}
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x0018E66C File Offset: 0x0018C86C
		public void DrawPopup(Rect bounds)
		{
			if (!this.open || !ProfilerGui.Painting)
			{
				return;
			}
			float a = 26f + (float)this.items.Count * 24f + 4f;
			this.popupRect = new Rect(Mathf.Min(this.buttonRect.x, bounds.xMax - 360f - 4f), this.buttonRect.yMax + 3f, 360f, Mathf.Min(a, bounds.yMax - this.buttonRect.yMax - 8f));
			ProfilerGui.Bordered(this.popupRect, ProfilerGui.TitleBar, ProfilerGui.FocusBorder);
			IReadOnlyList<ProfilerThreadRegistry.Entry> entries = ProfilerThreadRegistry.Entries;
			int num;
			int count;
			ProfilerThreadRegistry.GetSelectionSummary(out num, out count);
			bool flag = num == entries.Count;
			Vector2 mousePosition = Event.current.mousePosition;
			if (this.popupRect.Contains(mousePosition))
			{
				ProfilerGui.ClearTooltip();
			}
			Rect rect = new Rect(this.popupRect.x + 1f, this.popupRect.y + 1f, this.popupRect.width - 2f, 25f);
			MouseoverSounds.DoRegion(rect);
			if (rect.Contains(mousePosition))
			{
				ProfilerGui.Fill(rect, ProfilerGui.ChipBackground);
			}
			ProfilerGui.Fill(new Rect(rect.x + 10f, rect.y + 9f, 7f, 7f), flag ? ProfilerThreadFilter.MultiDot : ProfilerThreadFilter.DotOff);
			Rect rect2 = new Rect(rect.x + 26f, rect.y, 160f, rect.height);
			string text;
			if (!flag)
			{
				string str = "select all ";
				count = entries.Count;
				text = str + count.ToString();
			}
			else
			{
				string str2 = "all ";
				count = entries.Count;
				text = str2 + count.ToString() + " threads";
			}
			ProfilerGui.Label(rect2, text, ProfilerGui.TextBright, TextAnchor.MiddleLeft, GameFont.Tiny);
			Rect rect3 = new Rect(rect.x, rect.y, rect.width - 10f, rect.height);
			string str3 = num.ToString();
			string str4 = " of ";
			count = entries.Count;
			ProfilerGui.Label(rect3, str3 + str4 + count.ToString() + " selected", ProfilerGui.TextFaint, TextAnchor.MiddleRight, GameFont.Tiny);
			ProfilerGui.Fill(new Rect(this.popupRect.x + 1f, rect.yMax, this.popupRect.width - 2f, 1f), ProfilerGui.Divider);
			for (int i = 0; i < this.items.Count; i++)
			{
				Rect rect4 = new Rect(this.popupRect.x + 1f, this.popupRect.y + 26f + (float)i * 24f, this.popupRect.width - 2f, 24f);
				if (rect4.yMax > this.popupRect.yMax)
				{
					break;
				}
				ProfilerThreadFilter.Item item = this.items[i];
				bool flag2 = ProfilerThreadRegistry.IsSelected(item.threadId);
				MouseoverSounds.DoRegion(rect4);
				if (rect4.Contains(mousePosition))
				{
					ProfilerGui.Fill(rect4, ProfilerGui.ChipBackground);
				}
				ProfilerGui.Fill(new Rect(rect4.x + 10f, rect4.y + 8.5f, 7f, 7f), flag2 ? item.color : ProfilerThreadFilter.DotOff);
				ProfilerGui.Label(new Rect(rect4.x + 26f, rect4.y, 130f, rect4.height), item.label, flag2 ? ProfilerGui.TextMain : ProfilerGui.TextDim, TextAnchor.MiddleLeft, GameFont.Tiny);
				Rect rect5 = new Rect(rect4.x + 162f, rect4.y + 9f, 60f, 6f);
				ProfilerGui.Fill(rect5, ProfilerGui.BarTrack);
				ProfilerGui.Fill(new Rect(rect5.x, rect5.y, rect5.width * item.fraction, rect5.height), (!flag2) ? ProfilerThreadFilter.BarOff : ((item.fraction >= 0.95f) ? ProfilerThreadFilter.BarHot : ProfilerThreadFilter.BarOn));
				ProfilerGui.Label(new Rect(rect4.x + 226f, rect4.y, 60f, rect4.height), item.busyText, ProfilerGui.TextDim, TextAnchor.MiddleRight, GameFont.Tiny);
				ProfilerGui.Label(new Rect(rect4.x + 288f, rect4.y, 62f, rect4.height), item.utilText, ProfilerGui.TextFaint, TextAnchor.MiddleRight, GameFont.Tiny);
			}
		}

		// Token: 0x04002935 RID: 10549
		private const float RowHeight = 24f;

		// Token: 0x04002936 RID: 10550
		private const float PopupWidth = 360f;

		// Token: 0x04002937 RID: 10551
		private const float HeaderHeight = 26f;

		// Token: 0x04002938 RID: 10552
		private static readonly Color DotOff = ProfilerGui.Hex(4281942093U);

		// Token: 0x04002939 RID: 10553
		private static readonly Color BarOff = ProfilerGui.Hex(4281547333U);

		// Token: 0x0400293A RID: 10554
		private static readonly Color BarOn = ProfilerGui.Hex(4284185510U);

		// Token: 0x0400293B RID: 10555
		private static readonly Color BarHot = ProfilerGui.Hex(4291324496U);

		// Token: 0x0400293C RID: 10556
		private static readonly Color MultiDot = ProfilerGui.Hex(4285303992U);

		// Token: 0x0400293D RID: 10557
		private readonly ProfilerSession session;

		// Token: 0x0400293E RID: 10558
		private readonly List<ProfilerThreadFilter.Item> items = new List<ProfilerThreadFilter.Item>();

		// Token: 0x0400293F RID: 10559
		private readonly List<FrameProfiler.FrameSnapshot> frameScratch = new List<FrameProfiler.FrameSnapshot>();

		// Token: 0x04002940 RID: 10560
		private readonly Dictionary<int, float> valueByThreadId = new Dictionary<int, float>();

		// Token: 0x04002941 RID: 10561
		private ProfilerRefreshGate refreshGate;

		// Token: 0x04002942 RID: 10562
		private int lastRegistryVersion = -1;

		// Token: 0x04002943 RID: 10563
		private int lastSelectionVersion = -1;

		// Token: 0x04002944 RID: 10564
		private bool open;

		// Token: 0x04002945 RID: 10565
		private bool statsCoverItems;

		// Token: 0x04002946 RID: 10566
		private Rect buttonRect;

		// Token: 0x04002947 RID: 10567
		private Rect popupRect;

		// Token: 0x04002948 RID: 10568
		private string buttonLabel = "Main";

		// Token: 0x04002949 RID: 10569
		private string buttonBusyText = "";

		// Token: 0x0400294A RID: 10570
		private Color buttonDot = ProfilerThreadFilter.MultiDot;

		// Token: 0x0400294B RID: 10571
		private const float StatsInterval = 0.25f;

		// Token: 0x0400294C RID: 10572
		private float lastStatsTime;

		// Token: 0x0400294D RID: 10573
		private bool showBytes;

		// Token: 0x02002AA4 RID: 10916
		private sealed class Item
		{
			// Token: 0x0400AEB2 RID: 44722
			public int threadId;

			// Token: 0x0400AEB3 RID: 44723
			public string label;

			// Token: 0x0400AEB4 RID: 44724
			public bool isMain;

			// Token: 0x0400AEB5 RID: 44725
			public Color color;

			// Token: 0x0400AEB6 RID: 44726
			public float value;

			// Token: 0x0400AEB7 RID: 44727
			public float fraction;

			// Token: 0x0400AEB8 RID: 44728
			public string busyText = "";

			// Token: 0x0400AEB9 RID: 44729
			public string utilText = "";
		}
	}
}
