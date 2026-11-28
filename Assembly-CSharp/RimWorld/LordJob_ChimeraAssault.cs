using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C62 RID: 3170
	public class LordJob_ChimeraAssault : LordJob
	{
		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x060059F4 RID: 23028 RVA: 0x00211BEB File Offset: 0x0020FDEB
		private bool InAttackMode
		{
			get
			{
				return this.lord.CurLordToil is LordToil_ChimeraAttack;
			}
		}

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x060059F5 RID: 23029 RVA: 0x00211C00 File Offset: 0x0020FE00
		private bool CanSwitchMode
		{
			get
			{
				return (float)(Find.TickManager.TicksGame - this.currentModeStartedTick) > 7500f;
			}
		}

		// Token: 0x060059F7 RID: 23031 RVA: 0x00211C24 File Offset: 0x0020FE24
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ChimeraStalk lordToil_ChimeraStalk = new LordToil_ChimeraStalk();
			stateGraph.StartingToil = lordToil_ChimeraStalk;
			LordToil_ChimeraAttack lordToil_ChimeraAttack = new LordToil_ChimeraAttack();
			stateGraph.AddToil(lordToil_ChimeraAttack);
			Transition transition = new Transition(lordToil_ChimeraStalk, lordToil_ChimeraAttack, false, true);
			transition.AddPostAction(new TransitionAction_Custom(delegate()
			{
				this.currentModeStartedTick = Find.TickManager.TicksGame;
				this.SendAttackingLetter();
			}));
			transition.triggers.Add(new Trigger_Memo("StalkToAttack"));
			transition.triggers.Add(new LordJob_ChimeraAssault.Trigger_ChimeraHarmed(1f, true, null, DutyDefOf.ChimeraStalkFlee, new int?(2500)));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_ChimeraAttack, lordToil_ChimeraStalk, false, true);
			transition2.AddPostAction(new TransitionAction_Custom(delegate()
			{
				this.currentModeStartedTick = Find.TickManager.TicksGame;
				this.SendModeChangeMessage("MessageChimeraWithdrawing".Translate());
			}));
			transition2.triggers.Add(new Trigger_Memo("AttackToStalk"));
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x00211CF2 File Offset: 0x0020FEF2
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return reason != PawnLostCondition.Incapped;
		}

		// Token: 0x060059F9 RID: 23033 RVA: 0x00211CFC File Offset: 0x0020FEFC
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.CanSwitchMode)
			{
				if (this.InAttackMode && Rand.MTBEventOccurs(8f, 2500f, 1f))
				{
					this.lord.ReceiveMemo("AttackToStalk");
					return;
				}
				if (!this.InAttackMode && Rand.MTBEventOccurs(0.7f, 60000f, 1f))
				{
					this.lord.ReceiveMemo("StalkToAttack");
				}
			}
		}

		// Token: 0x060059FA RID: 23034 RVA: 0x00211D74 File Offset: 0x0020FF74
		public void SwitchMode()
		{
			if (this.lord == null)
			{
				return;
			}
			LordToil curLordToil = this.lord.CurLordToil;
			if (!(curLordToil is LordToil_ChimeraStalk))
			{
				if (!(curLordToil is LordToil_ChimeraAttack))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Chimera lord job tried switching from a toil which is not handled ");
					defaultInterpolatedStringHandler.AppendFormatted<LordToil>(this.lord.CurLordToil);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					this.lord.ReceiveMemo("AttackToStalk");
				}
			}
			else
			{
				this.lord.ReceiveMemo("StalkToAttack");
			}
			this.currentModeStartedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060059FB RID: 23035 RVA: 0x00211E10 File Offset: 0x00210010
		public override void Notify_PawnDowned(Pawn p)
		{
			base.Notify_PawnDowned(p);
			if (this.CanSwitchMode && Rand.Chance(0.075f) && this.lord.CurLordToil is LordToil_ChimeraAttack)
			{
				this.SwitchMode();
			}
		}

		// Token: 0x060059FC RID: 23036 RVA: 0x00211E45 File Offset: 0x00210045
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			if (this.CanSwitchMode && condition == PawnLostCondition.Killed && Rand.Chance(0.075f) && this.lord.CurLordToil is LordToil_ChimeraAttack)
			{
				this.SwitchMode();
			}
		}

		// Token: 0x060059FD RID: 23037 RVA: 0x00211E78 File Offset: 0x00210078
		private void SendModeChangeMessage(string verb)
		{
			if (this.NoActivePawns())
			{
				return;
			}
			Messages.Message((this.lord.ownedPawns.Count > 1) ? "MessageChimeraModeChangePlural".Translate(verb) : "MessageChimeraModeChangeSingular".Translate(verb), new LookTargets(this.lord.ownedPawns), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x060059FE RID: 23038 RVA: 0x00211EE4 File Offset: 0x002100E4
		private void SendAttackingLetter()
		{
			if (this.NoActivePawns())
			{
				return;
			}
			TaggedString label = "LetterChimerasAttackingLabel".Translate();
			TaggedString text = "LetterChimerasAttacking".Translate();
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, new LookTargets(this.lord.ownedPawns), null, null, null, null, 0, true);
		}

		// Token: 0x060059FF RID: 23039 RVA: 0x00211F38 File Offset: 0x00210138
		private bool NoActivePawns()
		{
			if (this.lord.ownedPawns.Count == 0)
			{
				return true;
			}
			bool flag = false;
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				flag = !pawn.Downed;
			}
			return !flag;
		}

		// Token: 0x06005A00 RID: 23040 RVA: 0x00211FB0 File Offset: 0x002101B0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.currentModeStartedTick, "currentModeStartedTick", 0, false);
		}

		// Token: 0x04003396 RID: 13206
		private int currentModeStartedTick;

		// Token: 0x04003397 RID: 13207
		private const float StalkToAttackMTBDays = 0.7f;

		// Token: 0x04003398 RID: 13208
		private const float AttackToStalkMTBHours = 8f;

		// Token: 0x04003399 RID: 13209
		private const float ChanceToFleeOnDown = 0.075f;

		// Token: 0x0400339A RID: 13210
		private const float MinTimeInModeTicks = 7500f;

		// Token: 0x0400339B RID: 13211
		private const int MinTicksFleeing = 2500;

		// Token: 0x0400339C RID: 13212
		public const string StalkToAttackMemo = "StalkToAttack";

		// Token: 0x0400339D RID: 13213
		private const string AttackToStalkMemo = "AttackToStalk";

		// Token: 0x02002E70 RID: 11888
		private class Trigger_ChimeraHarmed : Trigger_PawnHarmed
		{
			// Token: 0x060118FF RID: 71935 RVA: 0x00608D9C File Offset: 0x00606F9C
			public Trigger_ChimeraHarmed(float chance = 1f, bool requireInstigatorWithFaction = false, Faction requireInstigatorWithSpecificFaction = null, DutyDef skipDuty = null, int? minTicks = null) : base(chance, requireInstigatorWithFaction, requireInstigatorWithSpecificFaction, skipDuty, minTicks)
			{
			}

			// Token: 0x06011900 RID: 71936 RVA: 0x00608DAC File Offset: 0x00606FAC
			public override bool ActivateOn(Lord lord, TriggerSignal signal)
			{
				LordJob_ChimeraAssault lordJob_ChimeraAssault = lord.LordJob as LordJob_ChimeraAssault;
				return (lordJob_ChimeraAssault == null || lordJob_ChimeraAssault.CanSwitchMode) && base.ActivateOn(lord, signal);
			}
		}
	}
}
