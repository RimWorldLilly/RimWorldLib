using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C88 RID: 7304
	public abstract class Designator_Plants : Designator
	{
		// Token: 0x170021D6 RID: 8662
		// (get) Token: 0x0600BA6F RID: 47727 RVA: 0x0041B68B File Offset: 0x0041988B
		protected override DesignationDef Designation
		{
			get
			{
				return this.designationDef;
			}
		}

		// Token: 0x170021D7 RID: 8663
		// (get) Token: 0x0600BA70 RID: 47728 RVA: 0x0041B693 File Offset: 0x00419893
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Plants;
			}
		}

		// Token: 0x0600BA71 RID: 47729 RVA: 0x0041B69A File Offset: 0x0041989A
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (t.def.plant == null)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationOn(t, this.designationDef) != null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600BA72 RID: 47730 RVA: 0x0041B6D8 File Offset: 0x004198D8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map) || c.Fogged(base.Map))
			{
				return false;
			}
			Plant plant = c.GetPlant(base.Map);
			if (plant == null)
			{
				return "MessageMustDesignatePlants".Translate();
			}
			AcceptanceReport result = this.CanDesignateThing(plant);
			if (!result.Accepted)
			{
				return result;
			}
			return true;
		}

		// Token: 0x0600BA73 RID: 47731 RVA: 0x0041B741 File Offset: 0x00419941
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.DesignateThing(c.GetPlant(base.Map));
		}

		// Token: 0x0600BA74 RID: 47732 RVA: 0x0041B755 File Offset: 0x00419955
		public override void DesignateThing(Thing t)
		{
			base.Map.designationManager.RemoveAllDesignationsOn(t, false);
			base.Map.designationManager.AddDesignation(new Designation(t, this.designationDef, null));
		}

		// Token: 0x0600BA75 RID: 47733 RVA: 0x0041B78B File Offset: 0x0041998B
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x040069F6 RID: 27126
		protected DesignationDef designationDef;
	}
}
