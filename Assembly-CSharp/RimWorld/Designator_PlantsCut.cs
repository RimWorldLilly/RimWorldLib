using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C89 RID: 7305
	[StaticConstructorOnStartup]
	public class Designator_PlantsCut : Designator_Plants
	{
		// Token: 0x0600BA77 RID: 47735 RVA: 0x0041B79C File Offset: 0x0041999C
		public Designator_PlantsCut()
		{
			this.defaultLabel = "DesignatorCutPlants".Translate();
			this.defaultDesc = "DesignatorCutPlantsDesc".Translate();
			this.icon = Designator_PlantsCut.IconTex;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_CutPlants;
			this.hotKey = KeyBindingDefOf.Misc3;
			this.designationDef = DesignationDefOf.CutPlant;
		}

		// Token: 0x0600BA78 RID: 47736 RVA: 0x0041B824 File Offset: 0x00419A24
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			CompPlantPreventCutting compPlantPreventCutting;
			if (t.TryGetComp(out compPlantPreventCutting) && compPlantPreventCutting.PreventCutting)
			{
				return "MessageMustPlantCuttingForbidden".Translate();
			}
			return this.AffectsThing(t);
		}

		// Token: 0x0600BA79 RID: 47737 RVA: 0x0041B872 File Offset: 0x00419A72
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return this.AffectsThing(target.Thing);
		}

		// Token: 0x0600BA7A RID: 47738 RVA: 0x0041B884 File Offset: 0x00419A84
		private bool AffectsThing(Thing t)
		{
			Plant plant = t as Plant;
			return plant != null && (this.isOrder || !plant.def.plant.IsTree || !plant.HarvestableNow);
		}

		// Token: 0x0600BA7B RID: 47739 RVA: 0x0041B8C2 File Offset: 0x00419AC2
		public override void DesignateThing(Thing t)
		{
			Designator_PlantsHarvestWood.PossiblyWarnPlayerImportantPlantDesignateCut(t);
			if (ModsConfig.IdeologyActive && t.def.plant.IsTree && t.def.plant.treeLoversCareIfChopped)
			{
				Designator_PlantsHarvestWood.PossiblyWarnPlayerOnDesignatingTreeCut();
			}
			base.DesignateThing(t);
		}

		// Token: 0x040069F7 RID: 27127
		public static readonly Texture2D IconTex = ContentFinder<Texture2D>.Get("UI/Designators/CutPlants", true);
	}
}
