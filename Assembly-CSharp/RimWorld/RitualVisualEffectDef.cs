using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200172F RID: 5935
	public class RitualVisualEffectDef : Def
	{
		// Token: 0x06009401 RID: 37889 RVA: 0x0035F821 File Offset: 0x0035DA21
		public RitualVisualEffect GetInstance()
		{
			RitualVisualEffect ritualVisualEffect = (RitualVisualEffect)Activator.CreateInstance(this.workerClass);
			ritualVisualEffect.def = this;
			return ritualVisualEffect;
		}

		// Token: 0x0400595B RID: 22875
		public Type workerClass = typeof(RitualVisualEffect);

		// Token: 0x0400595C RID: 22876
		public List<CompProperties_RitualVisualEffect> comps;

		// Token: 0x0400595D RID: 22877
		public Color tintColor = Color.white;
	}
}
