using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001794 RID: 6036
	public class ScenPart_DisallowBuilding : ScenPart_Rule
	{
		// Token: 0x06009665 RID: 38501 RVA: 0x00369D5D File Offset: 0x00367F5D
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowBuilding(this.building, false);
		}

		// Token: 0x06009666 RID: 38502 RVA: 0x00369D75 File Offset: 0x00367F75
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "DisallowBuilding", "ScenPart_DisallowBuilding".Translate());
		}

		// Token: 0x06009667 RID: 38503 RVA: 0x00369D91 File Offset: 0x00367F91
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart_DisallowBuilding.<GetSummaryListEntries>d__4 <GetSummaryListEntries>d__ = new ScenPart_DisallowBuilding.<GetSummaryListEntries>d__4(-2);
			<GetSummaryListEntries>d__.<>4__this = this;
			<GetSummaryListEntries>d__.<>3__tag = tag;
			return <GetSummaryListEntries>d__;
		}

		// Token: 0x06009668 RID: 38504 RVA: 0x00369DA8 File Offset: 0x00367FA8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.building, "building");
		}

		// Token: 0x06009669 RID: 38505 RVA: 0x00369DC0 File Offset: 0x00367FC0
		public override void Randomize()
		{
			this.building = this.RandomizableBuildingDefs().RandomElement<ThingDef>();
		}

		// Token: 0x0600966A RID: 38506 RVA: 0x00369DD4 File Offset: 0x00367FD4
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.building.LabelCap, true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (ThingDef localTd2 in from t in this.PossibleBuildingDefs()
				orderby t.label
				select t)
				{
					ThingDef localTd = localTd2;
					list.Add(new FloatMenuOption(localTd.LabelCap, delegate()
					{
						this.building = localTd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600966B RID: 38507 RVA: 0x00369ED4 File Offset: 0x003680D4
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_DisallowBuilding scenPart_DisallowBuilding = other as ScenPart_DisallowBuilding;
			return scenPart_DisallowBuilding != null && scenPart_DisallowBuilding.building == this.building;
		}

		// Token: 0x0600966C RID: 38508 RVA: 0x00369EFC File Offset: 0x003680FC
		protected virtual IEnumerable<ThingDef> PossibleBuildingDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && d.BuildableByPlayer
			select d;
		}

		// Token: 0x0600966D RID: 38509 RVA: 0x00369F27 File Offset: 0x00368127
		private IEnumerable<ThingDef> RandomizableBuildingDefs()
		{
			return new ScenPart_DisallowBuilding.<RandomizableBuildingDefs>d__10(-2);
		}

		// Token: 0x0600966E RID: 38510 RVA: 0x00369F30 File Offset: 0x00368130
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.building == null;
		}

		// Token: 0x0600966F RID: 38511 RVA: 0x00369F45 File Offset: 0x00368145
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.building != null) ? this.building.GetHashCode() : 0);
		}

		// Token: 0x04005A4B RID: 23115
		private ThingDef building;

		// Token: 0x04005A4C RID: 23116
		private const string DisallowBuildingTag = "DisallowBuilding";
	}
}
