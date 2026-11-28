using System;

namespace RimWorld
{
	// Token: 0x020016AA RID: 5802
	public abstract class RitualObligationTriggerProperties
	{
		// Token: 0x0600915D RID: 37213 RVA: 0x00353760 File Offset: 0x00351960
		public virtual RitualObligationTrigger GetInstance(Precept_Ritual parent)
		{
			RitualObligationTrigger ritualObligationTrigger = (RitualObligationTrigger)Activator.CreateInstance(this.triggerClass);
			ritualObligationTrigger.ritual = parent;
			return ritualObligationTrigger;
		}

		// Token: 0x0400585D RID: 22621
		public Type triggerClass;

		// Token: 0x0400585E RID: 22622
		public bool mustBePlayerIdeo;
	}
}
