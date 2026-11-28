using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001717 RID: 5911
	public abstract class RitualSpectatorFilter
	{
		// Token: 0x060093A4 RID: 37796
		public abstract bool Allowed(Pawn p);

		// Token: 0x04005945 RID: 22853
		[MustTranslate]
		public string description;
	}
}
