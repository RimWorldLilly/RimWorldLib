using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D01 RID: 3329
	public class LordToil_Wait : LordToil
	{
		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x06005DCB RID: 24011 RVA: 0x00225A36 File Offset: 0x00223C36
		protected virtual DutyDef IdleDutyDef
		{
			get
			{
				if (!this.allowRandomInteractions)
				{
					return DutyDefOf.IdleNoInteraction;
				}
				return DutyDefOf.Idle;
			}
		}

		// Token: 0x06005DCC RID: 24012 RVA: 0x00225A4B File Offset: 0x00223C4B
		public LordToil_Wait(bool allowRandomInteractions = true)
		{
			this.allowRandomInteractions = allowRandomInteractions;
		}

		// Token: 0x06005DCD RID: 24013 RVA: 0x00225A64 File Offset: 0x00223C64
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(this.IdleDutyDef);
				this.DecoratePawnDuty(duty);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x06005DCE RID: 24014 RVA: 0x00225ABB File Offset: 0x00223CBB
		protected virtual void DecoratePawnDuty(PawnDuty duty)
		{
		}

		// Token: 0x0400353B RID: 13627
		public bool allowRandomInteractions = true;
	}
}
