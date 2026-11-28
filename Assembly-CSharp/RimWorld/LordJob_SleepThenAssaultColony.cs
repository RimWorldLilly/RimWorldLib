using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C86 RID: 3206
	public class LordJob_SleepThenAssaultColony : LordJob
	{
		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x06005AD5 RID: 23253 RVA: 0x00216770 File Offset: 0x00214970
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005AD6 RID: 23254 RVA: 0x00216773 File Offset: 0x00214973
		public LordJob_SleepThenAssaultColony()
		{
		}

		// Token: 0x06005AD7 RID: 23255 RVA: 0x0021677B File Offset: 0x0021497B
		public LordJob_SleepThenAssaultColony(Faction faction, bool sendWokenUpMessage = true)
		{
			this.faction = faction;
			this.sendWokenUpMessage = sendWokenUpMessage;
		}

		// Token: 0x06005AD8 RID: 23256 RVA: 0x00216794 File Offset: 0x00214994
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Sleep lordToil_Sleep = new LordToil_Sleep();
			stateGraph.StartingToil = lordToil_Sleep;
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true, false, false).CreateGraph()).StartingToil;
			Transition transition = new Transition(lordToil_Sleep, startingToil, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.DormancyWakeup || (this.awakeOnClamor && signal.type == TriggerSignalType.Clamor)));
			if (this.sendWokenUpMessage)
			{
				transition.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f, new Func<bool>(this.AnyAsleep)));
			}
			transition.AddTrigger(new Trigger_PawnHarmed(1f, false, Faction.OfPlayer, null, null));
			transition.AddPostAction(new TransitionAction_WakeAll());
			transition.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					vector += this.lord.ownedPawns[i].Position.ToVector3();
				}
				vector /= (float)this.lord.ownedPawns.Count;
				if (this.faction == Faction.OfMechanoids)
				{
					SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(vector.ToIntVec3(), base.Map, false));
					return;
				}
				if (ModsConfig.IdeologyActive && this.faction == Faction.OfInsects)
				{
					SoundDefOf.InsectsWakeUp.PlayOneShot(new TargetInfo(vector.ToIntVec3(), base.Map, false));
				}
			}));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06005AD9 RID: 23257 RVA: 0x00216898 File Offset: 0x00214A98
		private bool AnyAsleep()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.lord.ownedPawns[i].Spawned && !this.lord.ownedPawns[i].Dead && !this.lord.ownedPawns[i].Awake())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005ADA RID: 23258 RVA: 0x0021690B File Offset: 0x00214B0B
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			if (!p.Dead)
			{
				CompCanBeDormant compCanBeDormant = p.TryGetComp<CompCanBeDormant>();
				if (compCanBeDormant != null && !compCanBeDormant.Awake)
				{
					return false;
				}
			}
			return base.ShouldRemovePawn(p, reason);
		}

		// Token: 0x06005ADB RID: 23259 RVA: 0x00216938 File Offset: 0x00214B38
		public void Notify_PawnUnfogged()
		{
			if (!this.wakeOnPawnUnfogged || this.lord == null)
			{
				return;
			}
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!pawn.Dead && pawn.Spawned && pawn.mindState != null)
				{
					pawn.mindState.Active = true;
				}
			}
			this.lord.Notify_DormancyWakeup();
		}

		// Token: 0x06005ADC RID: 23260 RVA: 0x002169B4 File Offset: 0x00214BB4
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.sendWokenUpMessage, "sendWokenUpMessage", true, false);
			Scribe_Values.Look<bool>(ref this.awakeOnClamor, "awakeOnClamor", false, false);
			Scribe_Values.Look<bool>(ref this.wakeOnPawnUnfogged, "wakeOnPawnUnfogged", false, false);
		}

		// Token: 0x04003417 RID: 13335
		private Faction faction;

		// Token: 0x04003418 RID: 13336
		private bool sendWokenUpMessage;

		// Token: 0x04003419 RID: 13337
		public bool awakeOnClamor;

		// Token: 0x0400341A RID: 13338
		public bool wakeOnPawnUnfogged;
	}
}
