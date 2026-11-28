using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CE2 RID: 7394
	public class Dialog_ManageFoodPolicies : Dialog_ManagePolicies<FoodPolicy>
	{
		// Token: 0x1700229B RID: 8859
		// (get) Token: 0x0600BE16 RID: 48662 RVA: 0x00445154 File Offset: 0x00443354
		public static ThingFilter FoodGlobalFilter
		{
			get
			{
				if (Dialog_ManageFoodPolicies.foodGlobalFilter == null)
				{
					Dialog_ManageFoodPolicies.foodGlobalFilter = new ThingFilter();
					foreach (ThingDef thingDef in from x in DefDatabase<ThingDef>.AllDefs
					where x.GetStatValueAbstract(StatDefOf.Nutrition, null) > 0f
					select x)
					{
						Dialog_ManageFoodPolicies.foodGlobalFilter.SetAllow(thingDef, true);
					}
				}
				return Dialog_ManageFoodPolicies.foodGlobalFilter;
			}
		}

		// Token: 0x1700229C RID: 8860
		// (get) Token: 0x0600BE17 RID: 48663 RVA: 0x004451E0 File Offset: 0x004433E0
		protected override string TitleKey
		{
			get
			{
				return "FoodPolicyTitle";
			}
		}

		// Token: 0x1700229D RID: 8861
		// (get) Token: 0x0600BE18 RID: 48664 RVA: 0x004451E7 File Offset: 0x004433E7
		protected override string TipKey
		{
			get
			{
				return "FoodPolicyTip";
			}
		}

		// Token: 0x1700229E RID: 8862
		// (get) Token: 0x0600BE19 RID: 48665 RVA: 0x004451EE File Offset: 0x004433EE
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 700f);
			}
		}

		// Token: 0x0600BE1A RID: 48666 RVA: 0x004451FF File Offset: 0x004433FF
		public Dialog_ManageFoodPolicies(FoodPolicy policy) : base(policy)
		{
		}

		// Token: 0x0600BE1B RID: 48667 RVA: 0x00445213 File Offset: 0x00443413
		public override void PreOpen()
		{
			base.PreOpen();
			this.thingFilterState.quickSearch.Reset();
		}

		// Token: 0x0600BE1C RID: 48668 RVA: 0x0044522B File Offset: 0x0044342B
		protected override FoodPolicy CreateNewPolicy()
		{
			return Current.Game.foodRestrictionDatabase.MakeNewFoodRestriction();
		}

		// Token: 0x0600BE1D RID: 48669 RVA: 0x0044523C File Offset: 0x0044343C
		protected override FoodPolicy GetDefaultPolicy()
		{
			return Current.Game.foodRestrictionDatabase.DefaultFoodRestriction();
		}

		// Token: 0x0600BE1E RID: 48670 RVA: 0x0044524D File Offset: 0x0044344D
		protected override void SetDefaultPolicy(FoodPolicy policy)
		{
			Current.Game.foodRestrictionDatabase.SetDefault(policy);
		}

		// Token: 0x0600BE1F RID: 48671 RVA: 0x0044525F File Offset: 0x0044345F
		protected override AcceptanceReport TryDeletePolicy(FoodPolicy policy)
		{
			return Current.Game.foodRestrictionDatabase.TryDelete(policy);
		}

		// Token: 0x0600BE20 RID: 48672 RVA: 0x00445271 File Offset: 0x00443471
		protected override List<FoodPolicy> GetPolicies()
		{
			return Current.Game.foodRestrictionDatabase.AllFoodRestrictions;
		}

		// Token: 0x0600BE21 RID: 48673 RVA: 0x00445284 File Offset: 0x00443484
		protected override void DoContentsRect(Rect rect)
		{
			ThingFilterUI.DoThingFilterConfigWindow(rect, this.thingFilterState, base.SelectedPolicy.filter, Dialog_ManageFoodPolicies.FoodGlobalFilter, 1, null, this.HiddenSpecialThingFilters(), true, false, false, null, null);
		}

		// Token: 0x0600BE22 RID: 48674 RVA: 0x004452BA File Offset: 0x004434BA
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			return new Dialog_ManageFoodPolicies.<HiddenSpecialThingFilters>d__18(-2);
		}

		// Token: 0x04006CEE RID: 27886
		private readonly ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x04006CEF RID: 27887
		private static ThingFilter foodGlobalFilter;
	}
}
