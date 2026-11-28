using System;
using Verse;

namespace RimWorld.Utility
{
	// Token: 0x020020B0 RID: 8368
	public interface IReloadableComp : ICompWithCharges
	{
		// Token: 0x170025DA RID: 9690
		// (get) Token: 0x0600D2E0 RID: 53984
		Thing ReloadableThing { get; }

		// Token: 0x170025DB RID: 9691
		// (get) Token: 0x0600D2E1 RID: 53985
		ThingDef AmmoDef { get; }

		// Token: 0x170025DC RID: 9692
		// (get) Token: 0x0600D2E2 RID: 53986
		int BaseReloadTicks { get; }

		// Token: 0x170025DD RID: 9693
		// (get) Token: 0x0600D2E3 RID: 53987
		int MaxCharges { get; }

		// Token: 0x170025DE RID: 9694
		// (get) Token: 0x0600D2E4 RID: 53988
		string LabelRemaining { get; }

		// Token: 0x0600D2E5 RID: 53989
		bool NeedsReload(bool allowForceReload);

		// Token: 0x0600D2E6 RID: 53990
		int MinAmmoNeeded(bool allowForcedReload);

		// Token: 0x0600D2E7 RID: 53991
		int MaxAmmoNeeded(bool allowForcedReload);

		// Token: 0x0600D2E8 RID: 53992
		int MaxAmmoAmount();

		// Token: 0x0600D2E9 RID: 53993
		void ReloadFrom(Thing ammo);

		// Token: 0x0600D2EA RID: 53994
		string DisabledReason(int minNeeded, int maxNeeded);
	}
}
