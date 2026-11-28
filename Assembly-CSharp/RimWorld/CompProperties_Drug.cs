using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC8 RID: 3784
	public class CompProperties_Drug : CompProperties
	{
		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x06006367 RID: 25447 RVA: 0x0023B449 File Offset: 0x00239649
		public bool Addictive
		{
			get
			{
				return this.addictiveness > 0f;
			}
		}

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06006368 RID: 25448 RVA: 0x0023B458 File Offset: 0x00239658
		public bool CanCauseOverdose
		{
			get
			{
				return this.overdoseSeverityOffset.TrueMax > 0f;
			}
		}

		// Token: 0x06006369 RID: 25449 RVA: 0x0023B46C File Offset: 0x0023966C
		public CompProperties_Drug()
		{
			this.compClass = typeof(CompDrug);
		}

		// Token: 0x0600636A RID: 25450 RVA: 0x0023B4A5 File Offset: 0x002396A5
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Drug.<ConfigErrors>d__15 <ConfigErrors>d__ = new CompProperties_Drug.<ConfigErrors>d__15(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400379A RID: 14234
		public ChemicalDef chemical;

		// Token: 0x0400379B RID: 14235
		public float addictiveness;

		// Token: 0x0400379C RID: 14236
		public float minToleranceToAddict;

		// Token: 0x0400379D RID: 14237
		public float existingAddictionSeverityOffset = 0.1f;

		// Token: 0x0400379E RID: 14238
		public float needLevelOffset = 1f;

		// Token: 0x0400379F RID: 14239
		public FloatRange overdoseSeverityOffset = FloatRange.Zero;

		// Token: 0x040037A0 RID: 14240
		public float largeOverdoseChance;

		// Token: 0x040037A1 RID: 14241
		public bool isCombatEnhancingDrug;

		// Token: 0x040037A2 RID: 14242
		public bool teetotalerCanConsume;

		// Token: 0x040037A3 RID: 14243
		public float listOrder;
	}
}
