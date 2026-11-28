using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001189 RID: 4489
	public class IncidentWorker_MeteoriteImpact : IncidentWorker
	{
		// Token: 0x0600707D RID: 28797 RVA: 0x00275FDC File Offset: 0x002741DC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return IncidentWorker_MeteoriteImpact.TryFindCell(out intVec, map);
		}

		// Token: 0x0600707E RID: 28798 RVA: 0x00276000 File Offset: 0x00274200
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			List<Thing> contents;
			Skyfaller skyfaller = this.SpawnMeteoriteIncoming(parms, out contents);
			if (skyfaller == null)
			{
				return false;
			}
			skyfaller.impactLetter = this.MakeLetter(skyfaller, contents);
			return true;
		}

		// Token: 0x0600707F RID: 28799 RVA: 0x0027602B File Offset: 0x0027422B
		protected virtual List<Thing> GenerateMeteorContents(IncidentParms parms)
		{
			return ThingSetMakerDefOf.Meteorite.root.Generate();
		}

		// Token: 0x06007080 RID: 28800 RVA: 0x0027603C File Offset: 0x0027423C
		protected Skyfaller SpawnMeteoriteIncoming(IncidentParms parms, out List<Thing> contents)
		{
			Map map = (Map)parms.target;
			IntVec3 pos;
			if (!IncidentWorker_MeteoriteImpact.TryFindCell(out pos, map))
			{
				contents = null;
				return null;
			}
			contents = this.GenerateMeteorContents(parms);
			return SkyfallerMaker.SpawnSkyfaller(ThingDefOf.MeteoriteIncoming, contents, pos, map);
		}

		// Token: 0x06007081 RID: 28801 RVA: 0x0027607C File Offset: 0x0027427C
		protected virtual Letter MakeLetter(Skyfaller meteorite, List<Thing> contents)
		{
			Thing thing = (contents != null) ? contents[0] : null;
			if (thing == null)
			{
				return null;
			}
			LetterDef def = thing.def.building.isResourceRock ? LetterDefOf.PositiveEvent : LetterDefOf.NeutralEvent;
			string str = this.def.letterText.Formatted(thing.def.label).CapitalizeFirst();
			return LetterMaker.MakeLetter(this.def.letterLabel + ": " + thing.def.LabelCap, str, def, new TargetInfo(meteorite.Position, meteorite.Map, false), null, null, null);
		}

		// Token: 0x06007082 RID: 28802 RVA: 0x00276134 File Offset: 0x00274334
		private static bool TryFindCell(out IntVec3 cell, Map map)
		{
			int maxMineables = ThingSetMaker_Meteorite.MineablesCountRange.max;
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.MeteoriteIncoming, map, TerrainAffordanceDefOf.Light, out cell, 10, default(IntVec3), -1, true, false, false, false, true, true, delegate(IntVec3 x)
			{
				int num = Mathf.CeilToInt(Mathf.Sqrt((float)maxMineables)) + 2;
				CellRect other = CellRect.CenteredOn(x, num, num);
				int num2 = 0;
				foreach (IntVec3 c in other)
				{
					if (c.InBounds(map) && c.Standable(map))
					{
						num2++;
					}
				}
				if (ModsConfig.RoyaltyActive)
				{
					foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.MonumentMarker))
					{
						MonumentMarker monumentMarker = thing as MonumentMarker;
						if (monumentMarker.AllDone && monumentMarker.sketch.OccupiedRect.ExpandedBy(3).MovedBy(monumentMarker.Position).Overlaps(other))
						{
							return false;
						}
					}
				}
				return num2 >= maxMineables;
			});
		}
	}
}
