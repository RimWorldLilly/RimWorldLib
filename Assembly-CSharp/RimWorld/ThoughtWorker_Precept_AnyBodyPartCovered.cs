using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9B RID: 3483
	public class ThoughtWorker_Precept_AnyBodyPartCovered : ThoughtWorker_Precept
	{
		// Token: 0x06005FE3 RID: 24547 RVA: 0x0022DB6C File Offset: 0x0022BD6C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return ThoughtWorker_Precept_AnyBodyPartCovered.HasUnnecessarilyCoveredBodyParts(p);
		}

		// Token: 0x06005FE4 RID: 24548 RVA: 0x0022DB7C File Offset: 0x0022BD7C
		public static bool HasUnnecessarilyCoveredBodyParts(Pawn p)
		{
			return p.apparel != null && p.apparel.AnyClothing && PawnUtility.HasClothingNotRequiredByKind(p) && GenTemperature.SafeTemperatureRange(p.def, null).Includes(p.AmbientTemperature);
		}
	}
}
