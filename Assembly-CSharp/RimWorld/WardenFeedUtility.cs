using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BE6 RID: 3046
	public static class WardenFeedUtility
	{
		// Token: 0x0600575D RID: 22365 RVA: 0x00206350 File Offset: 0x00204550
		public static bool ShouldBeFed(Pawn p)
		{
			return p.IsPrisonerOfColony && p.InBed() && p.guest.CanBeBroughtFood && (p.Downed || HealthAIUtility.ShouldSeekMedicalRest(p));
		}
	}
}
