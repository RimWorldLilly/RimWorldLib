using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023E1 RID: 9185
	public class WorldPawnGC : IExposable
	{
		// Token: 0x0600E544 RID: 58692 RVA: 0x00551D1C File Offset: 0x0054FF1C
		public void WorldPawnGCTick()
		{
			if (this.lastSuccessfulGCTick < Find.TickManager.TicksGame / 15000 * 15000)
			{
				if (this.activeGCProcess == null)
				{
					this.activeGCProcess = this.PawnGCPass().GetEnumerator();
					if (DebugViewSettings.logWorldPawnGC)
					{
						Log.Message(string.Format("World pawn GC started at rate {0}", this.currentGCRate));
					}
				}
				if (this.activeGCProcess != null)
				{
					bool flag = false;
					int num = 0;
					while (num < this.currentGCRate && !flag)
					{
						flag = !this.activeGCProcess.MoveNext();
						num++;
					}
					if (flag)
					{
						this.lastSuccessfulGCTick = Find.TickManager.TicksGame;
						this.currentGCRate = 1;
						this.activeGCProcess = null;
						if (DebugViewSettings.logWorldPawnGC)
						{
							Log.Message("World pawn GC complete");
						}
					}
				}
			}
		}

		// Token: 0x0600E545 RID: 58693 RVA: 0x00551DE3 File Offset: 0x0054FFE3
		public void CancelGCPass()
		{
			if (this.activeGCProcess != null)
			{
				this.activeGCProcess = null;
				this.currentGCRate = Mathf.Min(this.currentGCRate * 2, 16777216);
				if (DebugViewSettings.logWorldPawnGC)
				{
					Log.Message("World pawn GC cancelled");
				}
			}
		}

		// Token: 0x0600E546 RID: 58694 RVA: 0x00551E1D File Offset: 0x0055001D
		private IEnumerable AccumulatePawnGCData(Dictionary<Pawn, string> keptPawns)
		{
			WorldPawnGC.<AccumulatePawnGCData>d__10 <AccumulatePawnGCData>d__ = new WorldPawnGC.<AccumulatePawnGCData>d__10(-2);
			<AccumulatePawnGCData>d__.<>4__this = this;
			<AccumulatePawnGCData>d__.<>3__keptPawns = keptPawns;
			return <AccumulatePawnGCData>d__;
		}

		// Token: 0x0600E547 RID: 58695 RVA: 0x00551E34 File Offset: 0x00550034
		public Dictionary<Pawn, string> AccumulatePawnGCDataImmediate()
		{
			Dictionary<Pawn, string> dictionary = new Dictionary<Pawn, string>();
			this.AccumulatePawnGCData(dictionary).ExecuteEnumerable();
			return dictionary;
		}

		// Token: 0x0600E548 RID: 58696 RVA: 0x00551E54 File Offset: 0x00550054
		public string PawnGCDebugResults()
		{
			Dictionary<Pawn, string> dictionary = this.AccumulatePawnGCDataImmediate();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			foreach (Pawn key in Find.WorldPawns.AllPawnsAliveOrDead)
			{
				string text = "Discarded";
				if (dictionary.ContainsKey(key))
				{
					text = dictionary[key];
				}
				if (!dictionary2.ContainsKey(text))
				{
					dictionary2[text] = 0;
				}
				Dictionary<string, int> dictionary3 = dictionary2;
				string key2 = text;
				int value = dictionary3[key2] + 1;
				dictionary3[key2] = value;
			}
			return (from kvp in dictionary2
			orderby kvp.Value descending
			select string.Format("{0}: {1}", kvp.Value, kvp.Key)).ToLineList(null, false);
		}

		// Token: 0x0600E549 RID: 58697 RVA: 0x00551F48 File Offset: 0x00550148
		public IEnumerable PawnGCPass()
		{
			WorldPawnGC.<PawnGCPass>d__13 <PawnGCPass>d__ = new WorldPawnGC.<PawnGCPass>d__13(-2);
			<PawnGCPass>d__.<>4__this = this;
			return <PawnGCPass>d__;
		}

		// Token: 0x0600E54A RID: 58698 RVA: 0x00551F58 File Offset: 0x00550158
		private string GetCriticalPawnReason(Pawn pawn)
		{
			if (pawn.Discarded)
			{
				return null;
			}
			if (PawnUtility.EverBeenColonistOrTameAnimal(pawn) && pawn.RaceProps.Humanlike)
			{
				return "Colonist";
			}
			if (pawn.RaceProps.Animal && TrainableUtility.GetAllColonistBondsFor(pawn).Any<Pawn>())
			{
				return "BondedAnimal";
			}
			if (PawnGenerator.IsBeingGenerated(pawn))
			{
				return "Generating";
			}
			if (PawnUtility.IsFactionLeader(pawn))
			{
				return "FactionLeader";
			}
			if (PawnUtility.IsKidnappedPawn(pawn))
			{
				return "Kidnapped";
			}
			if (pawn.IsCaravanMember())
			{
				return "CaravanMember";
			}
			if (PawnUtility.IsTravelingInTransportPodWorldObject(pawn))
			{
				return "TransportPod";
			}
			if (PawnUtility.ForSaleBySettlement(pawn))
			{
				return "ForSale";
			}
			if (Find.WorldPawns.ForcefullyKeptPawns.Contains(pawn))
			{
				return "ForceKept";
			}
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				return "Spawned";
			}
			if (!pawn.Corpse.DestroyedOrNull())
			{
				return "CorpseExists";
			}
			if (pawn.RaceProps.Humanlike && Current.ProgramState == ProgramState.Playing)
			{
				if (Find.PlayLog.AnyEntryConcerns(pawn))
				{
					return "InPlayLog";
				}
				if (Find.BattleLog.AnyEntryConcerns(pawn))
				{
					return "InBattleLog";
				}
			}
			if (Current.ProgramState == ProgramState.Playing && Find.TaleManager.AnyActiveTaleConcerns(pawn))
			{
				return "InActiveTale";
			}
			if (QuestUtility.IsReservedByQuestOrQuestBeingGenerated(pawn))
			{
				return "ReservedByQuest";
			}
			if (Find.WorldPawns.PawnSourced(pawn))
			{
				return "CompPawnSource";
			}
			return null;
		}

		// Token: 0x0600E54B RID: 58699 RVA: 0x005520B0 File Offset: 0x005502B0
		public void AddAllRelationships(Pawn pawn, Dictionary<Pawn, string> keptPawns)
		{
			if (pawn.relations == null)
			{
				return;
			}
			if (!pawn.Spawned)
			{
				if (!pawn.RaceProps.Humanlike)
				{
					return;
				}
				if (!pawn.Faction.IsPlayerSafe())
				{
					return;
				}
			}
			foreach (Pawn pawn2 in pawn.relations.RelatedPawns)
			{
				if (this.logDotgraph != null)
				{
					string text = string.Format("{0}->{1} [label=<{2}> color=\"purple\"];", WorldPawnGC.DotgraphIdentifier(pawn), WorldPawnGC.DotgraphIdentifier(pawn2), pawn.GetRelations(pawn2).FirstOrDefault<PawnRelationDef>().ToString());
					if (!this.logDotgraphUniqueLinks.Contains(text))
					{
						this.logDotgraphUniqueLinks.Add(text);
						this.logDotgraph.AppendLine(text);
					}
				}
				if (!keptPawns.ContainsKey(pawn2))
				{
					keptPawns[pawn2] = "Relationship";
				}
			}
		}

		// Token: 0x0600E54C RID: 58700 RVA: 0x00552198 File Offset: 0x00550398
		public void AddAllMemories(Pawn pawn, Dictionary<Pawn, string> keptPawns)
		{
			if (pawn.needs == null || pawn.needs.mood == null || pawn.needs.mood.thoughts == null || pawn.needs.mood.thoughts.memories == null)
			{
				return;
			}
			if (!pawn.Spawned)
			{
				if (!pawn.RaceProps.Humanlike)
				{
					return;
				}
				if (!pawn.Faction.IsPlayerSafe())
				{
					return;
				}
			}
			foreach (Thought_Memory thought_Memory in pawn.needs.mood.thoughts.memories.Memories)
			{
				if (thought_Memory.otherPawn != null)
				{
					if (this.logDotgraph != null)
					{
						string text = string.Format("{0}->{1} [label=<{2}> color=\"orange\"];", WorldPawnGC.DotgraphIdentifier(pawn), WorldPawnGC.DotgraphIdentifier(thought_Memory.otherPawn), thought_Memory.def);
						if (!this.logDotgraphUniqueLinks.Contains(text))
						{
							this.logDotgraphUniqueLinks.Add(text);
							this.logDotgraph.AppendLine(text);
						}
					}
					if (!keptPawns.ContainsKey(thought_Memory.otherPawn))
					{
						keptPawns[thought_Memory.otherPawn] = "Memory";
					}
				}
			}
		}

		// Token: 0x0600E54D RID: 58701 RVA: 0x005522DC File Offset: 0x005504DC
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastSuccessfulGCTick, "lastSuccessfulGCTick", 0, false);
			Scribe_Values.Look<int>(ref this.currentGCRate, "nextGCRate", 1, false);
		}

		// Token: 0x0600E54E RID: 58702 RVA: 0x00552302 File Offset: 0x00550502
		public void LogGC()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= GC =======");
			stringBuilder.AppendLine(this.PawnGCDebugResults());
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600E54F RID: 58703 RVA: 0x0055232C File Offset: 0x0055052C
		public void RunGC()
		{
			this.CancelGCPass();
			PerfLogger.Reset();
			foreach (object obj in this.PawnGCPass())
			{
			}
			float num = PerfLogger.Duration() * 1000f;
			PerfLogger.Flush();
			Log.Message(string.Format("World pawn GC run complete in {0} ms", num));
		}

		// Token: 0x0600E550 RID: 58704 RVA: 0x005523AC File Offset: 0x005505AC
		public void LogDotgraph()
		{
			this.logDotgraph = new StringBuilder();
			this.logDotgraphUniqueLinks = new HashSet<string>();
			this.logDotgraph.AppendLine("digraph { rankdir=LR;");
			this.AccumulatePawnGCDataImmediate();
			this.logDotgraph.AppendLine("}");
			GUIUtility.systemCopyBuffer = this.logDotgraph.ToString();
			Log.Message("Dotgraph copied to clipboard");
			this.logDotgraph = null;
			this.logDotgraphUniqueLinks = null;
		}

		// Token: 0x0600E551 RID: 58705 RVA: 0x00552420 File Offset: 0x00550620
		public static string DotgraphIdentifier(Pawn pawn)
		{
			return new string((from ch in pawn.LabelShort
			where char.IsLetter(ch)
			select ch).ToArray<char>()) + "_" + pawn.thingIDNumber.ToString();
		}

		// Token: 0x04009193 RID: 37267
		private int lastSuccessfulGCTick;

		// Token: 0x04009194 RID: 37268
		private int currentGCRate = 1;

		// Token: 0x04009195 RID: 37269
		private const int AdditionalRandomHumanlikes = 10;

		// Token: 0x04009196 RID: 37270
		private const float AdditionalRandomHumanlikeKeepChance = 0.25f;

		// Token: 0x04009197 RID: 37271
		private const int GCUpdateInterval = 15000;

		// Token: 0x04009198 RID: 37272
		private IEnumerator activeGCProcess;

		// Token: 0x04009199 RID: 37273
		private StringBuilder logDotgraph;

		// Token: 0x0400919A RID: 37274
		private HashSet<string> logDotgraphUniqueLinks;
	}
}
