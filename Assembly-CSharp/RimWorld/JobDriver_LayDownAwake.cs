using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A4C RID: 2636
	public class JobDriver_LayDownAwake : JobDriver_LayDown
	{
		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x06004F9B RID: 20379 RVA: 0x001E1723 File Offset: 0x001DF923
		public override bool CanSleep
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x06004F9C RID: 20380 RVA: 0x001E1726 File Offset: 0x001DF926
		public override bool CanRest
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x06004F9D RID: 20381 RVA: 0x001E1729 File Offset: 0x001DF929
		public override bool LookForOtherJobs
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06004F9E RID: 20382 RVA: 0x001E172C File Offset: 0x001DF92C
		public override Rot4 ForcedLayingRotation
		{
			get
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (thing != null)
				{
					return thing.Rotation.Rotated(RotationDirection.Clockwise);
				}
				return base.ForcedLayingRotation;
			}
		}

		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06004F9F RID: 20383 RVA: 0x001E1768 File Offset: 0x001DF968
		public override Vector3 ForcedBodyOffset
		{
			get
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (thing != null && thing.def.Size.z > 1)
				{
					return new Vector3(0f, 0f, 0.5f).RotatedBy(thing.Rotation);
				}
				return base.ForcedBodyOffset;
			}
		}

		// Token: 0x06004FA0 RID: 20384 RVA: 0x001E17C6 File Offset: 0x001DF9C6
		public override string GetReport()
		{
			return "ReportLayingDown".Translate();
		}
	}
}
