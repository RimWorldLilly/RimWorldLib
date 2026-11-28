using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D21 RID: 3361
	public static class SiegeBlueprintPlacer
	{
		// Token: 0x06005E4D RID: 24141 RVA: 0x0022797C File Offset: 0x00225B7C
		public static IEnumerable<Blueprint_Build> PlaceBlueprints(IntVec3 placeCenter, Map map, Faction placeFaction, float points)
		{
			SiegeBlueprintPlacer.<PlaceBlueprints>d__8 <PlaceBlueprints>d__ = new SiegeBlueprintPlacer.<PlaceBlueprints>d__8(-2);
			<PlaceBlueprints>d__.<>3__placeCenter = placeCenter;
			<PlaceBlueprints>d__.<>3__map = map;
			<PlaceBlueprints>d__.<>3__placeFaction = placeFaction;
			<PlaceBlueprints>d__.<>3__points = points;
			return <PlaceBlueprints>d__;
		}

		// Token: 0x06005E4E RID: 24142 RVA: 0x002279A4 File Offset: 0x00225BA4
		private static bool CanPlaceBlueprintAt(IntVec3 root, Rot4 rot, ThingDef buildingDef, Map map, ThingDef stuffDef)
		{
			return GenConstruct.CanPlaceBlueprintAt_NewTemp(buildingDef, root, rot, map, false, null, null, stuffDef, true, true, true, (Thing t) => SiegeBlueprintPlacer.IsHostilePlanToIgnore(t, SiegeBlueprintPlacer.faction)).Accepted;
		}

		// Token: 0x06005E4F RID: 24143 RVA: 0x002279E9 File Offset: 0x00225BE9
		private static bool IsHostilePlanToIgnore(Thing t, Faction placingFaction)
		{
			return placingFaction != null && t != null && (t is Blueprint || t is Frame) && (t.Faction != null && t.Faction != placingFaction) && t.Faction.HostileTo(placingFaction);
		}

		// Token: 0x06005E50 RID: 24144 RVA: 0x00227A24 File Offset: 0x00225C24
		private static void ClearHostilePlansAt(IntVec3 root, Rot4 rot, ThingDef buildingDef, ThingDef stuffDef, Map map)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(root, rot, buildingDef.Size))
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = thingList.Count - 1; i >= 0; i--)
				{
					Thing thing = thingList[i];
					if (SiegeBlueprintPlacer.IsHostilePlanToIgnore(thing, SiegeBlueprintPlacer.faction) && !GenConstruct.CanPlaceBlueprintOver(buildingDef, thing.def, stuffDef, thing.Stuff))
					{
						thing.Destroy(DestroyMode.Cancel);
					}
				}
			}
		}

		// Token: 0x06005E51 RID: 24145 RVA: 0x00227ACC File Offset: 0x00225CCC
		private static IEnumerable<Blueprint_Build> PlaceCoverBlueprints(Map map)
		{
			SiegeBlueprintPlacer.<PlaceCoverBlueprints>d__12 <PlaceCoverBlueprints>d__ = new SiegeBlueprintPlacer.<PlaceCoverBlueprints>d__12(-2);
			<PlaceCoverBlueprints>d__.<>3__map = map;
			return <PlaceCoverBlueprints>d__;
		}

		// Token: 0x06005E52 RID: 24146 RVA: 0x00227ADC File Offset: 0x00225CDC
		private static IEnumerable<Blueprint_Build> MakeCoverLine(IntVec3 root, Map map, Rot4 growDir, int maxLength, ThingDef coverThing, ThingDef coverStuff)
		{
			SiegeBlueprintPlacer.<MakeCoverLine>d__13 <MakeCoverLine>d__ = new SiegeBlueprintPlacer.<MakeCoverLine>d__13(-2);
			<MakeCoverLine>d__.<>3__root = root;
			<MakeCoverLine>d__.<>3__map = map;
			<MakeCoverLine>d__.<>3__growDir = growDir;
			<MakeCoverLine>d__.<>3__maxLength = maxLength;
			<MakeCoverLine>d__.<>3__coverThing = coverThing;
			<MakeCoverLine>d__.<>3__coverStuff = coverStuff;
			return <MakeCoverLine>d__;
		}

		// Token: 0x06005E53 RID: 24147 RVA: 0x00227B11 File Offset: 0x00225D11
		private static IEnumerable<Blueprint_Build> PlaceArtilleryBlueprints(float points, Map map)
		{
			SiegeBlueprintPlacer.<PlaceArtilleryBlueprints>d__14 <PlaceArtilleryBlueprints>d__ = new SiegeBlueprintPlacer.<PlaceArtilleryBlueprints>d__14(-2);
			<PlaceArtilleryBlueprints>d__.<>3__points = points;
			<PlaceArtilleryBlueprints>d__.<>3__map = map;
			return <PlaceArtilleryBlueprints>d__;
		}

		// Token: 0x06005E54 RID: 24148 RVA: 0x00227B28 File Offset: 0x00225D28
		private static IntVec3 FindCoverRoot(Map map, ThingDef coverThing, ThingDef coverStuff)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 13);
			cellRect.ClipInsideMap(map);
			CellRect cellRect2 = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect2.ClipInsideMap(map);
			int num = 0;
			for (;;)
			{
				num++;
				if (num > 200)
				{
					break;
				}
				IntVec3 randomCell = cellRect.RandomCell;
				if (!cellRect2.Contains(randomCell) && map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly) && SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, Rot4.North, coverThing, map, coverStuff))
				{
					bool flag = false;
					for (int i = 0; i < SiegeBlueprintPlacer.placedCoverLocs.Count; i++)
					{
						if ((float)(SiegeBlueprintPlacer.placedCoverLocs[i] - randomCell).LengthHorizontalSquared < 36f)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						return randomCell;
					}
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06005E55 RID: 24149 RVA: 0x00227BFC File Offset: 0x00225DFC
		private static IntVec3 FindArtySpot(ThingDef artyDef, Rot4 rot, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect.ClipInsideMap(map);
			int num = 0;
			for (;;)
			{
				num++;
				if (num > 200)
				{
					break;
				}
				IntVec3 randomCell = cellRect.RandomCell;
				if (map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly) && !randomCell.Roofed(map) && SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, rot, artyDef, map, ThingDefOf.Steel))
				{
					return randomCell;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0400356B RID: 13675
		private static IntVec3 center;

		// Token: 0x0400356C RID: 13676
		private static Faction faction;

		// Token: 0x0400356D RID: 13677
		private static List<IntVec3> placedCoverLocs = new List<IntVec3>();

		// Token: 0x0400356E RID: 13678
		private const int MaxArtyCount = 2;

		// Token: 0x0400356F RID: 13679
		public const float ArtyCost = 60f;

		// Token: 0x04003570 RID: 13680
		private const int MinCoverDistSquared = 36;

		// Token: 0x04003571 RID: 13681
		private static readonly IntRange NumCoverRange = new IntRange(2, 4);

		// Token: 0x04003572 RID: 13682
		private static readonly IntRange CoverLengthRange = new IntRange(2, 7);
	}
}
