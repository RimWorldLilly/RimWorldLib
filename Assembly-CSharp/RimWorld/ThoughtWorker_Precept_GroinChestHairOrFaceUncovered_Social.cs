using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DAA RID: 3498
	public class ThoughtWorker_Precept_GroinChestHairOrFaceUncovered_Social : ThoughtWorker_Precept_Social
	{
		// Token: 0x06006009 RID: 24585 RVA: 0x0022E31C File Offset: 0x0022C51C
		protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			return ThoughtWorker_Precept_GroinChestHairOrFaceUncovered.HasUncoveredGroinChestHairOrFace(otherPawn);
		}
	}
}
