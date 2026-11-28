using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000809 RID: 2057
	public class JobGiver_PickupDroppedWeapon : ThinkNode_JobGiver
	{
		// Token: 0x060040C1 RID: 16577 RVA: 0x0019BE1C File Offset: 0x0019A01C
		protected override Job TryGiveJob(Pawn pawn)
		{
			Pawn_MindState mindState = pawn.mindState;
			Thing thing = (mindState != null) ? mindState.droppedWeapon : null;
			if (thing == null || thing.Destroyed || !thing.Spawned || thing.Map != pawn.Map)
			{
				return null;
			}
			return JobGiver_PickupDroppedWeapon.PickupWeaponJob(pawn, thing, this.ignoreForbidden);
		}

		// Token: 0x060040C2 RID: 16578 RVA: 0x0019BE6C File Offset: 0x0019A06C
		public static Job PickupWeaponJob(Pawn pawn, Thing weapon, bool ignoreForbidden)
		{
			if (!pawn.CanReserveAndReach(weapon, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			if (weapon.IsBurning())
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Equip, weapon);
			job.ignoreForbidden = ignoreForbidden;
			return job;
		}

		// Token: 0x04002AF6 RID: 10998
		public bool ignoreForbidden;
	}
}
