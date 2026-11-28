using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001D2 RID: 466
	public static class ThinkTreeKeyAssigner
	{
		// Token: 0x06000D18 RID: 3352 RVA: 0x00043B24 File Offset: 0x00041D24
		public static void Reset()
		{
			ThinkTreeKeyAssigner.assignedKeys.Clear();
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x00043B30 File Offset: 0x00041D30
		public static void AssignKeys(ThinkNode rootNode, int startHash)
		{
			Rand.PushState(startHash);
			foreach (ThinkNode thinkNode in rootNode.ThisAndChildrenRecursive)
			{
				thinkNode.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(thinkNode));
			}
			Rand.PopState();
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00043B8C File Offset: 0x00041D8C
		public static void AssignSingleKey(ThinkNode node, int startHash)
		{
			Rand.PushState(startHash);
			node.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(node));
			Rand.PopState();
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00043BA8 File Offset: 0x00041DA8
		private static int NextUnusedKeyFor(ThinkNode node)
		{
			int num = 0;
			while (node != null)
			{
				num = Gen.HashCombineInt(num, GenText.StableStringHash(node.GetType().Name));
				node = node.parent;
			}
			while (ThinkTreeKeyAssigner.assignedKeys.Contains(num))
			{
				num ^= Rand.Int;
			}
			ThinkTreeKeyAssigner.assignedKeys.Add(num);
			return num;
		}

		// Token: 0x04000D25 RID: 3365
		private static HashSet<int> assignedKeys = new HashSet<int>();
	}
}
