using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DCC RID: 7628
	public static class DrugPolicyUIUtility
	{
		// Token: 0x0600C634 RID: 50740 RVA: 0x0048A7A8 File Offset: 0x004889A8
		public static void DoAssignDrugPolicyButtons(Rect rect, Pawn pawn)
		{
			if (pawn.drugs.CurrentPolicy == null)
			{
				return;
			}
			Rect rect2 = rect.ContractedBy(0f, 2f);
			string text = pawn.drugs.CurrentPolicy.label;
			Pawn_StoryTracker story = pawn.story;
			if (((story != null) ? story.traits : null) != null)
			{
				Trait trait = pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
				Gene_ChemicalDependency gene_ChemicalDependency;
				if (trait != null)
				{
					text = text + " (" + trait.Label + ")";
				}
				else if (ModsConfig.BiotechActive && PawnUtility.TryGetChemicalDependencyGene(pawn, out gene_ChemicalDependency))
				{
					text = text + " (" + gene_ChemicalDependency.Label + ")";
				}
			}
			Widgets.Dropdown<Pawn, DrugPolicy>(rect2, pawn, (Pawn p) => p.drugs.CurrentPolicy, new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>>>(DrugPolicyUIUtility.Button_GenerateMenu), text.Truncate(rect2.width, null), null, pawn.drugs.CurrentPolicy.label, null, delegate()
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
			}, true);
			UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignDrugs");
		}

		// Token: 0x0600C635 RID: 50741 RVA: 0x0048A8D0 File Offset: 0x00488AD0
		private static IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>> Button_GenerateMenu(Pawn pawn)
		{
			DrugPolicyUIUtility.<Button_GenerateMenu>d__2 <Button_GenerateMenu>d__ = new DrugPolicyUIUtility.<Button_GenerateMenu>d__2(-2);
			<Button_GenerateMenu>d__.<>3__pawn = pawn;
			return <Button_GenerateMenu>d__;
		}

		// Token: 0x040070F3 RID: 28915
		public const string AssigningDrugsTutorHighlightTag = "ButtonAssignDrugs";
	}
}
