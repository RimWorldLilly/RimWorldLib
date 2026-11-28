using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200087F RID: 2175
	public class MentalState_Jailbreaker : MentalState
	{
		// Token: 0x060043DD RID: 17373 RVA: 0x001AAC10 File Offset: 0x001A8E10
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if (this.pawn.IsHashIntervalTick(500, delta) && this.pawn.CurJobDef != JobDefOf.InducePrisonerToEscape && JailbreakerMentalStateUtility.FindPrisoner(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x060043DE RID: 17374 RVA: 0x001AAC5C File Offset: 0x001A8E5C
		public void Notify_InducedPrisonerToEscape()
		{
			MentalStateUtility.TryTransitionToWanderOwnRoom(this);
		}

		// Token: 0x04002C29 RID: 11305
		private const int NoPrisonerToFreeCheckInterval = 500;
	}
}
