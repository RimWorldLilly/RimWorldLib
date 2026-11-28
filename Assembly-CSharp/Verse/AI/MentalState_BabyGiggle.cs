using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200087C RID: 2172
	public class MentalState_BabyGiggle : MentalState_BabyFit
	{
		// Token: 0x060043D0 RID: 17360 RVA: 0x001AA78C File Offset: 0x001A898C
		protected override void AuraEffect(Thing source, Pawn hearer)
		{
			Pawn pawn = source as Pawn;
			if (pawn != null && hearer.needs.mood != null)
			{
				if (hearer == pawn.GetMother() || hearer == pawn.GetFather())
				{
					hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyGigglingBaby, pawn, null);
				}
				else
				{
					hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.GigglingBaby, pawn, null);
				}
				hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BabyGiggledSocial, pawn, null);
			}
		}
	}
}
