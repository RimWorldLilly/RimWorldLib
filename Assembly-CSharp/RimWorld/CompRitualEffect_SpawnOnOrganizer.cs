using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001748 RID: 5960
	public class CompRitualEffect_SpawnOnOrganizer : CompRitualEffect_SpawnOnPawn
	{
		// Token: 0x06009454 RID: 37972 RVA: 0x00360F68 File Offset: 0x0035F168
		protected override Pawn GetPawn(LordJob_Ritual ritual)
		{
			return ritual.Organizer;
		}
	}
}
