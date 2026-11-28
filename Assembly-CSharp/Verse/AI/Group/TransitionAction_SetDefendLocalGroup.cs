using System;

namespace Verse.AI.Group
{
	// Token: 0x0200095B RID: 2395
	public class TransitionAction_SetDefendLocalGroup : TransitionAction
	{
		// Token: 0x06004959 RID: 18777 RVA: 0x001CA288 File Offset: 0x001C8488
		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
		}
	}
}
