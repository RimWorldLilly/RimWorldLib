using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Grammar;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001955 RID: 6485
	public class CompRevenant : ThingComp
	{
		// Token: 0x17001C34 RID: 7220
		// (get) Token: 0x0600A2CB RID: 41675 RVA: 0x003A298C File Offset: 0x003A0B8C
		private Pawn Revenant
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x17001C35 RID: 7221
		// (get) Token: 0x0600A2CC RID: 41676 RVA: 0x003A299C File Offset: 0x003A0B9C
		public string BiosignatureName
		{
			get
			{
				string result;
				if ((result = this.biosignatureName) == null)
				{
					result = (this.biosignatureName = AnomalyUtility.GetBiosignatureName(this.biosignature));
				}
				return result;
			}
		}

		// Token: 0x17001C36 RID: 7222
		// (get) Token: 0x0600A2CD RID: 41677 RVA: 0x003A29C8 File Offset: 0x003A0BC8
		public HediffComp_Invisibility Invisibility
		{
			get
			{
				if (this.invisibility != null)
				{
					return this.invisibility;
				}
				Hediff hediff = this.Revenant.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.HoraxianInvisibility, false);
				if (hediff == null)
				{
					hediff = this.Revenant.health.AddHediff(HediffDefOf.HoraxianInvisibility, null, null, null);
				}
				this.invisibility = ((hediff != null) ? hediff.TryGetComp<HediffComp_Invisibility>() : null);
				return this.invisibility;
			}
		}

		// Token: 0x0600A2CE RID: 41678 RVA: 0x003A2A3C File Offset: 0x003A0C3C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<RevenantState>(ref this.revenantState, "revenantState", RevenantState.Wander, false);
			Scribe_Collections.Look<Pawn>(ref this.revenantVictims, "revenantVictims", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.everRevealed, "everRevealed", false, false);
			Scribe_Values.Look<bool>(ref this.revenantSmearNotified, "revenantSmearNotified", false, false);
			Scribe_Values.Look<int>(ref this.revenantLastLeftSmear, "revenantLastLeftSmear", 0, false);
			Scribe_Values.Look<int>(ref this.becomeInvisibleTick, "becomeInvisibleTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastForcedVisibilityMessage, "lastForcedVisibilityMessage", 0, false);
			Scribe_Values.Look<int>(ref this.escapeSecondStageStartedTick, "escapeSecondStageStartedTick", 0, false);
			Scribe_Values.Look<int>(ref this.biosignature, "biosignature", 0, false);
			Scribe_Values.Look<bool>(ref this.injuredWhileAttacking, "injuredWhileAttacking", false, false);
			Scribe_Values.Look<int>(ref this.nextHypnosis, "nextHypnosis", 0, false);
			Scribe_Values.Look<int>(ref this.lastHeardMessageTick, "lastHeardMessageTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastSeenLetterTick, "lastSeenLetterTick", 0, false);
		}

		// Token: 0x0600A2CF RID: 41679 RVA: 0x003A2B3D File Offset: 0x003A0D3D
		public void SetState(RevenantState state)
		{
			this.revenantState = state;
			this.Revenant.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
		}

		// Token: 0x0600A2D0 RID: 41680 RVA: 0x003A2B5A File Offset: 0x003A0D5A
		public override void PostPostMake()
		{
			this.biosignature = Rand.Int;
		}

		// Token: 0x0600A2D1 RID: 41681 RVA: 0x003A2B68 File Offset: 0x003A0D68
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame > this.becomeInvisibleTick)
			{
				this.Invisibility.BecomeInvisible(false);
				this.becomeInvisibleTick = int.MaxValue;
			}
			if (!this.Revenant.Spawned)
			{
				return;
			}
			if (this.Revenant.IsHashIntervalTick(60) && this.Revenant.IsPsychologicallyInvisible())
			{
				this.CheckIfHeard();
			}
			if (this.Revenant.IsHashIntervalTick(90))
			{
				this.CheckIfSeen();
			}
		}

		// Token: 0x0600A2D2 RID: 41682 RVA: 0x003A2BE4 File Offset: 0x003A0DE4
		private void CheckIfHeard()
		{
			AnalysisDetails analysisDetails;
			if (!Find.AnalysisManager.TryGetAnalysisProgress(this.biosignature, out analysisDetails) || analysisDetails.timesDone < 2)
			{
				return;
			}
			List<Pawn> freeColonistsSpawned = this.Revenant.Map.mapPawns.FreeColonistsSpawned;
			for (int i = 0; i < freeColonistsSpawned.Count; i++)
			{
				Pawn pawn = freeColonistsSpawned[i];
				if (this.Revenant.PositionHeld.InHorDistOf(pawn.PositionHeld, 7.9f) && WanderUtility.InSameRoom(pawn.PositionHeld, this.Revenant.PositionHeld, this.Revenant.Map) && !MentalStateUtility.IsHavingMentalBreak(pawn))
				{
					MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_RevenantHeard, new Vector3(0f, 0f, 1.1f), 1f, -1f);
					GrammarRequest request = default(GrammarRequest);
					request.Includes.Add(RulePackDefOf.RevenantNoises);
					if (Find.TickManager.TicksGame > this.lastHeardMessageTick + 1200)
					{
						Messages.Message("MessageRevenantHeard".Translate(pawn.Named("PAWN"), GrammarResolver.Resolve("verb", request, null, false, null, null, null, false)), pawn, MessageTypeDefOf.ThreatSmall, true);
						this.lastHeardMessageTick = Find.TickManager.TicksGame;
					}
				}
			}
		}

		// Token: 0x0600A2D3 RID: 41683 RVA: 0x003A2D48 File Offset: 0x003A0F48
		private void CheckIfSeen()
		{
			AnalysisDetails analysisDetails;
			if (!Find.AnalysisManager.TryGetAnalysisProgress(this.biosignature, out analysisDetails) || !analysisDetails.Satisfied)
			{
				return;
			}
			List<Pawn> freeColonistsSpawned = this.Revenant.Map.mapPawns.FreeColonistsSpawned;
			bool flag = false;
			for (int i = 0; i < freeColonistsSpawned.Count; i++)
			{
				Pawn pawn = freeColonistsSpawned[i];
				if (!PawnUtility.IsBiologicallyOrArtificiallyBlind(pawn) && this.Revenant.PositionHeld.InHorDistOf(pawn.PositionHeld, 8.9f) && GenSight.LineOfSightToThing(pawn.PositionHeld, this.Revenant, this.Revenant.Map, false, null))
				{
					if (this.Revenant.IsPsychologicallyInvisible() && Find.TickManager.TicksGame > this.lastSeenLetterTick + 1200)
					{
						Find.LetterStack.ReceiveLetter("LetterRevenantSeenLabel".Translate(), "LetterRevenantSeen".Translate(pawn.Named("PAWN")), LetterDefOf.ThreatBig, pawn, null, null, null, null, 0, true);
						this.lastSeenLetterTick = Find.TickManager.TicksGame;
					}
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.Invisibility.BecomeVisible(false);
				this.becomeInvisibleTick = Find.TickManager.TicksGame + 140;
			}
		}

		// Token: 0x0600A2D4 RID: 41684 RVA: 0x003A2E96 File Offset: 0x003A1096
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Revenant.Dead)
			{
				return;
			}
			if (this.revenantState == RevenantState.Attack)
			{
				this.injuredWhileAttacking = true;
			}
			if (this.revenantState == RevenantState.Sleep)
			{
				this.SetState(RevenantState.Escape);
			}
		}

		// Token: 0x0600A2D5 RID: 41685 RVA: 0x003A2EC6 File Offset: 0x003A10C6
		public override void Notify_BecameVisible()
		{
			if (!this.Revenant.Spawned)
			{
				return;
			}
			SoundDefOf.Pawn_Revenant_Revealed.PlayOneShotOnCamera(null);
			this.everRevealed = true;
			this.revenantSmearNotified = true;
		}

		// Token: 0x0600A2D6 RID: 41686 RVA: 0x003A2EEF File Offset: 0x003A10EF
		public override void Notify_BecameInvisible()
		{
			if (!this.Revenant.Spawned)
			{
				return;
			}
			SoundDefOf.Pawn_Revenant_Stealth.PlayOneShot(this.Revenant);
		}

		// Token: 0x0600A2D7 RID: 41687 RVA: 0x003A2F14 File Offset: 0x003A1114
		public override void Notify_ForcedVisible()
		{
			if (!this.Revenant.Spawned)
			{
				return;
			}
			if (!this.Revenant.Position.Fogged(this.Revenant.Map))
			{
				SoundDefOf.Pawn_Revenant_StartledScream.PlayOneShot(this.Revenant);
				if (Find.TickManager.TicksGame > this.lastForcedVisibilityMessage + 2400)
				{
					Messages.Message("MessageRevenantForcedVisibility".Translate(), this.Revenant, MessageTypeDefOf.ThreatSmall, true);
					this.lastForcedVisibilityMessage = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x0600A2D8 RID: 41688 RVA: 0x003A2FAE File Offset: 0x003A11AE
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			this.ClearVictims();
		}

		// Token: 0x0600A2D9 RID: 41689 RVA: 0x003A2FB6 File Offset: 0x003A11B6
		public override void Notify_PassedToWorld()
		{
			this.ClearVictims();
		}

		// Token: 0x0600A2DA RID: 41690 RVA: 0x003A2FC0 File Offset: 0x003A11C0
		private void ClearVictims()
		{
			if (!this.revenantVictims.NullOrEmpty<Pawn>())
			{
				foreach (Pawn pawn in this.revenantVictims)
				{
					Hediff hediff;
					if (pawn == null)
					{
						hediff = null;
					}
					else
					{
						Pawn_HealthTracker health = pawn.health;
						if (health == null)
						{
							hediff = null;
						}
						else
						{
							HediffSet hediffSet = health.hediffSet;
							hediff = ((hediffSet != null) ? hediffSet.GetFirstHediffOfDef(HediffDefOf.RevenantHypnosis, false) : null);
						}
					}
					Hediff hediff2 = hediff;
					if (hediff2 != null)
					{
						pawn.health.RemoveHediff(hediff2);
					}
				}
				this.revenantVictims.Clear();
			}
		}

		// Token: 0x0600A2DB RID: 41691 RVA: 0x003A3060 File Offset: 0x003A1260
		public void Hypnotize(Pawn victim)
		{
			if (victim.Dead)
			{
				return;
			}
			victim.health.AddHediff(HediffDefOf.RevenantHypnosis, null, null, null);
			RestUtility.WakeUp(victim, true);
			this.revenantVictims.Add(victim);
			this.Revenant.Drawer.renderer.SetAnimation(AnimationDefOf.RevenantSpasm);
			this.Revenant.mindState.lastEngageTargetTick = Find.TickManager.TicksGame;
			this.Revenant.mindState.enemyTarget = null;
			this.nextHypnosis = Find.TickManager.TicksGame + Mathf.FloorToInt(RevenantUtility.SearchForTargetCooldownRangeDays.RandomInRange * 60000f);
			if (PawnUtility.ShouldSendNotificationAbout(victim))
			{
				Find.LetterStack.ReceiveLetter("LetterLabelPawnHypnotized".Translate(victim.Named("PAWN")), "LetterPawnHypnotized".Translate(victim.Named("PAWN")), LetterDefOf.NegativeEvent, victim, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600A2DC RID: 41692 RVA: 0x003A3160 File Offset: 0x003A1360
		public override string CompInspectStringExtra()
		{
			TaggedString taggedString = "Biosignature".Translate() + ": " + this.BiosignatureName;
			if (DebugSettings.showHiddenInfo)
			{
				taggedString += "\nState: " + this.revenantState.ToString();
			}
			return taggedString;
		}

		// Token: 0x0600A2DD RID: 41693 RVA: 0x003A31BC File Offset: 0x003A13BC
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompRevenant.<CompGetGizmosExtra>d__45 <CompGetGizmosExtra>d__ = new CompRevenant.<CompGetGizmosExtra>d__45(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04006031 RID: 24625
		private const int ForcedVisibilityMessageCooldownTicks = 2400;

		// Token: 0x04006032 RID: 24626
		private const int HeardCheckInterval = 60;

		// Token: 0x04006033 RID: 24627
		private const int SeenCheckInterval = 90;

		// Token: 0x04006034 RID: 24628
		private const float HeardDistance = 7.9f;

		// Token: 0x04006035 RID: 24629
		private const float SeenDistance = 8.9f;

		// Token: 0x04006036 RID: 24630
		private const int HeardMessageCooldownTicks = 1200;

		// Token: 0x04006037 RID: 24631
		private const int SeenLetterCooldownTicks = 1200;

		// Token: 0x04006038 RID: 24632
		public const int LongerTracksUnlockIndex = 1;

		// Token: 0x04006039 RID: 24633
		public const int CanHearUnlockIndex = 2;

		// Token: 0x0400603A RID: 24634
		public RevenantState revenantState;

		// Token: 0x0400603B RID: 24635
		public List<Pawn> revenantVictims = new List<Pawn>();

		// Token: 0x0400603C RID: 24636
		public bool everRevealed;

		// Token: 0x0400603D RID: 24637
		public bool revenantSmearNotified;

		// Token: 0x0400603E RID: 24638
		public int revenantLastLeftSmear = -99999;

		// Token: 0x0400603F RID: 24639
		public int becomeInvisibleTick = int.MaxValue;

		// Token: 0x04006040 RID: 24640
		public int lastForcedVisibilityMessage = -99999;

		// Token: 0x04006041 RID: 24641
		public int escapeSecondStageStartedTick = -99999;

		// Token: 0x04006042 RID: 24642
		public int biosignature;

		// Token: 0x04006043 RID: 24643
		public bool injuredWhileAttacking;

		// Token: 0x04006044 RID: 24644
		public int nextHypnosis = -99999;

		// Token: 0x04006045 RID: 24645
		private int lastHeardMessageTick = -99999;

		// Token: 0x04006046 RID: 24646
		private int lastSeenLetterTick = -99999;

		// Token: 0x04006047 RID: 24647
		private string biosignatureName;

		// Token: 0x04006048 RID: 24648
		[Unsaved(false)]
		private HediffComp_Invisibility invisibility;
	}
}
