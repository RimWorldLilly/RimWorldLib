using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C8A RID: 7306
	public class Designator_PlantsHarvest : Designator_Plants
	{
		// Token: 0x0600BA7D RID: 47741 RVA: 0x0041B914 File Offset: 0x00419B14
		public Designator_PlantsHarvest()
		{
			this.defaultLabel = "DesignatorHarvest".Translate();
			this.defaultDesc = "DesignatorHarvestDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Harvest", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_HarvestPlants;
			this.hotKey = KeyBindingDefOf.Misc2;
			this.designationDef = DesignationDefOf.HarvestPlant;
		}

		// Token: 0x0600BA7E RID: 47742 RVA: 0x0041B9A0 File Offset: 0x00419BA0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			Plant plant = (Plant)t;
			if (!plant.HarvestableNow || plant.def.plant.harvestTag != "Standard")
			{
				return "MessageMustDesignateHarvestable".Translate();
			}
			return true;
		}

		// Token: 0x0600BA7F RID: 47743 RVA: 0x0041BA01 File Offset: 0x00419C01
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return target.Thing.def.plant.harvestTag == "Standard";
		}
	}
}
