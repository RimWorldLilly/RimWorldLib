using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C52 RID: 7250
	public class Designator_MoveGravship : Designator
	{
		// Token: 0x17002164 RID: 8548
		// (get) Token: 0x0600B8E7 RID: 47335 RVA: 0x0041357D File Offset: 0x0041177D
		public override string Label
		{
			get
			{
				return "DesignatorMoveGravship".Translate();
			}
		}

		// Token: 0x17002165 RID: 8549
		// (get) Token: 0x0600B8E8 RID: 47336 RVA: 0x0041358E File Offset: 0x0041178E
		public override string Desc
		{
			get
			{
				return "DesignatorMoveGravshipDesc".Translate();
			}
		}

		// Token: 0x17002166 RID: 8550
		// (get) Token: 0x0600B8E9 RID: 47337 RVA: 0x0041359F File Offset: 0x0041179F
		public override float Order
		{
			get
			{
				return -100f;
			}
		}

		// Token: 0x17002167 RID: 8551
		// (get) Token: 0x0600B8EA RID: 47338 RVA: 0x004135A6 File Offset: 0x004117A6
		public override bool AlwaysDoGuiControls
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002168 RID: 8552
		// (get) Token: 0x0600B8EB RID: 47339 RVA: 0x004135A9 File Offset: 0x004117A9
		public float PaneTopY
		{
			get
			{
				return (float)(UI.screenHeight - 35);
			}
		}

		// Token: 0x17002169 RID: 8553
		// (get) Token: 0x0600B8EC RID: 47340 RVA: 0x004135B4 File Offset: 0x004117B4
		public IntVec3 AdjustedMouseCell
		{
			get
			{
				return this.GetSizeRotAdjustedCell(UI.MouseCell());
			}
		}

		// Token: 0x0600B8ED RID: 47341 RVA: 0x004135C4 File Offset: 0x004117C4
		public Designator_MoveGravship(Map map, GravshipLandingMarker marker)
		{
			this.marker = marker;
			this.map = (map ?? marker.Map);
		}

		// Token: 0x0600B8EE RID: 47342 RVA: 0x00413610 File Offset: 0x00411810
		private IntVec3 GetSizeRotAdjustedCell(IntVec3 cell)
		{
			IntVec3 a = new IntVec3(this.marker.gravship.Bounds.Size.x / 2, 0, 0);
			IntVec3 b = new IntVec3(0, 0, this.marker.gravship.Bounds.Size.z / 2);
			if (this.marker.GravshipRotation == Rot4.North)
			{
				cell += a + b;
			}
			else if (this.marker.GravshipRotation == Rot4.East)
			{
				cell += a - b;
			}
			else if (this.marker.GravshipRotation == Rot4.South)
			{
				cell += -a - b;
			}
			else if (this.marker.GravshipRotation == Rot4.West)
			{
				cell += -a + b;
			}
			return cell;
		}

		// Token: 0x0600B8EF RID: 47343 RVA: 0x00413719 File Offset: 0x00411919
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			return this.ValidGravshipLocation(loc, null, null);
		}

		// Token: 0x0600B8F0 RID: 47344 RVA: 0x00413724 File Offset: 0x00411924
		private AcceptanceReport ValidGravshipLocation(IntVec3 loc, List<IntVec3> validCells = null, List<IntVec3> invalidCells = null)
		{
			Map currentMap = Find.CurrentMap;
			IntVec3 root = PrefabUtility.GetRoot(this.GetSizeRotAdjustedCell(loc), this.marker.gravship.Bounds.Size, this.marker.GravshipRotation);
			bool flag = false;
			AcceptanceReport result = true;
			foreach (IntVec3 b in this.marker.GravshipCells)
			{
				IntVec3 intVec = root + b;
				if (!flag && intVec.InBounds(currentMap) && intVec.GetTerrain(currentMap) != TerrainDefOf.Space)
				{
					flag = true;
				}
				AcceptanceReport acceptanceReport = Designator_MoveGravship.IsValidCell(intVec, currentMap);
				if (acceptanceReport.Accepted)
				{
					if (validCells != null)
					{
						validCells.Add(intVec);
					}
				}
				else
				{
					result = acceptanceReport;
					if (invalidCells != null)
					{
						invalidCells.Add(intVec);
					}
					if (validCells == null && invalidCells == null)
					{
						return acceptanceReport;
					}
				}
			}
			if (!flag)
			{
				if (invalidCells != null)
				{
					invalidCells.AddRange(validCells);
				}
				if (validCells != null)
				{
					validCells.Clear();
				}
				return "GravshipMustBeConnectedToLand".Translate();
			}
			return result;
		}

		// Token: 0x0600B8F1 RID: 47345 RVA: 0x00413844 File Offset: 0x00411A44
		private static AcceptanceReport IsValidCell(IntVec3 cell, Map map)
		{
			if (!cell.InBounds(map))
			{
				return "GravshipOutOfBounds".Translate();
			}
			if (!cell.InBounds(map, 1) || cell.InNoBuildEdgeArea(map))
			{
				return "GravshipInNoBuildArea".Translate();
			}
			if (map.landingBlockers != null)
			{
				foreach (CellRect cellRect in map.landingBlockers)
				{
					if (cellRect.Contains(cell))
					{
						return "GravshipInBlockedArea".Translate();
					}
				}
			}
			if (cell.Roofed(map))
			{
				return "GravshipBlockedByRoof".Translate();
			}
			if (cell.Fogged(map))
			{
				return "GravshipBlockedByFog".Translate();
			}
			foreach (Thing thing in cell.GetThingList(map))
			{
				if (!thing.def.preventGravshipLandingOn)
				{
					BuildingProperties building = thing.def.building;
					if (building == null || building.canLandGravshipOn)
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null && (pawn.RaceProps.Humanlike || pawn.HostileTo(Faction.OfPlayer)))
						{
							return "GravshipBlockedBy".Translate(pawn);
						}
						continue;
					}
				}
				return "GravshipBlockedBy".Translate(thing);
			}
			if (!GenConstruct.CanBuildOnTerrain(TerrainDefOf.Substructure, cell, map, Rot4.North, null, null))
			{
				return "GravshipBlockedByTerrain".Translate(cell.GetTerrain(map));
			}
			return true;
		}

		// Token: 0x0600B8F2 RID: 47346 RVA: 0x00413A28 File Offset: 0x00411C28
		public override void Selected()
		{
			if ((this.marker.Spawned && Find.CurrentMap != this.marker.Map) || Find.World.renderer.wantedMode == WorldRenderMode.Planet)
			{
				CameraJumper.TryJump(this.marker, CameraJumper.MovementMode.Pan);
			}
			this.deselectedRotation = this.marker.GravshipRotation;
		}

		// Token: 0x0600B8F3 RID: 47347 RVA: 0x00413A88 File Offset: 0x00411C88
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.marker.Position = PrefabUtility.GetRoot(this.GetSizeRotAdjustedCell(c), this.marker.gravship.Bounds.Size, this.marker.GravshipRotation);
			if (!this.marker.Spawned)
			{
				GenSpawn.Spawn(this.marker, this.marker.Position, this.map, WipeMode.Vanish);
			}
			this.marker.Notify_Moved();
			this.deselectedRotation = this.marker.GravshipRotation;
			Find.DesignatorManager.Deselect();
		}

		// Token: 0x0600B8F4 RID: 47348 RVA: 0x00413B20 File Offset: 0x00411D20
		public override void Deselected()
		{
			this.marker.GravshipRotation = this.deselectedRotation;
		}

		// Token: 0x0600B8F5 RID: 47349 RVA: 0x00413B34 File Offset: 0x00411D34
		public override void DrawMouseAttachments()
		{
			if (!this.lastAcceptanceReport.Accepted)
			{
				Texture iconTex = null;
				string reason = this.lastAcceptanceReport.Reason;
				float angle = 0f;
				Color? textColor = new Color?(ColorLibrary.RedReadable);
				Color textBgColor = new Color(0f, 0f, 0f, 0.5f);
				GenUI.DrawMouseAttachment(iconTex, reason, angle, default(Vector2), null, textColor, true, textBgColor, null, null);
			}
		}

		// Token: 0x0600B8F6 RID: 47350 RVA: 0x00413BAC File Offset: 0x00411DAC
		public override void SelectedUpdate()
		{
			this.tmpValidGravshipCells.Clear();
			this.tmpInvalidGravshipCells.Clear();
			this.lastAcceptanceReport = this.ValidGravshipLocation(UI.MouseCell(), this.tmpValidGravshipCells, this.tmpInvalidGravshipCells);
			this.tmpThrusterCells.Clear();
			this.tmpThrusterCells.AddRange(this.marker.ThrusterCells);
			IntVec3 root = PrefabUtility.GetRoot(this.GetSizeRotAdjustedCell(UI.MouseCell()), this.marker.gravship.Bounds.Size, this.marker.GravshipRotation);
			for (int i = 0; i < this.tmpThrusterCells.Count; i++)
			{
				List<IntVec3> list = this.tmpThrusterCells;
				int index = i;
				list[index] += root;
			}
			GenDraw.DrawFieldEdges(this.tmpValidGravshipCells, ColorLibrary.White, null, null, 2900);
			GenDraw.DrawFieldEdges(this.tmpInvalidGravshipCells, ColorLibrary.Red, null, null, 2900);
			GenDraw.DrawDiagonalStripes(this.tmpValidGravshipCells, new Color?(Designator_MoveGravship.diagonalsColorValid), null, 2900);
			GenDraw.DrawDiagonalStripes(this.tmpInvalidGravshipCells, new Color?(Designator_MoveGravship.diagonalsColorInvalid), null, 2900);
			GenDraw.DrawFieldEdges(this.tmpThrusterCells, ColorLibrary.Orange, null, null, 2900);
			foreach (KeyValuePair<Thing, PositionData.Data> keyValuePair in this.marker.gravship.ExteriorDoorPlacements)
			{
				Thing thing;
				PositionData.Data data;
				keyValuePair.Deconstruct(out thing, out data);
				Thing thing2 = thing;
				PositionData.Data data2 = data;
				GhostUtility.GhostGraphicFor(thing2.Graphic, thing2.def, Color.white, null).DrawFromDef(GenThing.TrueCenter(root + data2.local, data2.rotation, thing2.def.Size, AltitudeLayer.MetaOverlays.AltitudeFor()), data2.rotation, thing2.def, 0f);
			}
		}

		// Token: 0x0600B8F7 RID: 47351 RVA: 0x00413DE0 File Offset: 0x00411FE0
		public override void DoExtraGuiControls(float leftX, float bottomY)
		{
			DesignatorUtility.GUIDoRotationControls(leftX, bottomY, this.marker.GravshipRotation, delegate(Rot4 rot)
			{
				this.marker.GravshipRotation = rot;
			});
		}

		// Token: 0x0600B8F8 RID: 47352 RVA: 0x00413E00 File Offset: 0x00412000
		public override void SelectedProcessInput(Event ev)
		{
			RotationDirection rotationDirection = RotationDirection.None;
			if (Event.current.button == 2)
			{
				if (Event.current.type == EventType.mouseDown)
				{
					Event.current.Use();
					Designator_MoveGravship.middleMouseDownTime = Time.realtimeSinceStartup;
				}
				if (Event.current.type == EventType.mouseUp && Time.realtimeSinceStartup - Designator_MoveGravship.middleMouseDownTime < 0.15f)
				{
					rotationDirection = RotationDirection.Clockwise;
				}
			}
			if (KeyBindingDefOf.Designator_RotateRight.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Clockwise;
			}
			else if (KeyBindingDefOf.Designator_RotateLeft.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Counterclockwise;
			}
			if (rotationDirection != RotationDirection.None)
			{
				this.marker.GravshipRotation = this.marker.GravshipRotation.Rotated(rotationDirection);
			}
			DesignatorUtility.GUIDoRotationControls(0f, this.PaneTopY, this.marker.GravshipRotation, delegate(Rot4 rot)
			{
				this.marker.GravshipRotation = rot;
			});
		}

		// Token: 0x040069B8 RID: 27064
		public GravshipLandingMarker marker;

		// Token: 0x040069B9 RID: 27065
		public Map map;

		// Token: 0x040069BA RID: 27066
		private Rot4 deselectedRotation;

		// Token: 0x040069BB RID: 27067
		private AcceptanceReport lastAcceptanceReport;

		// Token: 0x040069BC RID: 27068
		private static readonly Color diagonalsColorValid = ColorLibrary.White.WithAlpha(0.5f);

		// Token: 0x040069BD RID: 27069
		private static readonly Color diagonalsColorInvalid = ColorLibrary.Red.WithAlpha(0.5f);

		// Token: 0x040069BE RID: 27070
		private readonly List<IntVec3> tmpValidGravshipCells = new List<IntVec3>();

		// Token: 0x040069BF RID: 27071
		private readonly List<IntVec3> tmpInvalidGravshipCells = new List<IntVec3>();

		// Token: 0x040069C0 RID: 27072
		private readonly List<IntVec3> tmpThrusterCells = new List<IntVec3>();

		// Token: 0x040069C1 RID: 27073
		private static float middleMouseDownTime;
	}
}
