using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C8B RID: 7307
	public class Designator_PlantsHarvestWood : Designator_Plants
	{
		// Token: 0x170021D8 RID: 8664
		// (get) Token: 0x0600BA80 RID: 47744 RVA: 0x0041BA24 File Offset: 0x00419C24
		private bool CanDesignateStumpsNow
		{
			get
			{
				if (Find.DesignatorManager.SelectedDesignator != this)
				{
					return true;
				}
				foreach (IntVec3 c in Find.DesignatorManager.Dragger.DragCells)
				{
					Plant plant = c.GetPlant(base.Map);
					if (plant != null && plant.def.plant.IsTree && !plant.def.plant.isStump && plant.HarvestableNow && base.Map.designationManager.DesignationOn(plant, this.designationDef) == null)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x0600BA81 RID: 47745 RVA: 0x0041BAE4 File Offset: 0x00419CE4
		public Designator_PlantsHarvestWood()
		{
			this.defaultLabel = "DesignatorHarvestWood".Translate();
			this.defaultDesc = "DesignatorHarvestWoodDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HarvestWood", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_HarvestWood;
			this.hotKey = KeyBindingDefOf.Misc1;
			this.designationDef = DesignationDefOf.HarvestPlant;
			this.tutorTag = "PlantsHarvestWood";
		}

		// Token: 0x0600BA82 RID: 47746 RVA: 0x0041BB7C File Offset: 0x00419D7C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			Plant plant = (Plant)t;
			if (!plant.HarvestableNow || !plant.def.plant.IsTree)
			{
				return "MessageMustDesignateHarvestableWood".Translate();
			}
			CompPlantPreventCutting compPlantPreventCutting;
			if (t.TryGetComp(out compPlantPreventCutting) && compPlantPreventCutting.PreventCutting)
			{
				return "MessageMustPlantCuttingForbidden".Translate();
			}
			if (t.def.plant.isStump && !this.CanDesignateStumpsNow)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600BA83 RID: 47747 RVA: 0x0041BC16 File Offset: 0x00419E16
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return target.Thing.def.plant.IsTree;
		}

		// Token: 0x0600BA84 RID: 47748 RVA: 0x0041BC2E File Offset: 0x00419E2E
		public override void DesignateThing(Thing t)
		{
			Designator_PlantsHarvestWood.PossiblyWarnPlayerImportantPlantDesignateCut(t);
			if (ModsConfig.IdeologyActive && t.def.plant.IsTree && t.def.plant.treeLoversCareIfChopped)
			{
				Designator_PlantsHarvestWood.PossiblyWarnPlayerOnDesignatingTreeCut();
			}
			base.DesignateThing(t);
		}

		// Token: 0x0600BA85 RID: 47749 RVA: 0x0041BC70 File Offset: 0x00419E70
		public static void PossiblyWarnPlayerImportantPlantDesignateCut(Thing t)
		{
			Plant plant = t as Plant;
			if (plant != null && plant.def.plant.warnIfMarkedForCut && (!plant.HarvestableNow || plant.def.plant.IsTree))
			{
				Messages.Message("MessageWarningCutImportantPlant".Translate(plant.LabelCap), plant, MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x0600BA86 RID: 47750 RVA: 0x0041BCE0 File Offset: 0x00419EE0
		public static void PossiblyWarnPlayerOnDesignatingTreeCut()
		{
			Designator_PlantsHarvestWood.tmpIdeoMemberNames.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				if (ideo.WarnPlayerOnDesignateChopTree)
				{
					Designator_PlantsHarvestWood.tmpIdeoMemberNames.Add(Find.ActiveLanguageWorker.Pluralize(ideo.memberName, -1));
				}
			}
			if (Designator_PlantsHarvestWood.tmpIdeoMemberNames.Any<string>())
			{
				Messages.Message("MessageWarningPlayerDesignatedTreeChopped".Translate(Designator_PlantsHarvestWood.tmpIdeoMemberNames.ToCommaList(true, false)), MessageTypeDefOf.CautionInput, false);
			}
			Designator_PlantsHarvestWood.tmpIdeoMemberNames.Clear();
		}

		// Token: 0x040069F8 RID: 27128
		private static readonly List<string> tmpIdeoMemberNames = new List<string>();
	}
}
