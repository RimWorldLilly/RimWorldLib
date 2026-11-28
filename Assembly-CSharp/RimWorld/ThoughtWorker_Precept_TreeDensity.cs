using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD5 RID: 3541
	public class ThoughtWorker_Precept_TreeDensity : ThoughtWorker_Precept
	{
		// Token: 0x06006081 RID: 24705 RVA: 0x0022F93B File Offset: 0x0022DB3B
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(this.ThoughtStageIndex(p));
		}

		// Token: 0x06006082 RID: 24706 RVA: 0x0022F958 File Offset: 0x0022DB58
		private int ThoughtStageIndex(Pawn p)
		{
			int playerResponsibleTreeDestructionCount = p.Map.treeDestructionTracker.PlayerResponsibleTreeDestructionCount;
			for (int i = ThoughtWorker_Precept_TreeDensity.treeDestructionThresholds.Length - 1; i >= 0; i--)
			{
				if (playerResponsibleTreeDestructionCount >= ThoughtWorker_Precept_TreeDensity.treeDestructionThresholds[i])
				{
					return i + 1;
				}
			}
			return 0;
		}

		// Token: 0x040035FA RID: 13818
		private static readonly int[] treeDestructionThresholds = new int[]
		{
			2,
			5,
			8,
			10,
			13,
			16,
			20
		};
	}
}
