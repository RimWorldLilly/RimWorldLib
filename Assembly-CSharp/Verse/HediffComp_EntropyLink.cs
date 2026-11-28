using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200042F RID: 1071
	public class HediffComp_EntropyLink : HediffComp
	{
		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001F24 RID: 7972 RVA: 0x000BDD93 File Offset: 0x000BBF93
		public HediffCompProperties_EntropyLink Props
		{
			get
			{
				return (HediffCompProperties_EntropyLink)this.props;
			}
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x000BDDA0 File Offset: 0x000BBFA0
		public override void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
			base.Notify_EntropyGained(baseAmount, finalAmount, source);
			HediffComp_Link hediffComp_Link = this.parent.TryGetComp<HediffComp_Link>();
			if (hediffComp_Link != null && hediffComp_Link.other != source && hediffComp_Link.OtherPawn.psychicEntropy != null)
			{
				hediffComp_Link.OtherPawn.psychicEntropy.TryAddEntropy(baseAmount * this.Props.entropyTransferAmount, this.parent.pawn, false, false);
				MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkPulse, this.parent.pawn, hediffComp_Link.other);
			}
		}
	}
}
