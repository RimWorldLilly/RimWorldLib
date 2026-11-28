using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020017CA RID: 6090
	public class HorrorRelaxTransition : MusicTransition
	{
		// Token: 0x0600980C RID: 38924 RVA: 0x003721D0 File Offset: 0x003703D0
		public override bool IsTransitionSatisfied()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!base.IsTransitionSatisfied())
			{
				return false;
			}
			foreach (Map map in Find.Maps)
			{
				if (map.weatherManager.curWeather == WeatherDefOf.GrayPall || map.gameConditionManager.ConditionIsActive(GameConditionDefOf.GrayPall))
				{
					return true;
				}
			}
			foreach (PocketMapParent pocketMapParent in Find.World.pocketMaps)
			{
				if (pocketMapParent.Map != null && HorrorRelaxTransition.IsValidPocketMap(pocketMapParent))
				{
					return true;
				}
			}
			using (List<Map>.Enumerator enumerator = Find.Maps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (HorrorRelaxTransition.IsValidMap(enumerator.Current))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600980D RID: 38925 RVA: 0x003722F8 File Offset: 0x003704F8
		private static bool IsValidPocketMap(PocketMapParent pocketMap)
		{
			return pocketMap.Map.generatorDef == MapGeneratorDefOf.Labyrinth || pocketMap.Map.generatorDef == MapGeneratorDefOf.Undercave;
		}

		// Token: 0x0600980E RID: 38926 RVA: 0x00372320 File Offset: 0x00370520
		private static bool IsValidMap(Map map)
		{
			return map.listerThings.AnyThingWithDef(ThingDefOf.PitGate) || map.listerThings.AnyThingWithDef(ThingDefOf.FleshmassHeart) || map.listerThings.AnyThingWithDef(ThingDefOf.Noctolith);
		}
	}
}
