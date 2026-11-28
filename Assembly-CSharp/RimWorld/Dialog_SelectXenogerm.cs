using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CC8 RID: 7368
	public class Dialog_SelectXenogerm : Window
	{
		// Token: 0x1700227B RID: 8827
		// (get) Token: 0x0600BD56 RID: 48470 RVA: 0x0043EBF0 File Offset: 0x0043CDF0
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 600f);
			}
		}

		// Token: 0x0600BD57 RID: 48471 RVA: 0x0043EC04 File Offset: 0x0043CE04
		public Dialog_SelectXenogerm(Pawn pawn, Map map, Xenogerm initialSelected, Action<Xenogerm> onSelect) : base(null)
		{
			this.pawn = pawn;
			this.onSelect = onSelect;
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.Xenogerm))
			{
				if (!thing.PositionHeld.Fogged(map))
				{
					this.xenogerms.Add((Xenogerm)thing);
				}
			}
			if (initialSelected != null && this.xenogerms.Contains(initialSelected))
			{
				this.selected = initialSelected;
			}
			this.closeOnAccept = false;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600BD58 RID: 48472 RVA: 0x0043ECCC File Offset: 0x0043CECC
		public override void PostOpen()
		{
			if (!ModLister.CheckBiotech("xenogerm"))
			{
				this.Close(false);
				return;
			}
			base.PostOpen();
		}

		// Token: 0x0600BD59 RID: 48473 RVA: 0x0043ECE8 File Offset: 0x0043CEE8
		public override void DoWindowContents(Rect rect)
		{
			Rect rect2 = rect;
			rect2.yMax -= Dialog_SelectXenogerm.ButSize.y + 4f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, "SelectXenogerm".Translate());
			Text.Font = GameFont.Small;
			rect2.yMin += 39f;
			this.DisplayXenogerms(rect2);
			Rect rect3 = rect;
			rect3.yMin = rect3.yMax - Dialog_SelectXenogerm.ButSize.y;
			if (this.selected != null)
			{
				if (Widgets.ButtonText(new Rect(rect3.xMax - Dialog_SelectXenogerm.ButSize.x, rect3.y, Dialog_SelectXenogerm.ButSize.x, Dialog_SelectXenogerm.ButSize.y), "Accept".Translate(), true, true, true, null))
				{
					this.Accept();
				}
				if (Widgets.ButtonText(new Rect(rect3.x, rect3.y, Dialog_SelectXenogerm.ButSize.x, Dialog_SelectXenogerm.ButSize.y), "Close".Translate(), true, true, true, null))
				{
					this.Close(true);
					return;
				}
			}
			else if (Widgets.ButtonText(new Rect((rect3.width - Dialog_SelectXenogerm.ButSize.x) / 2f, rect3.y, Dialog_SelectXenogerm.ButSize.x, Dialog_SelectXenogerm.ButSize.y), "Close".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
		}

		// Token: 0x0600BD5A RID: 48474 RVA: 0x0043EE80 File Offset: 0x0043D080
		private void DisplayXenogerms(Rect rect)
		{
			Widgets.DrawMenuSection(rect);
			rect = rect.ContractedBy(4f);
			GUI.BeginGroup(rect);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, this.scrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect.AtZero(), ref this.scrollPosition, viewRect, true);
			for (int i = 0; i < this.xenogerms.Count; i++)
			{
				float num2 = rect.width;
				if (this.scrollViewHeight > rect.height)
				{
					num2 -= 16f;
				}
				this.DrawXenogerm(new Rect(0f, num, num2, 32f), i);
				num += 32f;
			}
			if (Event.current.type == EventType.layout)
			{
				this.scrollViewHeight = num;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
		}

		// Token: 0x0600BD5B RID: 48475 RVA: 0x0043EF58 File Offset: 0x0043D158
		private void DrawXenogerm(Rect rect, int index)
		{
			Xenogerm xenogerm = this.xenogerms[index];
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
			if (this.selected == xenogerm)
			{
				Widgets.DrawHighlightSelected(rect);
			}
			Widgets.InfoCardButton(rect.xMax - 24f, rect.y + 4f, xenogerm);
			rect.xMax -= 36f;
			for (int i = Mathf.Min(xenogerm.GeneSet.GenesListForReading.Count, 10) - 1; i >= 0; i--)
			{
				GeneDef geneDef = xenogerm.GeneSet.GenesListForReading[i];
				Rect rect2 = new Rect(rect.xMax - 11f, rect.yMax - rect.height / 2f - 11f, 22f, 22f);
				Widgets.DefIcon(rect2, geneDef, null, 1.25f, null, false, null, null, null, 1f);
				Rect rect3 = rect2;
				rect3.yMin = rect.yMin;
				rect3.yMax = rect.yMax;
				if (Mouse.IsOver(rect3))
				{
					Widgets.DrawHighlight(rect3);
					TooltipHandler.TipRegion(rect3, geneDef.LabelCap + "\n\n" + geneDef.DescriptionFull);
				}
				rect.xMax -= 22f;
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, () => xenogerm.LabelCap + "\n\n" + "Genes".Translate().CapitalizeFirst() + ":\n" + (from x in xenogerm.GeneSet.GenesListForReading
				select x.LabelCap.ToString()).ToLineList("  - ", false), 128921381);
			}
			rect.xMin += 4f;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, xenogerm.LabelCap.Truncate(rect.width, this.truncateCache));
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.selected = xenogerm;
			}
		}

		// Token: 0x0600BD5C RID: 48476 RVA: 0x0043F164 File Offset: 0x0043D364
		private void Accept()
		{
			if (this.pawn != null)
			{
				int num = GeneUtility.MetabolismAfterImplanting(this.pawn, this.selected.GeneSet);
				if (num < GeneTuning.BiostatRange.TrueMin)
				{
					Messages.Message("OrderImplantationIntoPawn".Translate(this.pawn.Named("PAWN")).Resolve().UncapitalizeFirst() + ": " + "ResultingMetTooLow".Translate() + " (" + num.ToString() + ")", this.pawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				if (this.selected.PawnIdeoDisallowsImplanting(this.pawn))
				{
					Messages.Message("CannotGenericWorkCustom".Translate("OrderImplantationIntoPawn".Translate(this.pawn.Named("PAWN")).Resolve().UncapitalizeFirst() + ": " + "IdeoligionForbids".Translate()), this.pawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
			}
			if (this.onSelect != null)
			{
				this.onSelect(this.selected);
			}
			this.Close(true);
		}

		// Token: 0x04006C44 RID: 27716
		private Pawn pawn;

		// Token: 0x04006C45 RID: 27717
		private List<Xenogerm> xenogerms = new List<Xenogerm>();

		// Token: 0x04006C46 RID: 27718
		private Xenogerm selected;

		// Token: 0x04006C47 RID: 27719
		private Vector2 scrollPosition;

		// Token: 0x04006C48 RID: 27720
		private float scrollViewHeight;

		// Token: 0x04006C49 RID: 27721
		private Action<Xenogerm> onSelect;

		// Token: 0x04006C4A RID: 27722
		private const float HeaderHeight = 35f;

		// Token: 0x04006C4B RID: 27723
		public const float MiniGeneIconSize = 22f;

		// Token: 0x04006C4C RID: 27724
		private const float XenogermElementHeight = 32f;

		// Token: 0x04006C4D RID: 27725
		private const int MaxDisplayedGenes = 10;

		// Token: 0x04006C4E RID: 27726
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x04006C4F RID: 27727
		private Dictionary<string, string> truncateCache = new Dictionary<string, string>();
	}
}
