using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityStubs.Gui;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x0200078C RID: 1932
	public sealed class ProfilerFlamechartWidget
	{
		// Token: 0x06003D09 RID: 15625 RVA: 0x00181AF5 File Offset: 0x0017FCF5
		private float BandGapFor(int band)
		{
			if (this.bandLanes[band] > 1)
			{
				return 0.4f;
			}
			return 0f;
		}

		// Token: 0x06003D0A RID: 15626 RVA: 0x00181B10 File Offset: 0x0017FD10
		public ProfilerFlamechartWidget(ProfilerSession session)
		{
			this.session = session;
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x06003D0B RID: 15627 RVA: 0x00181D39 File Offset: 0x0017FF39
		private bool UseSpikes
		{
			get
			{
				return this.session.IsSpikes;
			}
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x00181D46 File Offset: 0x0017FF46
		public void NotifySourceChanged()
		{
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x00181D48 File Offset: 0x0017FF48
		public void HighlightZone(int ordinal, int pathId)
		{
			this.SelectedPathIds.Clear();
			if (pathId > 0)
			{
				this.SelectedPathIds.Add(pathId);
			}
			this.SelectedZoneOrdinal = ordinal;
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x00181D6C File Offset: 0x0017FF6C
		public void HighlightZones(int ordinal, List<int> pathIds)
		{
			this.SelectedPathIds.Clear();
			this.SelectedPathIds.AddRange(pathIds);
			this.SelectedZoneOrdinal = ordinal;
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x00181D8C File Offset: 0x0017FF8C
		public void ClearHighlight()
		{
			this.HighlightZone(-1, -1);
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x00181D96 File Offset: 0x0017FF96
		public void Clear()
		{
			this.ClearHighlight();
			this.ClearEntries();
			this.lastLayoutNewestOrdinal = int.MinValue;
			this.lastLayoutCount = -1;
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x00181DB8 File Offset: 0x0017FFB8
		public void Refresh()
		{
			bool selectionAgedOut = this.session.SelectionAgedOut;
			this.RetainedSnapshot = (selectionAgedOut ? this.session.Snapshot : null);
			int num;
			int num2;
			bool flag = this.session.TryGetSelectionBand(out num, out num2);
			this.SelectedOffset = (selectionAgedOut ? 0 : (flag ? num : -1));
			this.SelectedOldestOffset = (selectionAgedOut ? 0 : (flag ? num2 : -1));
			this.SelectionRangeStart = (double)this.session.SelectionStartNs;
			this.SelectionRangeEnd = (double)this.session.SelectionEndNs;
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x00181E44 File Offset: 0x00180044
		public void Draw(Rect rect)
		{
			ProfilerGui.Fill(rect, ProfilerGui.SunkenBackground);
			GUI.BeginGroup(rect);
			try
			{
				this.width = rect.width;
				this.height = rect.height;
				this.HandleInput();
				if (ProfilerGui.Painting)
				{
					this.Paint();
				}
			}
			finally
			{
				GUI.EndGroup();
			}
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x00181EA8 File Offset: 0x001800A8
		private void Quad(Color color, float x0, float y0, float x1, float y1)
		{
			x0 = Mathf.Max(0f, x0);
			y0 = Mathf.Max(0f, y0);
			x1 = Mathf.Min(this.width, x1);
			y1 = Mathf.Min(this.height, y1);
			if (x1 > x0 && y1 > y0)
			{
				GuiBackend.Current.FillRect(Rect.MinMaxRect(x0, y0, x1, y1), color);
			}
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x00181F10 File Offset: 0x00180110
		private void OutlineQuad(Color color, float x0, float y0, float x1, float y1)
		{
			this.Quad(color, x0, y0, x1, y0 + 1f);
			this.Quad(color, x0, y1 - 1f, x1, y1);
			this.Quad(color, x0, y0 + 1f, x0 + 1f, y1 - 1f);
			this.Quad(color, x1 - 1f, y0 + 1f, x1, y1 - 1f);
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x00181F81 File Offset: 0x00180181
		private static void Text(string text, float x, float y, float maxWidth, Color color)
		{
			ProfilerGui.Label(new Rect(x, y, maxWidth, 18f), text, color, TextAnchor.MiddleLeft, GameFont.Tiny);
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x00181F9C File Offset: 0x0018019C
		private static Color[] BuildPalette(float lighten)
		{
			Color[] array = new Color[14];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Color.Lerp(ProfilerPalette.ColorAt(i), Color.white, lighten);
			}
			return array;
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x00181FD8 File Offset: 0x001801D8
		private static Color BaseColorFor(ProfilerLabelId labelId)
		{
			return ProfilerFlamechartWidget.BaseColors[ProfilerPalette.ColorIndexFor(labelId)];
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x00181FEC File Offset: 0x001801EC
		private void Paint()
		{
			float num = this.width;
			float num2 = this.height;
			bool flag = this.BuildFrameLayout();
			if (this.frameEntries.Count == 0 || num < 142f || num2 < 16f)
			{
				this.view.CancelAnimation();
				this.contentVisible = false;
				return;
			}
			this.contentVisible = true;
			ValueTuple<double, double, int, int> valueTuple = this.CurrentSelectionKey();
			bool flag2;
			if (!flag)
			{
				ValueTuple<double, double, int, int> valueTuple2 = valueTuple;
				ValueTuple<double, double, int, int> valueTuple3 = this.lastSelectionKey;
				flag2 = (valueTuple2.Item1 != valueTuple3.Item1 || valueTuple2.Item2 != valueTuple3.Item2 || valueTuple2.Item3 != valueTuple3.Item3 || valueTuple2.Item4 != valueTuple3.Item4);
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			this.lastSelectionKey = valueTuple;
			double num3;
			double num4;
			if (flag3 && this.suppressSelectionZoom)
			{
				this.suppressSelectionZoom = false;
			}
			else if (flag3 && this.TryGetSelectionRange(out num3, out num4))
			{
				double num5 = Math.Max((num4 - num3) * 0.35, 2000.0);
				this.view.TopLane = 0f;
				if (this.snapView)
				{
					this.view.SetTo(num3 - num5, num4 - num3 + num5 * 2.0);
				}
				else
				{
					this.view.AnimateTo(num3 - num5, num4 - num3 + num5 * 2.0, false);
				}
			}
			else if (!this.view.UserView)
			{
				ProfilerFlamechartWidget.FrameEntry frameEntry = this.frameEntries[this.frameEntries.Count - 1];
				this.view.Span = Math.Max(1000.0, frameEntry.endUs - frameEntry.startUs);
				this.view.Start = frameEntry.endUs - this.view.Span;
				this.view.TopLane = 0f;
			}
			if (!flag3 && !flag)
			{
				this.snapView = false;
			}
			this.view.Advance();
			this.view.ClampRange();
			this.LayoutBands();
			this.AdvanceBandLayout();
			this.ClampView();
			this.RebuildRuler(num);
			this.RebuildPausedGaps(num, num2);
			this.RebuildSelectionBand(num, num2);
			this.RebuildBandChrome(num, num2);
			this.RebuildPauseLane(num);
			IGuiBackend guiBackend = GuiBackend.Current;
			guiBackend.PushClipRect(new Rect(0f, ProfilerFlamechartWidget.PlotTop, num, Mathf.Max(0f, num2 - ProfilerFlamechartWidget.PlotTop)));
			this.RebuildZones(num, num2);
			guiBackend.PopClipRect();
			this.RebuildOutsideRange(num, num2);
			this.RebuildSelectionRects(num);
			this.RebuildScrollThumb(num, num2);
			guiBackend.PushClipRect(new Rect(0f, ProfilerFlamechartWidget.PlotTop, num, Mathf.Max(0f, num2 - ProfilerFlamechartWidget.PlotTop)));
			this.RebuildGutterLabels(num2);
			guiBackend.PopClipRect();
			this.RebuildGutter(num, num2);
			this.PaintHover();
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x001822CC File Offset: 0x001804CC
		private void RebuildGutter(float w, float h)
		{
			this.Quad(ProfilerFlamechartWidget.GutterLineColor, 126f, 0f, 127f, h);
			this.Quad(ProfilerFlamechartWidget.GutterLineColor, 126f, 17f, Mathf.Max(126f, w), 18f);
			if (FrameProfiler.RuntimePausesSupported)
			{
				this.Quad(ProfilerFlamechartWidget.GutterLineColor, 126f, ProfilerFlamechartWidget.PlotTop - 1f, Mathf.Max(126f, w), ProfilerFlamechartWidget.PlotTop);
			}
			if (!FrameProfiler.RuntimePausesSupported)
			{
				return;
			}
			ProfilerGui.Label(new Rect(8f, 17f, 24f, ProfilerFlamechartWidget.GcLaneHeight + 2f), "GC", ProfilerFlamechartWidget.LaneNameColor, TextAnchor.MiddleLeft, GameFont.Tiny);
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			if (this.metaPauseCount > 0)
			{
				ProfilerTreeModel.AppendMs(scratch, this.metaPauseMs, "0.00", " ");
			}
			else
			{
				scratch.Append('—');
			}
			ProfilerGui.Label(new Rect(30f, 17f, 92f, ProfilerFlamechartWidget.GcLaneHeight + 2f), scratch.ToString(), ProfilerFlamechartWidget.RulerTextColor, TextAnchor.MiddleLeft, GameFont.Tiny);
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x001823F0 File Offset: 0x001805F0
		private void RebuildGutterLabels(float h)
		{
			this.metaPauseMs = 0f;
			this.metaPauseCount = 0;
			for (int i = 0; i < this.bandCount; i++)
			{
				this.bandBusyMs[i] = 0f;
			}
			int num = (this.SelectedOffset >= 0 && this.SelectedOffset < this.frameEntries.Count) ? this.SelectedOffset : 0;
			int num2 = Mathf.Clamp(Mathf.Max(num, this.SelectedOldestOffset), num, this.frameEntries.Count - 1);
			for (int j = num; j <= num2; j++)
			{
				FrameProfiler.FrameSnapshot snapshot = this.frameEntries[this.frameEntries.Count - 1 - j].snapshot;
				this.metaPauseMs += snapshot.pauseMs;
				this.metaPauseCount += snapshot.pauseCount;
				this.bandBusyMs[0] += snapshot.totalMs;
				for (int k = 1; k < snapshot.sectionCount; k++)
				{
					int num3 = this.BandIndexForThread(snapshot.sections[k].threadId);
					if (num3 > 0)
					{
						this.bandBusyMs[num3] += (float)snapshot.sections[k].busyNanoseconds / 1000000f;
					}
				}
			}
			StringBuilder scratch = ProfilerGui.Scratch;
			for (int l = 0; l < this.bandCount; l++)
			{
				float num4 = this.BandLaneTop(l, 0);
				float num5 = num4 + this.bandAnimLanes[l] * 19f;
				if (num5 > ProfilerFlamechartWidget.PlotTop && num4 < h)
				{
					bool flag = this.bandCollapsed[l];
					bool flag2 = this.bandAnimLanes[l] < 1.5f;
					Color color = flag ? ProfilerFlamechartWidget.RulerTextColor : ProfilerFlamechartWidget.LaneNameColor;
					float num6 = Mathf.Max(num4, Mathf.Min(ProfilerFlamechartWidget.PlotTop, num5 - (flag2 ? 16f : 31f)));
					ProfilerGui.Arrow(new Rect(4f, num6 + (flag2 ? 1f : 3f), 14f, 16f), !flag, ProfilerFlamechartWidget.RulerTextColor);
					scratch.Clear();
					scratch.Append(FrameProfiler.GetLabel(this.bandNameLabelIds[l]));
					if (flag2)
					{
						scratch.Append(" (");
						ProfilerTreeModel.AppendMs(scratch, this.bandBusyMs[l], "0.0", " ");
						scratch.Append(')');
						ProfilerGui.Label(new Rect(21f, num6 + 1f, 101f, 16f), scratch.ToString(), color, TextAnchor.MiddleLeft, GameFont.Tiny);
					}
					else
					{
						ProfilerGui.Label(new Rect(21f, num6 + 2f, 101f, 16f), scratch.ToString(), color, TextAnchor.MiddleLeft, GameFont.Tiny);
						scratch.Clear();
						ProfilerTreeModel.AppendMs(scratch, this.bandBusyMs[l], "0.0", " ");
						if (l > 0)
						{
							scratch.Append(" busy");
						}
						ProfilerGui.Label(new Rect(21f, num6 + 16f, 101f, 16f), scratch.ToString(), ProfilerFlamechartWidget.RulerTextColor, TextAnchor.MiddleLeft, GameFont.Tiny);
					}
				}
			}
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x00182738 File Offset: 0x00180938
		private void RebuildBandChrome(float w, float h)
		{
			for (int i = 0; i < this.bandCount; i++)
			{
				float num = this.BandLaneTop(i, 0);
				float num2 = num + this.bandAnimLanes[i] * 19f;
				if (num2 > ProfilerFlamechartWidget.PlotTop && num < h)
				{
					if ((i & 1) == 1)
					{
						this.Quad(ProfilerFlamechartWidget.BandTintColor, 0f, Mathf.Max(ProfilerFlamechartWidget.PlotTop, num), w, Mathf.Min(h, num2 + this.BandGapFor(i) * 19f));
					}
					float num3 = num2 + this.BandGapFor(i) * 19f - 1f;
					if (i < this.bandCount - 1 && num3 > ProfilerFlamechartWidget.PlotTop && num3 < h)
					{
						this.Quad(ProfilerFlamechartWidget.BandSeparatorColor, 0f, num3, w, num3 + 1f);
					}
				}
			}
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x00182804 File Offset: 0x00180A04
		private void RebuildScrollThumb(float w, float h)
		{
			float num = h - ProfilerFlamechartWidget.PlotTop;
			float num2 = this.animTotalLanes * 19f;
			if (num > 0f && num2 > num)
			{
				float num3 = Mathf.Max(24f, num * num / num2);
				float num4 = ProfilerFlamechartWidget.PlotTop + this.view.TopLane * 19f / num2 * num;
				this.Quad(ProfilerFlamechartWidget.ScrollThumbColor, w - 4f, Mathf.Max(ProfilerFlamechartWidget.PlotTop, num4), w - 1f, Mathf.Min(h, num4 + num3));
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x06003D1D RID: 15645 RVA: 0x0018288B File Offset: 0x00180A8B
		private ProfilerFrameSource Source
		{
			get
			{
				if (!this.UseSpikes)
				{
					return ProfilerFrameSource.History;
				}
				return ProfilerFrameSource.Spikes;
			}
		}

		// Token: 0x06003D1E RID: 15646 RVA: 0x00182898 File Offset: 0x00180A98
		private bool TryGetSourceSnapshot(int offset, out FrameProfiler.FrameSnapshot snapshot)
		{
			return ProfilerFrameStore.TryGetAt(this.Source, offset, out snapshot);
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x06003D1F RID: 15647 RVA: 0x001828A7 File Offset: 0x00180AA7
		private int SourceCount
		{
			get
			{
				return ProfilerFrameStore.CountOf(this.Source);
			}
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x001828B4 File Offset: 0x00180AB4
		private bool BuildFrameLayout()
		{
			FrameProfiler.FrameSnapshot retainedSnapshot = this.RetainedSnapshot;
			int num = (retainedSnapshot != null) ? 1 : this.SourceCount;
			FrameProfiler.FrameSnapshot frameSnapshot;
			int num2 = (retainedSnapshot != null) ? retainedSnapshot.captureOrdinal : (this.TryGetSourceSnapshot(0, out frameSnapshot) ? frameSnapshot.captureOrdinal : -1);
			bool flag = this.UseSpikes != this.lastLayoutUseSpikes || retainedSnapshot != null != this.lastLayoutRetained;
			if (!flag && num2 == this.lastLayoutNewestOrdinal && num == this.lastLayoutCount)
			{
				return false;
			}
			this.newestZonesStale = true;
			if (flag)
			{
				this.view.UserView = false;
				this.view.TopLane = 0f;
				this.view.CancelAnimation();
				this.snapView = true;
			}
			bool flag2 = retainedSnapshot == null && !this.UseSpikes;
			long num3 = this.layoutOriginNs;
			int num4 = (flag2 && !flag) ? this.KeepAgedEntries(num, num2 - this.lastLayoutNewestOrdinal) : 0;
			if (num4 == 0)
			{
				this.ClearEntries();
				this.layoutOriginNs = -1L;
			}
			double num5 = 0.0;
			if (retainedSnapshot != null)
			{
				this.layoutOriginNs = retainedSnapshot.beginTimeNanoseconds;
				this.AddFrameEntry(retainedSnapshot, ref num5);
			}
			else if (!flag2)
			{
				for (int i = num - 1; i >= 0; i--)
				{
					FrameProfiler.FrameSnapshot frameSnapshot2;
					if (this.TryGetSourceSnapshot(i, out frameSnapshot2))
					{
						if (this.layoutOriginNs < 0L)
						{
							this.layoutOriginNs = frameSnapshot2.beginTimeNanoseconds;
						}
						this.AddFrameEntry(frameSnapshot2, ref num5);
					}
				}
			}
			else if (num4 == 0 || !this.AppendWallEntries(num - num4, true))
			{
				this.ClearEntries();
				this.layoutOriginNs = -1L;
				this.AppendWallEntries(num - 1, false);
			}
			int num6;
			if (this.frameEntries.Count <= 0)
			{
				num6 = num2;
			}
			else
			{
				List<ProfilerFlamechartWidget.FrameEntry> list = this.frameEntries;
				num6 = list[list.Count - 1].ordinal;
			}
			this.lastLayoutNewestOrdinal = num6;
			this.lastLayoutCount = ((this.frameEntries.Count > 0) ? this.frameEntries.Count : num);
			this.lastLayoutUseSpikes = this.UseSpikes;
			this.lastLayoutRetained = (retainedSnapshot != null);
			if (!flag && num3 >= 0L && this.layoutOriginNs >= 0L && this.layoutOriginNs != num3)
			{
				double num7 = (double)(this.layoutOriginNs - num3) / 1000.0;
				this.view.ShiftOrigin(num7);
				if (this.dragArmed || this.dragSelecting)
				{
					this.dragStartUs -= num7;
					this.dragEndUs -= num7;
				}
			}
			this.view.Extent = 1.0;
			for (int j = 0; j < this.frameEntries.Count; j++)
			{
				this.view.Extent = Math.Max(this.view.Extent, this.frameEntries[j].maxEndUs);
			}
			return flag;
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x00182B78 File Offset: 0x00180D78
		private int KeepAgedEntries(int count, int advanced)
		{
			int num = count - advanced;
			if (advanced <= 0 || num <= 0 || num > this.frameEntries.Count)
			{
				return 0;
			}
			int num2 = this.frameEntries.Count - num;
			if (this.frameEntries[num2].snapshot.captureOrdinal != this.frameEntries[num2].ordinal)
			{
				return 0;
			}
			this.DropEntries(num2);
			long beginTimeNanoseconds = this.frameEntries[0].snapshot.beginTimeNanoseconds;
			double num3 = (double)(beginTimeNanoseconds - this.layoutOriginNs) / 1000.0;
			if (num3 != 0.0)
			{
				Span<ProfilerFlamechartWidget.FrameEntry> span = CollectionsMarshal.AsSpan<ProfilerFlamechartWidget.FrameEntry>(this.frameEntries);
				for (int i = 0; i < span.Length; i++)
				{
					ref ProfilerFlamechartWidget.FrameEntry ptr = ref span[i];
					ptr.startUs -= num3;
					ptr.endUs -= num3;
					ptr.boundaryUs -= num3;
					ptr.maxEndUs -= num3;
				}
			}
			this.layoutOriginNs = beginTimeNanoseconds;
			return num;
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x00182C78 File Offset: 0x00180E78
		private bool AppendWallEntries(int fromOffset, bool incremental)
		{
			double num = 0.0;
			for (int i = Math.Min(fromOffset, this.SourceCount - 1); i >= 0; i--)
			{
				FrameProfiler.FrameSnapshot frameSnapshot;
				if (this.TryGetSourceSnapshot(i, out frameSnapshot))
				{
					if (incremental)
					{
						List<ProfilerFlamechartWidget.FrameEntry> list = this.frameEntries;
						int ordinal = list[list.Count - 1].ordinal;
						if (frameSnapshot.captureOrdinal <= ordinal)
						{
							goto IL_90;
						}
						if (frameSnapshot.captureOrdinal != ordinal + 1)
						{
							return false;
						}
					}
					if (this.layoutOriginNs < 0L)
					{
						this.layoutOriginNs = frameSnapshot.beginTimeNanoseconds;
					}
					num = (double)(frameSnapshot.beginTimeNanoseconds - this.layoutOriginNs) / 1000.0;
					this.AddFrameEntry(frameSnapshot, ref num);
				}
				IL_90:;
			}
			return true;
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x00182D1E File Offset: 0x00180F1E
		private void ClearEntries()
		{
			this.DropEntries(this.frameEntries.Count);
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x00182D34 File Offset: 0x00180F34
		private void DropEntries(int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.frameEntries[i].snapshot.Release();
				this.frameEntries[i].zones.Invalidate();
				this.frameZonesPool.Push(this.frameEntries[i].zones);
			}
			this.frameEntries.RemoveRange(0, count);
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x00182DA4 File Offset: 0x00180FA4
		private void AddFrameEntry(FrameProfiler.FrameSnapshot snapshot, ref double cursor)
		{
			snapshot.Retain();
			double num = Math.Max(1.0, (double)(snapshot.endTimeNanoseconds - snapshot.beginTimeNanoseconds) / 1000.0);
			double boundaryUs = cursor + Math.Max(num, (double)snapshot.nodesEndNanoseconds / 1000.0);
			double num2 = cursor + num;
			for (int i = 0; i < snapshot.sectionCount; i++)
			{
				num2 = Math.Max(num2, cursor + (double)snapshot.sections[i].endNanoseconds / 1000.0);
			}
			for (int j = 0; j < snapshot.pauseCount; j++)
			{
				num2 = Math.Max(num2, cursor + (double)snapshot.pauses[j].endMicroseconds);
			}
			this.frameEntries.Add(new ProfilerFlamechartWidget.FrameEntry
			{
				snapshot = snapshot,
				ordinal = snapshot.captureOrdinal,
				startUs = cursor,
				endUs = cursor + num,
				boundaryUs = boundaryUs,
				maxEndUs = num2,
				zones = ((this.frameZonesPool.Count > 0) ? this.frameZonesPool.Pop() : new ProfilerFlamechartWidget.FrameZones())
			});
			cursor += num;
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x00182EE2 File Offset: 0x001810E2
		private long EntryTimeNs(in ProfilerFlamechartWidget.FrameEntry entry, double frameUs)
		{
			return entry.snapshot.beginTimeNanoseconds + (long)((frameUs - entry.startUs) * 1000.0);
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x00182F04 File Offset: 0x00181104
		[return: TupleElementNames(new string[]
		{
			"rangeStart",
			"rangeEnd",
			"newestOrdinal",
			"oldestOrdinal"
		})]
		private ValueTuple<double, double, int, int> CurrentSelectionKey()
		{
			int item = -1;
			int item2 = -1;
			int count = this.frameEntries.Count;
			int selectedOffset = this.SelectedOffset;
			int num = Mathf.Max(selectedOffset, this.SelectedOldestOffset);
			if (selectedOffset >= 0 && selectedOffset < count && num < count)
			{
				item = this.frameEntries[count - 1 - selectedOffset].snapshot.captureOrdinal;
				item2 = this.frameEntries[count - 1 - num].snapshot.captureOrdinal;
			}
			return new ValueTuple<double, double, int, int>(this.SelectionRangeStart, this.SelectionRangeEnd, item, item2);
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x00182F90 File Offset: 0x00181190
		private bool TryGetSelectionRange(out double startUs, out double endUs)
		{
			startUs = 0.0;
			endUs = 0.0;
			if (this.dragSelecting)
			{
				startUs = Math.Min(this.dragStartUs, this.dragEndUs);
				endUs = Math.Max(this.dragStartUs, this.dragEndUs);
				return endUs > startUs;
			}
			if (this.SelectionRangeStart >= 0.0 && this.SelectionRangeEnd > this.SelectionRangeStart && this.layoutOriginNs >= 0L)
			{
				startUs = (this.SelectionRangeStart - (double)this.layoutOriginNs) / 1000.0;
				endUs = (this.SelectionRangeEnd - (double)this.layoutOriginNs) / 1000.0;
				return true;
			}
			int count = this.frameEntries.Count;
			int selectedOffset = this.SelectedOffset;
			int num = Mathf.Max(selectedOffset, this.SelectedOldestOffset);
			if (selectedOffset < 0 || selectedOffset >= count || num >= count)
			{
				return false;
			}
			startUs = this.frameEntries[count - 1 - num].startUs;
			endUs = this.frameEntries[count - 1 - selectedOffset].endUs;
			return endUs > startUs;
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x001830A9 File Offset: 0x001812A9
		private static float PlotWidth(float w)
		{
			return w - 126f;
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x001830B2 File Offset: 0x001812B2
		private float TimeToX(double timeUs, float w)
		{
			return this.view.ToX(timeUs, 126f, ProfilerFlamechartWidget.PlotWidth(w));
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x001830CB File Offset: 0x001812CB
		private double XToTime(float x, float w)
		{
			return this.view.ToValue(x, 126f, ProfilerFlamechartWidget.PlotWidth(w));
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x001830E4 File Offset: 0x001812E4
		private int BandIndexForThread(int threadId)
		{
			for (int i = 0; i < this.bandCount; i++)
			{
				if (this.bandThreadIds[i] == threadId)
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x00183110 File Offset: 0x00181310
		private int SectionIndexForBand(in ProfilerFlamechartWidget.FrameEntry entry, int band)
		{
			FrameProfiler.FrameSnapshot snapshot = entry.snapshot;
			if (band < snapshot.sectionCount && snapshot.sections[band].threadId == this.bandThreadIds[band])
			{
				return band;
			}
			for (int i = 0; i < snapshot.sectionCount; i++)
			{
				if (snapshot.sections[i].threadId == this.bandThreadIds[band])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x00183179 File Offset: 0x00181379
		private void FrameRangeFor(in ProfilerFlamechartWidget.FrameEntry entry, int band, out long startNs, out long endNs, out double startUs, out double endUs)
		{
			startUs = entry.startUs;
			endUs = ((band == 0) ? entry.endUs : entry.boundaryUs);
			startNs = entry.snapshot.beginTimeNanoseconds;
			endNs = this.EntryTimeNs(entry, endUs);
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x001831B4 File Offset: 0x001813B4
		private void CollectZones(in ProfilerFlamechartWidget.FrameEntry entry, int band, double minVisibleUs, List<ProfilerFlamechartWidget.ZoneDraw> into)
		{
			into.Clear();
			long num;
			long num2;
			double num3;
			double num4;
			this.FrameRangeFor(entry, band, out num, out num2, out num3, out num4);
			FrameProfiler.CollectChunks(entry.snapshot, this.bandThreadIds[band], num, num2, this.chunkScratch);
			foreach (ZoneChunk zoneChunk in this.chunkScratch)
			{
				ZoneNode[] nodes = zoneChunk.nodes;
				int num5 = Math.Min(zoneChunk.Count, nodes.Length);
				this.ancestorEnds.Clear();
				int i = 0;
				while (i < num5)
				{
					while (this.ancestorEnds.Count > 0)
					{
						int num6 = i;
						List<int> list = this.ancestorEnds;
						if (num6 < list[list.Count - 1])
						{
							break;
						}
						this.ancestorEnds.RemoveAt(this.ancestorEnds.Count - 1);
					}
					ref ZoneNode ptr = ref nodes[i];
					bool flag = !ptr.IsOpen && ptr.subtreeEnd > i;
					int num7 = flag ? ptr.subtreeEnd : num5;
					long num8 = zoneChunk.baseNs + ptr.StartNanoseconds;
					long num9 = flag ? (zoneChunk.baseNs + ptr.EndNanoseconds) : num2;
					if (num8 >= num2 || num9 <= num)
					{
						i = num7;
					}
					else
					{
						int depth = this.ancestorEnds.Count + 1;
						double num10 = num3 + (double)(Math.Max(num8, num) - num) / 1000.0;
						double num11 = num3 + (double)(Math.Min(num9, num2) - num) / 1000.0;
						bool flag2 = num11 - num10 < minVisibleUs;
						into.Add(new ProfilerFlamechartWidget.ZoneDraw
						{
							chunk = zoneChunk,
							index = i,
							depth = depth,
							startUs = num10,
							endUs = Math.Max(num10, num11),
							folded = flag2
						});
						if (flag2)
						{
							i = num7;
						}
						else
						{
							this.ancestorEnds.Add(num7);
							i++;
						}
					}
				}
			}
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x001833DC File Offset: 0x001815DC
		private List<ProfilerFlamechartWidget.ZoneDraw> CollectZonesCached(in ProfilerFlamechartWidget.FrameEntry entry, int frameIndex, int band, double minVisibleUs)
		{
			ProfilerFlamechartWidget.FrameZones zones = entry.zones;
			if (zones.perBand.Length <= band)
			{
				int num = zones.perBand.Length;
				int num2 = Mathf.NextPowerOfTwo(band + 1);
				Array.Resize<List<ProfilerFlamechartWidget.ZoneDraw>>(ref zones.perBand, num2);
				Array.Resize<double>(ref zones.minVisibleUs, num2);
				Array.Fill<double>(zones.minVisibleUs, double.NaN, num, num2 - num);
			}
			List<ProfilerFlamechartWidget.ZoneDraw>[] perBand = zones.perBand;
			List<ProfilerFlamechartWidget.ZoneDraw> list;
			if ((list = perBand[band]) == null)
			{
				list = (perBand[band] = new List<ProfilerFlamechartWidget.ZoneDraw>());
			}
			List<ProfilerFlamechartWidget.ZoneDraw> list2 = list;
			if (zones.minVisibleUs[band] != minVisibleUs)
			{
				this.CollectZones(entry, band, minVisibleUs, list2);
				zones.minVisibleUs[band] = minVisibleUs;
			}
			return list2;
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x00183484 File Offset: 0x00181684
		private double MinVisibleUs(float w)
		{
			double a = this.view.Span * 3.0 / (double)Mathf.Max(1f, ProfilerFlamechartWidget.PlotWidth(w));
			return Math.Pow(2.0, Math.Floor(Math.Log(a, 2.0) * 4.0) / 4.0);
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x001834EF File Offset: 0x001816EF
		private bool FrameDetailed(in ProfilerFlamechartWidget.FrameEntry entry, float w)
		{
			return (entry.endUs - entry.startUs) / this.view.Span * (double)ProfilerFlamechartWidget.PlotWidth(w) >= 6.0;
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x00183520 File Offset: 0x00181720
		private void LayoutBands()
		{
			bool flag = this.lastBandRegistryVersion != ProfilerThreadRegistry.Version;
			if (flag)
			{
				for (int i = 0; i < this.frameEntries.Count; i++)
				{
					this.frameEntries[i].zones.Invalidate();
				}
			}
			else if (this.newestZonesStale)
			{
				for (int j = Mathf.Max(0, this.frameEntries.Count - 2); j < this.frameEntries.Count; j++)
				{
					this.frameEntries[j].zones.Invalidate();
				}
			}
			this.newestZonesStale = false;
			IReadOnlyList<ProfilerThreadRegistry.Entry> entries = ProfilerThreadRegistry.Entries;
			this.bandCount = Mathf.Max(1, entries.Count);
			if (this.bandLaneStart.Length < this.bandCount)
			{
				int num = Mathf.NextPowerOfTwo(this.bandCount);
				this.bandLaneStart = new int[num];
				this.bandTopLane = new float[num];
				this.bandLanes = new int[num];
				this.bandCollapsed = new bool[num];
				this.bandVisibleDepth = new int[num];
				this.bandBusyMs = new float[num];
				this.bandThreadIds = new int[num];
				this.bandNameLabelIds = new ProfilerLabelId[num];
				this.bandStickyDepth = new int[num];
				this.bandShrinkDeadline = new long[num];
				this.bandAnimStart = new float[num];
				this.bandAnimLanes = new float[num];
			}
			if (flag)
			{
				this.lastBandRegistryVersion = ProfilerThreadRegistry.Version;
				Array.Clear(this.bandStickyDepth, 0, this.bandStickyDepth.Length);
				Array.Clear(this.bandShrinkDeadline, 0, this.bandShrinkDeadline.Length);
				this.bandLayoutSnapPending = true;
			}
			for (int k = 0; k < this.bandCount; k++)
			{
				this.bandVisibleDepth[k] = 0;
				if (entries.Count > 0)
				{
					this.bandThreadIds[k] = entries[k].threadId;
					this.bandNameLabelIds[k] = entries[k].nameLabelId;
				}
			}
			if (entries.Count == 0 && this.frameEntries.Count > 0)
			{
				List<ProfilerFlamechartWidget.FrameEntry> list = this.frameEntries;
				FrameProfiler.FrameSnapshot snapshot = list[list.Count - 1].snapshot;
				this.bandThreadIds[0] = ((snapshot.sectionCount > 0) ? snapshot.sections[0].threadId : 0);
				this.bandNameLabelIds[0] = ((snapshot.sectionCount > 0) ? snapshot.sections[0].nameLabelId : ProfilerLabelId.None);
			}
			float w = this.width;
			Mathf.Max(1f, ProfilerFlamechartWidget.PlotWidth(w));
			double minVisibleUs = this.MinVisibleUs(w);
			double num2 = this.view.Start + this.view.Span;
			for (int l = 0; l < this.frameEntries.Count; l++)
			{
				ProfilerFlamechartWidget.FrameEntry frameEntry = this.frameEntries[l];
				if (frameEntry.maxEndUs > this.view.Start && frameEntry.startUs < num2 && this.FrameDetailed(frameEntry, w))
				{
					for (int m = 0; m < this.bandCount; m++)
					{
						if (!this.bandCollapsed[m] && this.SectionIndexForBand(frameEntry, m) >= 0)
						{
							List<ProfilerFlamechartWidget.ZoneDraw> list2 = this.CollectZonesCached(frameEntry, l, m, minVisibleUs);
							for (int n = 0; n < list2.Count; n++)
							{
								ProfilerFlamechartWidget.ZoneDraw zoneDraw = list2[n];
								if (!zoneDraw.folded && zoneDraw.depth > this.bandVisibleDepth[m] && zoneDraw.endUs > this.view.Start && zoneDraw.startUs < num2)
								{
									this.bandVisibleDepth[m] = zoneDraw.depth;
								}
							}
						}
					}
				}
			}
			bool flag2 = Math.Abs(this.view.Start - this.lastLayoutViewStartUs) > this.view.Span * 0.001 || Math.Abs(this.view.Span - this.lastLayoutViewSpanUs) > this.view.Span * 0.001;
			this.lastLayoutViewStartUs = this.view.Start;
			this.lastLayoutViewSpanUs = this.view.Span;
			bool flag3 = this.view.UserView && flag2;
			long timestamp = Stopwatch.GetTimestamp();
			int num3 = 0;
			float num4 = 0f;
			for (int num5 = 0; num5 < this.bandCount; num5++)
			{
				if (flag3 || this.bandVisibleDepth[num5] >= this.bandStickyDepth[num5])
				{
					this.bandStickyDepth[num5] = this.bandVisibleDepth[num5];
					this.bandShrinkDeadline[num5] = 0L;
				}
				else if (this.bandShrinkDeadline[num5] == 0L)
				{
					this.bandShrinkDeadline[num5] = timestamp + ProfilerFlamechartWidget.BandShrinkDelayTicks;
				}
				else if (timestamp > this.bandShrinkDeadline[num5])
				{
					this.bandStickyDepth[num5] = this.bandVisibleDepth[num5];
					this.bandShrinkDeadline[num5] = 0L;
				}
				this.bandLaneStart[num5] = num3;
				this.bandTopLane[num5] = num4;
				this.bandCollapsed[num5] = this.collapsedThreadIds.Contains(this.bandThreadIds[num5]);
				this.bandLanes[num5] = (this.bandCollapsed[num5] ? 1 : Mathf.Max(1, this.bandStickyDepth[num5] + 1));
				num3 += this.bandLanes[num5];
				num4 += (float)this.bandLanes[num5] + this.BandGapFor(num5);
			}
			this.totalIndexLanes = num3;
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x00183AC7 File Offset: 0x00181CC7
		private void ToggleBandCollapsed(int band)
		{
			if (!this.collapsedThreadIds.Add(this.bandThreadIds[band]))
			{
				this.collapsedThreadIds.Remove(this.bandThreadIds[band]);
			}
			this.anchorThreadId = -1;
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x00183AFC File Offset: 0x00181CFC
		private void AdvanceBandLayout()
		{
			long timestamp = Stopwatch.GetTimestamp();
			float num = Mathf.Clamp((float)((double)(timestamp - this.bandAnimTimestamp) / (double)Stopwatch.Frequency), 0f, 0.1f);
			this.bandAnimTimestamp = timestamp;
			float num2 = this.bandLayoutSnapPending ? 1f : (1f - Mathf.Exp(-num * 18f));
			this.bandLayoutSnapPending = false;
			bool flag = false;
			for (int i = 0; i < this.bandCount; i++)
			{
				this.bandAnimStart[i] += (this.bandTopLane[i] - this.bandAnimStart[i]) * num2;
				this.bandAnimLanes[i] += ((float)this.bandLanes[i] - this.bandAnimLanes[i]) * num2;
				if (Mathf.Abs(this.bandAnimStart[i] - this.bandTopLane[i]) > 0.01f || Mathf.Abs(this.bandAnimLanes[i] - (float)this.bandLanes[i]) > 0.01f)
				{
					flag = true;
				}
				else
				{
					this.bandAnimStart[i] = this.bandTopLane[i];
					this.bandAnimLanes[i] = (float)this.bandLanes[i];
				}
			}
			this.view.Animation.SetContentAnimating(flag);
			int num3 = this.bandCount - 1;
			this.animTotalLanes = this.bandAnimStart[num3] + this.bandAnimLanes[num3] + this.BandGapFor(num3);
			if (this.anchorThreadId >= 0)
			{
				for (int j = 0; j < this.bandCount; j++)
				{
					if (this.bandThreadIds[j] == this.anchorThreadId)
					{
						this.view.TopLane = this.bandAnimStart[j] + this.anchorIntraLanes - (this.anchorViewY - ProfilerFlamechartWidget.PlotTop) / 19f;
						break;
					}
				}
				if (!flag && !this.view.Animation.Active)
				{
					this.anchorThreadId = -1;
				}
			}
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x00183CEC File Offset: 0x00181EEC
		private bool TryFindBand(float lane, out int bandIndex, out float intraLanes)
		{
			for (int i = 0; i < this.bandCount; i++)
			{
				float num = lane - this.bandAnimStart[i];
				if (num >= 0f && num < this.bandAnimLanes[i] + this.BandGapFor(i))
				{
					bandIndex = i;
					intraLanes = num;
					return true;
				}
			}
			bandIndex = 0;
			intraLanes = 0f;
			return false;
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x00183D44 File Offset: 0x00181F44
		private bool TryFindBandForLane(float lane, out int bandIndex, out int intraDepth)
		{
			float num;
			if (this.TryFindBand(lane, out bandIndex, out num) && !this.bandCollapsed[bandIndex] && num < Mathf.Min((float)this.bandLanes[bandIndex], this.bandAnimLanes[bandIndex]))
			{
				intraDepth = Mathf.FloorToInt(num);
				return true;
			}
			intraDepth = 0;
			return false;
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x00183D91 File Offset: 0x00181F91
		private float BandLaneTop(int band, int intraDepth)
		{
			return ProfilerFlamechartWidget.PlotTop + (this.bandAnimStart[band] + (float)intraDepth - this.view.TopLane) * 19f;
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x00183DB8 File Offset: 0x00181FB8
		private void RebuildZones(float w, float h)
		{
			this.pauseNodes.Clear();
			Mathf.Max(0, Mathf.FloorToInt(this.view.TopLane));
			int num = Mathf.Max(this.totalIndexLanes, Mathf.CeilToInt(this.view.TopLane + (h - ProfilerFlamechartWidget.PlotTop) / 19f));
			if (this.laneLabelEnds.Length <= num || this.pendingRuns.Length < this.laneLabelEnds.Length)
			{
				int num2 = Mathf.NextPowerOfTwo(Mathf.Max(num + 1, this.laneLabelEnds.Length));
				this.laneLabelEnds = new float[num2];
				this.pendingRuns = new ProfilerFlamechartWidget.CollapsedRun[num2];
			}
			this.collapsedRuns.Clear();
			for (int i = 0; i < this.laneLabelEnds.Length; i++)
			{
				this.laneLabelEnds[i] = float.MinValue;
				this.pendingRuns[i].count = 0;
			}
			int num3 = 0;
			double num4 = this.view.Start + this.view.Span;
			double minVisibleUs = this.MinVisibleUs(w);
			for (int j = 0; j < this.frameEntries.Count; j++)
			{
				ProfilerFlamechartWidget.FrameEntry frameEntry = this.frameEntries[j];
				if (frameEntry.maxEndUs > this.view.Start && frameEntry.startUs < num4)
				{
					float num5 = this.TimeToX(frameEntry.startUs, w);
					float num6 = this.TimeToX(frameEntry.endUs, w);
					if (num5 >= 126f && num5 <= w && num6 - num5 >= 28f)
					{
						this.Quad(ProfilerFlamechartWidget.FrameSeparatorColor, num5, 18f, num5 + 1f, Mathf.Max(18f, h));
					}
					bool flag = this.FrameDetailed(frameEntry, w);
					for (int k = 0; k < this.bandCount; k++)
					{
						if (!this.bandCollapsed[k])
						{
							float num7 = this.BandLaneTop(k, 0);
							bool flag2 = num7 + 19f > ProfilerFlamechartWidget.PlotTop && num7 < h;
							if (flag || flag2)
							{
								int num8 = this.SectionIndexForBand(frameEntry, k);
								if (num8 >= 0)
								{
									ref FrameProfiler.ThreadSection ptr = ref frameEntry.snapshot.sections[num8];
									double num9 = (k == 0) ? frameEntry.startUs : (frameEntry.startUs + (double)ptr.startNanoseconds / 1000.0);
									double num10 = (k == 0) ? frameEntry.endUs : (frameEntry.startUs + (double)ptr.endNanoseconds / 1000.0);
									bool flag3 = flag2 && num10 > this.view.Start && num9 < num4;
									ProfilerLabelId labelId = (k == 0) ? FrameProfiler.RootLabelId : ptr.nameLabelId;
									Color color = ProfilerFlamechartWidget.BaseColorFor(labelId);
									if (!flag)
									{
										if (flag3)
										{
											this.AccumulateCollapsed(this.bandLaneStart[k], num7, Mathf.Max(126f, this.TimeToX(num9, w)), Mathf.Min(w, this.TimeToX(num10, w)), (k == 0) ? (frameEntry.snapshot.totalMs * 1000f) : ((float)ptr.busyNanoseconds * 0.001f), color, j, k, null, -1);
										}
									}
									else
									{
										if (flag3)
										{
											float x = Mathf.Max(126f, this.TimeToX(num9, w));
											float x2 = Mathf.Min(w, this.TimeToX(num10, w));
											string name = (k == 0) ? this.FrameName(frameEntry.snapshot.frameIndex) : FrameProfiler.GetLabel(ptr.nameLabelId);
											float durationUs = (k == 0) ? (frameEntry.snapshot.totalMs * 1000f) : ((float)ptr.busyNanoseconds * 0.001f);
											this.DrawZoneQuad(ProfilerPalette.ColorIndexFor(labelId), x, x2, num7, h, this.bandLaneStart[k], ref num3, name, (k == 0) ? ProfilerLabelId.None : ptr.nameLabelId, durationUs);
										}
										List<ProfilerFlamechartWidget.ZoneDraw> list = this.CollectZonesCached(frameEntry, j, k, minVisibleUs);
										for (int l = 0; l < list.Count; l++)
										{
											ProfilerFlamechartWidget.ZoneDraw zoneDraw = list[l];
											if (zoneDraw.depth < this.bandLanes[k] && (float)zoneDraw.depth < this.bandAnimLanes[k])
											{
												int lane = this.bandLaneStart[k] + zoneDraw.depth;
												float num11 = this.BandLaneTop(k, zoneDraw.depth);
												if (num11 + 19f > ProfilerFlamechartWidget.PlotTop && num11 < h && zoneDraw.endUs > this.view.Start && zoneDraw.startUs < num4)
												{
													float x3 = Mathf.Max(126f, this.TimeToX(zoneDraw.startUs, w));
													float x4 = Mathf.Min(w, this.TimeToX(zoneDraw.endUs, w));
													ProfilerLabelId profilerLabelId = FrameProfiler.PathLabel(zoneDraw.chunk.nodes[zoneDraw.index].pathId);
													if (zoneDraw.folded)
													{
														this.AccumulateCollapsed(lane, num11, x3, x4, (float)(zoneDraw.endUs - zoneDraw.startUs), ProfilerFlamechartWidget.BaseColorFor(profilerLabelId), j, k, zoneDraw.chunk, zoneDraw.index);
													}
													else
													{
														this.DrawZoneQuad(ProfilerPalette.ColorIndexFor(profilerLabelId), x3, x4, num11, h, lane, ref num3, FrameProfiler.GetLabel(profilerLabelId), profilerLabelId, this.ZoneDurationUs(zoneDraw.chunk, zoneDraw.index, frameEntry, k));
													}
												}
											}
										}
										if (k == 0)
										{
											this.DrawPauseNodes(frameEntry, j, list, w, h, num4);
										}
									}
								}
							}
						}
					}
				}
			}
			for (int m = 0; m < this.pendingRuns.Length; m++)
			{
				this.FlushCollapsed(m);
			}
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x00184374 File Offset: 0x00182574
		private void DrawZoneQuad(int colorIndex, float x0, float x1, float laneY, float h, int lane, ref int labelCount, string name, ProfilerLabelId nameId, float durationUs)
		{
			float num = Mathf.Max(ProfilerFlamechartWidget.PlotTop, laneY);
			float num2 = Mathf.Min(h, laneY + 19f - 1f);
			if (x1 - x0 > 4f && num2 - num > 3f)
			{
				this.Quad(ProfilerFlamechartWidget.LightColors[colorIndex], x0 + 0.5f, num, x1 - 0.5f, num2);
				this.Quad(ProfilerFlamechartWidget.BaseColors[colorIndex], x0 + 1.5f, num + 1f, x1 - 1.5f, num2 - 1f);
			}
			else
			{
				this.Quad(ProfilerFlamechartWidget.BaseColors[colorIndex], x0, num, x1, num2);
			}
			if (x1 - x0 < 40f || labelCount >= 160 || lane >= this.laneLabelEnds.Length || (x0 < this.laneLabelEnds[lane] - 0.5f && this.OverlapsDrawnLabel(labelCount, lane, x0, x1)))
			{
				return;
			}
			this.laneLabelEnds[lane] = Mathf.Max(this.laneLabelEnds[lane], x1);
			this.labelZoneX0[labelCount] = x0;
			this.labelZoneX1[labelCount] = x1;
			this.labelZoneDepth[labelCount] = lane;
			labelCount++;
			float num3 = x0 + 3f;
			float num4 = x1 - x0 - 6f;
			float num5 = this.NameWidth(name, nameId);
			if (num5 + 4f + 30f <= num4)
			{
				this.durationScratch.Clear();
				ProfilerTreeModel.AppendMs(this.durationScratch, durationUs / 1000f, "0.0", "");
				string text = this.durationScratch.ToString();
				if (num5 + 4f + ProfilerGui.Width(text, GameFont.Tiny) <= num4)
				{
					ProfilerFlamechartWidget.Text(name, num3, laneY, num5 + 4f, ProfilerPalette.ZoneLabelColor);
					ProfilerFlamechartWidget.Text(text, num3 + num5 + 4f, laneY, num4 - num5 - 4f + 4f, ProfilerPalette.ZoneLabelColor);
					return;
				}
			}
			ProfilerFlamechartWidget.Text(name, num3, laneY, num4, ProfilerPalette.ZoneLabelColor);
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00184570 File Offset: 0x00182770
		private float NameWidth(string name, ProfilerLabelId nameId)
		{
			if (!nameId.IsValid)
			{
				return ProfilerGui.Width(name, GameFont.Tiny);
			}
			float result;
			if (!this.labelWidths.TryGetValue(nameId.Value, out result))
			{
				result = (this.labelWidths[nameId.Value] = ProfilerGui.Width(name, GameFont.Tiny));
			}
			return result;
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x001845C0 File Offset: 0x001827C0
		private string FrameName(int frameIndex)
		{
			string result;
			if (this.frameNames.TryGetValue(frameIndex, out result))
			{
				return result;
			}
			if (this.frameNames.Count > 512)
			{
				this.frameNames.Clear();
			}
			return this.frameNames[frameIndex] = "Frame " + frameIndex.ToString();
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x0018461C File Offset: 0x0018281C
		private float ZoneDurationUs(ZoneChunk chunk, int index, in ProfilerFlamechartWidget.FrameEntry entry, int band)
		{
			ref ZoneNode ptr = ref chunk.nodes[index];
			if (!ptr.IsOpen)
			{
				return (float)((double)(ptr.EndNanoseconds - ptr.StartNanoseconds) / 1000.0);
			}
			long num;
			long num2;
			double num3;
			double num4;
			this.FrameRangeFor(entry, band, out num, out num2, out num3, out num4);
			return (float)Math.Max(0.0, (double)(num2 - (chunk.baseNs + ptr.StartNanoseconds)) / 1000.0);
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x00184694 File Offset: 0x00182894
		private static float ZoneSelfUs(ZoneChunk chunk, int index)
		{
			ZoneNode[] nodes = chunk.nodes;
			ref ZoneNode ptr = ref nodes[index];
			long num = ptr.EndNanoseconds - ptr.StartNanoseconds;
			int num2 = Math.Min(ptr.subtreeEnd, Math.Min(chunk.Count, nodes.Length));
			ZoneNode ptr2;
			for (int i = index + 1; i < num2; i = ptr2.subtreeEnd)
			{
				ptr2 = ref nodes[i];
				if (ptr2.IsOpen || ptr2.subtreeEnd <= i)
				{
					break;
				}
				num -= ptr2.EndNanoseconds - ptr2.StartNanoseconds;
			}
			return (float)((double)Math.Max(0L, num) / 1000.0);
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00184734 File Offset: 0x00182934
		private void RebuildPauseLane(float w)
		{
			double num = this.view.Start + this.view.Span;
			for (int i = 0; i < this.frameEntries.Count; i++)
			{
				ProfilerFlamechartWidget.FrameEntry frameEntry = this.frameEntries[i];
				if (frameEntry.maxEndUs > this.view.Start && frameEntry.startUs < num)
				{
					this.DrawPauseLane(frameEntry, w, num);
				}
			}
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x001847A4 File Offset: 0x001829A4
		private void DrawPauseLane(in ProfilerFlamechartWidget.FrameEntry entry, float w, double viewEndUs)
		{
			for (int i = 0; i < entry.snapshot.pauseCount; i++)
			{
				FrameProfiler.PauseSpan pauseSpan = entry.snapshot.pauses[i];
				double num = entry.startUs + (double)pauseSpan.startMicroseconds;
				double num2 = entry.startUs + (double)pauseSpan.endMicroseconds;
				if (num2 > this.view.Start && num < viewEndUs)
				{
					float num3 = Mathf.Max(126f, this.TimeToX(num, w));
					float num4 = Mathf.Min(w, Mathf.Max(num3 + 1f, this.TimeToX(num2, w)));
					Color color = ProfilerFlamechartWidget.IsGcReason(pauseSpan.reason) ? ProfilerFlamechartWidget.GcPauseColor : ProfilerFlamechartWidget.OtherPauseColor;
					float num5 = Mathf.Clamp(this.TimeToX(entry.startUs + (double)pauseSpan.suspendedMicroseconds, w), num3, num4);
					if (num5 - num3 >= 1f && num4 - num5 >= 1f)
					{
						this.Quad(ProfilerFlamechartWidget.PauseWaitColor, num3, 20f, num5, ProfilerFlamechartWidget.PlotTop - 2f);
						this.Quad(color, num5, 20f, num4, ProfilerFlamechartWidget.PlotTop - 2f);
					}
					else
					{
						this.Quad(color, num3, 20f, num4, ProfilerFlamechartWidget.PlotTop - 2f);
					}
				}
			}
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x001848F4 File Offset: 0x00182AF4
		private void DrawPauseNodes(in ProfilerFlamechartWidget.FrameEntry entry, int frameIndex, List<ProfilerFlamechartWidget.ZoneDraw> zones, float w, float h, double viewEndUs)
		{
			FrameProfiler.FrameSnapshot snapshot = entry.snapshot;
			for (int i = 0; i < snapshot.pauseCount; i++)
			{
				FrameProfiler.PauseSpan pauseSpan = snapshot.pauses[i];
				double num = entry.startUs + (double)pauseSpan.suspendedMicroseconds;
				double num2 = entry.startUs + (double)pauseSpan.endMicroseconds;
				if (num2 > num && num2 > this.view.Start && num < viewEndUs)
				{
					int num3 = 1;
					for (int j = 0; j < zones.Count; j++)
					{
						ProfilerFlamechartWidget.ZoneDraw zoneDraw = zones[j];
						if (!zoneDraw.folded && zoneDraw.startUs <= num && zoneDraw.endUs >= num2 && zoneDraw.depth + 1 > num3)
						{
							num3 = zoneDraw.depth + 1;
						}
					}
					if (num3 < this.bandLanes[0] && (float)num3 < this.bandAnimLanes[0])
					{
						float num4 = this.BandLaneTop(0, num3);
						if (num4 + 19f > ProfilerFlamechartWidget.PlotTop && num4 < h)
						{
							float num5 = Mathf.Max(126f, this.TimeToX(num, w));
							float num6 = Mathf.Min(w, Mathf.Max(num5 + 1f, this.TimeToX(num2, w)));
							ProfilerLabelId pauseLabelId = FrameProfiler.GetPauseLabelId(pauseSpan.reason, pauseSpan.condemnedGeneration);
							int num7 = 160;
							if (num6 - num5 >= 40f)
							{
								num7 = 0;
							}
							this.DrawZoneQuad(ProfilerPalette.ColorIndexFor(pauseLabelId), num5, num6, num4, h, this.bandLaneStart[0] + num3, ref num7, FrameProfiler.GetLabel(pauseLabelId), pauseLabelId, (float)(num2 - num));
							this.pauseNodes.Add(new ProfilerFlamechartWidget.PauseNode
							{
								frameIndex = frameIndex,
								pauseIndex = i,
								x0 = num5,
								x1 = num6,
								laneY = num4
							});
						}
					}
				}
			}
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x00184AD8 File Offset: 0x00182CD8
		private void AccumulateCollapsed(int lane, float laneY, float x0, float x1, float totalMicroseconds, Color color, int frameIndex, int band, ZoneChunk chunk, int nodeIndex)
		{
			if (lane >= this.pendingRuns.Length)
			{
				return;
			}
			ref ProfilerFlamechartWidget.CollapsedRun ptr = ref this.pendingRuns[lane];
			if (ptr.count > 0 && x0 - ptr.x1 > 3f)
			{
				this.FlushCollapsed(lane);
			}
			if (ptr.count == 0)
			{
				ptr.x0 = x0;
				ptr.x1 = float.MinValue;
				ptr.color = color;
				ptr.frameIndex = frameIndex;
				ptr.band = band;
				ptr.chunk = chunk;
				ptr.nodeIndex = nodeIndex;
			}
			ptr.laneY = laneY;
			ptr.x1 = Mathf.Max(ptr.x1, Mathf.Max(x1, x0 + 3f));
			ptr.count++;
			ptr.totalMicroseconds += (double)totalMicroseconds;
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x00184BA0 File Offset: 0x00182DA0
		private void FlushCollapsed(int lane)
		{
			ref ProfilerFlamechartWidget.CollapsedRun ptr = ref this.pendingRuns[lane];
			if (ptr.count == 0)
			{
				return;
			}
			float num = Mathf.Max(ProfilerFlamechartWidget.PlotTop, ptr.laneY);
			float num2 = Mathf.Min(this.height, ptr.laneY + 19f - 1f);
			if (num2 > num)
			{
				this.Quad(ptr.color, ptr.x0, num, ptr.x1, num2);
				float num3 = ptr.laneY + 9f;
				float num4 = Mathf.Max(ProfilerFlamechartWidget.PlotTop, num3 - 2f);
				float num5 = Mathf.Min(this.height, num3 + 2f);
				if (num5 > num4)
				{
					this.Quad(ProfilerFlamechartWidget.CollapsedRunColor, ptr.x0, num4, ptr.x1, num5);
				}
				if (this.collapsedRuns.Count < 4096)
				{
					this.collapsedRuns.Add(ptr);
				}
			}
			ptr.count = 0;
			ptr.totalMicroseconds = 0.0;
			ptr.x1 = float.MinValue;
			ptr.chunk = null;
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x00184CB4 File Offset: 0x00182EB4
		private bool OverlapsDrawnLabel(int drawnCount, int depth, float x0, float x1)
		{
			for (int i = 0; i < drawnCount; i++)
			{
				if (this.labelZoneDepth[i] == depth && x0 < this.labelZoneX1[i] - 0.5f && x1 > this.labelZoneX0[i] + 0.5f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x00184CFE File Offset: 0x00182EFE
		private static bool IsGcReason(RuntimePauseReason reason)
		{
			return reason == RuntimePauseReason.GC || reason == RuntimePauseReason.GCPrep;
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x00184D0C File Offset: 0x00182F0C
		private void RebuildRuler(float w)
		{
			float num = ProfilerFlamechartWidget.NiceStep((float)(this.view.Span / 1000.0) / 8f);
			int i = 0;
			float num2 = 0f;
			while (i < 24)
			{
				double num3 = this.view.Start + (double)num2 * 1000.0;
				if (num3 >= this.view.Start + this.view.Span)
				{
					break;
				}
				float num4 = this.TimeToX(num3, w);
				this.Quad(ProfilerFlamechartWidget.RulerTickColor, num4, 0f, num4 + 1f, this.height);
				ref ValueTuple<float, float, string> ptr = ref this.rulerLabels[i];
				if (ptr.Item3 == null || ptr.Item1 != num2 || ptr.Item2 != num)
				{
					ProfilerGui.Scratch.Clear();
					ProfilerFlamechartWidget.AppendRulerTime(ProfilerGui.Scratch, num2, num);
					ptr = new ValueTuple<float, float, string>(num2, num, ProfilerGui.Scratch.ToString());
				}
				ProfilerGui.Label(new Rect(num4 + 3f, 0f, 90f, 18f), ptr.Item3, ProfilerFlamechartWidget.RulerTextColor, TextAnchor.MiddleLeft, GameFont.Tiny);
				i++;
				num2 += num;
			}
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x00184E44 File Offset: 0x00183044
		private static void AppendRulerTime(StringBuilder builder, float relMs, float stepMs)
		{
			float num;
			string value;
			string format;
			if (stepMs >= 1f)
			{
				num = relMs;
				value = "ms";
				format = "0";
			}
			else if (stepMs >= 0.001f)
			{
				num = relMs * 1000f;
				value = "µs";
				format = "0";
			}
			else
			{
				num = relMs * 1000000f;
				value = "ns";
				format = ((stepMs * 1000000f >= 1f) ? "0" : "0.##");
			}
			builder.Append(num.ToString(format)).Append(' ').Append(value);
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x00184ECC File Offset: 0x001830CC
		private static float NiceStep(float raw)
		{
			if (raw <= 0f)
			{
				return 1E-09f;
			}
			float num = Mathf.Pow(10f, Mathf.Floor(MathF.Log10(raw)));
			float num2 = raw / num;
			return ((num2 < 1.5f) ? 1f : ((num2 < 3.5f) ? 2f : ((num2 < 7.5f) ? 5f : 10f))) * num;
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x00184F38 File Offset: 0x00183138
		private void RebuildPausedGaps(float w, float h)
		{
			this.pausedGapSpans.Clear();
			if (this.UseSpikes || this.lastLayoutRetained || this.layoutOriginNs < 0L)
			{
				return;
			}
			double num = this.view.Start + this.view.Span;
			for (int i = 1; i < this.frameEntries.Count; i++)
			{
				double maxEndUs = this.frameEntries[i - 1].maxEndUs;
				double startUs = this.frameEntries[i].startUs;
				if (startUs > maxEndUs && startUs > this.view.Start && maxEndUs < num && this.OverlapsCaptureGap(maxEndUs, startUs))
				{
					float num2 = Mathf.Max(126f, this.TimeToX(maxEndUs, w));
					float num3 = Mathf.Min(w, this.TimeToX(startUs, w));
					if (num3 - num2 >= 2f)
					{
						this.Quad(ProfilerFlamechartWidget.PausedGapColor, num2, 18f, num3, h);
						this.pausedGapSpans.Add(new ProfilerFlamechartWidget.PausedGapSpan
						{
							x0 = num2,
							x1 = num3,
							durationUs = startUs - maxEndUs
						});
					}
				}
			}
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x00185064 File Offset: 0x00183264
		private bool OverlapsCaptureGap(double startUs, double endUs)
		{
			for (int i = 0; i < FrameProfiler.CaptureGapCount; i++)
			{
				FrameProfiler.CaptureGap captureGap = FrameProfiler.GetCaptureGap(i);
				double num = (double)(captureGap.beginTimeNanoseconds - this.layoutOriginNs) / 1000.0;
				double num2 = (double)(captureGap.endTimeNanoseconds - this.layoutOriginNs) / 1000.0;
				if (num < endUs && num2 > startUs)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x001850C4 File Offset: 0x001832C4
		private void RebuildOutsideRange(float w, float h)
		{
			float num = this.TimeToX(0.0, w);
			float num2 = this.TimeToX(this.view.Extent, w);
			if (num > 126f)
			{
				this.Quad(ProfilerFlamechartWidget.OutsideRangeColor, 126f, 18f, Mathf.Min(w, num), h);
			}
			if (num2 < w)
			{
				this.Quad(ProfilerFlamechartWidget.OutsideRangeColor, Mathf.Max(126f, num2), 18f, w, h);
			}
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x0018513C File Offset: 0x0018333C
		private void RebuildSelectionBand(float w, float h)
		{
			double timeUs;
			double timeUs2;
			if (!this.TryGetSelectionRange(out timeUs, out timeUs2))
			{
				return;
			}
			float num = Mathf.Max(126f, this.TimeToX(timeUs, w));
			float num2 = Mathf.Min(w, this.TimeToX(timeUs2, w));
			if (num2 <= num)
			{
				return;
			}
			this.Quad(ProfilerFlamechartWidget.SelectionBandColor, num, 0f, num2, ProfilerFlamechartWidget.PlotTop);
			int num3 = -1;
			float num4;
			if (this.dragSelecting && this.lastMousePosition.y >= ProfilerFlamechartWidget.PlotTop && !this.TryFindBand((this.lastMousePosition.y - ProfilerFlamechartWidget.PlotTop) / 19f + this.view.TopLane, out num3, out num4))
			{
				num3 = -1;
			}
			bool flag = ProfilerThreadRegistry.Entries.Count == 0;
			int i = 0;
			while (i < this.bandCount)
			{
				if (this.dragSelecting && num3 >= 0)
				{
					if (i == num3)
					{
						goto IL_DA;
					}
				}
				else if (flag || ProfilerThreadRegistry.IsSelected(this.bandThreadIds[i]))
				{
					goto IL_DA;
				}
				IL_131:
				i++;
				continue;
				IL_DA:
				float num5 = this.BandLaneTop(i, 0);
				float num6 = num5 + (this.bandAnimLanes[i] + this.BandGapFor(i)) * 19f;
				if (num6 > ProfilerFlamechartWidget.PlotTop && num5 < h)
				{
					this.Quad(ProfilerFlamechartWidget.SelectionBandColor, num, Mathf.Max(ProfilerFlamechartWidget.PlotTop, num5), num2, Mathf.Min(h, num6));
					goto IL_131;
				}
				goto IL_131;
			}
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x00185290 File Offset: 0x00183490
		private void RebuildSelectionRects(float w)
		{
			int selectedZoneOrdinal = this.SelectedZoneOrdinal;
			List<int> selectedPathIds = this.SelectedPathIds;
			int num;
			if (selectedZoneOrdinal < 0 || selectedPathIds == null || selectedPathIds.Count == 0 || !this.TryGetFrameEntryIndexByOrdinal(selectedZoneOrdinal, out num))
			{
				return;
			}
			ProfilerFlamechartWidget.FrameEntry frameEntry = this.frameEntries[num];
			if (!this.FrameDetailed(frameEntry, w))
			{
				return;
			}
			ProfilerPathTable.Entry[] entries = ProfilerPathTable.Entries;
			int count = ProfilerPathTable.Count;
			double minVisibleUs = this.MinVisibleUs(w);
			for (int i = 0; i < this.bandCount; i++)
			{
				if (!this.bandCollapsed[i] && this.SectionIndexForBand(frameEntry, i) >= 0)
				{
					List<ProfilerFlamechartWidget.ZoneDraw> list = this.CollectZonesCached(frameEntry, num, i, minVisibleUs);
					for (int j = 0; j < list.Count; j++)
					{
						ProfilerFlamechartWidget.ZoneDraw zoneDraw = list[j];
						if (!zoneDraw.folded && zoneDraw.depth < this.bandLanes[i])
						{
							int pathId = zoneDraw.chunk.nodes[zoneDraw.index].pathId;
							int item = (pathId < count) ? entries[pathId].foldedPathId : pathId;
							if (selectedPathIds.Contains(pathId) || selectedPathIds.Contains(item))
							{
								float num2 = Mathf.Max(126f, this.TimeToX(zoneDraw.startUs, w));
								float num3 = Mathf.Min(w, this.TimeToX(zoneDraw.endUs, w));
								if (num3 - num2 >= 1f)
								{
									num3 = num2 + Mathf.Max(2f, num3 - num2);
									float num4 = this.BandLaneTop(i, zoneDraw.depth);
									float num5 = num4 + 19f - 1f;
									if (num5 > ProfilerFlamechartWidget.PlotTop && num4 < this.height)
									{
										num4 = Mathf.Max(ProfilerFlamechartWidget.PlotTop, num4);
										num5 = Mathf.Min(this.height, num5);
										this.OutlineQuad(ProfilerFlamechartWidget.SelectionStrokeColor, num2, num4, num3, num5);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x0018548C File Offset: 0x0018368C
		private bool TryGetFrameEntryIndexByOrdinal(int ordinal, out int frameIndex)
		{
			for (int i = this.frameEntries.Count - 1; i >= 0; i--)
			{
				if (this.frameEntries[i].snapshot.captureOrdinal == ordinal)
				{
					frameIndex = i;
					return true;
				}
			}
			frameIndex = -1;
			return false;
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x001854D4 File Offset: 0x001836D4
		private bool TryHitTest(Vector2 position, out ProfilerFlamechartWidget.ZoneHit hit)
		{
			hit = default(ProfilerFlamechartWidget.ZoneHit);
			float num = this.width;
			if (num <= 126f || this.frameEntries.Count == 0)
			{
				return false;
			}
			if (position.y < ProfilerFlamechartWidget.PlotTop || position.x < 126f)
			{
				return false;
			}
			double num2 = this.XToTime(position.x, num);
			float lane = (position.y - ProfilerFlamechartWidget.PlotTop) / 19f + this.view.TopLane;
			int num3;
			int intraDepth;
			if (!this.TryFindBandForLane(lane, out num3, out intraDepth))
			{
				return false;
			}
			int num4 = -1;
			for (int i = this.frameEntries.Count - 1; i >= 0; i--)
			{
				ProfilerFlamechartWidget.FrameEntry frameEntry = this.frameEntries[i];
				double num5 = (num3 == 0) ? frameEntry.endUs : frameEntry.boundaryUs;
				if (num2 >= frameEntry.startUs && num2 < num5)
				{
					num4 = i;
					break;
				}
			}
			return num4 >= 0 && this.TryHitTestFrame(num4, num2, num3, intraDepth, num, out hit);
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x001855CC File Offset: 0x001837CC
		private bool TryHitTestCollapsedRun(Vector2 position, out ProfilerFlamechartWidget.CollapsedRun hit)
		{
			for (int i = 0; i < this.collapsedRuns.Count; i++)
			{
				ProfilerFlamechartWidget.CollapsedRun collapsedRun = this.collapsedRuns[i];
				if (position.x >= collapsedRun.x0 - 1f && position.x <= collapsedRun.x1 + 1f && position.y >= collapsedRun.laneY && position.y <= collapsedRun.laneY + 19f)
				{
					hit = collapsedRun;
					return true;
				}
			}
			hit = default(ProfilerFlamechartWidget.CollapsedRun);
			return false;
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x00185658 File Offset: 0x00183858
		private bool TryHitTestFrame(int frameIndex, double timeUs, int bandIndex, int intraDepth, float w, out ProfilerFlamechartWidget.ZoneHit hit)
		{
			hit = default(ProfilerFlamechartWidget.ZoneHit);
			ProfilerFlamechartWidget.FrameEntry frameEntry = this.frameEntries[frameIndex];
			if (!this.FrameDetailed(frameEntry, w))
			{
				return false;
			}
			int num = this.SectionIndexForBand(frameEntry, bandIndex);
			if (num < 0)
			{
				return false;
			}
			FrameProfiler.ThreadSection threadSection = frameEntry.snapshot.sections[num];
			double num2 = (bandIndex == 0) ? frameEntry.startUs : (frameEntry.startUs + (double)threadSection.startNanoseconds / 1000.0);
			double num3 = (bandIndex == 0) ? frameEntry.endUs : (frameEntry.startUs + (double)threadSection.endNanoseconds / 1000.0);
			if (intraDepth != 0)
			{
				double minVisibleUs = this.MinVisibleUs(w);
				List<ProfilerFlamechartWidget.ZoneDraw> list = this.CollectZonesCached(frameEntry, frameIndex, bandIndex, minVisibleUs);
				for (int i = 0; i < list.Count; i++)
				{
					ProfilerFlamechartWidget.ZoneDraw zoneDraw = list[i];
					if (!zoneDraw.folded && zoneDraw.depth == intraDepth && timeUs >= zoneDraw.startUs && timeUs <= zoneDraw.endUs)
					{
						hit = new ProfilerFlamechartWidget.ZoneHit
						{
							frameIndex = frameIndex,
							band = bandIndex,
							chunk = zoneDraw.chunk,
							nodeIndex = zoneDraw.index,
							depth = zoneDraw.depth,
							startUs = zoneDraw.startUs,
							endUs = zoneDraw.endUs
						};
						return true;
					}
				}
				return false;
			}
			if (timeUs < num2 || timeUs > num3)
			{
				return false;
			}
			hit = new ProfilerFlamechartWidget.ZoneHit
			{
				frameIndex = frameIndex,
				band = bandIndex,
				nodeIndex = -1,
				startUs = num2,
				endUs = num3
			};
			return true;
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x00185814 File Offset: 0x00183A14
		private void HandleInput()
		{
			Event current = Event.current;
			Vector2 mousePosition = current.mousePosition;
			bool flag = mousePosition.x >= 0f && mousePosition.y >= 0f && mousePosition.x < this.width && mousePosition.y < this.height;
			if (current.type == EventType.mouseDown && flag)
			{
				this.lastMousePosition = mousePosition;
				if (current.button == 0)
				{
					if (current.clickCount < 2 && this.layoutOriginNs >= 0L && mousePosition.x >= 126f && this.width > 126f && this.contentVisible)
					{
						this.dragArmed = true;
						this.dragArmX = mousePosition.x;
						this.dragStartUs = this.XToTime(mousePosition.x, this.width);
						this.dragEndUs = this.dragStartUs;
					}
					else if (this.contentVisible)
					{
						this.HandleLeftClick(mousePosition, current.clickCount);
					}
				}
				else if (this.dragArmed || this.dragSelecting)
				{
					this.dragArmed = false;
					this.dragSelecting = false;
				}
				else
				{
					this.view.BeginPan(mousePosition);
				}
				current.Use();
				return;
			}
			if (current.type == EventType.scrollWheel && flag)
			{
				this.lastMousePosition = mousePosition;
				current.Use();
				if (this.width <= 126f || !this.contentVisible)
				{
					return;
				}
				int num;
				float num2;
				if (this.view.HandleWheel(current.delta, mousePosition.x, 126f, ProfilerFlamechartWidget.PlotWidth(this.width)) && mousePosition.y >= ProfilerFlamechartWidget.PlotTop && this.TryFindBand((mousePosition.y - ProfilerFlamechartWidget.PlotTop) / 19f + this.view.TopLane, out num, out num2))
				{
					this.anchorThreadId = this.bandThreadIds[num];
					this.anchorIntraLanes = num2;
					this.anchorViewY = mousePosition.y;
				}
				return;
			}
			else
			{
				if (current.type != EventType.repaint)
				{
					return;
				}
				this.lastMousePosition = mousePosition;
				this.mouseInside = flag;
				if (this.dragArmed || this.dragSelecting)
				{
					if (Input.GetMouseButton(0))
					{
						if (!this.dragSelecting && Mathf.Abs(mousePosition.x - this.dragArmX) > 4f)
						{
							this.dragSelecting = true;
						}
						if (this.dragSelecting)
						{
							this.dragEndUs = this.XToTime(Mathf.Max(126f, mousePosition.x), this.width);
						}
						return;
					}
					bool flag2 = this.dragSelecting;
					this.dragArmed = false;
					this.dragSelecting = false;
					if (!this.contentVisible)
					{
						return;
					}
					if (flag2)
					{
						this.CommitRangeSelection();
						return;
					}
					this.HandleLeftClick(mousePosition, 1);
					return;
				}
				else
				{
					if (!this.view.Panning)
					{
						return;
					}
					if (!Input.GetMouseButton(1) && !Input.GetMouseButton(2))
					{
						this.view.EndPan();
						return;
					}
					if (this.width > 126f && this.view.TryPan(mousePosition, ProfilerFlamechartWidget.PlotWidth(this.width), 19f))
					{
						this.anchorThreadId = -1;
						this.ClampView();
					}
					return;
				}
			}
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x00185B14 File Offset: 0x00183D14
		private void PaintHover()
		{
			if (this.dragSelecting)
			{
				this.ClearHover();
				StringBuilder scratch = ProfilerGui.Scratch;
				scratch.Clear();
				ProfilerTreeModel.AppendMs(scratch, (float)(Math.Abs(this.dragEndUs - this.dragStartUs) / 1000.0), "0.00", " ");
				ProfilerGui.SetTooltip(scratch.ToString());
				return;
			}
			if (!this.mouseInside || this.view.Panning || this.dragArmed)
			{
				this.HideTooltip();
				return;
			}
			this.UpdateTooltip(this.lastMousePosition);
			if (this.pauseBandVisible)
			{
				this.Quad(ProfilerFlamechartWidget.PauseBandColor, this.pauseBandBounds.xMin, this.pauseBandBounds.yMin, this.pauseBandBounds.xMax, this.pauseBandBounds.yMax);
			}
			if (this.hoverRectVisible)
			{
				this.OutlineQuad(ProfilerFlamechartWidget.HoverStrokeColor, this.hoverRectBounds.xMin, this.hoverRectBounds.yMin, this.hoverRectBounds.xMax, this.hoverRectBounds.yMax);
			}
			if (this.tooltipText != null)
			{
				ProfilerGui.SetTooltip(this.tooltipText);
			}
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x00185C34 File Offset: 0x00183E34
		private void HandleLeftClick(Vector2 position, int clickCount)
		{
			if (position.x < 126f && position.y >= ProfilerFlamechartWidget.PlotTop)
			{
				float lane = (position.y - ProfilerFlamechartWidget.PlotTop) / 19f + this.view.TopLane;
				int band;
				float num;
				if (this.TryFindBand(lane, out band, out num))
				{
					this.ToggleBandCollapsed(band);
				}
				return;
			}
			ProfilerFlamechartWidget.ZoneHit zoneHit;
			bool flag = this.TryHitTest(position, out zoneHit);
			ProfilerFlamechartWidget.CollapsedRun collapsedRun = default(ProfilerFlamechartWidget.CollapsedRun);
			bool flag2 = !flag && this.TryHitTestCollapsedRun(position, out collapsedRun);
			if (flag2 && collapsedRun.count == 1 && collapsedRun.chunk != null && collapsedRun.frameIndex < this.frameEntries.Count)
			{
				zoneHit = this.HitFromRun(collapsedRun);
				flag = true;
				flag2 = false;
			}
			if (clickCount >= 2)
			{
				if (flag)
				{
					this.ZoomToZone(zoneHit);
					return;
				}
				if (flag2)
				{
					this.ZoomToRun(collapsedRun, position);
					return;
				}
				this.ResetView();
				return;
			}
			else
			{
				if (!flag)
				{
					return;
				}
				this.suppressSelectionZoom = true;
				ProfilerFlamechartWidget.FrameEntry frameEntry = this.frameEntries[zoneHit.frameIndex];
				int pathId = zoneHit.IsRoot ? ProfilerFlamechartWidget.RootPathIdFor(frameEntry, zoneHit.band) : zoneHit.chunk.nodes[zoneHit.nodeIndex].pathId;
				Action<ProfilerZoneSelection> zoneSelected = this.ZoneSelected;
				if (zoneSelected == null)
				{
					return;
				}
				zoneSelected(new ProfilerZoneSelection(frameEntry.ordinal, this.bandThreadIds[zoneHit.band], pathId));
				return;
			}
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x00185D8C File Offset: 0x00183F8C
		private static int RootPathIdFor(in ProfilerFlamechartWidget.FrameEntry entry, int band)
		{
			FrameProfiler.FrameSnapshot snapshot = entry.snapshot;
			for (int i = 0; i < snapshot.sectionCount; i++)
			{
				FrameProfiler.ThreadSection threadSection = snapshot.sections[i];
				if (threadSection.pathEntryCount > 0 && ((band == 0) ? (i == 0) : (i > 0)))
				{
					return snapshot.pathEntries[threadSection.firstPathEntry].pathId;
				}
			}
			return -1;
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x00185DF0 File Offset: 0x00183FF0
		private ProfilerFlamechartWidget.ZoneHit HitFromRun(in ProfilerFlamechartWidget.CollapsedRun run)
		{
			ProfilerFlamechartWidget.FrameEntry frameEntry = this.frameEntries[run.frameIndex];
			int band = run.band;
			long num;
			long num2;
			double num3;
			double num4;
			this.FrameRangeFor(frameEntry, band, out num, out num2, out num3, out num4);
			ref ZoneNode ptr = ref run.chunk.nodes[run.nodeIndex];
			long val = run.chunk.baseNs + ptr.StartNanoseconds;
			long val2 = ptr.IsOpen ? num2 : (run.chunk.baseNs + ptr.EndNanoseconds);
			return new ProfilerFlamechartWidget.ZoneHit
			{
				frameIndex = run.frameIndex,
				band = band,
				chunk = run.chunk,
				nodeIndex = run.nodeIndex,
				startUs = num3 + (double)(Math.Max(val, num) - num) / 1000.0,
				endUs = num3 + (double)(Math.Min(val2, num2) - num) / 1000.0
			};
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x00185EEC File Offset: 0x001840EC
		private void ZoomToRun(in ProfilerFlamechartWidget.CollapsedRun run, Vector2 position)
		{
			float w = this.width;
			double num = this.XToTime(run.x0, w);
			double num2 = Math.Max(this.XToTime(run.x1, w) - num, this.view.MinSpan);
			if (num2 > this.view.Span * 0.5)
			{
				double num3 = this.XToTime(Mathf.Max(126f, position.x), w);
				num2 = this.view.Span * 0.5;
				num = num3 - (num3 - this.view.Start) * 0.5;
			}
			this.suppressSelectionZoom = true;
			this.view.AnimateTo(num, num2, false);
		}

		// Token: 0x06003D58 RID: 15704 RVA: 0x00185FA1 File Offset: 0x001841A1
		private void ZoomToZone(in ProfilerFlamechartWidget.ZoneHit hit)
		{
			this.suppressSelectionZoom = true;
			this.view.AnimateTo(hit.startUs, Math.Max(this.view.MinSpan, hit.endUs - hit.startUs), false);
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x00185FDC File Offset: 0x001841DC
		private void CommitRangeSelection()
		{
			double num = Math.Min(this.dragStartUs, this.dragEndUs);
			double num2 = Math.Max(this.dragStartUs, this.dragEndUs);
			if (num2 <= num || this.layoutOriginNs < 0L)
			{
				return;
			}
			int num3 = -1;
			int num4 = -1;
			for (int i = 0; i < this.frameEntries.Count; i++)
			{
				ProfilerFlamechartWidget.FrameEntry frameEntry = this.frameEntries[i];
				if (((i + 1 < this.frameEntries.Count) ? this.frameEntries[i + 1].startUs : Math.Max(frameEntry.endUs, frameEntry.maxEndUs)) > num && frameEntry.startUs < num2)
				{
					if (num4 < 0)
					{
						num4 = i;
					}
					num3 = i;
				}
			}
			if (num3 < 0)
			{
				return;
			}
			float lane = (this.lastMousePosition.y - ProfilerFlamechartWidget.PlotTop) / 19f + this.view.TopLane;
			int num5;
			float num6;
			if (this.lastMousePosition.y >= ProfilerFlamechartWidget.PlotTop && this.TryFindBand(lane, out num5, out num6))
			{
				ProfilerThreadRegistry.SelectOnly(this.bandThreadIds[num5]);
			}
			int count = this.frameEntries.Count;
			long num7 = this.layoutOriginNs + (long)(num * 1000.0);
			long endNs = Math.Max(this.layoutOriginNs + (long)(num2 * 1000.0), num7 + 1L);
			this.suppressSelectionZoom = true;
			this.session.SelectTimeRange(count - 1 - num3, count - 1 - num4, num7, endNs);
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x00186158 File Offset: 0x00184358
		private void ResetView()
		{
			this.view.TopLane = 0f;
			this.anchorThreadId = -1;
			this.lastSelectionKey = ProfilerFlamechartWidget.ForcedSelectionKey;
			double num;
			double num2;
			if (!this.TryGetSelectionRange(out num, out num2))
			{
				if (this.frameEntries.Count == 0)
				{
					this.view.UserView = false;
					return;
				}
				List<ProfilerFlamechartWidget.FrameEntry> list = this.frameEntries;
				ProfilerFlamechartWidget.FrameEntry frameEntry = list[list.Count - 1];
				double num3 = Math.Max(1000.0, frameEntry.endUs - frameEntry.startUs);
				this.view.AnimateTo(frameEntry.endUs - num3, num3, true);
			}
		}

		// Token: 0x06003D5B RID: 15707 RVA: 0x001861F2 File Offset: 0x001843F2
		private void ClampView()
		{
			this.view.Clamp(Mathf.Max(1f, (this.height - ProfilerFlamechartWidget.PlotTop) / 19f), this.animTotalLanes);
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x00186224 File Offset: 0x00184424
		private void UpdateTooltip(Vector2 position)
		{
			if (!this.contentVisible)
			{
				this.HideTooltip();
				return;
			}
			if (position.y >= 18f && position.y < ProfilerFlamechartWidget.PlotTop && position.x >= 126f)
			{
				int frameIndex;
				int pauseIndex;
				if (!this.TryHitTestPause(position, out frameIndex, out pauseIndex))
				{
					this.HideTooltip();
					return;
				}
				this.ShowPauseTooltip(position, frameIndex, pauseIndex);
				return;
			}
			else
			{
				int frameIndex2;
				int pauseIndex2;
				if (this.TryHitTestPauseNode(position, out frameIndex2, out pauseIndex2))
				{
					this.ShowPauseTooltip(position, frameIndex2, pauseIndex2);
					return;
				}
				ProfilerFlamechartWidget.CollapsedRun collapsedRun;
				if (this.TryHitTestCollapsedRun(position, out collapsedRun))
				{
					if (collapsedRun.count == 1 && collapsedRun.chunk != null && collapsedRun.frameIndex < this.frameEntries.Count)
					{
						ProfilerFlamechartWidget.ZoneHit zoneHit = this.HitFromRun(collapsedRun);
						this.ShowZoneTooltip(position, zoneHit);
						return;
					}
					this.ShowCollapsedTooltip(position, collapsedRun);
					return;
				}
				else
				{
					ProfilerFlamechartWidget.ZoneHit zoneHit2;
					if (this.TryHitTest(position, out zoneHit2))
					{
						this.ShowZoneTooltip(position, zoneHit2);
						return;
					}
					double durationUs;
					if (this.TryHitTestPausedGap(position, out durationUs))
					{
						this.ShowPausedGapTooltip(position, durationUs);
						return;
					}
					this.HideTooltip();
					return;
				}
			}
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x0018631C File Offset: 0x0018451C
		private bool TryHitTestPausedGap(Vector2 position, out double durationUs)
		{
			durationUs = 0.0;
			if (position.y < ProfilerFlamechartWidget.PlotTop)
			{
				return false;
			}
			for (int i = 0; i < this.pausedGapSpans.Count; i++)
			{
				ProfilerFlamechartWidget.PausedGapSpan pausedGapSpan = this.pausedGapSpans[i];
				if (position.x >= pausedGapSpan.x0 && position.x <= pausedGapSpan.x1)
				{
					durationUs = pausedGapSpan.durationUs;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003D5E RID: 15710 RVA: 0x00186390 File Offset: 0x00184590
		private void ShowPausedGapTooltip(Vector2 position, double durationUs)
		{
			this.ClearHover();
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			scratch.Append("Profiling paused\nno frames captured for ");
			ProfilerTreeModel.AppendMs(scratch, (float)(durationUs / 1000.0), "0.0", " ");
			this.tooltipText = scratch.ToString();
		}

		// Token: 0x06003D5F RID: 15711 RVA: 0x001863E4 File Offset: 0x001845E4
		private void ShowZoneTooltip(Vector2 position, in ProfilerFlamechartWidget.ZoneHit hit)
		{
			ProfilerFlamechartWidget.ZoneHit zoneHit = hit;
			bool flag = zoneHit.IsRoot && this.hoverRootFrame == this.frameEntries[hit.frameIndex].ordinal && this.hoverChunk == null && this.hoverNodeIndex == hit.band;
			zoneHit = hit;
			bool flag2 = !zoneHit.IsRoot && hit.chunk == this.hoverChunk && hit.nodeIndex == this.hoverNodeIndex;
			if ((flag || flag2) && this.hoverPauseIndex < 0)
			{
				this.UpdateHoverRect(hit);
				return;
			}
			this.hoverChunk = hit.chunk;
			zoneHit = hit;
			this.hoverNodeIndex = (zoneHit.IsRoot ? hit.band : hit.nodeIndex);
			zoneHit = hit;
			this.hoverRootFrame = (zoneHit.IsRoot ? this.frameEntries[hit.frameIndex].ordinal : -1);
			this.hoverPauseFrame = -1;
			this.hoverPauseIndex = -1;
			this.pauseBandVisible = false;
			this.UpdateHoverRect(hit);
			ProfilerFlamechartWidget.FrameEntry frameEntry = this.frameEntries[hit.frameIndex];
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			zoneHit = hit;
			if (zoneHit.IsRoot)
			{
				FrameProfiler.ThreadSection threadSection = frameEntry.snapshot.sections[this.SectionIndexForBand(frameEntry, hit.band)];
				if (hit.band == 0)
				{
					StringBuilder stringBuilder = scratch;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Frame ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(frameEntry.snapshot.frameIndex);
					stringBuilder.Append(defaultInterpolatedStringHandler.ToStringAndClear());
					scratch.Append("\ntotal ");
					ProfilerTreeModel.AppendMs(scratch, frameEntry.snapshot.totalMs, "0.000", " ");
				}
				else
				{
					scratch.Append(FrameProfiler.GetLabel(threadSection.nameLabelId));
					scratch.Append("\nbusy ");
					ProfilerTreeModel.AppendMs(scratch, (float)threadSection.busyNanoseconds / 1000000f, "0.000", " ");
				}
				FrameProfiler.PathEntry pathEntry = frameEntry.snapshot.pathEntries[threadSection.firstPathEntry];
				if (pathEntry.selfMicroseconds > 0f)
				{
					scratch.Append(" / unprofiled ");
					ProfilerTreeModel.AppendMs(scratch, pathEntry.selfMicroseconds / 1000f, "0.000", " ");
				}
				if (pathEntry.allocBytes > 0)
				{
					scratch.Append("\nalloc ");
					ProfilerTreeModel.AppendAllocBytes(scratch, (long)pathEntry.allocBytes, " ");
				}
			}
			else
			{
				ref ZoneNode ptr = ref hit.chunk.nodes[hit.nodeIndex];
				scratch.Append(FrameProfiler.GetLabel(FrameProfiler.PathLabel(ptr.pathId)));
				scratch.Append("\ntotal ");
				ProfilerTreeModel.AppendMs(scratch, this.ZoneDurationUs(hit.chunk, hit.nodeIndex, frameEntry, hit.band) / 1000f, "0.000", " ");
				if (ptr.IsOpen)
				{
					scratch.Append(" / still running");
				}
				else
				{
					scratch.Append(" / self ");
					ProfilerTreeModel.AppendMs(scratch, ProfilerFlamechartWidget.ZoneSelfUs(hit.chunk, hit.nodeIndex) / 1000f, "0.000", " ");
				}
				FrameProfiler.NodeAllocStat[] allocStats = hit.chunk.allocStats;
				if (allocStats != null && hit.nodeIndex < allocStats.Length)
				{
					FrameProfiler.NodeAllocStat nodeAllocStat = allocStats[hit.nodeIndex];
					int num = Math.Max(nodeAllocStat.totalBytes, nodeAllocStat.selfBytes);
					if (num > 0)
					{
						scratch.Append("\nalloc ");
						ProfilerTreeModel.AppendAllocBytes(scratch, (long)num, " ");
						if (nodeAllocStat.count > 0)
						{
							StringBuilder stringBuilder2 = scratch;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(8, 1);
							defaultInterpolatedStringHandler2.AppendLiteral(" (");
							defaultInterpolatedStringHandler2.AppendFormatted<int>(nodeAllocStat.count, "n0");
							defaultInterpolatedStringHandler2.AppendLiteral(" objs)");
							stringBuilder2.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
						}
					}
				}
			}
			this.tooltipText = scratch.ToString();
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x001867E4 File Offset: 0x001849E4
		private bool TryHitTestPause(Vector2 position, out int frameIndex, out int pauseIndex)
		{
			frameIndex = -1;
			pauseIndex = -1;
			float num = this.width;
			if (num <= 126f || this.frameEntries.Count == 0)
			{
				return false;
			}
			double num2 = this.view.Start + this.view.Span;
			for (int i = 0; i < this.frameEntries.Count; i++)
			{
				ProfilerFlamechartWidget.FrameEntry frameEntry = this.frameEntries[i];
				if (frameEntry.endUs > this.view.Start && frameEntry.startUs < num2)
				{
					FrameProfiler.FrameSnapshot snapshot = frameEntry.snapshot;
					for (int j = 0; j < snapshot.pauseCount; j++)
					{
						FrameProfiler.PauseSpan pauseSpan = snapshot.pauses[j];
						float num3 = this.TimeToX(frameEntry.startUs + (double)pauseSpan.startMicroseconds, num);
						float num4 = Mathf.Max(num3 + 1f, this.TimeToX(frameEntry.startUs + (double)pauseSpan.endMicroseconds, num));
						if (position.x >= num3 - 1f && position.x <= num4 + 1f)
						{
							frameIndex = i;
							pauseIndex = j;
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x00186910 File Offset: 0x00184B10
		private bool TryHitTestPauseNode(Vector2 position, out int frameIndex, out int pauseIndex)
		{
			for (int i = 0; i < this.pauseNodes.Count; i++)
			{
				ProfilerFlamechartWidget.PauseNode pauseNode = this.pauseNodes[i];
				if (position.x >= pauseNode.x0 - 1f && position.x <= pauseNode.x1 + 1f && position.y >= pauseNode.laneY && position.y <= pauseNode.laneY + 19f)
				{
					frameIndex = pauseNode.frameIndex;
					pauseIndex = pauseNode.pauseIndex;
					return true;
				}
			}
			frameIndex = -1;
			pauseIndex = -1;
			return false;
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x001869A4 File Offset: 0x00184BA4
		private void ShowPauseTooltip(Vector2 position, int frameIndex, int pauseIndex)
		{
			ProfilerFlamechartWidget.FrameEntry frameEntry = this.frameEntries[frameIndex];
			this.hoverChunk = null;
			this.hoverNodeIndex = -1;
			this.hoverRootFrame = -1;
			this.hoverPauseFrame = frameIndex;
			this.hoverPauseIndex = pauseIndex;
			FrameProfiler.PauseSpan pauseSpan = frameEntry.snapshot.pauses[pauseIndex];
			float num = this.width;
			float num2 = Mathf.Max(126f, this.TimeToX(frameEntry.startUs + (double)pauseSpan.startMicroseconds, num));
			float num3 = Mathf.Min(num, Mathf.Max(num2 + 1f, this.TimeToX(frameEntry.startUs + (double)pauseSpan.endMicroseconds, num)));
			this.hoverRectBounds = Rect.MinMaxRect(num2, 20f, num3, 18f + ProfilerFlamechartWidget.GcLaneHeight - 2f);
			this.hoverRectVisible = true;
			float xmin = Mathf.Clamp(this.TimeToX(frameEntry.startUs + (double)pauseSpan.suspendedMicroseconds, num), num2, num3);
			this.pauseBandBounds = Rect.MinMaxRect(xmin, ProfilerFlamechartWidget.PlotTop, num3, Mathf.Max(ProfilerFlamechartWidget.PlotTop, this.height));
			this.pauseBandVisible = true;
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			scratch.Append(FrameProfiler.DescribePause(pauseSpan.reason, pauseSpan.condemnedGeneration));
			scratch.Append("\nstopped ");
			ProfilerTreeModel.AppendMs(scratch, (float)(pauseSpan.endMicroseconds - pauseSpan.suspendedMicroseconds) / 1000f, "0.000", " ");
			scratch.Append(" / waiting for threads ");
			ProfilerTreeModel.AppendMs(scratch, (float)(pauseSpan.suspendedMicroseconds - pauseSpan.startMicroseconds) / 1000f, "0.000", " ");
			if (pauseSpan.collectedBytes > 0L || pauseSpan.promotedBytes > 0L)
			{
				scratch.Append("\ncollected ");
				ProfilerTreeModel.AppendAllocBytes(scratch, Math.Max(0L, pauseSpan.collectedBytes), " ");
				scratch.Append(" / promoted ");
				ProfilerTreeModel.AppendAllocBytes(scratch, pauseSpan.promotedBytes, " ");
			}
			scratch.Append("\ntriggered by ");
			if (pauseSpan.triggeringThreadId == 0)
			{
				scratch.Append("a runtime thread");
			}
			else if (pauseSpan.triggeringThreadId == Environment.CurrentManagedThreadId)
			{
				scratch.Append("the main thread");
			}
			else
			{
				StringBuilder stringBuilder = scratch;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 1);
				defaultInterpolatedStringHandler.AppendLiteral("thread ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(pauseSpan.triggeringThreadId);
				stringBuilder.Append(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.tooltipText = scratch.ToString();
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x00186C1C File Offset: 0x00184E1C
		private void ShowCollapsedTooltip(Vector2 position, in ProfilerFlamechartWidget.CollapsedRun run)
		{
			this.ClearHover();
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			StringBuilder stringBuilder = scratch;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Zones too small to display: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(run.count, "n0");
			stringBuilder.Append(defaultInterpolatedStringHandler.ToStringAndClear());
			scratch.Append("\ntotal ");
			ProfilerTreeModel.AppendMs(scratch, (float)(run.totalMicroseconds / 1000.0), "0.000", " ");
			this.tooltipText = scratch.ToString();
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x00186CAB File Offset: 0x00184EAB
		private void ClearHover()
		{
			this.hoverChunk = null;
			this.hoverNodeIndex = -1;
			this.hoverRootFrame = -1;
			this.hoverPauseFrame = -1;
			this.hoverPauseIndex = -1;
			this.hoverRectVisible = false;
			this.pauseBandVisible = false;
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x00186CDE File Offset: 0x00184EDE
		private void HideTooltip()
		{
			this.ClearHover();
			this.tooltipText = null;
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x00186CF0 File Offset: 0x00184EF0
		private void UpdateHoverRect(in ProfilerFlamechartWidget.ZoneHit hit)
		{
			float num = this.width;
			float num2 = Mathf.Max(126f, this.TimeToX(hit.startUs, num));
			float num3 = Mathf.Min(num, this.TimeToX(hit.endUs, num));
			float num4 = this.BandLaneTop(hit.band, hit.depth);
			float num5 = Mathf.Max(ProfilerFlamechartWidget.PlotTop, num4);
			float num6 = Mathf.Min(this.height, num4 + 19f - 1f);
			if (num3 - num2 < 1f || num6 <= num5)
			{
				this.hoverRectVisible = false;
				return;
			}
			this.hoverRectBounds = Rect.MinMaxRect(num2, num5, num3, num6);
			this.hoverRectVisible = true;
		}

		// Token: 0x040027B1 RID: 10161
		private const float RulerHeight = 18f;

		// Token: 0x040027B2 RID: 10162
		private static readonly float GcLaneHeight = FrameProfiler.RuntimePausesSupported ? 13f : 0f;

		// Token: 0x040027B3 RID: 10163
		private static readonly float PlotTop = 18f + ProfilerFlamechartWidget.GcLaneHeight;

		// Token: 0x040027B4 RID: 10164
		private const float GutterWidth = 126f;

		// Token: 0x040027B5 RID: 10165
		private const double MinViewSpanUs = 0.005;

		// Token: 0x040027B6 RID: 10166
		private const float LaneHeight = 19f;

		// Token: 0x040027B7 RID: 10167
		private const float LaneGap = 1f;

		// Token: 0x040027B8 RID: 10168
		private const float MinVisSize = 3f;

		// Token: 0x040027B9 RID: 10169
		private const float MinLabelWidth = 40f;

		// Token: 0x040027BA RID: 10170
		private const float LabelGap = 4f;

		// Token: 0x040027BB RID: 10171
		private const float DetailedFrameMinWidth = 6f;

		// Token: 0x040027BC RID: 10172
		private const float FrameSeparatorMinWidth = 28f;

		// Token: 0x040027BD RID: 10173
		private const int MaxLabels = 160;

		// Token: 0x040027BE RID: 10174
		private const int MaxRulerTicks = 24;

		// Token: 0x040027BF RID: 10175
		private static readonly Color RulerTextColor = ProfilerGui.TextFaint;

		// Token: 0x040027C0 RID: 10176
		private static readonly Color RulerTickColor = ProfilerGui.Hex(4280164906U);

		// Token: 0x040027C1 RID: 10177
		private static readonly Color GutterLineColor = ProfilerGui.SurfaceBorder;

		// Token: 0x040027C2 RID: 10178
		private static readonly Color LaneNameColor = ProfilerGui.TextMuted;

		// Token: 0x040027C3 RID: 10179
		private static readonly Color FrameSeparatorColor = ProfilerGui.Hex(654311423U);

		// Token: 0x040027C4 RID: 10180
		private static readonly Color BandTintColor = ProfilerGui.Hex(83886079U);

		// Token: 0x040027C5 RID: 10181
		private static readonly Color BandSeparatorColor = ProfilerGui.Hex(771751936U);

		// Token: 0x040027C6 RID: 10182
		private static readonly Color ScrollThumbColor = ProfilerGui.Hex(1191182335U);

		// Token: 0x040027C7 RID: 10183
		private static readonly Color CollapsedRunColor = ProfilerGui.Hex(1509949440U);

		// Token: 0x040027C8 RID: 10184
		private static readonly Color SelectionBandColor = ProfilerGui.Hex(385875967U);

		// Token: 0x040027C9 RID: 10185
		private static readonly Color GcPauseColor = ProfilerGui.Regression;

		// Token: 0x040027CA RID: 10186
		private static readonly Color PauseWaitColor = ProfilerGui.Hex(1725984863U);

		// Token: 0x040027CB RID: 10187
		private static readonly Color OtherPauseColor = ProfilerGui.Hex(4287266774U);

		// Token: 0x040027CC RID: 10188
		private static readonly Color PauseBandColor = ProfilerGui.Hex(820015199U);

		// Token: 0x040027CD RID: 10189
		private static readonly Color OutsideRangeColor = ProfilerGui.Hex(1140850688U);

		// Token: 0x040027CE RID: 10190
		private bool contentVisible;

		// Token: 0x040027CF RID: 10191
		private bool newestZonesStale = true;

		// Token: 0x040027D0 RID: 10192
		private float width;

		// Token: 0x040027D1 RID: 10193
		private float height;

		// Token: 0x040027D2 RID: 10194
		private float[] laneLabelEnds = new float[64];

		// Token: 0x040027D3 RID: 10195
		private readonly List<ZoneChunk> chunkScratch = new List<ZoneChunk>();

		// Token: 0x040027D4 RID: 10196
		private readonly Stack<ProfilerFlamechartWidget.FrameZones> frameZonesPool = new Stack<ProfilerFlamechartWidget.FrameZones>();

		// Token: 0x040027D5 RID: 10197
		private readonly List<int> ancestorEnds = new List<int>();

		// Token: 0x040027D6 RID: 10198
		private readonly List<ProfilerFlamechartWidget.PauseNode> pauseNodes = new List<ProfilerFlamechartWidget.PauseNode>();

		// Token: 0x040027D7 RID: 10199
		private const int MaxCollapsedRuns = 4096;

		// Token: 0x040027D8 RID: 10200
		private readonly List<ProfilerFlamechartWidget.CollapsedRun> collapsedRuns = new List<ProfilerFlamechartWidget.CollapsedRun>();

		// Token: 0x040027D9 RID: 10201
		private ProfilerFlamechartWidget.CollapsedRun[] pendingRuns = new ProfilerFlamechartWidget.CollapsedRun[64];

		// Token: 0x040027DA RID: 10202
		private const int MinBandLanes = 1;

		// Token: 0x040027DB RID: 10203
		private const float BandGapLanes = 0.4f;

		// Token: 0x040027DC RID: 10204
		private int[] bandLaneStart = new int[8];

		// Token: 0x040027DD RID: 10205
		private float[] bandTopLane = new float[8];

		// Token: 0x040027DE RID: 10206
		private int[] bandLanes = new int[8];

		// Token: 0x040027DF RID: 10207
		private bool[] bandCollapsed = new bool[8];

		// Token: 0x040027E0 RID: 10208
		private int[] bandVisibleDepth = new int[8];

		// Token: 0x040027E1 RID: 10209
		private float[] bandBusyMs = new float[8];

		// Token: 0x040027E2 RID: 10210
		private int[] bandThreadIds = new int[8];

		// Token: 0x040027E3 RID: 10211
		private ProfilerLabelId[] bandNameLabelIds = new ProfilerLabelId[8];

		// Token: 0x040027E4 RID: 10212
		private int[] bandStickyDepth = new int[8];

		// Token: 0x040027E5 RID: 10213
		private long[] bandShrinkDeadline = new long[8];

		// Token: 0x040027E6 RID: 10214
		private float[] bandAnimStart = new float[8];

		// Token: 0x040027E7 RID: 10215
		private float[] bandAnimLanes = new float[8];

		// Token: 0x040027E8 RID: 10216
		private int bandCount;

		// Token: 0x040027E9 RID: 10217
		private float animTotalLanes;

		// Token: 0x040027EA RID: 10218
		private int totalIndexLanes;

		// Token: 0x040027EB RID: 10219
		private int lastBandRegistryVersion = -1;

		// Token: 0x040027EC RID: 10220
		private bool bandLayoutSnapPending = true;

		// Token: 0x040027ED RID: 10221
		private long bandAnimTimestamp;

		// Token: 0x040027EE RID: 10222
		private readonly HashSet<int> collapsedThreadIds = new HashSet<int>();

		// Token: 0x040027EF RID: 10223
		private int anchorThreadId = -1;

		// Token: 0x040027F0 RID: 10224
		private float anchorIntraLanes;

		// Token: 0x040027F1 RID: 10225
		private float anchorViewY;

		// Token: 0x040027F2 RID: 10226
		private static readonly long BandShrinkDelayTicks = Stopwatch.Frequency * 3L / 2L;

		// Token: 0x040027F3 RID: 10227
		private ZoneChunk hoverChunk;

		// Token: 0x040027F4 RID: 10228
		private int hoverNodeIndex = -1;

		// Token: 0x040027F5 RID: 10229
		private int hoverRootFrame = -1;

		// Token: 0x040027F6 RID: 10230
		private int hoverPauseFrame = -1;

		// Token: 0x040027F7 RID: 10231
		private int hoverPauseIndex = -1;

		// Token: 0x040027F8 RID: 10232
		private Vector2 lastMousePosition;

		// Token: 0x040027F9 RID: 10233
		private readonly float[] labelZoneX0 = new float[160];

		// Token: 0x040027FA RID: 10234
		private readonly float[] labelZoneX1 = new float[160];

		// Token: 0x040027FB RID: 10235
		private readonly int[] labelZoneDepth = new int[160];

		// Token: 0x040027FC RID: 10236
		private readonly List<ProfilerFlamechartWidget.PausedGapSpan> pausedGapSpans = new List<ProfilerFlamechartWidget.PausedGapSpan>();

		// Token: 0x040027FD RID: 10237
		private readonly List<ProfilerFlamechartWidget.FrameEntry> frameEntries = new List<ProfilerFlamechartWidget.FrameEntry>();

		// Token: 0x040027FE RID: 10238
		private long layoutOriginNs = -1L;

		// Token: 0x040027FF RID: 10239
		private readonly ProfilerPanZoomView view = new ProfilerPanZoomView(0.005);

		// Token: 0x04002800 RID: 10240
		private static readonly ValueTuple<double, double, int, int> ForcedSelectionKey = new ValueTuple<double, double, int, int>(double.NaN, double.NaN, -2, -2);

		// Token: 0x04002801 RID: 10241
		[TupleElementNames(new string[]
		{
			"rangeStart",
			"rangeEnd",
			"newestOrdinal",
			"oldestOrdinal"
		})]
		private ValueTuple<double, double, int, int> lastSelectionKey = ProfilerFlamechartWidget.ForcedSelectionKey;

		// Token: 0x04002802 RID: 10242
		private bool snapView;

		// Token: 0x04002803 RID: 10243
		private bool dragArmed;

		// Token: 0x04002804 RID: 10244
		private bool dragSelecting;

		// Token: 0x04002805 RID: 10245
		private float dragArmX;

		// Token: 0x04002806 RID: 10246
		private double dragStartUs;

		// Token: 0x04002807 RID: 10247
		private double dragEndUs;

		// Token: 0x04002808 RID: 10248
		private bool suppressSelectionZoom;

		// Token: 0x04002809 RID: 10249
		private readonly ProfilerSession session;

		// Token: 0x0400280A RID: 10250
		private readonly StringBuilder durationScratch = new StringBuilder();

		// Token: 0x0400280B RID: 10251
		private readonly Dictionary<int, float> labelWidths = new Dictionary<int, float>();

		// Token: 0x0400280C RID: 10252
		private FrameProfiler.FrameSnapshot RetainedSnapshot;

		// Token: 0x0400280D RID: 10253
		private int SelectedOffset = -1;

		// Token: 0x0400280E RID: 10254
		private int SelectedOldestOffset = -1;

		// Token: 0x0400280F RID: 10255
		private double SelectionRangeStart = -1.0;

		// Token: 0x04002810 RID: 10256
		private double SelectionRangeEnd = -1.0;

		// Token: 0x04002811 RID: 10257
		private int SelectedZoneOrdinal = -1;

		// Token: 0x04002812 RID: 10258
		private readonly List<int> SelectedPathIds = new List<int>();

		// Token: 0x04002813 RID: 10259
		public Action<ProfilerZoneSelection> ZoneSelected;

		// Token: 0x04002814 RID: 10260
		private static readonly Color[] LightColors = ProfilerFlamechartWidget.BuildPalette(0.3f);

		// Token: 0x04002815 RID: 10261
		private static readonly Color[] BaseColors = ProfilerFlamechartWidget.BuildPalette(0f);

		// Token: 0x04002816 RID: 10262
		private float metaPauseMs;

		// Token: 0x04002817 RID: 10263
		private int metaPauseCount;

		// Token: 0x04002818 RID: 10264
		private int lastLayoutNewestOrdinal = int.MinValue;

		// Token: 0x04002819 RID: 10265
		private int lastLayoutCount = -1;

		// Token: 0x0400281A RID: 10266
		private bool lastLayoutUseSpikes;

		// Token: 0x0400281B RID: 10267
		private bool lastLayoutRetained;

		// Token: 0x0400281C RID: 10268
		private double lastLayoutViewStartUs;

		// Token: 0x0400281D RID: 10269
		private double lastLayoutViewSpanUs;

		// Token: 0x0400281E RID: 10270
		private readonly Dictionary<int, string> frameNames = new Dictionary<int, string>();

		// Token: 0x0400281F RID: 10271
		[TupleElementNames(new string[]
		{
			"relMs",
			"stepMs",
			"text"
		})]
		private readonly ValueTuple<float, float, string>[] rulerLabels = new ValueTuple<float, float, string>[24];

		// Token: 0x04002820 RID: 10272
		private bool mouseInside;

		// Token: 0x04002821 RID: 10273
		private string tooltipText;

		// Token: 0x04002822 RID: 10274
		private bool hoverRectVisible;

		// Token: 0x04002823 RID: 10275
		private Rect hoverRectBounds;

		// Token: 0x04002824 RID: 10276
		private bool pauseBandVisible;

		// Token: 0x04002825 RID: 10277
		private Rect pauseBandBounds;

		// Token: 0x04002826 RID: 10278
		private static readonly Color HoverStrokeColor = ProfilerGui.Hex(2868903935U);

		// Token: 0x04002827 RID: 10279
		private static readonly Color SelectionStrokeColor = ProfilerGui.Hex(uint.MaxValue);

		// Token: 0x04002828 RID: 10280
		private static readonly Color PausedGapColor = ProfilerGui.Hex(822083583U);

		// Token: 0x02002A98 RID: 10904
		private struct ZoneDraw
		{
			// Token: 0x0400AE7B RID: 44667
			public ZoneChunk chunk;

			// Token: 0x0400AE7C RID: 44668
			public int index;

			// Token: 0x0400AE7D RID: 44669
			public int depth;

			// Token: 0x0400AE7E RID: 44670
			public double startUs;

			// Token: 0x0400AE7F RID: 44671
			public double endUs;

			// Token: 0x0400AE80 RID: 44672
			public bool folded;
		}

		// Token: 0x02002A99 RID: 10905
		private sealed class FrameZones
		{
			// Token: 0x06010704 RID: 67332 RVA: 0x005C7FA2 File Offset: 0x005C61A2
			public void Invalidate()
			{
				Array.Fill<double>(this.minVisibleUs, double.NaN);
			}

			// Token: 0x0400AE81 RID: 44673
			public List<ProfilerFlamechartWidget.ZoneDraw>[] perBand = Array.Empty<List<ProfilerFlamechartWidget.ZoneDraw>>();

			// Token: 0x0400AE82 RID: 44674
			public double[] minVisibleUs = Array.Empty<double>();
		}

		// Token: 0x02002A9A RID: 10906
		private struct PauseNode
		{
			// Token: 0x0400AE83 RID: 44675
			public int frameIndex;

			// Token: 0x0400AE84 RID: 44676
			public int pauseIndex;

			// Token: 0x0400AE85 RID: 44677
			public float x0;

			// Token: 0x0400AE86 RID: 44678
			public float x1;

			// Token: 0x0400AE87 RID: 44679
			public float laneY;
		}

		// Token: 0x02002A9B RID: 10907
		private struct CollapsedRun
		{
			// Token: 0x0400AE88 RID: 44680
			public float x0;

			// Token: 0x0400AE89 RID: 44681
			public float x1;

			// Token: 0x0400AE8A RID: 44682
			public float laneY;

			// Token: 0x0400AE8B RID: 44683
			public int count;

			// Token: 0x0400AE8C RID: 44684
			public double totalMicroseconds;

			// Token: 0x0400AE8D RID: 44685
			public int frameIndex;

			// Token: 0x0400AE8E RID: 44686
			public int band;

			// Token: 0x0400AE8F RID: 44687
			public ZoneChunk chunk;

			// Token: 0x0400AE90 RID: 44688
			public int nodeIndex;

			// Token: 0x0400AE91 RID: 44689
			public Color color;
		}

		// Token: 0x02002A9C RID: 10908
		private struct FrameEntry
		{
			// Token: 0x0400AE92 RID: 44690
			public FrameProfiler.FrameSnapshot snapshot;

			// Token: 0x0400AE93 RID: 44691
			public int ordinal;

			// Token: 0x0400AE94 RID: 44692
			public double startUs;

			// Token: 0x0400AE95 RID: 44693
			public double endUs;

			// Token: 0x0400AE96 RID: 44694
			public double boundaryUs;

			// Token: 0x0400AE97 RID: 44695
			public double maxEndUs;

			// Token: 0x0400AE98 RID: 44696
			public ProfilerFlamechartWidget.FrameZones zones;
		}

		// Token: 0x02002A9D RID: 10909
		private struct PausedGapSpan
		{
			// Token: 0x0400AE99 RID: 44697
			public float x0;

			// Token: 0x0400AE9A RID: 44698
			public float x1;

			// Token: 0x0400AE9B RID: 44699
			public double durationUs;
		}

		// Token: 0x02002A9E RID: 10910
		private struct ZoneHit
		{
			// Token: 0x17002BE7 RID: 11239
			// (get) Token: 0x06010706 RID: 67334 RVA: 0x005C7FD6 File Offset: 0x005C61D6
			public bool IsRoot
			{
				get
				{
					return this.chunk == null;
				}
			}

			// Token: 0x0400AE9C RID: 44700
			public int frameIndex;

			// Token: 0x0400AE9D RID: 44701
			public int band;

			// Token: 0x0400AE9E RID: 44702
			public ZoneChunk chunk;

			// Token: 0x0400AE9F RID: 44703
			public int nodeIndex;

			// Token: 0x0400AEA0 RID: 44704
			public int depth;

			// Token: 0x0400AEA1 RID: 44705
			public double startUs;

			// Token: 0x0400AEA2 RID: 44706
			public double endUs;
		}
	}
}
