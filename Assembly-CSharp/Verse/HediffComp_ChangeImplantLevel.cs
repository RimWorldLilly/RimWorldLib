using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000414 RID: 1044
	public class HediffComp_ChangeImplantLevel : HediffComp
	{
		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001EC0 RID: 7872 RVA: 0x000BC94C File Offset: 0x000BAB4C
		public HediffCompProperties_ChangeImplantLevel Props
		{
			get
			{
				return (HediffCompProperties_ChangeImplantLevel)this.props;
			}
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x000BC95C File Offset: 0x000BAB5C
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			float mtbDays = this.Props.probabilityPerStage[this.parent.CurStageIndex].mtbDays;
			if (mtbDays <= 0f || !base.Pawn.IsHashIntervalTick(60, delta))
			{
				return;
			}
			ChangeImplantLevel_Probability changeImplantLevel_Probability = this.Props.probabilityPerStage[this.parent.CurStageIndex];
			if ((this.lastChangeLevelTick < 0 || (float)(Find.TickManager.TicksGame - this.lastChangeLevelTick) >= changeImplantLevel_Probability.minIntervalDays * 60000f) && Rand.MTBEventOccurs(mtbDays, 60000f, 60f))
			{
				Hediff_Level hediff_Level = this.parent.pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.implant, false) as Hediff_Level;
				if (hediff_Level != null)
				{
					hediff_Level.ChangeLevel(this.Props.levelOffset);
					this.lastChangeLevelTick = Find.TickManager.TicksGame;
					Messages.Message("MessageLostImplantLevelFromHediff".Translate(this.parent.pawn.Named("PAWN"), hediff_Level.LabelBase, this.parent.Label), this.parent.pawn, MessageTypeDefOf.NegativeEvent, true);
				}
			}
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x000BCAAA File Offset: 0x000BACAA
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.lastChangeLevelTick, "lastChangeLevelTick", 0, false);
		}

		// Token: 0x04001827 RID: 6183
		public int lastChangeLevelTick = -1;
	}
}
