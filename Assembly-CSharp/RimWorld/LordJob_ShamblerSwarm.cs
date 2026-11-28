using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C80 RID: 3200
	public class LordJob_ShamblerSwarm : LordJob_EntitySwarm
	{
		// Token: 0x06005AB6 RID: 23222 RVA: 0x00215ECB File Offset: 0x002140CB
		public LordJob_ShamblerSwarm()
		{
		}

		// Token: 0x06005AB7 RID: 23223 RVA: 0x00215ED3 File Offset: 0x002140D3
		public LordJob_ShamblerSwarm(IntVec3 startPos, IntVec3 destPos) : base(startPos, destPos)
		{
		}

		// Token: 0x06005AB8 RID: 23224 RVA: 0x00215EDD File Offset: 0x002140DD
		protected override LordToil CreateTravelingToil(IntVec3 start, IntVec3 dest)
		{
			return new LordToil_ShamblerSwarm(start, dest);
		}

		// Token: 0x06005AB9 RID: 23225 RVA: 0x00215EE6 File Offset: 0x002140E6
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return (reason != PawnLostCondition.Incapped || !p.IsMutant || !p.mutant.Def.canAttackWhileCrawling || !p.health.CanCrawl) && base.ShouldRemovePawn(p, reason);
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x00215F20 File Offset: 0x00214120
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
