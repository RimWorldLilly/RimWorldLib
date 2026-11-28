using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D88 RID: 3464
	public class ThinkNode_TraitBehaviors : ThinkNode
	{
		// Token: 0x06005F9A RID: 24474 RVA: 0x0022C97C File Offset: 0x0022AB7C
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			List<Trait> allTraits = pawn.story.traits.allTraits;
			for (int i = 0; i < allTraits.Count; i++)
			{
				if (!allTraits[i].Suppressed)
				{
					ThinkTreeDef thinkTree = allTraits[i].CurrentData.thinkTree;
					if (thinkTree != null)
					{
						return thinkTree.thinkRoot.TryIssueJobPackage(pawn, jobParams);
					}
				}
			}
			return ThinkResult.NoJob;
		}
	}
}
