using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B51 RID: 2897
	public class JobGiver_MetalhorrorFight : JobGiver_AIFightEnemy
	{
		// Token: 0x060054B8 RID: 21688 RVA: 0x001F79E8 File Offset: 0x001F5BE8
		public JobGiver_MetalhorrorFight()
		{
			this.targetAcquireRadius = 29.9f;
			this.targetKeepRadius = this.targetAcquireRadius * 2f;
		}

		// Token: 0x060054B9 RID: 21689 RVA: 0x001F7A0D File Offset: 0x001F5C0D
		protected override Thing FindAttackTarget(Pawn pawn)
		{
			return MetalhorrorUtility.FindTarget(pawn);
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x001F7A15 File Offset: 0x001F5C15
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest, Verb verbToUse = null)
		{
			dest = IntVec3.Invalid;
			return false;
		}
	}
}
