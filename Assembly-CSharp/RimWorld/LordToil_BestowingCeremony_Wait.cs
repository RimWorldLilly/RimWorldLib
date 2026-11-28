using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C5D RID: 3165
	public class LordToil_BestowingCeremony_Wait : LordToil_Wait
	{
		// Token: 0x060059C7 RID: 22983 RVA: 0x0021075E File Offset: 0x0020E95E
		public LordToil_BestowingCeremony_Wait(Pawn target, Pawn bestower) : base(true)
		{
			this.target = target;
			this.bestower = bestower;
		}

		// Token: 0x060059C8 RID: 22984 RVA: 0x00210778 File Offset: 0x0020E978
		public override void Init()
		{
			Messages.Message("MessageBestowerWaiting".Translate(this.target.Named("TARGET"), this.lord.ownedPawns[0].Named("BESTOWER")), new LookTargets(new Pawn[]
			{
				this.target,
				this.lord.ownedPawns[0]
			}), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x060059C9 RID: 22985 RVA: 0x002107F2 File Offset: 0x0020E9F2
		protected override void DecoratePawnDuty(PawnDuty duty)
		{
			duty.focus = this.target;
		}

		// Token: 0x060059CA RID: 22986 RVA: 0x00210805 File Offset: 0x0020EA05
		public override void DrawPawnGUIOverlay(Pawn pawn)
		{
			pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
		}

		// Token: 0x060059CB RID: 22987 RVA: 0x0021081A File Offset: 0x0020EA1A
		public override IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			LordToil_BestowingCeremony_Wait.<GetPawnGizmos>d__6 <GetPawnGizmos>d__ = new LordToil_BestowingCeremony_Wait.<GetPawnGizmos>d__6(-2);
			<GetPawnGizmos>d__.<>4__this = this;
			<GetPawnGizmos>d__.<>3__p = p;
			return <GetPawnGizmos>d__;
		}

		// Token: 0x060059CC RID: 22988 RVA: 0x00210831 File Offset: 0x0020EA31
		public override IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn target, Pawn forPawn)
		{
			LordToil_BestowingCeremony_Wait.<ExtraFloatMenuOptions>d__7 <ExtraFloatMenuOptions>d__ = new LordToil_BestowingCeremony_Wait.<ExtraFloatMenuOptions>d__7(-2);
			<ExtraFloatMenuOptions>d__.<>4__this = this;
			<ExtraFloatMenuOptions>d__.<>3__target = target;
			return <ExtraFloatMenuOptions>d__;
		}

		// Token: 0x060059CD RID: 22989 RVA: 0x00210848 File Offset: 0x0020EA48
		private void StartRitual(List<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				Lord lord = pawn.GetLord();
				if (((lord != null) ? lord.LordJob : null) is LordJob_VoluntarilyJoinable)
				{
					pawn.GetLord().Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
				}
			}
			this.lord.AddPawns(pawns, true);
			((LordJob_BestowingCeremony)this.lord.LordJob).colonistParticipants.AddRange(pawns);
			this.lord.ReceiveMemo(LordJob_BestowingCeremony.MemoCeremonyStarted);
			foreach (Pawn pawn2 in pawns)
			{
				if (pawn2.drafter != null)
				{
					pawn2.drafter.Drafted = false;
				}
				if (!pawn2.Awake())
				{
					RestUtility.WakeUp(pawn2, true);
				}
			}
		}

		// Token: 0x04003376 RID: 13174
		public Pawn target;

		// Token: 0x04003377 RID: 13175
		public Pawn bestower;
	}
}
