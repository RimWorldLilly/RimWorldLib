using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200118E RID: 4494
	public abstract class IncidentWorker_PsychicEmanation : IncidentWorker
	{
		// Token: 0x06007092 RID: 28818 RVA: 0x002766C4 File Offset: 0x002748C4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) && !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicSoothe) && map.listerThings.ThingsOfDef(ThingDefOf.PsychicDronerShipPart).Count <= 0 && map.mapPawns.FreeColonistsCount != 0;
		}

		// Token: 0x06007093 RID: 28819 RVA: 0x00276730 File Offset: 0x00274930
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.DoConditionAndLetter(parms, map, Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f), map.mapPawns.FreeColonists.RandomElement<Pawn>().gender, parms.points);
			return true;
		}

		// Token: 0x06007094 RID: 28820
		protected abstract void DoConditionAndLetter(IncidentParms parms, Map map, int duration, Gender gender, float points);
	}
}
