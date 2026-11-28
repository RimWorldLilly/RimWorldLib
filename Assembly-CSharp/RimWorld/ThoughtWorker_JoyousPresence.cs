using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8C RID: 3724
	public class ThoughtWorker_JoyousPresence : ThoughtWorker
	{
		// Token: 0x06006262 RID: 25186 RVA: 0x00235C44 File Offset: 0x00233E44
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (p.MapHeld == null)
			{
				return false;
			}
			if (!p.IsPlayerControlled && !p.IsPrisonerOfColony)
			{
				return false;
			}
			List<Pawn> freeColonistsSpawned = p.MapHeld.mapPawns.FreeColonistsSpawned;
			for (int i = 0; i < freeColonistsSpawned.Count; i++)
			{
				Pawn pawn = freeColonistsSpawned[i];
				if (pawn != p && pawn.story.traits.HasTrait(TraitDefOf.Joyous) && !pawn.HostileTo(p))
				{
					return ThoughtState.ActiveWithReason(this.def.stages[0].description.Formatted(pawn.Named("PAWN")));
				}
			}
			return false;
		}
	}
}
