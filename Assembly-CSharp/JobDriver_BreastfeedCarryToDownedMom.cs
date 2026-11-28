using System;

// Token: 0x02000007 RID: 7
public class JobDriver_BreastfeedCarryToDownedMom : JobDriver_BreastfeedCarryToMom
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000027 RID: 39 RVA: 0x000026C8 File Offset: 0x000008C8
	protected override bool MomMustBeImmobile
	{
		get
		{
			return true;
		}
	}
}
