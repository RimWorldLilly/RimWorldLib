using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200046A RID: 1130
	public class HediffComp_RandomizeStageWithInterval : HediffComp_Randomizer
	{
		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001FE1 RID: 8161 RVA: 0x000C0C66 File Offset: 0x000BEE66
		public HediffCompProperties_RandomizeStageWithInterval Props
		{
			get
			{
				return (HediffCompProperties_RandomizeStageWithInterval)this.props;
			}
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x000C0C74 File Offset: 0x000BEE74
		public override void Randomize()
		{
			int curStageIndex = this.parent.CurStageIndex;
			this.parent.Severity = this.parent.def.stages.RandomElement<HediffStage>().minSeverity;
			int curStageIndex2 = this.parent.CurStageIndex;
			if (curStageIndex != curStageIndex2 && !this.Props.notifyMessage.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.parent.pawn))
			{
				Messages.Message(this.Props.notifyMessage.Formatted(this.parent.pawn.Named("PAWN"), this.parent.def.stages[curStageIndex].label, this.parent.def.stages[curStageIndex2].label), this.parent.pawn, MessageTypeDefOf.NeutralEvent, true);
			}
		}
	}
}
