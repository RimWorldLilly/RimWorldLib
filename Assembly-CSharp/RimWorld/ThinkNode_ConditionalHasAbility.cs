using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D7B RID: 3451
	public class ThinkNode_ConditionalHasAbility : ThinkNode_Conditional
	{
		// Token: 0x06005F7B RID: 24443 RVA: 0x0022C603 File Offset: 0x0022A803
		protected override bool Satisfied(Pawn pawn)
		{
			Pawn_AbilityTracker abilities = pawn.abilities;
			return ((abilities != null) ? abilities.GetAbility(this.ability, false) : null) != null;
		}

		// Token: 0x06005F7C RID: 24444 RVA: 0x0022C621 File Offset: 0x0022A821
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalHasAbility thinkNode_ConditionalHasAbility = (ThinkNode_ConditionalHasAbility)base.DeepCopy(resolve);
			thinkNode_ConditionalHasAbility.ability = this.ability;
			return thinkNode_ConditionalHasAbility;
		}

		// Token: 0x040035C5 RID: 13765
		public AbilityDef ability;
	}
}
