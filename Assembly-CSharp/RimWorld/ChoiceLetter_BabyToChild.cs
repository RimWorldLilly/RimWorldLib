using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D52 RID: 7506
	public class ChoiceLetter_BabyToChild : ChoiceLetter
	{
		// Token: 0x170023DC RID: 9180
		// (get) Token: 0x0600C248 RID: 49736 RVA: 0x0046CE0C File Offset: 0x0046B00C
		public override bool CanShowInLetterStack
		{
			get
			{
				Faction faction = this.pawn.Faction;
				return faction != null && faction.IsPlayer;
			}
		}

		// Token: 0x170023DD RID: 9181
		// (get) Token: 0x0600C249 RID: 49737 RVA: 0x0046CE24 File Offset: 0x0046B024
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				ChoiceLetter_BabyToChild.<get_Choices>d__8 <get_Choices>d__ = new ChoiceLetter_BabyToChild.<get_Choices>d__8(-2);
				<get_Choices>d__.<>4__this = this;
				return <get_Choices>d__;
			}
		}

		// Token: 0x0600C24A RID: 49738 RVA: 0x0046CE34 File Offset: 0x0046B034
		public void Start()
		{
			base.StartTimeout(30000);
			this.pawn = (this.lookTargets.TryGetPrimaryTarget().Thing as Pawn);
			this.bornSlave = this.pawn.IsSlave;
			if (this.bornSlave)
			{
				this.ChoseColonistLabel = "Emancipate".Translate().CapitalizeFirst();
				this.ChoseSlaveLabel = "RemainX".Translate(this.pawn.LegalStatus).CapitalizeFirst();
				return;
			}
			this.ChoseColonistLabel = "RemainX".Translate(this.pawn.LegalStatus).CapitalizeFirst();
			this.ChoseSlaveLabel = "Enslave".Translate().CapitalizeFirst();
		}

		// Token: 0x0600C24B RID: 49739 RVA: 0x0046CF04 File Offset: 0x0046B104
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.bornSlave, "bornSlave", false, false);
			Scribe_Values.Look<TaggedString>(ref this.ChoseColonistLabel, "ChoseColonistLabel", default(TaggedString), false);
			Scribe_Values.Look<TaggedString>(ref this.ChoseSlaveLabel, "ChoseSlaveLabel", default(TaggedString), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawn = (this.lookTargets.TryGetPrimaryTarget().Thing as Pawn);
			}
		}

		// Token: 0x0600C24C RID: 49740 RVA: 0x0046CF83 File Offset: 0x0046B183
		private void ChoseColonist()
		{
			if (this.bornSlave)
			{
				GenGuest.SlaveRelease(this.pawn);
			}
			Find.LetterStack.RemoveLetter(this);
		}

		// Token: 0x0600C24D RID: 49741 RVA: 0x0046CFA3 File Offset: 0x0046B1A3
		private void ChoseSlave()
		{
			if (!this.bornSlave)
			{
				this.pawn.guest.SetGuestStatus(this.pawn.Faction, GuestStatus.Slave);
			}
			Find.LetterStack.RemoveLetter(this);
		}

		// Token: 0x04006F12 RID: 28434
		private const int TimeoutTicks = 30000;

		// Token: 0x04006F13 RID: 28435
		private Pawn pawn;

		// Token: 0x04006F14 RID: 28436
		private bool bornSlave;

		// Token: 0x04006F15 RID: 28437
		private TaggedString ChoseColonistLabel;

		// Token: 0x04006F16 RID: 28438
		private TaggedString ChoseSlaveLabel;
	}
}
