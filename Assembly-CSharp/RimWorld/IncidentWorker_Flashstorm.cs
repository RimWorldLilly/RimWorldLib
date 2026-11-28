using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001176 RID: 4470
	public class IncidentWorker_Flashstorm : IncidentWorker
	{
		// Token: 0x06007035 RID: 28725 RVA: 0x0027497E File Offset: 0x00272B7E
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return !((Map)parms.target).gameConditionManager.ConditionIsActive(GameConditionDefOf.Flashstorm);
		}

		// Token: 0x06007036 RID: 28726 RVA: 0x002749A0 File Offset: 0x00272BA0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
			GameCondition_Flashstorm gameCondition_Flashstorm = (GameCondition_Flashstorm)GameConditionMaker.MakeCondition(GameConditionDefOf.Flashstorm, duration);
			map.gameConditionManager.RegisterCondition(gameCondition_Flashstorm);
			base.SendStandardLetter(this.def.letterLabel, GameConditionDefOf.Flashstorm.letterText, this.def.letterDef, parms, new TargetInfo(gameCondition_Flashstorm.centerLocation.ToIntVec3, map, false), Array.Empty<NamedArgument>());
			if (map.weatherManager.curWeather.rainRate > 0.1f)
			{
				map.weatherDecider.StartNextWeather();
			}
			return true;
		}
	}
}
