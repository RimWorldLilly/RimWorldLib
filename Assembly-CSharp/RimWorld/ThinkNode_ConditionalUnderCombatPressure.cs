using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D84 RID: 3460
	public class ThinkNode_ConditionalUnderCombatPressure : ThinkNode_Conditional
	{
		// Token: 0x06005F90 RID: 24464 RVA: 0x0022C881 File Offset: 0x0022AA81
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Spawned && !pawn.Downed && PawnUtility.EnemiesAreNearby(pawn, 9, true, this.maxThreatDistance, this.minCloseTargets, false);
		}

		// Token: 0x06005F91 RID: 24465 RVA: 0x0022C8AB File Offset: 0x0022AAAB
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalUnderCombatPressure thinkNode_ConditionalUnderCombatPressure = (ThinkNode_ConditionalUnderCombatPressure)base.DeepCopy(resolve);
			thinkNode_ConditionalUnderCombatPressure.maxThreatDistance = this.maxThreatDistance;
			thinkNode_ConditionalUnderCombatPressure.minCloseTargets = this.minCloseTargets;
			return thinkNode_ConditionalUnderCombatPressure;
		}

		// Token: 0x040035CD RID: 13773
		public float maxThreatDistance = 2f;

		// Token: 0x040035CE RID: 13774
		public int minCloseTargets = 2;
	}
}
