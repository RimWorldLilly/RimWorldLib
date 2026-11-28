using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E06 RID: 3590
	public class ThoughtWorker_ImprisonedByFaction : ThoughtWorker
	{
		// Token: 0x060060F7 RID: 24823 RVA: 0x002312E3 File Offset: 0x0022F4E3
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			return p.IsPrisoner && p.guest.HostFaction == other.Faction;
		}
	}
}
