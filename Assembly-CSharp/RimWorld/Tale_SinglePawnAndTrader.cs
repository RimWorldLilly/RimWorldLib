using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017E1 RID: 6113
	public class Tale_SinglePawnAndTrader : Tale_SinglePawn
	{
		// Token: 0x0600989A RID: 39066 RVA: 0x00374824 File Offset: 0x00372A24
		public Tale_SinglePawnAndTrader()
		{
		}

		// Token: 0x0600989B RID: 39067 RVA: 0x0037482C File Offset: 0x00372A2C
		public Tale_SinglePawnAndTrader(Pawn pawn, ITrader trader) : base(pawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x0600989C RID: 39068 RVA: 0x00374841 File Offset: 0x00372A41
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x0600989D RID: 39069 RVA: 0x00374861 File Offset: 0x00372A61
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", Array.Empty<object>());
		}

		// Token: 0x0600989E RID: 39070 RVA: 0x0037487E File Offset: 0x00372A7E
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			Tale_SinglePawnAndTrader.<SpecialTextGenerationRules>d__5 <SpecialTextGenerationRules>d__ = new Tale_SinglePawnAndTrader.<SpecialTextGenerationRules>d__5(-2);
			<SpecialTextGenerationRules>d__.<>4__this = this;
			<SpecialTextGenerationRules>d__.<>3__outConstants = outConstants;
			return <SpecialTextGenerationRules>d__;
		}

		// Token: 0x0600989F RID: 39071 RVA: 0x00374895 File Offset: 0x00372A95
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x04005B30 RID: 23344
		public TaleData_Trader traderData;
	}
}
