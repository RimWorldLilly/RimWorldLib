using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020023B8 RID: 9144
	[StaticConstructorOnStartup]
	public static class SettlementAbandonUtility
	{
		// Token: 0x0600E3E6 RID: 58342 RVA: 0x0054B180 File Offset: 0x00549380
		public static Command AbandonCommand(MapParent settlement)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandAbandonHome".Translate();
			command_Action.defaultDesc = "CommandAbandonHomeDesc".Translate();
			command_Action.icon = SettlementAbandonUtility.AbandonCommandTex;
			command_Action.action = delegate()
			{
				SettlementAbandonUtility.TryAbandonViaInterface(settlement);
			};
			command_Action.Order = 3000f;
			if (SettlementAbandonUtility.AllColonistsThere(settlement))
			{
				command_Action.Disable("CommandAbandonHomeFailAllColonistsThere".Translate());
			}
			return command_Action;
		}

		// Token: 0x0600E3E7 RID: 58343 RVA: 0x0054B218 File Offset: 0x00549418
		public static bool AllColonistsThere(MapParent settlement)
		{
			return !CaravanUtility.PlayerHasAnyCaravan() && !Find.Maps.Any((Map x) => x.info.parent != settlement && x.mapPawns.FreeColonistsSpawned.Any<Pawn>());
		}

		// Token: 0x0600E3E8 RID: 58344 RVA: 0x0054B254 File Offset: 0x00549454
		public static void TryAbandonViaInterface(MapParent settlement)
		{
			ArchotechSettlement archotechSettlement = settlement as ArchotechSettlement;
			if (archotechSettlement != null && archotechSettlement.AnyArchotechBuildingRequiresStudy)
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("AbandonArchotechStructuresWarningDialog".Translate(), delegate()
				{
					SettlementAbandonUtility.TryAbandonWithColonyCheck(settlement);
				}, false, null, WindowLayer.Dialog));
				return;
			}
			SettlementAbandonUtility.TryAbandonWithColonyCheck(settlement);
		}

		// Token: 0x0600E3E9 RID: 58345 RVA: 0x0054B2BC File Offset: 0x005494BC
		private static void TryAbandonWithColonyCheck(MapParent settlement)
		{
			Map map = settlement.Map;
			if (map == null)
			{
				SettlementAbandonUtility.Abandon(settlement);
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			List<Pawn> source = map.mapPawns.PawnsInFaction(Faction.OfPlayer);
			if (source.Count<Pawn>() != 0)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (Pawn pawn in from x in source
				orderby x.IsColonist descending
				select x)
				{
					if (stringBuilder2.Length > 0)
					{
						stringBuilder2.AppendLine();
					}
					stringBuilder2.Append("    " + pawn.LabelCap);
				}
				stringBuilder.Append(ModsConfig.BiotechActive ? "ConfirmAbandonHomeWithColonyPawnsIncMechs".Translate(stringBuilder2) : "ConfirmAbandonHomeWithColonyPawns".Translate(stringBuilder2));
			}
			PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(map.mapPawns.AllPawns, PawnDiedOrDownedThoughtsKind.Banished, stringBuilder, null, "\n\n" + "ConfirmAbandonHomeNegativeThoughts_Everyone".Translate(), "ConfirmAbandonHomeNegativeThoughts");
			if (stringBuilder.Length == 0)
			{
				SettlementAbandonUtility.Abandon(settlement);
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				return;
			}
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), delegate()
			{
				SettlementAbandonUtility.Abandon(settlement);
			}, false, null, WindowLayer.Dialog));
		}

		// Token: 0x0600E3EA RID: 58346 RVA: 0x0054B460 File Offset: 0x00549660
		private static void Abandon(MapParent settlement)
		{
			settlement.Abandon(false);
			Find.GameEnder.CheckOrUpdateGameOver();
		}

		// Token: 0x0400911E RID: 37150
		private static readonly Texture2D AbandonCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/AbandonHome", true);
	}
}
