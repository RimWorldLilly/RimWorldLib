using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB8 RID: 7608
	public class PawnColumnWorker_Ideo : PawnColumnWorker_Icon
	{
		// Token: 0x170024B8 RID: 9400
		// (get) Token: 0x0600C5C7 RID: 50631 RVA: 0x00489692 File Offset: 0x00487892
		public override bool VisibleCurrently
		{
			get
			{
				return ModsConfig.IdeologyActive && !Find.IdeoManager.classicMode;
			}
		}

		// Token: 0x0600C5C8 RID: 50632 RVA: 0x004896AA File Offset: 0x004878AA
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			if (Find.IdeoManager.classicMode)
			{
				return null;
			}
			Ideo ideo = pawn.Ideo;
			if (ideo == null)
			{
				return null;
			}
			return ideo.Icon;
		}

		// Token: 0x0600C5C9 RID: 50633 RVA: 0x004896CB File Offset: 0x004878CB
		protected override Color GetIconColor(Pawn pawn)
		{
			if (pawn.Ideo == null)
			{
				return Color.white;
			}
			return pawn.Ideo.Color;
		}

		// Token: 0x0600C5CA RID: 50634 RVA: 0x004896E6 File Offset: 0x004878E6
		protected override string GetIconTip(Pawn pawn)
		{
			if (pawn.Ideo != null)
			{
				return pawn.Ideo.name + "\n\n" + "ClickForMoreInfo".Translate();
			}
			return null;
		}

		// Token: 0x0600C5CB RID: 50635 RVA: 0x0048971B File Offset: 0x0048791B
		protected override void ClickedIcon(Pawn pawn)
		{
			if (pawn.Ideo != null)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Ideos, true);
				IdeoUIUtility.OpenIdeoInfo(pawn.Ideo);
			}
		}
	}
}
