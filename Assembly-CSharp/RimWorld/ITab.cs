using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D5F RID: 7519
	public abstract class ITab : InspectTabBase
	{
		// Token: 0x17002404 RID: 9220
		// (get) Token: 0x0600C2CB RID: 49867 RVA: 0x00470344 File Offset: 0x0046E544
		protected object SelObject
		{
			get
			{
				return Find.Selector.SingleSelectedObject;
			}
		}

		// Token: 0x17002405 RID: 9221
		// (get) Token: 0x0600C2CC RID: 49868 RVA: 0x00470350 File Offset: 0x0046E550
		protected List<object> AllSelObjects
		{
			get
			{
				return Find.Selector.SelectedObjects;
			}
		}

		// Token: 0x17002406 RID: 9222
		// (get) Token: 0x0600C2CD RID: 49869 RVA: 0x0047035C File Offset: 0x0046E55C
		protected Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x17002407 RID: 9223
		// (get) Token: 0x0600C2CE RID: 49870 RVA: 0x00470368 File Offset: 0x0046E568
		protected virtual Pawn SelPawn
		{
			get
			{
				return this.SelThing as Pawn;
			}
		}

		// Token: 0x17002408 RID: 9224
		// (get) Token: 0x0600C2CF RID: 49871 RVA: 0x00470375 File Offset: 0x0046E575
		private MainTabWindow_Inspect InspectPane
		{
			get
			{
				return (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			}
		}

		// Token: 0x17002409 RID: 9225
		// (get) Token: 0x0600C2D0 RID: 49872 RVA: 0x00470388 File Offset: 0x0046E588
		protected override bool StillValid
		{
			get
			{
				if (Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
				{
					return false;
				}
				MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)Find.MainTabsRoot.OpenTab.TabWindow;
				return mainTabWindow_Inspect.CurTabs != null && mainTabWindow_Inspect.CurTabs.Contains(this);
			}
		}

		// Token: 0x1700240A RID: 9226
		// (get) Token: 0x0600C2D1 RID: 49873 RVA: 0x004703D3 File Offset: 0x0046E5D3
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x0600C2D2 RID: 49874 RVA: 0x004703E0 File Offset: 0x0046E5E0
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
			SoundDefOf.TabClose.PlayOneShotOnCamera(null);
		}
	}
}
