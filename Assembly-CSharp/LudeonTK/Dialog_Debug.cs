using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace LudeonTK
{
	// Token: 0x02000040 RID: 64
	public class Dialog_Debug : Window_Dev
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000BC79 File Offset: 0x00009E79
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0000BC7C File Offset: 0x00009E7C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000BC8F File Offset: 0x00009E8F
		public string Filter
		{
			get
			{
				return this.filter;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000BC97 File Offset: 0x00009E97
		private float FilterX
		{
			get
			{
				DebugActionNode debugActionNode = this.currentNode;
				if (((debugActionNode != null) ? debugActionNode.parent : null) == null || !this.currentNode.parent.IsRoot)
				{
					return 130f;
				}
				return 0f;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000BCCA File Offset: 0x00009ECA
		private int HighlightedIndex
		{
			get
			{
				return this.currentTabMenu.HighlightedIndex(this.currentHighlightIndex, this.prioritizedHighlightedIndex);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000BCE4 File Offset: 0x00009EE4
		public Rect? BoundingRectCached
		{
			get
			{
				if (this.boundingRectCachedForFrame != Time.frameCount)
				{
					if (this.boundingRect != null)
					{
						Rect value = this.boundingRect.Value;
						Vector2 vector = this.scrollPosition;
						value.x += vector.x;
						value.y += vector.y;
						this.boundingRectCached = new Rect?(value);
					}
					this.boundingRectCachedForFrame = Time.frameCount;
				}
				return this.boundingRectCached;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000BD63 File Offset: 0x00009F63
		public DebugActionNode CurrentNode
		{
			get
			{
				return this.currentNode;
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000BD6C File Offset: 0x00009F6C
		public Dialog_Debug()
		{
			this.Setup();
			this.SwitchTab(DebugTabMenuDefOf.Actions);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000BDB8 File Offset: 0x00009FB8
		public Dialog_Debug(DebugTabMenuDef def)
		{
			this.Setup();
			this.SwitchTab(def);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000BDF5 File Offset: 0x00009FF5
		public void NewColumn(float columnWidth)
		{
			this.curY = 0f;
			this.curX += columnWidth + 17f;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000BE16 File Offset: 0x0000A016
		protected void NewColumnIfNeeded(float columnWidth, float neededHeight)
		{
			if (this.curY + neededHeight > this.heightPerColumn)
			{
				this.NewColumn(columnWidth);
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000BE2F File Offset: 0x0000A02F
		public Rect GetRect(float height, float columnWidth, float widthPct = 1f)
		{
			this.NewColumnIfNeeded(columnWidth, height);
			Rect result = new Rect(this.curX, this.curY, columnWidth * widthPct, height);
			this.curY += height;
			return result;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000BE5C File Offset: 0x0000A05C
		private void Setup()
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
			this.focusFilter = true;
			this.menuDefsSorted.AddRange(DefDatabase<DebugTabMenuDef>.AllDefs.ToList<DebugTabMenuDef>());
			this.menuDefsSorted.SortBy((DebugTabMenuDef x) => x.displayOrder, (DebugTabMenuDef y) => y.label);
			DebugTabMenu debugTabMenu = this.currentTabMenu;
			if (debugTabMenu == null)
			{
				return;
			}
			debugTabMenu.Recache();
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000BEFC File Offset: 0x0000A0FC
		public void SwitchTab(DebugTabMenuDef def)
		{
			Dialog_Debug.TrySetupNodeGraph();
			this.scrollPosition = Vector2.zero;
			this.currentHighlightIndex = 0;
			this.prioritizedHighlightedIndex = 0;
			this.currentTabMenu = (this.menus.ContainsKey(def) ? this.menus[def] : DebugTabMenu.CreateMenu(def, this, Dialog_Debug.rootNode));
			this.currentTabMenu.Enter(Dialog_Debug.roots[def]);
			SoundDefOf.RowTabSelect.PlayOneShotOnCamera(null);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000BF78 File Offset: 0x0000A178
		public static void TrySetupNodeGraph()
		{
			if (Dialog_Debug.rootNode != null)
			{
				return;
			}
			Dialog_Debug.rootNode = new DebugActionNode("Root", DebugActionType.Action, null, null);
			foreach (DebugTabMenuDef debugTabMenuDef in DefDatabase<DebugTabMenuDef>.AllDefs)
			{
				Dialog_Debug.roots.Add(debugTabMenuDef, DebugTabMenu.CreateMenu(debugTabMenuDef, null, Dialog_Debug.rootNode).InitActions(Dialog_Debug.rootNode));
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000BFF8 File Offset: 0x0000A1F8
		private void DrawTabs(Rect rect)
		{
			float num = Mathf.Min(rect.width / (float)this.menuDefsSorted.Count, 200f);
			for (int i = 0; i < this.menuDefsSorted.Count; i++)
			{
				DebugTabMenuDef debugTabMenuDef = this.menuDefsSorted[i];
				Rect rect2 = new Rect(rect.x + (float)i * num, rect.y, num, rect.height).ContractedBy(1f);
				if (debugTabMenuDef == this.currentTabMenu.def)
				{
					GUI.DrawTexture(rect2, DevGUI.ButtonBackgroundClick);
					Text.Anchor = TextAnchor.MiddleCenter;
					DevGUI.Label(rect2, debugTabMenuDef.LabelCap);
					Text.Anchor = TextAnchor.UpperLeft;
				}
				else if (DevGUI.ButtonText(rect2, debugTabMenuDef.LabelCap, null))
				{
					this.SwitchTab(debugTabMenuDef);
				}
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
		public override void DoWindowContents(Rect inRect)
		{
			if (KeyBindingDefOf.Dev_ChangeSelectedDebugAction.KeyDownEvent)
			{
				int highlightedIndex = this.HighlightedIndex;
				if (highlightedIndex >= 0)
				{
					for (int i = 0; i < this.currentTabMenu.Count; i++)
					{
						int index = (highlightedIndex + i + 1) % this.currentTabMenu.Count;
						if (this.FilterAllows(this.currentTabMenu.LabelAtIndex(index)))
						{
							this.prioritizedHighlightedIndex = index;
							break;
						}
					}
				}
			}
			GUI.SetNextControlName("DebugFilter");
			Text.Font = GameFont.Small;
			Rect rect = new Rect(this.FilterX, 0f, Dialog_Debug.FilterInputSize.x, Dialog_Debug.FilterInputSize.y);
			this.filter = DevGUI.TextField(rect, this.filter);
			Rect rect2 = new Rect(rect.xMax + 10f, rect.y, inRect.width - rect.width - 10f, 32f);
			this.DrawTabs(rect2);
			if ((Event.current.type == EventType.keyDown || Event.current.type == EventType.repaint) && this.focusFilter)
			{
				GUI.FocusControl("DebugFilter");
				this.filter = string.Empty;
				this.focusFilter = false;
			}
			if (Event.current.type == EventType.layout)
			{
				this.totalOptionsHeight = 0f;
			}
			Rect outRect = new Rect(inRect);
			outRect.yMin += 42f;
			int num = (int)(this.InitialSize.x / 200f);
			this.heightPerColumn = Mathf.Max(outRect.height, (this.totalOptionsHeight + 50f * (float)(num - 1)) / (float)num);
			this.curX = 0f;
			this.curY = 0f;
			Rect rect3 = new Rect(0f, 0f, outRect.width - 16f, this.heightPerColumn);
			DevGUI.BeginScrollView(outRect, ref this.scrollPosition, rect3, true);
			DevGUI.BeginGroup(rect3);
			float columnWidth = (rect3.width - 17f * (float)(num - 1)) / (float)num;
			this.currentTabMenu.ListOptions(this.HighlightedIndex, columnWidth);
			DevGUI.EndGroup();
			DevGUI.EndScrollView();
			if (this.currentNode.parent != null && !this.currentNode.parent.IsRoot)
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Small;
				if (DevGUI.ButtonText(new Rect(0f, 0f, 120f, 32f), "Back", null))
				{
					this.currentNode.parent.Enter(this);
				}
				if (!this.currentNode.IsRoot)
				{
					Text.Anchor = TextAnchor.UpperRight;
					Text.Font = GameFont.Tiny;
					DevGUI.Label(new Rect(0f, 0f, outRect.width - 24f - 10f, 32f), this.currentNode.Path.Colorize(ColoredText.SubtleGrayColor));
					Text.Anchor = TextAnchor.UpperLeft;
				}
				Text.Font = font;
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000C3D0 File Offset: 0x0000A5D0
		public override void OnAcceptKeyPressed()
		{
			if (GUI.GetNameOfFocusedControl() == "DebugFilter")
			{
				int highlightedIndex = this.HighlightedIndex;
				this.currentTabMenu.OnAcceptKeyPressed(highlightedIndex);
				Event.current.Use();
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000C40C File Offset: 0x0000A60C
		public override void OnCancelKeyPressed()
		{
			if (this.currentNode.parent != null && !this.currentNode.parent.IsRoot)
			{
				this.currentNode.parent.Enter(this);
				Event.current.Use();
				return;
			}
			base.OnCancelKeyPressed();
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000C45C File Offset: 0x0000A65C
		public static DebugActionNode GetNode(string path)
		{
			Dialog_Debug.TrySetupNodeGraph();
			DebugActionNode debugActionNode = Dialog_Debug.rootNode;
			string[] s = path.Split('\\', StringSplitOptions.None);
			int i;
			Predicate<DebugActionNode> <>9__0;
			int j;
			for (i = 0; i < s.Length; i = j + 1)
			{
				List<DebugActionNode> children = debugActionNode.children;
				Predicate<DebugActionNode> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((DebugActionNode x) => x.label == s[i]));
				}
				DebugActionNode debugActionNode2 = children.FirstOrDefault(predicate);
				if (debugActionNode2 == null)
				{
					return null;
				}
				debugActionNode = debugActionNode2;
				debugActionNode.TrySetupChildren();
				j = i;
			}
			return debugActionNode;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000C4F0 File Offset: 0x0000A6F0
		public void SetCurrentNode(DebugActionNode node)
		{
			this.currentNode = node;
			foreach (DebugActionNode debugActionNode in this.currentNode.children)
			{
				debugActionNode.DirtyLabelCache();
			}
			this.scrollPosition = Vector2.zero;
			this.filter = string.Empty;
			this.currentHighlightIndex = 0;
			this.prioritizedHighlightedIndex = 0;
			DebugTabMenu debugTabMenu = this.currentTabMenu;
			if (debugTabMenu == null)
			{
				return;
			}
			debugTabMenu.Recache();
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000C580 File Offset: 0x0000A780
		public void DrawNode(DebugActionNode node, float columnWidth, bool highlight)
		{
			if (node.settingsField != null)
			{
				this.DoCheckbox(node, columnWidth, highlight);
				return;
			}
			this.DoButton(node, columnWidth, highlight);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000C5A4 File Offset: 0x0000A7A4
		public DebugActionButtonResult ButtonDebugPinnable(string label, float columnWidth, bool highlight, bool pinned)
		{
			Text.Font = GameFont.Tiny;
			this.NewColumnIfNeeded(columnWidth, 22f);
			Rect rect = new Rect(this.curX, this.curY, columnWidth - 22f, 22f);
			DebugActionButtonResult result = DebugActionButtonResult.None;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				result = DevGUI.ButtonDebugPinnable(rect, label, highlight, pinned);
			}
			this.curY += 22f + this.verticalSpacing;
			return result;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000C630 File Offset: 0x0000A830
		public DebugActionButtonResult CheckboxPinnable(string label, float columnWidth, ref bool checkOn, bool highlight, bool pinned)
		{
			Text.Font = GameFont.Tiny;
			this.NewColumnIfNeeded(columnWidth, 22f);
			Rect rect = new Rect(this.curX, this.curY, columnWidth - 22f, 22f);
			DebugActionButtonResult result = DebugActionButtonResult.None;
			if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
			{
				result = DevGUI.CheckboxPinnable(rect, label, ref checkOn, highlight, pinned);
			}
			this.curY += 22f + this.verticalSpacing;
			return result;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000C6C0 File Offset: 0x0000A8C0
		private void DoButton(DebugActionNode node, float columnWidth, bool highlight)
		{
			string labelNow = node.LabelNow;
			if (!this.FilterAllows(labelNow))
			{
				GUI.color = Dialog_Debug.DisallowedColor;
			}
			DebugActionButtonResult debugActionButtonResult = this.ButtonDebugPinnable(labelNow, columnWidth, highlight, Prefs.DebugActionsPalette.Contains(node.Path));
			if (debugActionButtonResult != DebugActionButtonResult.ButtonPressed)
			{
				if (debugActionButtonResult == DebugActionButtonResult.PinPressed)
				{
					Dialog_DevPalette.ToggleAction(node.Path);
				}
			}
			else
			{
				node.Enter(this);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.layout)
			{
				this.totalOptionsHeight += 22f + this.verticalSpacing;
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000C750 File Offset: 0x0000A950
		private void DoCheckbox(DebugActionNode node, float columnWidth, bool highlight)
		{
			string labelNow = node.LabelNow;
			FieldInfo settingsField = node.settingsField;
			bool flag = (bool)settingsField.GetValue(null);
			bool flag2 = flag;
			if (!this.FilterAllows(labelNow))
			{
				GUI.color = Dialog_Debug.DisallowedColor;
			}
			DebugActionButtonResult debugActionButtonResult = this.CheckboxPinnable(labelNow, columnWidth, ref flag, highlight, Prefs.DebugActionsPalette.Contains(node.Path));
			if (debugActionButtonResult != DebugActionButtonResult.ButtonPressed)
			{
				if (debugActionButtonResult == DebugActionButtonResult.PinPressed)
				{
					Dialog_DevPalette.ToggleAction(node.Path);
				}
			}
			else
			{
				node.Enter(this);
			}
			GUI.color = Color.white;
			if (Event.current.type == EventType.layout)
			{
				this.totalOptionsHeight += Text.LineHeight;
			}
			if (flag != flag2)
			{
				settingsField.SetValue(null, flag);
				MethodInfo method = settingsField.DeclaringType.GetMethod(settingsField.Name + "Toggled", BindingFlags.Static | BindingFlags.Public);
				if (method != null)
				{
					method.Invoke(null, null);
				}
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000C838 File Offset: 0x0000AA38
		public void DoLabel(string label, float columnWidth)
		{
			Text.Font = GameFont.Small;
			float num = Text.CalcHeight(label, columnWidth);
			DevGUI.Label(new Rect(this.curX, this.curY, columnWidth, num), label);
			this.curY += num;
			if (Event.current.type == EventType.layout)
			{
				this.totalOptionsHeight += num;
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000C895 File Offset: 0x0000AA95
		public void DoGap(float gapSize = 7f)
		{
			this.curY += gapSize;
			if (Event.current.type == EventType.layout)
			{
				this.totalOptionsHeight += gapSize;
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000C8C0 File Offset: 0x0000AAC0
		public bool FilterAllows(string label)
		{
			return this.filter.NullOrEmpty() || label.NullOrEmpty() || label.IndexOf(this.filter, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000C8EE File Offset: 0x0000AAEE
		public static void ResetStaticData()
		{
			Dialog_Debug.rootNode = null;
			Dialog_Debug.roots.Clear();
		}

		// Token: 0x040000B5 RID: 181
		public static DebugActionNode rootNode;

		// Token: 0x040000B6 RID: 182
		private DebugActionNode currentNode;

		// Token: 0x040000B7 RID: 183
		private Dictionary<DebugTabMenuDef, DebugTabMenu> menus = new Dictionary<DebugTabMenuDef, DebugTabMenu>();

		// Token: 0x040000B8 RID: 184
		private static Dictionary<DebugTabMenuDef, DebugActionNode> roots = new Dictionary<DebugTabMenuDef, DebugActionNode>();

		// Token: 0x040000B9 RID: 185
		private List<DebugTabMenuDef> menuDefsSorted = new List<DebugTabMenuDef>();

		// Token: 0x040000BA RID: 186
		private DebugTabMenu currentTabMenu;

		// Token: 0x040000BB RID: 187
		private float totalOptionsHeight;

		// Token: 0x040000BC RID: 188
		private string filter;

		// Token: 0x040000BD RID: 189
		private bool focusFilter;

		// Token: 0x040000BE RID: 190
		private int currentHighlightIndex;

		// Token: 0x040000BF RID: 191
		private int prioritizedHighlightedIndex;

		// Token: 0x040000C0 RID: 192
		private Vector2 scrollPosition;

		// Token: 0x040000C1 RID: 193
		protected float curY;

		// Token: 0x040000C2 RID: 194
		protected float curX;

		// Token: 0x040000C3 RID: 195
		private int boundingRectCachedForFrame = -1;

		// Token: 0x040000C4 RID: 196
		private Rect? boundingRectCached;

		// Token: 0x040000C5 RID: 197
		private Rect? boundingRect;

		// Token: 0x040000C6 RID: 198
		public float verticalSpacing = 2f;

		// Token: 0x040000C7 RID: 199
		private float heightPerColumn;

		// Token: 0x040000C8 RID: 200
		private const string FilterControlName = "DebugFilter";

		// Token: 0x040000C9 RID: 201
		private const float DebugOptionsGap = 7f;

		// Token: 0x040000CA RID: 202
		private static readonly Color DisallowedColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x040000CB RID: 203
		private static readonly Vector2 FilterInputSize = new Vector2(200f, 30f);

		// Token: 0x040000CC RID: 204
		private const float AssumedBiggestElementHeight = 50f;

		// Token: 0x040000CD RID: 205
		private const float BackButtonWidth = 120f;

		// Token: 0x040000CE RID: 206
		private const float PinnableActionHeight = 22f;

		// Token: 0x040000CF RID: 207
		private const float TabHeight = 32f;

		// Token: 0x040000D0 RID: 208
		private const float MaxTabWidth = 200f;
	}
}
