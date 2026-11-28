using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E62 RID: 3682
	public class ThoughtWorker_PsychicArchotechEmanator_Core : ThoughtWorker_PsychicArchotechEmanator
	{
		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x060061DF RID: 25055 RVA: 0x002341BB File Offset: 0x002323BB
		protected override ThingDef EmanatorDef
		{
			get
			{
				return ThingDefOf.ArchonexusCore;
			}
		}

		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x060061E0 RID: 25056 RVA: 0x002341C2 File Offset: 0x002323C2
		protected override int InnerRadius
		{
			get
			{
				return 11;
			}
		}

		// Token: 0x170010C4 RID: 4292
		// (get) Token: 0x060061E1 RID: 25057 RVA: 0x002341C6 File Offset: 0x002323C6
		protected override int OuterRadius
		{
			get
			{
				return 28;
			}
		}
	}
}
