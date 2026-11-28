using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B25 RID: 2853
	public static class StealAIDebugDrawer
	{
		// Token: 0x06005423 RID: 21539 RVA: 0x001F4B6C File Offset: 0x001F2D6C
		public static void DebugDraw()
		{
			if (!DebugViewSettings.drawStealDebug)
			{
				StealAIDebugDrawer.debugDrawLord = null;
				return;
			}
			Lord lord = StealAIDebugDrawer.debugDrawLord;
			StealAIDebugDrawer.debugDrawLord = StealAIDebugDrawer.FindHostileLord();
			if (StealAIDebugDrawer.debugDrawLord == null)
			{
				return;
			}
			StealAIDebugDrawer.CheckInitDebugDrawGrid();
			float num = StealAIUtility.StartStealingMarketValueThreshold(StealAIDebugDrawer.debugDrawLord);
			if (lord != StealAIDebugDrawer.debugDrawLord)
			{
				foreach (IntVec3 intVec in Find.CurrentMap.AllCells)
				{
					StealAIDebugDrawer.debugDrawGrid[intVec] = (StealAIDebugDrawer.TotalMarketValueAround(intVec, Find.CurrentMap, StealAIDebugDrawer.debugDrawLord.ownedPawns.Count) > num);
				}
			}
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				if (StealAIDebugDrawer.debugDrawGrid[c])
				{
					CellRenderer.RenderCell(c, 0.5f);
				}
			}
			StealAIDebugDrawer.tmpToSteal.Clear();
			for (int i = 0; i < StealAIDebugDrawer.debugDrawLord.ownedPawns.Count; i++)
			{
				Pawn pawn = StealAIDebugDrawer.debugDrawLord.ownedPawns[i];
				Thing thing;
				if (StealAIUtility.TryFindBestItemToSteal(pawn.Position, pawn.Map, 7f, out thing, pawn, StealAIDebugDrawer.tmpToSteal))
				{
					GenDraw.DrawLineBetween(pawn.TrueCenter(), thing.TrueCenter());
					StealAIDebugDrawer.tmpToSteal.Add(thing);
				}
			}
			StealAIDebugDrawer.tmpToSteal.Clear();
		}

		// Token: 0x06005424 RID: 21540 RVA: 0x001F4CF8 File Offset: 0x001F2EF8
		public static void Notify_ThingChanged(Thing thing)
		{
			if (StealAIDebugDrawer.debugDrawLord == null)
			{
				return;
			}
			StealAIDebugDrawer.CheckInitDebugDrawGrid();
			if (thing.def.category != ThingCategory.Building && thing.def.category != ThingCategory.Item && thing.def.passability != Traversability.Impassable)
			{
				return;
			}
			if (thing.def.passability == Traversability.Impassable)
			{
				StealAIDebugDrawer.debugDrawLord = null;
				return;
			}
			int num = GenRadial.NumCellsInRadius(8f);
			float num2 = StealAIUtility.StartStealingMarketValueThreshold(StealAIDebugDrawer.debugDrawLord);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = thing.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(thing.Map))
				{
					StealAIDebugDrawer.debugDrawGrid[intVec] = (StealAIDebugDrawer.TotalMarketValueAround(intVec, Find.CurrentMap, StealAIDebugDrawer.debugDrawLord.ownedPawns.Count) > num2);
				}
			}
		}

		// Token: 0x06005425 RID: 21541 RVA: 0x001F4DC4 File Offset: 0x001F2FC4
		private static float TotalMarketValueAround(IntVec3 center, Map map, int pawnsCount)
		{
			if (center.Impassable(map))
			{
				return 0f;
			}
			float num = 0f;
			StealAIDebugDrawer.tmpToSteal.Clear();
			for (int i = 0; i < pawnsCount; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (!intVec.InBounds(map) || intVec.Impassable(map) || !GenSight.LineOfSight(center, intVec, map))
				{
					intVec = center;
				}
				Thing thing;
				if (StealAIUtility.TryFindBestItemToSteal(intVec, map, 7f, out thing, null, StealAIDebugDrawer.tmpToSteal))
				{
					num += StealAIUtility.GetValue(thing);
					StealAIDebugDrawer.tmpToSteal.Add(thing);
				}
			}
			StealAIDebugDrawer.tmpToSteal.Clear();
			return num;
		}

		// Token: 0x06005426 RID: 21542 RVA: 0x001F4E64 File Offset: 0x001F3064
		private static Lord FindHostileLord()
		{
			Lord lord = null;
			List<Lord> lords = Find.CurrentMap.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				if (lords[i].faction != null && lords[i].faction.HostileTo(Faction.OfPlayer) && (lord == null || lords[i].ownedPawns.Count > lord.ownedPawns.Count))
				{
					lord = lords[i];
				}
			}
			return lord;
		}

		// Token: 0x06005427 RID: 21543 RVA: 0x001F4EE4 File Offset: 0x001F30E4
		private static void CheckInitDebugDrawGrid()
		{
			if (StealAIDebugDrawer.debugDrawGrid == null)
			{
				StealAIDebugDrawer.debugDrawGrid = new BoolGrid(Find.CurrentMap);
				return;
			}
			if (!StealAIDebugDrawer.debugDrawGrid.MapSizeMatches(Find.CurrentMap))
			{
				StealAIDebugDrawer.debugDrawGrid.ClearAndResizeTo(Find.CurrentMap);
			}
		}

		// Token: 0x04003260 RID: 12896
		private static List<Thing> tmpToSteal = new List<Thing>();

		// Token: 0x04003261 RID: 12897
		private static BoolGrid debugDrawGrid;

		// Token: 0x04003262 RID: 12898
		private static Lord debugDrawLord = null;
	}
}
