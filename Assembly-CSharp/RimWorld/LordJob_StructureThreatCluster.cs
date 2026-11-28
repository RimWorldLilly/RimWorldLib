using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C8A RID: 3210
	public class LordJob_StructureThreatCluster : LordJob
	{
		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x06005AEF RID: 23279 RVA: 0x0021700F File Offset: 0x0021520F
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005AF0 RID: 23280 RVA: 0x00217012 File Offset: 0x00215212
		public LordJob_StructureThreatCluster()
		{
		}

		// Token: 0x06005AF1 RID: 23281 RVA: 0x0021701A File Offset: 0x0021521A
		public LordJob_StructureThreatCluster(Faction faction, IntVec3 position, float wanderRadius, bool sendWokenUpMessage = true, bool awakeOnClamor = false)
		{
			this.faction = faction;
			this.sendWokenUpMessage = sendWokenUpMessage;
			this.position = position;
			this.wanderRadius = wanderRadius;
			this.awakeOnClamor = awakeOnClamor;
		}

		// Token: 0x06005AF2 RID: 23282 RVA: 0x00217047 File Offset: 0x00215247
		protected virtual LordToil GetIdleToil()
		{
			return new LordToil_Sleep();
		}

		// Token: 0x06005AF3 RID: 23283 RVA: 0x00217050 File Offset: 0x00215250
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil idleToil = this.GetIdleToil();
			stateGraph.StartingToil = idleToil;
			IntVec3 defendPoint = this.position;
			float? num = new float?(this.wanderRadius);
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(defendPoint, new float?(this.wanderRadius), num);
			stateGraph.AddToil(lordToil_DefendPoint);
			LordToil_HuntDownColonists lordToil_HuntDownColonists = new LordToil_HuntDownColonists();
			stateGraph.AddToil(lordToil_HuntDownColonists);
			Transition transition = new Transition(idleToil, lordToil_DefendPoint, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.DormancyWakeup || (this.awakeOnClamor && signal.type == TriggerSignalType.Clamor)));
			if (this.sendWokenUpMessage)
			{
				transition.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f, new Func<bool>(this.AnyAsleep)));
			}
			transition.AddPostAction(new TransitionAction_WakeAll());
			transition.AddPostAction(this.WakeUpPostAction());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(idleToil, lordToil_HuntDownColonists, false, true);
			transition2.AddTrigger(new Trigger_PawnHarmed(1f, false, Faction.OfPlayer, null, null));
			transition2.AddTrigger(new Trigger_AcquiredTarget(Faction.OfPlayer));
			if (this.sendWokenUpMessage)
			{
				transition2.AddPreAction(new TransitionAction_Message("MessageSleepingPawnsWokenUp".Translate(this.faction.def.pawnsPlural).CapitalizeFirst(), MessageTypeDefOf.ThreatBig, null, 1f, new Func<bool>(this.AnyAsleep)));
			}
			transition2.AddPostAction(new TransitionAction_WakeAll());
			transition2.AddPostAction(this.WakeUpPostAction());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_DefendPoint, lordToil_HuntDownColonists, false, true);
			transition3.AddTrigger(new Trigger_PawnHarmed(1f, false, Faction.OfPlayer, null, null));
			transition3.AddTrigger(new Trigger_AcquiredTarget(Faction.OfPlayer));
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_HuntDownColonists, lordToil_DefendPoint, false, true);
			transition4.AddTrigger(new Trigger_TicksPassedWithoutHarm(1200));
			stateGraph.AddTransition(transition4, false);
			return stateGraph;
		}

		// Token: 0x06005AF4 RID: 23284 RVA: 0x0021726A File Offset: 0x0021546A
		private TransitionAction_Custom WakeUpPostAction()
		{
			return new TransitionAction_Custom(delegate()
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
			});
		}

		// Token: 0x06005AF5 RID: 23285 RVA: 0x00217280 File Offset: 0x00215480
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

		// Token: 0x06005AF6 RID: 23286 RVA: 0x002172F3 File Offset: 0x002154F3
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

		// Token: 0x06005AF7 RID: 23287 RVA: 0x00217320 File Offset: 0x00215520
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.sendWokenUpMessage, "sendWokenUpMessage", true, false);
			Scribe_Values.Look<bool>(ref this.awakeOnClamor, "awakeOnClamor", false, false);
			Scribe_Values.Look<IntVec3>(ref this.position, "position", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.wanderRadius, "wanderRadius", 0f, false);
		}

		// Token: 0x04003423 RID: 13347
		private Faction faction;

		// Token: 0x04003424 RID: 13348
		private bool sendWokenUpMessage;

		// Token: 0x04003425 RID: 13349
		public bool awakeOnClamor;

		// Token: 0x04003426 RID: 13350
		public IntVec3 position;

		// Token: 0x04003427 RID: 13351
		public float wanderRadius;
	}
}
