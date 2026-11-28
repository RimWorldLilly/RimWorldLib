using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D5C RID: 3420
	public class ThinkNode_ConditionalCanReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x06005F37 RID: 24375 RVA: 0x0022BC06 File Offset: 0x00229E06
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCanReachMapEdge thinkNode_ConditionalCanReachMapEdge = (ThinkNode_ConditionalCanReachMapEdge)base.DeepCopy(resolve);
			thinkNode_ConditionalCanReachMapEdge.allowFlyOutInstead = this.allowFlyOutInstead;
			return thinkNode_ConditionalCanReachMapEdge;
		}

		// Token: 0x06005F38 RID: 24376 RVA: 0x0022BC20 File Offset: 0x00229E20
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.CanReachMapEdge() || (this.allowFlyOutInstead && (pawn.RaceProps.canLeaveMapFlying && !pawn.Position.Roofed(pawn.Map) && pawn.Faction != Faction.OfPlayer && pawn.flight.CanEverFly) && !pawn.IsQuestLodger());
		}

		// Token: 0x040035BC RID: 13756
		public bool allowFlyOutInstead;
	}
}
