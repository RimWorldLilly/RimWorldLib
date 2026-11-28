using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200179A RID: 6042
	public class ScenPart_ConfigPage_ConfigureStartingPawns : ScenPart_ConfigPage_ConfigureStartingPawnsBase
	{
		// Token: 0x170019A1 RID: 6561
		// (get) Token: 0x060096A1 RID: 38561 RVA: 0x0036A2AD File Offset: 0x003684AD
		protected override int TotalPawnCount
		{
			get
			{
				return this.pawnCount;
			}
		}

		// Token: 0x060096A2 RID: 38562 RVA: 0x0036A2B8 File Offset: 0x003684B8
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			base.DoEditInterface(listing);
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			scenPartRect.height = ScenPart.RowHeight;
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect = new Rect(scenPartRect.x - 200f, scenPartRect.y + ScenPart.RowHeight, 200f, ScenPart.RowHeight);
			rect.xMax -= 4f;
			Widgets.Label(rect, "ScenPart_StartWithPawns_OutOf".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldNumeric<int>(scenPartRect, ref this.pawnCount, ref this.pawnCountBuffer, 1f, 10f);
			scenPartRect.y += ScenPart.RowHeight;
			Widgets.TextFieldNumeric<int>(scenPartRect, ref this.pawnChoiceCount, ref this.pawnCountChoiceBuffer, (float)this.pawnCount, 10f);
		}

		// Token: 0x060096A3 RID: 38563 RVA: 0x0036A392 File Offset: 0x00368592
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.pawnCount, "pawnCount", 0, false);
		}

		// Token: 0x060096A4 RID: 38564 RVA: 0x0036A3AC File Offset: 0x003685AC
		public override string Summary(Scenario scen)
		{
			if (this.pawnCount == 1)
			{
				return "ScenPart_StartWithPawn".Translate();
			}
			return "ScenPart_StartWithPawns".Translate(this.pawnCount);
		}

		// Token: 0x060096A5 RID: 38565 RVA: 0x0036A3E1 File Offset: 0x003685E1
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.pawnCount;
		}

		// Token: 0x060096A6 RID: 38566 RVA: 0x0036A3F0 File Offset: 0x003685F0
		public override void Randomize()
		{
			this.pawnCount = Rand.RangeInclusive(1, 6);
			this.pawnChoiceCount = 10;
		}

		// Token: 0x060096A7 RID: 38567 RVA: 0x0036A408 File Offset: 0x00368608
		protected override void GenerateStartingPawns()
		{
			int num = 0;
			do
			{
				StartingPawnUtility.ClearAllStartingPawns();
				for (int i = 0; i < this.pawnCount; i++)
				{
					StartingPawnUtility.AddNewPawn(-1);
				}
				num++;
			}
			while (num <= 20 && !StartingPawnUtility.WorkTypeRequirementsSatisfied());
		}

		// Token: 0x060096A8 RID: 38568 RVA: 0x0036A443 File Offset: 0x00368643
		public override void PostIdeoChosen()
		{
			Find.GameInitData.allowedDevelopmentalStages = this.allowedDevelopmentalStages;
			Find.GameInitData.startingSkillsRequired = this.requiredSkills;
			base.PostIdeoChosen();
		}

		// Token: 0x04005A52 RID: 23122
		public int pawnCount = 3;

		// Token: 0x04005A53 RID: 23123
		public DevelopmentalStage allowedDevelopmentalStages = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04005A54 RID: 23124
		public List<SkillDef> requiredSkills;

		// Token: 0x04005A55 RID: 23125
		private string pawnCountBuffer;

		// Token: 0x04005A56 RID: 23126
		private string pawnCountChoiceBuffer;

		// Token: 0x04005A57 RID: 23127
		private const int MaxPawnChoiceCount = 10;
	}
}
