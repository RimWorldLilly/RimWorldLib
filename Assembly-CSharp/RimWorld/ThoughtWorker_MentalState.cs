using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2B RID: 3627
	public class ThoughtWorker_MentalState : ThoughtWorker
	{
		// Token: 0x0600614A RID: 24906 RVA: 0x00232649 File Offset: 0x00230849
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.MentalStateDef == this.def.mentalState;
		}
	}
}
