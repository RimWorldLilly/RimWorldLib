using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E76 RID: 3702
	public class ThoughtWorker_ChildlikePositivity : ThoughtWorker
	{
		// Token: 0x06006210 RID: 25104 RVA: 0x00234D4C File Offset: 0x00232F4C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsFreeColonist || p.Suspended || p.DevelopmentalStage.Adult())
			{
				return ThoughtState.Inactive;
			}
			float ageBiologicalYearsFloat = p.ageTracker.AgeBiologicalYearsFloat;
			if (ageBiologicalYearsFloat < 3f)
			{
				return ThoughtState.Inactive;
			}
			for (int i = 0; i < ThoughtWorker_ChildlikePositivity.AgeThresholds.Length; i++)
			{
				if (ageBiologicalYearsFloat <= (float)ThoughtWorker_ChildlikePositivity.AgeThresholds[i])
				{
					return ThoughtState.ActiveAtStage(i);
				}
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x0400361B RID: 13851
		private const int MinAge = 3;

		// Token: 0x0400361C RID: 13852
		private static readonly int[] AgeThresholds = new int[]
		{
			6,
			8,
			10,
			13
		};
	}
}
