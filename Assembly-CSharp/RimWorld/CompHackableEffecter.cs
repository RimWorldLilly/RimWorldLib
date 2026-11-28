using System;

namespace RimWorld
{
	// Token: 0x02000ED0 RID: 3792
	public class CompHackableEffecter : CompEffecter
	{
		// Token: 0x06006388 RID: 25480 RVA: 0x0023BB4A File Offset: 0x00239D4A
		protected override bool ShouldShowEffecter()
		{
			if (!base.ShouldShowEffecter())
			{
				return false;
			}
			if (this.hackable == null)
			{
				this.hackable = this.parent.GetComp<CompHackable>();
			}
			CompHackable compHackable = this.hackable;
			return compHackable == null || compHackable.ShouldBeLitNow();
		}

		// Token: 0x040037AA RID: 14250
		private CompHackable hackable;
	}
}
