using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB4 RID: 7604
	public class PawnColumnWorker_Faction : PawnColumnWorker_Icon
	{
		// Token: 0x0600C5AD RID: 50605 RVA: 0x00489360 File Offset: 0x00487560
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			Faction homeFaction = pawn.HomeFaction;
			if (homeFaction != null && homeFaction != Faction.OfPlayer)
			{
				return homeFaction.def.FactionIcon;
			}
			return null;
		}

		// Token: 0x0600C5AE RID: 50606 RVA: 0x0048938C File Offset: 0x0048758C
		protected override Color GetIconColor(Pawn pawn)
		{
			Faction homeFaction = pawn.HomeFaction;
			if (homeFaction != null && homeFaction != Faction.OfPlayer)
			{
				return homeFaction.Color;
			}
			return Color.white;
		}

		// Token: 0x0600C5AF RID: 50607 RVA: 0x004893B8 File Offset: 0x004875B8
		protected override string GetIconTip(Pawn pawn)
		{
			Faction homeFaction = pawn.HomeFaction;
			string text = (homeFaction != null) ? homeFaction.Name : null;
			if (!text.NullOrEmpty())
			{
				return "PawnFactionInfo".Translate(text, pawn);
			}
			return null;
		}

		// Token: 0x0600C5B0 RID: 50608 RVA: 0x00489400 File Offset: 0x00487600
		protected override void ClickedIcon(Pawn pawn)
		{
			Faction homeFaction = pawn.HomeFaction;
			if (homeFaction != null)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Factions, true);
				((MainTabWindow_Factions)Find.MainTabsRoot.OpenTab.TabWindow).ScrollToFaction(homeFaction);
			}
		}
	}
}
