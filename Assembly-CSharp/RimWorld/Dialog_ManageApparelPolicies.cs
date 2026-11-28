using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CE0 RID: 7392
	public class Dialog_ManageApparelPolicies : Dialog_ManagePolicies<ApparelPolicy>
	{
		// Token: 0x17002293 RID: 8851
		// (get) Token: 0x0600BDF8 RID: 48632 RVA: 0x0044480F File Offset: 0x00442A0F
		private static ThingFilter ApparelGlobalFilter
		{
			get
			{
				if (Dialog_ManageApparelPolicies.apparelGlobalFilter == null)
				{
					Dialog_ManageApparelPolicies.apparelGlobalFilter = new ThingFilter();
					Dialog_ManageApparelPolicies.apparelGlobalFilter.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
				}
				return Dialog_ManageApparelPolicies.apparelGlobalFilter;
			}
		}

		// Token: 0x17002294 RID: 8852
		// (get) Token: 0x0600BDF9 RID: 48633 RVA: 0x00444839 File Offset: 0x00442A39
		protected override string TitleKey
		{
			get
			{
				return "ApparelPolicyTitle";
			}
		}

		// Token: 0x17002295 RID: 8853
		// (get) Token: 0x0600BDFA RID: 48634 RVA: 0x00444840 File Offset: 0x00442A40
		protected override string TipKey
		{
			get
			{
				return "ApparelPolicyTip";
			}
		}

		// Token: 0x17002296 RID: 8854
		// (get) Token: 0x0600BDFB RID: 48635 RVA: 0x00444847 File Offset: 0x00442A47
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 700f);
			}
		}

		// Token: 0x0600BDFC RID: 48636 RVA: 0x00444858 File Offset: 0x00442A58
		public Dialog_ManageApparelPolicies(ApparelPolicy policy) : base(policy)
		{
		}

		// Token: 0x0600BDFD RID: 48637 RVA: 0x0044486C File Offset: 0x00442A6C
		public override void PreOpen()
		{
			base.PreOpen();
			this.thingFilterState.quickSearch.Reset();
		}

		// Token: 0x0600BDFE RID: 48638 RVA: 0x00444884 File Offset: 0x00442A84
		protected override ApparelPolicy CreateNewPolicy()
		{
			return Current.Game.outfitDatabase.MakeNewOutfit();
		}

		// Token: 0x0600BDFF RID: 48639 RVA: 0x00444895 File Offset: 0x00442A95
		protected override ApparelPolicy GetDefaultPolicy()
		{
			return Current.Game.outfitDatabase.DefaultOutfit();
		}

		// Token: 0x0600BE00 RID: 48640 RVA: 0x004448A6 File Offset: 0x00442AA6
		protected override void SetDefaultPolicy(ApparelPolicy policy)
		{
			Current.Game.outfitDatabase.SetDefault(policy);
		}

		// Token: 0x0600BE01 RID: 48641 RVA: 0x004448B8 File Offset: 0x00442AB8
		protected override AcceptanceReport TryDeletePolicy(ApparelPolicy policy)
		{
			return Current.Game.outfitDatabase.TryDelete(policy);
		}

		// Token: 0x0600BE02 RID: 48642 RVA: 0x004448CA File Offset: 0x00442ACA
		protected override List<ApparelPolicy> GetPolicies()
		{
			return Current.Game.outfitDatabase.AllOutfits;
		}

		// Token: 0x0600BE03 RID: 48643 RVA: 0x004448DC File Offset: 0x00442ADC
		protected override void DoContentsRect(Rect rect)
		{
			ThingFilterUI.DoThingFilterConfigWindow(rect, this.thingFilterState, base.SelectedPolicy.filter, Dialog_ManageApparelPolicies.ApparelGlobalFilter, 16, null, this.HiddenSpecialThingFilters(), false, false, false, null, null);
		}

		// Token: 0x0600BE04 RID: 48644 RVA: 0x00444913 File Offset: 0x00442B13
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			return new Dialog_ManageApparelPolicies.<HiddenSpecialThingFilters>d__18(-2);
		}

		// Token: 0x04006CE2 RID: 27874
		private readonly ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x04006CE3 RID: 27875
		private static ThingFilter apparelGlobalFilter;
	}
}
