using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009F5 RID: 2549
	public class JobDriver_GoSwimming : JobDriver
	{
		// Token: 0x06004D37 RID: 19767 RVA: 0x001D9C84 File Offset: 0x001D7E84
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004D38 RID: 19768 RVA: 0x001D9C88 File Offset: 0x001D7E88
		private bool NextDestIsOutdoorsAndNotEnjoyable()
		{
			if (base.Map == null)
			{
				return false;
			}
			if (!this.job.targetA.IsValid)
			{
				return false;
			}
			Room room = this.job.targetA.Cell.GetRoom(base.Map);
			return room != null && room.PsychologicallyOutdoors && !JoyGiver_GoSwimming.HappyToSwimOutsideOnMap(base.Map);
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x001D9CEA File Offset: 0x001D7EEA
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_GoSwimming.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_GoSwimming.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004D3A RID: 19770 RVA: 0x001D9CFA File Offset: 0x001D7EFA
		private void CheckForSwimmingPose()
		{
			if (this.pawn.Position.GetTerrain(this.pawn.Map).IsWater)
			{
				this.job.swimming = true;
				return;
			}
			this.job.swimming = false;
		}
	}
}
