using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000289 RID: 649
	public sealed class TerrainGrid : IExposable
	{
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x0006D838 File Offset: 0x0006BA38
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(new Func<int, bool>(this.CellBoolDrawerGetBoolInt), new Func<Color>(this.CellBoolDrawerColorInt), new Func<int, Color>(this.CellBoolDrawerGetExtraColorInt), this.map.Size.x, this.map.Size.z, 3600, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001368 RID: 4968 RVA: 0x0006D8AC File Offset: 0x0006BAAC
		public bool AnyWaterCells
		{
			get
			{
				return this.waterCells.Count > 0;
			}
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x0006D8BC File Offset: 0x0006BABC
		public TerrainGrid(Map map)
		{
			this.map = map;
			this.ResetGrids();
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0006D8DC File Offset: 0x0006BADC
		public void ResetGrids()
		{
			this.topGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
			this.underGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
			this.foundationGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
			this.tempGrid = new TerrainDef[this.map.cellIndices.NumGridCells];
			this.colorGrid = new ColorDef[this.map.cellIndices.NumGridCells];
			this.waterCells = new HashSet<IntVec3>();
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0006D97C File Offset: 0x0006BB7C
		public TerrainDef TerrainAt(int ind)
		{
			if (this.tempGrid[ind] != null)
			{
				return this.tempGrid[ind];
			}
			if (this.underGrid[ind] == null && this.foundationGrid[ind] != null)
			{
				return this.foundationGrid[ind];
			}
			if (this.topGrid[ind] == null)
			{
				return TerrainDefOf.Soil;
			}
			return this.topGrid[ind];
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x0006D9D1 File Offset: 0x0006BBD1
		public TerrainDef TerrainAtIgnoreTemp(int ind)
		{
			if (this.underGrid[ind] == null && this.foundationGrid[ind] != null)
			{
				return this.foundationGrid[ind];
			}
			if (this.topGrid[ind] == null)
			{
				return TerrainDefOf.Soil;
			}
			return this.topGrid[ind];
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x0006DA08 File Offset: 0x0006BC08
		public TerrainDef TerrainAt(IntVec3 c)
		{
			return this.TerrainAt(this.map.cellIndices.CellToIndex(c));
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x0006DA24 File Offset: 0x0006BC24
		public TerrainDef BaseTerrainAt(IntVec3 c)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (this.underGrid[num] != null)
			{
				return this.underGrid[num];
			}
			if (this.topGrid[num] == null)
			{
				return TerrainDefOf.Soil;
			}
			return this.topGrid[num];
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x0006DA6E File Offset: 0x0006BC6E
		public TerrainDef TopTerrainAt(int ind)
		{
			if (this.topGrid[ind] == null)
			{
				return TerrainDefOf.Soil;
			}
			return this.topGrid[ind];
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x0006DA88 File Offset: 0x0006BC88
		public TerrainDef TopTerrainAt(IntVec3 c)
		{
			if (this.topGrid[this.map.cellIndices.CellToIndex(c)] == null)
			{
				return TerrainDefOf.Soil;
			}
			return this.topGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0006DAC2 File Offset: 0x0006BCC2
		public TerrainDef UnderTerrainAt(int ind)
		{
			return this.underGrid[ind];
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x0006DACC File Offset: 0x0006BCCC
		public TerrainDef UnderTerrainAt(IntVec3 c)
		{
			return this.underGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x0006DAE6 File Offset: 0x0006BCE6
		public TerrainDef FoundationAt(int ind)
		{
			return this.foundationGrid[ind];
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0006DAF0 File Offset: 0x0006BCF0
		public TerrainDef FoundationAt(IntVec3 c)
		{
			return this.foundationGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x0006DB0A File Offset: 0x0006BD0A
		public TerrainDef TempTerrainAt(int ind)
		{
			return this.tempGrid[ind];
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x0006DB14 File Offset: 0x0006BD14
		public TerrainDef TempTerrainAt(IntVec3 c)
		{
			return this.tempGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x0006DB2E File Offset: 0x0006BD2E
		public ColorDef ColorAt(IntVec3 c)
		{
			return this.ColorAt(this.map.cellIndices.CellToIndex(c));
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0006DB47 File Offset: 0x0006BD47
		public bool WaterAt(IntVec3 c)
		{
			return this.waterCells.Contains(c);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0006DB55 File Offset: 0x0006BD55
		public ColorDef ColorAt(int ind)
		{
			if (this.tempGrid[ind] != null)
			{
				return null;
			}
			return this.colorGrid[ind];
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0006DB6C File Offset: 0x0006BD6C
		public Material GetMaterial(TerrainDef def, bool polluted, ColorDef color)
		{
			ValueTuple<TerrainDef, bool, ColorDef> key = new ValueTuple<TerrainDef, bool, ColorDef>(def, polluted, color);
			if (!this.terrainMatCache.ContainsKey(key))
			{
				Graphic graphic = polluted ? def.graphicPolluted : def.graphic;
				if (color != null)
				{
					this.terrainMatCache[key] = graphic.GetColoredVersion(def.graphic.Shader, color.color, Color.white).MatSingle;
				}
				else
				{
					this.terrainMatCache[key] = (polluted ? def.DrawMatPolluted : def.DrawMatSingle);
				}
			}
			return this.terrainMatCache[key];
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0006DC00 File Offset: 0x0006BE00
		public void SetTerrain(IntVec3 c, TerrainDef newTerr)
		{
			if (newTerr == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to set terrain at ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(c);
				defaultInterpolatedStringHandler.AppendLiteral(" to null.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			if (!c.InBounds(this.map))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(54, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Tried to set terrain in out of bounds cell ");
				defaultInterpolatedStringHandler2.AppendFormatted<IntVec3>(c);
				defaultInterpolatedStringHandler2.AppendLiteral(" (bounds: ");
				defaultInterpolatedStringHandler2.AppendFormatted<IntVec3>(this.map.Size);
				defaultInterpolatedStringHandler2.AppendLiteral(")");
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				return;
			}
			if (newTerr.temporary)
			{
				this.SetTempTerrain(c, newTerr);
				return;
			}
			if (newTerr.isFoundation)
			{
				this.SetFoundation(c, newTerr);
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (newTerr.layerable)
			{
				if (this.underGrid[num] == null)
				{
					if (this.topGrid[num] != null && this.topGrid[num].passability != Traversability.Impassable)
					{
						this.underGrid[num] = this.topGrid[num];
					}
					else
					{
						this.underGrid[num] = (this.map.generatorDef.defaultUnderGridTerrain ?? TerrainDefOf.Sand);
					}
				}
			}
			else
			{
				this.underGrid[num] = null;
			}
			TerrainDef oldTerr = null;
			if (this.topGrid[num] != null)
			{
				if (this.topGrid[num].glowRadius > 0f)
				{
					this.map.glowGrid.DeregisterTerrain(c);
				}
				if (this.topGrid[num].IsWater)
				{
					this.waterCells.Remove(c);
				}
				oldTerr = this.topGrid[num];
			}
			this.topGrid[num] = newTerr;
			this.colorGrid[num] = null;
			if (newTerr.glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			if (newTerr.IsWater)
			{
				this.waterCells.Add(c);
			}
			this.DoTerrainChangedEffects(c, oldTerr, newTerr, null);
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x0006DDF0 File Offset: 0x0006BFF0
		public void SetUnderTerrain(IntVec3 c, TerrainDef newTerr)
		{
			if (!c.InBounds(this.map))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to set terrain out of bounds at ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(c);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (this.underGrid[num] != null && this.underGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(c);
			}
			this.underGrid[num] = newTerr;
			if (newTerr.glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			this.map.events.Notify_TerrainChanged(c);
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0006DEB0 File Offset: 0x0006C0B0
		public void RemoveTopLayer(IntVec3 c, bool doLeavings = true)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			TerrainDef oldTerr = this.TerrainAt(c);
			if (this.tempGrid[num] != null)
			{
				this.RemoveTempTerrain(c, doLeavings, false);
				return;
			}
			if (this.foundationGrid[num] != null && this.underGrid[num] == null)
			{
				this.RemoveFoundation(c, doLeavings);
				return;
			}
			if (doLeavings)
			{
				GenLeaving.DoLeavingsFor(this.topGrid[num], c, this.map);
			}
			if (this.underGrid[num] == null)
			{
				return;
			}
			if (this.topGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(c);
			}
			this.topGrid[num] = this.underGrid[num];
			this.underGrid[num] = null;
			this.colorGrid[num] = null;
			if (this.foundationGrid[num] != null && this.foundationGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			else if (this.topGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			this.DoTerrainChangedEffects(c, oldTerr, this.TerrainAt(c), null);
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0006DFD8 File Offset: 0x0006C1D8
		public void SetFoundation(IntVec3 c, TerrainDef newTerr)
		{
			if (newTerr == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to set terrain at ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(c);
				defaultInterpolatedStringHandler.AppendLiteral(" to null.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			if (!newTerr.isFoundation)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(75, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Tried to set foundation terrain at ");
				defaultInterpolatedStringHandler2.AppendFormatted<IntVec3>(c);
				defaultInterpolatedStringHandler2.AppendLiteral(" to a type which is not a foundation (");
				defaultInterpolatedStringHandler2.AppendFormatted(newTerr.defName);
				defaultInterpolatedStringHandler2.AppendLiteral(").");
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			TerrainDef oldTerr = this.TerrainAt(num);
			if (this.TempTerrainAt(num) != null)
			{
				this.RemoveTempTerrain(c, false, true);
			}
			if (this.underGrid[num] != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(68, 2);
				defaultInterpolatedStringHandler3.AppendLiteral("Tried to set foundation at ");
				defaultInterpolatedStringHandler3.AppendFormatted<IntVec3>(c);
				defaultInterpolatedStringHandler3.AppendLiteral(" to ");
				defaultInterpolatedStringHandler3.AppendFormatted<TerrainDef>(newTerr);
				defaultInterpolatedStringHandler3.AppendLiteral(" but there is an under terrain there.");
				Log.Error(defaultInterpolatedStringHandler3.ToStringAndClear());
				return;
			}
			if (this.topGrid[num] != null && this.topGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(c);
			}
			this.foundationGrid[num] = newTerr;
			if (newTerr.glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			this.DoTerrainChangedEffects(c, oldTerr, newTerr, null);
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0006E158 File Offset: 0x0006C358
		public void RemoveFoundation(IntVec3 c, bool doLeavings = true)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			TerrainDef oldTerr = this.TerrainAt(num);
			TerrainDef oldFoundation = this.FoundationAt(num);
			if (doLeavings)
			{
				GenLeaving.DoLeavingsFor(this.foundationGrid[num], c, this.map);
			}
			if (this.foundationGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(c);
			}
			this.foundationGrid[num] = null;
			this.colorGrid[num] = null;
			if (this.topGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			this.DoTerrainChangedEffects(c, oldTerr, this.TerrainAt(num), oldFoundation);
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0006E20C File Offset: 0x0006C40C
		public void RemoveGravshipTerrainUnsafe(IntVec3 cell, int index)
		{
			TerrainDef terrainDef = this.TerrainAt(cell);
			TerrainDef terrainDef2 = this.underGrid[index] ?? this.topGrid[index];
			if (terrainDef2 == null)
			{
				return;
			}
			if (terrainDef.glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(cell);
			}
			this.tempGrid[index] = null;
			this.foundationGrid[index] = null;
			this.colorGrid[index] = null;
			this.underGrid[index] = null;
			this.topGrid[index] = terrainDef2;
			if (terrainDef2.glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(cell);
			}
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x0006E2A4 File Offset: 0x0006C4A4
		public void SetTempTerrain(IntVec3 c, TerrainDef newTerr)
		{
			if (newTerr == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to set terrain at ");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(c);
				defaultInterpolatedStringHandler.AppendLiteral(" to null.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			if (!newTerr.temporary)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(66, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Tried to set temp terrain at ");
				defaultInterpolatedStringHandler2.AppendFormatted<IntVec3>(c);
				defaultInterpolatedStringHandler2.AppendLiteral(" to a type which is not temporary (");
				defaultInterpolatedStringHandler2.AppendFormatted(newTerr.defName);
				defaultInterpolatedStringHandler2.AppendLiteral(").");
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				return;
			}
			TerrainDef oldTerr = this.TerrainAt(c);
			if (Current.ProgramState == ProgramState.Playing)
			{
				Designation designation = this.map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor);
				if (designation != null)
				{
					designation.Delete();
				}
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (this.topGrid[num] != null && this.topGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(c);
			}
			TempTerrainProps tempTerrain = newTerr.tempTerrain;
			if (tempTerrain != null && tempTerrain.destroysFloors && this.underGrid[num] != null && !this.topGrid[num].natural)
			{
				this.topGrid[num] = this.underGrid[num];
				this.underGrid[num] = null;
				this.colorGrid[num] = null;
			}
			this.tempGrid[num] = newTerr;
			if (newTerr.glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			this.DoTerrainChangedEffects(c, oldTerr, newTerr, null);
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0006E438 File Offset: 0x0006C638
		public void RemoveTempTerrain(IntVec3 c, bool doLeavings = false, bool preventDestroyEffects = false)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			if (doLeavings)
			{
				GenLeaving.DoLeavingsFor(this.tempGrid[num], c, this.map);
			}
			if (this.tempGrid[num] == null)
			{
				return;
			}
			TerrainDef terrainDef = this.tempGrid[num];
			if (terrainDef.glowRadius > 0f)
			{
				this.map.glowGrid.DeregisterTerrain(c);
			}
			this.tempGrid[num] = null;
			if (!preventDestroyEffects)
			{
				TempTerrainProps tempTerrain = terrainDef.tempTerrain;
				if (((tempTerrain != null) ? tempTerrain.terrainOnRemoved : null) != null)
				{
					if (terrainDef.tempTerrain.terrainOnRemoved.temporary)
					{
						this.SetTempTerrain(c, terrainDef.tempTerrain.terrainOnRemoved);
						goto IL_DA;
					}
					this.SetTerrain(c, terrainDef.tempTerrain.terrainOnRemoved);
					goto IL_DA;
				}
			}
			if (this.topGrid[num].glowRadius > 0f)
			{
				this.map.glowGrid.RegisterTerrain(c);
			}
			IL_DA:
			this.DoTerrainChangedEffects(c, terrainDef, this.TerrainAt(c), null);
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x0006E530 File Offset: 0x0006C730
		public void SetTerrainColor(IntVec3 c, ColorDef color)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			this.colorGrid[num] = color;
			this.DoTerrainChangedEffects(c, this.TerrainAt(c), this.TerrainAt(c), null);
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0006E570 File Offset: 0x0006C770
		public bool CanRemoveTopLayerAt(IntVec3 c)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			TerrainDef terrainDef = this.tempGrid[num];
			return (terrainDef != null && terrainDef.Removable) || (this.topGrid[num].Removable && this.underGrid[num] != null);
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x0006E5C4 File Offset: 0x0006C7C4
		public bool CanRemoveFoundationAt(IntVec3 c)
		{
			int num = this.map.cellIndices.CellToIndex(c);
			return this.foundationGrid[num] != null && this.foundationGrid[num].Removable;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x0006E5FC File Offset: 0x0006C7FC
		private void DoTerrainChangedEffects(IntVec3 c, TerrainDef oldTerr, TerrainDef newTerr, TerrainDef oldFoundation = null)
		{
			this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Terrain, true, false);
			List<Thing> thingList = c.GetThingList(this.map);
			int i = thingList.Count - 1;
			while (i >= 0)
			{
				if (thingList[i].def.category != ThingCategory.Plant)
				{
					goto IL_150;
				}
				if (this.map.fertilityGrid.FertilityAt(c) < thingList[i].def.plant.fertilityMin)
				{
					if (newTerr.IsFlood && !thingList[i].def.plant.destroyedByFlooding)
					{
						goto IL_20C;
					}
					if (!thingList[i].def.plant.completelyIgnoreFertility)
					{
						thingList[i].Destroy(DestroyMode.Vanish);
						goto IL_20C;
					}
				}
				if (thingList[i].def.plant.WildTerrainTags.Count > 0 && !thingList[i].def.plant.WildTerrainTags.Overlaps(newTerr.tags.OrElseEmptyEnumerable<string>()))
				{
					thingList[i].Destroy(DestroyMode.Vanish);
				}
				else
				{
					if (!thingList[i].def.plant.showInFrozenWater)
					{
						this.map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Things);
						goto IL_150;
					}
					goto IL_150;
				}
				IL_20C:
				i--;
				continue;
				IL_150:
				if (thingList[i].def.category == ThingCategory.Filth && !FilthMaker.TerrainAcceptsFilth(newTerr, thingList[i].def, FilthSourceFlags.None))
				{
					thingList[i].Destroy(DestroyMode.Vanish);
					goto IL_20C;
				}
				if ((thingList[i].def.IsBlueprint || thingList[i].def.IsFrame) && !GenConstruct.CanBuildOnTerrain(thingList[i].def.entityDefToBuild, thingList[i].Position, this.map, thingList[i].Rotation, thingList[i], ((IConstructible)thingList[i]).EntityToBuildStuff()))
				{
					thingList[i].Destroy(DestroyMode.Cancel);
					goto IL_20C;
				}
				goto IL_20C;
			}
			if (!newTerr.holdSnowOrSand)
			{
				this.map.snowGrid.SetDepth(c, 0f);
				SandGrid sandGrid = this.map.sandGrid;
				if (sandGrid != null)
				{
					sandGrid.SetDepth(c, 0f);
				}
			}
			Designation designation = this.map.designationManager.DesignationAt(c, DesignationDefOf.SmoothFloor);
			if (designation != null)
			{
				designation.Delete();
			}
			Designation designation2 = this.map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFloor);
			if (designation2 != null)
			{
				designation2.Delete();
			}
			Designation designation3 = this.map.designationManager.DesignationAt(c, DesignationDefOf.PaintFloor);
			if (designation3 != null)
			{
				designation3.Delete();
			}
			Designation designation4 = this.map.designationManager.DesignationAt(c, DesignationDefOf.RemovePaintFloor);
			if (designation4 != null)
			{
				designation4.Delete();
			}
			this.map.pathing.RecalculatePerceivedPathCostAt(c);
			CellBoolDrawer cellBoolDrawer = this.drawerInt;
			if (cellBoolDrawer != null)
			{
				cellBoolDrawer.SetDirty();
			}
			this.map.fertilityGrid.Drawer.SetDirty();
			if (ModsConfig.OdysseyActive && ((oldFoundation != null && oldFoundation.IsSubstructure) || newTerr.IsSubstructure))
			{
				SubstructureGrid substructureGrid = this.map.substructureGrid;
				if (substructureGrid != null)
				{
					substructureGrid.MarkDirty();
				}
			}
			Region regionAt_NoRebuild_InvalidAllowed = this.map.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c);
			if (regionAt_NoRebuild_InvalidAllowed != null)
			{
				District district = regionAt_NoRebuild_InvalidAllowed.District;
				if (district != null)
				{
					district.Notify_TerrainChanged();
				}
			}
			if (ModsConfig.OdysseyActive)
			{
				using (new ProfilerBlock("FishPopulationTracker.Notify_TerrainChanged"))
				{
					WaterBodyTracker waterBodyTracker = this.map.waterBodyTracker;
					if (waterBodyTracker != null)
					{
						waterBodyTracker.Notify_TerrainChanged(c, oldTerr, newTerr);
					}
				}
			}
			this.map.events.Notify_TerrainChanged(c);
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0006E9CC File Offset: 0x0006CBCC
		public void ExposeData()
		{
			this.ExposeTerrainGrid(this.topGrid, "topGrid", TerrainDefOf.Soil);
			this.ExposeTerrainGrid(this.underGrid, "underGrid", null);
			this.ExposeTerrainGrid(this.foundationGrid, "foundationGrid", null);
			this.ExposeTerrainGrid(this.tempGrid, "tempGrid", null);
			this.ExposeColorGrid();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				foreach (IntVec3 intVec in this.map.AllCells)
				{
					int num = this.map.cellIndices.CellToIndex(intVec);
					if (this.TerrainAt(num).glowRadius > 0f)
					{
						this.map.glowGrid.RegisterTerrain(intVec);
					}
					if (this.topGrid[num].IsWater)
					{
						this.waterCells.Add(intVec);
					}
					if (this.topGrid[num].isFoundation)
					{
						this.foundationGrid[num] = this.topGrid[num];
						this.topGrid[num] = this.underGrid[num];
						this.underGrid[num] = null;
					}
				}
			}
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x0006EB04 File Offset: 0x0006CD04
		public void Notify_TerrainBurned(IntVec3 c)
		{
			TerrainDef terrain = c.GetTerrain(this.map);
			TerrainDef terrainDef = this.FoundationAt(c);
			if (terrainDef != null && terrainDef.Flammable())
			{
				this.Notify_FoundationDestroyed(c);
				return;
			}
			this.Notify_TerrainDestroyed(c);
			if (terrain.burnedDef != null)
			{
				this.SetTerrain(c, terrain.burnedDef);
			}
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0006EB58 File Offset: 0x0006CD58
		public void Notify_TerrainDestroyed(IntVec3 c)
		{
			if (!this.CanRemoveTopLayerAt(c))
			{
				return;
			}
			TerrainDef terrainDef = this.TerrainAt(c);
			this.RemoveTopLayer(c, false);
			if (terrainDef.destroyBuildingsOnDestroyed)
			{
				Building firstBuilding = c.GetFirstBuilding(this.map);
				if (firstBuilding != null)
				{
					firstBuilding.Kill(null, null);
				}
			}
			if (terrainDef.destroyEffectWater != null && this.TerrainAt(c) != null && this.TerrainAt(c).IsWater)
			{
				Effecter effecter = terrainDef.destroyEffectWater.Spawn();
				effecter.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false), -1);
				effecter.Cleanup();
			}
			else if (terrainDef.destroyEffect != null)
			{
				Effecter effecter2 = terrainDef.destroyEffect.Spawn();
				effecter2.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false), -1);
				effecter2.Cleanup();
			}
			ThingUtility.CheckAutoRebuildTerrainOnDestroyed(terrainDef, c, this.map);
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0006EC44 File Offset: 0x0006CE44
		public void Notify_FoundationDestroyed(IntVec3 c)
		{
			TerrainDef terrainDef = this.FoundationAt(c);
			if (terrainDef == null)
			{
				return;
			}
			if (this.CanRemoveTopLayerAt(c))
			{
				this.Notify_TerrainDestroyed(c);
			}
			this.RemoveFoundation(c, false);
			if (terrainDef.destroyBuildingsOnDestroyed)
			{
				Building firstBuilding = c.GetFirstBuilding(this.map);
				if (firstBuilding != null)
				{
					firstBuilding.Kill(null, null);
				}
			}
			if (terrainDef.destroyEffectWater != null && this.TerrainAt(c) != null && this.TerrainAt(c).IsWater)
			{
				Effecter effecter = terrainDef.destroyEffectWater.Spawn();
				effecter.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false), -1);
				effecter.Cleanup();
			}
			else if (terrainDef.destroyEffect != null)
			{
				Effecter effecter2 = terrainDef.destroyEffect.Spawn();
				effecter2.Trigger(new TargetInfo(c, this.map, false), new TargetInfo(c, this.map, false), -1);
				effecter2.Cleanup();
			}
			ThingUtility.CheckAutoRebuildTerrainOnDestroyed(terrainDef, c, this.map);
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x0006ED38 File Offset: 0x0006CF38
		private void ExposeTerrainGrid(TerrainDef[] grid, string label, TerrainDef fallbackTerrain)
		{
			TerrainGrid.<>c__DisplayClass47_0 CS$<>8__locals1 = new TerrainGrid.<>c__DisplayClass47_0();
			CS$<>8__locals1.grid = grid;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.fallbackTerrain = fallbackTerrain;
			CS$<>8__locals1.terrainDefsByShortHash = new Dictionary<ushort, TerrainDef>();
			foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs)
			{
				CS$<>8__locals1.terrainDefsByShortHash.Add(terrainDef.shortHash, terrainDef);
			}
			MapExposeUtility.ExposeUshort(this.map, new Func<IntVec3, ushort>(CS$<>8__locals1.<ExposeTerrainGrid>g__Read|0), new Action<IntVec3, ushort>(CS$<>8__locals1.<ExposeTerrainGrid>g__Write|1), label);
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0006EDDC File Offset: 0x0006CFDC
		private void ExposeColorGrid()
		{
			TerrainGrid.<>c__DisplayClass48_0 CS$<>8__locals1 = new TerrainGrid.<>c__DisplayClass48_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.colorDefsByShortHash = new Dictionary<ushort, ColorDef>();
			foreach (ColorDef colorDef in DefDatabase<ColorDef>.AllDefs)
			{
				CS$<>8__locals1.colorDefsByShortHash.Add(colorDef.shortHash, colorDef);
			}
			MapExposeUtility.ExposeUshort(this.map, new Func<IntVec3, ushort>(CS$<>8__locals1.<ExposeColorGrid>g__Read|0), new Action<IntVec3, ushort>(CS$<>8__locals1.<ExposeColorGrid>g__Write|1), "colorGrid");
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x0006EE74 File Offset: 0x0006D074
		public string DebugStringAt(IntVec3 c)
		{
			if (c.InBounds(this.map))
			{
				TerrainDef terrain = c.GetTerrain(this.map);
				TerrainDef terrainDef = this.underGrid[this.map.cellIndices.CellToIndex(c)];
				return "top: " + ((terrain != null) ? terrain.defName : "null") + ", under=" + ((terrainDef != null) ? terrainDef.defName : "null");
			}
			return "out of bounds";
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x0006EEEA File Offset: 0x0006D0EA
		public void TerrainGridUpdate()
		{
			if (Find.PlaySettings.showTerrainAffordanceOverlay && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0006EF1A File Offset: 0x0006D11A
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0006EF24 File Offset: 0x0006D124
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			IntVec3 intVec = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			TerrainAffordanceDef terrainAffordanceDef;
			return !intVec.Filled(this.map) && !intVec.Fogged(this.map) && this.TryGetAffordanceDefToDraw(intVec, out terrainAffordanceDef);
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0006EF70 File Offset: 0x0006D170
		private bool TryGetAffordanceDefToDraw(IntVec3 cell, out TerrainAffordanceDef affordance)
		{
			if (cell.GetAffordances(this.map).NullOrEmpty<TerrainAffordanceDef>())
			{
				affordance = null;
				return true;
			}
			TerrainAffordanceDef terrainAffordanceDef = null;
			int num = int.MinValue;
			foreach (TerrainAffordanceDef terrainAffordanceDef2 in cell.GetAffordances(this.map))
			{
				if (terrainAffordanceDef2.visualizeOnAffordanceOverlay)
				{
					if (num < terrainAffordanceDef2.order)
					{
						num = terrainAffordanceDef2.order;
						terrainAffordanceDef = terrainAffordanceDef2;
					}
				}
				else if (terrainAffordanceDef2.blockAffordanceOverlay)
				{
					affordance = null;
					return false;
				}
			}
			affordance = terrainAffordanceDef;
			return true;
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x0006F018 File Offset: 0x0006D218
		private Color CellBoolDrawerGetExtraColorInt(int index)
		{
			IntVec3 cell = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			TerrainAffordanceDef terrainAffordanceDef;
			this.TryGetAffordanceDefToDraw(cell, out terrainAffordanceDef);
			if (terrainAffordanceDef == null)
			{
				return TerrainGrid.NoAffordanceColor;
			}
			return terrainAffordanceDef.affordanceOverlayColor;
		}

		// Token: 0x040010F6 RID: 4342
		private Map map;

		// Token: 0x040010F7 RID: 4343
		private TerrainDef[] tempGrid;

		// Token: 0x040010F8 RID: 4344
		public TerrainDef[] topGrid;

		// Token: 0x040010F9 RID: 4345
		public TerrainDef[] foundationGrid;

		// Token: 0x040010FA RID: 4346
		private TerrainDef[] underGrid;

		// Token: 0x040010FB RID: 4347
		public ColorDef[] colorGrid;

		// Token: 0x040010FC RID: 4348
		private CellBoolDrawer drawerInt;

		// Token: 0x040010FD RID: 4349
		private HashSet<IntVec3> waterCells;

		// Token: 0x040010FE RID: 4350
		private Dictionary<ValueTuple<TerrainDef, bool, ColorDef>, Material> terrainMatCache = new Dictionary<ValueTuple<TerrainDef, bool, ColorDef>, Material>();

		// Token: 0x040010FF RID: 4351
		private static readonly Color NoAffordanceColor = Color.red;
	}
}
