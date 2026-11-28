using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000444 RID: 1092
	public class HediffComp_GiveRandomSituationalThought : HediffComp
	{
		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001F5B RID: 8027 RVA: 0x000BEC38 File Offset: 0x000BCE38
		public HediffCompProperties_GiveRandomSituationalThought Props
		{
			get
			{
				return (HediffCompProperties_GiveRandomSituationalThought)this.props;
			}
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x000BEC45 File Offset: 0x000BCE45
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.selectedThought = this.Props.thoughtDefs.RandomElement<ThoughtDef>();
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001F5D RID: 8029 RVA: 0x000BEC63 File Offset: 0x000BCE63
		public override string CompLabelInBracketsExtra
		{
			get
			{
				ThoughtDef thoughtDef = this.selectedThought;
				if (thoughtDef == null)
				{
					return null;
				}
				return thoughtDef.labelInBracketsExtraForHediff;
			}
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x000BEC76 File Offset: 0x000BCE76
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Defs.Look<ThoughtDef>(ref this.selectedThought, "selectedThought");
		}

		// Token: 0x0400187A RID: 6266
		public ThoughtDef selectedThought;
	}
}
