using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F78 RID: 3960
	public class IncidentWorker
	{
		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x06006575 RID: 25973 RVA: 0x00241F25 File Offset: 0x00240125
		public virtual float BaseChanceThisGame
		{
			get
			{
				if (ModsConfig.RoyaltyActive && this.def.baseChanceWithRoyalty >= 0f)
				{
					return this.def.baseChanceWithRoyalty;
				}
				return this.def.baseChance;
			}
		}

		// Token: 0x06006576 RID: 25974 RVA: 0x00241F57 File Offset: 0x00240157
		public virtual float ChanceFactorNow(IIncidentTarget target)
		{
			return 1f;
		}

		// Token: 0x06006577 RID: 25975 RVA: 0x00241F60 File Offset: 0x00240160
		public bool CanFireNow(IncidentParms parms)
		{
			if (!parms.forced)
			{
				if (!this.def.TargetAllowed(parms.target))
				{
					return false;
				}
				if (!parms.bypassStorytellerSettings)
				{
					if (GenDate.DaysPassedSinceSettle < this.def.earliestDay)
					{
						return false;
					}
					if (!Find.Storyteller.difficulty.AllowedBy(this.def.disabledWhen) || (this.def.category == IncidentCategoryDefOf.ThreatBig && !Find.Storyteller.difficulty.allowBigThreats))
					{
						return false;
					}
				}
				if (parms.points >= 0f && parms.points < this.def.minThreatPoints)
				{
					return false;
				}
				if (parms.points >= 0f && parms.points > this.def.maxThreatPoints)
				{
					return false;
				}
				if (!this.def.allowedBiomes.NullOrEmpty<BiomeDef>())
				{
					BiomeDef primaryBiome = Find.WorldGrid[parms.target.Tile].PrimaryBiome;
					if (!this.def.allowedBiomes.Contains(primaryBiome))
					{
						return false;
					}
				}
				if (!this.def.disallowedBiomes.NullOrEmpty<BiomeDef>())
				{
					BiomeDef primaryBiome2 = Find.WorldGrid[parms.target.Tile].PrimaryBiome;
					if (this.def.disallowedBiomes.Contains(primaryBiome2))
					{
						return false;
					}
				}
				if (parms.target.Tile.Valid)
				{
					PlanetLayerDef layerDef = parms.target.Tile.LayerDef;
					if (!this.def.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !this.def.layerWhitelist.Contains(layerDef))
					{
						return false;
					}
					if (!this.def.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && this.def.layerBlacklist.Contains(layerDef))
					{
						return false;
					}
					if (!this.def.canOccurOnAllPlanetLayers && layerDef.onlyAllowWhitelistedIncidents && (this.def.layerWhitelist.NullOrEmpty<PlanetLayerDef>() || !this.def.layerWhitelist.Contains(layerDef)))
					{
						return false;
					}
				}
				if (!parms.bypassStorytellerSettings)
				{
					Scenario scenario = Find.Scenario;
					for (int i = 0; i < scenario.parts.Count; i++)
					{
						ScenPart_DisableIncident scenPart_DisableIncident = scenario.parts[i] as ScenPart_DisableIncident;
						if (scenPart_DisableIncident != null && scenPart_DisableIncident.Incident == this.def)
						{
							return false;
						}
					}
				}
				if (this.def.minPopulation > 0 && PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count<Pawn>() < this.def.minPopulation)
				{
					return false;
				}
				if (this.FiredTooRecently(parms.target))
				{
					return false;
				}
				if (this.def.minGreatestPopulation > 0 && Find.StoryWatcher.statsRecord.greatestPopulation < this.def.minGreatestPopulation)
				{
					return false;
				}
				if (ModsConfig.AnomalyActive && this.def.IsAnomalyIncident && Find.Anomaly.LevelDef.anomalyThreatTier < this.def.minAnomalyThreatLevel && Find.Anomaly.GenerateMonolith)
				{
					return false;
				}
				Map map = parms.target as Map;
				if (map != null)
				{
					using (List<GameCondition>.Enumerator enumerator = map.gameConditionManager.ActiveConditions.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.def.preventIncidents)
							{
								return false;
							}
						}
					}
				}
				if (Find.GameEnder.gameEnding && (this.def.category == IncidentCategoryDefOf.ThreatBig || this.def.category == IncidentCategoryDefOf.ThreatSmall))
				{
					return false;
				}
				if (Find.TickManager.TicksGame < Find.GameEnder.newWanderersCreatedTick + 300000 && this.def.category == IncidentCategoryDefOf.ThreatBig)
				{
					return false;
				}
			}
			if (this.lastCheckCanRunTick == Find.TickManager.TicksGame)
			{
				return this.lastCanRunResult;
			}
			this.lastCheckCanRunTick = Find.TickManager.TicksGame;
			if (ModsConfig.AnomalyActive && Find.Anomaly.VoidAwakeningActive())
			{
				this.lastCanRunResult = false;
			}
			else
			{
				this.lastCanRunResult = this.CanFireNowSub(parms);
			}
			return this.lastCanRunResult;
		}

		// Token: 0x06006578 RID: 25976 RVA: 0x00242388 File Offset: 0x00240588
		public bool FiredTooRecently(IIncidentTarget target)
		{
			Dictionary<IncidentDef, int> lastFireTicks = target.StoryState.lastFireTicks;
			int ticksGame = Find.TickManager.TicksGame;
			int num;
			if (lastFireTicks.TryGetValue(this.def, out num) && (float)(ticksGame - num) / 60000f < this.def.minRefireDays)
			{
				return true;
			}
			List<IncidentDef> refireCheckIncidents = this.def.RefireCheckIncidents;
			if (refireCheckIncidents != null)
			{
				for (int i = 0; i < refireCheckIncidents.Count; i++)
				{
					if (lastFireTicks.TryGetValue(refireCheckIncidents[i], out num) && (float)(ticksGame - num) / 60000f < this.def.minRefireDays)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006579 RID: 25977 RVA: 0x00242425 File Offset: 0x00240625
		protected virtual bool CanFireNowSub(IncidentParms parms)
		{
			return true;
		}

		// Token: 0x0600657A RID: 25978 RVA: 0x00242428 File Offset: 0x00240628
		public bool TryExecute(IncidentParms parms)
		{
			Map map = parms.target as Map;
			if (map != null && this.def.requireColonistsPresent && map.mapPawns.FreeColonistsSpawnedCount == 0)
			{
				return true;
			}
			bool flag = this.TryExecuteWorker(parms);
			if (flag)
			{
				if (this.def.tale != null)
				{
					Pawn pawn = null;
					Caravan caravan = parms.target as Caravan;
					if (caravan != null)
					{
						pawn = caravan.RandomOwner();
					}
					else
					{
						Map map2 = parms.target as Map;
						if (map2 != null)
						{
							pawn = map2.mapPawns.FreeColonistsSpawned.RandomElementWithFallback(null);
						}
						else if (parms.target is World)
						{
							pawn = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended.RandomElementWithFallback(null);
						}
					}
					if (pawn != null)
					{
						TaleRecorder.RecordTale(this.def.tale, new object[]
						{
							pawn
						});
					}
				}
				if (this.def.category.tale != null)
				{
					Tale tale = TaleRecorder.RecordTale(this.def.category.tale, Array.Empty<object>());
					if (tale != null)
					{
						tale.customLabel = this.def.label;
						if (this.def.hidden)
						{
							tale.hidden = true;
						}
					}
				}
			}
			Find.Storyteller.RecordIncidentFired(this.def);
			if (ModsConfig.AnomalyActive && this.def.codexEntry != null)
			{
				Find.EntityCodex.SetDiscovered(this.def.codexEntry, null, null);
			}
			return flag;
		}

		// Token: 0x0600657B RID: 25979 RVA: 0x00242588 File Offset: 0x00240788
		public static void SendIncidentLetter(TaggedString baseLetterLabel, TaggedString baseLetterText, LetterDef baseLetterDef, IncidentParms parms, LookTargets lookTargets, IncidentDef def, params NamedArgument[] textArgs)
		{
			if (parms.silent || !parms.sendLetter)
			{
				return;
			}
			if (baseLetterLabel.NullOrEmpty() || baseLetterText.NullOrEmpty())
			{
				Log.Error("Sending standard incident letter with no label or text.");
			}
			TaggedString taggedString = baseLetterText.Formatted(textArgs);
			TaggedString text;
			if (parms.customLetterText.NullOrEmpty())
			{
				text = taggedString;
			}
			else
			{
				List<NamedArgument> list = new List<NamedArgument>();
				if (textArgs != null)
				{
					list.AddRange(textArgs);
				}
				list.Add(taggedString.Named("BASETEXT"));
				text = parms.customLetterText.Formatted(list.ToArray());
			}
			TaggedString taggedString2 = baseLetterLabel.Formatted(textArgs);
			TaggedString label;
			if (parms.customLetterLabel.NullOrEmpty())
			{
				label = taggedString2;
			}
			else
			{
				List<NamedArgument> list2 = new List<NamedArgument>();
				if (textArgs != null)
				{
					list2.AddRange(textArgs);
				}
				list2.Add(taggedString2.Named("BASELABEL"));
				label = parms.customLetterLabel.Formatted(list2.ToArray());
			}
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, parms.customLetterDef ?? baseLetterDef, lookTargets, parms.faction, parms.quest, parms.letterHyperlinkThingDefs);
			List<HediffDef> list3 = new List<HediffDef>();
			if (!parms.letterHyperlinkHediffDefs.NullOrEmpty<HediffDef>())
			{
				list3.AddRange(parms.letterHyperlinkHediffDefs);
			}
			if (def != null && !def.letterHyperlinkHediffDefs.NullOrEmpty<HediffDef>())
			{
				list3.AddRange(def.letterHyperlinkHediffDefs);
			}
			choiceLetter.hyperlinkHediffDefs = list3;
			Find.LetterStack.ReceiveLetter(choiceLetter, null, 0, true);
		}

		// Token: 0x0600657C RID: 25980 RVA: 0x002426F3 File Offset: 0x002408F3
		protected virtual bool TryExecuteWorker(IncidentParms parms)
		{
			Log.Error("Unimplemented incident " + ((this != null) ? this.ToString() : null));
			return false;
		}

		// Token: 0x0600657D RID: 25981 RVA: 0x00242712 File Offset: 0x00240912
		protected void SendStandardLetter(IncidentParms parms, LookTargets lookTargets, params NamedArgument[] textArgs)
		{
			this.SendStandardLetter(this.def.letterLabel, this.def.letterText, this.def.letterDef, parms, lookTargets, textArgs);
		}

		// Token: 0x0600657E RID: 25982 RVA: 0x00242748 File Offset: 0x00240948
		protected void SendStandardLetter(TaggedString baseLetterLabel, TaggedString baseLetterText, LetterDef baseLetterDef, IncidentParms parms, LookTargets lookTargets, params NamedArgument[] textArgs)
		{
			IncidentWorker.SendIncidentLetter(baseLetterLabel, baseLetterText, baseLetterDef, parms, lookTargets, this.def, textArgs);
		}

		// Token: 0x04003C08 RID: 15368
		public IncidentDef def;

		// Token: 0x04003C09 RID: 15369
		[Unsaved(false)]
		private int lastCheckCanRunTick;

		// Token: 0x04003C0A RID: 15370
		[Unsaved(false)]
		private bool lastCanRunResult;
	}
}
