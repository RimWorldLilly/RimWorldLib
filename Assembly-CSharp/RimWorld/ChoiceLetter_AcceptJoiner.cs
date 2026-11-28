using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D4F RID: 7503
	public class ChoiceLetter_AcceptJoiner : ChoiceLetter
	{
		// Token: 0x170023D0 RID: 9168
		// (get) Token: 0x0600C22D RID: 49709 RVA: 0x0046C5E3 File Offset: 0x0046A7E3
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023D1 RID: 9169
		// (get) Token: 0x0600C22E RID: 49710 RVA: 0x0046C5E6 File Offset: 0x0046A7E6
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && this.quest != null && (this.quest.State == QuestState.Ongoing || this.quest.State == QuestState.NotYetAccepted);
			}
		}

		// Token: 0x170023D2 RID: 9170
		// (get) Token: 0x0600C22F RID: 49711 RVA: 0x0046C618 File Offset: 0x0046A818
		public Map MapToUse
		{
			get
			{
				Map result;
				if ((result = this.overrideMap) == null)
				{
					result = (this.lookTargets.PrimaryTarget.Map ?? Find.AnyPlayerHomeMap);
				}
				return result;
			}
		}

		// Token: 0x170023D3 RID: 9171
		// (get) Token: 0x0600C230 RID: 49712 RVA: 0x0046C64B File Offset: 0x0046A84B
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				ChoiceLetter_AcceptJoiner.<get_Choices>d__10 <get_Choices>d__ = new ChoiceLetter_AcceptJoiner.<get_Choices>d__10(-2);
				<get_Choices>d__.<>4__this = this;
				return <get_Choices>d__;
			}
		}

		// Token: 0x0600C231 RID: 49713 RVA: 0x0046C65B File Offset: 0x0046A85B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalAccept, "signalAccept", null, false);
			Scribe_Values.Look<string>(ref this.signalReject, "signalReject", null, false);
			Scribe_References.Look<Map>(ref this.overrideMap, "overrideMap", false);
		}

		// Token: 0x04006F0A RID: 28426
		public string signalAccept;

		// Token: 0x04006F0B RID: 28427
		public string signalReject;

		// Token: 0x04006F0C RID: 28428
		public Map overrideMap;
	}
}
