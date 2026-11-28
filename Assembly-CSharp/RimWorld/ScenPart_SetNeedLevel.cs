using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001793 RID: 6035
	public class ScenPart_SetNeedLevel : ScenPart_PawnModifier
	{
		// Token: 0x0600965A RID: 38490 RVA: 0x00369A20 File Offset: 0x00367C20
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f + 31f);
			if (Widgets.ButtonText(scenPartRect.TopPartPixels(ScenPart.RowHeight), this.need.LabelCap, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<NeedDef>(this.PossibleNeeds(), (NeedDef hd) => hd.LabelCap, (NeedDef n) => delegate()
				{
					this.need = n;
				});
			}
			Widgets.FloatRange(new Rect(scenPartRect.x, scenPartRect.y + ScenPart.RowHeight, scenPartRect.width, 31f), listing.CurHeight.GetHashCode(), ref this.levelRange, 0f, 1f, "ConfigurableLevel", ToStringStyle.FloatTwo, 0f, GameFont.Small, null, 0f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}

		// Token: 0x0600965B RID: 38491 RVA: 0x00369B23 File Offset: 0x00367D23
		private IEnumerable<NeedDef> PossibleNeeds()
		{
			return from x in DefDatabase<NeedDef>.AllDefsListForReading
			where x.major
			select x;
		}

		// Token: 0x0600965C RID: 38492 RVA: 0x00369B50 File Offset: 0x00367D50
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<NeedDef>(ref this.need, "need");
			Scribe_Values.Look<FloatRange>(ref this.levelRange, "levelRange", default(FloatRange), false);
		}

		// Token: 0x0600965D RID: 38493 RVA: 0x00369B90 File Offset: 0x00367D90
		public override string Summary(Scenario scen)
		{
			return "ScenPart_SetNeed".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent(), this.need.label, this.levelRange.min.ToStringPercent(), this.levelRange.max.ToStringPercent()).CapitalizeFirst();
		}

		// Token: 0x0600965E RID: 38494 RVA: 0x00369C10 File Offset: 0x00367E10
		public override void Randomize()
		{
			base.Randomize();
			this.need = this.PossibleNeeds().RandomElement<NeedDef>();
			this.levelRange.max = Rand.Range(0f, 1f);
			this.levelRange.min = this.levelRange.max * Rand.Range(0f, 0.95f);
		}

		// Token: 0x0600965F RID: 38495 RVA: 0x00369C74 File Offset: 0x00367E74
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_SetNeedLevel scenPart_SetNeedLevel = other as ScenPart_SetNeedLevel;
			if (scenPart_SetNeedLevel != null && this.need == scenPart_SetNeedLevel.need)
			{
				this.chance = GenMath.ChanceEitherHappens(this.chance, scenPart_SetNeedLevel.chance);
				return true;
			}
			return false;
		}

		// Token: 0x06009660 RID: 38496 RVA: 0x00369CB4 File Offset: 0x00367EB4
		protected override void ModifyPawnPostGenerate(Pawn p, bool redressed)
		{
			Need need;
			if (p.needs != null && p.needs.TryGetNeed(this.need, out need))
			{
				need.CurLevelPercentage = this.levelRange.RandomInRange;
			}
		}

		// Token: 0x06009661 RID: 38497 RVA: 0x00369CEF File Offset: 0x00367EEF
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.need == null;
		}

		// Token: 0x06009662 RID: 38498 RVA: 0x00369D04 File Offset: 0x00367F04
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.need != null) ? this.need.GetHashCode() : 0) ^ this.levelRange.GetHashCode();
		}

		// Token: 0x04005A49 RID: 23113
		private NeedDef need;

		// Token: 0x04005A4A RID: 23114
		private FloatRange levelRange;
	}
}
