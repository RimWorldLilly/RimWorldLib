using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D5C RID: 7516
	public interface IInspectPane
	{
		// Token: 0x170023FD RID: 9213
		// (get) Token: 0x0600C29D RID: 49821
		// (set) Token: 0x0600C29E RID: 49822
		float RecentHeight { get; set; }

		// Token: 0x170023FE RID: 9214
		// (get) Token: 0x0600C29F RID: 49823
		// (set) Token: 0x0600C2A0 RID: 49824
		Type OpenTabType { get; set; }

		// Token: 0x170023FF RID: 9215
		// (get) Token: 0x0600C2A1 RID: 49825
		bool AnythingSelected { get; }

		// Token: 0x17002400 RID: 9216
		// (get) Token: 0x0600C2A2 RID: 49826
		IEnumerable<InspectTabBase> CurTabs { get; }

		// Token: 0x17002401 RID: 9217
		// (get) Token: 0x0600C2A3 RID: 49827
		bool ShouldShowSelectNextInCellButton { get; }

		// Token: 0x17002402 RID: 9218
		// (get) Token: 0x0600C2A4 RID: 49828
		bool ShouldShowPaneContents { get; }

		// Token: 0x17002403 RID: 9219
		// (get) Token: 0x0600C2A5 RID: 49829
		float PaneTopY { get; }

		// Token: 0x0600C2A6 RID: 49830
		string GetLabel(Rect rect);

		// Token: 0x0600C2A7 RID: 49831
		void DoInspectPaneButtons(Rect rect, ref float lineEndWidth);

		// Token: 0x0600C2A8 RID: 49832
		void SelectNextInCell();

		// Token: 0x0600C2A9 RID: 49833
		void DoPaneContents(Rect rect);

		// Token: 0x0600C2AA RID: 49834
		void CloseOpenTab();

		// Token: 0x0600C2AB RID: 49835
		void Reset();
	}
}
