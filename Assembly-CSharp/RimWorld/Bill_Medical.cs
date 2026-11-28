using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001497 RID: 5271
	public class Bill_Medical : Bill
	{
		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x0600812E RID: 33070 RVA: 0x002E1D0E File Offset: 0x002DFF0E
		public override bool CheckIngredientsIfSociallyProper
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001625 RID: 5669
		// (get) Token: 0x0600812F RID: 33071 RVA: 0x002E1D11 File Offset: 0x002DFF11
		protected override bool CanCopy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001626 RID: 5670
		// (get) Token: 0x06008130 RID: 33072 RVA: 0x002E1D14 File Offset: 0x002DFF14
		public override bool CompletableEver
		{
			get
			{
				if (this.recipe.targetsBodyPart && !this.recipe.Worker.GetPartsToApplyOn(this.GiverPawn, this.recipe).Contains(this.part))
				{
					return false;
				}
				Recipe_Surgery recipe_Surgery = this.recipe.Worker as Recipe_Surgery;
				if (recipe_Surgery != null && !recipe_Surgery.CompletableEver(this.GiverPawn))
				{
					return false;
				}
				if (!this.uniqueRequiredIngredients.NullOrEmpty<Thing>() && !this.iterationCompleted)
				{
					using (List<Thing>.Enumerator enumerator = this.uniqueRequiredIngredients.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.DestroyedOrNull())
							{
								return false;
							}
						}
					}
					return true;
				}
				return true;
			}
		}

		// Token: 0x17001627 RID: 5671
		// (get) Token: 0x06008131 RID: 33073 RVA: 0x002E1DE0 File Offset: 0x002DFFE0
		// (set) Token: 0x06008132 RID: 33074 RVA: 0x002E1DE8 File Offset: 0x002DFFE8
		public BodyPartRecord Part
		{
			get
			{
				return this.part;
			}
			set
			{
				if (this.billStack == null && this.part != null)
				{
					Log.Error("Can only set Bill_Medical.Part after the bill has been added to a pawn's bill stack.");
					return;
				}
				this.part = value;
			}
		}

		// Token: 0x17001628 RID: 5672
		// (get) Token: 0x06008133 RID: 33075 RVA: 0x002E1E0C File Offset: 0x002E000C
		public Pawn GiverPawn
		{
			get
			{
				Pawn pawn = this.billStack.billGiver as Pawn;
				Corpse corpse = this.billStack.billGiver as Corpse;
				if (corpse != null)
				{
					pawn = corpse.InnerPawn;
				}
				if (pawn == null)
				{
					throw new InvalidOperationException("Medical bill on non-pawn.");
				}
				return pawn;
			}
		}

		// Token: 0x17001629 RID: 5673
		// (get) Token: 0x06008134 RID: 33076 RVA: 0x002E1E54 File Offset: 0x002E0054
		public override string Label
		{
			get
			{
				string text = this.recipe.Worker.LabelFromUniqueIngredients(this);
				if (!text.NullOrEmpty())
				{
					return text;
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.recipe.Worker.GetLabelWhenUsedOn(this.GiverPawn, this.part));
				if (this.Part != null && !this.recipe.hideBodyPartNames)
				{
					stringBuilder.Append(" (" + this.Part.Label + ")");
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06008135 RID: 33077 RVA: 0x002E1EE2 File Offset: 0x002E00E2
		public Bill_Medical()
		{
		}

		// Token: 0x06008136 RID: 33078 RVA: 0x002E1EF5 File Offset: 0x002E00F5
		public Bill_Medical(RecipeDef recipe, List<Thing> uniqueIngredients) : base(recipe, null)
		{
			this.uniqueRequiredIngredients = uniqueIngredients;
		}

		// Token: 0x06008137 RID: 33079 RVA: 0x002E1F11 File Offset: 0x002E0111
		public override bool ShouldDoNow()
		{
			return !this.suspended;
		}

		// Token: 0x06008138 RID: 33080 RVA: 0x002E1F20 File Offset: 0x002E0120
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			base.Notify_IterationCompleted(billDoer, ingredients);
			this.iterationCompleted = true;
			if (this.CompletableEver)
			{
				Pawn giverPawn = this.GiverPawn;
				this.recipe.Worker.ApplyOnPawn(giverPawn, this.Part, billDoer, ingredients, this);
				if (giverPawn.RaceProps.IsFlesh)
				{
					giverPawn.records.Increment(RecordDefOf.OperationsReceived);
					billDoer.records.Increment(RecordDefOf.OperationsPerformed);
					if (ModsConfig.AnomalyActive && Rand.Chance(0.5f) && MetalhorrorUtility.IsInfected(billDoer))
					{
						MetalhorrorUtility.Infect(giverPawn, billDoer, "SurgeryImplant", null);
					}
				}
			}
			this.billStack.Delete(this);
		}

		// Token: 0x06008139 RID: 33081 RVA: 0x002E1FC8 File Offset: 0x002E01C8
		public override void Notify_BillWorkStarted(Pawn billDoer)
		{
			base.Notify_BillWorkStarted(billDoer);
			this.consumedMedicine.Clear();
			if (!this.GiverPawn.Dead && this.recipe.anesthetize && HealthUtility.TryAnesthetize(this.GiverPawn))
			{
				List<ThingCountClass> placedThings = billDoer.CurJob.placedThings;
				if (placedThings != null)
				{
					int i = 0;
					while (i < placedThings.Count)
					{
						if (placedThings[i].thing is Medicine)
						{
							this.recipe.Worker.ConsumeIngredient(placedThings[i].thing.SplitOff(1), this.recipe, billDoer.MapHeld);
							ThingCountClass thingCountClass = placedThings[i];
							int count = thingCountClass.Count;
							thingCountClass.Count = count - 1;
							this.consumedMedicine.Add(placedThings[i].thing.def, 1);
							if (placedThings[i].thing.Destroyed || placedThings[i].Count <= 0)
							{
								placedThings.RemoveAt(i);
								return;
							}
							break;
						}
						else
						{
							i++;
						}
					}
				}
			}
		}

		// Token: 0x0600813A RID: 33082 RVA: 0x002E20E4 File Offset: 0x002E02E4
		public override bool PawnAllowedToStartAnew(Pawn pawn)
		{
			if (!base.PawnAllowedToStartAnew(pawn))
			{
				return false;
			}
			if (this.recipe.Worker is Recipe_AdministerIngestible)
			{
				ThingDef singleDef = this.recipe.ingredients[0].filter.BestThingRequest.singleDef;
				if (singleDef.IsDrug && !new HistoryEvent(HistoryEventDefOf.AdministeredDrug, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
				{
					return false;
				}
				if (singleDef.IsDrug && singleDef.ingestible.drugCategory == DrugCategory.Hard && !new HistoryEvent(HistoryEventDefOf.AdministeredHardDrug, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
				{
					return false;
				}
				if (singleDef.IsNonMedicalDrug && !new HistoryEvent(HistoryEventDefOf.AdministeredRecreationalDrug, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600813B RID: 33083 RVA: 0x002E21B4 File Offset: 0x002E03B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_BodyParts.Look(ref this.part, "part", null);
			Scribe_Collections.Look<ThingDef, int>(ref this.consumedMedicine, "consumedMedicine", LookMode.Undefined, LookMode.Undefined);
			Scribe_Collections.Look<Thing>(ref this.uniqueRequiredIngredients, "uniqueRequiredIngredients", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.iterationCompleted, "iterationCompleted", false, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				ThingDef thingDef = null;
				Scribe_Defs.Look<ThingDef>(ref thingDef, "consumedInitialMedicineDef");
				if (thingDef != null)
				{
					if (this.consumedMedicine == null)
					{
						this.consumedMedicine = new Dictionary<ThingDef, int>();
					}
					this.consumedMedicine.Add(thingDef, 1);
				}
			}
			else if (Scribe.mode == LoadSaveMode.PostLoadInit && this.consumedMedicine == null)
			{
				this.consumedMedicine = new Dictionary<ThingDef, int>();
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600813C RID: 33084 RVA: 0x002E226E File Offset: 0x002E046E
		public override Bill Clone()
		{
			Bill_Medical bill_Medical = (Bill_Medical)base.Clone();
			bill_Medical.part = this.part;
			bill_Medical.consumedMedicine = new Dictionary<ThingDef, int>(this.consumedMedicine);
			return bill_Medical;
		}

		// Token: 0x0400505E RID: 20574
		private BodyPartRecord part;

		// Token: 0x0400505F RID: 20575
		public Dictionary<ThingDef, int> consumedMedicine = new Dictionary<ThingDef, int>();

		// Token: 0x04005060 RID: 20576
		public List<Thing> uniqueRequiredIngredients;

		// Token: 0x04005061 RID: 20577
		public int temp_partIndexToSetLater;

		// Token: 0x04005062 RID: 20578
		public bool iterationCompleted;

		// Token: 0x04005063 RID: 20579
		private const float MetalhorrorInfectionChance = 0.5f;
	}
}
