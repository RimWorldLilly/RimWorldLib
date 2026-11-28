using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C76 RID: 7286
	public class Designator_Plan_Copy : Designator_Plan_Add
	{
		// Token: 0x170021B3 RID: 8627
		// (get) Token: 0x0600BA09 RID: 47625 RVA: 0x00419CB1 File Offset: 0x00417EB1
		public override bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170021B4 RID: 8628
		// (get) Token: 0x0600BA0A RID: 47626 RVA: 0x00419CB4 File Offset: 0x00417EB4
		protected override bool CanSelectColor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170021B5 RID: 8629
		// (get) Token: 0x0600BA0B RID: 47627 RVA: 0x00419CB7 File Offset: 0x00417EB7
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170021B6 RID: 8630
		// (get) Token: 0x0600BA0C RID: 47628 RVA: 0x00419CBA File Offset: 0x00417EBA
		public override bool AlwaysDoGuiControls
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021B7 RID: 8631
		// (get) Token: 0x0600BA0D RID: 47629 RVA: 0x00419CBD File Offset: 0x00417EBD
		public override bool CanRightClickToggleVisibility
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170021B8 RID: 8632
		// (get) Token: 0x0600BA0E RID: 47630 RVA: 0x00419CC0 File Offset: 0x00417EC0
		public float PaneTopY
		{
			get
			{
				return (float)(UI.screenHeight - 35);
			}
		}

		// Token: 0x170021B9 RID: 8633
		// (get) Token: 0x0600BA0F RID: 47631 RVA: 0x00419CCB File Offset: 0x00417ECB
		public override Color IconDrawColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x0600BA10 RID: 47632 RVA: 0x00419CD4 File Offset: 0x00417ED4
		public Designator_Plan_Copy()
		{
			this.useMouseIcon = false;
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanCopy", true);
			this.defaultLabel = "CommandCopyPlanLabel".Translate();
			this.defaultDesc = "CommandCopyPlanDesc".Translate();
			this.hotKey = KeyBindingDefOf.Designator_Cancel;
		}

		// Token: 0x0600BA11 RID: 47633 RVA: 0x00419D40 File Offset: 0x00417F40
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			this.cells.Clear();
			foreach (IntVec3 intVec in this.GetCurrentCells(UI.MouseCell()))
			{
				if (intVec.InBounds(base.Map) && !intVec.InNoBuildEdgeArea(base.Map))
				{
					this.cells.Add(intVec);
				}
			}
			GenDraw.DrawFieldEdges(this.cells, this.colorDef.color, null, null, 3600);
			GenDraw.DrawNoZoneEdgeLines();
		}

		// Token: 0x0600BA12 RID: 47634 RVA: 0x00419DF0 File Offset: 0x00417FF0
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			foreach (IntVec3 c in this.GetCurrentCells(loc))
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
			return true;
		}

		// Token: 0x0600BA13 RID: 47635 RVA: 0x00419E78 File Offset: 0x00418078
		public override void DesignateSingleCell(IntVec3 c)
		{
			base.PlanCells(this.cells);
			base.SelectedPlan = null;
		}

		// Token: 0x0600BA14 RID: 47636 RVA: 0x00419E8D File Offset: 0x0041808D
		protected IEnumerable<IntVec3> GetCurrentCells(IntVec3 c)
		{
			Designator_Plan_Copy.<GetCurrentCells>d__21 <GetCurrentCells>d__ = new Designator_Plan_Copy.<GetCurrentCells>d__21(-2);
			<GetCurrentCells>d__.<>4__this = this;
			<GetCurrentCells>d__.<>3__c = c;
			return <GetCurrentCells>d__;
		}

		// Token: 0x0600BA15 RID: 47637 RVA: 0x00419EA4 File Offset: 0x004180A4
		public override void Selected()
		{
			Plan selectedPlan = base.SelectedPlan;
			CellRect cellRect = CellRect.FromCellList(selectedPlan.Cells);
			this.colorDef = selectedPlan.Color;
			this.rotation = Rot4.North;
			this.grid = new BoolGrid(cellRect.Width, cellRect.Height);
			foreach (IntVec3 a in selectedPlan.Cells)
			{
				IntVec3 c = a - cellRect.Min;
				this.grid.Set(c, true);
			}
			base.SelectedPlan = null;
		}

		// Token: 0x0600BA16 RID: 47638 RVA: 0x00419F54 File Offset: 0x00418154
		public override void DoExtraGuiControls(float leftX, float bottomY)
		{
			DesignatorUtility.GUIDoRotationControls(leftX, bottomY, this.rotation, delegate(Rot4 rot)
			{
				this.rotation = rot;
			});
		}

		// Token: 0x0600BA17 RID: 47639 RVA: 0x00419F70 File Offset: 0x00418170
		public override void SelectedProcessInput(Event ev)
		{
			RotationDirection rotationDirection = RotationDirection.None;
			if (Event.current.button == 2)
			{
				if (Event.current.type == EventType.mouseDown)
				{
					Event.current.Use();
					Designator_Plan_Copy.middleMouseDownTime = Time.realtimeSinceStartup;
				}
				if (Event.current.type == EventType.mouseUp && Time.realtimeSinceStartup - Designator_Plan_Copy.middleMouseDownTime < 0.15f)
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

		// Token: 0x040069E3 RID: 27107
		private BoolGrid grid;

		// Token: 0x040069E4 RID: 27108
		private Rot4 rotation;

		// Token: 0x040069E5 RID: 27109
		private readonly List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x040069E6 RID: 27110
		private static float middleMouseDownTime;
	}
}
