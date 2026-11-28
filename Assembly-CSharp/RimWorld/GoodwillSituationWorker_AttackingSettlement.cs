using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200160B RID: 5643
	public class GoodwillSituationWorker_AttackingSettlement : GoodwillSituationWorker
	{
		// Token: 0x06008CAD RID: 36013 RVA: 0x00332023 File Offset: 0x00330223
		public override int GetMaxGoodwill(Faction other)
		{
			if (!this.IsAttackingSettlement(other))
			{
				return 100;
			}
			return -80;
		}

		// Token: 0x06008CAE RID: 36014 RVA: 0x00332033 File Offset: 0x00330233
		private bool IsAttackingSettlement(Faction other)
		{
			return Current.ProgramState != ProgramState.Entry && SettlementUtility.IsPlayerAttackingAnySettlementOf(other);
		}

		// Token: 0x0400563C RID: 22076
		private const int MaxGoodwill = -80;
	}
}
