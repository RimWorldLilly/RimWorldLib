using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000959 RID: 2393
	public class TransitionAction_WakeAll : TransitionAction
	{
		// Token: 0x06004955 RID: 18773 RVA: 0x001CA180 File Offset: 0x001C8380
		public override void DoAction(Transition trans)
		{
			List<Pawn> ownedPawns = trans.target.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				RestUtility.WakeUp(ownedPawns[i], true);
			}
			List<Building> ownedBuildings = trans.target.lord.ownedBuildings;
			for (int j = 0; j < ownedBuildings.Count; j++)
			{
				CompCanBeDormant compCanBeDormant = ownedBuildings[j].TryGetComp<CompCanBeDormant>();
				if (compCanBeDormant != null)
				{
					compCanBeDormant.WakeUpWithDelay();
				}
			}
		}
	}
}
