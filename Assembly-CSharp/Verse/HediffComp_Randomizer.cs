using System;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x02000468 RID: 1128
	public abstract class HediffComp_Randomizer : HediffComp
	{
		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x000C0B90 File Offset: 0x000BED90
		private HediffCompProperties_Randomizer Props
		{
			get
			{
				return (HediffCompProperties_Randomizer)this.props;
			}
		}

		// Token: 0x06001FD9 RID: 8153
		public abstract void Randomize();

		// Token: 0x06001FDA RID: 8154 RVA: 0x000C0B9D File Offset: 0x000BED9D
		public override void CompPostMake()
		{
			this.SetNextRandomizationTick();
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x000C0BA5 File Offset: 0x000BEDA5
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame >= this.nextRandomizationTick)
			{
				this.Randomize();
				this.SetNextRandomizationTick();
			}
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x000C0BC5 File Offset: 0x000BEDC5
		public void SetNextRandomizationTick()
		{
			this.nextRandomizationTick = Find.TickManager.TicksGame + this.Props.ticksToRandomize.RandomInRange;
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000C0BE8 File Offset: 0x000BEDE8
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.nextRandomizationTick, "nextRandomizationTick", 0, false);
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x000C0C04 File Offset: 0x000BEE04
		public override string CompDebugString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
			defaultInterpolatedStringHandler.AppendLiteral("ticks until randomization: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.nextRandomizationTick - Find.TickManager.TicksGame);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040018D1 RID: 6353
		private int nextRandomizationTick;
	}
}
