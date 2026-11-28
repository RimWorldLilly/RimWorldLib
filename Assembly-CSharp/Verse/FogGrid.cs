using System;
using System.Collections.Generic;
using RimWorld;
using Unity.Collections;

namespace Verse
{
	// Token: 0x02000270 RID: 624
	public sealed class FogGrid : IExposable, IDisposable
	{
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x0600121C RID: 4636 RVA: 0x00067D1D File Offset: 0x00065F1D
		internal NativeBitArray FogGrid_Unsafe
		{
			get
			{
				return this.fogGrid;
			}
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00067D25 File Offset: 0x00065F25
		public FogGrid(Map map)
		{
			this.map = map;
			this.fogGrid = new NativeBitArray(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00067D51 File Offset: 0x00065F51
		public void ExposeData()
		{
			DataExposeUtility.LookBitArray(ref this.fogGrid, this.map.Area, "fogGrid");
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x00067D70 File Offset: 0x00065F70
		public void Unfog(IntVec3 c)
		{
			this.UnfogWorker(c);
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing.def.Fillage == FillCategory.Full)
				{
					foreach (IntVec3 c2 in thing.OccupiedRect().Cells)
					{
						this.UnfogWorker(c2);
					}
				}
				thingList[i].Notify_Unfogged();
			}
			this.map.events.Notify_CellFogChanged(c, false);
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00067E24 File Offset: 0x00066024
		private void UnfogWorker(IntVec3 c)
		{
			int pos = this.map.cellIndices.CellToIndex(c);
			if (!this.fogGrid.IsSet(pos))
			{
				return;
			}
			this.fogGrid.Set(pos, false);
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.FogOfWar | MapMeshFlagDefOf.Things);
			}
			Designation designation = this.map.designationManager.DesignationAt(c, DesignationDefOf.Mine);
			if (designation != null && c.GetFirstMineable(this.map) == null)
			{
				designation.Delete();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.roofGrid.Drawer.SetDirty();
				this.map.mapTemperature.Drawer.SetDirty();
			}
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00067EED File Offset: 0x000660ED
		public bool IsFogged(IntVec3 c)
		{
			return this.fogGrid.IsCreated && c.InBounds(this.map) && this.fogGrid.IsSet(this.map.cellIndices.CellToIndex(c));
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00067F28 File Offset: 0x00066128
		public bool IsFogged(int index)
		{
			return this.fogGrid.IsCreated && this.fogGrid.IsSet(index);
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x00067F48 File Offset: 0x00066148
		public void ClearAllFog()
		{
			for (int i = 0; i < this.map.Size.x; i++)
			{
				for (int j = 0; j < this.map.Size.z; j++)
				{
					this.Unfog(new IntVec3(i, 0, j));
				}
			}
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x00067F9C File Offset: 0x0006619C
		public void Refog(CellRect rect)
		{
			foreach (IntVec3 intVec in rect)
			{
				if (intVec.InBounds(this.map))
				{
					int pos = this.map.cellIndices.CellToIndex(intVec);
					if (!this.fogGrid.IsSet(pos))
					{
						this.fogGrid.Set(pos, true);
						if (Current.ProgramState == ProgramState.Playing)
						{
							this.map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.FogOfWar | MapMeshFlagDefOf.Things);
						}
						this.map.events.Notify_CellFogChanged(intVec, true);
					}
				}
			}
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x00068068 File Offset: 0x00066268
		public void Notify_FogBlockerRemoved(Thing thing)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			bool flag = false;
			foreach (IntVec3 c in GenAdj.CellsAdjacent8Way(thing))
			{
				if (c.InBounds(this.map))
				{
					Building edifice = c.GetEdifice(this.map);
					if (!this.IsFogged(c) && ((edifice != null && edifice.def.IsDoor) || (edifice == null || !edifice.def.MakeFog)))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			this.FloodUnfogAdjacent(thing, !this.map.generatorDef.ignoreAreaRevealedLetter);
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x00068128 File Offset: 0x00066328
		public void Notify_PawnEnteringDoor(Building_Door door, Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				return;
			}
			this.FloodUnfogAdjacent(door.Position, false);
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00068154 File Offset: 0x00066354
		internal void SetAllFogged()
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.fogGrid.Set(cellIndices.CellToIndex(c), true);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.map.roofGrid.Drawer.SetDirty();
			}
			this.map.events.Notify_MapFogged();
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x000681EC File Offset: 0x000663EC
		public void FloodUnfogAdjacent(IntVec3 c, bool sendLetters = true)
		{
			this.Unfog(c);
			bool flag = false;
			FloodUnfogResult unfogResult = default(FloodUnfogResult);
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = c + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(this.map) && intVec.Fogged(this.map))
				{
					Building edifice = intVec.GetEdifice(this.map);
					if (edifice == null || !edifice.def.MakeFog)
					{
						flag = true;
						unfogResult = FloodFillerFog.FloodUnfog(intVec, this.map);
					}
					else
					{
						this.Unfog(intVec);
					}
				}
			}
			for (int j = 0; j < 8; j++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCells[j];
				if (c2.InBounds(this.map))
				{
					Building edifice2 = c2.GetEdifice(this.map);
					if (edifice2 != null && edifice2.def.MakeFog)
					{
						this.Unfog(c2);
					}
				}
			}
			if (flag && sendLetters)
			{
				this.NotifyAreaRevealed(c, unfogResult);
			}
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x000682E8 File Offset: 0x000664E8
		public void FloodUnfogAdjacent(Thing thing, bool sendLetters = true)
		{
			this.Unfog(thing.Position);
			bool flag = false;
			FloodUnfogResult unfogResult = default(FloodUnfogResult);
			foreach (IntVec3 intVec in GenAdj.CellsAdjacentCardinal(thing))
			{
				if (intVec.InBounds(this.map) && intVec.Fogged(this.map))
				{
					Building edifice = intVec.GetEdifice(this.map);
					if (edifice == null || !edifice.def.MakeFog)
					{
						flag = true;
						unfogResult = FloodFillerFog.FloodUnfog(intVec, this.map);
					}
					else
					{
						this.Unfog(intVec);
					}
				}
			}
			foreach (IntVec3 c in GenAdj.CellsAdjacent8Way(thing))
			{
				if (c.InBounds(this.map))
				{
					Building edifice2 = c.GetEdifice(this.map);
					if (edifice2 != null && edifice2.def.MakeFog)
					{
						this.Unfog(c);
					}
				}
			}
			if (flag && sendLetters)
			{
				this.NotifyAreaRevealed(thing.Position, unfogResult);
			}
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x00068418 File Offset: 0x00066618
		private void NotifyAreaRevealed(IntVec3 c, FloodUnfogResult unfogResult)
		{
			if (unfogResult.mechanoidFound)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelAreaRevealed".Translate(), "AreaRevealedWithMechanoids".Translate(), LetterDefOf.ThreatBig, new TargetInfo(c, this.map, false), null, null, null, null, 0, true);
				return;
			}
			if (!unfogResult.allOnScreen || unfogResult.cellsUnfogged >= 600)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelAreaRevealed".Translate(), "AreaRevealed".Translate(), LetterDefOf.NeutralEvent, new TargetInfo(c, this.map, false), null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x000684B9 File Offset: 0x000666B9
		public void Dispose()
		{
			this.fogGrid.Dispose();
		}

		// Token: 0x04001073 RID: 4211
		private readonly Map map;

		// Token: 0x04001074 RID: 4212
		private NativeBitArray fogGrid;

		// Token: 0x04001075 RID: 4213
		private const int AlwaysSendLetterIfUnfoggedMoreCellsThan = 600;
	}
}
