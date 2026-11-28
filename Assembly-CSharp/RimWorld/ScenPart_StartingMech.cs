using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017B9 RID: 6073
	public class ScenPart_StartingMech : ScenPart
	{
		// Token: 0x170019B8 RID: 6584
		// (get) Token: 0x06009773 RID: 38771 RVA: 0x0036F3C9 File Offset: 0x0036D5C9
		private IEnumerable<PawnKindDef> PossibleMechs
		{
			get
			{
				return from td in DefDatabase<PawnKindDef>.AllDefs
				where td.RaceProps.IsMechanoid && td.race.GetCompProperties<CompProperties_OverseerSubject>() != null
				select td;
			}
		}

		// Token: 0x06009774 RID: 38772 RVA: 0x0036F3F4 File Offset: 0x0036D5F4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.mechKind, "mechKind");
			Scribe_Values.Look<float>(ref this.overseenByPlayerPawnChance, "overseenByPlayerPawnChance", 0f, false);
		}

		// Token: 0x06009775 RID: 38773 RVA: 0x0036F424 File Offset: 0x0036D624
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, 2f * ScenPart.RowHeight + 4f);
			if (Widgets.ButtonText(new Rect(scenPartRect.xMin, scenPartRect.yMin, scenPartRect.width, ScenPart.RowHeight), (this.mechKind != null) ? this.mechKind.LabelCap : "RandomMech".Translate(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("RandomMech".Translate().CapitalizeFirst(), delegate()
				{
					this.mechKind = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				foreach (PawnKindDef localKind2 in this.PossibleMechs)
				{
					PawnKindDef localKind = localKind2;
					list.Add(new FloatMenuOption(localKind.LabelCap, delegate()
					{
						this.mechKind = localKind;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			Rect rect = new Rect(scenPartRect.xMin, scenPartRect.yMin + ScenPart.RowHeight + 4f, scenPartRect.width, ScenPart.RowHeight);
			TaggedString taggedString = "StartOverseenChance".Translate();
			float x = Text.CalcSize(taggedString).x;
			Widgets.Label(new Rect(rect.xMin, rect.yMin, x, ScenPart.RowHeight), taggedString);
			Rect rect2 = new Rect(rect.xMin + x + 4f, rect.yMin, rect.width - x - 4f, ScenPart.RowHeight);
			this.overseenByPlayerPawnChance = Widgets.HorizontalSlider(rect2, this.overseenByPlayerPawnChance, 0f, 1f, false, this.overseenByPlayerPawnChance.ToStringPercent(), null, null, 0.01f);
		}

		// Token: 0x06009776 RID: 38774 RVA: 0x0036F650 File Offset: 0x0036D850
		public override void Randomize()
		{
			this.mechKind = this.PossibleMechs.RandomElement<PawnKindDef>();
			this.overseenByPlayerPawnChance = Rand.Range(0f, 1f);
		}

		// Token: 0x06009777 RID: 38775 RVA: 0x0036F678 File Offset: 0x0036D878
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x06009778 RID: 38776 RVA: 0x0036F68A File Offset: 0x0036D88A
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart_StartingMech.<GetSummaryListEntries>d__8 <GetSummaryListEntries>d__ = new ScenPart_StartingMech.<GetSummaryListEntries>d__8(-2);
			<GetSummaryListEntries>d__.<>4__this = this;
			<GetSummaryListEntries>d__.<>3__tag = tag;
			return <GetSummaryListEntries>d__;
		}

		// Token: 0x06009779 RID: 38777 RVA: 0x0036F6A1 File Offset: 0x0036D8A1
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			ScenPart_StartingMech.<PlayerStartingThings>d__9 <PlayerStartingThings>d__ = new ScenPart_StartingMech.<PlayerStartingThings>d__9(-2);
			<PlayerStartingThings>d__.<>4__this = this;
			return <PlayerStartingThings>d__;
		}

		// Token: 0x0600977A RID: 38778 RVA: 0x0036F6B1 File Offset: 0x0036D8B1
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.mechKind != null) ? this.mechKind.GetHashCode() : 0);
		}

		// Token: 0x04005ABA RID: 23226
		private PawnKindDef mechKind;

		// Token: 0x04005ABB RID: 23227
		private float overseenByPlayerPawnChance = 0.5f;
	}
}
