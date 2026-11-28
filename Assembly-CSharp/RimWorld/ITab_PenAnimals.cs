using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D7E RID: 7550
	public class ITab_PenAnimals : ITab_PenBase
	{
		// Token: 0x0600C3A8 RID: 50088 RVA: 0x004790E9 File Offset: 0x004772E9
		public ITab_PenAnimals()
		{
			this.size = ITab_PenAnimals.WinSize;
			this.labelKey = "TabPenAnimals";
		}

		// Token: 0x0600C3A9 RID: 50089 RVA: 0x00479112 File Offset: 0x00477312
		public override void OnOpen()
		{
			base.OnOpen();
			this.animalFilterState.quickSearch.Reset();
		}

		// Token: 0x0600C3AA RID: 50090 RVA: 0x0047912C File Offset: 0x0047732C
		protected override void FillTab()
		{
			CompAnimalPenMarker selectedCompAnimalPenMarker = base.SelectedCompAnimalPenMarker;
			Map map = selectedCompAnimalPenMarker.parent.Map;
			Rect rect;
			Rect rect2;
			new Rect(0f, 0f, ITab_PenAnimals.WinSize.x, ITab_PenAnimals.WinSize.y).ContractedBy(10f).SplitHorizontally(18f, out rect, out rect2);
			ThingFilterUI.DoThingFilterConfigWindow(rect2, this.animalFilterState, selectedCompAnimalPenMarker.AnimalFilter, AnimalPenUtility.GetFixedAnimalFilter(), 1, null, null, false, false, false, null, map);
		}

		// Token: 0x04006FD4 RID: 28628
		private static readonly Vector2 WinSize = new Vector2(300f, 480f);

		// Token: 0x04006FD5 RID: 28629
		private ThingFilterUI.UIState animalFilterState = new ThingFilterUI.UIState();
	}
}
