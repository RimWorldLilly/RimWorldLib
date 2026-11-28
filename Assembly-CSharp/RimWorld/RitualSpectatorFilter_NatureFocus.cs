using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001718 RID: 5912
	public class RitualSpectatorFilter_NatureFocus : RitualSpectatorFilter
	{
		// Token: 0x060093A6 RID: 37798 RVA: 0x0035E1F9 File Offset: 0x0035C3F9
		public override bool Allowed(Pawn p)
		{
			return MeditationFocusDefOf.Natural.CanPawnUse(p);
		}
	}
}
