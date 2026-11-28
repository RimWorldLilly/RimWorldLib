using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AD6 RID: 6870
	public class Bill_Autonomous : Bill_Production
	{
		// Token: 0x17001F87 RID: 8071
		// (get) Token: 0x0600AF19 RID: 44825 RVA: 0x003DA84B File Offset: 0x003D8A4B
		public FormingState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17001F88 RID: 8072
		// (get) Token: 0x0600AF1A RID: 44826 RVA: 0x003DA853 File Offset: 0x003D8A53
		private Building_WorkTableAutonomous WorkTable
		{
			get
			{
				return (Building_WorkTableAutonomous)this.billStack.billGiver;
			}
		}

		// Token: 0x17001F89 RID: 8073
		// (get) Token: 0x0600AF1B RID: 44827 RVA: 0x003DA865 File Offset: 0x003D8A65
		public override bool CanFinishNow
		{
			get
			{
				return this.state == FormingState.Formed;
			}
		}

		// Token: 0x17001F8A RID: 8074
		// (get) Token: 0x0600AF1C RID: 44828 RVA: 0x003DA870 File Offset: 0x003D8A70
		private List<IngredientCount> CurrentBillIngredients
		{
			get
			{
				if (this.ingredients.Count == 0)
				{
					this.MakeIngredientsListInProcessingOrder(this.ingredients);
				}
				return this.ingredients;
			}
		}

		// Token: 0x0600AF1D RID: 44829 RVA: 0x003DA891 File Offset: 0x003D8A91
		public Bill_Autonomous()
		{
		}

		// Token: 0x0600AF1E RID: 44830 RVA: 0x003DA8A4 File Offset: 0x003D8AA4
		public Bill_Autonomous(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x0600AF1F RID: 44831 RVA: 0x003DA8B9 File Offset: 0x003D8AB9
		public virtual Thing CreateProducts()
		{
			return null;
		}

		// Token: 0x0600AF20 RID: 44832 RVA: 0x003DA8BC File Offset: 0x003D8ABC
		public override bool ShouldDoNow()
		{
			return base.ShouldDoNow() && this.state != FormingState.Forming;
		}

		// Token: 0x0600AF21 RID: 44833 RVA: 0x003DA8D4 File Offset: 0x003D8AD4
		public override bool PawnAllowedToStartAnew(Pawn p)
		{
			return base.PawnAllowedToStartAnew(p) && (this.WorkTable.ActiveBill == null || this.WorkTable.ActiveBill == this || (this.WorkTable.ActiveBill.suspended && this.WorkTable.ActiveBill.State == FormingState.Gathering && !this.WorkTable.innerContainer.Any<Thing>()));
		}

		// Token: 0x0600AF22 RID: 44834 RVA: 0x003DA940 File Offset: 0x003D8B40
		public override void Notify_DoBillStarted(Pawn billDoer)
		{
			base.Notify_DoBillStarted(billDoer);
			this.WorkTable.ActiveBill = this;
			this.startedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600AF23 RID: 44835 RVA: 0x003DA968 File Offset: 0x003D8B68
		public override void Notify_BillWorkFinished(Pawn billDoer)
		{
			base.Notify_BillWorkFinished(billDoer);
			switch (this.state)
			{
			case FormingState.Gathering:
				this.state = FormingState.Forming;
				this.formingTicks = (float)this.recipe.formingTicks;
				this.WorkTable.Notify_StartForming(billDoer);
				return;
			case FormingState.Preparing:
				this.formingTicks = (float)this.recipe.formingTicks;
				this.state = FormingState.Forming;
				break;
			case FormingState.Forming:
			case FormingState.Formed:
				break;
			default:
				return;
			}
		}

		// Token: 0x0600AF24 RID: 44836 RVA: 0x003DA9D9 File Offset: 0x003D8BD9
		public virtual void Reset()
		{
			this.ingredients.Clear();
			this.state = FormingState.Gathering;
		}

		// Token: 0x0600AF25 RID: 44837 RVA: 0x003DA9ED File Offset: 0x003D8BED
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			base.Notify_IterationCompleted(billDoer, ingredients);
			this.Reset();
			if (this.WorkTable.ActiveBill == this)
			{
				this.WorkTable.ActiveBill = null;
			}
		}

		// Token: 0x0600AF26 RID: 44838 RVA: 0x003DAA17 File Offset: 0x003D8C17
		public override float GetWorkAmount(Thing thing = null)
		{
			if (this.state == FormingState.Formed)
			{
				return 300f;
			}
			return base.GetWorkAmount(thing);
		}

		// Token: 0x0600AF27 RID: 44839 RVA: 0x003DAA30 File Offset: 0x003D8C30
		public virtual void BillTick()
		{
			if (!this.suspended && this.state == FormingState.Forming)
			{
				this.formingTicks -= 1f;
				if (this.formingTicks <= 0f)
				{
					this.state = FormingState.Formed;
					this.WorkTable.Notify_FormingCompleted();
				}
			}
		}

		// Token: 0x0600AF28 RID: 44840 RVA: 0x003DAA80 File Offset: 0x003D8C80
		public void AppendCurrentIngredientCount(StringBuilder sb)
		{
			foreach (IngredientCount ingredientCount in this.CurrentBillIngredients)
			{
				if (ingredientCount != null && ingredientCount.IsFixedIngredient)
				{
					TaggedString taggedString = ingredientCount.FixedIngredient.LabelCap;
					int value = this.WorkTable.innerContainer.TotalStackCountOfDef(ingredientCount.FixedIngredient);
					TaggedString t = taggedString;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(value);
					defaultInterpolatedStringHandler.AppendLiteral(" / ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(ingredientCount.CountRequiredOfFor(ingredientCount.FixedIngredient, this.recipe, this));
					taggedString = t + defaultInterpolatedStringHandler.ToStringAndClear();
					sb.AppendLine(taggedString);
				}
			}
		}

		// Token: 0x0600AF29 RID: 44841 RVA: 0x003DAB64 File Offset: 0x003D8D64
		public virtual void AppendInspectionData(StringBuilder sb)
		{
			if (this.State == FormingState.Forming || this.State == FormingState.Preparing)
			{
				sb.AppendTagged("FinishesIn".Translate() + ": " + ((int)this.formingTicks).ToStringTicksToPeriod(true, false, true, true, false));
			}
			if (this.State == FormingState.Formed)
			{
				sb.AppendLine("Finished".Translate());
			}
		}

		// Token: 0x0600AF2A RID: 44842 RVA: 0x003DABD4 File Offset: 0x003D8DD4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.formingTicks, "formingTicks", 0f, false);
			Scribe_Values.Look<FormingState>(ref this.state, "state", FormingState.Gathering, false);
			Scribe_Values.Look<int>(ref this.startedTick, "startedTick", 0, false);
		}

		// Token: 0x0400653A RID: 25914
		public const int CompleteBillTicks = 300;

		// Token: 0x0400653B RID: 25915
		public float formingTicks;

		// Token: 0x0400653C RID: 25916
		protected int startedTick;

		// Token: 0x0400653D RID: 25917
		protected FormingState state;

		// Token: 0x0400653E RID: 25918
		private List<IngredientCount> ingredients = new List<IngredientCount>();
	}
}
