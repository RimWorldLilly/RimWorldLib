using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017DB RID: 6107
	public class Tale_DoublePawnAndDef : Tale_DoublePawn
	{
		// Token: 0x06009875 RID: 39029 RVA: 0x003744A7 File Offset: 0x003726A7
		public Tale_DoublePawnAndDef()
		{
		}

		// Token: 0x06009876 RID: 39030 RVA: 0x003744AF File Offset: 0x003726AF
		public Tale_DoublePawnAndDef(Pawn firstPawn, Pawn secondPawn, Def def) : base(firstPawn, secondPawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x06009877 RID: 39031 RVA: 0x003744C5 File Offset: 0x003726C5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", Array.Empty<object>());
		}

		// Token: 0x06009878 RID: 39032 RVA: 0x003744E2 File Offset: 0x003726E2
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			Tale_DoublePawnAndDef.<SpecialTextGenerationRules>d__4 <SpecialTextGenerationRules>d__ = new Tale_DoublePawnAndDef.<SpecialTextGenerationRules>d__4(-2);
			<SpecialTextGenerationRules>d__.<>4__this = this;
			<SpecialTextGenerationRules>d__.<>3__outConstants = outConstants;
			return <SpecialTextGenerationRules>d__;
		}

		// Token: 0x06009879 RID: 39033 RVA: 0x003744F9 File Offset: 0x003726F9
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}

		// Token: 0x04005B2B RID: 23339
		public TaleData_Def defData;
	}
}
