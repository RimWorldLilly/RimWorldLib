using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBB RID: 3771
	public class CompProperties_NeuralSupercharger : CompProperties_Rechargeable
	{
		// Token: 0x06006342 RID: 25410 RVA: 0x0023ADB0 File Offset: 0x00238FB0
		public CompProperties_NeuralSupercharger()
		{
			this.compClass = typeof(CompNeuralSupercharger);
		}

		// Token: 0x06006343 RID: 25411 RVA: 0x0023ADC8 File Offset: 0x00238FC8
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_NeuralSupercharger.<ConfigErrors>d__3 <ConfigErrors>d__ = new CompProperties_NeuralSupercharger.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003778 RID: 14200
		[MustTranslate]
		public string jobString;

		// Token: 0x04003779 RID: 14201
		public EffecterDef effectCharged;
	}
}
