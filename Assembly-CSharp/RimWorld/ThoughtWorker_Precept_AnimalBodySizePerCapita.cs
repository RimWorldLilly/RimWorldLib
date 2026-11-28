using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB9 RID: 3513
	public class ThoughtWorker_Precept_AnimalBodySizePerCapita : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x0600602B RID: 24619 RVA: 0x0022E88C File Offset: 0x0022CA8C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (ThoughtUtility.ThoughtNullified(p, this.def))
			{
				return false;
			}
			if (p.IsSlave)
			{
				return false;
			}
			float num = PawnUtility.PlayerAnimalBodySizePerCapita();
			if (num <= 2f && GenTicks.TicksAbs < 900000)
			{
				return false;
			}
			if (num < 4f && this.def.minExpectationForNegativeThought != null && p.MapHeld != null && ExpectationsUtility.CurrentExpectationFor(p.MapHeld).order < this.def.minExpectationForNegativeThought.order)
			{
				return false;
			}
			if (this.ThoughtStageFromAnimalDensity(num) < 0)
			{
				return false;
			}
			return ThoughtState.ActiveAtStage(this.ThoughtStageFromAnimalDensity(PawnUtility.PlayerAnimalBodySizePerCapita()));
		}

		// Token: 0x0600602C RID: 24620 RVA: 0x0022E948 File Offset: 0x0022CB48
		public override string PostProcessDescription(Pawn p, string description)
		{
			return base.PostProcessDescription(p, description) + "\n\n" + "CurrentTotalAnimalBodySizePerColonist".Translate() + ": " + PawnUtility.PlayerAnimalBodySizePerCapita().ToString("F1") + "\n" + "MinAnimalBodySizePerColonist".Translate(4f.ToString("F1"));
		}

		// Token: 0x0600602D RID: 24621 RVA: 0x0022E9CC File Offset: 0x0022CBCC
		private int ThoughtStageFromAnimalDensity(float density)
		{
			if (density < 0f)
			{
				return 0;
			}
			if (density < 1f)
			{
				return 1;
			}
			if (density < 2f)
			{
				return 2;
			}
			if (density < 4f)
			{
				return -1;
			}
			if (density < 6f)
			{
				return 3;
			}
			if (density < 8f)
			{
				return 4;
			}
			return 5;
		}

		// Token: 0x0600602E RID: 24622 RVA: 0x0022EA0B File Offset: 0x0022CC0B
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			return new ThoughtWorker_Precept_AnimalBodySizePerCapita.<GetDescriptionArgs>d__10(-2);
		}

		// Token: 0x040035E6 RID: 13798
		private const float NoAnimals = 0f;

		// Token: 0x040035E7 RID: 13799
		private const float ScarceAnimals = 1f;

		// Token: 0x040035E8 RID: 13800
		private const float FewAnimals = 2f;

		// Token: 0x040035E9 RID: 13801
		private const float NoThought = 4f;

		// Token: 0x040035EA RID: 13802
		private const float SomeAnimals = 6f;

		// Token: 0x040035EB RID: 13803
		private const float LotsOfAnimals = 8f;

		// Token: 0x040035EC RID: 13804
		private const int MinimumTicksBeforeFewAnimals = 900000;
	}
}
