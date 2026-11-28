using System;

namespace RimWorld
{
	// Token: 0x020017F7 RID: 6135
	public class CompCauseGameCondition_AncientReactor : CompCauseGameCondition
	{
		// Token: 0x17001A0B RID: 6667
		// (get) Token: 0x06009929 RID: 39209 RVA: 0x00376F79 File Offset: 0x00375179
		public override bool Active
		{
			get
			{
				return !this.HackableComp.IsHacked;
			}
		}

		// Token: 0x17001A0C RID: 6668
		// (get) Token: 0x0600992A RID: 39210 RVA: 0x00376F8C File Offset: 0x0037518C
		public CompHackable HackableComp
		{
			get
			{
				CompHackable result;
				if ((result = this.hackableComp) == null)
				{
					result = (this.hackableComp = this.parent.GetComp<CompHackable>());
				}
				return result;
			}
		}

		// Token: 0x04005B76 RID: 23414
		private CompHackable hackableComp;
	}
}
