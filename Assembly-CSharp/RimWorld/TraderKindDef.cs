using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001004 RID: 4100
	public class TraderKindDef : Def
	{
		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x06006767 RID: 26471 RVA: 0x002495DC File Offset: 0x002477DC
		public float CalculatedCommonality
		{
			get
			{
				float num = this.commonality;
				if (this.commonalityMultFromPopulationIntent != null)
				{
					num *= this.commonalityMultFromPopulationIntent.Evaluate(StorytellerUtilityPopulation.PopulationIntent);
				}
				return num;
			}
		}

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x06006768 RID: 26472 RVA: 0x0024960C File Offset: 0x0024780C
		public RoyalTitleDef TitleRequiredToTrade
		{
			get
			{
				if (this.permitRequiredForTrading != null)
				{
					RoyalTitleDef royalTitleDef = this.faction.RoyalTitlesAwardableInSeniorityOrderForReading.FirstOrDefault((RoyalTitleDef x) => x.permits != null && x.permits.Contains(this.permitRequiredForTrading));
					if (royalTitleDef != null)
					{
						return royalTitleDef;
					}
				}
				return null;
			}
		}

		// Token: 0x06006769 RID: 26473 RVA: 0x00249644 File Offset: 0x00247844
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				stockGenerator.ResolveReferences(this);
			}
		}

		// Token: 0x0600676A RID: 26474 RVA: 0x0024969C File Offset: 0x0024789C
		public override IEnumerable<string> ConfigErrors()
		{
			TraderKindDef.<ConfigErrors>d__15 <ConfigErrors>d__ = new TraderKindDef.<ConfigErrors>d__15(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600676B RID: 26475 RVA: 0x002496AC File Offset: 0x002478AC
		public bool WillTrade(ThingDef td)
		{
			for (int i = 0; i < this.stockGenerators.Count; i++)
			{
				if (this.stockGenerators[i].HandlesThingDef(td))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600676C RID: 26476 RVA: 0x002496E8 File Offset: 0x002478E8
		public PriceType PriceTypeFor(ThingDef thingDef, TradeAction action)
		{
			if (thingDef == ThingDefOf.Silver)
			{
				return PriceType.Undefined;
			}
			if (action == TradeAction.PlayerBuys)
			{
				for (int i = 0; i < this.stockGenerators.Count; i++)
				{
					PriceType result;
					if (this.stockGenerators[i].TryGetPriceType(thingDef, action, out result))
					{
						return result;
					}
				}
			}
			return PriceType.Normal;
		}

		// Token: 0x0400415B RID: 16731
		public List<StockGenerator> stockGenerators = new List<StockGenerator>();

		// Token: 0x0400415C RID: 16732
		public bool orbital;

		// Token: 0x0400415D RID: 16733
		public bool requestable = true;

		// Token: 0x0400415E RID: 16734
		public bool hideThingsNotWillingToTrade;

		// Token: 0x0400415F RID: 16735
		public float commonality = 1f;

		// Token: 0x04004160 RID: 16736
		public string category;

		// Token: 0x04004161 RID: 16737
		public TradeCurrency tradeCurrency;

		// Token: 0x04004162 RID: 16738
		public SimpleCurve commonalityMultFromPopulationIntent;

		// Token: 0x04004163 RID: 16739
		public FactionDef faction;

		// Token: 0x04004164 RID: 16740
		public RoyalTitlePermitDef permitRequiredForTrading;
	}
}
