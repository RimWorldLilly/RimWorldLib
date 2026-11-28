using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200178A RID: 6026
	public class ScenPart_PermaGameCondition : ScenPart
	{
		// Token: 0x1700199B RID: 6555
		// (get) Token: 0x06009617 RID: 38423 RVA: 0x00368844 File Offset: 0x00366A44
		public override string Label
		{
			get
			{
				return "Permanent".Translate().CapitalizeFirst() + ": " + this.gameCondition.label.CapitalizeFirst();
			}
		}

		// Token: 0x06009618 RID: 38424 RVA: 0x00368888 File Offset: 0x00366A88
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.gameCondition.LabelCap, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<GameConditionDef>(this.AllowedGameConditions(), (GameConditionDef d) => d.LabelCap, (GameConditionDef d) => delegate()
				{
					this.gameCondition = d;
				});
			}
		}

		// Token: 0x06009619 RID: 38425 RVA: 0x003688FA File Offset: 0x00366AFA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<GameConditionDef>(ref this.gameCondition, "gameCondition");
		}

		// Token: 0x0600961A RID: 38426 RVA: 0x00368912 File Offset: 0x00366B12
		public override void Randomize()
		{
			this.gameCondition = this.AllowedGameConditions().RandomElement<GameConditionDef>();
		}

		// Token: 0x0600961B RID: 38427 RVA: 0x00368925 File Offset: 0x00366B25
		private IEnumerable<GameConditionDef> AllowedGameConditions()
		{
			return from d in DefDatabase<GameConditionDef>.AllDefs
			where d.canBePermanent
			select d;
		}

		// Token: 0x0600961C RID: 38428 RVA: 0x00368950 File Offset: 0x00366B50
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PermaGameCondition", "ScenPart_PermaGameCondition".Translate());
		}

		// Token: 0x0600961D RID: 38429 RVA: 0x0036896C File Offset: 0x00366B6C
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart_PermaGameCondition.<GetSummaryListEntries>d__9 <GetSummaryListEntries>d__ = new ScenPart_PermaGameCondition.<GetSummaryListEntries>d__9(-2);
			<GetSummaryListEntries>d__.<>4__this = this;
			<GetSummaryListEntries>d__.<>3__tag = tag;
			return <GetSummaryListEntries>d__;
		}

		// Token: 0x0600961E RID: 38430 RVA: 0x00368984 File Offset: 0x00366B84
		public override void GenerateIntoMap(Map map)
		{
			GameCondition cond = GameConditionMaker.MakeConditionPermanent(this.gameCondition);
			map.gameConditionManager.RegisterCondition(cond);
		}

		// Token: 0x0600961F RID: 38431 RVA: 0x003689AC File Offset: 0x00366BAC
		public override bool CanCoexistWith(ScenPart other)
		{
			if (this.gameCondition == null)
			{
				return true;
			}
			ScenPart_PermaGameCondition scenPart_PermaGameCondition = other as ScenPart_PermaGameCondition;
			return scenPart_PermaGameCondition == null || this.gameCondition.CanCoexistWith(scenPart_PermaGameCondition.gameCondition);
		}

		// Token: 0x06009620 RID: 38432 RVA: 0x003689E3 File Offset: 0x00366BE3
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.gameCondition == null;
		}

		// Token: 0x06009621 RID: 38433 RVA: 0x003689F8 File Offset: 0x00366BF8
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.gameCondition != null) ? this.gameCondition.GetHashCode() : 0);
		}

		// Token: 0x04005A38 RID: 23096
		private GameConditionDef gameCondition;

		// Token: 0x04005A39 RID: 23097
		public const string PermaGameConditionTag = "PermaGameCondition";
	}
}
