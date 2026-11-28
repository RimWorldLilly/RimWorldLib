using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A4D RID: 2637
	public class JobDriver_LayDownResting : JobDriver_LayDownAwake
	{
		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06004FA2 RID: 20386 RVA: 0x001E17DF File Offset: 0x001DF9DF
		public override Rot4 ForcedLayingRotation
		{
			get
			{
				return Rot4.Invalid;
			}
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x001E17E6 File Offset: 0x001DF9E6
		public override Toil LayDownToil(bool hasBed)
		{
			return Toils_LayDown.LayDown(TargetIndex.A, hasBed, this.LookForOtherJobs, this.CanSleep, this.CanRest, PawnPosture.LayingOnGroundFaceUp, false);
		}
	}
}
