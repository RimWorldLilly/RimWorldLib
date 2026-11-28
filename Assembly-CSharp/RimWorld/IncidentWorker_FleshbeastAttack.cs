using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001177 RID: 4471
	public class IncidentWorker_FleshbeastAttack : IncidentWorker
	{
		// Token: 0x06007038 RID: 28728 RVA: 0x00274A6A File Offset: 0x00272C6A
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Map map = (Map)parms.target;
			return base.CanFireNowSub(parms);
		}

		// Token: 0x06007039 RID: 28729 RVA: 0x00274A88 File Offset: 0x00272C88
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			float num = parms.points * 0.6f;
			List<Thing> list = new List<Thing>();
			int num2 = 0;
			while (num > 0f)
			{
				IntVec3 cell;
				if (!LargeBuildingCellFinder.TryFindCell(out cell, map, IncidentWorker_FleshbeastAttack.BurrowSpawnParms.ForThing(ThingDefOf.PitBurrow), null, null, false))
				{
					return false;
				}
				float num3 = Mathf.Min(num, 500f);
				Thing item = FleshbeastUtility.SpawnFleshbeastsFromPitBurrowEmergence(cell, map, num3, IncidentWorker_FleshbeastAttack.PitBurrowEmergenceDelayRangeTicks, IncidentWorker_FleshbeastAttack.FleshbeastSpawnDelayTicks, true);
				list.Add(item);
				num -= num3;
				num2++;
				if (num2 > 100)
				{
					break;
				}
			}
			base.SendStandardLetter(this.def.letterLabel, (list.Count > 1) ? this.def.letterTextPlural : this.def.letterText, this.def.letterDef, parms, list, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x040047A3 RID: 18339
		private const int MaxIterations = 100;

		// Token: 0x040047A4 RID: 18340
		private const float FleshbeastPointsFactor = 0.6f;

		// Token: 0x040047A5 RID: 18341
		private static readonly IntRange FleshbeastSpawnDelayTicks = new IntRange(180, 180);

		// Token: 0x040047A6 RID: 18342
		private static readonly IntRange PitBurrowEmergenceDelayRangeTicks = new IntRange(420, 420);

		// Token: 0x040047A7 RID: 18343
		private static readonly LargeBuildingSpawnParms BurrowSpawnParms = new LargeBuildingSpawnParms
		{
			maxDistanceToColonyBuilding = -1f,
			minDistToEdge = 10,
			attemptNotUnderBuildings = true,
			canSpawnOnImpassable = false,
			attemptSpawnLocationType = SpawnLocationType.Outdoors
		};
	}
}
