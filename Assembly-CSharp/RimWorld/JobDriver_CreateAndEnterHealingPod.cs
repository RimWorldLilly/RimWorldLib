using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BD5 RID: 3029
	public class JobDriver_CreateAndEnterHealingPod : JobDriver_CreateAndEnterDryadHolder
	{
		// Token: 0x0600571A RID: 22298 RVA: 0x00205019 File Offset: 0x00203219
		public override Toil EnterToil()
		{
			return Toils_General.Do(delegate
			{
				GenSpawn.Spawn(ThingDefOf.DryadHealingPod, this.job.targetB.Cell, this.pawn.Map, WipeMode.Vanish).TryGetComp<CompDryadHealingPod>().TryAcceptPawn(this.pawn);
			});
		}
	}
}
