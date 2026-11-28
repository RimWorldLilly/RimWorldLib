using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D7B RID: 7547
	public class ITab_Pawn_Social : ITab
	{
		// Token: 0x17002440 RID: 9280
		// (get) Token: 0x0600C39D RID: 50077 RVA: 0x00478ED3 File Offset: 0x004770D3
		public override bool IsVisible
		{
			get
			{
				return this.SelPawnForSocialInfo.RaceProps.IsFlesh && (!ModsConfig.AnomalyActive || !this.SelPawnForSocialInfo.RaceProps.IsAnomalyEntity);
			}
		}

		// Token: 0x17002441 RID: 9281
		// (get) Token: 0x0600C39E RID: 50078 RVA: 0x00478F08 File Offset: 0x00477108
		private Pawn SelPawnForSocialInfo
		{
			get
			{
				if (this.SelPawn != null)
				{
					return this.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				string str = "Social tab on non-pawn non-corpse ";
				Thing selThing = base.SelThing;
				throw new InvalidOperationException(str + ((selThing != null) ? selThing.ToString() : null));
			}
		}

		// Token: 0x0600C39F RID: 50079 RVA: 0x00478F5B File Offset: 0x0047715B
		public ITab_Pawn_Social()
		{
			this.size = new Vector2(540f, 510f);
			this.labelKey = "TabSocial";
			this.tutorTag = "Social";
		}

		// Token: 0x0600C3A0 RID: 50080 RVA: 0x00478F8E File Offset: 0x0047718E
		protected override void FillTab()
		{
			SocialCardUtility.DrawSocialCard(new Rect(0f, 0f, this.size.x, this.size.y), this.SelPawnForSocialInfo);
		}

		// Token: 0x04006FD3 RID: 28627
		public const float Width = 540f;
	}
}
