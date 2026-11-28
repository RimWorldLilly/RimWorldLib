using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020002E3 RID: 739
	public class Plan : IExposable, ISelectable, ILoadReferenceable, IRenameable, IHideable
	{
		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x0600166B RID: 5739 RVA: 0x000868FD File Offset: 0x00084AFD
		// (set) Token: 0x0600166C RID: 5740 RVA: 0x0008690F File Offset: 0x00084B0F
		public string RenamableLabel
		{
			get
			{
				return this.label ?? this.baseLabel;
			}
			set
			{
				this.label = value;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x0600166D RID: 5741 RVA: 0x00086918 File Offset: 0x00084B18
		public string BaseLabel
		{
			get
			{
				return this.baseLabel;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x0600166E RID: 5742 RVA: 0x00086920 File Offset: 0x00084B20
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x0600166F RID: 5743 RVA: 0x00086928 File Offset: 0x00084B28
		public Map Map
		{
			get
			{
				return this.planManager.map;
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x00086935 File Offset: 0x00084B35
		// (set) Token: 0x06001671 RID: 5745 RVA: 0x00086940 File Offset: 0x00084B40
		public bool Hidden
		{
			get
			{
				return this.hidden;
			}
			set
			{
				this.hidden = value;
				foreach (IntVec3 loc in this.Cells)
				{
					this.Map.mapDrawer.MapMeshDirty(loc, MapMeshFlagDefOf.Plan);
				}
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001672 RID: 5746 RVA: 0x000869B0 File Offset: 0x00084BB0
		// (set) Token: 0x06001673 RID: 5747 RVA: 0x000869B8 File Offset: 0x00084BB8
		public ColorDef Color
		{
			get
			{
				return this.color;
			}
			private set
			{
				if (this.color == value)
				{
					return;
				}
				this.color = value;
				this.materialInt = this.CreateMaterial();
				this.Dirty();
			}
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x000869E0 File Offset: 0x00084BE0
		public void MergeIn(Plan plan)
		{
			for (int i = plan.Cells.Count - 1; i >= 0; i--)
			{
				IntVec3 c = plan.Cells[i];
				plan.RemoveCell(c);
				this.AddCell(c);
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001675 RID: 5749 RVA: 0x00086A20 File Offset: 0x00084C20
		public Material Material
		{
			get
			{
				if (this.materialInt == null)
				{
					this.materialInt = this.CreateMaterial();
				}
				return this.materialInt;
			}
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x00086A3C File Offset: 0x00084C3C
		protected virtual Material CreateMaterial()
		{
			return MaterialPool.MatFrom(new MaterialRequest
			{
				BaseTexPath = "Designations/Plan",
				shader = ShaderDatabase.MetaOverlay,
				color = this.color.color
			});
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x00086A81 File Offset: 0x00084C81
		public IEnumerator<IntVec3> GetEnumerator()
		{
			return this.cells.GetEnumerator();
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x00086A93 File Offset: 0x00084C93
		public List<IntVec3> Cells
		{
			get
			{
				if (!this.cellsShuffled)
				{
					this.cells.Shuffle<IntVec3>();
					this.cellsShuffled = true;
				}
				return this.cells;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001679 RID: 5753 RVA: 0x00086AB5 File Offset: 0x00084CB5
		public int CellCount
		{
			get
			{
				return this.cells.Count;
			}
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x00086AC2 File Offset: 0x00084CC2
		public Plan()
		{
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x00086ADC File Offset: 0x00084CDC
		public Plan(ColorDef color, PlanManager planManager)
		{
			this.baseLabel = "NewPlan".Translate();
			this.label = planManager.NewPlanName("NewPlan".Translate());
			this.planManager = planManager;
			this.ID = Find.UniqueIDsManager.GetNextPlanID();
			this.Map.planManager.RegisterPlan(this);
			this.color = color;
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x00086B60 File Offset: 0x00084D60
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.baseLabel, "baseLabel", null, false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Defs.Look<ColorDef>(ref this.color, "color");
			Scribe_Collections.Look<IntVec3>(ref this.cells, "cells", LookMode.Undefined, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x00086BE4 File Offset: 0x00084DE4
		public virtual void AddCell(IntVec3 c)
		{
			if (this.cells.Contains(c))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Adding cell to zone which already has it. c=");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(c);
				defaultInterpolatedStringHandler.AppendLiteral(", zone=");
				defaultInterpolatedStringHandler.AppendFormatted<Plan>(this);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			this.cells.Add(c);
			this.planManager.AddPlanGridCell(this, c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Plan);
			this.cellsShuffled = false;
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x00086C7C File Offset: 0x00084E7C
		public virtual void RemoveCell(IntVec3 c)
		{
			if (!this.cells.Contains(c))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot remove cell from zone which doesn't have it. c=");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(c);
				defaultInterpolatedStringHandler.AppendLiteral(", zone=");
				defaultInterpolatedStringHandler.AppendFormatted<Plan>(this);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			this.cells.Remove(c);
			this.planManager.ClearPlanGridCell(c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Plan);
			this.cellsShuffled = false;
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x00086D24 File Offset: 0x00084F24
		public void Dirty()
		{
			foreach (IntVec3 loc in this.cells)
			{
				this.Map.mapDrawer.MapMeshDirty(loc, MapMeshFlagDefOf.Plan);
			}
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x00086D8C File Offset: 0x00084F8C
		public void Delete()
		{
			this.Delete(true);
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x00086D98 File Offset: 0x00084F98
		public virtual void Delete(bool playSound)
		{
			if (playSound)
			{
				SoundDefOf.Designate_ZoneDelete.PlayOneShotOnCamera(this.Map);
			}
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
			else
			{
				while (this.cells.Count > 0)
				{
					this.RemoveCell(this.cells.First<IntVec3>());
				}
			}
			Find.Selector.Deselect(this);
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x00086DF7 File Offset: 0x00084FF7
		public void Deregister()
		{
			this.planManager.DeregisterPlan(this);
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x00086E05 File Offset: 0x00085005
		public bool ContainsCell(IntVec3 c)
		{
			return this.cells.Contains(c);
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x00086E14 File Offset: 0x00085014
		public virtual string GetInspectString()
		{
			int num = 0;
			for (int i = 0; i < this.planManager.AllPlans.Count; i++)
			{
				Plan plan = this.planManager.AllPlans[i];
				num += plan.CellCount;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Size".Translate().CapitalizeFirst());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.CellCount);
			string text = defaultInterpolatedStringHandler.ToStringAndClear();
			if (this.planManager.AllPlans.Count > 1)
			{
				string str = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("\n");
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("TotalPlanningArea".Translate().CapitalizeFirst());
				defaultInterpolatedStringHandler2.AppendLiteral(": ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(num);
				text = str + defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			return text;
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x00086F02 File Offset: 0x00085102
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return null;
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x00086F05 File Offset: 0x00085105
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			Plan.<GetGizmos>d__45 <GetGizmos>d__ = new Plan.<GetGizmos>d__45(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x00086F18 File Offset: 0x00085118
		public void CheckContiguous()
		{
			Plan.<>c__DisplayClass49_0 CS$<>8__locals1 = new Plan.<>c__DisplayClass49_0();
			CS$<>8__locals1.<>4__this = this;
			if (this.cells.Count == 0)
			{
				return;
			}
			if (Plan.extantGrid == null)
			{
				Plan.extantGrid = new BoolGrid(this.Map);
			}
			else
			{
				Plan.extantGrid.ClearAndResizeTo(this.Map);
			}
			if (Plan.foundGrid == null)
			{
				Plan.foundGrid = new BoolGrid(this.Map);
			}
			else
			{
				Plan.foundGrid.ClearAndResizeTo(this.Map);
			}
			foreach (IntVec3 c in this.Map.AllCells)
			{
				Plan plan = c.GetPlan(this.Map);
				if (plan != null && plan == this)
				{
					Plan.extantGrid.Set(c, true);
				}
			}
			CS$<>8__locals1.numFound = 0;
			this.Map.floodFiller.FloodFill(this.cells.First<IntVec3>(), new Predicate<IntVec3>(Plan.<CheckContiguous>g__PassCheck|49_0), new Action<IntVec3>(CS$<>8__locals1.<CheckContiguous>g__Processor|1), int.MaxValue, false, null);
			if (CS$<>8__locals1.numFound < this.cells.Count)
			{
				foreach (IntVec3 intVec in this.Map.AllCells)
				{
					if (Plan.extantGrid[intVec] && !Plan.foundGrid[intVec])
					{
						FloodFiller floodFiller = this.Map.floodFiller;
						IntVec3 root = intVec;
						Predicate<IntVec3> passCheck = (IntVec3 cell) => Plan.extantGrid[cell] && !Plan.foundGrid[cell];
						Action<IntVec3> processor;
						if ((processor = CS$<>8__locals1.<>9__3) == null)
						{
							processor = (CS$<>8__locals1.<>9__3 = delegate(IntVec3 cell)
							{
								if (CS$<>8__locals1.<>4__this.Map.planManager.PlanAt(cell) == CS$<>8__locals1.<>4__this)
								{
									Plan.toSplit.Add(cell);
								}
								Plan.extantGrid[cell] = false;
							});
						}
						floodFiller.FloodFill(root, passCheck, processor, int.MaxValue, false, null);
						Plan plan2 = null;
						foreach (IntVec3 c2 in Plan.toSplit)
						{
							if (plan2 == null)
							{
								plan2 = new Plan(this.color, this.planManager);
							}
							plan2.RenamableLabel = this.RenamableLabel;
							this.RemoveCell(c2);
							plan2.AddCell(c2);
						}
						Plan.toSplit.Clear();
					}
				}
			}
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x000871A8 File Offset: 0x000853A8
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			return new Plan.<GetFloatMenuOptions>d__50(-2);
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x000871B1 File Offset: 0x000853B1
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x000871BC File Offset: 0x000853BC
		public string GetUniqueLoadID()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Plan_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.ID);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x000872C0 File Offset: 0x000854C0
		[CompilerGenerated]
		internal static bool <CheckContiguous>g__PassCheck|49_0(IntVec3 c)
		{
			return Plan.extantGrid[c] && !Plan.foundGrid[c];
		}

		// Token: 0x04001354 RID: 4948
		public PlanManager planManager;

		// Token: 0x04001355 RID: 4949
		private ColorDef color;

		// Token: 0x04001356 RID: 4950
		private int ID = -1;

		// Token: 0x04001357 RID: 4951
		public string label;

		// Token: 0x04001358 RID: 4952
		private string baseLabel;

		// Token: 0x04001359 RID: 4953
		private List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x0400135A RID: 4954
		private bool hidden;

		// Token: 0x0400135B RID: 4955
		private Material materialInt;

		// Token: 0x0400135C RID: 4956
		private bool cellsShuffled;

		// Token: 0x0400135D RID: 4957
		private static BoolGrid extantGrid;

		// Token: 0x0400135E RID: 4958
		private static BoolGrid foundGrid;

		// Token: 0x0400135F RID: 4959
		private static readonly HashSet<IntVec3> toSplit = new HashSet<IntVec3>();
	}
}
