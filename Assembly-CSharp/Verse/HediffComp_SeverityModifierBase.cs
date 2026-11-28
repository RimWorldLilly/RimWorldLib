using System;
using System.Text;

namespace Verse
{
	// Token: 0x02000493 RID: 1171
	public abstract class HediffComp_SeverityModifierBase : HediffComp
	{
		// Token: 0x0600207A RID: 8314
		public abstract float SeverityChangePerDay();

		// Token: 0x0600207B RID: 8315 RVA: 0x000C2E9C File Offset: 0x000C109C
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (base.Pawn.IsHashIntervalTick(200, delta))
			{
				float num = this.SeverityChangePerDay();
				num *= 0.0033333334f;
				severityAdjustment += num;
			}
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000C2ED4 File Offset: 0x000C10D4
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompDebugString());
			if (!base.Pawn.Dead)
			{
				stringBuilder.AppendLine("severity/day: " + this.SeverityChangePerDay().ToString("F3"));
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x04001925 RID: 6437
		protected const int SeverityUpdateInterval = 200;
	}
}
