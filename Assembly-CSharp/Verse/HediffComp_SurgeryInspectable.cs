using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000498 RID: 1176
	public class HediffComp_SurgeryInspectable : HediffComp
	{
		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600208D RID: 8333 RVA: 0x000C328B File Offset: 0x000C148B
		public HediffCompProperties_SurgeryInspectable Props
		{
			get
			{
				return (HediffCompProperties_SurgeryInspectable)this.props;
			}
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x000C3298 File Offset: 0x000C1498
		public virtual SurgicalInspectionOutcome DoSurgicalInspection(Pawn surgeon)
		{
			return SurgicalInspectionOutcome.Detected;
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x000C329B File Offset: 0x000C149B
		public virtual void DoSurgicalInspectionVisible(Pawn surgeon)
		{
		}
	}
}
