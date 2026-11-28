using System;

namespace Verse
{
	// Token: 0x02000604 RID: 1540
	public class Command_ActionWithLimitedUseCount : Command_Action
	{
		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x0600304F RID: 12367 RVA: 0x00135D44 File Offset: 0x00133F44
		public override string TopRightLabel
		{
			get
			{
				return this.usesLeftGetter().ToString() + " / " + this.maxUsesGetter().ToString();
			}
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x00135D81 File Offset: 0x00133F81
		public void UpdateUsesLeft()
		{
			if (this.usesLeftGetter() == 0)
			{
				base.Disable("CommandNoUsesLeft".Translate());
			}
		}

		// Token: 0x04001EF9 RID: 7929
		public Func<int> usesLeftGetter;

		// Token: 0x04001EFA RID: 7930
		public Func<int> maxUsesGetter;
	}
}
