using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200121F RID: 4639
	public class LayoutRoom : IExposable, ILoadReferenceable
	{
		// Token: 0x1700138D RID: 5005
		// (get) Token: 0x06007311 RID: 29457 RVA: 0x00284035 File Offset: 0x00282235
		public int Area
		{
			get
			{
				return this.rects.Sum((CellRect r) => r.Area);
			}
		}

		// Token: 0x1700138E RID: 5006
		// (get) Token: 0x06007312 RID: 29458 RVA: 0x00284064 File Offset: 0x00282264
		public CellRect Boundary
		{
			get
			{
				CellRect result = this.rects[0];
				for (int i = 1; i < this.rects.Count; i++)
				{
					result.Encapsulate(this.rects[i]);
				}
				return result;
			}
		}

		// Token: 0x1700138F RID: 5007
		// (get) Token: 0x06007313 RID: 29459 RVA: 0x002840A9 File Offset: 0x002822A9
		public bool DontDestroyWallsDoors
		{
			get
			{
				if (!this.defs.NullOrEmpty<LayoutRoomDef>())
				{
					return this.defs.Any((LayoutRoomDef d) => d.dontDestroyWallsDoors);
				}
				return false;
			}
		}

		// Token: 0x17001390 RID: 5008
		// (get) Token: 0x06007314 RID: 29460 RVA: 0x002840E4 File Offset: 0x002822E4
		public IEnumerable<IntVec3> Corners
		{
			get
			{
				return this.rects.SelectMany((CellRect r) => r.Corners);
			}
		}

		// Token: 0x17001391 RID: 5009
		// (get) Token: 0x06007315 RID: 29461 RVA: 0x00284110 File Offset: 0x00282310
		public string ThreatSignal
		{
			get
			{
				return this.threatSignal;
			}
		}

		// Token: 0x06007316 RID: 29462 RVA: 0x00284118 File Offset: 0x00282318
		public LayoutRoom()
		{
		}

		// Token: 0x06007317 RID: 29463 RVA: 0x00284144 File Offset: 0x00282344
		public LayoutRoom(LayoutStructureSketch sketch, List<CellRect> rects)
		{
			this.rects = rects;
			this.sketch = sketch;
			this.threatSignal = "RoomThreat" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			this.loadId = Find.UniqueIDsManager.GetNextRoomID();
		}

		// Token: 0x17001392 RID: 5010
		// (get) Token: 0x06007318 RID: 29464 RVA: 0x002841B8 File Offset: 0x002823B8
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				LayoutRoom.<get_Cells>d__24 <get_Cells>d__ = new LayoutRoom.<get_Cells>d__24(-2);
				<get_Cells>d__.<>4__this = this;
				return <get_Cells>d__;
			}
		}

		// Token: 0x06007319 RID: 29465 RVA: 0x002841C8 File Offset: 0x002823C8
		public bool TryGetRectOfSize(int minWidth, int minHeight, out CellRect rect)
		{
			foreach (CellRect cellRect in this.rects)
			{
				if (cellRect.AreSidesEqualOrGreater(minWidth, minHeight))
				{
					rect = cellRect;
					return true;
				}
			}
			rect = default(CellRect);
			return false;
		}

		// Token: 0x0600731A RID: 29466 RVA: 0x00284234 File Offset: 0x00282434
		public bool IsCorner(IntVec3 position)
		{
			for (int i = 0; i < this.rects.Count; i++)
			{
				if (this.rects[i].IsCorner(position))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600731B RID: 29467 RVA: 0x00284271 File Offset: 0x00282471
		public bool HasLayoutDef(LayoutRoomDef def)
		{
			return this.defs.Contains(def);
		}

		// Token: 0x0600731C RID: 29468 RVA: 0x00284280 File Offset: 0x00282480
		public bool IsAdjacentTo(LayoutRoom room, int minAdjacencyScore = 1)
		{
			foreach (CellRect cellRect in this.rects)
			{
				foreach (CellRect other in room.rects)
				{
					if (cellRect.ContractedBy(1).GetAdjacencyScore(other) >= minAdjacencyScore)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600731D RID: 29469 RVA: 0x00284324 File Offset: 0x00282524
		public bool Contains(IntVec3 position, int contractedBy = 0)
		{
			foreach (CellRect cellRect in this.rects)
			{
				if (cellRect.ContractedBy(contractedBy).Contains(position))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600731E RID: 29470 RVA: 0x0028438C File Offset: 0x0028258C
		public void SpawnRectTriggersForAction(SignalAction action, Map map)
		{
			foreach (CellRect rect in this.rects)
			{
				RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
				rectTrigger.signalTag = action.signalTag;
				rectTrigger.Rect = rect;
				GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
			}
		}

		// Token: 0x0600731F RID: 29471 RVA: 0x0028440C File Offset: 0x0028260C
		public bool TryGetRandomCellInRoom(ThingDef def, Map map, out IntVec3 cell, Rot4? rot = null, int contractedBy = 0, int padding = 0, Func<IntVec3, bool> validator = null, bool ignoreBuildings = false)
		{
			Rot4 rot2 = rot ?? Rot4.North;
			foreach (CellRect cellRect in this.rects.InRandomOrder(null))
			{
				foreach (IntVec3 intVec in cellRect.ContractedBy(contractedBy).Cells.InRandomOrder(null))
				{
					CellRect cellRect2 = intVec.RectAbout(def.size, rot2);
					if (GenSpawn.CanSpawnAt(def, intVec, map, rot, ignoreBuildings) && (!def.hasInteractionCell || ThingUtility.InteractionCellWhenAt(def, intVec, rot2, map).Standable(map)))
					{
						bool flag = false;
						foreach (IntVec3 intVec2 in cellRect2.ExpandedBy(padding))
						{
							flag = false;
							if (!intVec2.InBounds(map))
							{
								break;
							}
							if (!this.Contains(intVec2, 0))
							{
								break;
							}
							if (!ignoreBuildings && intVec2.GetEdifice(map) != null)
							{
								break;
							}
							if (!ignoreBuildings && intVec2.GetFirstBuilding(map) != null)
							{
								break;
							}
							if (!ignoreBuildings && def.passability != Traversability.Standable && RoomGenUtility.IsDoorAdjacentTo(intVec2, map, true))
							{
								break;
							}
							if (intVec2.GetFirstPawn(map) != null)
							{
								break;
							}
							if (validator != null && !validator(intVec2))
							{
								break;
							}
							flag = true;
						}
						if (flag)
						{
							cell = intVec;
							return true;
						}
					}
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06007320 RID: 29472 RVA: 0x00284614 File Offset: 0x00282814
		public bool TryGetRandomCellInRoom(Map map, out IntVec3 cell, int contractedBy = 0, int padding = 0, Func<IntVec3, bool> validator = null, bool ignoreBuildings = false)
		{
			foreach (CellRect cellRect in this.rects.InRandomOrder(null))
			{
				foreach (IntVec3 intVec in cellRect.ContractedBy(contractedBy).Cells.InRandomOrder(null))
				{
					bool flag = false;
					foreach (IntVec3 intVec2 in CellRect.FromCell(intVec).ExpandedBy(padding).ExpandedBy(padding))
					{
						flag = false;
						if (!intVec2.InBounds(map))
						{
							break;
						}
						if (!this.Contains(intVec2, 0))
						{
							break;
						}
						if (!ignoreBuildings && intVec2.GetEdifice(map) != null)
						{
							break;
						}
						if (!ignoreBuildings && intVec2.GetFirstBuilding(map) != null)
						{
							break;
						}
						if (intVec2.GetFirstPawn(map) != null)
						{
							break;
						}
						if (validator != null && !validator(intVec2))
						{
							break;
						}
						flag = true;
					}
					if (flag)
					{
						cell = intVec;
						return true;
					}
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06007321 RID: 29473 RVA: 0x002847AC File Offset: 0x002829AC
		public bool TryGetRectContainingCell(IntVec3 cell, out CellRect rect)
		{
			foreach (CellRect cellRect in this.rects)
			{
				if (cellRect.Contains(cell))
				{
					rect = cellRect;
					return true;
				}
			}
			rect = CellRect.Empty;
			return false;
		}

		// Token: 0x06007322 RID: 29474 RVA: 0x0028481C File Offset: 0x00282A1C
		public bool TryGetRandomCellInRoom(out IntVec3 cell, int contractedBy = 0, Func<IntVec3, bool> validator = null)
		{
			for (int i = 0; i < this.rects.Count; i++)
			{
				foreach (IntVec3 intVec in this.rects[i].ContractedBy(contractedBy).Cells)
				{
					if (validator == null || validator(intVec))
					{
						LayoutRoom.cells.Add(intVec);
					}
				}
			}
			if (LayoutRoom.cells.Count == 0)
			{
				cell = IntVec3.Invalid;
				return false;
			}
			cell = LayoutRoom.cells.RandomElement<IntVec3>();
			LayoutRoom.cells.Clear();
			return true;
		}

		// Token: 0x06007323 RID: 29475 RVA: 0x002848DC File Offset: 0x00282ADC
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.noExteriorDoors, "noExteriorDoors", false, false);
			Scribe_Values.Look<int>(ref this.loadId, "loadId", 0, false);
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<string>(ref this.threatSignal, "threatSignal", null, false);
			Scribe_Defs.Look<LayoutRoomDef>(ref this.requiredDef, "requiredDef");
			Scribe_References.Look<LayoutStructureSketch>(ref this.sketch, "sketch", false);
			Scribe_Collections.Look<LayoutRoomDef>(ref this.defs, "defs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<LayoutRoom>(ref this.merged, "merged", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<CellRect>(ref this.rects, "rects", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.entryCells, "entryCells", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.loadId == 0)
			{
				this.loadId = Rand.Int;
			}
		}

		// Token: 0x06007324 RID: 29476 RVA: 0x002849C8 File Offset: 0x00282BC8
		public string GetUniqueLoadID()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
			defaultInterpolatedStringHandler.AppendLiteral("LayoutRoom_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.loadId);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040048C5 RID: 18629
		public List<CellRect> rects;

		// Token: 0x040048C6 RID: 18630
		public List<LayoutRoomDef> defs = new List<LayoutRoomDef>();

		// Token: 0x040048C7 RID: 18631
		public List<LayoutRoom> merged = new List<LayoutRoom>();

		// Token: 0x040048C8 RID: 18632
		public List<IntVec3> entryCells;

		// Token: 0x040048C9 RID: 18633
		public LayoutRoomDef requiredDef;

		// Token: 0x040048CA RID: 18634
		public LayoutStructureSketch sketch;

		// Token: 0x040048CB RID: 18635
		public bool noExteriorDoors;

		// Token: 0x040048CC RID: 18636
		private string threatSignal;

		// Token: 0x040048CD RID: 18637
		public int loadId;

		// Token: 0x040048CE RID: 18638
		public int id;

		// Token: 0x040048CF RID: 18639
		public readonly List<LayoutRoom> connections = new List<LayoutRoom>();

		// Token: 0x040048D0 RID: 18640
		private static readonly HashSet<IntVec3> cells = new HashSet<IntVec3>();
	}
}
