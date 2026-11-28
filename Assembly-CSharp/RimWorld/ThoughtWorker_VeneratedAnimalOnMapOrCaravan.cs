using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DC2 RID: 3522
	public class ThoughtWorker_VeneratedAnimalOnMapOrCaravan : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x0600604F RID: 24655 RVA: 0x0022F0A0 File Offset: 0x0022D2A0
		public override string PostProcessLabel(Pawn p, string label)
		{
			Pawn pawn = PawnUtility.FirstVeneratedAnimalOnMapOrCaravan(p);
			if (pawn == null)
			{
				return label;
			}
			return label.Formatted(pawn.kindDef.Named("ANIMAL"));
		}

		// Token: 0x06006050 RID: 24656 RVA: 0x0022F0D4 File Offset: 0x0022D2D4
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.IsSlave)
			{
				return false;
			}
			int num = this.ThoughtStageFromAnimalDensity(PawnUtility.PlayerVeneratedAnimalBodySizePerCapitaOnMapOrCaravan(p));
			if (num >= 0)
			{
				return ThoughtState.ActiveAtStage(num);
			}
			return false;
		}

		// Token: 0x06006051 RID: 24657 RVA: 0x0022F10E File Offset: 0x0022D30E
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			return new ThoughtWorker_VeneratedAnimalOnMapOrCaravan.<GetDescriptionArgs>d__6(-2);
		}

		// Token: 0x06006052 RID: 24658 RVA: 0x0022F117 File Offset: 0x0022D317
		private int ThoughtStageFromAnimalDensity(float density)
		{
			if (density <= 0f)
			{
				return -1;
			}
			if (density < 1f)
			{
				return 0;
			}
			if (density < 2f)
			{
				return 1;
			}
			if (density < 4f)
			{
				return 2;
			}
			if (density < 6f)
			{
				return 3;
			}
			return 4;
		}

		// Token: 0x040035F4 RID: 13812
		private const float FewAnimals = 1f;

		// Token: 0x040035F5 RID: 13813
		private const float SomeAnimals = 2f;

		// Token: 0x040035F6 RID: 13814
		private const float ManyAnimals = 4f;

		// Token: 0x040035F7 RID: 13815
		private const float LotsOfAnimals = 6f;
	}
}
