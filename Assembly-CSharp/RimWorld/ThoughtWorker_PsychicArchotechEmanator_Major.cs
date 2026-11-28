using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E60 RID: 3680
	public class ThoughtWorker_PsychicArchotechEmanator_Major : ThoughtWorker_PsychicArchotechEmanator
	{
		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x060061D7 RID: 25047 RVA: 0x0023418E File Offset: 0x0023238E
		protected override ThingDef EmanatorDef
		{
			get
			{
				return ThingDefOf.MajorArchotechStructureStudiable;
			}
		}

		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x060061D8 RID: 25048 RVA: 0x00234195 File Offset: 0x00232395
		protected override int InnerRadius
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x060061D9 RID: 25049 RVA: 0x00234198 File Offset: 0x00232398
		protected override int OuterRadius
		{
			get
			{
				return 14;
			}
		}
	}
}
