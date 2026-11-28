using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2C RID: 3628
	public class ThoughtWorker_GameCondition : ThoughtWorker
	{
		// Token: 0x0600614C RID: 24908 RVA: 0x0023266C File Offset: 0x0023086C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.SpawnedOrAnyParentSpawned && p.MapHeld.gameConditionManager.ConditionIsActive(this.def.gameCondition))
			{
				return true;
			}
			if (Find.World.gameConditionManager.ConditionIsActive(this.def.gameCondition))
			{
				return true;
			}
			return false;
		}
	}
}
