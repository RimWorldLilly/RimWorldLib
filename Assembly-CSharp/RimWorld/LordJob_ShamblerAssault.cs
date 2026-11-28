using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C7F RID: 3199
	public class LordJob_ShamblerAssault : LordJob
	{
		// Token: 0x06005AB2 RID: 23218 RVA: 0x00215E34 File Offset: 0x00214034
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil toil = new LordToil_ShamblerAssault();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		// Token: 0x06005AB3 RID: 23219 RVA: 0x00215E53 File Offset: 0x00214053
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return (reason != PawnLostCondition.Incapped || !p.IsMutant || !p.mutant.Def.canAttackWhileCrawling || !p.health.CanCrawl) && base.ShouldRemovePawn(p, reason);
		}

		// Token: 0x06005AB4 RID: 23220 RVA: 0x00215E8C File Offset: 0x0021408C
		public override void Notify_PawnDowned(Pawn p)
		{
			base.Notify_PawnDowned(p);
			if (!p.health.CanCrawl)
			{
				this.lord.Notify_PawnLost(p, PawnLostCondition.Incapped, null);
			}
		}
	}
}
