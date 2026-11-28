using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200207F RID: 8319
	public interface IGeneResourceDrain
	{
		// Token: 0x170025A2 RID: 9634
		// (get) Token: 0x0600D1CE RID: 53710
		Gene_Resource Resource { get; }

		// Token: 0x170025A3 RID: 9635
		// (get) Token: 0x0600D1CF RID: 53711
		bool CanOffset { get; }

		// Token: 0x170025A4 RID: 9636
		// (get) Token: 0x0600D1D0 RID: 53712
		float ResourceLossPerDay { get; }

		// Token: 0x170025A5 RID: 9637
		// (get) Token: 0x0600D1D1 RID: 53713
		Pawn Pawn { get; }

		// Token: 0x170025A6 RID: 9638
		// (get) Token: 0x0600D1D2 RID: 53714
		string DisplayLabel { get; }
	}
}
