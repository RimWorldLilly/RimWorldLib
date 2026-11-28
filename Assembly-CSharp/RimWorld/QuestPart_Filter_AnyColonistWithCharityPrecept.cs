using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001064 RID: 4196
	public class QuestPart_Filter_AnyColonistWithCharityPrecept : QuestPart_Filter
	{
		// Token: 0x0600696F RID: 26991 RVA: 0x0025318D File Offset: 0x0025138D
		protected override bool Pass(SignalArgs args)
		{
			return IdeoUtility.AllColonistsWithCharityPrecept().Any<Pawn>();
		}
	}
}
