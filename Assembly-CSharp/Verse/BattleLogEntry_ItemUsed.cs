using System;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001E4 RID: 484
	public class BattleLogEntry_ItemUsed : BattleLogEntry_Event
	{
		// Token: 0x06000D7E RID: 3454 RVA: 0x000455FE File Offset: 0x000437FE
		public BattleLogEntry_ItemUsed()
		{
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x00045606 File Offset: 0x00043806
		public BattleLogEntry_ItemUsed(Pawn caster, Thing target, ThingDef itemUsed, RulePackDef eventDef) : base(target, eventDef, caster)
		{
			this.itemUsed = itemUsed;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00045619 File Offset: 0x00043819
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.itemUsed, "itemUsed");
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00045634 File Offset: 0x00043834
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForDef("ITEM", this.itemUsed));
			return result;
		}

		// Token: 0x04000D97 RID: 3479
		public ThingDef itemUsed;
	}
}
