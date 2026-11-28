using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D7D RID: 7549
	public abstract class ITab_PenBase : ITab
	{
		// Token: 0x17002443 RID: 9283
		// (get) Token: 0x0600C3A5 RID: 50085 RVA: 0x004790C3 File Offset: 0x004772C3
		public CompAnimalPenMarker SelectedCompAnimalPenMarker
		{
			get
			{
				Thing selThing = base.SelThing;
				if (selThing == null)
				{
					return null;
				}
				return selThing.TryGetComp<CompAnimalPenMarker>();
			}
		}

		// Token: 0x17002444 RID: 9284
		// (get) Token: 0x0600C3A6 RID: 50086 RVA: 0x004790D6 File Offset: 0x004772D6
		public override bool IsVisible
		{
			get
			{
				return this.SelectedCompAnimalPenMarker != null;
			}
		}
	}
}
