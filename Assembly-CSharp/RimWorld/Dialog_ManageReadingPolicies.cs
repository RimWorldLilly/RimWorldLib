using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CE4 RID: 7396
	public class Dialog_ManageReadingPolicies : Dialog_ManagePolicies<ReadingPolicy>
	{
		// Token: 0x170022A4 RID: 8868
		// (get) Token: 0x0600BE36 RID: 48694 RVA: 0x00445BF4 File Offset: 0x00443DF4
		private static ThingFilter PolicyGlobalFilter
		{
			get
			{
				if (Dialog_ManageReadingPolicies.policyGlobalFilter == null)
				{
					Dialog_ManageReadingPolicies.policyGlobalFilter = new ThingFilter();
					foreach (ThingDef thingDef in from x in DefDatabase<ThingDef>.AllDefs
					where x.HasComp<CompBook>()
					select x)
					{
						Dialog_ManageReadingPolicies.policyGlobalFilter.SetAllow(thingDef, true);
					}
				}
				return Dialog_ManageReadingPolicies.policyGlobalFilter;
			}
		}

		// Token: 0x170022A5 RID: 8869
		// (get) Token: 0x0600BE37 RID: 48695 RVA: 0x00445C80 File Offset: 0x00443E80
		protected override string TitleKey
		{
			get
			{
				return "ReadingPolicyTitle";
			}
		}

		// Token: 0x170022A6 RID: 8870
		// (get) Token: 0x0600BE38 RID: 48696 RVA: 0x00445C87 File Offset: 0x00443E87
		protected override string TipKey
		{
			get
			{
				return "ReadingPolicyTip";
			}
		}

		// Token: 0x170022A7 RID: 8871
		// (get) Token: 0x0600BE39 RID: 48697 RVA: 0x00445C8E File Offset: 0x00443E8E
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(860f, 700f);
			}
		}

		// Token: 0x0600BE3A RID: 48698 RVA: 0x00445C9F File Offset: 0x00443E9F
		public Dialog_ManageReadingPolicies(ReadingPolicy policy) : base(policy)
		{
		}

		// Token: 0x0600BE3B RID: 48699 RVA: 0x00445CBE File Offset: 0x00443EBE
		public override void PreOpen()
		{
			base.PreOpen();
			this.thingFilterState.quickSearch.Reset();
			this.effectFilterState.quickSearch.Reset();
		}

		// Token: 0x0600BE3C RID: 48700 RVA: 0x00445CE6 File Offset: 0x00443EE6
		protected override ReadingPolicy CreateNewPolicy()
		{
			return Current.Game.readingPolicyDatabase.MakeNewReadingPolicy();
		}

		// Token: 0x0600BE3D RID: 48701 RVA: 0x00445CF7 File Offset: 0x00443EF7
		protected override ReadingPolicy GetDefaultPolicy()
		{
			return Current.Game.readingPolicyDatabase.DefaultReadingPolicy();
		}

		// Token: 0x0600BE3E RID: 48702 RVA: 0x00445D08 File Offset: 0x00443F08
		protected override void SetDefaultPolicy(ReadingPolicy policy)
		{
			Current.Game.readingPolicyDatabase.SetDefault(policy);
		}

		// Token: 0x0600BE3F RID: 48703 RVA: 0x00445D1A File Offset: 0x00443F1A
		protected override AcceptanceReport TryDeletePolicy(ReadingPolicy policy)
		{
			return Current.Game.readingPolicyDatabase.TryDelete(policy);
		}

		// Token: 0x0600BE40 RID: 48704 RVA: 0x00445D2C File Offset: 0x00443F2C
		protected override List<ReadingPolicy> GetPolicies()
		{
			return Current.Game.readingPolicyDatabase.AllReadingPolicies;
		}

		// Token: 0x0600BE41 RID: 48705 RVA: 0x00445D40 File Offset: 0x00443F40
		protected override void DoContentsRect(Rect rect)
		{
			Rect rect2;
			Rect rect3;
			rect.SplitVerticallyWithMargin(out rect2, out rect3, 10f);
			ThingFilterUI.DoThingFilterConfigWindow(rect2, this.thingFilterState, base.SelectedPolicy.defFilter, Dialog_ManageReadingPolicies.PolicyGlobalFilter, 1, null, null, true, false, false, null, null);
			ThingFilterUI.DoThingFilterConfigWindow(rect3, this.effectFilterState, base.SelectedPolicy.effectFilter, null, 1, null, null, true, true, true, null, null);
		}

		// Token: 0x04006CF6 RID: 27894
		private readonly ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x04006CF7 RID: 27895
		private readonly ThingFilterUI.UIState effectFilterState = new ThingFilterUI.UIState();

		// Token: 0x04006CF8 RID: 27896
		private static ThingFilter policyGlobalFilter;
	}
}
