using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x02000791 RID: 1937
	public sealed class ProfilerHistogramWidget
	{
		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x06003D88 RID: 15752 RVA: 0x00187B48 File Offset: 0x00185D48
		// (set) Token: 0x06003D89 RID: 15753 RVA: 0x00187B50 File Offset: 0x00185D50
		public bool AllocMode { get; private set; }

		// Token: 0x06003D8A RID: 15754 RVA: 0x00187B5C File Offset: 0x00185D5C
		public ProfilerHistogramWidget(ProfilerSession session)
		{
			this.session = session;
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x06003D8B RID: 15755 RVA: 0x00187C03 File Offset: 0x00185E03
		private int SlotCapacity
		{
			get
			{
				if (!this.session.IsSpikes)
				{
					return FrameProfiler.HistoryCapacity;
				}
				return 24;
			}
		}

		// Token: 0x06003D8C RID: 15756 RVA: 0x00187C1A File Offset: 0x00185E1A
		public static float NormalizeAlloc(long bytes)
		{
			if (bytes > 0L)
			{
				return Mathf.Max(0.0001f, Mathf.Clamp01((MathF.Log2((float)bytes) - 7f) / 18f));
			}
			return 0f;
		}

		// Token: 0x06003D8D RID: 15757 RVA: 0x00187C49 File Offset: 0x00185E49
		public void SetAllocMode(bool value)
		{
			if (this.AllocMode == value)
			{
				return;
			}
			this.AllocMode = value;
			this.Clear();
		}

		// Token: 0x06003D8E RID: 15758 RVA: 0x00187C62 File Offset: 0x00185E62
		public void NotifySourceChanged()
		{
			this.lastPushedCaptureCount = -1;
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x00187C6C File Offset: 0x00185E6C
		public void Clear()
		{
			this.bars.Clear();
			this.gcMarks.Clear();
			this.pauseMarks.Clear();
			this.spikeLabels.Clear();
			this.lastPushedEndNs = -1L;
			this.lastPushedCaptureCount = -1;
			this.lastVersion = -1;
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x00187CBC File Offset: 0x00185EBC
		public void Refresh()
		{
			if (this.lastSlotCapacity != this.SlotCapacity)
			{
				this.lastSlotCapacity = this.SlotCapacity;
				this.viewAnimation.Cancel();
				this.zoom = 1f;
				this.rightOffset = 0f;
			}
			if (this.lastVersion == this.session.Version && this.lastOrdinal == this.session.Ordinal && this.lastCaptureCount == FrameProfiler.CompletedFrameCount && this.lastSpikeCount == FrameProfiler.SpikeCount)
			{
				return;
			}
			this.lastVersion = this.session.Version;
			this.lastOrdinal = this.session.Ordinal;
			this.lastCaptureCount = FrameProfiler.CompletedFrameCount;
			this.lastSpikeCount = FrameProfiler.SpikeCount;
			int capacity = Mathf.Max(1, this.SlotCapacity);
			this.bars.EnsureCapacity(capacity);
			this.gcMarks.EnsureCapacity(capacity);
			this.pauseMarks.EnsureCapacity(capacity);
			if (this.session.IsSpikes)
			{
				this.RefreshSpikeBars();
				return;
			}
			this.RefreshHistoryBars();
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x00187DC8 File Offset: 0x00185FC8
		private void RefreshSpikeBars()
		{
			this.bars.Clear();
			this.gcMarks.Clear();
			this.pauseMarks.Clear();
			this.spikeLabels.Clear();
			this.lastPushedEndNs = -1L;
			this.lastPushedCaptureCount = -1;
			int spikeCount = FrameProfiler.SpikeCount;
			for (int i = 0; i < spikeCount; i++)
			{
				FrameProfiler.FrameSnapshot frameSnapshot;
				this.spikeLabels.Add(FrameProfiler.TryGetSpikeSnapshot(i, out frameSnapshot) ? ("#" + frameSnapshot.frameIndex.ToString()) : "");
			}
			for (int j = spikeCount - 1; j >= 0; j--)
			{
				FrameProfiler.FrameSnapshot frameSnapshot2;
				this.PushBar(FrameProfiler.TryGetSpikeSnapshot(j, out frameSnapshot2) ? frameSnapshot2 : null);
			}
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x00187E78 File Offset: 0x00186078
		private void RefreshHistoryBars()
		{
			this.spikeLabels.Clear();
			int completedFrameCount = FrameProfiler.CompletedFrameCount;
			int historyCount = FrameProfiler.HistoryCount;
			int num = completedFrameCount - this.lastPushedCaptureCount;
			bool flag = this.lastPushedCaptureCount >= 0 && num >= 0 && num <= historyCount && Mathf.Min(this.SlotCapacity, this.bars.Count + num) == historyCount;
			int num2 = flag ? (num - 1) : (historyCount - 1);
			if (!flag)
			{
				this.bars.Clear();
				this.gcMarks.Clear();
				this.pauseMarks.Clear();
				this.lastPushedEndNs = -1L;
			}
			for (int i = num2; i >= 0; i--)
			{
				FrameProfiler.FrameSnapshot frameSnapshot;
				this.PushBar(FrameProfiler.TryGetHistorySnapshot(i, out frameSnapshot) ? frameSnapshot : null);
			}
			this.lastPushedCaptureCount = completedFrameCount;
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x00187F38 File Offset: 0x00186138
		private void PushBar(FrameProfiler.FrameSnapshot snapshot)
		{
			if (snapshot == null)
			{
				this.bars.Push(0f);
				this.gcMarks.Push(0f);
				this.pauseMarks.Push(0f);
				return;
			}
			this.bars.Push(this.AllocMode ? ProfilerHistogramWidget.NormalizeAlloc(snapshot.DisplayAllocBytes) : Mathf.Clamp01(snapshot.totalMs / 66.666664f));
			this.gcMarks.Push(ProfilerHistogramWidget.GcMarkValue(snapshot));
			this.pauseMarks.Push(this.session.IsSpikes ? 0f : this.PauseGapMsBefore(snapshot));
			this.lastPushedEndNs = snapshot.endTimeNanoseconds;
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x00187FF0 File Offset: 0x001861F0
		private float PauseGapMsBefore(FrameProfiler.FrameSnapshot snapshot)
		{
			if (this.lastPushedEndNs < 0L)
			{
				return 0f;
			}
			for (int i = 0; i < FrameProfiler.CaptureGapCount; i++)
			{
				FrameProfiler.CaptureGap captureGap = FrameProfiler.GetCaptureGap(i);
				if (captureGap.beginTimeNanoseconds < snapshot.beginTimeNanoseconds && captureGap.endTimeNanoseconds > this.lastPushedEndNs)
				{
					return (float)(captureGap.endTimeNanoseconds - captureGap.beginTimeNanoseconds) / 1000000f;
				}
			}
			return 0f;
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x0018805A File Offset: 0x0018625A
		private static float GcMarkValue(FrameProfiler.FrameSnapshot snapshot)
		{
			if (snapshot.gen0Collections + snapshot.gen1Collections + snapshot.gen2Collections == 0)
			{
				return 0f;
			}
			return Mathf.Max(snapshot.pauseMs, 0.0001f);
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x00188088 File Offset: 0x00186288
		public void Draw(Rect rect)
		{
			ProfilerGui.Fill(rect, ProfilerGui.SunkenBackground);
			GUI.BeginGroup(rect);
			try
			{
				this.width = rect.width;
				int count = this.bars.Count;
				if (this.width >= 8f && count != 0)
				{
					this.HandleInput(new Rect(0f, 0f, rect.width, rect.height), count);
					if (ProfilerGui.Painting)
					{
						this.Paint(rect.width, rect.height, count);
					}
				}
			}
			finally
			{
				GUI.EndGroup();
			}
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x0018812C File Offset: 0x0018632C
		private void HandleInput(Rect local, int n)
		{
			Event current = Event.current;
			Vector2 mousePosition = current.mousePosition;
			bool flag = local.Contains(mousePosition);
			if (current.type == EventType.mouseDown && flag)
			{
				if (current.button == 0)
				{
					if (current.clickCount >= 2)
					{
						this.session.ClearSelection();
					}
					else
					{
						this.selecting = true;
						int newestOffset;
						if (this.hoverOffset >= 0)
						{
							this.session.BeginSelection(this.hoverOffset);
						}
						else if (this.TryGetOffsetAt(mousePosition.x, out newestOffset))
						{
							this.session.BeginSelection(newestOffset);
						}
					}
				}
				else
				{
					this.panning = true;
					this.panLastX = mousePosition.x;
				}
				current.Use();
				return;
			}
			if (current.type == EventType.scrollWheel && flag)
			{
				this.OnWheel(current.delta, mousePosition.x, n);
				current.Use();
				return;
			}
			if (current.type == EventType.repaint)
			{
				if (this.selecting)
				{
					int newestOffset2;
					if (!Input.GetMouseButton(0))
					{
						this.selecting = false;
					}
					else if (this.TryGetOffsetAt(mousePosition.x, out newestOffset2))
					{
						this.session.ExtendSelection(newestOffset2);
					}
				}
				if (this.panning)
				{
					if (!Input.GetMouseButton(1) && !Input.GetMouseButton(2))
					{
						this.panning = false;
					}
					else
					{
						this.PanTo(mousePosition.x, n);
					}
				}
				this.hoverOffset = ((flag && !this.selecting && !this.panning) ? this.FindHover(mousePosition) : -1);
			}
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x00188290 File Offset: 0x00186490
		private void Paint(float w, float h, int n)
		{
			float num = (this.spikeLabels.Count == n && this.Pitch(w) >= 24f) ? 14f : 0f;
			float num2 = h - num - 9f;
			this.gcRowTop = num2;
			int num3 = (this.SelectedOffset >= 0) ? this.session.Ordinal : -1;
			if (num3 != this.lastSeenSelectedOrdinal)
			{
				this.lastSeenSelectedOrdinal = num3;
				if (!this.selecting)
				{
					this.EnsureSelectionVisible(n);
				}
			}
			this.AdvanceAnimation();
			this.ClampRightOffset(n);
			float num4 = (float)Mathf.Max(1, this.SlotCapacity) / this.zoom;
			this.viewLeft = ProfilerHistogramWidget.LeftAgeOf(n, num4, this.rightOffset);
			float num5 = this.Pitch(w);
			float num6 = Mathf.Clamp(num5 * 0.7f, 2f, Mathf.Max(2f, num5 - 1f));
			float num7 = num6 * 0.5f;
			float num8 = 92f + ((float)this.SlotCapacity - this.viewLeft) * num5;
			if (num8 < w)
			{
				ProfilerGui.Fill(Mathf.Max(92f, num8), 0f, w, h, ProfilerHistogramWidget.OutsideCapacityColor);
			}
			this.PaintGuides(w, num2);
			int selectedOffset = this.SelectedOffset;
			int num9 = Mathf.Min(Mathf.Max(this.SelectedOldestOffset, selectedOffset), n);
			float num10 = Mathf.Max(92f, this.SlotCenterX((float)num9, num5, n) - num7);
			float num11 = Mathf.Min(w, this.SlotCenterX((float)selectedOffset, num5, n) + num7);
			bool flag = selectedOffset >= 0 && selectedOffset < n && num11 > num10;
			Rect rect = Rect.MinMaxRect(num10, 0f, num11, num2);
			if (flag)
			{
				ProfilerGui.Fill(rect, ProfilerHistogramWidget.SelectionColor);
			}
			if (this.hoverOffset >= 0)
			{
				float num12 = this.SlotCenterX((float)this.hoverOffset, num5, n);
				ProfilerGui.Fill(Mathf.Max(92f, num12 - num5 * 0.5f), 0f, Mathf.Min(w, num12 + num5 * 0.5f), num2, ProfilerHistogramWidget.HoverColor);
			}
			float budgetMs = this.session.BudgetMs;
			float budgetNorm = budgetMs / 66.666664f;
			int num13 = Mathf.Max(0, n - 2 - Mathf.CeilToInt(this.viewLeft + num4));
			int num14 = Mathf.Min(n - 1, n - Mathf.FloorToInt(this.viewLeft));
			for (int i = num13; i <= num14; i++)
			{
				float num15 = this.SlotCenterX((float)i, num5, n);
				float num16 = Mathf.Max(92f, num15 - num7);
				float num17 = Mathf.Min(w, num15 + num7);
				if (num17 > 92f && num16 < w)
				{
					if (this.gcMarks[i] > 0f)
					{
						ProfilerGui.Fill(num16, num2 + 2f, num17, num2 + 9f - 1f, ProfilerHistogramWidget.BucketColors[ProfilerHistogramWidget.GcBucketOf(this.gcMarks[i], budgetMs)]);
					}
					if (this.pauseMarks[i] > 0f)
					{
						float num18 = num15 - num5 * 0.5f;
						float num19 = Mathf.Max(1.5f, (num5 - num6) * 0.5f);
						ProfilerGui.Fill(Mathf.Max(92f, num18 - num19), 0f, Mathf.Min(w, num18 + num19), num2 + 9f, ProfilerHistogramWidget.PausedGapColor);
					}
					float num20 = this.bars[i];
					if (!this.AllocMode || num20 > 0f)
					{
						float num21 = Mathf.Max(1f, num20 * num2);
						ProfilerGui.Fill(num16, num2 - num21, num17, num2, ProfilerHistogramWidget.BucketColors[this.AllocMode ? 4 : ProfilerHistogramWidget.BucketOf(num20, budgetNorm)]);
					}
					if (num > 0f)
					{
						ProfilerGui.Label(new Rect(num15 - num5 * 0.5f, num2 + 9f, num5, num), this.spikeLabels[i], ProfilerGui.TextFaint, TextAnchor.MiddleCenter, GameFont.Tiny);
					}
				}
			}
			if (flag)
			{
				ProfilerGui.Outline(rect, (rect.width < 8f) ? ProfilerHistogramWidget.FadedSelectionStrokeColor : ProfilerHistogramWidget.SelectionStrokeColor);
			}
			if (this.hoverOffset >= 0)
			{
				this.PaintTooltip();
			}
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x001886B4 File Offset: 0x001868B4
		private void PaintGuides(float w, float plotH)
		{
			StringBuilder scratch = ProfilerGui.Scratch;
			if (this.AllocMode)
			{
				for (int i = 0; i < ProfilerHistogramWidget.AllocGuideBytes.Length; i++)
				{
					float num = plotH - ProfilerHistogramWidget.NormalizeAlloc(ProfilerHistogramWidget.AllocGuideBytes[i]) * plotH;
					ProfilerGui.Fill(new Rect(92f, num, w - 92f, 1f), ProfilerHistogramWidget.GuideColor);
					if (ProfilerHistogramWidget.allocGuideLabels[i] == null)
					{
						scratch.Clear();
						ProfilerTreeModel.AppendAllocBytes(scratch, ProfilerHistogramWidget.AllocGuideBytes[i], " ");
						ProfilerHistogramWidget.allocGuideLabels[i] = scratch.ToString();
					}
					ProfilerGui.Label(new Rect(0f, Mathf.Clamp(num - 8f, 0f, plotH - 16f), 84f, 16f), ProfilerHistogramWidget.allocGuideLabels[i], ProfilerGui.TextFaint, TextAnchor.MiddleRight, GameFont.Tiny);
				}
				return;
			}
			float num2 = float.MaxValue;
			for (int j = 0; j < ProfilerHistogramWidget.GuideFps.Length; j++)
			{
				float num3 = 1000f / (float)ProfilerHistogramWidget.GuideFps[j];
				if (num3 <= 66.666664f)
				{
					float num4 = plotH - num3 / 66.666664f * plotH;
					ProfilerGui.Fill(new Rect(92f, num4, w - 92f, 1f), ProfilerHistogramWidget.GuideColor);
					float num5 = Mathf.Clamp(num4 - 8f, 0f, plotH - 16f);
					if (Mathf.Abs(num5 - num2) >= 13f)
					{
						num2 = num5;
						ProfilerGui.Label(new Rect(0f, num5, 84f, 16f), ProfilerHistogramWidget.GuideLabel(j, num3), ProfilerGui.TextFaint, TextAnchor.MiddleRight, GameFont.Tiny);
					}
				}
			}
			float budgetMs = this.session.BudgetMs;
			if (budgetMs > 0f && budgetMs <= 66.666664f)
			{
				ProfilerGui.Fill(new Rect(92f, plotH - budgetMs / 66.666664f * plotH, w - 92f, 1f), ProfilerHistogramWidget.BudgetGuideColor);
			}
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x001888A0 File Offset: 0x00186AA0
		private static string GuideLabel(int index, float ms)
		{
			if (ProfilerHistogramWidget.guideLabels[index] != null)
			{
				return ProfilerHistogramWidget.guideLabels[index];
			}
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			scratch.Append(ProfilerHistogramWidget.GuideFps[index]).Append(" FPS (");
			ProfilerTreeModel.AppendMs(scratch, ms, "0.#", " ");
			scratch.Append(')');
			return ProfilerHistogramWidget.guideLabels[index] = scratch.ToString();
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x06003D9B RID: 15771 RVA: 0x0018890D File Offset: 0x00186B0D
		private int SelectedOffset
		{
			get
			{
				if (this.session.IsSpikes || !this.session.FollowingNewest)
				{
					return this.session.DisplayOffset;
				}
				return -1;
			}
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x06003D9C RID: 15772 RVA: 0x00188936 File Offset: 0x00186B36
		private int SelectedOldestOffset
		{
			get
			{
				if (this.session.IsSpikes || !this.session.FollowingNewest)
				{
					return this.session.DisplayOffsetOldest;
				}
				return -1;
			}
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x0018895F File Offset: 0x00186B5F
		private static int BucketOf(float normalized, float budgetNorm)
		{
			if (normalized <= budgetNorm)
			{
				return 0;
			}
			if (normalized <= budgetNorm * 1.5f)
			{
				return 1;
			}
			if (normalized <= budgetNorm * 3f)
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x00188980 File Offset: 0x00186B80
		private static int GcBucketOf(float pauseMs, float budgetMs)
		{
			float num = (budgetMs > 0f) ? (pauseMs / budgetMs) : 0f;
			if (num >= 0.3f)
			{
				return 3;
			}
			if (num < 0.1f)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x001889B8 File Offset: 0x00186BB8
		private int FindHover(Vector2 position)
		{
			int num;
			if (position.x < 92f || !this.TryGetOffsetAt(position.x, out num))
			{
				return -1;
			}
			if (position.y >= this.gcRowTop - 2f && position.y <= this.gcRowTop + 9f + 2f)
			{
				return this.FindGcMark(num);
			}
			if (position.y >= this.gcRowTop)
			{
				return -1;
			}
			if (this.Pitch(this.width) < 5f)
			{
				return this.FindUnambiguousBar(num, position.y);
			}
			return num;
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x00188A4C File Offset: 0x00186C4C
		private void PaintTooltip()
		{
			FrameProfiler.FrameSnapshot frameSnapshot;
			if (!this.session.TryGetSnapshotAt(this.hoverOffset, out frameSnapshot))
			{
				return;
			}
			if (frameSnapshot.frameIndex != this.lastTooltipFrameIndex)
			{
				this.lastTooltipFrameIndex = frameSnapshot.frameIndex;
				StringBuilder scratch = ProfilerGui.Scratch;
				scratch.Clear();
				scratch.Append("Frame ").Append(frameSnapshot.frameIndex);
				scratch.Append("\ntotal ");
				ProfilerTreeModel.AppendMs(scratch, frameSnapshot.totalMs, "0.000", " ");
				if (frameSnapshot.gen0Collections + frameSnapshot.gen1Collections + frameSnapshot.gen2Collections > 0)
				{
					scratch.Append(" / GC ").Append(frameSnapshot.gen0Collections).Append('/').Append(frameSnapshot.gen1Collections).Append('/').Append(frameSnapshot.gen2Collections);
					if (frameSnapshot.promotedBytes > 0L)
					{
						scratch.Append(", promoted ");
						ProfilerTreeModel.AppendAllocBytes(scratch, frameSnapshot.promotedBytes, " ");
					}
				}
				if (frameSnapshot.pauseCount > 0)
				{
					scratch.Append('\n').Append(frameSnapshot.pauseCount).Append((frameSnapshot.pauseCount > 1) ? " pauses" : " pause").Append(" / stopped ");
					ProfilerTreeModel.AppendMs(scratch, frameSnapshot.pauseMs, "0.000", " ");
				}
				if (frameSnapshot.DisplayAllocBytes > 0L)
				{
					scratch.Append("\nalloc ");
					ProfilerTreeModel.AppendAllocBytes(scratch, frameSnapshot.DisplayAllocBytes, " ");
					if (frameSnapshot.allocatedObjectCount > 0)
					{
						scratch.Append(" (").Append(frameSnapshot.allocatedObjectCount.ToString("n0")).Append(" objs)");
					}
				}
				if (this.hoverOffset < this.pauseMarks.Count && this.pauseMarks[this.hoverOffset] > 0f)
				{
					scratch.Append("\nprofiling paused ");
					ProfilerTreeModel.AppendMs(scratch, this.pauseMarks[this.hoverOffset], "0.0", " ");
					scratch.Append(" before this frame");
				}
				this.lastTooltip = scratch.ToString();
			}
			ProfilerGui.SetTooltip(this.lastTooltip);
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x00188C78 File Offset: 0x00186E78
		private int HoverSearchRadius()
		{
			return Mathf.FloorToInt(4f / Mathf.Max(0.01f, this.Pitch(this.width)));
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x00188C9C File Offset: 0x00186E9C
		private int FindUnambiguousBar(int hitOffset, float y)
		{
			int num = this.HoverSearchRadius();
			int num2 = -1;
			int num3 = Mathf.Min(this.bars.Count - 1, hitOffset + num);
			for (int i = Mathf.Max(0, hitOffset - num); i <= num3; i++)
			{
				if (y >= this.gcRowTop - this.bars[i] * this.gcRowTop)
				{
					if (num2 >= 0)
					{
						return -1;
					}
					num2 = i;
				}
			}
			return num2;
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x00188D04 File Offset: 0x00186F04
		private int FindGcMark(int offset)
		{
			if (offset < this.gcMarks.Count && this.gcMarks[offset] > 0f)
			{
				return offset;
			}
			int num = this.HoverSearchRadius();
			for (int i = 1; i <= num; i++)
			{
				if (offset - i >= 0 && offset - i < this.gcMarks.Count && this.gcMarks[offset - i] > 0f)
				{
					return offset - i;
				}
				if (offset + i < this.gcMarks.Count && this.gcMarks[offset + i] > 0f)
				{
					return offset + i;
				}
			}
			return -1;
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x00188DA0 File Offset: 0x00186FA0
		private void EnsureSelectionVisible(int barCount)
		{
			int selectedOffset = this.SelectedOffset;
			if (this.width <= 0f || selectedOffset < 0 || selectedOffset >= barCount)
			{
				return;
			}
			float num = (float)this.SlotCapacity / this.zoom;
			float num2 = ProfilerHistogramWidget.LeftAgeOf(barCount, num, this.rightOffset);
			int num3 = barCount - 1 - selectedOffset;
			if ((float)num3 >= num2 && (float)num3 < num2 + num)
			{
				return;
			}
			this.viewAnimation.Cancel();
			this.rightOffset = Mathf.Max(0f, (float)selectedOffset - num * 0.5f);
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x00188E20 File Offset: 0x00187020
		private void OnWheel(Vector2 delta, float mouseX, int n)
		{
			int num = Mathf.Max(1, this.SlotCapacity);
			float num2 = this.viewAnimation.Active ? ((float)this.viewAnimation.TargetStart) : this.rightOffset;
			float num3 = this.viewAnimation.Active ? ((float)this.viewAnimation.TargetSpan) : ((float)num / this.zoom);
			if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
			{
				this.viewAnimation.Cancel();
				this.zoom = (float)num / num3;
				this.rightOffset = num2 + delta.x / 3f * 0.15f * num3;
				this.ClampRightOffset(n);
				return;
			}
			float p = -delta.y / 3f;
			float num4 = Mathf.Max(92f, mouseX);
			float num5 = ProfilerHistogramWidget.LeftAgeOf(n, num3, num2) + (num4 - 92f) / ProfilerHistogramWidget.PitchOfSpan(num3, this.width);
			float num6 = Mathf.Pow(1.25f, p);
			float max = Mathf.Max(0.25f, 32f * (float)num / Mathf.Max(1f, this.width - 92f));
			float num7 = Mathf.Clamp((float)num / num3 * num6, 0.25f, max);
			float num8 = (float)num / num7;
			float num9 = num5 - (num4 - 92f) / ProfilerHistogramWidget.PitchOfSpan(num8, this.width);
			float num10 = Mathf.Clamp((float)n - num8 - num9, 0f, Mathf.Max(0f, (float)n - num8));
			this.viewAnimation.Start((double)this.rightOffset, (double)((float)num / this.zoom), (double)num10, (double)num8);
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x00188FC0 File Offset: 0x001871C0
		private void AdvanceAnimation()
		{
			if (!this.viewAnimation.Active)
			{
				return;
			}
			int num = Mathf.Max(1, this.SlotCapacity);
			double num2 = (double)this.rightOffset;
			double num3 = (double)((float)num / this.zoom);
			this.viewAnimation.Advance(ref num2, ref num3);
			this.rightOffset = (float)num2;
			this.zoom = (float)num / Mathf.Max(1f, (float)num3);
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x00189028 File Offset: 0x00187228
		private void PanTo(float x, int n)
		{
			float num = this.Pitch(this.width);
			if (num <= 0f)
			{
				return;
			}
			if (x != this.panLastX)
			{
				this.viewAnimation.Cancel();
			}
			this.rightOffset += (x - this.panLastX) / num;
			this.panLastX = x;
			this.ClampRightOffset(n);
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x00189084 File Offset: 0x00187284
		private bool TryGetOffsetAt(float positionX, out int newestOffset)
		{
			newestOffset = 0;
			float num = this.Pitch(this.width);
			int count = this.bars.Count;
			if (num <= 0f || count <= 0)
			{
				return false;
			}
			float num2 = Mathf.Clamp(positionX, 92f, this.width);
			float f = this.viewLeft + (num2 - 92f) / num;
			newestOffset = Mathf.Clamp(count - 1 - Mathf.FloorToInt(f), 0, count - 1);
			return true;
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x001890F4 File Offset: 0x001872F4
		private static float LeftAgeOf(int count, float span, float offset)
		{
			return Mathf.Max(0f, (float)count - span - offset);
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x00189106 File Offset: 0x00187306
		private float Pitch(float w)
		{
			return (w - 92f) / (float)Mathf.Max(1, this.SlotCapacity) * this.zoom;
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x00189124 File Offset: 0x00187324
		private static float PitchOfSpan(float span, float w)
		{
			return (w - 92f) / Mathf.Max(1f, span);
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x0018913C File Offset: 0x0018733C
		private void ClampRightOffset(int barCount)
		{
			float b = (float)barCount - (float)Mathf.Max(1, this.SlotCapacity) / this.zoom;
			this.rightOffset = Mathf.Clamp(this.rightOffset, 0f, Mathf.Max(0f, b));
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x00189182 File Offset: 0x00187382
		private float SlotCenterX(float newestOffset, float pitch, int barCount)
		{
			return 92f + ((float)(barCount - 1) - newestOffset - this.viewLeft + 0.5f) * pitch;
		}

		// Token: 0x0400284F RID: 10319
		public const float Height = 104f;

		// Token: 0x04002850 RID: 10320
		public const float ScaleMs = 66.666664f;

		// Token: 0x04002851 RID: 10321
		private static readonly int[] GuideFps = new int[]
		{
			120,
			60,
			30,
			20,
			15,
			10,
			5
		};

		// Token: 0x04002852 RID: 10322
		private static readonly long[] AllocGuideBytes = new long[]
		{
			1024L,
			8192L,
			65536L,
			524288L,
			4194304L
		};

		// Token: 0x04002853 RID: 10323
		private static readonly Color GuideColor = ProfilerGui.Hex(4281020476U);

		// Token: 0x04002854 RID: 10324
		private static readonly Color BudgetGuideColor = ProfilerGui.Hex(4285217330U);

		// Token: 0x04002855 RID: 10325
		private static readonly Color SelectionColor = ProfilerGui.Hex(2352631667U);

		// Token: 0x04002856 RID: 10326
		private static readonly Color SelectionStrokeColor = ProfilerGui.Hex(4284381608U);

		// Token: 0x04002857 RID: 10327
		private static readonly Color FadedSelectionStrokeColor = ProfilerGui.Hex(1717467560U);

		// Token: 0x04002858 RID: 10328
		private static readonly Color HoverColor = ProfilerGui.Hex(318767103U);

		// Token: 0x04002859 RID: 10329
		private static readonly Color OutsideCapacityColor = ProfilerGui.Hex(1140850688U);

		// Token: 0x0400285A RID: 10330
		private static readonly Color PausedGapColor = ProfilerGui.Hex(822083583U);

		// Token: 0x0400285B RID: 10331
		private static readonly Color[] BucketColors = new Color[]
		{
			ProfilerGui.Hex(4284267883U),
			ProfilerGui.Hex(4292265034U),
			ProfilerGui.Hex(4292905288U),
			ProfilerGui.Hex(4292893770U),
			ProfilerGui.Hex(4293306446U)
		};

		// Token: 0x0400285C RID: 10332
		private const float AllocLog2Floor = 7f;

		// Token: 0x0400285D RID: 10333
		private const float AllocLog2Range = 18f;

		// Token: 0x0400285E RID: 10334
		private const float MinZoom = 0.25f;

		// Token: 0x0400285F RID: 10335
		private const float PanPerNotch = 0.15f;

		// Token: 0x04002860 RID: 10336
		private const float MaxBarPitch = 32f;

		// Token: 0x04002861 RID: 10337
		private const float GutterWidth = 92f;

		// Token: 0x04002862 RID: 10338
		private const int AllocBucket = 4;

		// Token: 0x04002863 RID: 10339
		private const float GcRowHeight = 9f;

		// Token: 0x04002864 RID: 10340
		private const float UnambiguousHoverPitch = 5f;

		// Token: 0x04002865 RID: 10341
		private const float ScrollUnitsPerNotch = 3f;

		// Token: 0x04002866 RID: 10342
		private readonly ProfilerSession session;

		// Token: 0x04002867 RID: 10343
		private readonly ProfilerBarRing bars = new ProfilerBarRing();

		// Token: 0x04002868 RID: 10344
		private readonly ProfilerBarRing gcMarks = new ProfilerBarRing();

		// Token: 0x04002869 RID: 10345
		private readonly ProfilerBarRing pauseMarks = new ProfilerBarRing();

		// Token: 0x0400286A RID: 10346
		private readonly List<string> spikeLabels = new List<string>();

		// Token: 0x0400286B RID: 10347
		private readonly ProfilerViewAnimation viewAnimation = new ProfilerViewAnimation();

		// Token: 0x0400286C RID: 10348
		private long lastPushedEndNs = -1L;

		// Token: 0x0400286D RID: 10349
		private int lastVersion = -1;

		// Token: 0x0400286E RID: 10350
		private int lastOrdinal = -1;

		// Token: 0x0400286F RID: 10351
		private int lastCaptureCount = -1;

		// Token: 0x04002870 RID: 10352
		private int lastSpikeCount = -1;

		// Token: 0x04002871 RID: 10353
		private int lastPushedCaptureCount = -1;

		// Token: 0x04002872 RID: 10354
		private int lastSlotCapacity = -1;

		// Token: 0x04002873 RID: 10355
		private float zoom = 1f;

		// Token: 0x04002874 RID: 10356
		private float rightOffset;

		// Token: 0x04002875 RID: 10357
		private float viewLeft;

		// Token: 0x04002876 RID: 10358
		private float width;

		// Token: 0x04002877 RID: 10359
		private float gcRowTop;

		// Token: 0x04002878 RID: 10360
		private bool selecting;

		// Token: 0x04002879 RID: 10361
		private bool panning;

		// Token: 0x0400287A RID: 10362
		private float panLastX;

		// Token: 0x0400287B RID: 10363
		private int hoverOffset = -1;

		// Token: 0x0400287C RID: 10364
		private int lastSeenSelectedOrdinal = -1;

		// Token: 0x0400287E RID: 10366
		private static readonly string[] guideLabels = new string[ProfilerHistogramWidget.GuideFps.Length];

		// Token: 0x0400287F RID: 10367
		private static readonly string[] allocGuideLabels = new string[ProfilerHistogramWidget.AllocGuideBytes.Length];

		// Token: 0x04002880 RID: 10368
		private int lastTooltipFrameIndex = int.MinValue;

		// Token: 0x04002881 RID: 10369
		private string lastTooltip;
	}
}
