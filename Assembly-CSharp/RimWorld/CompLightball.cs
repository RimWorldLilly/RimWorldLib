using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A24 RID: 6692
	public class CompLightball : CompAutoPowered
	{
		// Token: 0x17001D77 RID: 7543
		// (get) Token: 0x0600A80E RID: 43022 RVA: 0x003BC23F File Offset: 0x003BA43F
		private CompProperties_Lightball Props
		{
			get
			{
				return (CompProperties_Lightball)this.props;
			}
		}

		// Token: 0x17001D78 RID: 7544
		// (get) Token: 0x0600A80F RID: 43023 RVA: 0x003BC24C File Offset: 0x003BA44C
		public bool Playing
		{
			get
			{
				return this.soundToPlay != null;
			}
		}

		// Token: 0x17001D79 RID: 7545
		// (get) Token: 0x0600A810 RID: 43024 RVA: 0x003BC257 File Offset: 0x003BA457
		public SoundDef SoundToPlay
		{
			get
			{
				return this.soundToPlay;
			}
		}

		// Token: 0x17001D7A RID: 7546
		// (get) Token: 0x0600A811 RID: 43025 RVA: 0x003BC25F File Offset: 0x003BA45F
		public override bool WantsToBeOn
		{
			get
			{
				return this.inRitual;
			}
		}

		// Token: 0x0600A812 RID: 43026 RVA: 0x003BC268 File Offset: 0x003BA468
		public override void CompTick()
		{
			base.CompTick();
			if (!ModLister.CheckIdeology("Lightball"))
			{
				return;
			}
			if (this.parent.Spawned && (this.parent.IsHashIntervalTick(20) || this.numActiveSpeakers == -1))
			{
				this.numActiveSpeakers = 0;
				foreach (Thing thing in this.parent.Map.listerBuldingOfDefInProximity.GetForCell(this.parent.Position, (float)this.Props.maxSpeakerDistance, ThingDefOf.Loudspeaker, null))
				{
					CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
					if (thing.GetRoom(RegionType.Set_All) == this.parent.GetRoom(RegionType.Set_All) && compPowerTrader.PowerOn)
					{
						this.numActiveSpeakers++;
					}
				}
				base.UpdateOverlays();
			}
			if (this.compPowerCached == null)
			{
				this.compPowerCached = this.parent.GetComp<CompPowerTrader>();
			}
			if (this.parent.IsHashIntervalTick(30))
			{
				this.inRitual = this.parent.IsRitualTarget();
			}
			if (this.compPowerCached.PowerOn && !this.inRitual)
			{
				this.parent.BroadcastCompSignal("AutoPoweredWantsOff");
			}
			if (!this.compPowerCached.PowerOn || !this.inRitual)
			{
				Mote mote = this.rotationMote;
				if (mote != null)
				{
					mote.Destroy(DestroyMode.Vanish);
				}
				this.rotationMote = null;
				Mote mote2 = this.lightsMote;
				if (mote2 != null)
				{
					mote2.Destroy(DestroyMode.Vanish);
				}
				this.lightsMote = null;
				return;
			}
			if (this.numActiveSpeakers > 0)
			{
				this.soundToPlay = ((!this.Props.soundDefsPerSpeakerCount.NullOrEmpty<SoundDef>()) ? this.Props.soundDefsPerSpeakerCount[Math.Min(this.numActiveSpeakers, this.Props.soundDefsPerSpeakerCount.Count) - 1] : null);
			}
			if (this.rotationMote == null || this.rotationMote.Destroyed)
			{
				this.rotationMote = MoteMaker.MakeStaticMote(this.parent.TrueCenter(), this.parent.Map, ThingDefOf.Mote_LightBall, 1f, false, 0f);
			}
			Mote mote3 = this.rotationMote;
			if (mote3 != null)
			{
				mote3.Maintain();
			}
			if (this.lightsMote == null || this.lightsMote.Destroyed)
			{
				this.lightsMote = MoteMaker.MakeStaticMote(this.parent.TrueCenter(), this.parent.Map, ThingDefOf.Mote_LightBallLights, 1f, false, 0f);
				if (this.lightsMote != null)
				{
					this.lightsMote.rotationRate = -3f;
				}
			}
			Mote mote4 = this.lightsMote;
			if (mote4 == null)
			{
				return;
			}
			mote4.Maintain();
		}

		// Token: 0x0600A813 RID: 43027 RVA: 0x003BC520 File Offset: 0x003BA720
		public override void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag == "RitualStarted")
			{
				this.inRitual = this.parent.IsRitualTarget();
			}
		}

		// Token: 0x0600A814 RID: 43028 RVA: 0x003BC545 File Offset: 0x003BA745
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				this.inRitual = this.parent.IsRitualTarget();
			}
		}

		// Token: 0x0600A815 RID: 43029 RVA: 0x003BC55B File Offset: 0x003BA75B
		public override void PostDrawExtraSelectionOverlays()
		{
			PlaceWorker_ShowSpeakerConnections.DrawConnections(this.parent.def, this.parent.Position, this.parent.Map);
		}

		// Token: 0x040062CA RID: 25290
		private Mote rotationMote;

		// Token: 0x040062CB RID: 25291
		private Mote lightsMote;

		// Token: 0x040062CC RID: 25292
		private bool inRitual;

		// Token: 0x040062CD RID: 25293
		private int numActiveSpeakers = -1;

		// Token: 0x040062CE RID: 25294
		private SoundDef soundToPlay;
	}
}
