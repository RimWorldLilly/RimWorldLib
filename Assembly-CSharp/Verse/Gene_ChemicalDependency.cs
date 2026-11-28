using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020003E0 RID: 992
	public class Gene_ChemicalDependency : Gene
	{
		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001D36 RID: 7478 RVA: 0x000B262B File Offset: 0x000B082B
		public override bool Active
		{
			get
			{
				return base.Active && this.pawn != null && !this.pawn.IsGhoul;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001D37 RID: 7479 RVA: 0x000B2650 File Offset: 0x000B0850
		public Hediff_ChemicalDependency LinkedHediff
		{
			get
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff_ChemicalDependency hediff_ChemicalDependency = hediffs[i] as Hediff_ChemicalDependency;
					if (hediff_ChemicalDependency != null && hediff_ChemicalDependency.chemical == this.def.chemical)
					{
						return hediff_ChemicalDependency;
					}
				}
				return null;
			}
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x000B26AC File Offset: 0x000B08AC
		public override void PostAdd()
		{
			if (!ModLister.CheckBiotech("Chemical dependency"))
			{
				return;
			}
			base.PostAdd();
			if (this.def.chemical.addictionHediff != null)
			{
				Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(this.def.chemical.addictionHediff, false);
				if (firstHediffOfDef != null)
				{
					this.pawn.health.RemoveHediff(firstHediffOfDef);
				}
			}
			this.AddHediff();
			this.lastIngestedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x000B2730 File Offset: 0x000B0930
		private void AddHediff()
		{
			if (this.Active)
			{
				Hediff_ChemicalDependency hediff_ChemicalDependency = (Hediff_ChemicalDependency)HediffMaker.MakeHediff(HediffDefOf.GeneticDrugNeed, this.pawn, null);
				hediff_ChemicalDependency.chemical = this.def.chemical;
				this.pawn.health.AddHediff(hediff_ChemicalDependency, null, null, null);
			}
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x000B278C File Offset: 0x000B098C
		public override void PostRemove()
		{
			Hediff_ChemicalDependency linkedHediff = this.LinkedHediff;
			if (linkedHediff != null)
			{
				this.pawn.health.RemoveHediff(linkedHediff);
			}
			base.PostRemove();
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x000B27BC File Offset: 0x000B09BC
		public override void Notify_IngestedThing(Thing thing, int numTaken)
		{
			if (!thing.def.thingCategories.NullOrEmpty<ThingCategoryDef>() && !thing.def.thingCategories.Contains(ThingCategoryDefOf.Drugs))
			{
				return;
			}
			CompDrug compDrug = thing.TryGetComp<CompDrug>();
			if (compDrug == null || compDrug.Props.chemical != this.def.chemical)
			{
				return;
			}
			this.Reset();
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x000B281C File Offset: 0x000B0A1C
		public override void Reset()
		{
			Hediff_ChemicalDependency linkedHediff = this.LinkedHediff;
			if (linkedHediff != null)
			{
				linkedHediff.Severity = linkedHediff.def.initialSeverity;
			}
			else
			{
				this.AddHediff();
			}
			this.lastIngestedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x000B285C File Offset: 0x000B0A5C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastIngestedTick, "lastIngestedTick", 0, false);
		}

		// Token: 0x04001770 RID: 6000
		public int lastIngestedTick;
	}
}
