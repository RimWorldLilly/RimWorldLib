using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009DE RID: 2526
	public class JobDriver_BoardOrLeaveGravship : JobDriver_StandAndBeSociallyActive
	{
		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06004C96 RID: 19606 RVA: 0x001D76E5 File Offset: 0x001D58E5
		private string FailedKey
		{
			get
			{
				if (!this.job.def.boardingGravship)
				{
					return "FailedToLeaveGravship";
				}
				return "FailedToBoardGravship";
			}
		}

		// Token: 0x06004C97 RID: 19607 RVA: 0x001D7704 File Offset: 0x001D5904
		protected override Toil GetGotoToil()
		{
			Toil toil = Toils_Goto.GotoCell(base.TargetLocA, PathEndMode.OnCell);
			toil.AddFinishAction(delegate
			{
				if (this.pawn.Dead || this.pawn.Destroyed)
				{
					this.CheckFailedToBoardOrLeave(false, null);
					return;
				}
				if (this.pawn.Downed)
				{
					this.CheckFailedToBoardOrLeave(true, "DownedLower".Translate());
					return;
				}
				this.CheckFailedToBoardOrLeave(true, null);
			});
			return toil;
		}

		// Token: 0x06004C98 RID: 19608 RVA: 0x001D7724 File Offset: 0x001D5924
		public override void Notify_PatherFailed()
		{
			this.CheckFailedToBoardOrLeave(true, "NoPath".Translate());
			base.Notify_PatherFailed();
		}

		// Token: 0x06004C99 RID: 19609 RVA: 0x001D7744 File Offset: 0x001D5944
		private void CheckFailedToBoardOrLeave(bool doMessage = true, string reason = null)
		{
			if (this.hasChecked)
			{
				return;
			}
			this.hasChecked = true;
			Building_GravEngine playerGravEngine_NewTemp = GravshipUtility.GetPlayerGravEngine_NewTemp(this.pawn.Map);
			if (playerGravEngine_NewTemp == null || (playerGravEngine_NewTemp.pawnsToBoard == null && playerGravEngine_NewTemp.pawnsToLeave == null))
			{
				return;
			}
			if (playerGravEngine_NewTemp.ValidSubstructure.Contains(this.pawn.Position) == this.job.def.boardingGravship)
			{
				return;
			}
			if (doMessage)
			{
				TaggedString taggedString = this.FailedKey.Translate(this.pawn.Named("PAWN"), playerGravEngine_NewTemp.RenamableLabel);
				if (!reason.NullOrEmpty())
				{
					taggedString += ": " + reason.CapitalizeFirst();
				}
				if (MessagesRepeatAvoider.MessageShowAllowed(taggedString, 10f))
				{
					Messages.Message(taggedString, this.pawn, MessageTypeDefOf.NegativeEvent, false);
				}
			}
			HashSet<Pawn> pawnsToBoard = playerGravEngine_NewTemp.pawnsToBoard;
			if (pawnsToBoard != null)
			{
				pawnsToBoard.Remove(this.pawn);
			}
			HashSet<Pawn> pawnsToLeave = playerGravEngine_NewTemp.pawnsToLeave;
			if (pawnsToLeave == null)
			{
				return;
			}
			pawnsToLeave.Remove(this.pawn);
		}

		// Token: 0x0400301A RID: 12314
		private bool hasChecked;
	}
}
