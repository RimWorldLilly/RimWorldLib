using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000824 RID: 2084
	public class JobDriver_AttackStatic : JobDriver
	{
		// Token: 0x06004178 RID: 16760 RVA: 0x0019F70C File Offset: 0x0019D90C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.startedIncapacitated, "startedIncapacitated", false, false);
			Scribe_Values.Look<int>(ref this.numAttacksMade, "numAttacksMade", 0, false);
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x0019F738 File Offset: 0x0019D938
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x0019F73B File Offset: 0x0019D93B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_AttackStatic.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_AttackStatic.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002B29 RID: 11049
		private const int AutotargetRadius = 4;

		// Token: 0x04002B2A RID: 11050
		private bool startedIncapacitated;

		// Token: 0x04002B2B RID: 11051
		private int numAttacksMade;
	}
}
