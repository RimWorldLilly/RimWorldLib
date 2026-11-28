using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001D3 RID: 467
	public class ThinkTreeDef : Def
	{
		// Token: 0x06000D1D RID: 3357 RVA: 0x00043C0C File Offset: 0x00041E0C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.thinkRoot.ResolveSubnodesAndRecur();
			foreach (ThinkNode thinkNode in this.thinkRoot.ThisAndChildrenRecursive)
			{
				thinkNode.ResolveReferences();
			}
			ThinkTreeKeyAssigner.AssignKeys(this.thinkRoot, GenText.StableStringHash(this.defName));
			this.ResolveParentNodes(this.thinkRoot);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00043C90 File Offset: 0x00041E90
		public override IEnumerable<string> ConfigErrors()
		{
			ThinkTreeDef.<ConfigErrors>d__4 <ConfigErrors>d__ = new ThinkTreeDef.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00043CA0 File Offset: 0x00041EA0
		public bool TryGetThinkNodeWithSaveKey(int key, out ThinkNode outNode)
		{
			outNode = null;
			if (key == -1)
			{
				return false;
			}
			if (key == this.thinkRoot.UniqueSaveKey)
			{
				outNode = this.thinkRoot;
				return true;
			}
			foreach (ThinkNode thinkNode in this.thinkRoot.ChildrenRecursive)
			{
				if (thinkNode.UniqueSaveKey == key)
				{
					outNode = thinkNode;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00043D20 File Offset: 0x00041F20
		private void ResolveParentNodes(ThinkNode node)
		{
			for (int i = 0; i < node.subNodes.Count; i++)
			{
				if (node.subNodes[i].parent != null)
				{
					string[] array = new string[7];
					array[0] = "Think node ";
					int num = 1;
					ThinkNode thinkNode = node.subNodes[i];
					array[num] = ((thinkNode != null) ? thinkNode.ToString() : null);
					array[2] = " from think tree ";
					array[3] = this.defName;
					array[4] = " already has a parent node (";
					int num2 = 5;
					ThinkNode parent = node.subNodes[i].parent;
					array[num2] = ((parent != null) ? parent.ToString() : null);
					array[6] = "). This means that it's referenced by more than one think tree (should have been copied instead).";
					Log.Warning(string.Concat(array));
				}
				else
				{
					node.subNodes[i].parent = node;
					this.ResolveParentNodes(node.subNodes[i]);
				}
			}
		}

		// Token: 0x04000D26 RID: 3366
		public ThinkNode thinkRoot;

		// Token: 0x04000D27 RID: 3367
		[NoTranslate]
		public string insertTag;

		// Token: 0x04000D28 RID: 3368
		public float insertPriority;
	}
}
