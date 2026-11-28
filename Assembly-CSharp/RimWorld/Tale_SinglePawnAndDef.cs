using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017DF RID: 6111
	public class Tale_SinglePawnAndDef : Tale_SinglePawn
	{
		// Token: 0x0600988D RID: 39053 RVA: 0x00374706 File Offset: 0x00372906
		public Tale_SinglePawnAndDef()
		{
		}

		// Token: 0x0600988E RID: 39054 RVA: 0x0037470E File Offset: 0x0037290E
		public Tale_SinglePawnAndDef(Pawn pawn, Def def) : base(pawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x0600988F RID: 39055 RVA: 0x00374723 File Offset: 0x00372923
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", Array.Empty<object>());
		}

		// Token: 0x06009890 RID: 39056 RVA: 0x00374740 File Offset: 0x00372940
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			Tale_SinglePawnAndDef.<SpecialTextGenerationRules>d__4 <SpecialTextGenerationRules>d__ = new Tale_SinglePawnAndDef.<SpecialTextGenerationRules>d__4(-2);
			<SpecialTextGenerationRules>d__.<>4__this = this;
			<SpecialTextGenerationRules>d__.<>3__outConstants = outConstants;
			return <SpecialTextGenerationRules>d__;
		}

		// Token: 0x06009891 RID: 39057 RVA: 0x00374757 File Offset: 0x00372957
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}

		// Token: 0x04005B2E RID: 23342
		public TaleData_Def defData;
	}
}
