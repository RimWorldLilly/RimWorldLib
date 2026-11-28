using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010FD RID: 4349
	public class QuestPart_StartWick : QuestPart
	{
		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x06006C87 RID: 27783 RVA: 0x00260F6E File Offset: 0x0025F16E
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_StartWick.<get_QuestLookTargets>d__4 <get_QuestLookTargets>d__ = new QuestPart_StartWick.<get_QuestLookTargets>d__4(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006C88 RID: 27784 RVA: 0x00260F80 File Offset: 0x0025F180
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.explosiveThing != null)
			{
				CompExplosive compExplosive = this.explosiveThing.TryGetComp<CompExplosive>();
				if (compExplosive != null)
				{
					compExplosive.StartWick(this.initiator);
				}
			}
		}

		// Token: 0x06006C89 RID: 27785 RVA: 0x00260FCA File Offset: 0x0025F1CA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Thing>(ref this.explosiveThing, "explosiveThing", false);
			Scribe_References.Look<Thing>(ref this.initiator, "initiator", false);
		}

		// Token: 0x04004516 RID: 17686
		public string inSignal;

		// Token: 0x04004517 RID: 17687
		public Thing explosiveThing;

		// Token: 0x04004518 RID: 17688
		public Thing initiator;
	}
}
