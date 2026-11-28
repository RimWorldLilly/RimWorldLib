using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D7A RID: 7546
	public class ITab_Pawn_Needs : ITab
	{
		// Token: 0x1700243F RID: 9279
		// (get) Token: 0x0600C398 RID: 50072 RVA: 0x00478DC8 File Offset: 0x00476FC8
		public override bool IsVisible
		{
			get
			{
				if (this.SelPawn.RaceProps.Animal && this.SelPawn.Faction == null)
				{
					return false;
				}
				if (this.SelPawn.RaceProps.Insect && this.SelPawn.Faction != Faction.OfPlayer)
				{
					return false;
				}
				CompHoldingPlatformTarget compHoldingPlatformTarget = this.SelPawn.TryGetComp<CompHoldingPlatformTarget>();
				return (compHoldingPlatformTarget == null || !compHoldingPlatformTarget.CurrentlyHeldOnPlatform) && this.SelPawn.needs != null && this.SelPawn.needs.AllNeeds.Count > 0;
			}
		}

		// Token: 0x0600C399 RID: 50073 RVA: 0x00478E5C File Offset: 0x0047705C
		public ITab_Pawn_Needs()
		{
			this.labelKey = "TabNeeds";
			this.tutorTag = "Needs";
		}

		// Token: 0x0600C39A RID: 50074 RVA: 0x00478E7A File Offset: 0x0047707A
		public override void OnOpen()
		{
			this.thoughtScrollPosition = default(Vector2);
		}

		// Token: 0x0600C39B RID: 50075 RVA: 0x00478E88 File Offset: 0x00477088
		protected override void FillTab()
		{
			NeedsCardUtility.DoNeedsMoodAndThoughts(new Rect(0f, 0f, this.size.x, this.size.y), this.SelPawn, ref this.thoughtScrollPosition);
		}

		// Token: 0x0600C39C RID: 50076 RVA: 0x00478EC0 File Offset: 0x004770C0
		protected override void UpdateSize()
		{
			this.size = NeedsCardUtility.GetSize(this.SelPawn);
		}

		// Token: 0x04006FD2 RID: 28626
		private Vector2 thoughtScrollPosition;
	}
}
