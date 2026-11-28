using System;

namespace RimWorld
{
	// Token: 0x0200160E RID: 5646
	public class GoodwillSituationWorker_PermanentEnemy : GoodwillSituationWorker
	{
		// Token: 0x06008CB7 RID: 36023 RVA: 0x00332195 File Offset: 0x00330395
		public override int GetMaxGoodwill(Faction other)
		{
			if (Faction.OfPlayerSilentFail == null)
			{
				return 100;
			}
			if (!GoodwillSituationWorker_PermanentEnemy.ArePermanentEnemies(Faction.OfPlayer, other))
			{
				return 100;
			}
			return -100;
		}

		// Token: 0x06008CB8 RID: 36024 RVA: 0x003321B4 File Offset: 0x003303B4
		public static bool ArePermanentEnemies(Faction a, Faction b)
		{
			return a.def.permanentEnemy || b.def.permanentEnemy || (a.def.permanentEnemyToEveryoneExceptPlayer && !b.IsPlayer) || (b.def.permanentEnemyToEveryoneExceptPlayer && !a.IsPlayer) || (a.def.permanentEnemyToEveryoneExcept != null && !a.def.permanentEnemyToEveryoneExcept.Contains(b.def)) || (b.def.permanentEnemyToEveryoneExcept != null && !b.def.permanentEnemyToEveryoneExcept.Contains(a.def));
		}
	}
}
