using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C75 RID: 3189
	public abstract class LordJob_MechanoidDefendBase : LordJob
	{
		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x06005A72 RID: 23154 RVA: 0x002145A8 File Offset: 0x002127A8
		public override bool KeepExistingWhileHasAnyBuilding
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005A73 RID: 23155 RVA: 0x002145AB File Offset: 0x002127AB
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.isMechCluster && !this.mechClusterDefeated && !MechClusterUtility.AnyThreatBuilding(this.things))
			{
				this.OnDefeat();
			}
		}

		// Token: 0x06005A74 RID: 23156 RVA: 0x002145D6 File Offset: 0x002127D6
		public override void Notify_LordDestroyed()
		{
			if (this.isMechCluster && !this.mechClusterDefeated)
			{
				this.OnDefeat();
			}
		}

		// Token: 0x06005A75 RID: 23157 RVA: 0x002145F0 File Offset: 0x002127F0
		private void OnDefeat()
		{
			foreach (Thing thing in this.things)
			{
				thing.SetFaction(null, null);
				CompSendSignalOnMotion compSendSignalOnMotion = thing.TryGetComp<CompSendSignalOnMotion>();
				if (compSendSignalOnMotion != null)
				{
					compSendSignalOnMotion.Expire();
				}
				CompSendSignalOnCountdown compSendSignalOnCountdown = thing.TryGetComp<CompSendSignalOnCountdown>();
				if (compSendSignalOnCountdown != null)
				{
					compSendSignalOnCountdown.ticksLeft = 0;
				}
				ThingWithComps thingWithComps = thing as ThingWithComps;
				if (thingWithComps != null)
				{
					thingWithComps.BroadcastCompSignal("MechClusterDefeated");
				}
			}
			this.lord.Notify_MechClusterDefeated();
			for (int i = 0; i < this.thingsToNotifyOnDefeat.Count; i++)
			{
				this.thingsToNotifyOnDefeat[i].Notify_LordDestroyed();
			}
			if (!base.Map.IsPlayerHome)
			{
				IdeoUtility.Notify_PlayerRaidedSomeone(base.Map.mapPawns.FreeColonistsSpawned);
			}
			this.mechClusterDefeated = true;
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				if (needs != null)
				{
					Need_Mood mood = needs.mood;
					if (mood != null)
					{
						mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DefeatedMechCluster, null, null);
					}
				}
			}
			QuestUtility.SendQuestTargetSignals(this.lord.questTags, "AllEnemiesDefeated");
			Messages.Message("MessageMechClusterDefeated".Translate(), new LookTargets(this.defSpot, base.Map), MessageTypeDefOf.PositiveEvent, true);
			SoundDefOf.MechClusterDefeated.PlayOneShotOnCamera(base.Map);
		}

		// Token: 0x06005A76 RID: 23158 RVA: 0x00214798 File Offset: 0x00212998
		public void AddThingToNotifyOnDefeat(Thing t)
		{
			this.thingsToNotifyOnDefeat.AddUnique(t);
		}

		// Token: 0x06005A77 RID: 23159 RVA: 0x002147A8 File Offset: 0x002129A8
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defSpot, "defSpot", default(IntVec3), false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<float>(ref this.defendRadius, "defendRadius", 0f, false);
			Scribe_Values.Look<bool>(ref this.canAssaultColony, "canAssaultColony", false, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.thingsToNotifyOnDefeat, "thingsToNotifyOnDefeat", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.isMechCluster, "isMechCluster", false, false);
			Scribe_Values.Look<bool>(ref this.mechClusterDefeated, "mechClusterDefeated", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x.DestroyedOrNull());
				this.thingsToNotifyOnDefeat.RemoveAll((Thing x) => x.DestroyedOrNull());
			}
		}

		// Token: 0x040033DE RID: 13278
		public List<Thing> things = new List<Thing>();

		// Token: 0x040033DF RID: 13279
		protected List<Thing> thingsToNotifyOnDefeat = new List<Thing>();

		// Token: 0x040033E0 RID: 13280
		protected IntVec3 defSpot;

		// Token: 0x040033E1 RID: 13281
		protected Faction faction;

		// Token: 0x040033E2 RID: 13282
		protected float defendRadius;

		// Token: 0x040033E3 RID: 13283
		protected bool canAssaultColony;

		// Token: 0x040033E4 RID: 13284
		protected bool isMechCluster;

		// Token: 0x040033E5 RID: 13285
		protected bool mechClusterDefeated;
	}
}
