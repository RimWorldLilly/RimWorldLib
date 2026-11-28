using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D87 RID: 3463
	public class ThinkNode_ConditionalSentryDroneMode : ThinkNode_Conditional
	{
		// Token: 0x06005F97 RID: 24471 RVA: 0x0022C944 File Offset: 0x0022AB44
		protected override bool Satisfied(Pawn pawn)
		{
			CompSentryDrone compSentryDrone = pawn.TryGetComp<CompSentryDrone>();
			return compSentryDrone != null && compSentryDrone.Mode == this.mode;
		}

		// Token: 0x040035D0 RID: 13776
		public CompSentryDrone.SentryDroneMode mode;
	}
}
