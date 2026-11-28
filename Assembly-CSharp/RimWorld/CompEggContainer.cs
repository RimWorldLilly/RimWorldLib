using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019BA RID: 6586
	public class CompEggContainer : CompThingContainer
	{
		// Token: 0x17001CCD RID: 7373
		// (get) Token: 0x0600A55A RID: 42330 RVA: 0x003AFA16 File Offset: 0x003ADC16
		public bool CanEmpty
		{
			get
			{
				return !base.Empty && (this.EggsRuined || base.ContainedThing.stackCount >= base.Props.minCountToEmpty);
			}
		}

		// Token: 0x17001CCE RID: 7374
		// (get) Token: 0x0600A55B RID: 42331 RVA: 0x003AFA48 File Offset: 0x003ADC48
		private bool EggsRuined
		{
			get
			{
				if (base.Empty)
				{
					return false;
				}
				CompTemperatureRuinable compTemperatureRuinable = base.ContainedThing.TryGetComp<CompTemperatureRuinable>();
				if (compTemperatureRuinable != null && compTemperatureRuinable.Ruined)
				{
					return true;
				}
				CompHatcher compHatcher = base.ContainedThing.TryGetComp<CompHatcher>();
				return compHatcher != null && compHatcher.TemperatureDamaged;
			}
		}

		// Token: 0x0600A55C RID: 42332 RVA: 0x003AFA94 File Offset: 0x003ADC94
		public override bool Accepts(ThingDef thingDef)
		{
			return (base.ContainedThing == null || !this.EggsRuined) && (thingDef.thingCategories != null && (thingDef.thingCategories.Contains(ThingCategoryDefOf.EggsFertilized) || thingDef.thingCategories.Contains(ThingCategoryDefOf.EggsUnfertilized))) && base.Accepts(thingDef);
		}

		// Token: 0x0600A55D RID: 42333 RVA: 0x003AFAE8 File Offset: 0x003ADCE8
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!base.Empty && !this.EggsRuined)
			{
				CompHatcher compHatcher = base.ContainedThing.TryGetComp<CompHatcher>();
				if (compHatcher != null)
				{
					string text2 = compHatcher.CompInspectStringExtra();
					if (!text2.NullOrEmpty())
					{
						text = text + "\n" + text2;
					}
				}
			}
			return text;
		}

		// Token: 0x0600A55E RID: 42334 RVA: 0x003AFB38 File Offset: 0x003ADD38
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompEggContainer.<CompGetGizmosExtra>d__6 <CompGetGizmosExtra>d__ = new CompEggContainer.<CompGetGizmosExtra>d__6(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}
	}
}
