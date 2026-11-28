using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02002284 RID: 8836
	public abstract class QuestNode_Root_MysteriousCargo : QuestNode
	{
		// Token: 0x17002620 RID: 9760
		// (get) Token: 0x0600DA3D RID: 55869 RVA: 0x00511B26 File Offset: 0x0050FD26
		protected virtual bool RequiresPawn { get; } = 1;

		// Token: 0x0600DA3E RID: 55870 RVA: 0x00511B30 File Offset: 0x0050FD30
		protected override bool TestRunInt(Slate slate)
		{
			Map map = QuestGen_Get.GetMap(false, null, true);
			Pawn pawn;
			return map != null && (!this.RequiresPawn || QuestUtility.TryGetIdealColonist(out pawn, map, new Func<Pawn, bool>(this.ValidatePawn)));
		}

		// Token: 0x0600DA3F RID: 55871 RVA: 0x00511B74 File Offset: 0x0050FD74
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, true);
			float points = slate.Get<float>("points", 0f, false);
			slate.Set<Map>("map", map, false);
			Pawn asker = this.FindAsker();
			Pawn pawn = null;
			if (this.RequiresPawn && !QuestUtility.TryGetIdealColonist(out pawn, map, new Func<Pawn, bool>(this.ValidatePawn)))
			{
				Log.ErrorOnce("Attempted to create a mysterious cargo quest but no valid pawns or world pawns could be found", 94657346);
				quest.End(QuestEndOutcome.InvalidPreAcceptance, true, true);
			}
			Thing thing = this.GenerateThing(pawn);
			quest.Delay(120, delegate
			{
				quest.DropPods(map.Parent, new List<Thing>
				{
					thing
				}, "[deliveredLetterLabel]", null, "[deliveredLetterText]", null, new bool?(true), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true, true, false, true, null);
				this.AddPostDroppedQuestParts(pawn, thing, quest);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.Delay(Mathf.RoundToInt(QuestNode_Root_MysteriousCargo.DelayHours.RandomInRange * 2500f), delegate
			{
				Quest quest = quest;
				RewardsGeneratorParams parms = new RewardsGeneratorParams
				{
					rewardValue = QuestNode_Root_MysteriousCargo.RewardValueCurve.Evaluate(points),
					thingRewardItemsOnly = true
				};
				string inSignal = null;
				string customLetterLabel = null;
				string customLetterText = null;
				RulePack customLetterLabelRules = null;
				RulePack customLetterTextRules = null;
				Pawn asker = asker;
				quest.GiveRewards(parms, inSignal, customLetterLabel, customLetterText, customLetterLabelRules, customLetterTextRules, null, null, null, false, asker, false, false, null);
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			slate.Set<Pawn>("asker", asker, false);
			slate.Set<bool>("askerIsNull", asker == null, false);
			slate.Set<Pawn>("pawn", pawn, false);
			Slate slate2 = slate;
			string name = "pawnOnMap";
			Pawn pawn2 = pawn;
			slate2.Set<bool>(name, ((pawn2 != null) ? pawn2.MapHeld : null) == map, false);
		}

		// Token: 0x0600DA40 RID: 55872
		protected abstract Thing GenerateThing(Pawn pawn);

		// Token: 0x0600DA41 RID: 55873 RVA: 0x00511D0B File Offset: 0x0050FF0B
		protected virtual void AddPostDroppedQuestParts(Pawn pawn, Thing thing, Quest quest)
		{
		}

		// Token: 0x0600DA42 RID: 55874 RVA: 0x00511D0D File Offset: 0x0050FF0D
		protected virtual bool ValidatePawn(Pawn pawn)
		{
			return pawn.IsColonist || pawn.IsSlaveOfColony;
		}

		// Token: 0x0600DA43 RID: 55875 RVA: 0x00511D20 File Offset: 0x0050FF20
		private Pawn FindAsker()
		{
			Faction faction;
			if ((from f in Find.FactionManager.AllFactionsVisible
			where f.def.humanlikeFaction && !f.IsPlayer && !f.HostileTo(Faction.OfPlayer) && f.def.techLevel > TechLevel.Neolithic && f.leader != null && !f.temporary && !f.Hidden
			select f).TryRandomElement(out faction))
			{
				return faction.leader;
			}
			return null;
		}

		// Token: 0x04008B82 RID: 35714
		private static readonly FloatRange DelayHours = new FloatRange(1f, 3f);

		// Token: 0x04008B83 RID: 35715
		private static readonly SimpleCurve RewardValueCurve = new SimpleCurve
		{
			{
				new CurvePoint(500f, 800f),
				true
			},
			{
				new CurvePoint(3000f, 1200f),
				true
			}
		};
	}
}
