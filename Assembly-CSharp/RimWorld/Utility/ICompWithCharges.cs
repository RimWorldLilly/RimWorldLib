using System;

namespace RimWorld.Utility
{
	// Token: 0x020020AF RID: 8367
	public interface ICompWithCharges
	{
		// Token: 0x170025D9 RID: 9689
		// (get) Token: 0x0600D2DE RID: 53982
		int RemainingCharges { get; }

		// Token: 0x0600D2DF RID: 53983
		bool CanBeUsed(out string reason);
	}
}
