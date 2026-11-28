using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AA2 RID: 2722
	public class JobDriver_Ingest : JobDriver, IEatingDriver
	{
		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x060051EA RID: 20970 RVA: 0x001E86F0 File Offset: 0x001E68F0
		private Thing IngestibleSource
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x060051EB RID: 20971 RVA: 0x001E8711 File Offset: 0x001E6911
		public bool EatingFromInventory
		{
			get
			{
				return this.eatingFromInventory;
			}
		}

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x060051EC RID: 20972 RVA: 0x001E871C File Offset: 0x001E691C
		public bool GainingNutritionNow
		{
			get
			{
				Thing ingestibleSource = this.IngestibleSource;
				return !ingestibleSource.DestroyedOrNull() && ingestibleSource.def.IsNutritionGivingIngestible && base.CurToil == this.chewing;
			}
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x060051ED RID: 20973 RVA: 0x001E8758 File Offset: 0x001E6958
		private float ChewDurationMultiplier
		{
			get
			{
				Thing ingestibleSource = this.IngestibleSource;
				if (ingestibleSource.def.ingestible != null && !ingestibleSource.def.ingestible.useEatingSpeedStat)
				{
					return 1f;
				}
				return 1f / this.pawn.GetStatValue(StatDefOf.EatingSpeed, true, -1);
			}
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x001E87A9 File Offset: 0x001E69A9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usingNutrientPasteDispenser, "usingNutrientPasteDispenser", false, false);
			Scribe_Values.Look<bool>(ref this.eatingFromInventory, "eatingFromInventory", false, false);
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x001E87D8 File Offset: 0x001E69D8
		public override string GetReport()
		{
			if (this.usingNutrientPasteDispenser)
			{
				return JobUtility.GetResolvedJobReportRaw(this.job.def.reportString, ThingDefOf.MealNutrientPaste.label, ThingDefOf.MealNutrientPaste, "", "", "", "");
			}
			Thing thing = this.job.targetA.Thing;
			if (((thing != null) ? thing.def.ingestible : null) != null)
			{
				if (!thing.def.ingestible.ingestReportStringEat.NullOrEmpty() && (thing.def.ingestible.ingestReportString.NullOrEmpty() || this.pawn.RaceProps.intelligence < Intelligence.ToolUser))
				{
					return thing.def.ingestible.ingestReportStringEat.Formatted(this.job.targetA.Thing.LabelShort, this.job.targetA.Thing);
				}
				if (!thing.def.ingestible.ingestReportString.NullOrEmpty())
				{
					return thing.def.ingestible.ingestReportString.Formatted(this.job.targetA.Thing.LabelShort, this.job.targetA.Thing);
				}
			}
			return base.GetReport();
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x001E8940 File Offset: 0x001E6B40
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usingNutrientPasteDispenser = (this.IngestibleSource is Building_NutrientPasteDispenser);
			this.eatingFromInventory = (this.pawn.inventory != null && this.pawn.inventory.Contains(this.IngestibleSource));
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x001E8994 File Offset: 0x001E6B94
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.pawn.Faction != null && !(this.IngestibleSource is Building_NutrientPasteDispenser))
			{
				Thing ingestibleSource = this.IngestibleSource;
				int maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(ingestibleSource, this.pawn, this.job.count);
				if (!this.pawn.Reserve(ingestibleSource, this.job, 10, maxAmountToPickup, null, errorOnFailed, false))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x001E89FC File Offset: 0x001E6BFC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Ingest.<MakeNewToils>d__19 <MakeNewToils>d__ = new JobDriver_Ingest.<MakeNewToils>d__19(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x001E8A0C File Offset: 0x001E6C0C
		private IEnumerable<Toil> PrepareToIngestToils(Toil chewToil)
		{
			if (this.usingNutrientPasteDispenser)
			{
				return this.PrepareToIngestToils_Dispenser();
			}
			if (this.pawn.RaceProps.ToolUser && (!this.pawn.IsMutant || !this.pawn.mutant.Def.disableEatingAtTable))
			{
				return this.PrepareToIngestToils_ToolUser(chewToil);
			}
			return this.PrepareToIngestToils_NonToolUser();
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x001E8A6C File Offset: 0x001E6C6C
		private IEnumerable<Toil> PrepareToIngestToils_Dispenser()
		{
			JobDriver_Ingest.<PrepareToIngestToils_Dispenser>d__21 <PrepareToIngestToils_Dispenser>d__ = new JobDriver_Ingest.<PrepareToIngestToils_Dispenser>d__21(-2);
			<PrepareToIngestToils_Dispenser>d__.<>4__this = this;
			return <PrepareToIngestToils_Dispenser>d__;
		}

		// Token: 0x060051F5 RID: 20981 RVA: 0x001E8A7C File Offset: 0x001E6C7C
		private IEnumerable<Toil> PrepareToIngestToils_ToolUser(Toil chewToil)
		{
			JobDriver_Ingest.<PrepareToIngestToils_ToolUser>d__22 <PrepareToIngestToils_ToolUser>d__ = new JobDriver_Ingest.<PrepareToIngestToils_ToolUser>d__22(-2);
			<PrepareToIngestToils_ToolUser>d__.<>4__this = this;
			<PrepareToIngestToils_ToolUser>d__.<>3__chewToil = chewToil;
			return <PrepareToIngestToils_ToolUser>d__;
		}

		// Token: 0x060051F6 RID: 20982 RVA: 0x001E8A93 File Offset: 0x001E6C93
		private IEnumerable<Toil> PrepareToIngestToils_NonToolUser()
		{
			JobDriver_Ingest.<PrepareToIngestToils_NonToolUser>d__23 <PrepareToIngestToils_NonToolUser>d__ = new JobDriver_Ingest.<PrepareToIngestToils_NonToolUser>d__23(-2);
			<PrepareToIngestToils_NonToolUser>d__.<>4__this = this;
			return <PrepareToIngestToils_NonToolUser>d__;
		}

		// Token: 0x060051F7 RID: 20983 RVA: 0x001E8AA3 File Offset: 0x001E6CA3
		private IEnumerable<Toil> TakeExtraIngestibles()
		{
			JobDriver_Ingest.<TakeExtraIngestibles>d__24 <TakeExtraIngestibles>d__ = new JobDriver_Ingest.<TakeExtraIngestibles>d__24(-2);
			<TakeExtraIngestibles>d__.<>4__this = this;
			return <TakeExtraIngestibles>d__;
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x001E8AB3 File Offset: 0x001E6CB3
		private Toil ReserveFood()
		{
			Toil toil = ToilMaker.MakeToil("ReserveFood");
			toil.initAction = delegate()
			{
				if (this.pawn.Faction == null)
				{
					return;
				}
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (this.pawn.carryTracker.CarriedThing == thing)
				{
					return;
				}
				int maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(thing, this.pawn, this.job.count);
				if (maxAmountToPickup == 0)
				{
					return;
				}
				if (!this.pawn.Reserve(thing, this.job, 10, maxAmountToPickup, null, true, false))
				{
					string[] array = new string[8];
					array[0] = "Pawn food reservation for ";
					int num = 1;
					Pawn pawn = this.pawn;
					array[num] = ((pawn != null) ? pawn.ToString() : null);
					array[2] = " on job ";
					array[3] = ((this != null) ? this.ToString() : null);
					array[4] = " failed, because it could not register food from ";
					int num2 = 5;
					Thing thing2 = thing;
					array[num2] = ((thing2 != null) ? thing2.ToString() : null);
					array[6] = " - amount: ";
					array[7] = maxAmountToPickup.ToString();
					Log.Error(string.Concat(array));
					this.pawn.jobs.EndCurrentJob(JobCondition.Errored, true, true);
				}
				this.job.count = maxAmountToPickup;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x001E8AE0 File Offset: 0x001E6CE0
		public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool flip)
		{
			IntVec3 cell = this.job.GetTarget(TargetIndex.B).Cell;
			return JobDriver_Ingest.ModifyCarriedThingDrawPosWorker(ref drawPos, ref flip, cell, this.pawn);
		}

		// Token: 0x060051FA RID: 20986 RVA: 0x001E8B10 File Offset: 0x001E6D10
		public static bool ModifyCarriedThingDrawPosWorker(ref Vector3 drawPos, ref bool flip, IntVec3 placeCell, Pawn pawn)
		{
			if (pawn.pather.Moving)
			{
				return false;
			}
			Thing carriedThing = pawn.carryTracker.CarriedThing;
			if (carriedThing == null || !carriedThing.IngestibleNow)
			{
				return false;
			}
			if (placeCell.IsValid && placeCell.AdjacentToCardinal(pawn.Position) && placeCell.HasEatSurface(pawn.Map) && carriedThing.def.ingestible.ingestHoldUsesTable)
			{
				drawPos = new Vector3((float)placeCell.x + 0.5f, drawPos.y, (float)placeCell.z + 0.5f);
				return true;
			}
			HoldOffsetSet ingestHoldOffsetStanding = carriedThing.def.ingestible.ingestHoldOffsetStanding;
			HoldOffset holdOffset = (ingestHoldOffsetStanding != null) ? ingestHoldOffsetStanding.Pick(pawn.Rotation) : null;
			if (holdOffset != null)
			{
				drawPos += holdOffset.offset;
				flip = holdOffset.flip;
				return true;
			}
			return false;
		}

		// Token: 0x040031AA RID: 12714
		private bool usingNutrientPasteDispenser;

		// Token: 0x040031AB RID: 12715
		private bool eatingFromInventory;

		// Token: 0x040031AC RID: 12716
		private Toil chewing;

		// Token: 0x040031AD RID: 12717
		public const float EatCorpseBodyPartsUntilFoodLevelPct = 0.9f;

		// Token: 0x040031AE RID: 12718
		public const TargetIndex IngestibleSourceInd = TargetIndex.A;

		// Token: 0x040031AF RID: 12719
		private const TargetIndex TableCellInd = TargetIndex.B;

		// Token: 0x040031B0 RID: 12720
		private const TargetIndex ExtraIngestiblesToCollectInd = TargetIndex.C;
	}
}
