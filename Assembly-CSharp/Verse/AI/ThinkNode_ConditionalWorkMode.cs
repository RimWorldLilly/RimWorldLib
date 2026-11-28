using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008CB RID: 2251
	public class ThinkNode_ConditionalWorkMode : ThinkNode_Conditional
	{
		// Token: 0x060045BB RID: 17851 RVA: 0x001B74CF File Offset: 0x001B56CF
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalWorkMode thinkNode_ConditionalWorkMode = (ThinkNode_ConditionalWorkMode)base.DeepCopy(resolve);
			thinkNode_ConditionalWorkMode.workMode = this.workMode;
			return thinkNode_ConditionalWorkMode;
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x001B74EC File Offset: 0x001B56EC
		protected override bool Satisfied(Pawn pawn)
		{
			if (!pawn.RaceProps.IsMechanoid || pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			Pawn overseer = pawn.GetOverseer();
			return overseer != null && overseer.mechanitor.GetControlGroup(pawn).WorkMode == this.workMode;
		}

		// Token: 0x04002D56 RID: 11606
		public MechWorkModeDef workMode;
	}
}
