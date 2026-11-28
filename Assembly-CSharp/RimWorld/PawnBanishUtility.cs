using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B8 RID: 5304
	public static class PawnBanishUtility
	{
		// Token: 0x060081D6 RID: 33238 RVA: 0x002E5564 File Offset: 0x002E3764
		public static void Banish(Pawn pawn, bool giveThoughts = true)
		{
			PawnBanishUtility.Banish(pawn, PlanetTile.Invalid, giveThoughts);
		}

		// Token: 0x060081D7 RID: 33239 RVA: 0x002E5574 File Offset: 0x002E3774
		public static void Banish(Pawn pawn, PlanetTile tile, bool giveThoughts = true)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				Log.Warning("Tried to banish " + ((pawn != null) ? pawn.ToString() : null) + " but he's neither a colonist, tame animal, nor prisoner.");
				return;
			}
			if (!tile.Valid)
			{
				tile = pawn.Tile;
			}
			bool flag = PawnBanishUtility.WouldBeLeftToDie(pawn, tile);
			if (!pawn.IsQuestLodger() && giveThoughts)
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(pawn, null, flag ? PawnDiedOrDownedThoughtsKind.BanishedToDie : PawnDiedOrDownedThoughtsKind.Banished);
			}
			Caravan caravan = pawn.GetCaravan();
			if (caravan != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
				caravan.RemovePawn(pawn);
				if (flag)
				{
					if (Rand.Value < 0.8f)
					{
						pawn.Kill(null, null);
					}
					else
					{
						PawnBanishUtility.HealIfPossible(pawn);
					}
				}
			}
			if (pawn.guest != null)
			{
				pawn.guest.SetGuestStatus(null, GuestStatus.Guest);
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				Faction faction;
				if (!pawn.Spawned && Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, pawn.Faction != null && pawn.Faction.def.techLevel >= TechLevel.Medieval, false, TechLevel.Undefined, TechLevel.Undefined, false, false))
				{
					if (pawn.Faction != faction)
					{
						pawn.SetFaction(faction, null);
					}
				}
				else if (pawn.Faction != null)
				{
					pawn.SetFaction(null, null);
				}
				FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
				if (ideos != null)
				{
					ideos.RecalculateIdeosBasedOnPlayerPawns();
				}
			}
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "Banished", pawn.Named("SUBJECT"));
		}

		// Token: 0x060081D8 RID: 33240 RVA: 0x002E56F0 File Offset: 0x002E38F0
		public static bool WouldBeLeftToDie(Pawn p, PlanetTile tile)
		{
			if (p.Downed)
			{
				return true;
			}
			if (p.health.hediffSet.BleedRateTotal > 0.4f)
			{
				return true;
			}
			if (tile.Valid)
			{
				float f = GenTemperature.AverageTemperatureAtTileForTwelfth(tile, GenLocalDate.Twelfth(p));
				if (!p.SafeTemperatureRange().Includes(f))
				{
					return true;
				}
			}
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].IsCurrentlyLifeThreatening)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060081D9 RID: 33241 RVA: 0x002E577C File Offset: 0x002E397C
		public static string GetBanishPawnDialogText(Pawn banishedPawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = PawnBanishUtility.WouldBeLeftToDie(banishedPawn, banishedPawn.Tile);
			stringBuilder.Append("ConfirmBanishPawnDialog".Translate(banishedPawn.Label, banishedPawn).Resolve());
			if (flag)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ConfirmBanishPawnDialog_LeftToDie".Translate(banishedPawn.LabelShort, banishedPawn).Resolve().CapitalizeFirst());
			}
			List<ThingWithComps> list = (banishedPawn.equipment != null) ? banishedPawn.equipment.AllEquipmentListForReading : null;
			List<Apparel> list2 = (banishedPawn.apparel != null) ? banishedPawn.apparel.WornApparel : null;
			ThingOwner<Thing> thingOwner = (banishedPawn.inventory != null && PawnBanishUtility.WillTakeInventoryIfBanished(banishedPawn)) ? banishedPawn.inventory.innerContainer : null;
			if (!list.NullOrEmpty<ThingWithComps>() || !list2.NullOrEmpty<Apparel>() || !thingOwner.NullOrEmpty<Thing>())
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				stringBuilder.Append("ConfirmBanishPawnDialog_Items".Translate(banishedPawn.LabelShort, banishedPawn).Resolve().CapitalizeFirst().AdjustedFor(banishedPawn, "PAWN", true));
				stringBuilder.AppendLine();
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append("  - " + list[i].LabelCap);
					}
				}
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append("  - " + list2[j].LabelCap);
					}
				}
				if (thingOwner != null)
				{
					for (int k = 0; k < thingOwner.Count; k++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append("  - " + thingOwner[k].LabelCap);
					}
				}
			}
			if (!banishedPawn.IsQuestLodger() && (banishedPawn.guilt == null || !banishedPawn.guilt.IsGuilty))
			{
				PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(banishedPawn, null, flag ? PawnDiedOrDownedThoughtsKind.BanishedToDie : PawnDiedOrDownedThoughtsKind.Banished, stringBuilder, "\n\n" + "ConfirmBanishPawnDialog_IndividualThoughts".Translate(banishedPawn.LabelShort, banishedPawn), "\n\n" + "ConfirmBanishPawnDialog_AllColonistsThoughts".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060081DA RID: 33242 RVA: 0x002E59FC File Offset: 0x002E3BFC
		public static void ShowBanishPawnConfirmationDialog(Pawn pawn, Action onConfirm = null)
		{
			Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation(PawnBanishUtility.GetBanishPawnDialogText(pawn), delegate()
			{
				PawnBanishUtility.Banish(pawn, true);
				Action onConfirm2 = onConfirm;
				if (onConfirm2 == null)
				{
					return;
				}
				onConfirm2();
			}, true, null, WindowLayer.Dialog);
			Find.WindowStack.Add(window);
		}

		// Token: 0x060081DB RID: 33243 RVA: 0x002E5A50 File Offset: 0x002E3C50
		public static string GetBanishButtonTip(Pawn pawn)
		{
			if (PawnBanishUtility.WouldBeLeftToDie(pawn, pawn.Tile))
			{
				return "BanishTip".Translate() + "\n\n" + "BanishTipWillDie".Translate(pawn.LabelShort, pawn).CapitalizeFirst();
			}
			return "BanishTip".Translate();
		}

		// Token: 0x060081DC RID: 33244 RVA: 0x002E5ABC File Offset: 0x002E3CBC
		private static void HealIfPossible(Pawn p)
		{
			PawnBanishUtility.tmpHediffs.Clear();
			PawnBanishUtility.tmpHediffs.AddRange(p.health.hediffSet.hediffs);
			for (int i = 0; i < PawnBanishUtility.tmpHediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury = PawnBanishUtility.tmpHediffs[i] as Hediff_Injury;
				if (hediff_Injury != null && !hediff_Injury.IsPermanent())
				{
					p.health.RemoveHediff(hediff_Injury);
				}
				else
				{
					ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(PawnBanishUtility.tmpHediffs[i].def);
					if (immunityRecord != null)
					{
						immunityRecord.immunity = 1f;
					}
				}
			}
		}

		// Token: 0x060081DD RID: 33245 RVA: 0x002E5B5B File Offset: 0x002E3D5B
		private static bool WillTakeInventoryIfBanished(Pawn pawn)
		{
			return !pawn.IsCaravanMember();
		}

		// Token: 0x0400507D RID: 20605
		private const float DeathChanceForCaravanPawnBanishedToDie = 0.8f;

		// Token: 0x0400507E RID: 20606
		private static readonly List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
