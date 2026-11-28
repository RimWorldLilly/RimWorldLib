using System;

namespace Verse
{
	// Token: 0x02000403 RID: 1027
	public class HediffGiver_Random : HediffGiver
	{
		// Token: 0x06001DD3 RID: 7635 RVA: 0x000B7F3C File Offset: 0x000B613C
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float num = this.mtbDays;
			float num2 = this.ChanceFactor(pawn);
			if (num2 == 0f)
			{
				return;
			}
			if (Rand.MTBEventOccurs(num / num2, 60000f, 60f) && base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}

		// Token: 0x040017DC RID: 6108
		public float mtbDays;
	}
}
