using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A22 RID: 2594
	public class JobDriver_ControlMech : JobDriver
	{
		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x06004E61 RID: 20065 RVA: 0x001DD8A0 File Offset: 0x001DBAA0
		private Pawn Mech
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x06004E62 RID: 20066 RVA: 0x001DD8C6 File Offset: 0x001DBAC6
		private int MechControlTime
		{
			get
			{
				return Mathf.RoundToInt(this.Mech.GetStatValue(StatDefOf.ControlTakingTime, true, -1) * 60f);
			}
		}

		// Token: 0x06004E63 RID: 20067 RVA: 0x001DD8E5 File Offset: 0x001DBAE5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Mech, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004E64 RID: 20068 RVA: 0x001DD908 File Offset: 0x001DBB08
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ControlMech.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_ControlMech.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030B3 RID: 12467
		private const TargetIndex MechInd = TargetIndex.A;
	}
}
