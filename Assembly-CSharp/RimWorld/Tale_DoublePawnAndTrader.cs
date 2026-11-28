using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017DC RID: 6108
	public class Tale_DoublePawnAndTrader : Tale_DoublePawn
	{
		// Token: 0x0600987B RID: 39035 RVA: 0x00374539 File Offset: 0x00372739
		public Tale_DoublePawnAndTrader()
		{
		}

		// Token: 0x0600987C RID: 39036 RVA: 0x00374541 File Offset: 0x00372741
		public Tale_DoublePawnAndTrader(Pawn firstPawn, Pawn secondPawn, ITrader trader) : base(firstPawn, secondPawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x0600987D RID: 39037 RVA: 0x00374557 File Offset: 0x00372757
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x0600987E RID: 39038 RVA: 0x00374577 File Offset: 0x00372777
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", Array.Empty<object>());
		}

		// Token: 0x0600987F RID: 39039 RVA: 0x00374594 File Offset: 0x00372794
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			Tale_DoublePawnAndTrader.<SpecialTextGenerationRules>d__5 <SpecialTextGenerationRules>d__ = new Tale_DoublePawnAndTrader.<SpecialTextGenerationRules>d__5(-2);
			<SpecialTextGenerationRules>d__.<>4__this = this;
			<SpecialTextGenerationRules>d__.<>3__outConstants = outConstants;
			return <SpecialTextGenerationRules>d__;
		}

		// Token: 0x06009880 RID: 39040 RVA: 0x003745AB File Offset: 0x003727AB
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x04005B2C RID: 23340
		public TaleData_Trader traderData;
	}
}
