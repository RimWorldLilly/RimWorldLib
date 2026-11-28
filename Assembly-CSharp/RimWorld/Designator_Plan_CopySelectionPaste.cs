using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C78 RID: 7288
	public class Designator_Plan_CopySelectionPaste : Designator_Plan_Add
	{
		// Token: 0x170021BC RID: 8636
		// (get) Token: 0x0600BA20 RID: 47648 RVA: 0x0041A110 File Offset: 0x00418310
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021BD RID: 8637
		// (get) Token: 0x0600BA21 RID: 47649 RVA: 0x0041A113 File Offset: 0x00418313
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170021BE RID: 8638
		// (get) Token: 0x0600BA22 RID: 47650 RVA: 0x0041A116 File Offset: 0x00418316
		public override bool AlwaysDoGuiControls
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021BF RID: 8639
		// (get) Token: 0x0600BA23 RID: 47651 RVA: 0x0041A119 File Offset: 0x00418319
		public float PaneTopY
		{
			get
			{
				return (float)(UI.screenHeight - 35);
			}
		}

		// Token: 0x170021C0 RID: 8640
		// (get) Token: 0x0600BA24 RID: 47652 RVA: 0x0041A124 File Offset: 0x00418324
		public override bool CanRightClickToggleVisibility
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600BA25 RID: 47653 RVA: 0x0041A128 File Offset: 0x00418328
		public Designator_Plan_CopySelectionPaste()
		{
			this.useMouseIcon = true;
			this.hideMouseIcon = true;
			this.mouseText = "CommandCopyPlanSelectionPasteMouse".Translate();
		}

		// Token: 0x0600BA26 RID: 47654 RVA: 0x0041A180 File Offset: 0x00418380
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			foreach (ColorDef colorDef in this.colors)
			{
				foreach (IntVec3 intVec in this.GetCurrentCells(UI.MouseCell(), colorDef))
				{
					if (intVec.InBounds(base.Map) && !intVec.InNoBuildEdgeArea(base.Map))
					{
						this.cells.Add(intVec);
					}
				}
				GenDraw.DrawFieldEdges(this.cells, colorDef.color, null, null, 3600);
				this.cells.Clear();
			}
			GenDraw.DrawNoZoneEdgeLines();
		}

		// Token: 0x0600BA27 RID: 47655 RVA: 0x0041A26C File Offset: 0x0041846C
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			foreach (ColorDef color in this.colors)
			{
				foreach (IntVec3 c in this.GetCurrentCells(loc, color))
				{
					if (!c.InBounds(base.Map))
					{
						return false;
					}
					if (c.InNoBuildEdgeArea(base.Map))
					{
						return "TooCloseToMapEdge".Translate();
					}
				}
			}
			return true;
		}

		// Token: 0x0600BA28 RID: 47656 RVA: 0x0041A334 File Offset: 0x00418534
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (ColorDef colorDef in this.colors)
			{
				base.SelectedPlan = null;
				this.colorDef = colorDef;
				base.PlanCells(this.GetCurrentCells(c, colorDef));
			}
			base.SelectedPlan = null;
		}

		// Token: 0x0600BA29 RID: 47657 RVA: 0x0041A3A4 File Offset: 0x004185A4
		protected IEnumerable<IntVec3> GetCurrentCells(IntVec3 c, ColorDef color)
		{
			Designator_Plan_CopySelectionPaste.<GetCurrentCells>d__19 <GetCurrentCells>d__ = new Designator_Plan_CopySelectionPaste.<GetCurrentCells>d__19(-2);
			<GetCurrentCells>d__.<>4__this = this;
			<GetCurrentCells>d__.<>3__c = c;
			<GetCurrentCells>d__.<>3__color = color;
			return <GetCurrentCells>d__;
		}

		// Token: 0x0600BA2A RID: 47658 RVA: 0x0041A3C4 File Offset: 0x004185C4
		public void Initialize(IEnumerable<IntVec3> selected)
		{
			this.rotation = Rot4.North;
			Designator_Plan_CopySelectionPaste.tmpSelected.Clear();
			Designator_Plan_CopySelectionPaste.tmpSelected.AddRange(selected);
			CellRect cellRect = CellRect.FromCellList(Designator_Plan_CopySelectionPaste.tmpSelected);
			this.indices = new CellIndices(cellRect.Size.x, cellRect.Size.z);
			this.grid.Clear();
			this.colors.Clear();
			this.grid.Capacity = Mathf.Max(this.grid.Capacity, cellRect.Area);
			for (int i = 0; i < this.indices.NumGridCells; i++)
			{
				IntVec3 c = cellRect.Min + this.indices[i];
				Plan plan = base.Map.planManager.PlanAt(c);
				ColorDef colorDef = (plan != null) ? plan.Color : null;
				this.grid.Add(colorDef);
				if (colorDef != null)
				{
					this.colors.Add(colorDef);
				}
			}
		}

		// Token: 0x0600BA2B RID: 47659 RVA: 0x0041A4C0 File Offset: 0x004186C0
		public override void DoExtraGuiControls(float leftX, float bottomY)
		{
			DesignatorUtility.GUIDoRotationControls(leftX, bottomY, this.rotation, delegate(Rot4 rot)
			{
				this.rotation = rot;
			});
		}

		// Token: 0x0600BA2C RID: 47660 RVA: 0x0041A4DC File Offset: 0x004186DC
		public override void SelectedProcessInput(Event ev)
		{
			RotationDirection rotationDirection = RotationDirection.None;
			if (Event.current.button == 2)
			{
				if (Event.current.type == EventType.mouseDown)
				{
					Event.current.Use();
					Designator_Plan_CopySelectionPaste.middleMouseDownTime = Time.realtimeSinceStartup;
				}
				if (Event.current.type == EventType.mouseUp && Time.realtimeSinceStartup - Designator_Plan_CopySelectionPaste.middleMouseDownTime < 0.15f)
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
				this.rotation.Rotate(rotationDirection);
			}
			DesignatorUtility.GUIDoRotationControls(0f, this.PaneTopY, this.rotation, delegate(Rot4 rot)
			{
				this.rotation = rot;
			});
		}

		// Token: 0x040069E7 RID: 27111
		private readonly List<ColorDef> grid = new List<ColorDef>();

		// Token: 0x040069E8 RID: 27112
		private readonly HashSet<ColorDef> colors = new HashSet<ColorDef>();

		// Token: 0x040069E9 RID: 27113
		private CellIndices indices;

		// Token: 0x040069EA RID: 27114
		private Rot4 rotation;

		// Token: 0x040069EB RID: 27115
		private readonly List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x040069EC RID: 27116
		private static readonly List<IntVec3> tmpSelected = new List<IntVec3>();

		// Token: 0x040069ED RID: 27117
		private static float middleMouseDownTime;
	}
}
