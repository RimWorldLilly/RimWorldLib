using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200224C RID: 8780
	public class QuestNode_Root_Gravcore_AbandonedSettlement : QuestNode_Root_Gravcore
	{
		// Token: 0x0600D924 RID: 55588 RVA: 0x00506E28 File Offset: 0x00505028
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			PlanetTile tile;
			if (!this.TryFindSiteTile(out tile))
			{
				return;
			}
			Faction hostileFaction;
			Faction faction;
			if (!this.TryGetFactions(out faction, out hostileFaction))
			{
				return;
			}
			slate.Set<Faction>("settlementFaction", faction, false);
			slate.Set<Faction>("hostileFaction", hostileFaction, false);
			float points = slate.Get<float>("points", 0f, false);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			string inSignalEnable = QuestGenUtility.HardcodedSignalWithQuestID("gravcore.Unfogged");
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.AbandonedSettlement, new SitePartParams
				{
					points = 3000f,
					threatPoints = slate.Get<float>("points", 0f, false)
				})
			}, tile, faction, false, null, WorldObjectDefOf.ClaimableSite);
			site.desiredThreatPoints = 100f;
			site.doorsAlwaysOpenForPlayerPawns = true;
			slate.Set<Site>("site", site, false);
			quest.SpawnWorldObject(site, null, null);
			slate.Set<Thing>("gravcore", site.parts[0].things.FirstOrDefault((Thing t) => t.def == ThingDefOf.Gravcore), false);
			quest.Letter(LetterDefOf.NeutralEvent, inSignal, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[letterTextMapGenerated]", null, "[letterLabelMapGenerated]", null, null);
			if (Find.Storyteller.difficulty.allowViolentQuests)
			{
				quest.Delay(Rand.RangeInclusive(60, 120), delegate
				{
					int num = 0;
					int raidsCount = QuestNode_Root_Gravcore_AbandonedSettlement.RaidsCountRange.RandomInRange;
					for (int i = 0; i < raidsCount; i++)
					{
						int localIndex = i;
						int num2 = (i == 0) ? 0 : QuestNode_Root_Gravcore_AbandonedSettlement.TicksBetweenRaidsRange.RandomInRange;
						quest.Delay(num + num2, delegate
						{
							Faction hostileFaction;
							string text = "GravcoreAbandonedSettlementRaid".Translate(hostileFaction.Named("FACTION")).Resolve();
							if (localIndex < raidsCount - 1)
							{
								text = text + "\n\n" + "GravcoreAbandonedSettlementRaid_Reinforcements".Translate().Resolve();
							}
							Quest quest = quest;
							MapParent site = site;
							FloatRange pointsRange = new FloatRange(points);
							hostileFaction = hostileFaction;
							string inSignal3 = null;
							RaidStrategyDef immediateAttack = RaidStrategyDefOf.ImmediateAttack;
							quest.RandomRaid(site, pointsRange, hostileFaction, inSignal3, PawnsArrivalModeDefOf.EdgeWalkInGroups, immediateAttack, "LetterLabelAmbushInExistingMap".Translate().CapitalizeFirst() + ": " + hostileFaction.Name, text, false);
							if (localIndex == raidsCount - 1)
							{
								quest.End(QuestEndOutcome.Success, false, false);
							}
						}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Raid delay";
						num += num2;
					}
				}, inSignalEnable, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			}
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_DefinedThingDef(ThingDefOf.Gravcore));
			quest.RewardChoice(null, null).choices.Add(choice);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x0600D925 RID: 55589 RVA: 0x0050704C File Offset: 0x0050524C
		protected override bool TestRunInt(Slate slate)
		{
			Faction faction;
			Faction faction2;
			return base.TestRunInt(slate) && this.TryGetFactions(out faction, out faction2);
		}

		// Token: 0x0600D926 RID: 55590 RVA: 0x00507074 File Offset: 0x00505274
		private bool TryGetFactions(out Faction settlementFaction, out Faction hostileFaction)
		{
			IEnumerable<Faction> source = from x in Find.FactionManager.AllFactionsVisible
			where x.def.humanlikeFaction && !x.IsPlayer && !x.temporary
			select x;
			Faction setFac;
			if (!source.TryRandomElement(out setFac))
			{
				hostileFaction = null;
				settlementFaction = null;
				return false;
			}
			settlementFaction = setFac;
			if (!(from x in source
			where x.HostileTo(Faction.OfPlayer) && x != setFac
			select x).TryRandomElement(out hostileFaction))
			{
				hostileFaction = null;
				return false;
			}
			return true;
		}

		// Token: 0x04008AD5 RID: 35541
		private static readonly IntRange TicksBetweenRaidsRange = new IntRange(30000, 60000);

		// Token: 0x04008AD6 RID: 35542
		private static readonly IntRange RaidsCountRange = new IntRange(3, 4);

		// Token: 0x04008AD7 RID: 35543
		private const int SettlementPoints = 3000;

		// Token: 0x04008AD8 RID: 35544
		private const string GravcoreAlias = "gravcore";
	}
}
