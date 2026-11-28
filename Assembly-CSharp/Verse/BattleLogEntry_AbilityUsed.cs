using System;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001E0 RID: 480
	public class BattleLogEntry_AbilityUsed : BattleLogEntry_Event
	{
		// Token: 0x06000D57 RID: 3415 RVA: 0x00044D72 File Offset: 0x00042F72
		public BattleLogEntry_AbilityUsed()
		{
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00044D7A File Offset: 0x00042F7A
		public BattleLogEntry_AbilityUsed(Pawn caster, Thing target, AbilityDef ability, RulePackDef eventDef) : base(target, eventDef, caster)
		{
			this.abilityUsed = ability;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00044D8D File Offset: 0x00042F8D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<AbilityDef>(ref this.abilityUsed, "abilityUsed");
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00044DA8 File Offset: 0x00042FA8
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForDef("ABILITY", this.abilityUsed));
			if (this.subjectPawn == null && this.subjectThing == null)
			{
				result.Rules.Add(new Rule_String("SUBJECT_definite", "AreaLower".Translate()));
			}
			return result;
		}

		// Token: 0x04000D87 RID: 3463
		public AbilityDef abilityUsed;
	}
}
