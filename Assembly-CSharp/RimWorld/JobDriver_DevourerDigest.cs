using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009F3 RID: 2547
	public class JobDriver_DevourerDigest : JobDriver
	{
		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06004D2E RID: 19758 RVA: 0x001D9C10 File Offset: 0x001D7E10
		private CompDevourer Comp
		{
			get
			{
				CompDevourer result;
				if ((result = this.comp) == null)
				{
					result = (this.comp = this.pawn.GetComp<CompDevourer>());
				}
				return result;
			}
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x001D9C3B File Offset: 0x001D7E3B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004D30 RID: 19760 RVA: 0x001D9C3E File Offset: 0x001D7E3E
		public override string GetReport()
		{
			return null;
		}

		// Token: 0x06004D31 RID: 19761 RVA: 0x001D9C41 File Offset: 0x001D7E41
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_DevourerDigest.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_DevourerDigest.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003053 RID: 12371
		private CompDevourer comp;
	}
}
