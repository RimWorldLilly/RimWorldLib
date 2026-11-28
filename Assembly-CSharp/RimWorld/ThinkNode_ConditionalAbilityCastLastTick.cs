using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D7C RID: 3452
	public class ThinkNode_ConditionalAbilityCastLastTick : ThinkNode_Conditional
	{
		// Token: 0x06005F7E RID: 24446 RVA: 0x0022C644 File Offset: 0x0022A844
		protected override bool Satisfied(Pawn pawn)
		{
			Pawn_AbilityTracker abilities = pawn.abilities;
			Ability ability = (abilities != null) ? abilities.GetAbility(this.ability, false) : null;
			if (ability == null)
			{
				return false;
			}
			if (ability.lastCastTick < 0)
			{
				return false;
			}
			int num = Find.TickManager.TicksGame - ability.lastCastTick;
			return num <= this.maxTicksAgo && num >= this.minTicksAgo;
		}

		// Token: 0x06005F7F RID: 24447 RVA: 0x0022C6A4 File Offset: 0x0022A8A4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalAbilityCastLastTick thinkNode_ConditionalAbilityCastLastTick = (ThinkNode_ConditionalAbilityCastLastTick)base.DeepCopy(resolve);
			thinkNode_ConditionalAbilityCastLastTick.ability = this.ability;
			thinkNode_ConditionalAbilityCastLastTick.maxTicksAgo = this.maxTicksAgo;
			thinkNode_ConditionalAbilityCastLastTick.minTicksAgo = this.minTicksAgo;
			return thinkNode_ConditionalAbilityCastLastTick;
		}

		// Token: 0x040035C6 RID: 13766
		public int maxTicksAgo = int.MaxValue;

		// Token: 0x040035C7 RID: 13767
		public int minTicksAgo;

		// Token: 0x040035C8 RID: 13768
		public AbilityDef ability;
	}
}
