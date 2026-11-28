using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021BE RID: 8638
	public class QuestNode_GetFaction : QuestNode
	{
		// Token: 0x0600D71E RID: 55070 RVA: 0x004FD3D8 File Offset: 0x004FB5D8
		protected override bool TestRunInt(Slate slate)
		{
			Faction faction;
			if (slate.TryGet<Faction>(this.storeAs.GetValue(slate), out faction, false) && this.IsGoodFaction(faction, slate))
			{
				return true;
			}
			if (this.TryFindFaction(out faction, slate))
			{
				slate.Set<Faction>(this.storeAs.GetValue(slate), faction, false);
				return true;
			}
			return false;
		}

		// Token: 0x0600D71F RID: 55071 RVA: 0x004FD42C File Offset: 0x004FB62C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Faction faction;
			if (QuestGen.slate.TryGet<Faction>(this.storeAs.GetValue(slate), out faction, false) && this.IsGoodFaction(faction, QuestGen.slate))
			{
				return;
			}
			if (!this.TryFindFaction(out faction, QuestGen.slate))
			{
				return;
			}
			QuestGen.slate.Set<Faction>(this.storeAs.GetValue(slate), faction, false);
			if (!faction.Hidden)
			{
				QuestPart_InvolvedFactions questPart_InvolvedFactions = new QuestPart_InvolvedFactions();
				questPart_InvolvedFactions.factions.Add(faction);
				QuestGen.quest.AddPart(questPart_InvolvedFactions);
			}
		}

		// Token: 0x0600D720 RID: 55072 RVA: 0x004FD4B8 File Offset: 0x004FB6B8
		private bool TryFindFaction(out Faction faction, Slate slate)
		{
			return (from x in Find.FactionManager.GetFactions(true, false, true, TechLevel.Undefined, false)
			where this.IsGoodFaction(x, slate)
			select x).TryRandomElement(out faction);
		}

		// Token: 0x0600D721 RID: 55073 RVA: 0x004FD500 File Offset: 0x004FB700
		private bool IsGoodFaction(Faction faction, Slate slate)
		{
			if (faction.Hidden && (this.allowedHiddenFactions.GetValue(slate) == null || !this.allowedHiddenFactions.GetValue(slate).Contains(faction)))
			{
				return false;
			}
			if (this.ofPawn.GetValue(slate) != null && faction != this.ofPawn.GetValue(slate).Faction)
			{
				return false;
			}
			if (this.exclude.GetValue(slate) != null && this.exclude.GetValue(slate).Contains(faction))
			{
				return false;
			}
			if (this.mustBePermanentEnemy.GetValue(slate) && !faction.def.permanentEnemy)
			{
				return false;
			}
			if (!this.allowEnemy.GetValue(slate) && faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			if (!this.allowNeutral.GetValue(slate) && faction.PlayerRelationKind == FactionRelationKind.Neutral)
			{
				return false;
			}
			if (!this.allowAlly.GetValue(slate) && faction.PlayerRelationKind == FactionRelationKind.Ally)
			{
				return false;
			}
			if (!this.allowPermanentEnemy.GetValue(slate).GetValueOrDefault(true) && faction.def.permanentEnemy)
			{
				return false;
			}
			if (this.playerCantBeAttackingCurrently.GetValue(slate) && SettlementUtility.IsPlayerAttackingAnySettlementOf(faction))
			{
				return false;
			}
			if (this.mustHaveGoodwillRewardsEnabled.GetValue(slate) && !faction.allowGoodwillRewards)
			{
				return false;
			}
			if (this.peaceTalksCantExist.GetValue(slate))
			{
				if (this.PeaceTalksExist(faction))
				{
					return false;
				}
				string tag = QuestNode_QuestUnique.GetProcessedTag("PeaceTalks", faction);
				if (Find.QuestManager.questsInDisplayOrder.Any((Quest q) => q.tags.Contains(tag)))
				{
					return false;
				}
			}
			if (this.leaderMustBeSafe.GetValue(slate) && (faction.leader == null || faction.leader.Spawned || faction.leader.IsPrisoner))
			{
				return false;
			}
			Thing value = this.mustBeHostileToFactionOf.GetValue(slate);
			return value == null || value.Faction == null || (value.Faction != faction && faction.HostileTo(value.Faction));
		}

		// Token: 0x0600D722 RID: 55074 RVA: 0x004FD6F8 File Offset: 0x004FB8F8
		private bool PeaceTalksExist(Faction faction)
		{
			List<PeaceTalks> peaceTalks = Find.WorldObjects.PeaceTalks;
			for (int i = 0; i < peaceTalks.Count; i++)
			{
				if (peaceTalks[i].Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008875 RID: 34933
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008876 RID: 34934
		public SlateRef<bool> allowEnemy;

		// Token: 0x04008877 RID: 34935
		public SlateRef<bool> allowNeutral;

		// Token: 0x04008878 RID: 34936
		public SlateRef<bool> allowAlly;

		// Token: 0x04008879 RID: 34937
		public SlateRef<bool> allowAskerFaction;

		// Token: 0x0400887A RID: 34938
		public SlateRef<bool?> allowPermanentEnemy;

		// Token: 0x0400887B RID: 34939
		public SlateRef<bool> mustBePermanentEnemy;

		// Token: 0x0400887C RID: 34940
		public SlateRef<bool> playerCantBeAttackingCurrently;

		// Token: 0x0400887D RID: 34941
		public SlateRef<bool> peaceTalksCantExist;

		// Token: 0x0400887E RID: 34942
		public SlateRef<bool> leaderMustBeSafe;

		// Token: 0x0400887F RID: 34943
		public SlateRef<bool> mustHaveGoodwillRewardsEnabled;

		// Token: 0x04008880 RID: 34944
		public SlateRef<Pawn> ofPawn;

		// Token: 0x04008881 RID: 34945
		public SlateRef<Thing> mustBeHostileToFactionOf;

		// Token: 0x04008882 RID: 34946
		public SlateRef<IEnumerable<Faction>> exclude;

		// Token: 0x04008883 RID: 34947
		public SlateRef<IEnumerable<Faction>> allowedHiddenFactions;
	}
}
