using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DD2 RID: 7634
	public class PawnColumnWorker_Outfit : PawnColumnWorker
	{
		// Token: 0x0600C661 RID: 50785 RVA: 0x0048B08C File Offset: 0x0048928C
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageApparelPolicies".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ManageApparelPolicies(null));
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Outfits, KnowledgeAmount.Total);
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageOutfits");
		}

		// Token: 0x0600C662 RID: 50786 RVA: 0x0048B128 File Offset: 0x00489328
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.outfits == null)
			{
				return;
			}
			Rect rect2 = rect.ContractedBy(0f, 2f);
			bool somethingIsForced = pawn.outfits.forcedHandler.SomethingIsForced;
			Rect rect3 = rect2;
			Rect rect4 = default(Rect);
			if (somethingIsForced)
			{
				rect2.SplitVerticallyWithMargin(out rect3, out rect4, 4f);
			}
			if (pawn.IsQuestLodger())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, "Unchangeable".Translate().Truncate(rect3.width, null));
				TooltipHandler.TipRegionByKey(rect3, "QuestRelated_Outfit");
				Text.Anchor = TextAnchor.UpperLeft;
			}
			else
			{
				Widgets.Dropdown<Pawn, ApparelPolicy>(rect3, pawn, (Pawn p) => p.outfits.CurrentApparelPolicy, new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<ApparelPolicy>>>(this.Button_GenerateMenu), pawn.outfits.CurrentApparelPolicy.label.Truncate(rect3.width, null), null, pawn.outfits.CurrentApparelPolicy.label, null, null, true);
			}
			if (somethingIsForced)
			{
				if (Widgets.ButtonText(rect4, "ClearForcedApparel".Translate(), true, true, true, null))
				{
					pawn.outfits.forcedHandler.Reset();
				}
				if (Mouse.IsOver(rect4))
				{
					TooltipHandler.TipRegion(rect4, new TipSignal(delegate()
					{
						string text = "ForcedApparel".Translate() + ":\n";
						foreach (Apparel apparel in pawn.outfits.forcedHandler.ForcedApparel)
						{
							text = text + "\n   " + apparel.LabelCap;
						}
						return text;
					}, pawn.GetHashCode() * 612));
				}
			}
		}

		// Token: 0x0600C663 RID: 50787 RVA: 0x0048B2B7 File Offset: 0x004894B7
		private IEnumerable<Widgets.DropdownMenuElement<ApparelPolicy>> Button_GenerateMenu(Pawn pawn)
		{
			PawnColumnWorker_Outfit.<Button_GenerateMenu>d__4 <Button_GenerateMenu>d__ = new PawnColumnWorker_Outfit.<Button_GenerateMenu>d__4(-2);
			<Button_GenerateMenu>d__.<>3__pawn = pawn;
			return <Button_GenerateMenu>d__;
		}

		// Token: 0x0600C664 RID: 50788 RVA: 0x0048B2C7 File Offset: 0x004894C7
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x0600C665 RID: 50789 RVA: 0x0048B2DF File Offset: 0x004894DF
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(251f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600C666 RID: 50790 RVA: 0x0048B2FE File Offset: 0x004894FE
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x0600C667 RID: 50791 RVA: 0x0048B310 File Offset: 0x00489510
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C668 RID: 50792 RVA: 0x0048B333 File Offset: 0x00489533
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.outfits != null && pawn.outfits.CurrentApparelPolicy != null)
			{
				return pawn.outfits.CurrentApparelPolicy.id;
			}
			return int.MinValue;
		}

		// Token: 0x040070FC RID: 28924
		public const int TopAreaHeight = 65;

		// Token: 0x040070FD RID: 28925
		public const int ManageOutfitsButtonHeight = 32;
	}
}
