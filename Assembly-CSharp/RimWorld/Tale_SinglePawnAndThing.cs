using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017E0 RID: 6112
	public class Tale_SinglePawnAndThing : Tale_SinglePawn
	{
		// Token: 0x06009893 RID: 39059 RVA: 0x00374797 File Offset: 0x00372997
		public Tale_SinglePawnAndThing()
		{
		}

		// Token: 0x06009894 RID: 39060 RVA: 0x0037479F File Offset: 0x0037299F
		public Tale_SinglePawnAndThing(Pawn pawn, Thing item) : base(pawn)
		{
			this.thingData = TaleData_Thing.GenerateFrom(item);
		}

		// Token: 0x06009895 RID: 39061 RVA: 0x003747B4 File Offset: 0x003729B4
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || th.thingIDNumber == this.thingData.thingID;
		}

		// Token: 0x06009896 RID: 39062 RVA: 0x003747D4 File Offset: 0x003729D4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Thing>(ref this.thingData, "thingData", Array.Empty<object>());
		}

		// Token: 0x06009897 RID: 39063 RVA: 0x003747F1 File Offset: 0x003729F1
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			Tale_SinglePawnAndThing.<SpecialTextGenerationRules>d__5 <SpecialTextGenerationRules>d__ = new Tale_SinglePawnAndThing.<SpecialTextGenerationRules>d__5(-2);
			<SpecialTextGenerationRules>d__.<>4__this = this;
			<SpecialTextGenerationRules>d__.<>3__outConstants = outConstants;
			return <SpecialTextGenerationRules>d__;
		}

		// Token: 0x06009898 RID: 39064 RVA: 0x00374808 File Offset: 0x00372A08
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.thingData = TaleData_Thing.GenerateRandom();
		}

		// Token: 0x04005B2F RID: 23343
		public TaleData_Thing thingData;
	}
}
