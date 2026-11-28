using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Waylith.Profiling;

namespace Verse.ProfilerUI
{
	// Token: 0x020007A7 RID: 1959
	public sealed class Window_FrameProfiler : Window
	{
		// Token: 0x06003F11 RID: 16145 RVA: 0x00191EB0 File Offset: 0x001900B0
		public Window_FrameProfiler() : base(null)
		{
			this.layer = WindowLayer.Super;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.doCloseX = false;
			this.closeOnAccept = false;
			this.closeOnClickedOutside = false;
			this.absorbInputAroundWindow = false;
			this.preventCameraMotion = false;
			this.forcePause = false;
			this.focusWhenOpened = true;
			this.soundAppear = null;
			this.soundClose = null;
			this.session = new ProfilerSession();
			this.histogram = new ProfilerHistogramWidget(this.session);
			this.callTree = new ProfilerCallTreeTable(this.session);
			this.callTree.CopyViewRequested = new Action(this.CopyView);
			this.allocTable = new ProfilerAllocTable(this.session);
			this.allocTable.CopyViewRequested = new Action(this.CopyView);
			this.allocTable.ZoneJumpRequested = new Action<ProfilerLabelId>(this.OnAllocZoneJump);
			this.allocTable.EnableTrackingRequested = new Action(this.EnableAllocTracking);
			this.vramTable = new ProfilerVramTable(this.session);
			this.vramTable.CopyViewRequested = new Action(this.CopyView);
			this.ramTable = new ProfilerRamTable(this.session);
			this.ramTable.CopyViewRequested = new Action(this.CopyView);
			this.activeSection = this.callTree;
			this.callTree.RowSelectedByUser = new Action(this.OnRowSelectedByUser);
			this.flamechart = new ProfilerFlamechartWidget(this.session);
			this.threadFilter = new ProfilerThreadFilter(this.session);
			this.settings = new ProfilerSettingsDrawer(this.session);
			this.flamechart.ZoneSelected = new Action<ProfilerZoneSelection>(this.OnZoneSelected);
			this.session.SelectionReset = new Action(this.OnSelectionReset);
			this.session.DisplayedFrameChanged = new Action(this.flamechart.ClearHighlight);
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06003F12 RID: 16146 RVA: 0x00192160 File Offset: 0x00190360
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1100f, 760f);
			}
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06003F13 RID: 16147 RVA: 0x00192171 File Offset: 0x00190371
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06003F14 RID: 16148 RVA: 0x00192178 File Offset: 0x00190378
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x0019217C File Offset: 0x0019037C
		public override void PreOpen()
		{
			base.PreOpen();
			Rect windowRect = ProfilerPrefs.Current.windowRect;
			if (windowRect.width >= Window_FrameProfiler.MinSize.x && windowRect.height >= Window_FrameProfiler.MinSize.y)
			{
				this.windowRect = windowRect;
			}
			this.ClampToScreen();
			FrameProfiler.SetEnabled(true);
			ProfilerPrefs.Apply();
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x001921D8 File Offset: 0x001903D8
		public override void PostClose()
		{
			base.PostClose();
			ProfilerPrefs.Current.windowRect = this.windowRect;
			ProfilerPrefs.Save();
			this.flamechart.Clear();
			if (ProfilerPrefs.Current.keepRunningWhenClosed)
			{
				FrameProfiler.SetPaused(false);
				return;
			}
			FrameProfiler.SetEnabled(false);
		}

		// Token: 0x06003F17 RID: 16151 RVA: 0x00192224 File Offset: 0x00190424
		private void OnSelectionReset()
		{
			this.flamechart.ClearHighlight();
			this.callTree.ResetSelection();
			this.histogram.NotifySourceChanged();
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x00192247 File Offset: 0x00190447
		private void SwitchSection(ProfilerRowSection section)
		{
			if (this.activeSection == section)
			{
				return;
			}
			this.activeSection = section;
			this.threadFilter.ShowBytes = (section == this.allocTable);
			section.View.SelectQuietly(-1);
			section.Invalidate();
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x00192280 File Offset: 0x00190480
		private void EnableAllocTracking()
		{
			ProfilerPrefs.Current.allocTracking = true;
			ProfilerPrefs.Apply();
			ProfilerPrefs.Save();
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x00192298 File Offset: 0x00190498
		private void OnAllocZoneJump(ProfilerLabelId labelId)
		{
			if (!this.session.HasData)
			{
				return;
			}
			this.session.Tree.CollectPathsWithLabel(labelId, this.allocJumpScratch);
			if (this.allocJumpScratch.Count == 0)
			{
				return;
			}
			ValueTuple<float, int> valueTuple = this.allocJumpScratch[0];
			for (int i = 1; i < this.allocJumpScratch.Count; i++)
			{
				if (this.allocJumpScratch[i].Item1 > valueTuple.Item1)
				{
					valueTuple = this.allocJumpScratch[i];
				}
			}
			this.RevealPathInCallTree(valueTuple.Item2);
			this.flamechart.HighlightZone(this.session.Snapshot.captureOrdinal, valueTuple.Item2);
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x00192350 File Offset: 0x00190550
		private void RevealPathInCallTree(int pathId)
		{
			this.session.Tree.AppendLabelChain(pathId, this.pathScratch);
			this.SwitchSection(this.callTree);
			if (this.session.Tree.Inverted)
			{
				this.session.Tree.Inverted = false;
				this.session.Invalidate();
			}
			this.callTree.ExpandTo(this.pathScratch);
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x001923C0 File Offset: 0x001905C0
		private void OnRowSelectedByUser()
		{
			int ordinal;
			if (this.callTree.TryGetSelectedZones(this.zoneScratch, out ordinal))
			{
				this.flamechart.HighlightZones(ordinal, this.zoneScratch);
				return;
			}
			this.flamechart.ClearHighlight();
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x00192400 File Offset: 0x00190600
		private void OnZoneSelected(ProfilerZoneSelection zone)
		{
			bool flag = ProfilerPathTable.Entries[zone.pathId].parentPathId == 0;
			bool flag2 = ProfilerThreadRegistry.IsSelected(zone.threadId);
			bool flag3;
			if (!this.session.PinOrdinal(zone.ordinal, !flag && flag2, out flag3))
			{
				return;
			}
			FrameProfiler.FrameSnapshot frameSnapshot;
			bool flag4 = this.session.TryGetSnapshotForOrdinal(zone.ordinal, out frameSnapshot);
			bool flag5 = flag4 && frameSnapshot.sectionCount > 0 && frameSnapshot.sections[0].threadId == zone.threadId;
			if ((flag3 || !flag2) && !flag)
			{
				ProfilerThreadRegistry.SelectOnly(zone.threadId);
				if (!flag5 && flag4)
				{
					for (int i = 1; i < frameSnapshot.sectionCount; i++)
					{
						if (frameSnapshot.sections[i].threadId == zone.threadId)
						{
							long num = frameSnapshot.beginTimeNanoseconds + frameSnapshot.sections[i].startNanoseconds;
							long endNs = Math.Max(frameSnapshot.beginTimeNanoseconds + frameSnapshot.sections[i].endNanoseconds, num + 1L);
							this.session.SelectZoneTimeRange(zone.ordinal, num, endNs);
							break;
						}
					}
				}
				else if (flag4 && !flag2 && !flag3)
				{
					this.session.SelectZoneTimeRange(zone.ordinal, frameSnapshot.beginTimeNanoseconds, frameSnapshot.endTimeNanoseconds);
				}
			}
			this.flamechart.HighlightZone(zone.ordinal, flag ? -1 : zone.pathId);
			if (flag)
			{
				return;
			}
			this.RevealPathInCallTree(zone.pathId);
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x0019259C File Offset: 0x0019079C
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			using (FrameProfiler.BeginRead())
			{
				this.UpdateNavRepeat();
				this.UpdateCameraBlock();
				long timestamp = Stopwatch.GetTimestamp();
				int refreshTick = this.session.RefreshTick;
				this.session.Poll();
				if (!this.session.HasData)
				{
					this.RefreshToolbar();
					if (this.activeSection == this.vramTable)
					{
						this.vramTable.Refresh();
					}
					if (this.activeSection == this.ramTable)
					{
						this.ramTable.Refresh();
					}
					if (this.hadData)
					{
						this.callTree.Clear();
						this.allocTable.Clear();
						this.histogram.Clear();
						this.flamechart.Clear();
					}
					this.hadData = false;
				}
				else
				{
					this.hadData = true;
					this.RefreshToolbar();
					this.threadFilter.Refresh();
					this.activeSection.Refresh();
					this.histogram.Refresh();
					this.flamechart.Refresh();
					this.RefreshSummary();
					if (this.session.RefreshTick != refreshTick)
					{
						this.session.ReportRefreshCost(timestamp);
					}
				}
			}
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x001926E8 File Offset: 0x001908E8
		private void UpdateCameraBlock()
		{
			bool flag = Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2);
			if (!flag)
			{
				this.mouseHeldFromWindow = false;
			}
			else if (!this.anyButtonLastFrame)
			{
				this.mouseHeldFromWindow = this.windowRect.Contains(UI.MousePositionOnUIInverted);
				this.unfocusSearchPending = (!this.mouseHeldFromWindow && this.searchFocused);
			}
			this.anyButtonLastFrame = flag;
			this.preventCameraMotion = (this.mouseHeldFromWindow || this.searchFocused);
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x00192770 File Offset: 0x00190970
		private void RefreshToolbar()
		{
			if (this.lastToolbarVersion == this.session.Version && this.lastToolbarOrdinal == this.session.Ordinal && this.lastToolbarCaptureCount == FrameProfiler.CompletedFrameCount)
			{
				return;
			}
			this.lastToolbarVersion = this.session.Version;
			this.lastToolbarOrdinal = this.session.Ordinal;
			this.lastToolbarCaptureCount = FrameProfiler.CompletedFrameCount;
			this.frameStepperText = (this.session.HasData ? ("Frame " + this.session.Snapshot.frameIndex.ToString()) : "Frame —");
			ProfilerFrameStats stats = this.session.Stats;
			this.medianText = Window_FrameProfiler.FrameMs(stats.medianMs);
			this.p99Text = Window_FrameProfiler.FrameMs(stats.p99Ms);
			this.budgetText = Window_FrameProfiler.FrameMs(this.session.BudgetMs);
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x0019285B File Offset: 0x00190A5B
		private static string FrameMs(float ms)
		{
			StringBuilder scratch = ProfilerGui.Scratch;
			scratch.Clear();
			ProfilerTreeModel.AppendFrameMs(scratch, ms, "0.0");
			return scratch.ToString();
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x0019287C File Offset: 0x00190A7C
		private void RefreshSummary()
		{
			if (!this.summaryGate.ShouldRefresh(this.session))
			{
				return;
			}
			FrameProfiler.FrameSnapshot snapshot = this.session.Snapshot;
			ProfilerFrameStats stats = this.session.Stats;
			ProfilerTreeModel.WindowSummary windowSummary = (this.session.Span > 1) ? this.session.SummarizeSelection() : default(ProfilerTreeModel.WindowSummary);
			StringBuilder scratch = ProfilerGui.Scratch;
			float num;
			if (this.session.Span > 1 && windowSummary.framesIncluded > 0)
			{
				num = windowSummary.averageFrameMs;
				this.summaryHead = "Avg over " + windowSummary.framesIncluded.ToString() + " frames";
				this.summaryTotalLabel = "    frame ";
				scratch.Clear();
				ProfilerTreeModel.AppendFrameMs(scratch, num, "0.000");
				this.summaryTotal = scratch.ToString();
				scratch.Clear();
				scratch.Append("    ").Append("ticks +").Append(windowSummary.averageTickerDelta.ToString("0.0")).Append("    ").Append("nodes ").Append(windowSummary.averageNodeCount.ToString("0"));
				Window_FrameProfiler.AppendAllocTail(scratch, windowSummary.averageAllocBytes, Mathf.RoundToInt(windowSummary.averageAllocCount), windowSummary.gen0Collections, windowSummary.gen1Collections, windowSummary.gen2Collections, windowSummary.averagePromotedBytes);
				if (windowSummary.averageOverflowedScopes > 0f)
				{
					scratch.Append("    ").Append("dropped scopes/frame ").Append(windowSummary.averageOverflowedScopes.ToString("0.0"));
				}
				this.summaryTail = scratch.ToString();
			}
			else
			{
				num = snapshot.totalMs;
				this.summaryHead = "Frame " + snapshot.frameIndex.ToString();
				this.summaryTotalLabel = "    total ";
				this.summaryTotal = Window_FrameProfiler.FrameMs(num);
				scratch.Clear();
				scratch.Append("    ").Append("ticks +").Append(snapshot.tickDelta).Append("    ").Append("nodes ").Append(snapshot.publishedNodeCount);
				Window_FrameProfiler.AppendAllocTail(scratch, snapshot.DisplayAllocBytes, snapshot.allocatedObjectCount, snapshot.gen0Collections, snapshot.gen1Collections, snapshot.gen2Collections, snapshot.promotedBytes);
				if (snapshot.overflowedScopes > 0)
				{
					scratch.Append("    ").Append("dropped scopes ").Append(snapshot.overflowedScopes);
				}
				if (snapshot.unbalancedEnds > 0)
				{
					scratch.Append("    ").Append("unbalanced ends ").Append(snapshot.unbalancedEnds);
				}
				if (snapshot.droppedEvents > 0L)
				{
					scratch.Append("    ").Append("dropped events ").Append(snapshot.droppedEvents);
				}
				if (snapshot.isSpike)
				{
					scratch.Append("    ").Append("SPIKE");
				}
				this.summaryTail = scratch.ToString();
			}
			float num2 = num - stats.medianMs;
			this.summaryTotalSeverity = ((num > this.session.BudgetMs) ? 1 : 0);
			this.summaryDeltaSeverity = ((num2 >= 0.5f) ? 1 : ((num2 <= -0.5f) ? -1 : 0));
			scratch.Clear();
			if (num2 >= 0f)
			{
				scratch.Append('+');
			}
			ProfilerTreeModel.AppendMs(scratch, num2, "0.0", " ");
			this.summaryDelta = scratch.ToString();
			long timestamp = Stopwatch.GetTimestamp();
			if (timestamp - this.slowUpdateTimestamp >= Stopwatch.Frequency)
			{
				this.slowUpdateTimestamp = timestamp;
				float num3 = (float)((snapshot.sectionCount > 0) ? snapshot.sections[0].recordedScopes : 0) * FrameProfiler.ScopeOverheadNanoseconds / 1000000f;
				float num4 = (snapshot.totalMs > 0f) ? (num3 / snapshot.totalMs * 100f) : 0f;
				this.overheadPercent = ((this.overheadPercent > 0f) ? Mathf.Lerp(this.overheadPercent, num4, 0.3f) : num4);
			}
			scratch.Clear();
			float scopeOverheadNanoseconds = FrameProfiler.ScopeOverheadNanoseconds;
			if (scopeOverheadNanoseconds > 0f)
			{
				scratch.Append("overhead ").Append(scopeOverheadNanoseconds.ToString("0")).Append(" ns/scope");
				if (this.overheadPercent > 0f)
				{
					scratch.Append(" (~").Append(this.overheadPercent.ToString("0.00")).Append("% of frame)");
				}
				scratch.Append(" / ");
			}
			scratch.Append("timer res ").Append(FrameProfiler.TimerResolutionNanoseconds.ToString("0.#")).Append(" ns");
			this.statusText = scratch.ToString();
		}

		// Token: 0x06003F23 RID: 16163 RVA: 0x00192D4C File Offset: 0x00190F4C
		private static void AppendAllocTail(StringBuilder builder, long allocBytes, int allocCount, int gen0, int gen1, int gen2, long promotedBytes)
		{
			if (allocBytes > 0L)
			{
				builder.Append("    ").Append("alloc ");
				ProfilerTreeModel.AppendAllocBytes(builder, allocBytes, " ");
				if (allocCount > 0)
				{
					builder.Append(" (").Append(allocCount.ToString("n0")).Append(" objs)");
				}
			}
			if (promotedBytes > 0L)
			{
				builder.Append("    ").Append("promoted ");
				ProfilerTreeModel.AppendAllocBytes(builder, promotedBytes, " ");
			}
			if (gen0 + gen1 + gen2 > 0)
			{
				builder.Append("    ").Append("GC ").Append(gen0).Append('/').Append(gen1).Append('/').Append(gen2);
			}
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x00192E18 File Offset: 0x00191018
		public override void DoWindowContents(Rect inRect)
		{
			using (FrameProfiler.BeginRead())
			{
				if (this.unfocusSearchPending)
				{
					this.unfocusSearchPending = false;
					this.searchFocused = false;
					UI.UnfocusCurrentControl();
				}
				ProfilerGui.ClearOverlays();
				this.settings.MaskInput(inRect);
				this.threadFilter.HandleInput();
				this.HandleWindowDrag(inRect);
				this.HandleKeys();
				bool flag = this.IsTopmost();
				ProfilerGui.Fill(inRect, flag ? ProfilerGui.FocusBorder : ProfilerGui.SurfaceBorder);
				ProfilerGui.Fill(inRect.ContractedBy(1f), ProfilerGui.WindowBackground);
				float num = inRect.y + 1f;
				this.DrawTitleBar(new Rect(inRect.x + 1f, num, inRect.width - 2f, 30f), flag);
				num += 30f;
				this.DrawToolbar(new Rect(inRect.x + 1f, num, inRect.width - 2f, 32f));
				num += 32f;
				ProfilerGui.Fill(new Rect(inRect.x + 1f, num, inRect.width - 2f, 1f), ProfilerGui.Divider);
				num += 8f;
				this.DrawHistogramHeader(new Rect(inRect.x + 12f, num, inRect.width - 24f, 22f));
				num += 28f;
				this.histogram.Draw(new Rect(inRect.x + 12f, num, inRect.width - 24f, 104f));
				num += 110f;
				this.DrawSummary(new Rect(inRect.x + 12f, num, inRect.width - 24f, 16f));
				num += 23f;
				ProfilerGui.Fill(new Rect(inRect.x + 1f, num, inRect.width - 2f, 1f), ProfilerGui.Divider);
				float num2 = inRect.yMax - 24f - 1f;
				num = this.DrawFlameSection(inRect, num + 1f, num2);
				ProfilerGui.Fill(Rect.MinMaxRect(inRect.x + 1f, num + 1f, inRect.xMax - 1f, num2), ProfilerGui.SectionBackground);
				this.DrawTabStrip(new Rect(inRect.x + 12f, num + 6f, inRect.width - 24f, 21f));
				num += 33f;
				ProfilerGui.Fill(new Rect(inRect.x + 1f, num, inRect.width - 2f, 1f), ProfilerGui.Divider);
				Rect rect = Rect.MinMaxRect(inRect.x + 1f, num + 1f, inRect.xMax - 1f, num2);
				if (this.session.HasData || this.activeSection == this.vramTable || this.activeSection == this.ramTable)
				{
					this.activeSection.Draw(rect);
				}
				else
				{
					ProfilerGui.Label(rect, this.session.IsSpikes ? "No spikes captured yet." : "No completed frames captured yet.", ProfilerGui.TextFaint, TextAnchor.MiddleCenter, GameFont.Tiny);
				}
				this.DrawStatusBar(new Rect(inRect.x + 1f, num2, inRect.width - 2f, 24f));
				this.threadFilter.DrawPopup(inRect);
				this.settings.Draw(inRect);
				ProfilerGui.FlushTooltip(inRect);
			}
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x001931D4 File Offset: 0x001913D4
		private bool IsTopmost()
		{
			if (!ProfilerGui.Painting)
			{
				return true;
			}
			IList<Window> windows = Find.WindowStack.Windows;
			return windows.Count > 0 && windows[windows.Count - 1] == this;
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x00193214 File Offset: 0x00191414
		private float DrawFlameSection(Rect inRect, float top, float bottom)
		{
			float num = bottom - top - 5f;
			if (num < 160f)
			{
				return top - 1f;
			}
			float num2 = Mathf.Clamp(Mathf.Round(num * this.flameFraction), 80f, num - 80f);
			Rect rect = new Rect(inRect.x + 1f, top + num2, inRect.width - 2f, 5f);
			Event current = Event.current;
			if (current.type == EventType.mouseDown && current.button == 0 && rect.Contains(current.mousePosition))
			{
				this.draggingSplitter = true;
				current.Use();
			}
			else if (this.draggingSplitter && current.type == EventType.repaint)
			{
				if (!Input.GetMouseButton(0))
				{
					this.draggingSplitter = false;
				}
				else
				{
					this.flameFraction = Mathf.Clamp01((current.mousePosition.y - top - 2.5f) / num);
				}
			}
			if (this.session.HasData)
			{
				this.flamechart.Draw(new Rect(inRect.x + 1f, top, inRect.width - 2f, num2));
			}
			else
			{
				ProfilerGui.Fill(new Rect(inRect.x + 1f, top, inRect.width - 2f, num2), ProfilerGui.SunkenBackground);
			}
			ProfilerGui.Fill(rect, (this.draggingSplitter || rect.Contains(current.mousePosition)) ? ProfilerGui.FocusBorder : ProfilerGui.Divider);
			return rect.yMax - 1f;
		}

		// Token: 0x06003F27 RID: 16167 RVA: 0x00193398 File Offset: 0x00191598
		private void DrawTitleBar(Rect rect, bool focused)
		{
			ProfilerGui.Fill(rect, focused ? ProfilerGui.TitleBar : ProfilerGui.TitleBarUnfocused);
			ProfilerGui.Fill(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), ProfilerGui.Divider);
			ProfilerGui.Run(rect.x + 12f, rect.y, rect.height, "Frame profiler", focused ? ProfilerGui.TextMain : ProfilerGui.TextDim);
			float num = rect.xMax - 8f;
			Rect rect2 = new Rect(num - 22f, rect.y + 4f, 22f, 22f);
			if (ProfilerGui.Ghost(rect2, "×", false))
			{
				this.Close(true);
				return;
			}
			num = rect2.x - 8f;
			if (Window_FrameProfiler.TitleToggle(ref num, rect, "Settings", this.settings.IsOpen))
			{
				this.settings.Toggle();
			}
		}

		// Token: 0x06003F28 RID: 16168 RVA: 0x0019349C File Offset: 0x0019169C
		private static bool TitleToggle(ref float right, Rect bar, string label, bool active)
		{
			float num = ProfilerGui.Width(label, GameFont.Tiny) + 18f;
			Rect rect = new Rect(right - num, bar.y + 5f, num, 20f);
			right = rect.x - 4f;
			return ProfilerGui.Chip(rect, label, active, true, null);
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x001934F8 File Offset: 0x001916F8
		private void DrawToolbar(Rect rect)
		{
			float x = rect.x + 12f;
			float num = rect.y + 5f;
			Rect rect2 = new Rect(x, num, 64f, 22f);
			if (ProfilerGui.Chip(rect2, this.session.IsPaused ? "Resume" : "Pause", this.session.IsPaused, true, null))
			{
				this.session.SetPaused(!this.session.IsPaused);
			}
			x = rect2.xMax + 8f;
			Window_FrameProfiler.ToolbarDivider(ref x, num);
			float num2 = Mathf.Max(72f, ProfilerGui.Width(this.frameStepperText, GameFont.Tiny)) + 16f;
			Rect rect3 = new Rect(x, num, 24f + num2 + 24f + 30f + 3f, 22f);
			ProfilerGui.Bordered(rect3, ProfilerGui.TitleBar, ProfilerGui.Divider);
			Rect rect4 = new Rect(rect3.x + 1f, num + 1f, 23f, 20f);
			if (ProfilerGui.Ghost(rect4, "◀", false))
			{
				this.session.Step(1);
			}
			ProfilerGui.Fill(new Rect(rect4.xMax, num + 1f, 1f, 20f), ProfilerGui.Divider);
			ProfilerGui.Label(new Rect(rect4.xMax + 1f, num, num2, 22f), this.frameStepperText, ProfilerGui.TextBright, TextAnchor.MiddleCenter, GameFont.Tiny);
			rect4 = new Rect(rect4.xMax + 1f + num2, num + 1f, 24f, 20f);
			ProfilerGui.Fill(new Rect(rect4.x, num + 1f, 1f, 20f), ProfilerGui.Divider);
			if (ProfilerGui.Ghost(new Rect(rect4.x + 1f, rect4.y, 23f, 20f), "▶", false))
			{
				this.session.Step(-1);
			}
			ProfilerGui.Fill(new Rect(rect4.xMax, num + 1f, 1f, 20f), ProfilerGui.Divider);
			if (ProfilerGui.Ghost(new Rect(rect4.xMax + 1f, rect4.y, 29f, 20f), "▶▶", false))
			{
				this.session.JumpToNewest();
			}
			x = rect3.xMax + 8f;
			Window_FrameProfiler.ToolbarDivider(ref x, num);
			Window_FrameProfiler.SourceLabels[0] = "Ring buffer";
			if (this.spikesLabel == null || this.spikesLabelCount != FrameProfiler.SpikeCount)
			{
				this.spikesLabelCount = FrameProfiler.SpikeCount;
				this.spikesLabel = "Spikes " + this.spikesLabelCount.ToString();
			}
			Window_FrameProfiler.SourceLabels[1] = this.spikesLabel;
			int num3 = ProfilerGui.Segments(ref x, num, Window_FrameProfiler.SourceLabels, (this.session.IsSpikes > false) ? 1 : 0, null, 1);
			if (num3 >= 0)
			{
				this.session.SwitchSource((num3 == 1) ? ProfilerFrameSource.Spikes : ProfilerFrameSource.History);
				this.histogram.NotifySourceChanged();
			}
			Window_FrameProfiler.RightRun(Window_FrameProfiler.RightRun(Window_FrameProfiler.RightRun(Window_FrameProfiler.RightRun(Window_FrameProfiler.RightRun(Window_FrameProfiler.RightRun(rect.xMax - 12f, num, this.budgetText, ProfilerGui.TextDim), num, "    budget ", ProfilerGui.TextFaint), num, this.p99Text, ProfilerGui.TextDim), num, "    p99 ", ProfilerGui.TextFaint), num, this.medianText, ProfilerGui.TextDim), num, "median ", ProfilerGui.TextFaint);
		}

		// Token: 0x06003F2A RID: 16170 RVA: 0x00193894 File Offset: 0x00191A94
		private static float RightRun(float right, float y, string text, Color color)
		{
			return ProfilerGui.RunRightAligned(right, y, 22f, text, color);
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x001938A4 File Offset: 0x00191AA4
		private static void ToolbarDivider(ref float x, float y)
		{
			ProfilerGui.Fill(new Rect(x, y + 3f, 1f, 16f), ProfilerGui.Divider);
			x += 9f;
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x001938D4 File Offset: 0x00191AD4
		private void DrawHistogramHeader(Rect rect)
		{
			float num = rect.x;
			int num2 = ProfilerGui.Segments(ref num, rect.y, Window_FrameProfiler.HistogramModeLabels, (this.histogram.AllocMode > false) ? 1 : 0, null, -1);
			if (num2 >= 0)
			{
				this.histogram.SetAllocMode(num2 == 1);
			}
			if (this.session.IsSpikes)
			{
				num = ProfilerGui.Run(num + 10f, rect.y, rect.height, "Saved spike frames", ProfilerGui.TextMuted);
				ProfilerGui.Run(num + 10f, rect.y, rect.height, string.Concat(new string[]
				{
					"trigger from ",
					25f.ToString("0"),
					" ms    from ",
					1.75f.ToString("0.0#"),
					"× avg    keep last ",
					24.ToString()
				}), ProfilerGui.TextFaint);
			}
			if (ProfilerGui.Ghost(new Rect(rect.xMax - 48f, rect.y, 48f, 21f), "Clear", false))
			{
				this.session.ClearFrames();
				this.histogram.Clear();
			}
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x00193A20 File Offset: 0x00191C20
		private void DrawSummary(Rect rect)
		{
			if (!this.session.HasData)
			{
				return;
			}
			ProfilerGui.Run(ProfilerGui.Run(ProfilerGui.Run(ProfilerGui.Run(ProfilerGui.Run(ProfilerGui.Run(rect.x, rect.y, rect.height, this.summaryHead, ProfilerGui.TextMain), rect.y, rect.height, this.summaryTotalLabel, ProfilerGui.TextDim), rect.y, rect.height, this.summaryTotal, (this.summaryTotalSeverity > 0) ? ProfilerGui.Regression : ProfilerGui.TextMain), rect.y, rect.height, "    Δ vs median ", ProfilerGui.TextDim), rect.y, rect.height, this.summaryDelta, (this.summaryDeltaSeverity > 0) ? ProfilerGui.Regression : ((this.summaryDeltaSeverity < 0) ? ProfilerGui.Improvement : ProfilerGui.TextDim)), rect.y, rect.height, this.summaryTail, ProfilerGui.TextDim);
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x00193B28 File Offset: 0x00191D28
		private void DrawTabStrip(Rect rect)
		{
			float num = rect.x;
			int num2 = ProfilerGui.Segments(ref num, rect.y - 0.5f, Window_FrameProfiler.TabLabels, (this.activeSection == this.callTree) ? 0 : ((this.activeSection == this.allocTable) ? 1 : ((this.activeSection == this.vramTable) ? 2 : 3)), null, -1);
			if (num2 >= 0)
			{
				this.SwitchSection((num2 == 0) ? this.callTree : ((num2 == 1) ? this.allocTable : ((num2 == 2) ? this.vramTable : this.ramTable)));
			}
			num += 10f;
			if (this.activeSection == this.callTree)
			{
				int num3 = ProfilerGui.Segments(ref num, rect.y - 0.5f, Window_FrameProfiler.DisplayLabels, (this.callTree.FlamegraphMode > false) ? 1 : 0, null, -1);
				if (num3 >= 0)
				{
					this.callTree.SetFlamegraphMode(num3 == 1);
				}
				num += 10f;
			}
			if (this.activeSection != this.vramTable && this.activeSection != this.ramTable)
			{
				this.threadFilter.DrawButton(ref num, rect.y);
			}
			float num4 = rect.xMax;
			Rect rect2 = new Rect(num4 - 44f, rect.y, 44f, 21f);
			if (ProfilerGui.Ghost(rect2, "Copy", false))
			{
				this.CopyView();
			}
			num4 = rect2.x - 8f;
			Rect rect3 = new Rect(num4 - 24f, rect.y, 24f, 21f);
			Rect rect4 = new Rect(rect3.x - 24f, rect.y, 24f, 21f);
			if (this.activeSection != this.callTree || !this.callTree.FlamegraphMode)
			{
				if (ProfilerGui.Ghost(rect3, "−", false))
				{
					this.activeSection.CollapseAll();
				}
				if (ProfilerGui.Ghost(rect4, "+", false))
				{
					this.activeSection.ExpandAll();
				}
			}
			num4 = rect4.x - 8f;
			if (this.activeSection == this.vramTable)
			{
				bool flag;
				num4 = Window_FrameProfiler.TabButton(num4, rect.y, "Export", false, out flag);
				if (flag)
				{
					this.vramTable.Export();
				}
				bool flag2;
				num4 = Window_FrameProfiler.TabButton(num4, rect.y, "Δ sort", this.vramTable.SortByDelta, out flag2);
				if (flag2)
				{
					this.vramTable.SortByDelta = !this.vramTable.SortByDelta;
				}
				if (this.vramTable.HasBaseline)
				{
					bool flag3;
					num4 = Window_FrameProfiler.TabButton(num4, rect.y, "Clear Δ", false, out flag3);
					if (flag3)
					{
						this.vramTable.ClearBaseline();
					}
				}
				bool flag4;
				num4 = Window_FrameProfiler.TabButton(num4, rect.y, "Baseline", false, out flag4);
				if (flag4)
				{
					this.vramTable.CaptureBaseline();
				}
			}
			else if (this.activeSection == this.ramTable)
			{
				bool flag5;
				num4 = Window_FrameProfiler.TabButton(num4, rect.y, "Export", false, out flag5);
				if (flag5)
				{
					this.ramTable.Export();
				}
			}
			else
			{
				bool active = (this.activeSection == this.allocTable) ? this.allocTable.IsInverted : this.session.Tree.Inverted;
				bool flag6;
				num4 = Window_FrameProfiler.TabButton(num4, rect.y, "↑↓ Invert", active, out flag6);
				if (flag6 && this.activeSection == this.allocTable)
				{
					this.allocTable.IsInverted = !this.allocTable.IsInverted;
				}
				else if (flag6)
				{
					this.session.Tree.Inverted = !this.session.Tree.Inverted;
					this.callTree.ResetSelection();
					this.session.Invalidate();
				}
			}
			Rect rect5 = new Rect(num4 - 180f, rect.y, 180f, 21f);
			ProfilerGui.Bordered(rect5, ProfilerGui.FieldBackground, ProfilerGui.Divider);
			Text.Font = GameFont.Tiny;
			GUI.SetNextControlName("FrameProfilerSearch");
			string text = Widgets.TextField(rect5.ContractedBy(1f), this.searchQuery);
			Text.Font = GameFont.Small;
			if (text != this.searchQuery)
			{
				this.searchQuery = text;
				this.session.Search.Query = text;
				this.callTree.Notify_SearchChanged();
				this.allocTable.Notify_SearchChanged();
				this.vramTable.Notify_SearchChanged();
				this.ramTable.Notify_SearchChanged();
			}
			if (ProfilerGui.Painting)
			{
				this.searchFocused = GUI.GetNameOfFocusedControl().StartsWith("FrameProfiler");
			}
			if (this.searchQuery.Length == 0 && GUI.GetNameOfFocusedControl() != "FrameProfilerSearch")
			{
				ProfilerGui.Label(new Rect(rect5.x + 8f, rect5.y, rect5.width, rect5.height), "search", ProfilerGui.TextFaint, TextAnchor.MiddleLeft, GameFont.Tiny);
			}
			num4 = rect5.x - 6f;
			string searchStatus = this.activeSection.View.SearchStatus;
			if (searchStatus.Length > 0)
			{
				float num5 = ProfilerGui.Width(searchStatus, GameFont.Tiny);
				ProfilerGui.Label(new Rect(num4 - num5 - 4f, rect.y, num5 + 4f, 21f), searchStatus, ProfilerGui.TextDim, TextAnchor.MiddleLeft, GameFont.Tiny);
			}
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x00194090 File Offset: 0x00192290
		private static float TabButton(float right, float y, string label, bool active, out bool clicked)
		{
			float num = ProfilerGui.Width(label, GameFont.Tiny) + 18f;
			Rect rect = new Rect(right - num, y, num, 21f);
			clicked = ProfilerGui.Chip(rect, label, active, true, null);
			return rect.x - 8f;
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x001940E0 File Offset: 0x001922E0
		private void DrawStatusBar(Rect rect)
		{
			ProfilerGui.Fill(new Rect(rect.x, rect.y, rect.width, 1f), ProfilerGui.Divider);
			ProfilerGui.Label(new Rect(rect.x + 12f, rect.y + 1f, rect.width - 24f - 16f, rect.height - 1f), this.statusText, ProfilerGui.TextFaint, TextAnchor.MiddleLeft, GameFont.Tiny);
			Color color = (ProfilerGui.Hovered(new Rect(rect.xMax - 16f, rect.yMax - 16f, 16f, 16f)) || this.resizingWindow) ? ProfilerGui.TextDim : ProfilerGui.TextFaint;
			for (int i = 0; i < 3; i++)
			{
				ProfilerGui.Fill(new Rect(rect.xMax - 6f - (float)i * 4f, rect.yMax - 6f, 2f, 2f), color);
			}
			for (int j = 0; j < 2; j++)
			{
				ProfilerGui.Fill(new Rect(rect.xMax - 6f - (float)j * 4f, rect.yMax - 10f, 2f, 2f), color);
			}
			ProfilerGui.Fill(new Rect(rect.xMax - 6f, rect.yMax - 14f, 2f, 2f), color);
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x00194264 File Offset: 0x00192464
		private void HandleWindowDrag(Rect inRect)
		{
			Event current = Event.current;
			if (current.type == EventType.mouseDown && current.button == 0)
			{
				Rect rect = new Rect(inRect.xMax - 16f, inRect.yMax - 16f, 16f, 16f);
				Rect rect2 = new Rect(inRect.x, inRect.y, inRect.width - 200f, 30f);
				if (rect.Contains(current.mousePosition))
				{
					this.resizingWindow = true;
					this.dragGrab = this.windowRect.max - UI.MousePositionOnUIInverted;
					current.Use();
				}
				else if (rect2.Contains(current.mousePosition))
				{
					this.movingWindow = true;
					this.dragGrab = UI.MousePositionOnUIInverted - this.windowRect.position;
					current.Use();
				}
			}
			if (current.type != EventType.repaint || (!this.movingWindow && !this.resizingWindow))
			{
				return;
			}
			if (!Input.GetMouseButton(0))
			{
				this.movingWindow = false;
				this.resizingWindow = false;
				return;
			}
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			if (this.movingWindow)
			{
				this.windowRect.position = mousePositionOnUIInverted - this.dragGrab;
			}
			else
			{
				Vector2 vector = mousePositionOnUIInverted + this.dragGrab;
				this.windowRect.width = Mathf.Max(Window_FrameProfiler.MinSize.x, vector.x - this.windowRect.x);
				this.windowRect.height = Mathf.Max(Window_FrameProfiler.MinSize.y, vector.y - this.windowRect.y);
			}
			this.ClampToScreen();
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x00194418 File Offset: 0x00192618
		private void ClampToScreen()
		{
			this.windowRect.width = Mathf.Min(this.windowRect.width, (float)UI.screenWidth);
			this.windowRect.height = Mathf.Min(this.windowRect.height, (float)UI.screenHeight);
			this.windowRect.x = Mathf.Clamp(this.windowRect.x, 0f, (float)UI.screenWidth - this.windowRect.width);
			this.windowRect.y = Mathf.Clamp(this.windowRect.y, 0f, (float)UI.screenHeight - this.windowRect.height);
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x001944CC File Offset: 0x001926CC
		private void HandleKeys()
		{
			Event current = Event.current;
			if (current.type != EventType.keyDown)
			{
				return;
			}
			bool flag = GUI.GetNameOfFocusedControl() == "FrameProfilerSearch";
			if (current.keyCode == KeyCode.F3 || (flag && (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter)))
			{
				this.activeSection.View.SelectNextMatch(current.shift ? -1 : 1);
				current.Use();
				return;
			}
			if (flag || GUI.GetNameOfFocusedControl().StartsWith("FrameProfiler"))
			{
				return;
			}
			bool flag2 = true;
			KeyCode keyCode = current.keyCode;
			if (keyCode <= KeyCode.Return)
			{
				if (keyCode != KeyCode.Backspace)
				{
					if (keyCode != KeyCode.Return)
					{
						goto IL_1E0;
					}
					goto IL_156;
				}
			}
			else
			{
				if (keyCode == KeyCode.Space)
				{
					this.session.SetPaused(!this.session.IsPaused);
					goto IL_1E2;
				}
				if (keyCode != KeyCode.C)
				{
					switch (keyCode)
					{
					case KeyCode.KeypadEnter:
						goto IL_156;
					case KeyCode.KeypadEquals:
					case KeyCode.Insert:
					case KeyCode.End:
						goto IL_1E0;
					case KeyCode.UpArrow:
						this.activeSection.View.MoveSelection(-1);
						this.StartNavRepeat(KeyCode.UpArrow);
						goto IL_1E2;
					case KeyCode.DownArrow:
						this.activeSection.View.MoveSelection(1);
						this.StartNavRepeat(KeyCode.DownArrow);
						goto IL_1E2;
					case KeyCode.RightArrow:
						this.activeSection.View.ActivateSelected(true);
						goto IL_1E2;
					case KeyCode.LeftArrow:
						break;
					case KeyCode.Home:
						this.session.JumpToNewest();
						goto IL_1E2;
					case KeyCode.PageUp:
						this.session.Step(1);
						goto IL_1E2;
					case KeyCode.PageDown:
						this.session.Step(-1);
						goto IL_1E2;
					default:
						goto IL_1E0;
					}
				}
				else
				{
					if (current.control && current.shift)
					{
						this.CopyView();
						goto IL_1E2;
					}
					if (current.control)
					{
						this.activeSection.View.CopySelectedRow();
						goto IL_1E2;
					}
					goto IL_1E0;
				}
			}
			this.activeSection.CollapseSelected();
			goto IL_1E2;
			IL_156:
			this.activeSection.View.ActivateSelected(false);
			goto IL_1E2;
			IL_1E0:
			flag2 = false;
			IL_1E2:
			if (flag2)
			{
				current.Use();
			}
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x001946C4 File Offset: 0x001928C4
		private void StartNavRepeat(KeyCode key)
		{
			this.navKey = key;
			this.navNextTime = Time.realtimeSinceStartup + 0.35f;
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x001946E0 File Offset: 0x001928E0
		private void UpdateNavRepeat()
		{
			if (this.navKey == KeyCode.None)
			{
				return;
			}
			if (!Input.GetKey(this.navKey))
			{
				this.navKey = KeyCode.None;
				return;
			}
			if (Time.realtimeSinceStartup < this.navNextTime)
			{
				return;
			}
			this.navNextTime = Time.realtimeSinceStartup + 0.05f;
			this.activeSection.View.MoveSelection((this.navKey == KeyCode.UpArrow) ? -1 : 1);
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x0019474C File Offset: 0x0019294C
		private void CopyView()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.summaryHead).Append(this.summaryTotalLabel).Append(this.summaryTotal).Append("    ").Append("Δ vs median ").Append(this.summaryDelta).Append(this.summaryTail).AppendLine();
			stringBuilder.Append(this.statusText).Append(" / median ").Append(this.medianText).Append(" / p99 ").Append(this.p99Text).Append(" / budget ").Append(this.budgetText).AppendLine();
			this.activeSection.AppendCopyText(stringBuilder);
			ProfilerClipboard.Put(stringBuilder);
		}

		// Token: 0x04002998 RID: 10648
		private const float TitleBarHeight = 30f;

		// Token: 0x04002999 RID: 10649
		private const float ToolbarHeight = 32f;

		// Token: 0x0400299A RID: 10650
		private const float StatusBarHeight = 24f;

		// Token: 0x0400299B RID: 10651
		private const float TabStripHeight = 33f;

		// Token: 0x0400299C RID: 10652
		private const float SidePad = 12f;

		// Token: 0x0400299D RID: 10653
		private const float ResizeGrip = 16f;

		// Token: 0x0400299E RID: 10654
		private const float NavRepeatDelay = 0.35f;

		// Token: 0x0400299F RID: 10655
		private const float NavRepeatInterval = 0.05f;

		// Token: 0x040029A0 RID: 10656
		private const string SearchControlName = "FrameProfilerSearch";

		// Token: 0x040029A1 RID: 10657
		private const string Gap = "    ";

		// Token: 0x040029A2 RID: 10658
		private static readonly Vector2 MinSize = new Vector2(640f, 420f);

		// Token: 0x040029A3 RID: 10659
		private static readonly string[] SourceLabels = new string[2];

		// Token: 0x040029A4 RID: 10660
		private static readonly string[] HistogramModeLabels = new string[]
		{
			"time",
			"alloc"
		};

		// Token: 0x040029A5 RID: 10661
		private static readonly string[] DisplayLabels = new string[]
		{
			"Call tree",
			"Flamegraph"
		};

		// Token: 0x040029A6 RID: 10662
		private static readonly string[] TabLabels = new string[]
		{
			"CPU",
			"Alloc",
			"VRAM",
			"RAM"
		};

		// Token: 0x040029A7 RID: 10663
		private readonly ProfilerSession session;

		// Token: 0x040029A8 RID: 10664
		private readonly ProfilerHistogramWidget histogram;

		// Token: 0x040029A9 RID: 10665
		private readonly ProfilerCallTreeTable callTree;

		// Token: 0x040029AA RID: 10666
		private readonly ProfilerAllocTable allocTable;

		// Token: 0x040029AB RID: 10667
		private readonly ProfilerVramTable vramTable;

		// Token: 0x040029AC RID: 10668
		private readonly ProfilerRamTable ramTable;

		// Token: 0x040029AD RID: 10669
		private ProfilerRowSection activeSection;

		// Token: 0x040029AE RID: 10670
		private readonly ProfilerFlamechartWidget flamechart;

		// Token: 0x040029AF RID: 10671
		private readonly ProfilerThreadFilter threadFilter;

		// Token: 0x040029B0 RID: 10672
		private readonly ProfilerSettingsDrawer settings;

		// Token: 0x040029B1 RID: 10673
		private readonly List<int> zoneScratch = new List<int>();

		// Token: 0x040029B2 RID: 10674
		private readonly List<ProfilerLabelId> pathScratch = new List<ProfilerLabelId>();

		// Token: 0x040029B3 RID: 10675
		private float flameFraction = 0.45454547f;

		// Token: 0x040029B4 RID: 10676
		private bool draggingSplitter;

		// Token: 0x040029B5 RID: 10677
		private ProfilerRefreshGate summaryGate;

		// Token: 0x040029B6 RID: 10678
		private bool hadData;

		// Token: 0x040029B7 RID: 10679
		private bool movingWindow;

		// Token: 0x040029B8 RID: 10680
		private bool resizingWindow;

		// Token: 0x040029B9 RID: 10681
		private Vector2 dragGrab;

		// Token: 0x040029BA RID: 10682
		private KeyCode navKey;

		// Token: 0x040029BB RID: 10683
		private float navNextTime;

		// Token: 0x040029BC RID: 10684
		private string frameStepperText = "Frame —";

		// Token: 0x040029BD RID: 10685
		private string medianText = "";

		// Token: 0x040029BE RID: 10686
		private string p99Text = "";

		// Token: 0x040029BF RID: 10687
		private string budgetText = "";

		// Token: 0x040029C0 RID: 10688
		private string summaryHead = "";

		// Token: 0x040029C1 RID: 10689
		private string summaryTotalLabel = "";

		// Token: 0x040029C2 RID: 10690
		private string summaryTotal = "";

		// Token: 0x040029C3 RID: 10691
		private string summaryDelta = "";

		// Token: 0x040029C4 RID: 10692
		private string summaryTail = "";

		// Token: 0x040029C5 RID: 10693
		private int summaryTotalSeverity;

		// Token: 0x040029C6 RID: 10694
		private int summaryDeltaSeverity;

		// Token: 0x040029C7 RID: 10695
		private string statusText = "";

		// Token: 0x040029C8 RID: 10696
		private long slowUpdateTimestamp;

		// Token: 0x040029C9 RID: 10697
		private float overheadPercent;

		// Token: 0x040029CA RID: 10698
		private int lastToolbarVersion = -1;

		// Token: 0x040029CB RID: 10699
		private int lastToolbarOrdinal = int.MinValue;

		// Token: 0x040029CC RID: 10700
		private int lastToolbarCaptureCount = -1;

		// Token: 0x040029CD RID: 10701
		private string searchQuery = "";

		// Token: 0x040029CE RID: 10702
		[TupleElementNames(new string[]
		{
			"totalMicroseconds",
			"pathId"
		})]
		private readonly List<ValueTuple<float, int>> allocJumpScratch = new List<ValueTuple<float, int>>();

		// Token: 0x040029CF RID: 10703
		private bool mouseHeldFromWindow;

		// Token: 0x040029D0 RID: 10704
		private bool anyButtonLastFrame;

		// Token: 0x040029D1 RID: 10705
		private bool searchFocused;

		// Token: 0x040029D2 RID: 10706
		private bool unfocusSearchPending;

		// Token: 0x040029D3 RID: 10707
		private string spikesLabel;

		// Token: 0x040029D4 RID: 10708
		private int spikesLabelCount;
	}
}
