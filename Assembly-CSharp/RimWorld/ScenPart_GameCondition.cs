using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001789 RID: 6025
	public class ScenPart_GameCondition : ScenPart
	{
		// Token: 0x1700199A RID: 6554
		// (get) Token: 0x0600960B RID: 38411 RVA: 0x00368673 File Offset: 0x00366873
		public override string Label
		{
			get
			{
				return this.def.gameCondition.LabelCap;
			}
		}

		// Token: 0x0600960C RID: 38412 RVA: 0x0036868A File Offset: 0x0036688A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.durationDays, "durationDayS", 0f, false);
		}

		// Token: 0x0600960D RID: 38413 RVA: 0x003686A8 File Offset: 0x003668A8
		public override string Summary(Scenario scen)
		{
			return this.def.gameCondition.LabelCap + ": " + this.def.gameCondition.description + " (" + ((int)(this.durationDays * 60000f)).ToStringTicksToDays("F1") + ")";
		}

		// Token: 0x0600960E RID: 38414 RVA: 0x00368719 File Offset: 0x00366919
		public override void Randomize()
		{
			this.durationDays = Mathf.Round(this.def.durationRandomRange.RandomInRange);
		}

		// Token: 0x0600960F RID: 38415 RVA: 0x00368736 File Offset: 0x00366936
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Widgets.TextFieldNumericLabeled<float>(listing.GetScenPartRect(this, ScenPart.RowHeight), "durationDays".Translate(), ref this.durationDays, ref this.durationDaysBuf, 0f, 1E+09f);
		}

		// Token: 0x06009610 RID: 38416 RVA: 0x0036876E File Offset: 0x0036696E
		public override void GenerateIntoMap(Map map)
		{
			if (!this.def.gameConditionTargetsWorld)
			{
				map.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x06009611 RID: 38417 RVA: 0x0036878E File Offset: 0x0036698E
		public override void PostWorldGenerate()
		{
			if (this.def.gameConditionTargetsWorld)
			{
				Find.World.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x06009612 RID: 38418 RVA: 0x003687B2 File Offset: 0x003669B2
		private GameCondition MakeCondition()
		{
			return GameConditionMaker.MakeCondition(this.def.gameCondition, (int)(this.durationDays * 60000f));
		}

		// Token: 0x06009613 RID: 38419 RVA: 0x003687D4 File Offset: 0x003669D4
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_GameCondition scenPart_GameCondition = other as ScenPart_GameCondition;
			return scenPart_GameCondition == null || scenPart_GameCondition.def.gameCondition.CanCoexistWith(this.def.gameCondition);
		}

		// Token: 0x06009614 RID: 38420 RVA: 0x0036880B File Offset: 0x00366A0B
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.def.gameCondition == null;
		}

		// Token: 0x06009615 RID: 38421 RVA: 0x00368825 File Offset: 0x00366A25
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.durationDays.GetHashCode();
		}

		// Token: 0x04005A36 RID: 23094
		private float durationDays;

		// Token: 0x04005A37 RID: 23095
		private string durationDaysBuf;
	}
}
