using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016AB RID: 5803
	public abstract class RitualObligationTrigger : IExposable
	{
		// Token: 0x17001906 RID: 6406
		// (get) Token: 0x0600915F RID: 37215 RVA: 0x00353781 File Offset: 0x00351981
		public virtual string TriggerExtraDesc
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06009160 RID: 37216 RVA: 0x00353784 File Offset: 0x00351984
		public virtual void Init(RitualObligationTriggerProperties props)
		{
			this.mustBePlayerIdeo = props.mustBePlayerIdeo;
		}

		// Token: 0x06009161 RID: 37217 RVA: 0x00353792 File Offset: 0x00351992
		public virtual void Tick()
		{
		}

		// Token: 0x06009162 RID: 37218 RVA: 0x00353794 File Offset: 0x00351994
		public virtual void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
		}

		// Token: 0x06009163 RID: 37219 RVA: 0x00353796 File Offset: 0x00351996
		public virtual void Notify_MemberGenerated(Pawn pawn)
		{
		}

		// Token: 0x06009164 RID: 37220 RVA: 0x00353798 File Offset: 0x00351998
		public virtual void Notify_MemberDied(Pawn p)
		{
		}

		// Token: 0x06009165 RID: 37221 RVA: 0x0035379A File Offset: 0x0035199A
		public virtual void Notify_MemberCorpseDestroyed(Pawn p)
		{
		}

		// Token: 0x06009166 RID: 37222 RVA: 0x0035379C File Offset: 0x0035199C
		public virtual void Notify_MemberLost(Pawn p)
		{
		}

		// Token: 0x06009167 RID: 37223 RVA: 0x0035379E File Offset: 0x0035199E
		public virtual void Notify_MemberGained(Pawn p)
		{
		}

		// Token: 0x06009168 RID: 37224 RVA: 0x003537A0 File Offset: 0x003519A0
		public virtual void Notify_GameStarted()
		{
		}

		// Token: 0x06009169 RID: 37225 RVA: 0x003537A2 File Offset: 0x003519A2
		public virtual void Notify_IdeoReformed()
		{
		}

		// Token: 0x0600916A RID: 37226 RVA: 0x003537A4 File Offset: 0x003519A4
		public virtual void Notify_RitualExecuted(LordJob_Ritual ritual)
		{
		}

		// Token: 0x0600916B RID: 37227 RVA: 0x003537A6 File Offset: 0x003519A6
		public virtual void Notify_MemberGuestStatusChanged(Pawn pawn)
		{
		}

		// Token: 0x0600916C RID: 37228 RVA: 0x003537A8 File Offset: 0x003519A8
		public virtual void CopyTo(RitualObligationTrigger other)
		{
			other.ritual = this.ritual;
			other.mustBePlayerIdeo = this.mustBePlayerIdeo;
		}

		// Token: 0x0600916D RID: 37229 RVA: 0x003537C2 File Offset: 0x003519C2
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.mustBePlayerIdeo, "mustBePlayerIdeo", false, false);
		}

		// Token: 0x0400585F RID: 22623
		public Precept_Ritual ritual;

		// Token: 0x04005860 RID: 22624
		protected bool mustBePlayerIdeo;
	}
}
