using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000888 RID: 2184
	public class MentalState_TantrumAll : MentalState_TantrumRandom
	{
		// Token: 0x06004406 RID: 17414 RVA: 0x001AB1DD File Offset: 0x001A93DD
		protected override void GetPotentialTargets(List<Thing> outThings)
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, outThings, this.GetCustomValidator(), 0, 40);
		}
	}
}
