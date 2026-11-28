using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009DA RID: 2522
	public class JobDriver_ExtinguishFiresNearby : JobDriver
	{
		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06004C7D RID: 19581 RVA: 0x001D71AA File Offset: 0x001D53AA
		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		// Token: 0x06004C7E RID: 19582 RVA: 0x001D71C1 File Offset: 0x001D53C1
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x001D71C4 File Offset: 0x001D53C4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ExtinguishFiresNearby.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_ExtinguishFiresNearby.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x001D71D4 File Offset: 0x001D53D4
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

		// Token: 0x06004C81 RID: 19585 RVA: 0x001D7242 File Offset: 0x001D5442
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksSpentExtinguishing, "ticksSpentExtinguishing", 0, false);
		}

		// Token: 0x0400300D RID: 12301
		private int ticksSpentExtinguishing;

		// Token: 0x0400300E RID: 12302
		private readonly int maxDurationTicks = 600f.SecondsToTicks();

		// Token: 0x0400300F RID: 12303
		private const TargetIndex FireInd = TargetIndex.A;

		// Token: 0x04003010 RID: 12304
		private const float MaxDurationSeconds = 600f;
	}
}
