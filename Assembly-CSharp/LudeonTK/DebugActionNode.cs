using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000036 RID: 54
	public class DebugActionNode
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000169 RID: 361 RVA: 0x0000ABEE File Offset: 0x00008DEE
		public bool IsRoot
		{
			get
			{
				return this.parent == null;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600016A RID: 362 RVA: 0x0000ABFC File Offset: 0x00008DFC
		public bool On
		{
			get
			{
				if (this.cachedCheckOn == null)
				{
					if (this.settingsField == null)
					{
						this.cachedCheckOn = new bool?(false);
					}
					else
					{
						this.cachedCheckOn = new bool?((bool)this.settingsField.GetValue(null));
					}
				}
				return this.cachedCheckOn.Value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000AC5C File Offset: 0x00008E5C
		public string LabelNow
		{
			get
			{
				if (Time.frameCount >= this.lastLabelCacheFrame + 30)
				{
					this.DirtyLabelCache();
				}
				if (this.cachedLabelNow == null)
				{
					if (this.labelGetter != null)
					{
						try
						{
							this.cachedLabelNow = this.labelGetter();
							goto IL_72;
						}
						catch (Exception ex)
						{
							string str = "Exception getting label for DebugActionNode: ";
							Exception ex2 = ex;
							Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
							this.cachedLabelNow = null;
							return this.label;
						}
					}
					this.cachedLabelNow = this.label;
				}
				IL_72:
				return this.cachedLabelNow;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600016C RID: 364 RVA: 0x0000ACF4 File Offset: 0x00008EF4
		public bool ActiveNow
		{
			get
			{
				DebugActionType debugActionType = this.actionType;
				if (debugActionType - DebugActionType.ToolMap > 1)
				{
					return debugActionType != DebugActionType.ToolWorld || WorldRendererUtility.WorldSelected;
				}
				return WorldRendererUtility.DrawingMap;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000AD1F File Offset: 0x00008F1F
		public bool VisibleNow
		{
			get
			{
				return (this.sourceAttribute == null || this.sourceAttribute.IsAllowedInCurrentGameState) && (this.visibilityGetter == null || this.visibilityGetter());
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600016E RID: 366 RVA: 0x0000AD50 File Offset: 0x00008F50
		public string Path
		{
			get
			{
				if (this.cachedPath == null)
				{
					if (this.parent != null && !this.parent.IsRoot)
					{
						this.cachedPath = this.parent.Path + "\\" + this.label;
					}
					else
					{
						this.cachedPath = this.label;
					}
				}
				return this.cachedPath;
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000ADAF File Offset: 0x00008FAF
		public DebugActionNode()
		{
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000ADC9 File Offset: 0x00008FC9
		public DebugActionNode(string label, DebugActionType actionType = DebugActionType.Action, Action action = null, Action<Pawn> pawnAction = null)
		{
			this.label = label;
			this.actionType = actionType;
			this.action = action;
			this.pawnAction = pawnAction;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000AE00 File Offset: 0x00009000
		public void AddChild(DebugActionNode child)
		{
			child.SetParent(this);
			this.children.Add(child);
			this.sorted = false;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000AE1C File Offset: 0x0000901C
		public void SetParent(DebugActionNode newParent)
		{
			this.parent = newParent;
			this.DirtyPath();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000AE2C File Offset: 0x0000902C
		private void DirtyPath()
		{
			this.cachedPath = null;
			foreach (DebugActionNode debugActionNode in this.children)
			{
				debugActionNode.DirtyPath();
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000AE84 File Offset: 0x00009084
		public void DirtyLabelCache()
		{
			this.cachedLabelNow = null;
			this.cachedCheckOn = null;
			this.lastLabelCacheFrame = Time.frameCount;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000AEA4 File Offset: 0x000090A4
		public void TrySort()
		{
			if (this.sorted)
			{
				return;
			}
			if (!this.children.NullOrEmpty<DebugActionNode>())
			{
				this.children = (from r in this.children
				orderby DebugActionCategories.GetOrderFor(r.category), r.category, -r.displayPriority
				select r).ToList<DebugActionNode>();
			}
			this.sorted = true;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000AF4C File Offset: 0x0000914C
		public void TrySetupChildren()
		{
			if (!this.childrenSetup && this.childGetter != null)
			{
				foreach (DebugActionNode child in this.childGetter())
				{
					this.AddChild(child);
				}
			}
			this.childrenSetup = true;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000AFBC File Offset: 0x000091BC
		private static IEnumerable<Pawn> PawnsInside(IThingHolder holder)
		{
			DebugActionNode.<PawnsInside>d__40 <PawnsInside>d__ = new DebugActionNode.<PawnsInside>d__40(-2);
			<PawnsInside>d__.<>3__holder = holder;
			return <PawnsInside>d__;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000AFCC File Offset: 0x000091CC
		private static IEnumerable<Pawn> PawnsAt(IntVec3 cell, Map map)
		{
			DebugActionNode.<PawnsAt>d__41 <PawnsAt>d__ = new DebugActionNode.<PawnsAt>d__41(-2);
			<PawnsAt>d__.<>3__cell = cell;
			<PawnsAt>d__.<>3__map = map;
			return <PawnsAt>d__;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000AFE4 File Offset: 0x000091E4
		public void Enter(Dialog_Debug dialog)
		{
			this.TrySetupChildren();
			if (this.children.Any<DebugActionNode>())
			{
				this.TrySort();
				if (dialog == null)
				{
					dialog = new Dialog_Debug();
					Find.WindowStack.Add(dialog);
				}
				dialog.SetCurrentNode(this);
				return;
			}
			if (dialog != null)
			{
				dialog.Close(true);
			}
			switch (this.actionType)
			{
			case DebugActionType.Action:
				this.action();
				break;
			case DebugActionType.ToolMap:
			case DebugActionType.ToolWorld:
				DebugTools.curTool = new DebugTool(this.LabelNow, this.action, null);
				break;
			case DebugActionType.ToolMapForPawns:
			{
				DebugActionNode instance = this;
				DebugTools.curTool = new DebugTool(this.LabelNow, delegate()
				{
					if (UI.MouseCell().InBounds(Find.CurrentMap))
					{
						foreach (Pawn obj in DebugActionNode.PawnsAt(UI.MouseCell(), Find.CurrentMap).ToList<Pawn>())
						{
							instance.pawnAction(obj);
						}
					}
				}, null);
				break;
			}
			}
			this.DirtyLabelCache();
		}

		// Token: 0x04000091 RID: 145
		public string label;

		// Token: 0x04000092 RID: 146
		public DebugActionType actionType;

		// Token: 0x04000093 RID: 147
		public Action action;

		// Token: 0x04000094 RID: 148
		public Action<Pawn> pawnAction;

		// Token: 0x04000095 RID: 149
		public Func<List<DebugActionNode>> childGetter;

		// Token: 0x04000096 RID: 150
		public DebugActionAttribute sourceAttribute;

		// Token: 0x04000097 RID: 151
		public Func<bool> visibilityGetter;

		// Token: 0x04000098 RID: 152
		public Func<string> labelGetter;

		// Token: 0x04000099 RID: 153
		public string category;

		// Token: 0x0400009A RID: 154
		public int displayPriority;

		// Token: 0x0400009B RID: 155
		public FieldInfo settingsField;

		// Token: 0x0400009C RID: 156
		private bool childrenSetup;

		// Token: 0x0400009D RID: 157
		public DebugActionNode parent;

		// Token: 0x0400009E RID: 158
		public List<DebugActionNode> children = new List<DebugActionNode>();

		// Token: 0x0400009F RID: 159
		private string cachedPath;

		// Token: 0x040000A0 RID: 160
		private string cachedLabelNow;

		// Token: 0x040000A1 RID: 161
		private bool sorted;

		// Token: 0x040000A2 RID: 162
		private bool? cachedCheckOn;

		// Token: 0x040000A3 RID: 163
		private int lastLabelCacheFrame = -1;

		// Token: 0x040000A4 RID: 164
		private const int LabelRecacheFrameCount = 30;
	}
}
