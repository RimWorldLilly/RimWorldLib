using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AB9 RID: 6841
	public class Building_HackableDoor : Building_SupportedDoor, IHackable
	{
		// Token: 0x17001F23 RID: 7971
		// (get) Token: 0x0600ADC7 RID: 44487 RVA: 0x003D41B4 File Offset: 0x003D23B4
		public CompHackable Hackable
		{
			get
			{
				CompHackable result;
				if ((result = this.hackableInt) == null)
				{
					result = (this.hackableInt = base.GetComp<CompHackable>());
				}
				return result;
			}
		}

		// Token: 0x17001F24 RID: 7972
		// (get) Token: 0x0600ADC8 RID: 44488 RVA: 0x003D41DA File Offset: 0x003D23DA
		public bool Locked
		{
			get
			{
				return !this.Hackable.IsHacked;
			}
		}

		// Token: 0x17001F25 RID: 7973
		// (get) Token: 0x0600ADC9 RID: 44489 RVA: 0x003D41EA File Offset: 0x003D23EA
		protected override bool CheckFaction
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600ADCA RID: 44490 RVA: 0x003D41ED File Offset: 0x003D23ED
		public override bool PawnCanOpen(Pawn p)
		{
			return !this.Locked && base.PawnCanOpen(p);
		}

		// Token: 0x0600ADCB RID: 44491 RVA: 0x003D4200 File Offset: 0x003D2400
		public void OnLockedOut(Pawn pawn = null)
		{
		}

		// Token: 0x0600ADCC RID: 44492 RVA: 0x003D4202 File Offset: 0x003D2402
		public void OnHacked(Pawn pawn = null)
		{
			if (((pawn != null) ? pawn.Faction : null) != null)
			{
				this.SetFaction(pawn.Faction, null);
			}
		}

		// Token: 0x040064B1 RID: 25777
		private CompHackable hackableInt;
	}
}
