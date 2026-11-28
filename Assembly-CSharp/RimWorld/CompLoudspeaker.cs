using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001A26 RID: 6694
	public class CompLoudspeaker : CompAutoPowered, IThingGlower
	{
		// Token: 0x17001D7B RID: 7547
		// (get) Token: 0x0600A81A RID: 43034 RVA: 0x003BC5CA File Offset: 0x003BA7CA
		private CompProperties_Loudspeaker Props
		{
			get
			{
				return (CompProperties_Loudspeaker)this.props;
			}
		}

		// Token: 0x17001D7C RID: 7548
		// (get) Token: 0x0600A81B RID: 43035 RVA: 0x003BC5D7 File Offset: 0x003BA7D7
		public bool Active
		{
			get
			{
				return this.inRitual;
			}
		}

		// Token: 0x17001D7D RID: 7549
		// (get) Token: 0x0600A81C RID: 43036 RVA: 0x003BC5DF File Offset: 0x003BA7DF
		public override bool WantsToBeOn
		{
			get
			{
				return this.inRitual;
			}
		}

		// Token: 0x0600A81D RID: 43037 RVA: 0x003BC5E7 File Offset: 0x003BA7E7
		public bool ShouldBeLitNow()
		{
			return this.Active;
		}

		// Token: 0x0600A81E RID: 43038 RVA: 0x003BC5F0 File Offset: 0x003BA7F0
		public override void CompTick()
		{
			base.CompTick();
			if (!ModLister.CheckIdeology("Speaker"))
			{
				return;
			}
			if (this.compPowerCached == null)
			{
				this.compPowerCached = this.parent.GetComp<CompPowerTrader>();
			}
			if (this.parent.IsHashIntervalTick(30) || !this.initialized)
			{
				this.initialized = true;
				this.UpdateInRitual();
				base.UpdateOverlays();
			}
			if (this.compPowerCached.PowerOn && this.inRitual)
			{
				if (this.parent.Rotation == Rot4.North && (this.lightsMote == null || this.lightsMote.Destroyed))
				{
					this.lightsMote = MoteMaker.MakeStaticMote(this.parent.TrueCenter(), this.parent.Map, ThingDefOf.Mote_LoudspeakerLights, 1f, false, 0f);
				}
				Mote mote = this.lightsMote;
				if (mote != null)
				{
					mote.Maintain();
				}
			}
			else
			{
				Mote mote2 = this.lightsMote;
				if (mote2 != null)
				{
					mote2.Destroy(DestroyMode.Vanish);
				}
				this.lightsMote = null;
			}
			if (this.compPowerCached.PowerOn && !this.inRitual)
			{
				this.parent.BroadcastCompSignal("AutoPoweredWantsOff");
			}
		}

		// Token: 0x0600A81F RID: 43039 RVA: 0x003BC716 File Offset: 0x003BA916
		public override void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag == "RitualStarted")
			{
				this.UpdateInRitual();
			}
		}

		// Token: 0x0600A820 RID: 43040 RVA: 0x003BC730 File Offset: 0x003BA930
		private void UpdateInRitual()
		{
			this.inRitual = false;
			if (!this.parent.Spawned)
			{
				return;
			}
			foreach (Lord lord in this.parent.Map.lordManager.lords)
			{
				LordJob_Ritual lordJob_Ritual = lord.LordJob as LordJob_Ritual;
				if (lordJob_Ritual != null && lordJob_Ritual.selectedTarget.Thing != null && lordJob_Ritual.selectedTarget.Thing.def == ThingDefOf.LightBall && this.parent.GetRoom(RegionType.Set_All) == lordJob_Ritual.selectedTarget.Thing.GetRoom(RegionType.Set_All))
				{
					this.inRitual = true;
					break;
				}
			}
		}

		// Token: 0x0600A821 RID: 43041 RVA: 0x003BC7FC File Offset: 0x003BA9FC
		public override void PostDrawExtraSelectionOverlays()
		{
			PlaceWorker_ShowSpeakerConnections.DrawConnections(this.parent.def, this.parent.Position, this.parent.Map);
		}

		// Token: 0x040062CF RID: 25295
		public const int Range = 12;

		// Token: 0x040062D0 RID: 25296
		private Mote lightsMote;

		// Token: 0x040062D1 RID: 25297
		private bool inRitual;

		// Token: 0x040062D2 RID: 25298
		private bool initialized;
	}
}
