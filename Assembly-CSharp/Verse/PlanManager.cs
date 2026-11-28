using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002E4 RID: 740
	public sealed class PlanManager : IExposable
	{
		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x0600168E RID: 5774 RVA: 0x000872E1 File Offset: 0x000854E1
		public List<Plan> AllPlans
		{
			get
			{
				return this.allPlans;
			}
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x000872E9 File Offset: 0x000854E9
		public PlanManager(Map map)
		{
			this.map = map;
			this.planGrid = new Plan[map.cellIndices.NumGridCells];
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00087319 File Offset: 0x00085519
		public void ExposeData()
		{
			Scribe_Collections.Look<Plan>(ref this.allPlans, "allPlans", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdatePlanManagerLinks();
				this.RebuildPlanGrid();
			}
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x00087348 File Offset: 0x00085548
		private void UpdatePlanManagerLinks()
		{
			for (int i = 0; i < this.allPlans.Count; i++)
			{
				this.allPlans[i].planManager = this;
			}
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x00087380 File Offset: 0x00085580
		private void RebuildPlanGrid()
		{
			CellIndices cellIndices = this.map.cellIndices;
			this.planGrid = new Plan[cellIndices.NumGridCells];
			foreach (Plan plan in this.allPlans)
			{
				foreach (IntVec3 c in plan)
				{
					this.planGrid[cellIndices.CellToIndex(c)] = plan;
				}
			}
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x0008742C File Offset: 0x0008562C
		public void RegisterPlan(Plan newPlan)
		{
			this.allPlans.Add(newPlan);
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x0008743A File Offset: 0x0008563A
		public void DeregisterPlan(Plan oldPlan)
		{
			this.allPlans.Remove(oldPlan);
			if (Find.Selector.SelectedPlan == oldPlan)
			{
				Find.Selector.ClearSelection();
			}
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x00087460 File Offset: 0x00085660
		internal void AddPlanGridCell(Plan plan, IntVec3 c)
		{
			this.planGrid[this.map.cellIndices.CellToIndex(c)] = plan;
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x0008747B File Offset: 0x0008567B
		internal void ClearPlanGridCell(IntVec3 c)
		{
			this.planGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x00087496 File Offset: 0x00085696
		public Plan PlanAt(IntVec3 c)
		{
			return this.planGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x000874B0 File Offset: 0x000856B0
		public bool TryGetPlan(IntVec3 cell, out Plan plan)
		{
			plan = this.planGrid[this.map.cellIndices.CellToIndex(cell)];
			return plan != null;
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x000874D4 File Offset: 0x000856D4
		public string NewPlanName(string nameBase)
		{
			for (int i = 1; i <= 1000; i++)
			{
				string cand = nameBase + " " + i.ToString();
				if (!this.allPlans.Any((Plan z) => z.label == cand))
				{
					return cand;
				}
			}
			Log.Error("Ran out of plan names.");
			return "Plan X";
		}

		// Token: 0x04001360 RID: 4960
		public readonly Map map;

		// Token: 0x04001361 RID: 4961
		private List<Plan> allPlans = new List<Plan>();

		// Token: 0x04001362 RID: 4962
		private Plan[] planGrid;
	}
}
