using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001045 RID: 4165
	public class QuestManager : IExposable
	{
		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x060068D9 RID: 26841 RVA: 0x00250BA4 File Offset: 0x0024EDA4
		public List<Quest> QuestsListForReading
		{
			get
			{
				return this.allQuests;
			}
		}

		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x060068DA RID: 26842 RVA: 0x00250BAC File Offset: 0x0024EDAC
		public List<Quest> ActiveQuestsListForReading
		{
			get
			{
				return this.activeQuests;
			}
		}

		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x060068DB RID: 26843 RVA: 0x00250BB4 File Offset: 0x0024EDB4
		public List<QuestPart_SituationalThought> SituationalThoughtQuestParts
		{
			get
			{
				return this.cachedSituationalThoughtQuestParts;
			}
		}

		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x060068DC RID: 26844 RVA: 0x00250BBC File Offset: 0x0024EDBC
		public List<QuestPart_ExtraFaction> ExtraFactionQuestParts
		{
			get
			{
				return this.cachedExtraFactionQuestParts;
			}
		}

		// Token: 0x060068DD RID: 26845 RVA: 0x00250BC4 File Offset: 0x0024EDC4
		public void Add(Quest quest)
		{
			if (quest == null)
			{
				Log.Error("Tried to add a null quest.");
				return;
			}
			if (this.Contains(quest))
			{
				Log.Error("Tried to add the same quest twice: " + quest.ToStringSafe<Quest>());
				return;
			}
			this.activeQuests.Add(quest);
			this.allQuests.Add(quest);
			this.AddToCache(quest);
			Find.SignalManager.RegisterReceiver(quest);
			List<QuestPart> partsListForReading = quest.PartsListForReading;
			for (int i = 0; i < partsListForReading.Count; i++)
			{
				partsListForReading[i].PostQuestAdded();
			}
			quest.PostAdded();
			if (quest.initiallyAccepted)
			{
				quest.Initiate();
			}
		}

		// Token: 0x060068DE RID: 26846 RVA: 0x00250C60 File Offset: 0x0024EE60
		public void Remove(Quest quest)
		{
			if (!this.Contains(quest))
			{
				Log.Error("Tried to remove non-existent quest: " + quest.ToStringSafe<Quest>());
				return;
			}
			this.allQuests.Remove(quest);
			this.activeQuests.Remove(quest);
			this.historicalQuests.Remove(quest);
			this.RemoveFromCache(quest);
			Find.SignalManager.DeregisterReceiver(quest);
		}

		// Token: 0x060068DF RID: 26847 RVA: 0x00250CC5 File Offset: 0x0024EEC5
		public bool Contains(Quest quest)
		{
			return this.allQuests.Contains(quest);
		}

		// Token: 0x060068E0 RID: 26848 RVA: 0x00250CD4 File Offset: 0x0024EED4
		public void QuestManagerTick()
		{
			for (int i = 0; i < this.historicalQuests.Count; i++)
			{
				this.historicalQuests[i].QuestTick();
			}
			for (int j = 0; j < this.activeQuests.Count; j++)
			{
				this.activeQuests[j].QuestTick();
				if (this.activeQuests[j].Historical)
				{
					this.historicalQuests.Add(this.activeQuests[j]);
					this.activeQuests[j] = null;
				}
			}
			this.activeQuests.RemoveAll((Quest quest) => quest == null);
		}

		// Token: 0x060068E1 RID: 26849 RVA: 0x00250D94 File Offset: 0x0024EF94
		public bool IsReservedByAnyQuest(Pawn p)
		{
			int count = this.activeQuests.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.activeQuests[i] != null && this.activeQuests[i].QuestReserves(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060068E2 RID: 26850 RVA: 0x00250DE0 File Offset: 0x0024EFE0
		public bool IsReservedByAnyQuest(Faction f)
		{
			for (int i = 0; i < this.activeQuests.Count; i++)
			{
				if (this.activeQuests[i] != null && this.activeQuests[i].QuestReserves(f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060068E3 RID: 26851 RVA: 0x00250E28 File Offset: 0x0024F028
		public bool IsReservedByAnyQuest(TransportShip ship)
		{
			for (int i = 0; i < this.activeQuests.Count; i++)
			{
				if (this.activeQuests[i] != null && this.activeQuests[i].QuestReserves(ship))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060068E4 RID: 26852 RVA: 0x00250E70 File Offset: 0x0024F070
		private void AddToCache(Quest quest)
		{
			this.questsInDisplayOrder.Add(quest);
			this.questsInDisplayOrder.SortBy((Quest x) => x.TicksSinceAppeared);
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_SituationalThought questPart_SituationalThought = quest.PartsListForReading[i] as QuestPart_SituationalThought;
				if (questPart_SituationalThought != null)
				{
					this.cachedSituationalThoughtQuestParts.Add(questPart_SituationalThought);
				}
				QuestPart_ExtraFaction questPart_ExtraFaction = quest.PartsListForReading[i] as QuestPart_ExtraFaction;
				if (questPart_ExtraFaction != null)
				{
					this.cachedExtraFactionQuestParts.Add(questPart_ExtraFaction);
				}
			}
		}

		// Token: 0x060068E5 RID: 26853 RVA: 0x00250F0C File Offset: 0x0024F10C
		private void RemoveFromCache(Quest quest)
		{
			this.questsInDisplayOrder.Remove(quest);
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_SituationalThought questPart_SituationalThought = quest.PartsListForReading[i] as QuestPart_SituationalThought;
				if (questPart_SituationalThought != null)
				{
					this.cachedSituationalThoughtQuestParts.Remove(questPart_SituationalThought);
				}
				QuestPart_ExtraFaction questPart_ExtraFaction = quest.PartsListForReading[i] as QuestPart_ExtraFaction;
				if (questPart_ExtraFaction != null)
				{
					this.cachedExtraFactionQuestParts.Remove(questPart_ExtraFaction);
				}
			}
		}

		// Token: 0x060068E6 RID: 26854 RVA: 0x00250F80 File Offset: 0x0024F180
		public void Notify_PawnDiscarded(Pawn pawn)
		{
			for (int i = 0; i < this.allQuests.Count; i++)
			{
				this.allQuests[i].Notify_PawnDiscarded(pawn);
			}
		}

		// Token: 0x060068E7 RID: 26855 RVA: 0x00250FB8 File Offset: 0x0024F1B8
		public void ExposeData()
		{
			Scribe_Collections.Look<Quest>(ref this.allQuests, "quests", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				int num = this.allQuests.RemoveAll((Quest x) => x == null);
				if (num != 0)
				{
					Log.Error(num.ToString() + " quest(s) were null after loading.");
				}
				int num2 = this.allQuests.RemoveAll((Quest q) => q.root == null);
				if (num2 != 0)
				{
					Log.Error(num2.ToString() + " quest(s) had null roots after loading.");
				}
				this.cachedExtraFactionQuestParts.Clear();
				this.cachedSituationalThoughtQuestParts.Clear();
				this.questsInDisplayOrder.Clear();
				for (int i = 0; i < this.allQuests.Count; i++)
				{
					this.AddToCache(this.allQuests[i]);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int j = 0; j < this.allQuests.Count; j++)
				{
					Find.SignalManager.RegisterReceiver(this.allQuests[j]);
				}
				this.activeQuests.Clear();
				this.historicalQuests.Clear();
				for (int k = 0; k < this.allQuests.Count; k++)
				{
					if (this.allQuests[k].Historical)
					{
						this.historicalQuests.Add(this.allQuests[k]);
					}
					else
					{
						this.activeQuests.Add(this.allQuests[k]);
					}
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060068E8 RID: 26856 RVA: 0x0025116C File Offset: 0x0024F36C
		public void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
			for (int i = 0; i < this.activeQuests.Count; i++)
			{
				if (this.activeQuests[i].State == QuestState.Ongoing)
				{
					this.activeQuests[i].Notify_ThingsProduced(worker, things);
				}
			}
		}

		// Token: 0x060068E9 RID: 26857 RVA: 0x002511B8 File Offset: 0x0024F3B8
		public void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
			for (int i = 0; i < this.activeQuests.Count; i++)
			{
				if (this.activeQuests[i].State == QuestState.Ongoing)
				{
					this.activeQuests[i].Notify_PlantHarvested(worker, harvested);
				}
			}
		}

		// Token: 0x060068EA RID: 26858 RVA: 0x00251204 File Offset: 0x0024F404
		public void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			for (int i = 0; i < this.activeQuests.Count; i++)
			{
				if (this.activeQuests[i].State == QuestState.Ongoing)
				{
					this.activeQuests[i].Notify_PawnKilled(pawn, dinfo);
				}
			}
		}

		// Token: 0x060068EB RID: 26859 RVA: 0x00251250 File Offset: 0x0024F450
		public void Notify_PawnBorn(Thing baby, Thing birther, Pawn mother, Pawn father)
		{
			for (int i = 0; i < this.activeQuests.Count; i++)
			{
				if (this.activeQuests[i].State == QuestState.Ongoing)
				{
					this.activeQuests[i].Notify_PawnBorn(baby, birther, mother, father);
				}
			}
		}

		// Token: 0x060068EC RID: 26860 RVA: 0x002512A0 File Offset: 0x0024F4A0
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.allQuests.Count; i++)
			{
				this.allQuests[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x040042C9 RID: 17097
		private List<Quest> historicalQuests = new List<Quest>();

		// Token: 0x040042CA RID: 17098
		private List<Quest> activeQuests = new List<Quest>();

		// Token: 0x040042CB RID: 17099
		private List<Quest> allQuests = new List<Quest>();

		// Token: 0x040042CC RID: 17100
		public List<Quest> questsInDisplayOrder = new List<Quest>();

		// Token: 0x040042CD RID: 17101
		private List<QuestPart_SituationalThought> cachedSituationalThoughtQuestParts = new List<QuestPart_SituationalThought>();

		// Token: 0x040042CE RID: 17102
		private List<QuestPart_ExtraFaction> cachedExtraFactionQuestParts = new List<QuestPart_ExtraFaction>();
	}
}
