using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8D RID: 3725
	public class ThoughtWorker_SwallowedByDarkness : ThoughtWorker
	{
		// Token: 0x06006264 RID: 25188 RVA: 0x00235D12 File Offset: 0x00233F12
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return ModsConfig.AnomalyActive && ThoughtWorker_SwallowedByDarkness.InDarkness(p) && p.MapHeld.GameConditionManager.MapBrightness < 0.5f;
		}

		// Token: 0x06006265 RID: 25189 RVA: 0x00235D42 File Offset: 0x00233F42
		public static bool InDarkness(Pawn pawn)
		{
			return pawn.SpawnedOrAnyParentSpawned && !pawn.Dead && !pawn.Suspended && pawn.MapHeld.glowGrid.GroundGlowAt(pawn.PositionHeld, false, false) <= 0f;
		}
	}
}
