using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002366 RID: 9062
	public class TimedDetectionRaids : WorldObjectComp
	{
		// Token: 0x1700277E RID: 10110
		// (get) Token: 0x0600E0A2 RID: 57506 RVA: 0x00537E8F File Offset: 0x0053608F
		public bool NextRaidCountdownActiveAndVisible
		{
			get
			{
				return this.ticksLeftToSendRaid >= 0 && this.ticksLeftTillNotifyPlayer == 0;
			}
		}

		// Token: 0x1700277F RID: 10111
		// (get) Token: 0x0600E0A3 RID: 57507 RVA: 0x00537EA5 File Offset: 0x005360A5
		public string DetectionCountdownTimeLeftString
		{
			get
			{
				if (!this.NextRaidCountdownActiveAndVisible)
				{
					return "";
				}
				return TimedDetectionRaids.GetDetectionCountdownTimeLeftString(this.ticksLeftToSendRaid);
			}
		}

		// Token: 0x17002780 RID: 10112
		// (get) Token: 0x0600E0A4 RID: 57508 RVA: 0x00537EC0 File Offset: 0x005360C0
		private Faction RaidFaction
		{
			get
			{
				if (this.parent.Faction != null && !this.parent.Faction.IsPlayer && !this.parent.Faction.def.raidsForbidden)
				{
					return this.parent.Faction;
				}
				return Faction.OfMechanoids;
			}
		}

		// Token: 0x17002781 RID: 10113
		// (get) Token: 0x0600E0A5 RID: 57509 RVA: 0x00537F14 File Offset: 0x00536114
		public int TicksLeftToSendRaids
		{
			get
			{
				return this.ticksLeftToSendRaid;
			}
		}

		// Token: 0x17002782 RID: 10114
		// (get) Token: 0x0600E0A6 RID: 57510 RVA: 0x00537F1C File Offset: 0x0053611C
		public int RaidsSentCount
		{
			get
			{
				return this.raidsSentCount;
			}
		}

		// Token: 0x17002783 RID: 10115
		// (get) Token: 0x0600E0A7 RID: 57511 RVA: 0x00537F24 File Offset: 0x00536124
		public bool DetectionCountdownStarted
		{
			get
			{
				return this.ticksLeftToSendRaid >= 0;
			}
		}

		// Token: 0x0600E0A8 RID: 57512 RVA: 0x00537F32 File Offset: 0x00536132
		public void StartDetectionCountdown(int ticks, int notifyTicks = -1)
		{
			this.ticksLeftToSendRaid = ticks;
			this.ticksLeftTillNotifyPlayer = ((notifyTicks == -1) ? Mathf.Min((int)(60000f * Rand.Range(0.8f, 1.2f)), ticks / 2) : notifyTicks);
		}

		// Token: 0x0600E0A9 RID: 57513 RVA: 0x00537F68 File Offset: 0x00536168
		public void ResetCountdown()
		{
			this.ticksLeftTillNotifyPlayer = (this.ticksLeftToSendRaid = -1);
		}

		// Token: 0x0600E0AA RID: 57514 RVA: 0x00537F85 File Offset: 0x00536185
		public void SetNotifiedSilently()
		{
			this.ticksLeftTillNotifyPlayer = 0;
		}

		// Token: 0x0600E0AB RID: 57515 RVA: 0x00537F90 File Offset: 0x00536190
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToSendRaid, "ticksLeftToForceExitAndRemoveMap", -1, false);
			Scribe_Values.Look<int>(ref this.ticksLeftTillNotifyPlayer, "ticksLeftTillNotifyPlayer", -1, false);
			Scribe_Values.Look<bool>(ref this.alertRaidsArrivingIn, "alertRaidsArrivingIn", false, false);
			Scribe_Values.Look<int>(ref this.raidsSentCount, "raidsSentCount", 0, false);
			Scribe_Values.Look<FloatRange>(ref this.delayRangeHours, "delayRangeHours", TimedDetectionRaids.DefaultDelayRangeHours, false);
		}

		// Token: 0x0600E0AC RID: 57516 RVA: 0x00538004 File Offset: 0x00536204
		public override string CompInspectStringExtra()
		{
			string text = null;
			if (this.NextRaidCountdownActiveAndVisible)
			{
				text += "CaravanDetectedRaidCountdown".Translate(this.DetectionCountdownTimeLeftString) + ".\n";
			}
			if (Prefs.DevMode)
			{
				if (this.ticksLeftToSendRaid != -1)
				{
					text = text + "[DEV]: Time left to send raid: " + this.ticksLeftToSendRaid.ToStringTicksToPeriod(true, false, true, true, false) + "\n";
				}
				if (this.ticksLeftTillNotifyPlayer != -1)
				{
					text = text + "[DEV]: Time left till notify player about incoming raid: " + this.ticksLeftTillNotifyPlayer.ToStringTicksToPeriod(true, false, true, true, false) + "\n";
				}
			}
			if (text != null)
			{
				text = text.TrimEndNewlines();
			}
			return text;
		}

		// Token: 0x0600E0AD RID: 57517 RVA: 0x005380AC File Offset: 0x005362AC
		public override void CompTickInterval(int delta)
		{
			MapParent mapParent = (MapParent)this.parent;
			if (mapParent.HasMap)
			{
				if (this.ticksLeftTillNotifyPlayer > 0)
				{
					this.ticksLeftTillNotifyPlayer -= delta;
					if (this.ticksLeftTillNotifyPlayer <= 0)
					{
						this.NotifyPlayer();
					}
				}
				if (this.ticksLeftToSendRaid > 0)
				{
					this.ticksLeftToSendRaid -= delta;
					if (this.ticksLeftToSendRaid <= 0)
					{
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = mapParent.Map;
						incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(incidentParms.target) * 1.5f;
						incidentParms.faction = this.RaidFaction;
						this.ticksLeftToSendRaid = (int)(this.delayRangeHours.RandomInRange * 2500f);
						if (IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms))
						{
							Messages.Message("MessageCaravanDetectedRaidArrived".Translate(incidentParms.faction.def.pawnsPlural, incidentParms.faction, this.ticksLeftToSendRaid.ToStringTicksToDays("F1")), MessageTypeDefOf.ThreatBig, true);
							this.raidsSentCount++;
							return;
						}
					}
				}
			}
			else
			{
				this.ResetCountdown();
			}
		}

		// Token: 0x0600E0AE RID: 57518 RVA: 0x005381E0 File Offset: 0x005363E0
		private void NotifyPlayer()
		{
			Find.LetterStack.ReceiveLetter("LetterLabelSiteCountdownStarted".Translate(), "LetterTextSiteCountdownStarted".Translate(this.ticksLeftToSendRaid.ToStringTicksToDays("F1"), this.RaidFaction.def.pawnsPlural, this.RaidFaction), LetterDefOf.ThreatBig, this.parent, null, null, null, null, 0, true);
			this.alertRaidsArrivingIn = true;
		}

		// Token: 0x0600E0AF RID: 57519 RVA: 0x0053825D File Offset: 0x0053645D
		public static string GetDetectionCountdownTimeLeftString(int ticksLeft)
		{
			if (ticksLeft < 0)
			{
				return "";
			}
			return ticksLeft.ToStringTicksToPeriod(true, false, true, true, false);
		}

		// Token: 0x0600E0B0 RID: 57520 RVA: 0x00538274 File Offset: 0x00536474
		public override IEnumerable<Gizmo> GetGizmos()
		{
			TimedDetectionRaids.<GetGizmos>d__27 <GetGizmos>d__ = new TimedDetectionRaids.<GetGizmos>d__27(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600E0B1 RID: 57521 RVA: 0x00538284 File Offset: 0x00536484
		public void CopyFrom(TimedDetectionRaids other)
		{
			this.ticksLeftToSendRaid = other.ticksLeftToSendRaid;
			this.ticksLeftTillNotifyPlayer = other.ticksLeftTillNotifyPlayer;
			this.delayRangeHours = other.delayRangeHours;
		}

		// Token: 0x0400900D RID: 36877
		public bool alertRaidsArrivingIn;

		// Token: 0x0400900E RID: 36878
		public const float RaidThreatPointsMultiplier = 1.5f;

		// Token: 0x0400900F RID: 36879
		private static readonly FloatRange DefaultDelayRangeHours = new FloatRange(18f, 24f);

		// Token: 0x04009010 RID: 36880
		private int ticksLeftToSendRaid = -1;

		// Token: 0x04009011 RID: 36881
		private int ticksLeftTillNotifyPlayer = -1;

		// Token: 0x04009012 RID: 36882
		private int raidsSentCount;

		// Token: 0x04009013 RID: 36883
		public FloatRange delayRangeHours = TimedDetectionRaids.DefaultDelayRangeHours;
	}
}
