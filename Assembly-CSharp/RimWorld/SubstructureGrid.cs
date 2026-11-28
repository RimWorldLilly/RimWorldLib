using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200139C RID: 5020
	public class SubstructureGrid
	{
		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x06007C71 RID: 31857 RVA: 0x002C7428 File Offset: 0x002C5628
		public CellBoolDrawer Drawer
		{
			get
			{
				CellBoolDrawer result;
				if ((result = this.drawerInt) == null)
				{
					result = (this.drawerInt = new CellBoolDrawer(new Func<int, bool>(this.CellBoolDrawerGetBoolInt), new Func<Color>(this.CellBoolDrawerColorInt), new Func<int, Color>(this.CellBoolDrawerGetExtraColorInt), this.map.Size.x, this.map.Size.z, 3700, 0.33f));
				}
				return result;
			}
		}

		// Token: 0x06007C72 RID: 31858 RVA: 0x002C749B File Offset: 0x002C569B
		public SubstructureGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007C73 RID: 31859 RVA: 0x002C74BC File Offset: 0x002C56BC
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		// Token: 0x06007C74 RID: 31860 RVA: 0x002C74C3 File Offset: 0x002C56C3
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			TerrainDef terrainDef = this.map.terrainGrid.FoundationAt(index);
			return terrainDef != null && terrainDef.IsSubstructure;
		}

		// Token: 0x06007C75 RID: 31861 RVA: 0x002C74E4 File Offset: 0x002C56E4
		private Color CellBoolDrawerGetExtraColorInt(int index)
		{
			if (Find.TickManager.TicksGame != this.lastGotEnginesTick)
			{
				this.tmpEngines.Clear();
				this.tmpEngines.AddRange(this.map.listerBuildings.AllBuildingsColonistOfClass<Building_GravEngine>());
				this.lastGotEnginesTick = Find.TickManager.TicksGame;
			}
			TerrainDef terrainDef = this.map.terrainGrid.FoundationAt(index);
			if (terrainDef != null && terrainDef.IsSubstructure)
			{
				using (List<Building_GravEngine>.Enumerator enumerator = this.tmpEngines.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ValidSubstructureAt(this.map.cellIndices.IndexToCell(index)))
						{
							return GravshipUtility.ConnectedSubstructureColor;
						}
					}
				}
			}
			return GravshipUtility.DisconnectedSubstructureColor;
		}

		// Token: 0x06007C76 RID: 31862 RVA: 0x002C75C0 File Offset: 0x002C57C0
		public void DrawSubstructureGrid()
		{
			if (GravshipUtility.ShowConnectedSubstructure && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06007C77 RID: 31863 RVA: 0x002C75EC File Offset: 0x002C57EC
		public static void DrawSubstructureFootprint(List<IntVec3> extraCells = null, Thing thingToExclude = null)
		{
			if (Time.frameCount == SubstructureGrid.lastUpdateFrame)
			{
				return;
			}
			SubstructureGrid.lastUpdateFrame = Time.frameCount;
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsOfDef(ThingDefOf.GravEngine))
			{
				SubstructureGrid.tmpFootprintCells.Clear();
				SubstructureGrid.tmpFootprintCellsHashSet.Clear();
				SubstructureGrid.tmpGravEngineCells.Clear();
				SubstructureGrid.tmpGravEngineCellsHashSet.Clear();
				Building_GravEngine building_GravEngine = thing as Building_GravEngine;
				if (building_GravEngine != thingToExclude)
				{
					CompSubstructureFootprint compSubstructureFootprint = building_GravEngine.TryGetComp<CompSubstructureFootprint>();
					SubstructureGrid.tmpGravEngineCells.AddRange(GenRadial.RadialCellsAround(building_GravEngine.Position, compSubstructureFootprint.Props.radius, true));
					SubstructureGrid.tmpGravEngineCellsHashSet.AddRange(SubstructureGrid.tmpGravEngineCells);
					GenDraw.DrawFieldEdges(SubstructureGrid.tmpGravEngineCells, GravshipUtility.ConnectedSubstructureColor, null, null, 2900);
				}
				foreach (Thing thing2 in building_GravEngine.AffectedByFacilities.LinkedFacilitiesListForReading)
				{
					CompSubstructureFootprint compSubstructureFootprint2;
					if ((thingToExclude == null || thing2 != thingToExclude) && thing2.TryGetComp(out compSubstructureFootprint2) && compSubstructureFootprint2.Valid)
					{
						foreach (IntVec3 intVec in GenRadial.RadialCellsAround(thing2.Position, compSubstructureFootprint2.Props.radius, true))
						{
							if (intVec.InBounds(Find.CurrentMap) && !intVec.Fogged(Find.CurrentMap) && !SubstructureGrid.tmpGravEngineCellsHashSet.Contains(intVec) && !SubstructureGrid.tmpFootprintCellsHashSet.Contains(intVec))
							{
								SubstructureGrid.tmpFootprintCells.Add(intVec);
								SubstructureGrid.tmpFootprintCellsHashSet.Add(intVec);
							}
						}
					}
				}
				if (extraCells != null)
				{
					foreach (IntVec3 intVec2 in extraCells)
					{
						if (intVec2.InBounds(Find.CurrentMap) && !intVec2.Fogged(Find.CurrentMap) && !SubstructureGrid.tmpGravEngineCellsHashSet.Contains(intVec2) && !SubstructureGrid.tmpFootprintCellsHashSet.Contains(intVec2))
						{
							SubstructureGrid.tmpFootprintCells.Add(intVec2);
							SubstructureGrid.tmpFootprintCellsHashSet.Add(intVec2);
						}
					}
				}
				List<IntVec3> cells = SubstructureGrid.tmpFootprintCells;
				Color connectedSubstructureColor = GravshipUtility.ConnectedSubstructureColor;
				HashSet<IntVec3> ignoreBorderCells = SubstructureGrid.tmpGravEngineCellsHashSet;
				GenDraw.DrawFieldEdges(cells, connectedSubstructureColor, null, ignoreBorderCells, 2900);
			}
		}

		// Token: 0x06007C78 RID: 31864 RVA: 0x002C78D8 File Offset: 0x002C5AD8
		public static void DrawSubstructureCountOnGUI()
		{
			if (Time.frameCount == SubstructureGrid.lastGUIFrame)
			{
				return;
			}
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsOfDef(ThingDefOf.GravEngine))
			{
				Building_GravEngine building_GravEngine = thing as Building_GravEngine;
				int count = building_GravEngine.AllConnectedSubstructure.Count;
				float statValue = building_GravEngine.GetStatValue(StatDefOf.SubstructureSupport, true, -1);
				Vector2 screenPos = building_GravEngine.DrawPos.MapToUIPosition();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(count);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted<float>(statValue);
				Widgets.DrawStringOnMap(screenPos, defaultInterpolatedStringHandler.ToStringAndClear(), Color.white);
			}
		}

		// Token: 0x06007C79 RID: 31865 RVA: 0x002C799C File Offset: 0x002C5B9C
		public static void DrawSubstructureFootprintWithExtra(CompProperties_SubstructureFootprint footprint, IntVec3 center, Thing thing)
		{
			SubstructureGrid.tmpExtraFootprintCells.Clear();
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(center, footprint.radius, true))
			{
				if (intVec.InBounds(Find.CurrentMap) && !intVec.Fogged(Find.CurrentMap))
				{
					SubstructureGrid.tmpExtraFootprintCells.Add(intVec);
				}
			}
			SubstructureGrid.DrawSubstructureFootprint(SubstructureGrid.tmpExtraFootprintCells, thing);
		}

		// Token: 0x06007C7A RID: 31866 RVA: 0x002C7A24 File Offset: 0x002C5C24
		public void MarkDirty()
		{
			foreach (Building_GravEngine building_GravEngine in this.map.listerBuildings.AllBuildingsColonistOfClass<Building_GravEngine>())
			{
				building_GravEngine.substructureDirty = true;
			}
			this.Drawer.SetDirty();
		}

		// Token: 0x04004E50 RID: 20048
		private Map map;

		// Token: 0x04004E51 RID: 20049
		private CellBoolDrawer drawerInt;

		// Token: 0x04004E52 RID: 20050
		private List<Building_GravEngine> tmpEngines = new List<Building_GravEngine>();

		// Token: 0x04004E53 RID: 20051
		private int lastGotEnginesTick = -1;

		// Token: 0x04004E54 RID: 20052
		private static readonly List<IntVec3> tmpFootprintCells = new List<IntVec3>();

		// Token: 0x04004E55 RID: 20053
		private static readonly HashSet<IntVec3> tmpFootprintCellsHashSet = new HashSet<IntVec3>();

		// Token: 0x04004E56 RID: 20054
		private static readonly List<IntVec3> tmpGravEngineCells = new List<IntVec3>();

		// Token: 0x04004E57 RID: 20055
		private static readonly HashSet<IntVec3> tmpGravEngineCellsHashSet = new HashSet<IntVec3>();

		// Token: 0x04004E58 RID: 20056
		private static int lastUpdateFrame = -1;

		// Token: 0x04004E59 RID: 20057
		private static int lastGUIFrame = -1;

		// Token: 0x04004E5A RID: 20058
		private static readonly List<IntVec3> tmpExtraFootprintCells = new List<IntVec3>();
	}
}
