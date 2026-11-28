using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020011C1 RID: 4545
	public class IncidentWorker_GiveQuest : IncidentWorker
	{
		// Token: 0x06007186 RID: 29062 RVA: 0x0027C7E0 File Offset: 0x0027A9E0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			QuestScriptDef questScriptDef = this.def.questScriptDef ?? parms.questScriptDef;
			return (questScriptDef == null || questScriptDef.CanRun(parms.points, parms.target)) && IncidentWorker_GiveQuest.CanQuestOccurOnTile(parms.target.Tile, questScriptDef) && PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended.Any<Pawn>();
		}

		// Token: 0x06007187 RID: 29063 RVA: 0x0027C848 File Offset: 0x0027AA48
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			QuestScriptDef questScriptDef;
			if ((questScriptDef = this.def.questScriptDef) == null)
			{
				questScriptDef = (parms.questScriptDef ?? NaturalRandomQuestChooser.ChooseNaturalRandomQuest(parms.points, parms.target));
			}
			QuestScriptDef questScriptDef2 = questScriptDef;
			if (questScriptDef2 == null)
			{
				return false;
			}
			if (!IncidentWorker_GiveQuest.CanQuestOccurOnTile(parms.target.Tile, questScriptDef2))
			{
				return false;
			}
			parms.questScriptDef = questScriptDef2;
			this.GiveQuest(parms, questScriptDef2);
			return true;
		}

		// Token: 0x06007188 RID: 29064 RVA: 0x0027C8AC File Offset: 0x0027AAAC
		protected virtual void GiveQuest(IncidentParms parms, QuestScriptDef questDef)
		{
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questDef, parms.points);
			if (!quest.hidden && questDef.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, null);
			}
		}

		// Token: 0x06007189 RID: 29065 RVA: 0x0027C8E0 File Offset: 0x0027AAE0
		private static bool CanQuestOccurOnTile(PlanetTile tile, QuestScriptDef quest)
		{
			if (!tile.Valid)
			{
				return true;
			}
			if (quest != null)
			{
				PlanetLayerDef layerDef = tile.LayerDef;
				if (!quest.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !quest.layerWhitelist.Contains(layerDef))
				{
					return false;
				}
				if (!quest.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && quest.layerBlacklist.Contains(layerDef))
				{
					return false;
				}
				if (!quest.canOccurOnAllPlanetLayers && layerDef.onlyAllowWhitelistedIncidents && (quest.layerWhitelist.NullOrEmpty<PlanetLayerDef>() || !quest.layerWhitelist.Contains(layerDef)))
				{
					return false;
				}
				if (!quest.everAcceptableInSpace && layerDef.isSpace)
				{
					return false;
				}
			}
			return !tile.LayerDef.onlyAllowWhitelistedQuests;
		}
	}
}
