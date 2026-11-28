using System;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001969 RID: 6505
	[StaticConstructorOnStartup]
	public class CompSendSignalOnMotion : ThingComp, IThingGlower
	{
		// Token: 0x17001C44 RID: 7236
		// (get) Token: 0x0600A33E RID: 41790 RVA: 0x003A491C File Offset: 0x003A2B1C
		public CompProperties_SendSignalOnMotion Props
		{
			get
			{
				return (CompProperties_SendSignalOnMotion)this.props;
			}
		}

		// Token: 0x17001C45 RID: 7237
		// (get) Token: 0x0600A33F RID: 41791 RVA: 0x003A4929 File Offset: 0x003A2B29
		public bool Sent
		{
			get
			{
				return this.sent;
			}
		}

		// Token: 0x17001C46 RID: 7238
		// (get) Token: 0x0600A340 RID: 41792 RVA: 0x003A4931 File Offset: 0x003A2B31
		public bool Enabled
		{
			get
			{
				return this.ticksUntilEnabled <= 0;
			}
		}

		// Token: 0x0600A341 RID: 41793 RVA: 0x003A493F File Offset: 0x003A2B3F
		public bool ShouldBeLitNow()
		{
			return !this.Sent;
		}

		// Token: 0x0600A342 RID: 41794 RVA: 0x003A494A File Offset: 0x003A2B4A
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.signalTag = this.Props.signalTag;
			this.ticksUntilEnabled = this.Props.enableAfterTicks;
		}

		// Token: 0x0600A343 RID: 41795 RVA: 0x003A4978 File Offset: 0x003A2B78
		public override void CompTick()
		{
			base.CompTick();
			if (this.sent || !this.parent.Spawned)
			{
				return;
			}
			if (this.Enabled && Find.TickManager.TicksGame % 250 == 0)
			{
				this.CompTickRare();
			}
			if (this.ticksUntilEnabled > 0)
			{
				this.ticksUntilEnabled--;
			}
		}

		// Token: 0x0600A344 RID: 41796 RVA: 0x003A49D8 File Offset: 0x003A2BD8
		public override void CompTickRare()
		{
			base.CompTickRare();
			Predicate<Thing> predicate = null;
			if (this.Props.onlyHumanlike)
			{
				predicate = delegate(Thing t)
				{
					Pawn pawn = t as Pawn;
					return pawn != null && pawn.RaceProps.Humanlike;
				};
			}
			Thing thing = null;
			if (this.Props.triggerOnPawnInRoom)
			{
				foreach (Thing thing2 in this.parent.GetRoom(RegionType.Set_All).ContainedAndAdjacentThings)
				{
					if (predicate(thing2))
					{
						thing = thing2;
						break;
					}
				}
			}
			if (thing == null && this.Props.radius > 0f)
			{
				thing = GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), this.Props.radius, predicate, null, 0, -1, false, RegionType.Set_Passable, false, false);
			}
			if (thing != null)
			{
				this.Trigger(thing);
			}
		}

		// Token: 0x0600A345 RID: 41797 RVA: 0x003A4AE4 File Offset: 0x003A2CE4
		protected void Trigger(Thing initiator)
		{
			Effecter effecter = new Effecter(EffecterDefOf.ActivatorProximityTriggered);
			effecter.Trigger(this.parent, TargetInfo.Invalid, -1);
			effecter.Cleanup();
			Messages.Message("MessageActivatorProximityTriggered".Translate(initiator), this.parent, MessageTypeDefOf.ThreatBig, true);
			Find.SignalManager.SendSignal(new Signal(this.signalTag, this.parent.Named("SUBJECT")));
			SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			this.sent = true;
		}

		// Token: 0x0600A346 RID: 41798 RVA: 0x003A4B9A File Offset: 0x003A2D9A
		public void Expire()
		{
			this.sent = true;
		}

		// Token: 0x0600A347 RID: 41799 RVA: 0x003A4BA4 File Offset: 0x003A2DA4
		public override void Notify_SignalReceived(Signal signal)
		{
			Thing thing;
			if (signal.tag == "CompCanBeDormant.WakeUp" && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= 40f)
			{
				this.sent = true;
			}
			LocalTargetInfo localTargetInfo;
			if (!this.sent && signal.tag == CompAbilityEffect_Teleport.SkipUsedSignalTag && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && signal.args.TryGetArg<LocalTargetInfo>("POSITION", out localTargetInfo) && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(localTargetInfo.Cell) <= 40f)
			{
				this.Trigger(thing);
			}
		}

		// Token: 0x0600A348 RID: 41800 RVA: 0x003A4C9C File Offset: 0x003A2E9C
		public override string CompInspectStringExtra()
		{
			if (!this.Enabled)
			{
				return "SendSignalOnCountdownCompTime".Translate(this.ticksUntilEnabled.ToStringTicksToPeriod(true, false, true, false, false)).Resolve();
			}
			if (!this.sent)
			{
				return "radius".Translate().CapitalizeFirst() + ": " + this.Props.radius.ToString("F0");
			}
			return "expired".Translate().CapitalizeFirst();
		}

		// Token: 0x0600A349 RID: 41801 RVA: 0x003A4D34 File Offset: 0x003A2F34
		public override void PostExposeData()
		{
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_Values.Look<bool>(ref this.sent, "sent", false, false);
			Scribe_Values.Look<int>(ref this.ticksUntilEnabled, "ticksUntilEnabled", 0, false);
		}

		// Token: 0x0400606E RID: 24686
		public string signalTag;

		// Token: 0x0400606F RID: 24687
		private bool sent;

		// Token: 0x04006070 RID: 24688
		private int ticksUntilEnabled;

		// Token: 0x04006071 RID: 24689
		private const float MaxDistActivationByOther = 40f;
	}
}
