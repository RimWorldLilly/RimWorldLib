using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001DC8 RID: 7624
	public class PawnColumnWorker_AllowedArea : PawnColumnWorker
	{
		// Token: 0x0600C617 RID: 50711 RVA: 0x0048A13C File Offset: 0x0048833C
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 200);
		}

		// Token: 0x0600C618 RID: 50712 RVA: 0x0048A14F File Offset: 0x0048834F
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(273, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600C619 RID: 50713 RVA: 0x0048A169 File Offset: 0x00488369
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x0600C61A RID: 50714 RVA: 0x0048A17C File Offset: 0x0048837C
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Faction == Faction.OfPlayer)
			{
				if (pawn.IsMutant && !pawn.mutant.Def.respectsAllowedArea)
				{
					return;
				}
				if (pawn.RaceProps.IsMechanoid && pawn.GetOverseer() == null)
				{
					return;
				}
				if (pawn.playerSettings.SupportsAllowedAreas)
				{
					AreaAllowedGUI.DoAllowedAreaSelectors(rect, pawn);
					return;
				}
				if (AnimalPenUtility.NeedsToBeManagedByRope(pawn))
				{
					AnimalPenGUI.DoAllowedAreaMessage(rect, pawn);
					return;
				}
				if (pawn.RaceProps.Dryad)
				{
					Text.Anchor = TextAnchor.MiddleCenter;
					Text.Font = GameFont.Tiny;
					GUI.color = Color.gray;
					Widgets.Label(rect, "CannotAssignAllowedAreaToDryad".Translate());
					GUI.color = Color.white;
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
		}

		// Token: 0x0600C61B RID: 50715 RVA: 0x0048A238 File Offset: 0x00488438
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			if (Widgets.ButtonText(new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f), "ManageAreas".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ManageAreas(Find.CurrentMap));
			}
		}

		// Token: 0x0600C61C RID: 50716 RVA: 0x0048A2BC File Offset: 0x004884BC
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x0600C61D RID: 50717 RVA: 0x0048A2E0 File Offset: 0x004884E0
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer || !pawn.playerSettings.SupportsAllowedAreas)
			{
				return int.MinValue;
			}
			Area areaRestrictionInPawnCurrentMap = pawn.playerSettings.AreaRestrictionInPawnCurrentMap;
			if (areaRestrictionInPawnCurrentMap == null)
			{
				return -2147483647;
			}
			return areaRestrictionInPawnCurrentMap.ID;
		}

		// Token: 0x0600C61E RID: 50718 RVA: 0x0048A328 File Offset: 0x00488528
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift && Find.CurrentMap != null)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (pawnsListForReading[i].Faction == Faction.OfPlayer && pawnsListForReading[i].playerSettings.SupportsAllowedAreas)
					{
						if (Event.current.button == 0)
						{
							pawnsListForReading[i].playerSettings.AreaRestrictionInPawnCurrentMap = Find.CurrentMap.areaManager.Home;
						}
						else if (Event.current.button == 1)
						{
							pawnsListForReading[i].playerSettings.AreaRestrictionInPawnCurrentMap = null;
						}
					}
				}
				if (Event.current.button == 0)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				if (Event.current.button == 1)
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x0600C61F RID: 50719 RVA: 0x0048A414 File Offset: 0x00488614
		protected override string GetHeaderTip(PawnTable table)
		{
			string text = base.GetHeaderTip(table);
			if (!SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				text += "\n" + "AllowedAreaShiftClickTip".Translate();
			}
			return text;
		}

		// Token: 0x040070EE RID: 28910
		private const int TopAreaHeight = 65;

		// Token: 0x040070EF RID: 28911
		private const int ManageAreasButtonHeight = 32;
	}
}
