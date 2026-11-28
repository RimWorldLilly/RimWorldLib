using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009D9 RID: 2521
	public class JobDriver_BeatFire : JobDriver
	{
		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06004C78 RID: 19576 RVA: 0x001D7109 File Offset: 0x001D5309
		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x001D7120 File Offset: 0x001D5320
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x001D7123 File Offset: 0x001D5323
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_BeatFire.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_BeatFire.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004C7B RID: 19579 RVA: 0x001D7134 File Offset: 0x001D5334
		private bool StartBeatingFireIfAnyAt(IntVec3 cell, Toil nextToil)
		{
			List<Thing> thingList = cell.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Fire fire = thingList[i] as Fire;
				if (fire != null && fire.parent == null)
				{
					this.job.targetA = fire;
					this.pawn.pather.StopDead();
					base.JumpToToil(nextToil);
					return true;
				}
			}
			return false;
		}
	}
}
