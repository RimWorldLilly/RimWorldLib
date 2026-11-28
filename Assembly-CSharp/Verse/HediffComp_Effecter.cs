using System;

namespace Verse
{
	// Token: 0x0200042D RID: 1069
	public class HediffComp_Effecter : HediffComp
	{
		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001F20 RID: 7968 RVA: 0x000BDD0D File Offset: 0x000BBF0D
		public HediffCompProperties_Effecter Props
		{
			get
			{
				return (HediffCompProperties_Effecter)this.props;
			}
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x000BDD1C File Offset: 0x000BBF1C
		public EffecterDef CurrentStateEffecter()
		{
			if (this.parent.CurStageIndex >= this.Props.severityIndices.min && (this.Props.severityIndices.max < 0 || this.parent.CurStageIndex <= this.Props.severityIndices.max))
			{
				return this.Props.stateEffecter;
			}
			return null;
		}
	}
}
