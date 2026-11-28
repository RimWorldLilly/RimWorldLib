using System;

namespace Verse.AI.Group
{
	// Token: 0x02000949 RID: 2377
	public class LordToilData_ExitMap : LordToilData
	{
		// Token: 0x06004913 RID: 18707 RVA: 0x001C925D File Offset: 0x001C745D
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.interruptCurrentJob, "interruptCurrentJob", false, false);
		}

		// Token: 0x04002EEE RID: 12014
		public LocomotionUrgency locomotion;

		// Token: 0x04002EEF RID: 12015
		public bool canDig;

		// Token: 0x04002EF0 RID: 12016
		public bool interruptCurrentJob;
	}
}
