using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000055 RID: 85
	internal class EditWindow_DefEditor : EditWindow
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00011EA2 File Offset: 0x000100A2
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 600f);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00011EB3 File Offset: 0x000100B3
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00011EB8 File Offset: 0x000100B8
		public EditWindow_DefEditor(Def def)
		{
			this.def = def;
			this.cleanCopy = EditTreeNodeDatabase.RootOf(EditWindow_DefEditor.DeepClone(def));
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.optionalTitle = def.ToString();
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00011F08 File Offset: 0x00010108
		public void InstanceEffecter()
		{
			Effecter effecter = ((EffecterDef)this.def).Spawn();
			IntVec3 cell = UI.UIToMapPosition(Find.Camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f))).ToIntVec3();
			effecter.Trigger(new TargetInfo(cell, Find.CurrentMap, false), new TargetInfo(cell, Find.CurrentMap, false), -1);
			effecter.Cleanup();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00011F74 File Offset: 0x00010174
		public override void DoWindowContents(Rect inRect)
		{
			if (Event.current.type == EventType.keyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Escape))
			{
				UI.UnfocusCurrentControl();
			}
			Rect rect = new Rect(0f, 0f, inRect.width, 16f);
			this.labelColumnWidth = Widgets.HorizontalSlider(rect, this.labelColumnWidth, 0f, inRect.width, false, null, null, null, -1f);
			Rect outRect = inRect.AtZero();
			outRect.yMin += 16f;
			if (this.def is EffecterDef)
			{
				float x = inRect.x;
				base.DoRowButton(ref x, outRect.yMin, "Spawn Effecter", "spawn the effecter in the world", new Action(this.InstanceEffecter));
				base.DoRowButton(ref x, outRect.yMin, "Log Diff", "Diff the in-memory Def with the original", new Action(this.PrintDirty));
				outRect.yMin += 24f;
			}
			Rect rect2 = new Rect(0f, 0f, outRect.width - 16f, this.viewHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			Listing_TreeDefs listing_TreeDefs = new Listing_TreeDefs(this.labelColumnWidth);
			listing_TreeDefs.Begin(rect2);
			TreeNode_Editor node = EditTreeNodeDatabase.RootOf(this.def);
			listing_TreeDefs.ContentLines(node, 0);
			listing_TreeDefs.End();
			if (Event.current.type == EventType.layout)
			{
				this.viewHeight = listing_TreeDefs.CurHeight + 200f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0001211A File Offset: 0x0001031A
		public override void PreClose()
		{
			base.PreClose();
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00012124 File Offset: 0x00010324
		public void PrintDirty()
		{
			Dictionary<string, string> dictionary = this.Compare(this.cleanCopy, EditTreeNodeDatabase.RootOf(this.def));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("DirtyValues For " + this.def.defName);
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				stringBuilder.AppendLine(keyValuePair.Key + ":" + keyValuePair.Value);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000121D0 File Offset: 0x000103D0
		private Dictionary<string, string> Compare(TreeNode_Editor fromTree, TreeNode_Editor toTree)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			List<TreeNode_Editor> list = this.Flatten(fromTree, new List<TreeNode>()).Cast<TreeNode_Editor>().ToList<TreeNode_Editor>();
			List<TreeNode_Editor> list2 = this.Flatten(toTree, new List<TreeNode>()).Cast<TreeNode_Editor>().ToList<TreeNode_Editor>();
			int num = (list.Count < list2.Count) ? list.Count : list2.Count;
			for (int i = 0; i < num; i++)
			{
				TreeNode_Editor treeNode_Editor = list[i];
				TreeNode_Editor treeNode_Editor2 = list2[i];
				if (treeNode_Editor.HasValue && treeNode_Editor2.HasValue && treeNode_Editor.Value != null && treeNode_Editor2.Value != null && treeNode_Editor.Value.ToString() != treeNode_Editor2.Value.ToString())
				{
					Dictionary<string, string> dictionary2 = dictionary;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(treeNode_Editor.LabelText);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted<int>(treeNode_Editor.GetHashCode());
					string key = defaultInterpolatedStringHandler.ToStringAndClear();
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(4, 2);
					defaultInterpolatedStringHandler2.AppendFormatted<object>(treeNode_Editor.Value);
					defaultInterpolatedStringHandler2.AppendLiteral(" -> ");
					defaultInterpolatedStringHandler2.AppendFormatted<object>(treeNode_Editor2.Value);
					dictionary2[key] = defaultInterpolatedStringHandler2.ToStringAndClear();
				}
			}
			return dictionary;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00012320 File Offset: 0x00010520
		private List<TreeNode> Flatten(TreeNode tree, List<TreeNode> treeNodes)
		{
			treeNodes.Add(tree);
			if (tree.children != null)
			{
				foreach (TreeNode tree2 in tree.children)
				{
					this.Flatten(tree2, treeNodes);
				}
			}
			return treeNodes;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00012388 File Offset: 0x00010588
		public static object DeepClone(object objSource)
		{
			Type type = objSource.GetType();
			object obj = Activator.CreateInstance(type);
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (propertyInfo.CanWrite)
				{
					if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.IsEnum || propertyInfo.PropertyType == typeof(string))
					{
						propertyInfo.SetValue(obj, propertyInfo.GetValue(objSource, null), null);
					}
					else
					{
						object value = propertyInfo.GetValue(objSource, null);
						if (value == null)
						{
							propertyInfo.SetValue(obj, null, null);
						}
						else
						{
							propertyInfo.SetValue(obj, EditWindow_DefEditor.DeepClone(value), null);
						}
					}
				}
			}
			return obj;
		}

		// Token: 0x04000132 RID: 306
		public Def def;

		// Token: 0x04000133 RID: 307
		public TreeNode_Editor cleanCopy;

		// Token: 0x04000134 RID: 308
		private float viewHeight;

		// Token: 0x04000135 RID: 309
		private Vector2 scrollPosition;

		// Token: 0x04000136 RID: 310
		private float labelColumnWidth = 140f;

		// Token: 0x04000137 RID: 311
		private const float TopAreaHeight = 16f;

		// Token: 0x04000138 RID: 312
		private const float ExtraScrollHeight = 200f;
	}
}
