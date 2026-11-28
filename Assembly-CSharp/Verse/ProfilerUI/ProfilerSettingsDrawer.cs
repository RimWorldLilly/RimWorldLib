using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Waylith.Profiling;
using Waylith.Profiling.Instrumentation;

namespace Verse.ProfilerUI
{
	// Token: 0x020007A0 RID: 1952
	public sealed class ProfilerSettingsDrawer
	{
		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06003E72 RID: 15986 RVA: 0x0018CDFE File Offset: 0x0018AFFE
		// (set) Token: 0x06003E73 RID: 15987 RVA: 0x0018CE06 File Offset: 0x0018B006
		public bool IsOpen { get; private set; }

		// Token: 0x06003E74 RID: 15988 RVA: 0x0018CE0F File Offset: 0x0018B00F
		public ProfilerSettingsDrawer(ProfilerSession session)
		{
			this.session = session;
		}

		// Token: 0x06003E75 RID: 15989 RVA: 0x0018CE3C File Offset: 0x0018B03C
		public void Toggle()
		{
			this.IsOpen = !this.IsOpen;
			if (this.IsOpen)
			{
				this.LoadDrafts();
			}
		}

		// Token: 0x06003E76 RID: 15990 RVA: 0x0018CE5C File Offset: 0x0018B05C
		private void LoadDrafts()
		{
			this.capacityDraft = FrameProfiler.HistoryCapacity.ToString();
			this.fpsDraft = (1000f / this.session.BudgetMs).ToString("0", CultureInfo.InvariantCulture);
			this.targetsDraft = string.Join("\n", ProfilerPrefs.Current.instrumentationTargets);
			this.RefreshTargetMatches();
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x0018CEC8 File Offset: 0x0018B0C8
		public void MaskInput(Rect windowRect)
		{
			this.masked = false;
			if (!this.IsOpen)
			{
				return;
			}
			this.panelRect = ProfilerSettingsDrawer.PanelRect(windowRect);
			ProfilerGui.AddOverlay(this.panelRect);
			Event current = Event.current;
			if ((current.type == EventType.mouseDown || current.type == EventType.scrollWheel) && this.panelRect.Contains(current.mousePosition))
			{
				this.maskedType = current.type;
				current.type = EventType.ignore;
				this.masked = true;
			}
		}

		// Token: 0x06003E78 RID: 15992 RVA: 0x0018CF41 File Offset: 0x0018B141
		private static Rect PanelRect(Rect windowRect)
		{
			return new Rect(windowRect.xMax - 400f - 1f, windowRect.y + 31f, 400f, windowRect.height - 32f);
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x0018CF7C File Offset: 0x0018B17C
		public void Draw(Rect windowRect)
		{
			if (!this.IsOpen)
			{
				return;
			}
			Event current = Event.current;
			if (this.masked && current.type == EventType.ignore)
			{
				current.type = this.maskedType;
			}
			this.masked = false;
			ProfilerGui.ClearOverlays();
			this.panelRect = ProfilerSettingsDrawer.PanelRect(windowRect);
			if (this.panelRect.Contains(current.mousePosition))
			{
				ProfilerGui.ClearTooltip();
			}
			ProfilerGui.Fill(this.panelRect, ProfilerGui.TitleBar);
			ProfilerGui.Fill(new Rect(this.panelRect.x, this.panelRect.y, 1f, this.panelRect.height), ProfilerGui.FocusBorder);
			this.DrawHeader(new Rect(this.panelRect.x + 1f, this.panelRect.y, this.panelRect.width - 1f, 38f));
			Rect position = new Rect(this.panelRect.x + 1f, this.panelRect.y + 38f, this.panelRect.width - 1f, this.panelRect.height - 38f);
			float max = Mathf.Max(0f, this.contentHeight - position.height);
			if (current.type == EventType.scrollWheel && position.Contains(current.mousePosition))
			{
				this.scrollY += current.delta.y / 3f * 40f;
				current.Use();
			}
			this.scrollY = Mathf.Clamp(this.scrollY, 0f, max);
			this.bodyHovered = position.Contains(current.mousePosition);
			GUI.BeginGroup(position);
			try
			{
				this.contentHeight = this.DrawBody(position.width, -this.scrollY) + this.scrollY;
			}
			finally
			{
				GUI.EndGroup();
			}
			if (current.type == EventType.mouseDown && this.panelRect.Contains(current.mousePosition))
			{
				current.Use();
			}
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x0018D198 File Offset: 0x0018B398
		private void DrawHeader(Rect rect)
		{
			ProfilerGui.Label(new Rect(rect.x + 14f, rect.y, 80f, rect.height), "Settings", ProfilerGui.TextMain, TextAnchor.MiddleLeft, GameFont.Tiny);
			Rect rect2 = new Rect(rect.xMax - 14f - 48f, rect.y + 8f, 48f, 22f);
			if (ProfilerGui.Chip(rect2, "Done", false, true, null))
			{
				this.IsOpen = false;
			}
			float num = ProfilerGui.Width("Reset to defaults", GameFont.Tiny) + 18f;
			if (ProfilerGui.Ghost(new Rect(rect2.x - num - 6f, rect2.y, num, 22f), "Reset to defaults", false))
			{
				ProfilerPrefs.Reset();
				FrameProfiler.ClearMutedLabels();
				this.OnFoldRecursionChanged();
				this.LoadDrafts();
			}
			ProfilerGui.Fill(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), ProfilerGui.Divider);
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x0018D2B0 File Offset: 0x0018B4B0
		private float DrawBody(float w, float y)
		{
			ProfilerPrefsData prefs = ProfilerPrefs.Current;
			ProfilerSettingsDrawer.Section(ref y, w, "CAPTURE");
			if (this.Checkbox(ref y, w, "Start on boot", prefs.startOnBoot, true))
			{
				ProfilerSettingsDrawer.Change(delegate
				{
					prefs.startOnBoot = !prefs.startOnBoot;
				});
			}
			if (this.Checkbox(ref y, w, "Keep running when closed", prefs.keepRunningWhenClosed, true))
			{
				ProfilerSettingsDrawer.Change(delegate
				{
					prefs.keepRunningWhenClosed = !prefs.keepRunningWhenClosed;
				});
			}
			int frames;
			if (ProfilerSettingsDrawer.Field(ref y, w, "Ring buffer", "frames", "Capacity", ref this.capacityDraft, FrameProfiler.HistoryCapacity.ToString()) && int.TryParse(this.capacityDraft.Replace(" ", "").Replace(",", ""), out frames))
			{
				ProfilerSettingsDrawer.Change(delegate
				{
					prefs.historyCapacity = frames;
				});
				this.capacityDraft = FrameProfiler.HistoryCapacity.ToString();
			}
			if (this.Checkbox(ref y, w, "Allocation tracking", prefs.allocTracking, FrameProfiler.AllocationTrackingSupported))
			{
				ProfilerSettingsDrawer.Change(delegate
				{
					prefs.allocTracking = !prefs.allocTracking;
				});
			}
			if (this.Checkbox(ref y, w, "Main thread only", prefs.mainThreadOnly, true))
			{
				ProfilerSettingsDrawer.Change(delegate
				{
					prefs.mainThreadOnly = !prefs.mainThreadOnly;
				});
			}
			if (this.Checkbox(ref y, w, "Ignore throttle", prefs.ignoreThrottle, true))
			{
				ProfilerSettingsDrawer.Change(delegate
				{
					prefs.ignoreThrottle = !prefs.ignoreThrottle;
				});
			}
			ProfilerSettingsDrawer.Section(ref y, w, "INSTRUMENTATION");
			bool isSupported = ProfilerInstrumentation.IsSupported;
			if (this.Checkbox(ref y, w, "Enabled", prefs.instrumentation, isSupported))
			{
				ProfilerSettingsDrawer.Change(delegate
				{
					prefs.instrumentation = !prefs.instrumentation;
				});
			}
			this.DrawTargets(ref y, w, isSupported);
			if (this.Checkbox(ref y, w, "Auto-mute trivial scopes", prefs.autoMuteTrivial, true))
			{
				ProfilerSettingsDrawer.Change(delegate
				{
					prefs.autoMuteTrivial = !prefs.autoMuteTrivial;
				});
			}
			ProfilerSettingsDrawer.Section(ref y, w, "TIMELINE");
			float fps;
			if (ProfilerSettingsDrawer.Field(ref y, w, "FPS target", "fps", "Fps", ref this.fpsDraft, (1000f / this.session.BudgetMs).ToString("0", CultureInfo.InvariantCulture)) && float.TryParse(this.fpsDraft.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out fps) && fps > 0f)
			{
				ProfilerSettingsDrawer.Change(delegate
				{
					prefs.budgetMs = 1000f / fps;
				});
				this.fpsDraft = (1000f / this.session.BudgetMs).ToString("0", CultureInfo.InvariantCulture);
			}
			ProfilerSettingsDrawer.Section(ref y, w, "CALL TREE");
			if (this.Checkbox(ref y, w, "Fold recursion", prefs.foldRecursion, true))
			{
				ProfilerSettingsDrawer.Change(delegate
				{
					prefs.foldRecursion = !prefs.foldRecursion;
				});
				this.OnFoldRecursionChanged();
			}
			return y + 14f;
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x0018D5BB File Offset: 0x0018B7BB
		private void OnFoldRecursionChanged()
		{
			this.session.Tree.FoldRecursion = ProfilerPrefs.Current.foldRecursion;
			this.session.Invalidate();
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x0018D5E2 File Offset: 0x0018B7E2
		private static void Change(Action change)
		{
			change();
			ProfilerPrefs.Apply();
			ProfilerPrefs.Save();
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x0018D5F4 File Offset: 0x0018B7F4
		private static string InstrumentationStats()
		{
			if (!ProfilerInstrumentation.IsEnabled)
			{
				return "not instrumented";
			}
			int mutedMethodCount = ProfilerInstrumentation.MutedMethodCount;
			int num = ProfilerInstrumentation.MatchedMethodCount - ProfilerInstrumentation.InstrumentedMethodCount - mutedMethodCount;
			return string.Concat(new string[]
			{
				ProfilerInstrumentation.InstrumentedMethodCount.ToString("n0"),
				" instrumented / ",
				(mutedMethodCount == 0) ? "" : (mutedMethodCount.ToString("n0") + " muted / "),
				num.ToString("n0"),
				" skipped as trivial"
			});
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x0018D684 File Offset: 0x0018B884
		private static void Section(ref float y, float w, string title)
		{
			ProfilerGui.Label(new Rect(14f, y + 8f, 160f, 20f), title, ProfilerGui.TextFaint, TextAnchor.MiddleLeft, GameFont.Tiny);
			ProfilerGui.Fill(new Rect(14f, y + 28f - 1f, w - 28f, 1f), ProfilerGui.Divider);
			y += 28f;
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x0018D6F2 File Offset: 0x0018B8F2
		private static void RowLabel(float y, float w, string label, bool enabled, float controlWidth)
		{
			ProfilerGui.Label(new Rect(14f, y, w - 28f - controlWidth, 30f), label, enabled ? ProfilerGui.TextMain : ProfilerGui.TextDisabled, TextAnchor.MiddleLeft, GameFont.Tiny);
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x0018D728 File Offset: 0x0018B928
		private bool Checkbox(ref float y, float w, string label, bool value, bool enabled = true)
		{
			ProfilerSettingsDrawer.RowLabel(y, w, label, enabled, 40f);
			bool flag = value;
			if (enabled && this.bodyHovered)
			{
				Widgets.ToggleInvisibleDraggable(new Rect(0f, y, w, 30f), ref flag, true, false);
			}
			Widgets.CheckboxDraw(w - 14f - 24f, y + 3f, flag, !enabled, 24f, null, null);
			y += 30f;
			return flag != value;
		}

		// Token: 0x06003E82 RID: 16002 RVA: 0x0018D7A8 File Offset: 0x0018B9A8
		private static bool Field(ref float y, float w, string label, string unit, string controlName, ref string draft, string applied)
		{
			ProfilerSettingsDrawer.RowLabel(y, w, label, true, 150f);
			float num = ProfilerGui.Width(unit, GameFont.Tiny) + 6f;
			Rect rect = new Rect(w - 14f - num - 64f, y + 4f, 64f, 22f);
			ProfilerGui.Label(new Rect(rect.xMax + 6f, rect.y, num, rect.height), unit, ProfilerGui.TextFaint, TextAnchor.MiddleLeft, GameFont.Tiny);
			string text = "FrameProfiler" + controlName;
			Event current = Event.current;
			bool flag = current.type == EventType.keyDown && (current.keyCode == KeyCode.Return || current.keyCode == KeyCode.KeypadEnter) && GUI.GetNameOfFocusedControl() == text;
			ProfilerGui.Bordered(rect, ProfilerGui.FieldBackground, ProfilerGui.Divider);
			Text.Font = GameFont.Tiny;
			GUI.SetNextControlName(text);
			draft = Widgets.TextField(rect.ContractedBy(1f), draft ?? applied);
			Text.Font = GameFont.Small;
			bool result;
			bool flag2 = result = flag;
			if (draft != applied && ProfilerGui.Chip(new Rect(rect.x - 50f, rect.y, 44f, 22f), "Apply", false, true, null))
			{
				result = true;
			}
			if (flag2)
			{
				current.Use();
			}
			y += 30f;
			return result;
		}

		// Token: 0x06003E83 RID: 16003 RVA: 0x0018D908 File Offset: 0x0018BB08
		private void DrawTargets(ref float y, float w, bool enabled)
		{
			ProfilerGui.Label(new Rect(14f, y + 4f, w - 28f, 18f), "Filters", enabled ? ProfilerGui.TextMain : ProfilerGui.TextDisabled, TextAnchor.MiddleLeft, GameFont.Tiny);
			if (ProfilerGui.Chip(new Rect(w - 14f - 82f, y + 2f, 82f, 20f), "Assemblies...", false, enabled, null))
			{
				Find.WindowStack.TryRemove(typeof(Dialog_ProfilerAssemblies), true);
				Find.WindowStack.Add(new Dialog_ProfilerAssemblies(() => this.targetsDraft ?? "", delegate(string draft)
				{
					this.targetsDraft = draft;
					this.RefreshTargetMatches();
				}));
			}
			ProfilerGui.Label(new Rect(14f, y + 20f, w - 28f, 18f), "Assembly!Type.Full.Name::Method. One per line. Supports * and ? wildcards.", ProfilerGui.TextFaint, TextAnchor.MiddleLeft, GameFont.Tiny);
			y += 40f;
			Rect rect = new Rect(14f, y, w - 28f, 96f);
			ProfilerGui.Bordered(rect, ProfilerGui.FieldBackground, ProfilerGui.Divider);
			Text.Font = GameFont.Tiny;
			GUI.SetNextControlName("FrameProfilerTargets");
			string a = Widgets.TextArea(rect.ContractedBy(1f), this.targetsDraft ?? "", !enabled);
			Text.Font = GameFont.Small;
			if (a != this.targetsDraft)
			{
				this.targetsDraft = a;
				this.RefreshTargetMatches();
			}
			y += 100f;
			bool flag = this.targetsDirty && enabled;
			float num = w - 28f - (flag ? 60f : 0f);
			string text = ProfilerSettingsDrawer.InstrumentationStats();
			string text2 = this.targetsMatchText + " / " + text;
			bool flag2 = ProfilerGui.Width(text2, GameFont.Tiny) > num;
			ProfilerGui.Label(new Rect(14f, y, num, 20f), flag2 ? this.targetsMatchText : text2, ProfilerGui.TextFaint, TextAnchor.MiddleLeft, GameFont.Tiny);
			if (flag && ProfilerGui.Chip(new Rect(w - 14f - 52f, y, 52f, 20f), "Apply", false, true, null))
			{
				List<string> targets = ProfilerSettingsDrawer.SplitTargets(this.targetsDraft);
				ProfilerSettingsDrawer.Change(delegate
				{
					ProfilerPrefs.Current.instrumentationTargets = targets;
				});
				this.RefreshTargetMatches();
			}
			y += 22f;
			if (flag2)
			{
				ProfilerGui.Label(new Rect(14f, y - 4f, w - 28f, 20f), text, ProfilerGui.TextFaint, TextAnchor.MiddleLeft, GameFont.Tiny);
				y += 18f;
			}
			if (this.targetsUnmatchedText.Length > 0)
			{
				ProfilerGui.Label(new Rect(14f, y, w - 28f, 18f), this.targetsUnmatchedText, ProfilerGui.Regression, TextAnchor.MiddleLeft, GameFont.Tiny);
				y += 20f;
			}
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x0018DBE4 File Offset: 0x0018BDE4
		private static List<string> SplitTargets(string text)
		{
			List<string> list = new List<string>();
			string[] array = text.Split(ProfilerSettingsDrawer.TargetSeparators, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i].Trim();
				if (text2.Length > 0)
				{
					list.Add(text2);
				}
			}
			return list;
		}

		// Token: 0x06003E85 RID: 16005 RVA: 0x0018DC2C File Offset: 0x0018BE2C
		private void RefreshTargetMatches()
		{
			List<string> list = ProfilerSettingsDrawer.SplitTargets(this.targetsDraft ?? "");
			int num;
			int[] array = ProfilerInstrumentation.CountMatches(list, out num);
			List<string> list2 = new List<string>();
			for (int i = 0; i < list.Count; i++)
			{
				if (array[i] == 0)
				{
					list2.Add(list[i]);
				}
			}
			this.targetsMatchText = num.ToString("n0") + " methods match";
			this.targetsUnmatchedText = ((list2.Count == 0) ? "" : ("no match: " + string.Join(", ", list2)));
			IReadOnlyList<string> targets = ProfilerInstrumentation.Targets;
			this.targetsDirty = (list.Count != targets.Count);
			int num2 = 0;
			while (num2 < list.Count && !this.targetsDirty)
			{
				this.targetsDirty = (list[num2] != targets[num2]);
				num2++;
			}
		}

		// Token: 0x0400291E RID: 10526
		public const string ControlPrefix = "FrameProfiler";

		// Token: 0x0400291F RID: 10527
		private const float Width = 400f;

		// Token: 0x04002920 RID: 10528
		private const float HeaderHeight = 38f;

		// Token: 0x04002921 RID: 10529
		private const float RowHeight = 30f;

		// Token: 0x04002922 RID: 10530
		private const float SectionHeight = 28f;

		// Token: 0x04002923 RID: 10531
		private const float Pad = 14f;

		// Token: 0x04002924 RID: 10532
		private const float TargetsHeight = 96f;

		// Token: 0x04002925 RID: 10533
		private const float ScrollUnitsPerNotch = 3f;

		// Token: 0x04002926 RID: 10534
		private static readonly char[] TargetSeparators = new char[]
		{
			'\n',
			'\r'
		};

		// Token: 0x04002927 RID: 10535
		private readonly ProfilerSession session;

		// Token: 0x04002928 RID: 10536
		private string capacityDraft;

		// Token: 0x04002929 RID: 10537
		private string fpsDraft;

		// Token: 0x0400292A RID: 10538
		private string targetsDraft;

		// Token: 0x0400292B RID: 10539
		private string targetsMatchText = "";

		// Token: 0x0400292C RID: 10540
		private string targetsUnmatchedText = "";

		// Token: 0x0400292D RID: 10541
		private bool targetsDirty;

		// Token: 0x0400292E RID: 10542
		private float scrollY;

		// Token: 0x0400292F RID: 10543
		private float contentHeight;

		// Token: 0x04002930 RID: 10544
		private Rect panelRect;

		// Token: 0x04002931 RID: 10545
		private EventType maskedType = EventType.ignore;

		// Token: 0x04002932 RID: 10546
		private bool masked;

		// Token: 0x04002933 RID: 10547
		private bool bodyHovered;
	}
}
