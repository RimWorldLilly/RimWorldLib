using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000039 RID: 57
	public abstract class DebugTabMenu
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000B344 File Offset: 0x00009544
		public int Count
		{
			get
			{
				return this.VisibleActions.Count;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0000B354 File Offset: 0x00009554
		protected List<DebugActionNode> VisibleActions
		{
			get
			{
				if (this.cachedVisibleActions == null)
				{
					this.cachedVisibleActions = new List<DebugActionNode>();
					List<DebugActionNode> children = this.dialog.CurrentNode.children;
					for (int i = 0; i < children.Count; i++)
					{
						DebugActionNode debugActionNode = children[i];
						if (debugActionNode.VisibleNow)
						{
							this.cachedVisibleActions.Add(debugActionNode);
						}
					}
				}
				return this.cachedVisibleActions;
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000B3B8 File Offset: 0x000095B8
		public DebugTabMenu(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode rootNode)
		{
			this.def = def;
			this.dialog = dialog;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000B3CE File Offset: 0x000095CE
		public void Enter(DebugActionNode root)
		{
			this.myRoot = root;
			this.myRoot.Enter(this.dialog);
		}

		// Token: 0x06000184 RID: 388
		public abstract DebugActionNode InitActions(DebugActionNode root);

		// Token: 0x06000185 RID: 389 RVA: 0x0000B3E8 File Offset: 0x000095E8
		public int HighlightedIndex(int currentHighlightIndex, int prioritizedHighlightedIndex)
		{
			List<DebugActionNode> visibleActions = this.VisibleActions;
			if (visibleActions.NullOrEmpty<DebugActionNode>() || prioritizedHighlightedIndex >= visibleActions.Count)
			{
				return -1;
			}
			if (this.dialog.FilterAllows(visibleActions[prioritizedHighlightedIndex].LabelNow))
			{
				return prioritizedHighlightedIndex;
			}
			if (this.dialog.Filter.NullOrEmpty())
			{
				return 0;
			}
			for (int i = 0; i < visibleActions.Count; i++)
			{
				if (this.dialog.FilterAllows(visibleActions[i].LabelNow))
				{
					currentHighlightIndex = i;
					break;
				}
			}
			return currentHighlightIndex;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000B46E File Offset: 0x0000966E
		public string LabelAtIndex(int index)
		{
			return this.VisibleActions[index].LabelNow;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000B484 File Offset: 0x00009684
		public void ListOptions(int highlightedIndex, float columnWidth)
		{
			string b = null;
			List<DebugActionNode> visibleActions = this.VisibleActions;
			for (int i = 0; i < visibleActions.Count; i++)
			{
				DebugActionNode debugActionNode = visibleActions[i];
				if (this.dialog.FilterAllows(debugActionNode.LabelNow))
				{
					if (debugActionNode.category != b)
					{
						this.dialog.DoGap(7f);
						this.dialog.DoLabel(debugActionNode.category, columnWidth);
						b = debugActionNode.category;
					}
					Log.openOnMessage = true;
					try
					{
						this.dialog.DrawNode(debugActionNode, columnWidth, highlightedIndex == i);
					}
					finally
					{
						Log.openOnMessage = false;
					}
				}
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000B530 File Offset: 0x00009730
		public void Recache()
		{
			this.cachedVisibleActions = null;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000B539 File Offset: 0x00009739
		public void OnAcceptKeyPressed(int index)
		{
			if (index < 0)
			{
				return;
			}
			this.VisibleActions[index].Enter(this.dialog);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000B557 File Offset: 0x00009757
		public static DebugTabMenu CreateMenu(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode root)
		{
			return (DebugTabMenu)Activator.CreateInstance(def.menuClass, new object[]
			{
				def,
				dialog,
				root
			});
		}

		// Token: 0x040000A7 RID: 167
		protected DebugActionNode myRoot;

		// Token: 0x040000A8 RID: 168
		public readonly DebugTabMenuDef def;

		// Token: 0x040000A9 RID: 169
		protected readonly Dialog_Debug dialog;

		// Token: 0x040000AA RID: 170
		private List<DebugActionNode> cachedVisibleActions;
	}
}
