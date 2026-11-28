using System;

namespace RimWorld
{
	// Token: 0x0200160D RID: 5645
	public class GoodwillSituationWorker_NaturalEnemy : GoodwillSituationWorker
	{
		// Token: 0x06008CB5 RID: 36021 RVA: 0x00332177 File Offset: 0x00330377
		public override int GetNaturalGoodwillOffset(Faction other)
		{
			if (!other.def.naturalEnemy)
			{
				return 0;
			}
			return -130;
		}
	}
}
