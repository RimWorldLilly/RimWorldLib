using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003FB RID: 1019
	public class HediffGiver_AddSeverity : HediffGiver
	{
		// Token: 0x06001DBB RID: 7611 RVA: 0x000B7640 File Offset: 0x000B5840
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (pawn.IsNestedHashIntervalTick(60, HediffGiver_AddSeverity.mtbCheckInterval) && Rand.MTBEventOccurs(this.mtbHours, 2500f, (float)HediffGiver_AddSeverity.mtbCheckInterval))
			{
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
				pawn.health.hediffSet.GetFirstHediffOfDef(this.hediff, false).Severity += this.severityAmount;
			}
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x000B76AF File Offset: 0x000B58AF
		public override IEnumerable<string> ConfigErrors()
		{
			HediffGiver_AddSeverity.<ConfigErrors>d__4 <ConfigErrors>d__ = new HediffGiver_AddSeverity.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040017CA RID: 6090
		public float severityAmount = float.NaN;

		// Token: 0x040017CB RID: 6091
		public float mtbHours = -1f;

		// Token: 0x040017CC RID: 6092
		private static int mtbCheckInterval = 1200;
	}
}
