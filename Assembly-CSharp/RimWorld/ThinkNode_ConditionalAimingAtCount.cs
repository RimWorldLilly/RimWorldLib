using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D7F RID: 3455
	public class ThinkNode_ConditionalAimingAtCount : ThinkNode_Conditional
	{
		// Token: 0x06005F85 RID: 24453 RVA: 0x0022C7B4 File Offset: 0x0022A9B4
		protected override bool Satisfied(Pawn pawn)
		{
			List<IAttackTarget> potentialTargetsFor = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn);
			int num = 0;
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				if (potentialTargetsFor[i].TargetCurrentlyAimingAt == pawn)
				{
					num++;
					if (num >= this.thresholdAiming)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040035CB RID: 13771
		public int thresholdAiming = 1;
	}
}
