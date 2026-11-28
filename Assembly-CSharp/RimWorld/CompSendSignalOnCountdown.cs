using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001967 RID: 6503
	public class CompSendSignalOnCountdown : ThingComp, IThingGlower
	{
		// Token: 0x17001C43 RID: 7235
		// (get) Token: 0x0600A331 RID: 41777 RVA: 0x003A467B File Offset: 0x003A287B
		private CompProperties_SendSignalOnCountdown Props
		{
			get
			{
				return (CompProperties_SendSignalOnCountdown)this.props;
			}
		}

		// Token: 0x0600A332 RID: 41778 RVA: 0x003A4688 File Offset: 0x003A2888
		public bool ShouldBeLitNow()
		{
			return this.ticksLeft > 0;
		}

		// Token: 0x0600A333 RID: 41779 RVA: 0x003A4693 File Offset: 0x003A2893
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.signalTag = this.Props.signalTag;
			this.ticksLeft = Mathf.CeilToInt(Rand.ByCurve(this.Props.countdownCurveTicks));
		}

		// Token: 0x0600A334 RID: 41780 RVA: 0x003A46C8 File Offset: 0x003A28C8
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompSendSignalOnCountdown.<CompGetGizmosExtra>d__7 <CompGetGizmosExtra>d__ = new CompSendSignalOnCountdown.<CompGetGizmosExtra>d__7(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A335 RID: 41781 RVA: 0x003A46D8 File Offset: 0x003A28D8
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x0600A336 RID: 41782 RVA: 0x003A46F2 File Offset: 0x003A28F2
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.TickRareWorker();
		}

		// Token: 0x0600A337 RID: 41783 RVA: 0x003A4700 File Offset: 0x003A2900
		public void TickRareWorker()
		{
			if (this.ticksLeft <= 0 || !this.parent.Spawned)
			{
				return;
			}
			this.ticksLeft -= 250;
			if (this.ticksLeft <= 0)
			{
				Find.SignalManager.SendSignal(new Signal(this.signalTag, this.parent.Named("SUBJECT")));
				SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
		}

		// Token: 0x0600A338 RID: 41784 RVA: 0x003A4790 File Offset: 0x003A2990
		public override string CompInspectStringExtra()
		{
			if (!this.parent.Spawned)
			{
				return null;
			}
			if (this.ticksLeft <= 0)
			{
				return "expired".Translate().CapitalizeFirst();
			}
			return "SendSignalOnCountdownCompTime".Translate(this.ticksLeft.ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
		}

		// Token: 0x0600A339 RID: 41785 RVA: 0x003A47F4 File Offset: 0x003A29F4
		public override void Notify_SignalReceived(Signal signal)
		{
			Thing thing;
			if (signal.tag == "CompCanBeDormant.WakeUp" && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= 40f)
			{
				this.ticksLeft = 0;
			}
		}

		// Token: 0x0600A33A RID: 41786 RVA: 0x003A486B File Offset: 0x003A2A6B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04006065 RID: 24677
		public string signalTag;

		// Token: 0x04006066 RID: 24678
		public int ticksLeft;

		// Token: 0x04006067 RID: 24679
		private const float MaxDistActivationByOther = 40f;
	}
}
