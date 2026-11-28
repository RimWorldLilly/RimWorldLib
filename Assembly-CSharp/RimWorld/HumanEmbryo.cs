using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B11 RID: 6929
	[StaticConstructorOnStartup]
	public class HumanEmbryo : GeneSetHolderBase
	{
		// Token: 0x17001FEC RID: 8172
		// (get) Token: 0x0600B119 RID: 45337 RVA: 0x003E6751 File Offset: 0x003E4951
		public Pawn Mother
		{
			get
			{
				List<Pawn> pawnSources = this.TryGetComp<CompHasPawnSources>().pawnSources;
				if (pawnSources == null)
				{
					return null;
				}
				return pawnSources.FirstOrDefault((Pawn p) => p.gender == Gender.Female);
			}
		}

		// Token: 0x17001FED RID: 8173
		// (get) Token: 0x0600B11A RID: 45338 RVA: 0x003E6788 File Offset: 0x003E4988
		public Pawn Father
		{
			get
			{
				List<Pawn> pawnSources = this.TryGetComp<CompHasPawnSources>().pawnSources;
				if (pawnSources == null)
				{
					return null;
				}
				return pawnSources.FirstOrDefault((Pawn p) => p.gender == Gender.Male);
			}
		}

		// Token: 0x17001FEE RID: 8174
		// (get) Token: 0x0600B11B RID: 45339 RVA: 0x003E67BF File Offset: 0x003E49BF
		protected override string InspectGeneDescription
		{
			get
			{
				return "InspectGenesEmbryoDesc".Translate();
			}
		}

		// Token: 0x0600B11C RID: 45340 RVA: 0x003E67D0 File Offset: 0x003E49D0
		public override void PostMake()
		{
			if (!ModLister.CheckBiotech("human embryo"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostMake();
		}

		// Token: 0x0600B11D RID: 45341 RVA: 0x003E67EC File Offset: 0x003E49EC
		public override IEnumerable<Gizmo> GetGizmos()
		{
			HumanEmbryo.<GetGizmos>d__11 <GetGizmos>d__ = new HumanEmbryo.<GetGizmos>d__11(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600B11E RID: 45342 RVA: 0x003E67FC File Offset: 0x003E49FC
		private void EnsureImplantTargetValid()
		{
			if (this.implantTarget == null)
			{
				return;
			}
			Pawn pawn = this.implantTarget as Pawn;
			if (pawn != null)
			{
				if (pawn.Destroyed || !this.ImplantPawnValid(false))
				{
					this.implantTarget = null;
					return;
				}
			}
			else
			{
				Building_GrowthVat building_GrowthVat = this.implantTarget as Building_GrowthVat;
				if (building_GrowthVat != null && (building_GrowthVat.Destroyed || !this.ImplantVatValid(false)))
				{
					this.implantTarget = null;
				}
			}
		}

		// Token: 0x0600B11F RID: 45343 RVA: 0x003E6864 File Offset: 0x003E4A64
		private bool ImplantPawnValid(bool cancel)
		{
			Pawn pawn = this.implantTarget as Pawn;
			if (pawn != null)
			{
				foreach (Bill bill in pawn.BillStack)
				{
					Bill_Medical bill_Medical = bill as Bill_Medical;
					if (bill_Medical != null && !bill_Medical.uniqueRequiredIngredients.NullOrEmpty<Thing>())
					{
						foreach (Thing thing in bill_Medical.uniqueRequiredIngredients)
						{
							if (this == thing)
							{
								if (cancel)
								{
									bill_Medical.billStack.Delete(bill_Medical);
									this.implantTarget = null;
									return false;
								}
								return true;
							}
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600B120 RID: 45344 RVA: 0x003E6938 File Offset: 0x003E4B38
		private bool ImplantVatValid(bool cancel)
		{
			Building_GrowthVat building_GrowthVat = this.implantTarget as Building_GrowthVat;
			if (building_GrowthVat == null)
			{
				return false;
			}
			if (cancel)
			{
				building_GrowthVat.selectedEmbryo = null;
				this.implantTarget = null;
				return false;
			}
			return building_GrowthVat.selectedEmbryo == this;
		}

		// Token: 0x0600B121 RID: 45345 RVA: 0x003E6974 File Offset: 0x003E4B74
		private Building_GrowthVat BestAvailableGrowthVat()
		{
			if (base.MapHeld == null)
			{
				return null;
			}
			List<Building> list = base.MapHeld.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.GrowthVat);
			this.tmpEligibleVats.Clear();
			foreach (Building building in list)
			{
				Building_GrowthVat building_GrowthVat = building as Building_GrowthVat;
				if (building_GrowthVat != null && !building_GrowthVat.Working && building_GrowthVat.selectedEmbryo == null && building_GrowthVat.SelectedPawn == null)
				{
					this.tmpEligibleVats.Add(building_GrowthVat);
				}
			}
			if (this.tmpEligibleVats.NullOrEmpty<Building_GrowthVat>())
			{
				return null;
			}
			this.tmpEligibleVats.SortBy((Building_GrowthVat v) => (!v.PowerOn) ? 1 : 0, (Building_GrowthVat v) => (base.PositionHeld - v.Position).LengthHorizontal);
			return this.tmpEligibleVats[0];
		}

		// Token: 0x0600B122 RID: 45346 RVA: 0x003E6A64 File Offset: 0x003E4C64
		private FloatMenuOption CanImplantFloatOption(Pawn pawn, bool pawnAsLabel)
		{
			AcceptanceReport acceptanceReport = this.CanImplantReport(pawn);
			if (acceptanceReport.Accepted)
			{
				Action <>9__1;
				return new FloatMenuOption(pawnAsLabel ? pawn.LabelShort : "ImplantLabel".Translate().ToString(), delegate()
				{
					float num = PregnancyUtility.PregnancyChanceImplantEmbryo(pawn);
					if (num < 1f)
					{
						TaggedString text = "ImplantSurgeryConfirm".Translate(this.Label, pawn.LabelShort, num.ToStringPercent());
						Action confirmedAct;
						if ((confirmedAct = <>9__1) == null)
						{
							confirmedAct = (<>9__1 = delegate()
							{
								HealthCardUtility.CreateSurgeryBill(pawn, RecipeDefOf.ImplantEmbryo, null, new List<Thing>
								{
									this
								}, true);
								this.implantTarget = pawn;
							});
						}
						Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation(text, confirmedAct, true, null, WindowLayer.Dialog);
						Find.WindowStack.Add(window);
						return;
					}
					HealthCardUtility.CreateSurgeryBill(pawn, RecipeDefOf.ImplantEmbryo, null, new List<Thing>
					{
						this
					}, true);
					this.implantTarget = pawn;
				}, pawn, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!acceptanceReport.Reason.NullOrEmpty())
			{
				return new FloatMenuOption(pawnAsLabel ? "DisabledOption".Translate(pawn.LabelShort, acceptanceReport.Reason).ToString() : ("ImplantCannotFloatOption".Translate().ToString() + ": " + acceptanceReport.Reason), null, pawn, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return null;
		}

		// Token: 0x0600B123 RID: 45347 RVA: 0x003E6B80 File Offset: 0x003E4D80
		private AcceptanceReport CanImplantReport(Pawn pawn)
		{
			if (pawn.gender != Gender.Female)
			{
				return false;
			}
			if (pawn.IsQuestLodger())
			{
				return false;
			}
			HashSet<Pawn> hashSet = new HashSet<Pawn>();
			base.Map.reservationManager.ReserversOf(pawn, hashSet);
			if (hashSet.Any<Pawn>())
			{
				Pawn pawn2 = hashSet.First<Pawn>();
				return "ReservedBy".Translate(pawn2.LabelShort, pawn2);
			}
			if (pawn.BillStack.Bills.Any((Bill b) => b.recipe == RecipeDefOf.ImplantEmbryo))
			{
				return "CannotImplantingOtherEmbryo".Translate();
			}
			if (pawn.ageTracker.AgeBiologicalYears < 16)
			{
				return "CannotMustBeAge".Translate(16).CapitalizeFirst();
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.PregnantHuman, false))
			{
				return "CannotPregnant".Translate();
			}
			if (pawn.Sterile())
			{
				return "CannotSterile".Translate();
			}
			return true;
		}

		// Token: 0x0600B124 RID: 45348 RVA: 0x003E6CB0 File Offset: 0x003E4EB0
		public bool TryPopulateGenes()
		{
			bool result = true;
			if (this.geneSet == null)
			{
				this.geneSet = PregnancyUtility.GetInheritedGeneSet(this.Father, this.Mother, out result);
			}
			return result;
		}

		// Token: 0x0600B125 RID: 45349 RVA: 0x003E6CE4 File Offset: 0x003E4EE4
		public override void Notify_DebugSpawned()
		{
			CompHasPawnSources compHasPawnSources = this.TryGetComp<CompHasPawnSources>();
			Pawn pawn;
			if ((from x in base.Map.mapPawns.AllPawns
			where x.RaceProps.Humanlike && x.gender == Gender.Male
			select x).TryRandomElement(out pawn))
			{
				compHasPawnSources.AddSource(pawn);
			}
			Pawn pawn2;
			if ((from x in base.Map.mapPawns.AllPawns
			where x.RaceProps.Humanlike && x.gender == Gender.Female
			select x).TryRandomElement(out pawn2))
			{
				compHasPawnSources.AddSource(pawn2);
			}
			this.geneSet = PregnancyUtility.GetInheritedGeneSet(this.Father, this.Mother);
		}

		// Token: 0x0600B126 RID: 45350 RVA: 0x003E6D97 File Offset: 0x003E4F97
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.implantTarget, "implantTarget", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.geneSet == null)
			{
				this.TryPopulateGenes();
			}
		}

		// Token: 0x0400665A RID: 26202
		private const int MinImplantationAge = 16;

		// Token: 0x0400665B RID: 26203
		public static readonly CachedTexture ImplantIcon = new CachedTexture("UI/Gizmos/ImplantEmbryo");

		// Token: 0x0400665C RID: 26204
		private static readonly Texture2D CancelImplantIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x0400665D RID: 26205
		public Thing implantTarget;

		// Token: 0x0400665E RID: 26206
		private List<Building_GrowthVat> tmpEligibleVats = new List<Building_GrowthVat>();
	}
}
