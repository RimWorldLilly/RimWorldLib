using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C75 RID: 7285
	public class Designator_Plan_Add : Designator_Plan
	{
		// Token: 0x170021AC RID: 8620
		// (get) Token: 0x0600B9F7 RID: 47607 RVA: 0x00419465 File Offset: 0x00417665
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021AD RID: 8621
		// (get) Token: 0x0600B9F8 RID: 47608 RVA: 0x00419468 File Offset: 0x00417668
		public virtual bool CanRightClickToggleVisibility
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021AE RID: 8622
		// (get) Token: 0x0600B9F9 RID: 47609 RVA: 0x0041946C File Offset: 0x0041766C
		public static IEnumerable<ColorDef> Colors
		{
			get
			{
				return from x in DefDatabase<ColorDef>.AllDefs
				where x.colorType == ColorType.Planning
				orderby x.displayOrder
				select x;
			}
		}

		// Token: 0x170021AF RID: 8623
		// (get) Token: 0x0600B9FA RID: 47610 RVA: 0x004194C8 File Offset: 0x004176C8
		public override Color IconDrawColor
		{
			get
			{
				Color color = this.colorDef.color;
				color.a = 0.8f;
				return color;
			}
		}

		// Token: 0x170021B0 RID: 8624
		// (get) Token: 0x0600B9FB RID: 47611 RVA: 0x004194EE File Offset: 0x004176EE
		protected virtual bool CanSelectColor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021B1 RID: 8625
		// (get) Token: 0x0600B9FC RID: 47612 RVA: 0x004194F1 File Offset: 0x004176F1
		// (set) Token: 0x0600B9FD RID: 47613 RVA: 0x00419518 File Offset: 0x00417718
		protected Plan SelectedPlan
		{
			get
			{
				Plan selectedPlan = Find.Selector.SelectedPlan;
				if (selectedPlan != null)
				{
					this.colorDef = Find.Selector.SelectedPlan.Color;
				}
				return selectedPlan;
			}
			set
			{
				if (Find.Selector.SelectedPlan == value)
				{
					return;
				}
				Find.Selector.ClearSelection();
				if (value != null)
				{
					Find.Selector.Select(value, false, false);
					if (Find.Selector.SelectedPlan != null)
					{
						this.colorDef = Find.Selector.SelectedPlan.Color;
					}
				}
			}
		}

		// Token: 0x0600B9FE RID: 47614 RVA: 0x00419570 File Offset: 0x00417770
		public Designator_Plan_Add()
		{
			this.colorDef = Designator_Plan_Add.Colors.FirstOrDefault<ColorDef>();
			this.defaultLabel = "DesignatorPlan".Translate();
			this.defaultDesc = "DesignatorPlanDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanOn", true);
			this.soundSucceeded = SoundDefOf.Designate_PlanAdd;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.hotKey = KeyBindingDefOf.Misc9;
			this.useMouseIcon = true;
		}

		// Token: 0x0600B9FF RID: 47615 RVA: 0x00419617 File Offset: 0x00417817
		protected virtual Plan CreateNewPlan()
		{
			return new Plan(this.colorDef, Find.CurrentMap.planManager);
		}

		// Token: 0x0600BA00 RID: 47616 RVA: 0x0041962E File Offset: 0x0041782E
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.InNoBuildEdgeArea(base.Map))
			{
				return "TooCloseToMapEdge".Translate();
			}
			return true;
		}

		// Token: 0x0600BA01 RID: 47617 RVA: 0x0041966C File Offset: 0x0041786C
		public override void DrawMouseAttachments()
		{
			if (!this.useMouseIcon)
			{
				return;
			}
			string text = this.mouseText;
			if (!Input.GetKey(KeyCode.Mouse0) && text.NullOrEmpty())
			{
				Plan selectedPlan = Find.Selector.SelectedPlan;
				text = ((selectedPlan != null) ? "ExpandOrCreatePlan".Translate(selectedPlan.label) : "CreateNewPlan".Translate());
			}
			GenUI.DrawMouseAttachment(this.icon, text, 0f, default(Vector2), null, null, false, default(Color), null, null);
		}

		// Token: 0x0600BA02 RID: 47618 RVA: 0x00419718 File Offset: 0x00417918
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			GenUI.RenderMouseoverBracket();
			if (Find.Selector.SelectedPlan != null && Find.Selector.SelectedPlan.Color != this.colorDef)
			{
				Find.Selector.Deselect(Find.Selector.SelectedPlan);
			}
			if (!base.Map.IsPocketMap)
			{
				if (!base.Map.Tile.LayerDef.ignoreNoBuildArea)
				{
					GenDraw.DrawNoBuildEdgeLines();
					return;
				}
				GenDraw.DrawMapBoundaryLines();
			}
		}

		// Token: 0x0600BA03 RID: 47619 RVA: 0x0041979C File Offset: 0x0041799C
		public override void ProcessInput(Event ev)
		{
			if (!this.CanSelectColor)
			{
				base.ProcessInput(ev);
				return;
			}
			if (!base.CheckCanInteract())
			{
				return;
			}
			if (ev.button == 0)
			{
				List<FloatMenuGridOption> list = new List<FloatMenuGridOption>();
				foreach (ColorDef newCol2 in Designator_Plan_Add.Colors)
				{
					ColorDef newCol = newCol2;
					Color color = newCol.color;
					color.a = 0.8f;
					list.Add(new FloatMenuGridOption(BaseContent.WhiteTex, delegate()
					{
						this.<>n__0(ev);
						Find.DesignatorManager.Select(this);
						this.colorDef = newCol;
					}, new Color?(color), new TipSignal?(newCol.LabelCap)));
				}
				Find.WindowStack.Add(new FloatMenuGrid(list));
				Find.DesignatorManager.Select(this);
				return;
			}
			if (ev.button == 1 && this.CanRightClickToggleVisibility)
			{
				List<FloatMenuOption> options = new List<FloatMenuOption>(Command_Hide_Plans.GetHideOptions());
				Find.WindowStack.Add(new FloatMenu(options));
			}
		}

		// Token: 0x170021B2 RID: 8626
		// (get) Token: 0x0600BA04 RID: 47620 RVA: 0x004198E4 File Offset: 0x00417AE4
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				Designator_Plan_Add.<get_RightClickFloatMenuOptions>d__22 <get_RightClickFloatMenuOptions>d__ = new Designator_Plan_Add.<get_RightClickFloatMenuOptions>d__22(-2);
				<get_RightClickFloatMenuOptions>d__.<>4__this = this;
				return <get_RightClickFloatMenuOptions>d__;
			}
		}

		// Token: 0x0600BA05 RID: 47621 RVA: 0x004198F4 File Offset: 0x00417AF4
		public override void DesignateSingleCell(IntVec3 c)
		{
			Plan plan = base.Map.planManager.PlanAt(c);
			if (plan != null && plan.Color == this.colorDef)
			{
				this.SelectedPlan = plan;
				base.Finalize(false);
				return;
			}
			this.PlanCells(new List<IntVec3>
			{
				c
			});
		}

		// Token: 0x0600BA06 RID: 47622 RVA: 0x00419948 File Offset: 0x00417B48
		public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			if (cells.Count<IntVec3>() == 1)
			{
				Plan plan = base.Map.planManager.PlanAt(cells.First<IntVec3>());
				if (plan != null && plan.Color == this.colorDef)
				{
					this.SelectedPlan = plan;
					base.Finalize(false);
					return;
				}
			}
			this.PlanCells(cells);
		}

		// Token: 0x0600BA07 RID: 47623 RVA: 0x0041999C File Offset: 0x00417B9C
		protected void PlanCells(IEnumerable<IntVec3> cells)
		{
			this.modified.Clear();
			this.unsetCells.Clear();
			this.unsetCells.AddRange(cells);
			if (this.SelectedPlan == null)
			{
				Plan plan = null;
				foreach (IntVec3 c in cells)
				{
					Plan plan2 = base.Map.planManager.PlanAt(c);
					if (plan2 != null && plan2.Color == this.colorDef)
					{
						if (plan == null)
						{
							plan = plan2;
						}
						else if (plan2 != plan)
						{
							plan = null;
							break;
						}
					}
				}
				this.SelectedPlan = plan;
			}
			if (this.unsetCells.Count == 0)
			{
				return;
			}
			if (this.SelectedPlan == null)
			{
				Plan plan3 = base.Map.planManager.PlanAt(this.unsetCells[0]);
				if (plan3 != null)
				{
					plan3.RemoveCell(this.unsetCells[0]);
					this.modified.Add(plan3);
				}
				this.SelectedPlan = this.CreateNewPlan();
				this.SelectedPlan.AddCell(this.unsetCells[0]);
				this.unsetCells.RemoveAt(0);
			}
			this.modified.Add(this.SelectedPlan);
			bool somethingSucceeded;
			for (;;)
			{
				somethingSucceeded = true;
				int count = this.unsetCells.Count;
				for (int i = this.unsetCells.Count - 1; i >= 0; i--)
				{
					IntVec3 intVec = this.unsetCells[i];
					for (int j = 0; j < 4; j++)
					{
						IntVec3 c2 = intVec + GenAdj.CardinalDirections[j];
						if (c2.InBounds(base.Map))
						{
							Plan plan4 = base.Map.planManager.PlanAt(c2);
							if (plan4 != null && plan4.Color == this.colorDef && plan4 != this.SelectedPlan)
							{
								this.SelectedPlan.MergeIn(plan4);
							}
						}
					}
					Plan plan5 = base.Map.planManager.PlanAt(intVec);
					if (plan5 != null && plan5 != this.SelectedPlan)
					{
						plan5.RemoveCell(intVec);
						this.modified.Add(plan5);
					}
					if (!this.SelectedPlan.ContainsCell(intVec))
					{
						this.SelectedPlan.AddCell(intVec);
					}
					this.unsetCells.RemoveAt(i);
				}
				if (this.unsetCells.Count == 0)
				{
					break;
				}
				if (this.unsetCells.Count == count)
				{
					this.SelectedPlan = this.CreateNewPlan();
					this.SelectedPlan.AddCell(this.unsetCells[0]);
					this.unsetCells.RemoveAt(0);
				}
			}
			foreach (Plan plan6 in this.modified)
			{
				plan6.CheckContiguous();
			}
			base.Finalize(somethingSucceeded);
			this.modified.Clear();
			this.unsetCells.Clear();
		}

		// Token: 0x040069DF RID: 27103
		protected ColorDef colorDef;

		// Token: 0x040069E0 RID: 27104
		public const float UIAlpha = 0.8f;

		// Token: 0x040069E1 RID: 27105
		private readonly List<IntVec3> unsetCells = new List<IntVec3>();

		// Token: 0x040069E2 RID: 27106
		private readonly HashSet<Plan> modified = new HashSet<Plan>();
	}
}
