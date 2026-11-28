using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011F7 RID: 4599
	public class StoryState : IExposable
	{
		// Token: 0x17001364 RID: 4964
		// (get) Token: 0x0600724E RID: 29262 RVA: 0x002807D6 File Offset: 0x0027E9D6
		public IIncidentTarget Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17001365 RID: 4965
		// (get) Token: 0x0600724F RID: 29263 RVA: 0x002807DE File Offset: 0x0027E9DE
		public List<QuestScriptDef> RecentRandomQuests
		{
			get
			{
				return this.recentRandomQuests;
			}
		}

		// Token: 0x17001366 RID: 4966
		// (get) Token: 0x06007250 RID: 29264 RVA: 0x002807E6 File Offset: 0x0027E9E6
		public List<QuestScriptDef> RecentRandomDecrees
		{
			get
			{
				return this.recentRandomDecrees;
			}
		}

		// Token: 0x17001367 RID: 4967
		// (get) Token: 0x06007251 RID: 29265 RVA: 0x002807EE File Offset: 0x0027E9EE
		public List<IncidentDef> RecentRandomIncidents
		{
			get
			{
				return this.recentRandomIncidents;
			}
		}

		// Token: 0x17001368 RID: 4968
		// (get) Token: 0x06007252 RID: 29266 RVA: 0x002807F6 File Offset: 0x0027E9F6
		public int LastRoyalFavorQuestTick
		{
			get
			{
				return this.lastRoyalFavorQuestTick;
			}
		}

		// Token: 0x17001369 RID: 4969
		// (get) Token: 0x06007253 RID: 29267 RVA: 0x00280800 File Offset: 0x0027EA00
		public int LastThreatBigTick
		{
			get
			{
				if (this.lastThreatBigTick > Find.TickManager.TicksGame + 1000)
				{
					Log.Error(string.Concat(new string[]
					{
						"Latest big threat queue time was ",
						this.lastThreatBigTick.ToString(),
						" at tick ",
						Find.TickManager.TicksGame.ToString(),
						". This is too far in the future. Resetting."
					}));
					this.lastThreatBigTick = Find.TickManager.TicksGame - 1;
				}
				return this.lastThreatBigTick;
			}
		}

		// Token: 0x06007254 RID: 29268 RVA: 0x00280888 File Offset: 0x0027EA88
		public StoryState(IIncidentTarget target)
		{
			this.target = target;
		}

		// Token: 0x06007255 RID: 29269 RVA: 0x002808E8 File Offset: 0x0027EAE8
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastThreatBigTick, "lastThreatBigTick", 0, true);
			Scribe_Values.Look<int>(ref this.lastRoyalFavorQuestTick, "lastRoyalFavorQuestTick", 0, false);
			Scribe_References.Look<Faction>(ref this.lastRaidFaction, "lastRaidFaction", false);
			Scribe_Collections.Look<QuestScriptDef>(ref this.recentRandomQuests, "recentRandomQuests", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<QuestScriptDef>(ref this.recentRandomDecrees, "recentRandomDecrees", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<IncidentDef>(ref this.recentRandomIncidents, "recentRandomIncidents", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<int, int>(ref this.colonistCountTicks, "colonistCountTicks", LookMode.Value, LookMode.Value);
			Scribe_Collections.Look<IncidentDef, int>(ref this.lastFireTicks, "lastFireTicks", LookMode.Def, LookMode.Value);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.recentRandomQuests == null)
				{
					this.recentRandomQuests = new List<QuestScriptDef>();
				}
				if (this.recentRandomDecrees == null)
				{
					this.recentRandomDecrees = new List<QuestScriptDef>();
				}
				if (this.recentRandomIncidents == null)
				{
					this.recentRandomIncidents = new List<IncidentDef>();
				}
				if (this.colonistCountTicks == null)
				{
					this.colonistCountTicks = new Dictionary<int, int>();
				}
				this.RecordPopulationIncrease();
			}
		}

		// Token: 0x06007256 RID: 29270 RVA: 0x002809EC File Offset: 0x0027EBEC
		public void Notify_IncidentFired(FiringIncident fi)
		{
			if (fi.parms.forced || fi.parms.target != this.target)
			{
				return;
			}
			int ticksGame = Find.TickManager.TicksGame;
			if (fi.def.category == IncidentCategoryDefOf.ThreatBig)
			{
				this.lastThreatBigTick = ticksGame;
				Find.StoryWatcher.statsRecord.numThreatBigs++;
			}
			this.lastFireTicks[fi.def] = ticksGame;
			if (fi.def != IncidentDefOf.GiveQuest_Random)
			{
				if (!fi.def.ShouldIgnoreRecentWeighting)
				{
					this.RecordRandomIncidentFired(fi.def);
				}
				return;
			}
			if (fi.parms.questScriptDef == null)
			{
				Log.Error("Tried to record random quest fired with null questScript. parms=" + fi.parms.ToString());
				return;
			}
			this.RecordRandomQuestFired(fi.parms.questScriptDef);
		}

		// Token: 0x06007257 RID: 29271 RVA: 0x00280AC8 File Offset: 0x0027ECC8
		public void RecordRandomQuestFired(QuestScriptDef questScript)
		{
			this.recentRandomQuests.Insert(0, questScript);
			while (this.recentRandomQuests.Count > 5)
			{
				this.recentRandomQuests.RemoveAt(this.recentRandomQuests.Count - 1);
			}
			if (questScript.canGiveRoyalFavor)
			{
				this.lastRoyalFavorQuestTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06007258 RID: 29272 RVA: 0x00280B22 File Offset: 0x0027ED22
		public void RecordDecreeFired(QuestScriptDef questScript)
		{
			this.recentRandomDecrees.Insert(0, questScript);
			while (this.recentRandomDecrees.Count > 5)
			{
				this.recentRandomDecrees.RemoveAt(this.recentRandomDecrees.Count - 1);
			}
		}

		// Token: 0x06007259 RID: 29273 RVA: 0x00280B59 File Offset: 0x0027ED59
		private void RecordRandomIncidentFired(IncidentDef incident)
		{
			this.recentRandomIncidents.Insert(0, incident);
			while (this.recentRandomIncidents.Count > 5)
			{
				this.recentRandomIncidents.RemoveAt(this.recentRandomIncidents.Count - 1);
			}
		}

		// Token: 0x0600725A RID: 29274 RVA: 0x00280B90 File Offset: 0x0027ED90
		public void RecordPopulationIncrease()
		{
			int count = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count;
			if (!this.colonistCountTicks.ContainsKey(count))
			{
				this.colonistCountTicks.Add(count, Find.TickManager.TicksGame);
			}
		}

		// Token: 0x0600725B RID: 29275 RVA: 0x00280BCC File Offset: 0x0027EDCC
		public int GetTicksFromColonistCount(int count)
		{
			if (!this.colonistCountTicks.ContainsKey(count))
			{
				this.colonistCountTicks.Add(count, Find.TickManager.TicksGame);
			}
			return this.colonistCountTicks[count];
		}

		// Token: 0x0600725C RID: 29276 RVA: 0x00280C00 File Offset: 0x0027EE00
		public void CopyTo(StoryState other)
		{
			other.lastThreatBigTick = this.lastThreatBigTick;
			other.lastFireTicks.Clear();
			foreach (KeyValuePair<IncidentDef, int> keyValuePair in this.lastFireTicks)
			{
				other.lastFireTicks.Add(keyValuePair.Key, keyValuePair.Value);
			}
			other.RecentRandomQuests.Clear();
			other.RecentRandomQuests.AddRange(this.RecentRandomQuests);
			other.RecentRandomDecrees.Clear();
			other.RecentRandomDecrees.AddRange(this.RecentRandomDecrees);
			other.RecentRandomIncidents.Clear();
			other.RecentRandomIncidents.AddRange(this.RecentRandomIncidents);
			other.lastRoyalFavorQuestTick = this.lastRoyalFavorQuestTick;
			other.colonistCountTicks.Clear();
			foreach (KeyValuePair<int, int> keyValuePair2 in this.colonistCountTicks)
			{
				other.colonistCountTicks.Add(keyValuePair2.Key, keyValuePair2.Value);
			}
			other.lastRaidFaction = this.lastRaidFaction;
		}

		// Token: 0x04004852 RID: 18514
		private IIncidentTarget target;

		// Token: 0x04004853 RID: 18515
		private int lastThreatBigTick = -1;

		// Token: 0x04004854 RID: 18516
		private Dictionary<int, int> colonistCountTicks = new Dictionary<int, int>();

		// Token: 0x04004855 RID: 18517
		public Dictionary<IncidentDef, int> lastFireTicks = new Dictionary<IncidentDef, int>();

		// Token: 0x04004856 RID: 18518
		public Faction lastRaidFaction;

		// Token: 0x04004857 RID: 18519
		private int lastRoyalFavorQuestTick = -1;

		// Token: 0x04004858 RID: 18520
		private List<QuestScriptDef> recentRandomQuests = new List<QuestScriptDef>();

		// Token: 0x04004859 RID: 18521
		private List<QuestScriptDef> recentRandomDecrees = new List<QuestScriptDef>();

		// Token: 0x0400485A RID: 18522
		private List<IncidentDef> recentRandomIncidents = new List<IncidentDef>();

		// Token: 0x0400485B RID: 18523
		private const int RecentRandomQuestsMaxStorage = 5;
	}
}
