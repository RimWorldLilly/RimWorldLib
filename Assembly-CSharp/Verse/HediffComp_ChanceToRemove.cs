using System;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x02000411 RID: 1041
	public class HediffComp_ChanceToRemove : HediffComp
	{
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x000BC7EA File Offset: 0x000BA9EA
		public HediffCompProperties_ChanceToRemove Props
		{
			get
			{
				return (HediffCompProperties_ChanceToRemove)this.props;
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001EB7 RID: 7863 RVA: 0x000BC7F7 File Offset: 0x000BA9F7
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || (this.removeNextInterval && this.currentInterval <= 0);
			}
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x000BC81C File Offset: 0x000BAA1C
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.CompShouldRemove)
			{
				return;
			}
			if (this.currentInterval > 0)
			{
				this.currentInterval -= delta;
				return;
			}
			if (Rand.Chance(this.Props.chance))
			{
				this.removeNextInterval = true;
				this.currentInterval = Rand.Range(0, this.Props.intervalTicks);
				return;
			}
			this.currentInterval = this.Props.intervalTicks;
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x000BC88C File Offset: 0x000BAA8C
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Values.Look<bool>(ref this.removeNextInterval, "removeNextInterval", false, false);
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x000BC8B4 File Offset: 0x000BAAB4
		public override string CompDebugString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 2);
			defaultInterpolatedStringHandler.AppendLiteral("currentInterval: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.currentInterval);
			defaultInterpolatedStringHandler.AppendLiteral("\nremove: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.removeNextInterval);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04001820 RID: 6176
		public int currentInterval;

		// Token: 0x04001821 RID: 6177
		public bool removeNextInterval;
	}
}
