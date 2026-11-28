using System;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001977 RID: 6519
	public class CompSightstealer : ThingComp
	{
		// Token: 0x17001C70 RID: 7280
		// (get) Token: 0x0600A3C2 RID: 41922 RVA: 0x003A82BB File Offset: 0x003A64BB
		private Pawn Sightstealer
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x17001C71 RID: 7281
		// (get) Token: 0x0600A3C3 RID: 41923 RVA: 0x003A82C8 File Offset: 0x003A64C8
		private HediffComp_Invisibility Invisibility
		{
			get
			{
				HediffComp_Invisibility result;
				if ((result = this.invisibility) == null)
				{
					Hediff firstHediffOfDef = this.Sightstealer.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.HoraxianInvisibility, false);
					result = (this.invisibility = ((firstHediffOfDef != null) ? firstHediffOfDef.TryGetComp<HediffComp_Invisibility>() : null));
				}
				return result;
			}
		}

		// Token: 0x0600A3C4 RID: 41924 RVA: 0x003A830F File Offset: 0x003A650F
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastDetectedTick, "lastDetectedTick", 0, false);
		}

		// Token: 0x0600A3C5 RID: 41925 RVA: 0x003A8324 File Offset: 0x003A6524
		public override void CompTick()
		{
			if (this.Sightstealer.IsShambler)
			{
				return;
			}
			if (this.Invisibility == null)
			{
				this.Sightstealer.health.AddHediff(HediffDefOf.HoraxianInvisibility, null, null, null);
			}
			if (!this.Sightstealer.Spawned)
			{
				return;
			}
			if (this.Sightstealer.IsHashIntervalTick(7))
			{
				if (Find.TickManager.TicksGame > this.lastDetectedTick + 1200)
				{
					this.CheckDetected();
				}
				if (Find.TickManager.TicksGame > this.lastDetectedTick + 1200)
				{
					this.Invisibility.BecomeInvisible(false);
				}
			}
			Lord lord = this.Sightstealer.GetLord();
			if (lord != null && Rand.MTBEventOccurs(600f, 1f, 1f))
			{
				Job curJob = this.Sightstealer.CurJob;
				if (((curJob != null) ? curJob.def : null) == JobDefOf.Wait || lord.LordJob is LordJob_EntitySwarm)
				{
					Pawn_CallTracker caller = this.Sightstealer.caller;
					if (caller == null)
					{
						return;
					}
					caller.DoCall(false);
				}
			}
		}

		// Token: 0x0600A3C6 RID: 41926 RVA: 0x003A842C File Offset: 0x003A662C
		private void CheckDetected()
		{
			foreach (Thing thing in this.Sightstealer.Map.listerThings.ThingsInGroup(ThingRequestGroup.Pawn))
			{
				Pawn pawn = (Pawn)thing;
				if (this.PawnCanDetect(pawn))
				{
					if (!this.Invisibility.PsychologicallyVisible)
					{
						this.Invisibility.BecomeVisible(false);
					}
					this.lastDetectedTick = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x0600A3C7 RID: 41927 RVA: 0x003A84C0 File Offset: 0x003A66C0
		private bool PawnCanDetect(Pawn pawn)
		{
			return pawn.Faction != Faction.OfEntities && !pawn.Downed && pawn.Awake() && !pawn.IsAnimal && this.Sightstealer.Position.InHorDistOf(pawn.Position, CompSightstealer.GetPawnSightRadius(pawn, this.Sightstealer)) && GenSight.LineOfSightToThing(pawn.Position, this.Sightstealer, this.parent.Map, false, null);
		}

		// Token: 0x0600A3C8 RID: 41928 RVA: 0x003A8540 File Offset: 0x003A6740
		private static float GetPawnSightRadius(Pawn pawn, Pawn sightstealer)
		{
			float num = 14f;
			if (pawn.genes == null || pawn.genes.AffectedByDarkness)
			{
				float t = sightstealer.Map.glowGrid.GroundGlowAt(sightstealer.Position, false, false);
				num *= Mathf.Lerp(0.33f, 1f, t);
			}
			return num * pawn.health.capacities.GetLevel(PawnCapacityDefOf.Sight);
		}

		// Token: 0x0600A3C9 RID: 41929 RVA: 0x003A85AD File Offset: 0x003A67AD
		public override void Notify_UsedVerb(Pawn pawn, Verb verb)
		{
			base.Notify_UsedVerb(pawn, verb);
			if (this.Sightstealer.IsShambler)
			{
				return;
			}
			this.Invisibility.BecomeVisible(false);
			this.lastDetectedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600A3CA RID: 41930 RVA: 0x003A85E4 File Offset: 0x003A67E4
		public override void Notify_BecameVisible()
		{
			SoundDefOf.Pawn_Sightstealer_Howl.PlayOneShotOnCamera(null);
			foreach (Thing thing in this.Sightstealer.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.Pawn))
			{
				Pawn pawn = (Pawn)thing;
				if (pawn.kindDef == PawnKindDefOf.Sightstealer && pawn != this.Sightstealer && pawn.Position.InHorDistOf(this.Sightstealer.Position, 30f) && !pawn.IsPsychologicallyInvisible() && GenSight.LineOfSight(pawn.Position, this.Sightstealer.Position, pawn.Map))
				{
					return;
				}
			}
			if (RealTime.LastRealTime > CompSightstealer.lastNotified + 60f)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelSightstealerRevealed".Translate(), "LetterSightstealerRevealed".Translate(), LetterDefOf.ThreatBig, this.Sightstealer, null, null, null, null, 6, true);
			}
			else
			{
				Messages.Message("MessageSightstealerRevealed".Translate(), this.Sightstealer, MessageTypeDefOf.ThreatBig, true);
			}
			CompSightstealer.lastNotified = RealTime.LastRealTime;
			this.lastDetectedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x040060B7 RID: 24759
		private const float BaseVisibleRadius = 14f;

		// Token: 0x040060B8 RID: 24760
		private const int UndetectedTimeout = 1200;

		// Token: 0x040060B9 RID: 24761
		private const int CheckDetectedIntervalTicks = 7;

		// Token: 0x040060BA RID: 24762
		private const float FirstDetectedRadius = 30f;

		// Token: 0x040060BB RID: 24763
		private const int RevealedLetterDelayTicks = 6;

		// Token: 0x040060BC RID: 24764
		private const int AmbushCallMTBTicks = 600;

		// Token: 0x040060BD RID: 24765
		[Unsaved(false)]
		private HediffComp_Invisibility invisibility;

		// Token: 0x040060BE RID: 24766
		private int lastDetectedTick = -99999;

		// Token: 0x040060BF RID: 24767
		private static float lastNotified = -99999f;

		// Token: 0x040060C0 RID: 24768
		private const float NotifyCooldownSeconds = 60f;
	}
}
