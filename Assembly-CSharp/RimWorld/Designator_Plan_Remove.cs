using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C7A RID: 7290
	public class Designator_Plan_Remove : Designator_Plan
	{
		// Token: 0x170021C3 RID: 8643
		// (get) Token: 0x0600BA34 RID: 47668 RVA: 0x0041A5DD File Offset: 0x004187DD
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.RemovePlans;
			}
		}

		// Token: 0x0600BA35 RID: 47669 RVA: 0x0041A5E4 File Offset: 0x004187E4
		public Designator_Plan_Remove()
		{
			this.defaultLabel = "DesignatorPlanRemove".Translate();
			this.defaultDesc = "DesignatorPlanRemoveDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/PlanOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_PlanRemove;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc8;
		}

		// Token: 0x0600BA36 RID: 47670 RVA: 0x0041A66C File Offset: 0x0041886C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (base.Map.planManager.PlanAt(c) == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600BA37 RID: 47671 RVA: 0x0041A6A4 File Offset: 0x004188A4
		public override void DesignateSingleCell(IntVec3 c)
		{
			Plan plan = base.Map.planManager.PlanAt(c);
			plan.RemoveCell(c);
			this.justDesignated.Add(plan);
		}

		// Token: 0x0600BA38 RID: 47672 RVA: 0x0041A6D8 File Offset: 0x004188D8
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			foreach (Plan plan in this.justDesignated)
			{
				plan.CheckContiguous();
			}
			this.justDesignated.Clear();
		}

		// Token: 0x040069EE RID: 27118
		private readonly HashSet<Plan> justDesignated = new HashSet<Plan>();
	}
}
