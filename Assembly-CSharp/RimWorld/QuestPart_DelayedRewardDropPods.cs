using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B3 RID: 4275
	public class QuestPart_DelayedRewardDropPods : QuestPart_AddQuest
	{
		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x06006AF6 RID: 27382 RVA: 0x00258EE3 File Offset: 0x002570E3
		public override QuestScriptDef QuestDef
		{
			get
			{
				return QuestScriptDefOf.DelayedRewardDropPods;
			}
		}

		// Token: 0x17001251 RID: 4689
		// (get) Token: 0x06006AF7 RID: 27383 RVA: 0x00258EEA File Offset: 0x002570EA
		public override bool CanAdd
		{
			get
			{
				return Rand.Chance(this.chance);
			}
		}

		// Token: 0x06006AF8 RID: 27384 RVA: 0x00258EF8 File Offset: 0x002570F8
		public override Slate GetSlate()
		{
			Slate slate = new Slate();
			slate.Set<List<Thing>>("rewards", this.rewards, false);
			slate.Set<Faction>("faction", this.faction, false);
			slate.Set<Pawn>("giver", this.giver, false);
			slate.Set<int>("delayTicks", this.delayTicks, false);
			slate.Set<string>("customLetterLabel", this.customLetterLabel, false);
			slate.Set<string>("customLetterText", this.customLetterText, false);
			return slate;
		}

		// Token: 0x06006AF9 RID: 27385 RVA: 0x00258F76 File Offset: 0x00257176
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06006AFA RID: 27386 RVA: 0x00258F88 File Offset: 0x00257188
		public override void PostAdd()
		{
			this.rewards.Clear();
		}

		// Token: 0x06006AFB RID: 27387 RVA: 0x00258F98 File Offset: 0x00257198
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.rewardThings.Clear();
				this.rewardPawns.Clear();
				foreach (Thing thing in this.rewards)
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						this.rewardPawns.Add(pawn);
					}
					else
					{
						this.rewardThings.Add(thing);
					}
				}
			}
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.giver, "giver", false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Values.Look<float>(ref this.chance, "chance", 0f, false);
			Scribe_Collections.Look<Pawn>(ref this.rewardPawns, "rewardPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.rewardThings, "rewardThings", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.rewardPawns.RemoveAll((Pawn x) => x == null);
				foreach (Pawn item in this.rewardPawns)
				{
					this.rewards.Add(item);
				}
				foreach (Thing item2 in this.rewardThings)
				{
					this.rewards.Add(item2);
				}
				this.rewardThings.Clear();
				this.rewardPawns.Clear();
			}
		}

		// Token: 0x040043FB RID: 17403
		public List<Thing> rewards = new List<Thing>();

		// Token: 0x040043FC RID: 17404
		public int delayTicks;

		// Token: 0x040043FD RID: 17405
		public string customLetterLabel;

		// Token: 0x040043FE RID: 17406
		public string customLetterText;

		// Token: 0x040043FF RID: 17407
		public Faction faction;

		// Token: 0x04004400 RID: 17408
		public Pawn giver;

		// Token: 0x04004401 RID: 17409
		public float chance = 1f;

		// Token: 0x04004402 RID: 17410
		private List<Thing> rewardThings = new List<Thing>();

		// Token: 0x04004403 RID: 17411
		private List<Pawn> rewardPawns = new List<Pawn>();
	}
}
