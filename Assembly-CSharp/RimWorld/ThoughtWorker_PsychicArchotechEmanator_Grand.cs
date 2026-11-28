using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E61 RID: 3681
	public class ThoughtWorker_PsychicArchotechEmanator_Grand : ThoughtWorker_PsychicArchotechEmanator
	{
		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x060061DB RID: 25051 RVA: 0x002341A4 File Offset: 0x002323A4
		protected override ThingDef EmanatorDef
		{
			get
			{
				return ThingDefOf.GrandArchotechStructure;
			}
		}

		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x060061DC RID: 25052 RVA: 0x002341AB File Offset: 0x002323AB
		protected override int InnerRadius
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x060061DD RID: 25053 RVA: 0x002341AF File Offset: 0x002323AF
		protected override int OuterRadius
		{
			get
			{
				return 18;
			}
		}
	}
}
