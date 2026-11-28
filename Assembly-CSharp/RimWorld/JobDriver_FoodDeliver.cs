using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A9F RID: 2719
	public class JobDriver_FoodDeliver : JobDriver
	{
		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x060051D2 RID: 20946 RVA: 0x001E829B File Offset: 0x001E649B
		private Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetB.Thing;
			}
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x001E82B2 File Offset: 0x001E64B2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usingNutrientPasteDispenser, "usingNutrientPasteDispenser", false, false);
			Scribe_Values.Look<bool>(ref this.eatingFromInventory, "eatingFromInventory", false, false);
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x001E82E0 File Offset: 0x001E64E0
		public override string GetReport()
		{
			if (this.job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser && this.Deliveree != null)
			{
				return JobUtility.GetResolvedJobReportRaw(this.job.def.reportString, ThingDefOf.MealNutrientPaste.label, ThingDefOf.MealNutrientPaste, this.Deliveree.LabelShort, this.Deliveree, "", "");
			}
			return base.GetReport();
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x001E8358 File Offset: 0x001E6558
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (base.TargetThingA is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(base.TargetThingA));
		}

		// Token: 0x060051D6 RID: 20950 RVA: 0x001E83AB File Offset: 0x001E65AB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060051D7 RID: 20951 RVA: 0x001E83CE File Offset: 0x001E65CE
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FoodDeliver.<MakeNewToils>d__10 <MakeNewToils>d__ = new JobDriver_FoodDeliver.<MakeNewToils>d__10(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040031A0 RID: 12704
		private bool usingNutrientPasteDispenser;

		// Token: 0x040031A1 RID: 12705
		private bool eatingFromInventory;

		// Token: 0x040031A2 RID: 12706
		private const TargetIndex FoodSourceInd = TargetIndex.A;

		// Token: 0x040031A3 RID: 12707
		private const TargetIndex DelivereeInd = TargetIndex.B;
	}
}
