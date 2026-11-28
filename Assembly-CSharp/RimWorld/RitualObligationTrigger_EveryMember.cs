using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016B0 RID: 5808
	public abstract class RitualObligationTrigger_EveryMember : RitualObligationTrigger
	{
		// Token: 0x06009175 RID: 37237
		protected abstract void Recache();

		// Token: 0x06009176 RID: 37238 RVA: 0x003539E0 File Offset: 0x00351BE0
		public override void Notify_MemberDied(Pawn p)
		{
			this.Recache();
		}

		// Token: 0x06009177 RID: 37239 RVA: 0x003539E8 File Offset: 0x00351BE8
		public override void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
			this.Recache();
		}

		// Token: 0x06009178 RID: 37240 RVA: 0x003539F0 File Offset: 0x00351BF0
		public override void Notify_MemberGenerated(Pawn pawn)
		{
			this.Recache();
		}

		// Token: 0x06009179 RID: 37241 RVA: 0x003539F8 File Offset: 0x00351BF8
		public override void Notify_GameStarted()
		{
			this.Recache();
		}

		// Token: 0x0600917A RID: 37242 RVA: 0x00353A00 File Offset: 0x00351C00
		public override void Notify_RitualExecuted(LordJob_Ritual ritual)
		{
			this.Recache();
		}

		// Token: 0x0600917B RID: 37243 RVA: 0x00353A08 File Offset: 0x00351C08
		public override void Notify_MemberGained(Pawn pawn)
		{
			this.Recache();
		}

		// Token: 0x0600917C RID: 37244 RVA: 0x00353A10 File Offset: 0x00351C10
		public override void Notify_MemberLost(Pawn pawn)
		{
			this.Recache();
		}

		// Token: 0x0600917D RID: 37245 RVA: 0x00353A18 File Offset: 0x00351C18
		public override void Notify_MemberGuestStatusChanged(Pawn pawn)
		{
			this.Recache();
		}

		// Token: 0x0600917E RID: 37246 RVA: 0x00353A20 File Offset: 0x00351C20
		public override void Notify_IdeoReformed()
		{
			this.Recache();
		}
	}
}
