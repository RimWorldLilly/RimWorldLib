using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B1E RID: 6942
	[StaticConstructorOnStartup]
	public class Xenogerm : GeneSetHolderBase
	{
		// Token: 0x17002009 RID: 8201
		// (get) Token: 0x0600B1B8 RID: 45496 RVA: 0x003E9B8C File Offset: 0x003E7D8C
		public override string LabelNoCount
		{
			get
			{
				if (this.xenotypeName.NullOrEmpty())
				{
					return base.LabelNoCount;
				}
				return "NamedXenogerm".Translate(this.xenotypeName.Named("NAME"));
			}
		}

		// Token: 0x1700200A RID: 8202
		// (get) Token: 0x0600B1B9 RID: 45497 RVA: 0x003E9BC4 File Offset: 0x003E7DC4
		private int RequiredMedicineForImplanting
		{
			get
			{
				int num = 0;
				for (int i = 0; i < RecipeDefOf.ImplantXenogerm.ingredients.Count; i++)
				{
					IngredientCount ingredientCount = RecipeDefOf.ImplantXenogerm.ingredients[i];
					if (ingredientCount.filter.Allows(ThingDefOf.MedicineIndustrial))
					{
						num += (int)ingredientCount.GetBaseCount();
					}
				}
				return num;
			}
		}

		// Token: 0x0600B1BA RID: 45498 RVA: 0x003E9C1C File Offset: 0x003E7E1C
		public override void Notify_DebugSpawned()
		{
			this.geneSet = GeneUtility.GenerateGeneSet(null);
			this.xenotypeName = "Unique".Translate();
			this.iconDef = XenotypeIconDefOf.Basic;
		}

		// Token: 0x0600B1BB RID: 45499 RVA: 0x003E9C5D File Offset: 0x003E7E5D
		public override void PostMake()
		{
			if (!ModLister.CheckBiotech("xenogerm"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostMake();
		}

		// Token: 0x0600B1BC RID: 45500 RVA: 0x003E9C7C File Offset: 0x003E7E7C
		public void SetTargetPawn(Pawn newTarget)
		{
			int trueMax = HediffDefOf.XenogerminationComa.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks.TrueMax;
			TaggedString taggedString = "ImplantXenogermWarningDesc".Translate(newTarget.Named("PAWN"), trueMax.ToStringTicksToPeriod(true, false, true, true, false).Named("COMADURATION"));
			if (newTarget.genes.Xenogenes.Any<Gene>())
			{
				taggedString += "\n\n" + "ImplantXenogermWarningOverwriteXenogenes".Translate(newTarget.Named("PAWN"), newTarget.genes.XenotypeLabelCap.Named("XENOTYPE"), (from x in newTarget.genes.Xenogenes
				select x.LabelCap).ToLineList("  - ", false).Named("XENOGENES"));
			}
			int num = GeneUtility.MetabolismAfterImplanting(newTarget, this.geneSet);
			taggedString += "\n\n" + "ImplantXenogermWarningNewMetabolism".Translate(newTarget.Named("PAWN"), num.Named("MET"), GeneTuning.MetabolismToFoodConsumptionFactorCurve.Evaluate((float)num).ToStringPercent().Named("CONSUMPTION"));
			taggedString += "\n\n" + "WouldYouLikeToContinue".Translate();
			Predicate<Bill> <>9__2;
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(taggedString, delegate()
			{
				Pawn pawn = this.targetPawn;
				Bill bill;
				if (pawn == null)
				{
					bill = null;
				}
				else
				{
					BillStack billStack = pawn.BillStack;
					if (billStack == null)
					{
						bill = null;
					}
					else
					{
						List<Bill> bills = billStack.Bills;
						if (bills == null)
						{
							bill = null;
						}
						else
						{
							Predicate<Bill> predicate;
							if ((predicate = <>9__2) == null)
							{
								predicate = (<>9__2 = ((Bill x) => x.xenogerm == this));
							}
							bill = bills.FirstOrDefault(predicate);
						}
					}
				}
				Bill bill2 = bill;
				if (bill2 != null)
				{
					this.targetPawn.BillStack.Delete(bill2);
				}
				HealthCardUtility.CreateSurgeryBill(newTarget, RecipeDefOf.ImplantXenogerm, null, null, true).xenogerm = this;
				this.targetPawn = newTarget;
				this.SendImplantationLetter(newTarget);
			}, true, null, WindowLayer.Dialog));
		}

		// Token: 0x0600B1BD RID: 45501 RVA: 0x003E9E2C File Offset: 0x003E802C
		private void SendImplantationLetter(Pawn targetPawn)
		{
			string arg = string.Empty;
			if (!targetPawn.InBed() && !targetPawn.Map.listerBuildings.allBuildingsColonist.Any((Building x) => x is Building_Bed && RestUtility.CanUseBedEver(targetPawn, x.def) && ((Building_Bed)x).Medical))
			{
				arg = "XenogermOrderedImplantedBedNeeded".Translate(targetPawn.Named("PAWN"));
			}
			int requiredMedicineForImplanting = this.RequiredMedicineForImplanting;
			string arg2 = string.Empty;
			if (targetPawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Medicine).Sum((Thing x) => x.stackCount) < requiredMedicineForImplanting)
			{
				arg2 = "XenogermOrderedImplantedMedicineNeeded".Translate(requiredMedicineForImplanting.Named("MEDICINENEEDED"));
			}
			Find.LetterStack.ReceiveLetter("LetterLabelXenogermOrderedImplanted".Translate(), "LetterXenogermOrderedImplanted".Translate(targetPawn.Named("PAWN"), requiredMedicineForImplanting.Named("MEDICINENEEDED"), arg.Named("BEDINFO"), arg2.Named("MEDICINEINFO")), LetterDefOf.NeutralEvent, null, 0, true);
		}

		// Token: 0x0600B1BE RID: 45502 RVA: 0x003E9F68 File Offset: 0x003E8168
		public void Initialize(List<Genepack> genepacks, string xenotypeName, XenotypeIconDef iconDef)
		{
			this.xenotypeName = xenotypeName;
			this.iconDef = iconDef;
			this.geneSet = new GeneSet();
			for (int i = 0; i < genepacks.Count; i++)
			{
				if (genepacks[i].GeneSet != null)
				{
					List<GeneDef> genesListForReading = genepacks[i].GeneSet.GenesListForReading;
					for (int j = 0; j < genesListForReading.Count; j++)
					{
						this.geneSet.AddGene(genesListForReading[j]);
					}
				}
			}
		}

		// Token: 0x0600B1BF RID: 45503 RVA: 0x003E9FE2 File Offset: 0x003E81E2
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Xenogerm.<GetFloatMenuOptions>d__14 <GetFloatMenuOptions>d__ = new Xenogerm.<GetFloatMenuOptions>d__14(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600B1C0 RID: 45504 RVA: 0x003E9FFC File Offset: 0x003E81FC
		public bool PawnIdeoDisallowsImplanting(Pawn selPawn)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive)
			{
				return false;
			}
			if (!IdeoUtility.DoerWillingToDo(HistoryEventDefOf.PropagateBloodfeederGene, selPawn))
			{
				if (base.GeneSet.GenesListForReading.Any((GeneDef x) => x == GeneDefOf.Bloodfeeder))
				{
					return true;
				}
			}
			return !IdeoUtility.DoerWillingToDo(HistoryEventDefOf.BecomeNonPreferredXenotype, selPawn);
		}

		// Token: 0x0600B1C1 RID: 45505 RVA: 0x003EA069 File Offset: 0x003E8269
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Xenogerm.<GetGizmos>d__16 <GetGizmos>d__ = new Xenogerm.<GetGizmos>d__16(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600B1C2 RID: 45506 RVA: 0x003EA079 File Offset: 0x003E8279
		public void Notify_BillRemoved()
		{
			this.targetPawn = null;
		}

		// Token: 0x0600B1C3 RID: 45507 RVA: 0x003EA084 File Offset: 0x003E8284
		public bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!target.IsValid || target.Pawn == null)
			{
				return false;
			}
			if (target.Pawn.IsQuestLodger())
			{
				if (showMessages)
				{
					Messages.Message("MessageCannotImplantInTempFactionMembers".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!target.Pawn.IsColonist && !target.Pawn.IsPrisonerOfColony && !target.Pawn.IsSlaveOfColony)
			{
				if (showMessages)
				{
					Messages.Message("MessageCanOnlyTargetColonistsPrisonersAndSlaves".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600B1C4 RID: 45508 RVA: 0x003EA134 File Offset: 0x003E8334
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.targetPawn != null && this.targetPawn.Map == base.Map)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.targetPawn.TrueCenter());
			}
		}

		// Token: 0x0600B1C5 RID: 45509 RVA: 0x003EA170 File Offset: 0x003E8370
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.targetPawn, "targetPawn", false);
			Scribe_Values.Look<string>(ref this.xenotypeName, "xenotypeName", null, false);
			Scribe_Defs.Look<XenotypeIconDef>(ref this.iconDef, "iconDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.iconDef == null)
			{
				this.iconDef = XenotypeIconDefOf.Basic;
			}
		}

		// Token: 0x040066A4 RID: 26276
		private Pawn targetPawn;

		// Token: 0x040066A5 RID: 26277
		public string xenotypeName;

		// Token: 0x040066A6 RID: 26278
		public XenotypeIconDef iconDef;

		// Token: 0x040066A7 RID: 26279
		private static readonly CachedTexture ImplantTex = new CachedTexture("UI/Gizmos/ImplantGenes");

		// Token: 0x040066A8 RID: 26280
		private static readonly Texture2D CancelIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
	}
}
