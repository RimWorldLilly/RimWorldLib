using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020008F1 RID: 2289
	public abstract class ThinkNode
	{
		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x0600461F RID: 17951 RVA: 0x001B85EC File Offset: 0x001B67EC
		public int UniqueSaveKey
		{
			get
			{
				return this.uniqueSaveKeyInt;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06004620 RID: 17952 RVA: 0x001B85F4 File Offset: 0x001B67F4
		public IEnumerable<ThinkNode> ThisAndChildrenRecursive
		{
			get
			{
				ThinkNode.<get_ThisAndChildrenRecursive>d__12 <get_ThisAndChildrenRecursive>d__ = new ThinkNode.<get_ThisAndChildrenRecursive>d__12(-2);
				<get_ThisAndChildrenRecursive>d__.<>4__this = this;
				return <get_ThisAndChildrenRecursive>d__;
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06004621 RID: 17953 RVA: 0x001B8604 File Offset: 0x001B6804
		public IEnumerable<ThinkNode> ChildrenRecursive
		{
			get
			{
				ThinkNode.<get_ChildrenRecursive>d__14 <get_ChildrenRecursive>d__ = new ThinkNode.<get_ChildrenRecursive>d__14(-2);
				<get_ChildrenRecursive>d__.<>4__this = this;
				return <get_ChildrenRecursive>d__;
			}
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x001B8614 File Offset: 0x001B6814
		protected ThinkNode()
		{
			this.hasPriority = (this.priority >= 0f || base.GetType().IsMethodOverriden("GetPriority"));
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x001B866C File Offset: 0x001B686C
		public T FirstNodeOfType<T>(bool includeThis = false) where T : ThinkNode
		{
			if (includeThis)
			{
				T t = this as T;
				if (t != null)
				{
					return t;
				}
			}
			foreach (ThinkNode thinkNode in this.subNodes)
			{
				T t2 = thinkNode.FirstNodeOfType<T>(true);
				if (t2 != null)
				{
					return t2;
				}
			}
			return default(T);
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x001B86F0 File Offset: 0x001B68F0
		public bool TryGetPriority(Pawn pawn, out float value)
		{
			if (!this.hasPriority)
			{
				value = 0f;
				return false;
			}
			value = this.GetPriority(pawn);
			return true;
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x001B870D File Offset: 0x001B690D
		public virtual float GetPriority(Pawn pawn)
		{
			if (this.priority < 0f)
			{
				Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + ((this != null) ? this.ToString() : null), this.GetHashCode());
				return 0f;
			}
			return this.priority;
		}

		// Token: 0x06004626 RID: 17958
		public abstract ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams);

		// Token: 0x06004627 RID: 17959 RVA: 0x001B874A File Offset: 0x001B694A
		protected virtual void ResolveSubnodes()
		{
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x001B874C File Offset: 0x001B694C
		public void ResolveSubnodesAndRecur()
		{
			if (this.uniqueSaveKeyInt != -2)
			{
				return;
			}
			this.ResolveSubnodes();
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				this.subNodes[i].ResolveSubnodesAndRecur();
			}
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x001B8791 File Offset: 0x001B6991
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x001B8794 File Offset: 0x001B6994
		public virtual ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode thinkNode = (ThinkNode)Activator.CreateInstance(base.GetType());
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				thinkNode.subNodes.Add(this.subNodes[i].DeepCopy(resolve));
			}
			thinkNode.priority = this.priority;
			thinkNode.leaveJoinableLordIfIssuesJob = this.leaveJoinableLordIfIssuesJob;
			thinkNode.uniqueSaveKeyInt = this.uniqueSaveKeyInt;
			thinkNode.tag = this.tag;
			if (resolve)
			{
				thinkNode.ResolveSubnodesAndRecur();
			}
			ThinkTreeKeyAssigner.AssignSingleKey(thinkNode, 0);
			return thinkNode;
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x001B8826 File Offset: 0x001B6A26
		internal void SetUniqueSaveKey(int key)
		{
			this.uniqueSaveKeyInt = key;
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x001B882F File Offset: 0x001B6A2F
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.uniqueSaveKeyInt, 1157295731);
		}

		// Token: 0x04002D84 RID: 11652
		public List<ThinkNode> subNodes = new List<ThinkNode>();

		// Token: 0x04002D85 RID: 11653
		public bool leaveJoinableLordIfIssuesJob;

		// Token: 0x04002D86 RID: 11654
		protected float priority = -1f;

		// Token: 0x04002D87 RID: 11655
		[Unsaved(false)]
		private int uniqueSaveKeyInt = -2;

		// Token: 0x04002D88 RID: 11656
		[NoTranslate]
		public string tag;

		// Token: 0x04002D89 RID: 11657
		[Unsaved(false)]
		public ThinkNode parent;

		// Token: 0x04002D8A RID: 11658
		[Unsaved(false)]
		private bool hasPriority;

		// Token: 0x04002D8B RID: 11659
		public const int InvalidSaveKey = -1;

		// Token: 0x04002D8C RID: 11660
		protected const int UnresolvedSaveKey = -2;
	}
}
