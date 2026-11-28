using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006AD RID: 1709
	public sealed class IngredientCount : IExposable
	{
		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x0600363D RID: 13885 RVA: 0x0015DB7B File Offset: 0x0015BD7B
		public bool IsFixedIngredient
		{
			get
			{
				return this.filter.AllowedDefCount == 1;
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x0600363E RID: 13886 RVA: 0x0015DB8B File Offset: 0x0015BD8B
		public ThingDef FixedIngredient
		{
			get
			{
				if (!this.IsFixedIngredient)
				{
					Log.Error("Called for SingleIngredient on an IngredientCount that is not IsSingleIngredient: " + ((this != null) ? this.ToString() : null));
				}
				return this.filter.AnyAllowedDef;
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x0600363F RID: 13887 RVA: 0x0015DBBC File Offset: 0x0015BDBC
		public string Summary
		{
			get
			{
				return this.count.ToString() + "x " + this.filter.Summary;
			}
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06003640 RID: 13888 RVA: 0x0015DBDE File Offset: 0x0015BDDE
		public string SummaryFilterFirst
		{
			get
			{
				return this.filter.Summary.CapitalizeFirst() + " x" + this.count.ToString();
			}
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x0015DC08 File Offset: 0x0015BE08
		public string SummaryFor(RecipeDef recipe)
		{
			return this.CountFor(recipe).ToString() + "x " + this.filter.Summary;
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x0015DC3C File Offset: 0x0015BE3C
		public float CountFor(RecipeDef recipe)
		{
			float num = this.GetBaseCount();
			ThingDef thingDef = this.filter.AllowedThingDefs.FirstOrDefault((ThingDef x) => recipe.fixedIngredientFilter.Allows(x) && !x.smallVolume) ?? this.filter.AllowedThingDefs.FirstOrDefault((ThingDef x) => recipe.fixedIngredientFilter.Allows(x));
			if (thingDef != null)
			{
				float num2 = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
				if (Math.Abs(num2) > 1E-45f)
				{
					num /= num2;
				}
			}
			return num;
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x0015DCC4 File Offset: 0x0015BEC4
		public int CountRequiredOfFor(ThingDef thingDef, RecipeDef recipe, Bill bill = null)
		{
			float num = recipe.IngredientValueGetter.ValuePerUnitOf(thingDef);
			return Mathf.CeilToInt(((bill == null) ? this.count : recipe.Worker.GetIngredientCount(this, bill)) / num);
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x0015DCFD File Offset: 0x0015BEFD
		public float GetBaseCount()
		{
			return this.count;
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x0015DD05 File Offset: 0x0015BF05
		public void SetBaseCount(float count)
		{
			this.count = count;
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x0015DD0E File Offset: 0x0015BF0E
		public void ResolveReferences()
		{
			this.filter.ResolveReferences();
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x0015DD1B File Offset: 0x0015BF1B
		public override string ToString()
		{
			return "(" + this.Summary + ")";
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x0015DD32 File Offset: 0x0015BF32
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.count, "count", 0f, false);
		}

		// Token: 0x04002331 RID: 9009
		public ThingFilter filter = new ThingFilter();

		// Token: 0x04002332 RID: 9010
		private float count = 1f;
	}
}
