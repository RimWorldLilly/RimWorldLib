using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020005A7 RID: 1447
	public class UnfinishedThing : ThingWithComps
	{
		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06002BBD RID: 11197 RVA: 0x0010987A File Offset: 0x00107A7A
		// (set) Token: 0x06002BBE RID: 11198 RVA: 0x00109882 File Offset: 0x00107A82
		public Pawn Creator
		{
			get
			{
				return this.creatorInt;
			}
			set
			{
				if (value == null)
				{
					Log.Error("Cannot set creator to null.");
					return;
				}
				this.creatorInt = value;
				this.creatorName = value.LabelShort;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06002BBF RID: 11199 RVA: 0x001098A5 File Offset: 0x00107AA5
		public RecipeDef Recipe
		{
			get
			{
				return this.recipeInt;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06002BC0 RID: 11200 RVA: 0x001098AD File Offset: 0x00107AAD
		// (set) Token: 0x06002BC1 RID: 11201 RVA: 0x001098E0 File Offset: 0x00107AE0
		public Bill_ProductionWithUft BoundBill
		{
			get
			{
				if (this.boundBillInt != null && (this.boundBillInt.DeletedOrDereferenced || this.boundBillInt.BoundUft != this))
				{
					this.boundBillInt = null;
				}
				return this.boundBillInt;
			}
			set
			{
				if (value == this.boundBillInt)
				{
					return;
				}
				Bill_ProductionWithUft bill_ProductionWithUft = this.boundBillInt;
				this.boundBillInt = value;
				if (bill_ProductionWithUft != null && bill_ProductionWithUft.BoundUft == this)
				{
					bill_ProductionWithUft.SetBoundUft(null, false);
				}
				if (value != null)
				{
					this.recipeInt = value.recipe;
					if (value.BoundUft != this)
					{
						value.SetBoundUft(this, false);
					}
				}
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06002BC2 RID: 11202 RVA: 0x0010993C File Offset: 0x00107B3C
		public Thing BoundWorkTable
		{
			get
			{
				if (this.BoundBill == null)
				{
					return null;
				}
				Thing thing = this.BoundBill.billStack.billGiver as Thing;
				if (thing.Destroyed)
				{
					return null;
				}
				return thing;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06002BC3 RID: 11203 RVA: 0x00109974 File Offset: 0x00107B74
		public override string LabelNoCount
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				if (base.Stuff == null)
				{
					return "UnfinishedItem".Translate(this.Recipe.products[0].thingDef.label);
				}
				return "UnfinishedItemWithStuff".Translate(base.Stuff.LabelAsStuff, this.Recipe.products[0].thingDef.label);
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06002BC4 RID: 11204 RVA: 0x00109A07 File Offset: 0x00107C07
		public override string DescriptionDetailed
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				return this.Recipe.ProducedThingDef.DescriptionDetailed;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06002BC5 RID: 11205 RVA: 0x00109A28 File Offset: 0x00107C28
		public override string DescriptionFlavor
		{
			get
			{
				if (this.Recipe == null)
				{
					return base.LabelNoCount;
				}
				return this.Recipe.ProducedThingDef.description;
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06002BC6 RID: 11206 RVA: 0x00109A49 File Offset: 0x00107C49
		public bool Initialized
		{
			get
			{
				return this.workLeft > -5000f;
			}
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x00109A58 File Offset: 0x00107C58
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && this.boundBillInt != null && this.boundBillInt.DeletedOrDereferenced)
			{
				this.boundBillInt = null;
			}
			Scribe_References.Look<Pawn>(ref this.creatorInt, "creator", false);
			Scribe_Values.Look<string>(ref this.creatorName, "creatorName", null, false);
			Scribe_References.Look<Bill_ProductionWithUft>(ref this.boundBillInt, "bill", false);
			Scribe_Defs.Look<RecipeDef>(ref this.recipeInt, "recipe");
			Scribe_Values.Look<float>(ref this.workLeft, "workLeft", 0f, false);
			Scribe_Collections.Look<Thing>(ref this.ingredients, "ingredients", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x00109B00 File Offset: 0x00107D00
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode == DestroyMode.Cancel)
			{
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					int num = GenMath.RoundRandom((float)this.ingredients[i].stackCount * 0.75f);
					if (num > 0)
					{
						this.ingredients[i].stackCount = num;
						GenPlace.TryPlaceThing(this.ingredients[i], base.Position, base.Map, ThingPlaceMode.Near, null, null, null, 1);
					}
				}
				this.ingredients.Clear();
			}
			base.Destroy(mode);
			this.BoundBill = null;
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x00109BA2 File Offset: 0x00107DA2
		public override IEnumerable<Gizmo> GetGizmos()
		{
			UnfinishedThing.<GetGizmos>d__28 <GetGizmos>d__ = new UnfinishedThing.<GetGizmos>d__28(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x00109BB4 File Offset: 0x00107DB4
		public Bill_ProductionWithUft BillOnTableForMe(Thing workTable)
		{
			if (this.Recipe.AllRecipeUsers.Contains(workTable.def))
			{
				IBillGiver billGiver = (IBillGiver)workTable;
				for (int i = 0; i < billGiver.BillStack.Count; i++)
				{
					Bill_ProductionWithUft bill_ProductionWithUft = billGiver.BillStack[i] as Bill_ProductionWithUft;
					if (bill_ProductionWithUft != null && bill_ProductionWithUft.ShouldDoNow() && bill_ProductionWithUft != null && bill_ProductionWithUft.recipe == this.Recipe)
					{
						return bill_ProductionWithUft;
					}
				}
			}
			return null;
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x00109C27 File Offset: 0x00107E27
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.BoundWorkTable != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.BoundWorkTable.TrueCenter());
			}
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x00109C50 File Offset: 0x00107E50
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "Author".Translate() + ": " + this.creatorName;
			text += "\n" + "WorkLeft".Translate() + ": " + this.workLeft.ToStringWorkAmount();
			Bill_ProductionWithUft boundBill = this.BoundBill;
			bool flag;
			if (boundBill == null)
			{
				flag = (null != null);
			}
			else
			{
				ThingStyleDef style = boundBill.style;
				flag = (((style != null) ? style.Category : null) != null);
			}
			if (flag && base.StyleSourcePrecept == null)
			{
				text += "\n" + "Style".Translate() + ": " + this.BoundBill.style.Category.LabelCap;
			}
			return text;
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x00109D4A File Offset: 0x00107F4A
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			UnfinishedThing.<SpecialDisplayStats>d__32 <SpecialDisplayStats>d__ = new UnfinishedThing.<SpecialDisplayStats>d__32(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x04001D8C RID: 7564
		private Pawn creatorInt;

		// Token: 0x04001D8D RID: 7565
		private string creatorName = "ErrorCreatorName";

		// Token: 0x04001D8E RID: 7566
		private RecipeDef recipeInt;

		// Token: 0x04001D8F RID: 7567
		public List<Thing> ingredients = new List<Thing>();

		// Token: 0x04001D90 RID: 7568
		private Bill_ProductionWithUft boundBillInt;

		// Token: 0x04001D91 RID: 7569
		public float workLeft = -10000f;

		// Token: 0x04001D92 RID: 7570
		public bool debugCompleted;

		// Token: 0x04001D93 RID: 7571
		private const float CancelIngredientRecoveryFraction = 0.75f;
	}
}
