using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200235D RID: 9053
	public class DefeatAllEnemiesQuestComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x1700276D RID: 10093
		// (get) Token: 0x0600E05D RID: 57437 RVA: 0x005373D0 File Offset: 0x005355D0
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x0600E05E RID: 57438 RVA: 0x005373D8 File Offset: 0x005355D8
		public DefeatAllEnemiesQuestComp()
		{
			this.rewards = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600E05F RID: 57439 RVA: 0x005373EC File Offset: 0x005355EC
		public void StartQuest(Faction requestingFaction, int relationsImprovement, List<Thing> rewards)
		{
			this.StopQuest();
			this.active = true;
			this.requestingFaction = requestingFaction;
			this.relationsImprovement = relationsImprovement;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
			this.rewards.TryAddRangeOrTransfer(rewards, true, false);
		}

		// Token: 0x0600E060 RID: 57440 RVA: 0x00537423 File Offset: 0x00535623
		public void StopQuest()
		{
			this.active = false;
			this.requestingFaction = null;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x0600E061 RID: 57441 RVA: 0x00537440 File Offset: 0x00535640
		public override void CompTickInterval(int delta)
		{
			base.CompTickInterval(delta);
			if (this.active)
			{
				MapParent mapParent = this.parent as MapParent;
				if (mapParent != null)
				{
					this.CheckAllEnemiesDefeated(mapParent);
				}
			}
		}

		// Token: 0x0600E062 RID: 57442 RVA: 0x00537472 File Offset: 0x00535672
		private void CheckAllEnemiesDefeated(MapParent mapParent)
		{
			if (!mapParent.HasMap)
			{
				return;
			}
			if (GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map, true, false))
			{
				return;
			}
			this.GiveRewardsAndSendLetter();
			this.StopQuest();
		}

		// Token: 0x0600E063 RID: 57443 RVA: 0x0053749C File Offset: 0x0053569C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
			Scribe_Values.Look<int>(ref this.relationsImprovement, "relationsImprovement", 0, false);
			Scribe_References.Look<Faction>(ref this.requestingFaction, "requestingFaction", false);
			Scribe_Deep.Look<ThingOwner>(ref this.rewards, "rewards", new object[]
			{
				this
			});
		}

		// Token: 0x0600E064 RID: 57444 RVA: 0x00537500 File Offset: 0x00535700
		private void GiveRewardsAndSendLetter()
		{
			Map map = Find.AnyPlayerHomeMap ?? ((MapParent)this.parent).Map;
			DefeatAllEnemiesQuestComp.tmpRewards.AddRange(this.rewards);
			this.rewards.Clear();
			IntVec3 intVec = DropCellFinder.TradeDropSpot(map);
			DropPodUtility.DropThingsNear(intVec, map, DefeatAllEnemiesQuestComp.tmpRewards, 110, false, false, false, true, true, null);
			DefeatAllEnemiesQuestComp.tmpRewards.Clear();
			FactionRelationKind playerRelationKind = this.requestingFaction.PlayerRelationKind;
			TaggedString text = "LetterDefeatAllEnemiesQuestCompleted".Translate(this.requestingFaction.Name, this.relationsImprovement.ToString());
			Faction.OfPlayer.TryAffectGoodwillWith(this.requestingFaction, this.relationsImprovement, false, false, HistoryEventDefOf.QuestGoodwillReward, null);
			this.requestingFaction.TryAppendRelationKindChangedInfo(ref text, playerRelationKind, this.requestingFaction.PlayerRelationKind, null);
			Find.LetterStack.ReceiveLetter("LetterLabelDefeatAllEnemiesQuestCompleted".Translate(), text, LetterDefOf.PositiveEvent, new GlobalTargetInfo(intVec, map, false), this.requestingFaction, null, null, null, 0, true);
		}

		// Token: 0x0600E065 RID: 57445 RVA: 0x00537612 File Offset: 0x00535812
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600E066 RID: 57446 RVA: 0x00537620 File Offset: 0x00535820
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.rewards;
		}

		// Token: 0x0600E067 RID: 57447 RVA: 0x00537628 File Offset: 0x00535828
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x0600E068 RID: 57448 RVA: 0x0053763C File Offset: 0x0053583C
		public override string CompInspectStringExtra()
		{
			if (this.active)
			{
				string value = GenThing.ThingsToCommaList(this.rewards, true, true, 5).CapitalizeFirst();
				return "QuestTargetDestroyInspectString".Translate(this.requestingFaction.Name, value, GenThing.GetMarketValue(this.rewards).ToStringMoney(null)).CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x04009001 RID: 36865
		private bool active;

		// Token: 0x04009002 RID: 36866
		public Faction requestingFaction;

		// Token: 0x04009003 RID: 36867
		public int relationsImprovement;

		// Token: 0x04009004 RID: 36868
		public ThingOwner rewards;

		// Token: 0x04009005 RID: 36869
		private static readonly List<Thing> tmpRewards = new List<Thing>();
	}
}
