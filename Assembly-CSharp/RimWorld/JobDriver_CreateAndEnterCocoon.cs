using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BD4 RID: 3028
	public class JobDriver_CreateAndEnterCocoon : JobDriver_CreateAndEnterDryadHolder
	{
		// Token: 0x06005717 RID: 22295 RVA: 0x00204FC6 File Offset: 0x002031C6
		public override Toil EnterToil()
		{
			return Toils_General.Do(delegate
			{
				GenSpawn.Spawn(ThingDefOf.DryadCocoon, this.job.targetB.Cell, this.pawn.Map, WipeMode.Vanish).TryGetComp<CompDryadCocoon>().TryAcceptPawn(this.pawn);
			});
		}
	}
}
