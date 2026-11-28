using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A1 RID: 673
	[StaticConstructorOnStartup]
	public class VacuumComponent : MapComponent, ICellBoolGiver, IDisposable
	{
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001496 RID: 5270 RVA: 0x00075A74 File Offset: 0x00073C74
		private CellBoolDrawer Drawer
		{
			get
			{
				CellBoolDrawer result;
				if ((result = this.drawerInt) == null)
				{
					result = (this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 3600, 0.33f));
				}
				return result;
			}
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x00075AC4 File Offset: 0x00073CC4
		private Material WarningMat
		{
			get
			{
				return VacuumComponent.IntWarningMat;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x00075ACB File Offset: 0x00073CCB
		private Material VacuumOverlayMat0
		{
			get
			{
				return VacuumComponent.IntVacuumOverlayMat0;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x00075AD2 File Offset: 0x00073CD2
		private Material VacuumOverlayTransitionMat0
		{
			get
			{
				return VacuumComponent.IntVacuumOverlayTransitionMat0;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x0600149A RID: 5274 RVA: 0x00075AD9 File Offset: 0x00073CD9
		private Material VacuumOverlayMat1
		{
			get
			{
				return VacuumComponent.IntVacuumOverlayMat1;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x0600149B RID: 5275 RVA: 0x00075AE0 File Offset: 0x00073CE0
		private Material VacuumOverlayTransitionMat1
		{
			get
			{
				return VacuumComponent.IntVacuumOverlayTransitionMat1;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x0600149C RID: 5276 RVA: 0x00075AE7 File Offset: 0x00073CE7
		private bool ActiveOnMap
		{
			get
			{
				return ModLister.OdysseyInstalled && this.map.Biome.inVacuum;
			}
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x00075B04 File Offset: 0x00073D04
		public VacuumComponent(Map map) : base(map)
		{
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				return;
			}
			this.InitializeIfRequired();
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x00075B90 File Offset: 0x00073D90
		private void InitializeIfRequired()
		{
			if (!this.ActiveOnMap)
			{
				return;
			}
			this.map.events.RegionsRoomsChanged += this.Dirty;
			this.map.events.DoorOpened += delegate(Building_Door _)
			{
				this.Dirty();
			};
			this.map.events.DoorClosed += delegate(Building_Door _)
			{
				this.Dirty();
			};
			this.map.events.TerrainChanged += delegate(IntVec3 _)
			{
				this.Dirty();
			};
			this.map.events.RoofChanged += delegate(IntVec3 _)
			{
				this.Dirty();
			};
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x00075C32 File Offset: 0x00073E32
		public override void FinalizeInit()
		{
			if (this.ActiveOnMap)
			{
				this.RebuildData();
			}
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x00075C42 File Offset: 0x00073E42
		public override void MapComponentTick()
		{
			if (!this.ActiveOnMap)
			{
				return;
			}
			if (!this.map.IsHashIntervalTick(250))
			{
				return;
			}
			if (this.dirty)
			{
				this.RebuildData();
			}
			this.ExchangeRoomVacuum();
			this.UpdateVacuumOverlay();
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x00075C7C File Offset: 0x00073E7C
		private bool HasDirectPathToVacuum(Room room, out HashSet<IntVec3> warning)
		{
			VacuumComponent.frontier.Enqueue(room);
			Room room2;
			while (VacuumComponent.frontier.TryDequeue(out room2))
			{
				VacuumComponent.visited.Add(room2);
				if (room2.ExposedToSpace)
				{
					warning = VacuumComponent.exteriorCells[room2];
					VacuumComponent.visited.Clear();
					VacuumComponent.exteriorCells.Clear();
					VacuumComponent.frontier.Clear();
					return true;
				}
				VacuumComponent.GroupData groupData;
				if (this.roomGroups.TryGetValue(room2, out groupData))
				{
					using (Dictionary<Room, HashSet<IntVec3>>.Enumerator enumerator = groupData.adjacent.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<Room, HashSet<IntVec3>> keyValuePair = enumerator.Current;
							Room room3;
							HashSet<IntVec3> hashSet;
							keyValuePair.Deconstruct(out room3, out hashSet);
							Room room4 = room3;
							HashSet<IntVec3> hashSet2 = hashSet;
							if (!VacuumComponent.visited.Contains(room4) && !VacuumComponent.frontier.Contains(room4))
							{
								foreach (IntVec3 c in hashSet2)
								{
									Building edifice = c.GetEdifice(this.map);
									if (edifice != null)
									{
										Building_Door building_Door = edifice as Building_Door;
										if (building_Door == null || !building_Door.Open)
										{
											continue;
										}
									}
									VacuumComponent.exteriorCells[room4] = hashSet2;
									VacuumComponent.frontier.Enqueue(room4);
									break;
								}
							}
						}
						continue;
					}
					break;
				}
			}
			VacuumComponent.visited.Clear();
			VacuumComponent.exteriorCells.Clear();
			VacuumComponent.frontier.Clear();
			warning = null;
			return false;
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x00075E08 File Offset: 0x00074008
		private void ExchangeRoomVacuum()
		{
			this.warningCells.Clear();
			this.warningMatrices.Clear();
			foreach (KeyValuePair<Room, VacuumComponent.GroupData> keyValuePair in this.roomGroups)
			{
				Room room;
				VacuumComponent.GroupData groupData;
				keyValuePair.Deconstruct(out room, out groupData);
				Room room2 = room;
				VacuumComponent.GroupData groupData2 = groupData;
				this.roomVacuums[room2] = room2.UnsanitizedVacuum;
				foreach (KeyValuePair<Room, HashSet<IntVec3>> keyValuePair2 in groupData2.adjacent)
				{
					HashSet<IntVec3> hashSet;
					keyValuePair2.Deconstruct(out room, out hashSet);
					Room room3 = room;
					this.roomVacuums[room3] = room3.UnsanitizedVacuum;
				}
			}
			foreach (KeyValuePair<Room, VacuumComponent.GroupData> keyValuePair in this.roomGroups)
			{
				Room room;
				VacuumComponent.GroupData groupData;
				keyValuePair.Deconstruct(out room, out groupData);
				Room room4 = room;
				VacuumComponent.GroupData groupData3 = groupData;
				if (groupData3.adjacent.Count != 0 && !room4.IsDoorway)
				{
					float num = this.roomVacuums[room4];
					int num2 = room4.CellCount;
					float num3 = (float)room4.CellCount * num;
					int num4 = room4.CellCount;
					if (!room4.ExposedToSpace)
					{
						foreach (KeyValuePair<Room, HashSet<IntVec3>> keyValuePair2 in groupData3.adjacent)
						{
							HashSet<IntVec3> hashSet;
							keyValuePair2.Deconstruct(out room, out hashSet);
							Room room5 = room;
							HashSet<IntVec3> hashSet2 = hashSet;
							num2 += room5.CellCount;
							num3 += (float)room5.CellCount * this.roomVacuums[room5];
							if (!room5.ExposedToSpace)
							{
								num4 += room5.CellCount;
							}
							else
							{
								foreach (IntVec3 item in hashSet2)
								{
									if (this.warningCells.Add(item))
									{
										this.warningMatrices.Add(Matrix4x4.Translate(item.ToVector3ShiftedWithAltitude(AltitudeLayer.MapDataOverlay)));
									}
								}
							}
						}
						float num5 = Mathf.Clamp(num3 / (float)num2, -0.1f, 1f);
						if (groupData3.hasDirectPathToVacuum)
						{
							if (room4.Vacuum < 1f)
							{
								foreach (IntVec3 item2 in groupData3.directWarnings)
								{
									if (this.warningCells.Add(item2))
									{
										this.warningMatrices.Add(Matrix4x4.Translate(item2.ToVector3ShiftedWithAltitude(AltitudeLayer.MapDataOverlay)));
									}
								}
								float num6 = Mathf.Min(0.1f / ((float)num4 / 100f), 1f);
								num5 = Mathf.Clamp(num5 + num6, -0.1f, 1f);
								float num7 = Mathf.Min(0.5f / ((float)num4 / 100f), 1f);
								float num8 = (num5 - num) * 1.0416666f * num7;
								room4.Vacuum = num + num8;
							}
						}
						else if (!Mathf.Approximately(num, num5))
						{
							float num9 = Mathf.Min(0.5f / ((float)num4 / 100f), 1f);
							float num10 = (num5 - num) * 1.0416666f * num9;
							room4.Vacuum = num + num10;
						}
					}
				}
			}
			this.roomVacuums.Clear();
			this.Drawer.SetDirty();
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x00076230 File Offset: 0x00074430
		public void RebuildData()
		{
			using (ProfilerBlock.Scope("VacuumComponent.RebuildData"))
			{
				this.dirty = false;
				this.ClearBuffer();
				this.MergeRoomsIntoGroups();
				this.RegenerateVacuumOverlay();
			}
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x00076284 File Offset: 0x00074484
		public void Dirty()
		{
			this.dirty = true;
			this.SetDrawerDirty();
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x00076294 File Offset: 0x00074494
		private void ClearBuffer()
		{
			foreach (KeyValuePair<Room, VacuumComponent.GroupData> keyValuePair in this.roomGroups)
			{
				Room room;
				VacuumComponent.GroupData item;
				keyValuePair.Deconstruct(out room, out item);
				FullPool<VacuumComponent.GroupData>.Return(item);
			}
			this.roomGroups.Clear();
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x000762FC File Offset: 0x000744FC
		private void MergeRoomsIntoGroups()
		{
			foreach (Room room in this.map.regionGrid.AllRooms)
			{
				if (!room.ExposedToSpace)
				{
					VacuumComponent.GroupData orAddGroup = this.GetOrAddGroup(room);
					if (room.IsDoorway)
					{
						Building_Door door = room.Cells.First<IntVec3>().GetDoor(this.map);
						if (door == null || !door.ExchangeVacuum)
						{
							continue;
						}
						CellRect cellRect = door.OccupiedRect();
						using (CellRect.Enumerator enumerator2 = cellRect.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								IntVec3 intVec = enumerator2.Current;
								for (int i = 0; i < 4; i++)
								{
									IntVec3 intVec2 = intVec + GenAdj.CardinalDirections[i];
									if (!cellRect.Contains(intVec2))
									{
										Room room2 = intVec2.GetRoom(this.map);
										if (room2 != null && room2 != room)
										{
											HashSet<IntVec3> hashSet;
											if (!orAddGroup.adjacent.TryGetValue(room2, out hashSet))
											{
												hashSet = (orAddGroup.adjacent[room2] = SimplePool<HashSet<IntVec3>>.Get());
											}
											hashSet.Add(intVec);
										}
									}
								}
							}
							continue;
						}
					}
					foreach (IntVec3 intVec3 in room.BorderCellsCardinal)
					{
						Building edifice = intVec3.GetEdifice(this.map);
						if (edifice != null && edifice.ExchangeVacuum)
						{
							CellRect cellRect2 = edifice.OccupiedRect();
							foreach (IntVec3 a in cellRect2)
							{
								for (int j = 0; j < 4; j++)
								{
									IntVec3 intVec4 = a + GenAdj.CardinalDirections[j];
									if (!cellRect2.Contains(intVec4))
									{
										Room room3 = intVec4.GetRoom(this.map);
										if (room3 != null && room3 != room)
										{
											HashSet<IntVec3> hashSet2;
											if (!orAddGroup.adjacent.TryGetValue(room3, out hashSet2))
											{
												hashSet2 = (orAddGroup.adjacent[room3] = SimplePool<HashSet<IntVec3>>.Get());
											}
											hashSet2.Add(intVec3);
										}
									}
								}
							}
						}
					}
				}
			}
			foreach (KeyValuePair<Room, VacuumComponent.GroupData> keyValuePair in this.roomGroups)
			{
				Room room4;
				VacuumComponent.GroupData groupData;
				keyValuePair.Deconstruct(out room4, out groupData);
				Room room5 = room4;
				VacuumComponent.GroupData groupData2 = groupData;
				HashSet<IntVec3> directWarnings;
				if (this.HasDirectPathToVacuum(room5, out directWarnings))
				{
					groupData2.hasDirectPathToVacuum = true;
					groupData2.directWarnings = directWarnings;
				}
			}
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x00076630 File Offset: 0x00074830
		private VacuumComponent.GroupData GetOrAddGroup(Room room)
		{
			VacuumComponent.GroupData result;
			if (this.roomGroups.TryGetValue(room, out result))
			{
				return result;
			}
			VacuumComponent.GroupData groupData = FullPool<VacuumComponent.GroupData>.Get();
			this.roomGroups[room] = groupData;
			return groupData;
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x00076663 File Offset: 0x00074863
		public void SetDrawerDirty()
		{
			this.Drawer.SetDirty();
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x00076670 File Offset: 0x00074870
		private void RegenerateVacuumOverlay()
		{
			this.vacuumBoxMatrices.Clear();
			this.vacuumBoxIntensities.Clear();
			this.vacuumedCells.Clear();
			foreach (KeyValuePair<Room, VacuumComponent.GroupData> keyValuePair in this.roomGroups)
			{
				Room room;
				VacuumComponent.GroupData groupData;
				keyValuePair.Deconstruct(out room, out groupData);
				Room room2 = room;
				room2.vacuumOverlayDrawn = VacuumComponent.RoomVacuumVisible(room2);
				room2.vacuumOverlayRects = 0;
				if (room2.vacuumOverlayDrawn)
				{
					float vacuum = room2.Vacuum;
					foreach (IntVec3 key in room2.Cells)
					{
						this.vacuumedCells.Add(key, vacuum);
					}
					foreach (Region region in room2.Regions)
					{
						foreach (CellRect cellRect in region.EnumerateRectangleCover())
						{
							Vector3 centerVector = cellRect.CenterVector3;
							centerVector.y = AltitudeLayer.Gas.AltitudeFor();
							Matrix4x4 item = Matrix4x4.TRS(centerVector, Quaternion.identity, new Vector3((float)cellRect.Width, 1f, (float)cellRect.Height));
							room2.vacuumOverlayRects++;
							this.vacuumBoxMatrices.Add(item);
							this.vacuumBoxIntensities.Add(vacuum);
						}
					}
				}
			}
			this.vacuumTransitionMatrices.Clear();
			this.vacuumTransitionIntensities.Clear();
			foreach (KeyValuePair<IntVec3, float> keyValuePair2 in this.vacuumedCells)
			{
				IntVec3 intVec;
				float num;
				keyValuePair2.Deconstruct(out intVec, out num);
				IntVec3 a = intVec;
				float item2 = num;
				for (int i = 0; i < 4; i++)
				{
					IntVec3 key2 = a + GenAdj.CardinalDirections[i];
					if (!this.vacuumedCells.ContainsKey(key2))
					{
						Matrix4x4 item3 = Matrix4x4.TRS(key2.ToVector3ShiftedWithAltitude(AltitudeLayer.Gas), new Rot4(i).AsQuat, Vector3.one);
						this.vacuumTransitionMatrices.Add(item3);
						this.vacuumTransitionIntensities.Add(item2);
					}
				}
			}
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x00076960 File Offset: 0x00074B60
		private void UpdateVacuumOverlay()
		{
			int num = 0;
			foreach (KeyValuePair<Room, VacuumComponent.GroupData> keyValuePair in this.roomGroups)
			{
				Room room;
				VacuumComponent.GroupData groupData;
				keyValuePair.Deconstruct(out room, out groupData);
				Room room2 = room;
				if (VacuumComponent.RoomVacuumVisible(room2) != room2.vacuumOverlayDrawn)
				{
					this.RegenerateVacuumOverlay();
					break;
				}
				for (int i = 0; i < room2.vacuumOverlayRects; i++)
				{
					this.vacuumBoxIntensities[num++] = room2.Vacuum;
				}
			}
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x00076A00 File Offset: 0x00074C00
		private static bool RoomVacuumVisible(Room room)
		{
			return !room.ExposedToSpace && room.UnsanitizedVacuum > 0.5f && (!room.IsDoorway || room.Door.Open);
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x00076A30 File Offset: 0x00074C30
		public override void MapComponentDraw()
		{
			if (!this.map.Biome.inVacuum)
			{
				return;
			}
			if (Find.PlaySettings.showVacuumOverlay && !WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				this.Drawer.MarkForDraw();
				if (this.warningMatrices.Count > 0)
				{
					Graphics.DrawMeshInstanced(MeshPool.plane08, 0, this.WarningMat, this.warningMatrices);
				}
			}
			using (ProfilerBlock.Scope("VacuumComponent.Drawer.CellBoolDrawerUpdate()"))
			{
				this.Drawer.CellBoolDrawerUpdate();
			}
			if (WorldComponent_GravshipController.CutsceneInProgress)
			{
				return;
			}
			if (this.vacuumBoxMatrices.Count > 0)
			{
				if (this.vacuumBoxPropBlock == null)
				{
					this.vacuumBoxPropBlock = new MaterialPropertyBlock();
				}
				this.vacuumBoxPropBlock.Clear();
				if (this.vacuumBoxIntensities.Count > 0)
				{
					this.vacuumBoxPropBlock.SetFloatArray(VacuumComponent.VacuumPercent, this.vacuumBoxIntensities);
				}
				Graphics.DrawMeshInstanced(MeshPool.plane10, 0, this.VacuumOverlayMat0, this.vacuumBoxMatrices, this.vacuumBoxPropBlock);
				Graphics.DrawMeshInstanced(MeshPool.plane10, 0, this.VacuumOverlayMat1, this.vacuumBoxMatrices, this.vacuumBoxPropBlock);
			}
			if (this.vacuumTransitionMatrices.Count > 0)
			{
				if (this.vacuumTransitionPropBlock == null)
				{
					this.vacuumTransitionPropBlock = new MaterialPropertyBlock();
				}
				this.vacuumTransitionPropBlock.Clear();
				if (this.vacuumTransitionIntensities.Count > 0)
				{
					this.vacuumTransitionPropBlock.SetFloatArray(VacuumComponent.VacuumPercent, this.vacuumTransitionIntensities);
				}
				Graphics.DrawMeshInstanced(MeshPool.plane10, 0, this.VacuumOverlayTransitionMat0, this.vacuumTransitionMatrices, this.vacuumTransitionPropBlock);
				Graphics.DrawMeshInstanced(MeshPool.plane10, 0, this.VacuumOverlayTransitionMat1, this.vacuumTransitionMatrices, this.vacuumTransitionPropBlock);
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060014AD RID: 5293 RVA: 0x00076BE8 File Offset: 0x00074DE8
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x00076BF0 File Offset: 0x00074DF0
		public bool GetCellBool(int index)
		{
			IntVec3 c = this.map.cellIndices.IndexToCell(index);
			Building edifice = c.GetEdifice(this.map);
			return !c.Fogged(this.map) && (edifice == null || edifice.ExchangeVacuum);
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x00076C38 File Offset: 0x00074E38
		public Color GetCellExtraColor(int index)
		{
			float vacuum = this.map.cellIndices.IndexToCell(index).GetVacuum(this.map);
			if (vacuum < 0.5f)
			{
				return Color.Lerp(Color.green, Color.yellow, Mathf.InverseLerp(0f, 0.5f, vacuum));
			}
			return Color.Lerp(Color.yellow, Color.red, Mathf.InverseLerp(0.5f, 1f, vacuum));
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00076CA9 File Offset: 0x00074EA9
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.InitializeIfRequired();
			}
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x00076CB9 File Offset: 0x00074EB9
		public void Dispose()
		{
			this.ClearBuffer();
		}

		// Token: 0x04001222 RID: 4642
		private CellBoolDrawer drawerInt;

		// Token: 0x04001223 RID: 4643
		private bool dirty = true;

		// Token: 0x04001224 RID: 4644
		private const int UpdateInterval = 250;

		// Token: 0x04001225 RID: 4645
		private const float VacuumDeltaPerSecondFactor = 1.0416666f;

		// Token: 0x04001226 RID: 4646
		private const float VacuumFactorPerHundredCells = 0.5f;

		// Token: 0x04001227 RID: 4647
		private const float DirectVacuumPenaltyPerHundredCells = 0.1f;

		// Token: 0x04001228 RID: 4648
		private const float WarningLevel = 0.5f;

		// Token: 0x04001229 RID: 4649
		private readonly Dictionary<Room, VacuumComponent.GroupData> roomGroups = new Dictionary<Room, VacuumComponent.GroupData>();

		// Token: 0x0400122A RID: 4650
		private static readonly Material IntWarningMat = MatLoader.LoadMat("Map/OxygenOverlay/Warning", -1);

		// Token: 0x0400122B RID: 4651
		private static readonly Material IntVacuumOverlayMat0 = MatLoader.LoadMat("Map/OxygenOverlay/VacuumOverlay", -1);

		// Token: 0x0400122C RID: 4652
		private static readonly Material IntVacuumOverlayTransitionMat0 = MatLoader.LoadMat("Map/OxygenOverlay/VacuumOverlayTransition", -1);

		// Token: 0x0400122D RID: 4653
		private static readonly Material IntVacuumOverlayMat1 = MatLoader.LoadMat("Map/OxygenOverlay/VacuumOverlay2", -1);

		// Token: 0x0400122E RID: 4654
		private static readonly Material IntVacuumOverlayTransitionMat1 = MatLoader.LoadMat("Map/OxygenOverlay/VacuumOverlayTransition2", -1);

		// Token: 0x0400122F RID: 4655
		private static readonly int VacuumPercent = Shader.PropertyToID("_VacuumPercent");

		// Token: 0x04001230 RID: 4656
		private readonly List<Matrix4x4> vacuumBoxMatrices = new List<Matrix4x4>();

		// Token: 0x04001231 RID: 4657
		private readonly List<float> vacuumBoxIntensities = new List<float>();

		// Token: 0x04001232 RID: 4658
		private MaterialPropertyBlock vacuumBoxPropBlock;

		// Token: 0x04001233 RID: 4659
		private readonly Dictionary<IntVec3, float> vacuumedCells = new Dictionary<IntVec3, float>();

		// Token: 0x04001234 RID: 4660
		private readonly List<Matrix4x4> vacuumTransitionMatrices = new List<Matrix4x4>();

		// Token: 0x04001235 RID: 4661
		private readonly List<float> vacuumTransitionIntensities = new List<float>();

		// Token: 0x04001236 RID: 4662
		private readonly Dictionary<Room, float> roomVacuums = new Dictionary<Room, float>();

		// Token: 0x04001237 RID: 4663
		private readonly HashSet<IntVec3> warningCells = new HashSet<IntVec3>();

		// Token: 0x04001238 RID: 4664
		private readonly List<Matrix4x4> warningMatrices = new List<Matrix4x4>();

		// Token: 0x04001239 RID: 4665
		private MaterialPropertyBlock vacuumTransitionPropBlock;

		// Token: 0x0400123A RID: 4666
		private static readonly HashSet<Room> visited = new HashSet<Room>();

		// Token: 0x0400123B RID: 4667
		private static readonly Dictionary<Room, HashSet<IntVec3>> exteriorCells = new Dictionary<Room, HashSet<IntVec3>>();

		// Token: 0x0400123C RID: 4668
		private static readonly Queue<Room> frontier = new Queue<Room>();

		// Token: 0x020025F8 RID: 9720
		private class GroupData : IFullPoolable
		{
			// Token: 0x0600F1A8 RID: 61864 RVA: 0x0058BDFC File Offset: 0x00589FFC
			public void Reset()
			{
				foreach (KeyValuePair<Room, HashSet<IntVec3>> keyValuePair in this.adjacent)
				{
					Room room;
					HashSet<IntVec3> hashSet;
					keyValuePair.Deconstruct(out room, out hashSet);
					HashSet<IntVec3> hashSet2 = hashSet;
					hashSet2.Clear();
					SimplePool<HashSet<IntVec3>>.Return(hashSet2);
				}
				this.adjacent.Clear();
				this.directWarnings = null;
				this.hasDirectPathToVacuum = false;
			}

			// Token: 0x04009BC1 RID: 39873
			public readonly Dictionary<Room, HashSet<IntVec3>> adjacent = new Dictionary<Room, HashSet<IntVec3>>();

			// Token: 0x04009BC2 RID: 39874
			public HashSet<IntVec3> directWarnings;

			// Token: 0x04009BC3 RID: 39875
			public bool hasDirectPathToVacuum;
		}
	}
}
