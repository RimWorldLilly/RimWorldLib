using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010EA RID: 4330
	public class QuestPart_RequirementsToAcceptPlayerWealth : QuestPart_RequirementsToAccept
	{
		// Token: 0x06006C2E RID: 27694 RVA: 0x0025FA93 File Offset: 0x0025DC93
		public override AcceptanceReport CanAccept()
		{
			if (WealthUtility.PlayerWealth < this.requiredPlayerWealth)
			{
				return new AcceptanceReport("QuestRequiredPlayerWealth".Translate(this.requiredPlayerWealth.ToStringMoney(null)));
			}
			return true;
		}

		// Token: 0x06006C2F RID: 27695 RVA: 0x0025FACE File Offset: 0x0025DCCE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.requiredPlayerWealth, "requiredPlayerWealth", 0f, false);
		}

		// Token: 0x040044E3 RID: 17635
		public float requiredPlayerWealth = -1f;
	}
}
