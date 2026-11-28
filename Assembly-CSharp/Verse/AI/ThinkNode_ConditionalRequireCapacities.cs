using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020008C6 RID: 2246
	public class ThinkNode_ConditionalRequireCapacities : ThinkNode_Conditional
	{
		// Token: 0x060045AF RID: 17839 RVA: 0x001B7371 File Offset: 0x001B5571
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalRequireCapacities thinkNode_ConditionalRequireCapacities = (ThinkNode_ConditionalRequireCapacities)base.DeepCopy(resolve);
			thinkNode_ConditionalRequireCapacities.requiredCapacities = this.requiredCapacities;
			return thinkNode_ConditionalRequireCapacities;
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x001B738C File Offset: 0x001B558C
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.health != null && pawn.health.capacities != null)
			{
				foreach (PawnCapacityDef capacity in this.requiredCapacities)
				{
					if (!pawn.health.capacities.CapableOf(capacity))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x04002D54 RID: 11604
		public List<PawnCapacityDef> requiredCapacities;
	}
}
