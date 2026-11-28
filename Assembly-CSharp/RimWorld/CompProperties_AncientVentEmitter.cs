using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBF RID: 3775
	public class CompProperties_AncientVentEmitter : CompProperties
	{
		// Token: 0x0600635B RID: 25435 RVA: 0x0023B2E2 File Offset: 0x002394E2
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_AncientVentEmitter.<ConfigErrors>d__8 <ConfigErrors>d__ = new CompProperties_AncientVentEmitter.<ConfigErrors>d__8(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003783 RID: 14211
		public GameConditionDef conditionToCause;

		// Token: 0x04003784 RID: 14212
		public int onDurationMtbDays;

		// Token: 0x04003785 RID: 14213
		public int offDurationMtbDays;

		// Token: 0x04003786 RID: 14214
		public int heatToPush;

		// Token: 0x04003787 RID: 14215
		public string startupMessageKey;

		// Token: 0x04003788 RID: 14216
		public string shutdownMessageKey;

		// Token: 0x04003789 RID: 14217
		public string activeInspectStringKey;

		// Token: 0x0400378A RID: 14218
		public SoundDef activeSound;
	}
}
