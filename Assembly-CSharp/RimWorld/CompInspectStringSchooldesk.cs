using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C7 RID: 6343
	public class CompInspectStringSchooldesk : CompInspectString
	{
		// Token: 0x06009F05 RID: 40709 RVA: 0x00390F34 File Offset: 0x0038F134
		public override string CompInspectStringExtra()
		{
			float num = LearningUtility.SchoolDeskLearningRate(this.parent);
			if (num > 1f)
			{
				return base.Props.inspectString + ": " + (num - 1f).ToStringPercent();
			}
			return null;
		}
	}
}
