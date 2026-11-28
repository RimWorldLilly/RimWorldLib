using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000E7 RID: 231
	public class JobDriver_CastAbilityWorld : JobDriver
	{
		// Token: 0x0600086C RID: 2156 RVA: 0x000304CB File Offset: 0x0002E6CB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x000304CE File Offset: 0x0002E6CE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CastAbilityWorld.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_CastAbilityWorld.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x000304DE File Offset: 0x0002E6DE
		public override string GetReport()
		{
			return "UsingVerb".Translate(this.job.ability.def.label, this.job.globalTarget.Label);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0003051E File Offset: 0x0002E71E
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			Ability ability = this.job.ability;
			if (ability == null)
			{
				return;
			}
			ability.Notify_StartedCasting();
		}
	}
}
