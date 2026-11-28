using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D60 RID: 7520
	public class ITab_Art : ITab
	{
		// Token: 0x1700240B RID: 9227
		// (get) Token: 0x0600C2D4 RID: 49876 RVA: 0x00470400 File Offset: 0x0046E600
		private CompArt SelectedCompArt
		{
			get
			{
				Thing thing = Find.Selector.SingleSelectedThing;
				MinifiedThing minifiedThing = thing as MinifiedThing;
				if (minifiedThing != null)
				{
					thing = minifiedThing.InnerThing;
				}
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompArt>();
			}
		}

		// Token: 0x1700240C RID: 9228
		// (get) Token: 0x0600C2D5 RID: 49877 RVA: 0x00470434 File Offset: 0x0046E634
		public override bool IsVisible
		{
			get
			{
				return this.SelectedCompArt != null && this.SelectedCompArt.Active;
			}
		}

		// Token: 0x0600C2D6 RID: 49878 RVA: 0x0047044B File Offset: 0x0046E64B
		public ITab_Art()
		{
			this.size = ITab_Art.WinSize;
			this.labelKey = "TabArt";
			this.tutorTag = "Art";
		}

		// Token: 0x0600C2D7 RID: 49879 RVA: 0x00470474 File Offset: 0x0046E674
		protected override void FillTab()
		{
			Rect rect2;
			Rect rect = rect2 = new Rect(0f, 0f, ITab_Art.WinSize.x, ITab_Art.WinSize.y).ContractedBy(10f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, this.SelectedCompArt.Title.Truncate(rect2.width, null));
			if (ITab_Art.cachedImageSource != this.SelectedCompArt || ITab_Art.cachedTaleRef != this.SelectedCompArt.TaleRef)
			{
				ITab_Art.cachedImageDescription = this.SelectedCompArt.GenerateImageDescription();
				ITab_Art.cachedImageSource = this.SelectedCompArt;
				ITab_Art.cachedTaleRef = this.SelectedCompArt.TaleRef;
			}
			Rect rect3 = rect;
			rect3.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect3, ITab_Art.cachedImageDescription);
		}

		// Token: 0x04006F5C RID: 28508
		private static string cachedImageDescription;

		// Token: 0x04006F5D RID: 28509
		private static CompArt cachedImageSource;

		// Token: 0x04006F5E RID: 28510
		private static TaleReference cachedTaleRef;

		// Token: 0x04006F5F RID: 28511
		private static readonly Vector2 WinSize = new Vector2(400f, 300f);
	}
}
