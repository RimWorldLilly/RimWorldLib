using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200095E RID: 2398
	public class TransitionAction_CheckGiveGift : TransitionAction
	{
		// Token: 0x0600495F RID: 18783 RVA: 0x001CA3B8 File Offset: 0x001C85B8
		public override void DoAction(Transition trans)
		{
			if (DebugSettings.instantVisitorsGift || (trans.target.lord.numPawnsLostViolently == 0 && Rand.Chance(VisitorGiftForPlayerUtility.ChanceToLeaveGift(trans.target.lord.faction, trans.Map))))
			{
				VisitorGiftForPlayerUtility.GiveRandomGift(trans.target.lord.ownedPawns, trans.target.lord.faction);
			}
		}
	}
}
