using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001515 RID: 5397
	public class InteractionWorker_KindWords : InteractionWorker
	{
		// Token: 0x06008440 RID: 33856 RVA: 0x002F9198 File Offset: 0x002F7398
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (initiator.Inhumanized())
			{
				return 0f;
			}
			Trait trait = initiator.story.traits.GetTrait(TraitDefOf.Kind);
			if (trait != null && !trait.Suppressed)
			{
				return 0.01f;
			}
			return 0f;
		}
	}
}
