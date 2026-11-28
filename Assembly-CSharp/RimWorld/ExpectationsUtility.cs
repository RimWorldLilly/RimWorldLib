using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F16 RID: 7958
	public static class ExpectationsUtility
	{
		// Token: 0x0600CAE1 RID: 51937 RVA: 0x004A72DC File Offset: 0x004A54DC
		public static void Reset()
		{
			ExpectationsUtility.wealthExpectationsInOrder = (from ed in DefDatabase<ExpectationDef>.AllDefs
			where ed.WealthTriggered
			orderby ed.order
			select ed).ToList<ExpectationDef>();
			ExpectationsUtility.roleExpectationsInOrder = (from ed in DefDatabase<ExpectationDef>.AllDefs
			where ed.forRoles
			orderby ed.order
			select ed).ToList<ExpectationDef>();
		}

		// Token: 0x0600CAE2 RID: 51938 RVA: 0x004A7398 File Offset: 0x004A5598
		public static ExpectationDef CurrentExpectationFor(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return null;
			}
			if (p.Faction != Faction.OfPlayer && !p.IsPrisonerOfColony)
			{
				return ExpectationDefOf.ExtremelyLow;
			}
			if (p.MapHeld != null)
			{
				ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p.MapHeld);
				if (p.royalty != null && p.MapHeld.IsPlayerHome)
				{
					foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
					{
						RoyalTitle currentTitleInFaction = p.royalty.GetCurrentTitleInFaction(faction);
						if (currentTitleInFaction != null && currentTitleInFaction.conceited && currentTitleInFaction.def.minExpectation != null && currentTitleInFaction.def.minExpectation.order > expectationDef.order)
						{
							expectationDef = currentTitleInFaction.def.minExpectation;
						}
					}
				}
				if (ModsConfig.IdeologyActive)
				{
					Ideo ideo = p.Ideo;
					Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(p) : null;
					if (precept_Role != null && precept_Role.def.expectationsOffset != 0 && !MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive)
					{
						ExpectationDef expectationDef2 = ExpectationsUtility.ExpectationForOrder(Math.Max(expectationDef.order + precept_Role.def.expectationsOffset, 0), true);
						if (expectationDef2 != null)
						{
							expectationDef = expectationDef2;
						}
					}
				}
				return expectationDef;
			}
			return ExpectationDefOf.VeryLow;
		}

		// Token: 0x0600CAE3 RID: 51939 RVA: 0x004A74EC File Offset: 0x004A56EC
		public static ExpectationDef CurrentExpectationFor(Map m)
		{
			float wealthTotal = m.wealthWatcher.WealthTotal;
			for (int i = 0; i < ExpectationsUtility.wealthExpectationsInOrder.Count; i++)
			{
				ExpectationDef expectationDef = ExpectationsUtility.wealthExpectationsInOrder[i];
				if (wealthTotal < expectationDef.maxMapWealth)
				{
					return expectationDef;
				}
			}
			List<ExpectationDef> list = ExpectationsUtility.wealthExpectationsInOrder;
			return list[list.Count - 1];
		}

		// Token: 0x0600CAE4 RID: 51940 RVA: 0x004A7544 File Offset: 0x004A5744
		public static ExpectationDef ExpectationForOrder(int order, bool forRole = false)
		{
			for (int i = 0; i < ExpectationsUtility.wealthExpectationsInOrder.Count; i++)
			{
				ExpectationDef expectationDef = ExpectationsUtility.wealthExpectationsInOrder[i];
				if (order == expectationDef.order)
				{
					return expectationDef;
				}
			}
			if (forRole)
			{
				for (int j = 0; j < ExpectationsUtility.roleExpectationsInOrder.Count; j++)
				{
					ExpectationDef expectationDef2 = ExpectationsUtility.roleExpectationsInOrder[j];
					if (order == expectationDef2.order)
					{
						return expectationDef2;
					}
				}
			}
			return null;
		}

		// Token: 0x0600CAE5 RID: 51941 RVA: 0x004A75B0 File Offset: 0x004A57B0
		public static bool OffsetByRole(Pawn p)
		{
			if (ModsConfig.IdeologyActive && p.ideo != null)
			{
				Precept_Role role = p.Ideo.GetRole(p);
				if (role != null && role.def.expectationsOffset != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008173 RID: 33139
		private static List<ExpectationDef> wealthExpectationsInOrder;

		// Token: 0x04008174 RID: 33140
		private static List<ExpectationDef> roleExpectationsInOrder;
	}
}
