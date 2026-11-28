using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B1 RID: 4273
	public class QuestPart_Choice : QuestPart
	{
		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x06006AE6 RID: 27366 RVA: 0x00258948 File Offset: 0x00256B48
		public override bool PreventsAutoAccept
		{
			get
			{
				return this.choices.Count >= 2;
			}
		}

		// Token: 0x06006AE7 RID: 27367 RVA: 0x0025895C File Offset: 0x00256B5C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalChoiceUsed)
			{
				this.choiceUsed = true;
				for (int i = 0; i < this.choices.Count; i++)
				{
					for (int j = 0; j < this.choices[i].rewards.Count; j++)
					{
						this.choices[i].rewards[j].Notify_Used();
					}
				}
			}
		}

		// Token: 0x06006AE8 RID: 27368 RVA: 0x002589E0 File Offset: 0x00256BE0
		public override void Notify_PreCleanup()
		{
			base.Notify_PreCleanup();
			for (int i = 0; i < this.choices.Count; i++)
			{
				for (int j = 0; j < this.choices[i].rewards.Count; j++)
				{
					this.choices[i].rewards[j].Notify_PreCleanup();
				}
			}
		}

		// Token: 0x06006AE9 RID: 27369 RVA: 0x00258A48 File Offset: 0x00256C48
		public override void Notify_PawnDiscarded(Pawn pawn)
		{
			foreach (QuestPart_Choice.Choice choice in this.choices)
			{
				for (int i = choice.rewards.Count - 1; i >= 0; i--)
				{
					Reward_Pawn reward_Pawn = choice.rewards[i] as Reward_Pawn;
					if (reward_Pawn != null && reward_Pawn.pawn == pawn)
					{
						choice.rewards.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x06006AEA RID: 27370 RVA: 0x00258AD8 File Offset: 0x00256CD8
		public void Choose(QuestPart_Choice.Choice choice)
		{
			for (int i = this.choices.Count - 1; i >= 0; i--)
			{
				if (this.choices[i] != choice)
				{
					for (int j = 0; j < this.choices[i].questParts.Count; j++)
					{
						if (!choice.questParts.Contains(this.choices[i].questParts[j]))
						{
							this.choices[i].questParts[j].Notify_PreCleanup();
							this.choices[i].questParts[j].Cleanup();
							this.quest.RemovePart(this.choices[i].questParts[j]);
						}
					}
					this.choices.RemoveAt(i);
				}
			}
		}

		// Token: 0x06006AEB RID: 27371 RVA: 0x00258BC8 File Offset: 0x00256DC8
		public override void PreQuestAccept()
		{
			base.PreQuestAccept();
			if (this.choices.Count >= 2)
			{
				Log.Error("Tried to accept a quest but " + base.GetType().Name + " still has a choice unresolved. Auto-choosing the first option.");
				this.Choose(this.choices[0]);
			}
		}

		// Token: 0x06006AEC RID: 27372 RVA: 0x00258C1C File Offset: 0x00256E1C
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			bool flag = false;
			for (int i = 0; i < this.choices.Count; i++)
			{
				for (int j = 0; j < this.choices[i].rewards.Count; j++)
				{
					Reward_Items reward_Items = this.choices[i].rewards[j] as Reward_Items;
					if (reward_Items != null)
					{
						for (int k = 0; k < reward_Items.items.Count; k++)
						{
							if (!flag && reward_Items.items[k].def == ThingDefOf.PsychicAmplifier)
							{
								Find.History.Notify_PsylinkAvailable();
								flag = true;
							}
							else if (ModsConfig.BiotechActive && !Find.History.mechlinkEverAvailable && reward_Items.items[k].def == ThingDefOf.Mechlink)
							{
								MechanitorUtility.Notify_MechlinkQuestRewardAvailable(this.quest, null);
							}
						}
					}
				}
			}
		}

		// Token: 0x06006AED RID: 27373 RVA: 0x00258D10 File Offset: 0x00256F10
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalChoiceUsed, "inSignalChoiceUsed", null, false);
			Scribe_Collections.Look<QuestPart_Choice.Choice>(ref this.choices, "choices", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.choiceUsed, "choiceUsed", false, false);
		}

		// Token: 0x040043F5 RID: 17397
		public string inSignalChoiceUsed;

		// Token: 0x040043F6 RID: 17398
		public List<QuestPart_Choice.Choice> choices = new List<QuestPart_Choice.Choice>();

		// Token: 0x040043F7 RID: 17399
		public bool choiceUsed;

		// Token: 0x02002FF4 RID: 12276
		public class Choice : IExposable
		{
			// Token: 0x060121E8 RID: 74216 RVA: 0x0062739F File Offset: 0x0062559F
			public void ExposeData()
			{
				Scribe_Collections.Look<QuestPart>(ref this.questParts, "questParts", LookMode.Reference, Array.Empty<object>());
				Scribe_Collections.Look<Reward>(ref this.rewards, "rewards", LookMode.Deep, Array.Empty<object>());
			}

			// Token: 0x0400C2D1 RID: 49873
			public List<QuestPart> questParts = new List<QuestPart>();

			// Token: 0x0400C2D2 RID: 49874
			public List<Reward> rewards = new List<Reward>();
		}
	}
}
