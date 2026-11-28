using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AF2 RID: 6898
	public class GravshipLandingMarker : Thing
	{
		// Token: 0x17001FC3 RID: 8131
		// (get) Token: 0x0600B039 RID: 45113 RVA: 0x003E1633 File Offset: 0x003DF833
		public IEnumerable<IntVec3> GravshipCells
		{
			get
			{
				return this.gravshipCells;
			}
		}

		// Token: 0x17001FC4 RID: 8132
		// (get) Token: 0x0600B03A RID: 45114 RVA: 0x003E163B File Offset: 0x003DF83B
		public IEnumerable<IntVec3> ThrusterCells
		{
			get
			{
				return this.thrusterCells;
			}
		}

		// Token: 0x17001FC5 RID: 8133
		// (get) Token: 0x0600B03B RID: 45115 RVA: 0x003E1643 File Offset: 0x003DF843
		public IEnumerable<IntVec3> DoorCells
		{
			get
			{
				return this.doorCells;
			}
		}

		// Token: 0x17001FC6 RID: 8134
		// (get) Token: 0x0600B03C RID: 45116 RVA: 0x003E164B File Offset: 0x003DF84B
		protected bool Visible
		{
			get
			{
				return base.Spawned && this.gravship != null && !(Find.DesignatorManager.SelectedDesignator is Designator_MoveGravship);
			}
		}

		// Token: 0x17001FC7 RID: 8135
		// (get) Token: 0x0600B03D RID: 45117 RVA: 0x003E1674 File Offset: 0x003DF874
		// (set) Token: 0x0600B03E RID: 45118 RVA: 0x003E1681 File Offset: 0x003DF881
		public Rot4 GravshipRotation
		{
			get
			{
				return this.gravship.Rotation;
			}
			set
			{
				if (this.gravship.Rotation == value)
				{
					return;
				}
				this.gravship.Rotation = value;
				this.CacheCells();
				this.Notify_Moved();
			}
		}

		// Token: 0x17001FC8 RID: 8136
		// (get) Token: 0x0600B03F RID: 45119 RVA: 0x003E16B0 File Offset: 0x003DF8B0
		public override CellRect? CustomRectForSelector
		{
			get
			{
				if (this.Visible)
				{
					return new CellRect?(this.selectorRect);
				}
				return null;
			}
		}

		// Token: 0x0600B040 RID: 45120 RVA: 0x003E16DC File Offset: 0x003DF8DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<Gravship>(ref this.gravship, "gravship", Array.Empty<object>());
			Scribe_Values.Look<CellRect>(ref this.selectorRect, "selectorRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Current.Game.Gravship = this.gravship;
				this.CacheCells();
			}
		}

		// Token: 0x0600B041 RID: 45121 RVA: 0x003E173C File Offset: 0x003DF93C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckOdyssey("Gravship"))
			{
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (Find.CurrentMap != map || Find.World.renderer.wantedMode == WorldRenderMode.Planet)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					CameraJumper.TryJump(this, CameraJumper.MovementMode.Cut);
				});
			}
			this.CacheCells();
			this.Notify_Moved();
			if (!respawningAfterLoad)
			{
				Find.GravshipController.Notify_LandingAreaConfirmationStarted(this);
			}
		}

		// Token: 0x0600B042 RID: 45122 RVA: 0x003E17A4 File Offset: 0x003DF9A4
		public void CacheCells()
		{
			this.gravshipCells.Clear();
			this.gravshipCells.AddRange(this.gravship.Foundations.Keys);
			this.doorCells.Clear();
			this.thrusterCells.Clear();
			this.tmpExclusionZoneCells.Clear();
			foreach (KeyValuePair<Thing, PositionData.Data> keyValuePair in this.gravship.ThrusterPlacements)
			{
				Thing thing;
				PositionData.Data data;
				keyValuePair.Deconstruct(out thing, out data);
				Thing thing2 = thing;
				PositionData.Data data2 = data;
				thing2.def.GetCompProperties<CompProperties_GravshipThruster>().GetExclusionZone(data2.local, data2.rotation, ref this.tmpExclusionZoneCells);
				this.thrusterCells.AddRange(this.tmpExclusionZoneCells);
			}
		}

		// Token: 0x0600B043 RID: 45123 RVA: 0x003E1880 File Offset: 0x003DFA80
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (!this.Visible)
			{
				return;
			}
			this.tmpGravshipCells.Clear();
			this.tmpGravshipCells.AddRange(this.GravshipCells);
			IntVec3 position = base.Position;
			for (int i = 0; i < this.tmpGravshipCells.Count; i++)
			{
				List<IntVec3> list = this.tmpGravshipCells;
				int index = i;
				list[index] += position;
			}
			this.tmpThrusterCells.Clear();
			this.tmpThrusterCells.AddRange(this.ThrusterCells);
			for (int j = 0; j < this.tmpThrusterCells.Count; j++)
			{
				List<IntVec3> list = this.tmpThrusterCells;
				int index = j;
				list[index] += position;
			}
			GenDraw.DrawFieldEdges(this.tmpGravshipCells, 2900);
			GenDraw.DrawDiagonalStripes(this.tmpGravshipCells, new Color?(GravshipLandingMarker.diagonalsColor), null, 2900);
			GenDraw.DrawFieldEdges(this.tmpThrusterCells, ColorLibrary.Orange, null, null, 2900);
			foreach (KeyValuePair<Thing, PositionData.Data> keyValuePair in this.gravship.ExteriorDoorPlacements)
			{
				Thing thing;
				PositionData.Data data;
				keyValuePair.Deconstruct(out thing, out data);
				Thing thing2 = thing;
				PositionData.Data data2 = data;
				GhostUtility.GhostGraphicFor(thing2.Graphic, thing2.def, Color.white, null).DrawFromDef(GenThing.TrueCenter(position + data2.local, data2.rotation, thing2.def.Size, AltitudeLayer.MetaOverlays.AltitudeFor()), data2.rotation, thing2.def, 0f);
			}
		}

		// Token: 0x0600B044 RID: 45124 RVA: 0x003E1A50 File Offset: 0x003DFC50
		public void Notify_Moved()
		{
			if (this.gravship == null)
			{
				return;
			}
			this.selectorRect = CellRect.CenteredOn(base.Position, 1);
			foreach (IntVec3 a in this.gravship.Foundations.Keys)
			{
				this.selectorRect = this.selectorRect.Encapsulate(a + base.Position);
			}
		}

		// Token: 0x0600B045 RID: 45125 RVA: 0x003E1AE0 File Offset: 0x003DFCE0
		public void BeginLanding(WorldComponent_GravshipController gravshipController)
		{
			Map map = base.Map;
			CameraJumper.TryJump(this, CameraJumper.MovementMode.Pan);
			this.Destroy(DestroyMode.Vanish);
			gravshipController.InitiateLanding(this.gravship, map, base.Position, this.GravshipRotation);
		}

		// Token: 0x0600B046 RID: 45126 RVA: 0x003E1B20 File Offset: 0x003DFD20
		public override string GetInspectString()
		{
			return "ConfirmLandingToContinue".Translate();
		}

		// Token: 0x040065B4 RID: 26036
		public Gravship gravship;

		// Token: 0x040065B5 RID: 26037
		private CellRect selectorRect;

		// Token: 0x040065B6 RID: 26038
		private readonly List<IntVec3> gravshipCells = new List<IntVec3>();

		// Token: 0x040065B7 RID: 26039
		private readonly List<IntVec3> thrusterCells = new List<IntVec3>();

		// Token: 0x040065B8 RID: 26040
		private readonly List<IntVec3> doorCells = new List<IntVec3>();

		// Token: 0x040065B9 RID: 26041
		private static readonly Color diagonalsColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x040065BA RID: 26042
		private List<IntVec3> tmpExclusionZoneCells = new List<IntVec3>();

		// Token: 0x040065BB RID: 26043
		private readonly List<IntVec3> tmpGravshipCells = new List<IntVec3>();

		// Token: 0x040065BC RID: 26044
		private readonly List<IntVec3> tmpThrusterCells = new List<IntVec3>();
	}
}
