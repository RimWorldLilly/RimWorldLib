using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001121 RID: 4385
	public static class QuestUtility
	{
		// Token: 0x06006DA4 RID: 28068 RVA: 0x00266C74 File Offset: 0x00264E74
		public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, float points)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			return QuestUtility.GenerateQuestAndMakeAvailable(root, slate);
		}

		// Token: 0x06006DA5 RID: 28069 RVA: 0x00266C9C File Offset: 0x00264E9C
		public static Quest GenerateQuestAndMakeAvailable(QuestScriptDef root, Slate vars)
		{
			Quest quest = QuestGen.Generate(root, vars);
			Find.QuestManager.Add(quest);
			return quest;
		}

		// Token: 0x06006DA6 RID: 28070 RVA: 0x00266CC0 File Offset: 0x00264EC0
		public static void SendLetterQuestAvailable(Quest quest, string discoveryMethod = null)
		{
			TaggedString label = QuestUtility.QuestAvailableLetterLabel(quest);
			TaggedString text = QuestUtility.QuestAvailableLetterText(quest, discoveryMethod);
			QuestScriptDef root = quest.root;
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, ((root != null) ? root.questAvailableLetterDef : null) ?? IncidentDefOf.GiveQuest_Random.letterDef, LookTargets.Invalid, null, quest, null);
			choiceLetter.title = quest.name;
			Find.LetterStack.ReceiveLetter(choiceLetter, null, 0, true);
		}

		// Token: 0x06006DA7 RID: 28071 RVA: 0x00266D24 File Offset: 0x00264F24
		private static TaggedString QuestAvailableLetterLabel(Quest quest)
		{
			if (quest.root != null && !quest.root.questAvailableLetterLabel.NullOrEmpty())
			{
				return quest.root.questAvailableLetterLabel;
			}
			if (quest.initiallyAccepted)
			{
				return "LetterLabelQuestAutomaticallyAcceptedTitle".Translate(quest.name);
			}
			return "LetterLabelQuestAvailableTitle".Translate(quest.name);
		}

		// Token: 0x06006DA8 RID: 28072 RVA: 0x00266D90 File Offset: 0x00264F90
		private static TaggedString QuestAvailableLetterText(Quest quest, string discoveryMethod = null)
		{
			TaggedString taggedString;
			if (quest.root != null && quest.root.questAvailableLetterTextIsDescription)
			{
				taggedString = quest.description;
			}
			else
			{
				if (!discoveryMethod.NullOrEmpty())
				{
					taggedString = "LetterNewQuest".Translate(discoveryMethod.Named("DISCOVEREDFROM"));
				}
				else
				{
					taggedString = "LetterNewQuestFromUnknown".Translate();
				}
				taggedString += "\n\n" + "LetterQuestIsNamed".Translate(quest.name);
			}
			if (quest.initiallyAccepted)
			{
				int questTicksRemaining = QuestUtility.GetQuestTicksRemaining(quest);
				if (questTicksRemaining > 0)
				{
					taggedString += "\n\n" + "LetterQuestActiveNowTime".Translate(questTicksRemaining.ToStringTicksToPeriod(false, false, false, true, false));
				}
			}
			else if (quest.TicksUntilExpiry >= 0)
			{
				taggedString += "\n\n" + "LetterQuestRequiresAcceptance".Translate(quest.TicksUntilExpiry.ToStringTicksToPeriod(false, false, false, true, false));
			}
			return taggedString;
		}

		// Token: 0x06006DA9 RID: 28073 RVA: 0x00266E88 File Offset: 0x00265088
		public static int GetQuestTicksRemaining(Quest quest)
		{
			foreach (QuestPart questPart in quest.PartsListForReading)
			{
				QuestPart_Delay questPart_Delay = questPart as QuestPart_Delay;
				if (questPart_Delay != null && questPart_Delay.State == QuestPartState.Enabled && questPart_Delay.isBad && !questPart_Delay.expiryInfoPart.NullOrEmpty())
				{
					return questPart_Delay.TicksLeft;
				}
			}
			return 0;
		}

		// Token: 0x06006DAA RID: 28074 RVA: 0x00266F08 File Offset: 0x00265108
		public static void GenerateBackCompatibilityNameFor(Quest quest)
		{
			quest.name = NameGenerator.GenerateName(RulePackDefOf.NamerQuestDefault, from x in Find.QuestManager.QuestsListForReading
			select x.name, false, "defaultQuestName");
		}

		// Token: 0x06006DAB RID: 28075 RVA: 0x00266F5C File Offset: 0x0026515C
		public static bool CanPawnAcceptQuest(Pawn p, Quest quest)
		{
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_RequirementsToAccept questPart_RequirementsToAccept = quest.PartsListForReading[i] as QuestPart_RequirementsToAccept;
				if (questPart_RequirementsToAccept != null && !questPart_RequirementsToAccept.CanPawnAccept(p))
				{
					return false;
				}
			}
			return !p.Destroyed && p.IsFreeColonist && !p.Downed && !p.Suspended && !p.IsQuestLodger();
		}

		// Token: 0x06006DAC RID: 28076 RVA: 0x00266FCC File Offset: 0x002651CC
		public static AcceptanceReport CanAcceptQuest(Quest quest)
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return false;
			}
			if (!Current.Game.PlayerHasControl)
			{
				return false;
			}
			for (int i = 0; i < quest.PartsListForReading.Count; i++)
			{
				QuestPart_RequirementsToAccept questPart_RequirementsToAccept = quest.PartsListForReading[i] as QuestPart_RequirementsToAccept;
				if (questPart_RequirementsToAccept != null)
				{
					AcceptanceReport result = questPart_RequirementsToAccept.CanAccept();
					if (!result.Accepted)
					{
						return result;
					}
				}
			}
			return true;
		}

		// Token: 0x06006DAD RID: 28077 RVA: 0x00267040 File Offset: 0x00265240
		public static Vector2 GetLocForDates()
		{
			if (Find.AnyPlayerHomeMap != null)
			{
				return Find.WorldGrid.LongLatOf(Find.AnyPlayerHomeMap.Tile);
			}
			return default(Vector2);
		}

		// Token: 0x06006DAE RID: 28078 RVA: 0x00267074 File Offset: 0x00265274
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, default(SignalArgs));
		}

		// Token: 0x06006DAF RID: 28079 RVA: 0x00267091 File Offset: 0x00265291
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1));
		}

		// Token: 0x06006DB0 RID: 28080 RVA: 0x002670A0 File Offset: 0x002652A0
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2));
		}

		// Token: 0x06006DB1 RID: 28081 RVA: 0x002670B0 File Offset: 0x002652B0
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3));
		}

		// Token: 0x06006DB2 RID: 28082 RVA: 0x002670C2 File Offset: 0x002652C2
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(arg1, arg2, arg3, arg4));
		}

		// Token: 0x06006DB3 RID: 28083 RVA: 0x002670D6 File Offset: 0x002652D6
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, NamedArgument[] args)
		{
			QuestUtility.SendQuestTargetSignals(questTags, signalPart, new SignalArgs(args));
		}

		// Token: 0x06006DB4 RID: 28084 RVA: 0x002670E8 File Offset: 0x002652E8
		public static void SendQuestTargetSignals(List<string> questTags, string signalPart, SignalArgs args)
		{
			if (questTags == null)
			{
				return;
			}
			for (int i = 0; i < questTags.Count; i++)
			{
				Find.SignalManager.SendSignal(new Signal(questTags[i] + "." + signalPart, args, false));
			}
		}

		// Token: 0x06006DB5 RID: 28085 RVA: 0x0026712D File Offset: 0x0026532D
		public static void AddQuestTag(ref List<string> questTags, string questTagToAdd)
		{
			if (questTags == null)
			{
				questTags = new List<string>();
			}
			if (questTags.Contains(questTagToAdd))
			{
				return;
			}
			questTags.Add(questTagToAdd);
		}

		// Token: 0x06006DB6 RID: 28086 RVA: 0x00267150 File Offset: 0x00265350
		public static void AddQuestTag(object obj, string questTagToAdd)
		{
			if (questTagToAdd.NullOrEmpty())
			{
				return;
			}
			Thing thing = obj as Thing;
			if (thing != null)
			{
				QuestUtility.AddQuestTag(ref thing.questTags, questTagToAdd);
				return;
			}
			WorldObject worldObject = obj as WorldObject;
			if (worldObject != null)
			{
				QuestUtility.AddQuestTag(ref worldObject.questTags, questTagToAdd);
				return;
			}
			Map map = obj as Map;
			if (map != null)
			{
				QuestUtility.AddQuestTag(ref map.Parent.questTags, questTagToAdd);
				return;
			}
			Lord lord = obj as Lord;
			if (lord != null)
			{
				QuestUtility.AddQuestTag(ref lord.questTags, questTagToAdd);
				return;
			}
			Faction faction = obj as Faction;
			if (faction != null)
			{
				QuestUtility.AddQuestTag(ref faction.questTags, questTagToAdd);
				return;
			}
			TransportShip transportShip = obj as TransportShip;
			if (transportShip != null)
			{
				QuestUtility.AddQuestTag(ref transportShip.questTags, questTagToAdd);
				return;
			}
			IEnumerable enumerable = obj as IEnumerable;
			if (enumerable != null)
			{
				foreach (object obj2 in enumerable)
				{
					Thing thing2 = obj2 as Thing;
					if (thing2 != null)
					{
						QuestUtility.AddQuestTag(ref thing2.questTags, questTagToAdd);
					}
					else
					{
						WorldObject worldObject2 = obj2 as WorldObject;
						if (worldObject2 != null)
						{
							QuestUtility.AddQuestTag(ref worldObject2.questTags, questTagToAdd);
						}
						else
						{
							Map map2 = obj2 as Map;
							if (map2 != null)
							{
								QuestUtility.AddQuestTag(ref map2.Parent.questTags, questTagToAdd);
							}
							else
							{
								Faction faction2 = obj2 as Faction;
								if (faction2 != null)
								{
									QuestUtility.AddQuestTag(ref faction2.questTags, questTagToAdd);
								}
								else
								{
									TransportShip transportShip2 = enumerable as TransportShip;
									if (transportShip2 != null)
									{
										QuestUtility.AddQuestTag(ref transportShip2.questTags, questTagToAdd);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06006DB7 RID: 28087 RVA: 0x002672E4 File Offset: 0x002654E4
		public static bool TryGetIdealColonist(out Pawn pawn, Map idealMap = null, Func<Pawn, bool> validator = null)
		{
			QuestUtility.tmpPawns.Clear();
			if (idealMap != null)
			{
				for (int i = 0; i <= 3; i++)
				{
					QuestUtility.CacheIdealColonists(idealMap.mapPawns.AllHumanlikeSpawned, i, QuestUtility.tmpPawns, validator);
					if (!QuestUtility.tmpPawns.Empty<Pawn>())
					{
						pawn = QuestUtility.tmpPawns.RandomElement<Pawn>();
						QuestUtility.tmpPawns.Clear();
						return true;
					}
				}
			}
			for (int j = 0; j <= 3; j++)
			{
				foreach (Map map in Find.Maps)
				{
					if (idealMap == null || idealMap != map)
					{
						QuestUtility.CacheIdealColonists(map.mapPawns.AllHumanlikeSpawned, j, QuestUtility.tmpPawns, validator);
					}
				}
				if (!QuestUtility.tmpPawns.Empty<Pawn>())
				{
					pawn = QuestUtility.tmpPawns.RandomElement<Pawn>();
					QuestUtility.tmpPawns.Clear();
					return true;
				}
			}
			for (int k = 0; k <= 3; k++)
			{
				QuestUtility.CacheIdealColonists(Find.WorldPawns.AllPawnsAlive, k, QuestUtility.tmpPawns, validator);
				if (!QuestUtility.tmpPawns.Empty<Pawn>())
				{
					pawn = QuestUtility.tmpPawns.RandomElement<Pawn>();
					QuestUtility.tmpPawns.Clear();
					return true;
				}
			}
			QuestUtility.tmpPawns.Clear();
			pawn = null;
			return false;
		}

		// Token: 0x06006DB8 RID: 28088 RVA: 0x0026742C File Offset: 0x0026562C
		private static void CacheIdealColonists(List<Pawn> pawnsToScan, int level, List<Pawn> pawns, Func<Pawn, bool> validator = null)
		{
			foreach (Pawn pawn in pawnsToScan)
			{
				if (!pawn.DevelopmentalStage.Baby() && (validator == null || validator(pawn)) && (pawn.IsColonistPlayerControlled || (pawn.IsColonist && pawn.IsCaravanMember())) && (level > 2 || pawn.IsSlaveOfColony || pawn.IsColonist) && (level > 1 || pawn.ageTracker.AgeBiologicalYears >= 13) && (level > 0 || pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving)))
				{
					pawns.Add(pawn);
				}
			}
		}

		// Token: 0x06006DB9 RID: 28089 RVA: 0x002674F4 File Offset: 0x002656F4
		public static bool AnyMatchingTags(List<string> first, List<string> second)
		{
			if (first.NullOrEmpty<string>() || second.NullOrEmpty<string>())
			{
				return false;
			}
			for (int i = 0; i < first.Count; i++)
			{
				for (int j = 0; j < second.Count; j++)
				{
					if (first[i] == second[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006DBA RID: 28090 RVA: 0x0026754D File Offset: 0x0026574D
		public static bool IsReservedByQuestOrQuestBeingGenerated(Pawn pawn)
		{
			return Find.QuestManager.IsReservedByAnyQuest(pawn) || (QuestGen.quest != null && QuestGen.quest.QuestReserves(pawn)) || QuestGen.WasGeneratedForQuestBeingGenerated(pawn);
		}

		// Token: 0x06006DBB RID: 28091 RVA: 0x00267578 File Offset: 0x00265778
		public static List<QuestPart_WorkDisabled> GetWorkDisabledQuestPart(Pawn p)
		{
			QuestUtility.tmpQuestWorkDisabled.Clear();
			List<Quest> activeQuestsListForReading = Find.QuestManager.ActiveQuestsListForReading;
			for (int i = 0; i < activeQuestsListForReading.Count; i++)
			{
				if (activeQuestsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = activeQuestsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_WorkDisabled questPart_WorkDisabled = partsListForReading[j] as QuestPart_WorkDisabled;
						if (questPart_WorkDisabled != null && questPart_WorkDisabled.pawns.Contains(p))
						{
							QuestUtility.tmpQuestWorkDisabled.Add(questPart_WorkDisabled);
						}
					}
				}
			}
			return QuestUtility.tmpQuestWorkDisabled;
		}

		// Token: 0x06006DBC RID: 28092 RVA: 0x0026760A File Offset: 0x0026580A
		public static bool IsQuestLodger(this Pawn p)
		{
			return p.HasExtraHomeFaction(null) || p.HasExtraMiniFaction(null);
		}

		// Token: 0x06006DBD RID: 28093 RVA: 0x00267620 File Offset: 0x00265820
		public static bool IsQuestHelper(this Pawn p)
		{
			if (!p.IsQuestLodger())
			{
				return false;
			}
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_ExtraFaction questPart_ExtraFaction = partsListForReading[j] as QuestPart_ExtraFaction;
						if (questPart_ExtraFaction != null && questPart_ExtraFaction.affectedPawns.Contains(p) && questPart_ExtraFaction.areHelpers)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06006DBE RID: 28094 RVA: 0x002676B0 File Offset: 0x002658B0
		public static bool IsQuestReward(this Pawn pawn)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if ((questsListForReading[i].State == QuestState.NotYetAccepted || questsListForReading[i].State == QuestState.Ongoing) && pawn.IsQuestReward(questsListForReading[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006DBF RID: 28095 RVA: 0x00267708 File Offset: 0x00265908
		public static bool IsQuestReward(this Pawn pawn, Quest quest)
		{
			List<QuestPart> partsListForReading = quest.PartsListForReading;
			for (int i = 0; i < partsListForReading.Count; i++)
			{
				QuestPart_Choice questPart_Choice = partsListForReading[i] as QuestPart_Choice;
				if (questPart_Choice != null)
				{
					for (int j = 0; j < questPart_Choice.choices.Count; j++)
					{
						QuestPart_Choice.Choice choice = questPart_Choice.choices[j];
						for (int k = 0; k < choice.rewards.Count; k++)
						{
							Reward_Pawn reward_Pawn = choice.rewards[k] as Reward_Pawn;
							if (reward_Pawn != null && reward_Pawn.pawn == pawn)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06006DC0 RID: 28096 RVA: 0x002677A8 File Offset: 0x002659A8
		public static bool LodgerAllowedDecrees(this Pawn p)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_AllowDecreesForLodger questPart_AllowDecreesForLodger = partsListForReading[j] as QuestPart_AllowDecreesForLodger;
						if (questPart_AllowDecreesForLodger != null && questPart_AllowDecreesForLodger.lodger == p)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06006DC1 RID: 28097 RVA: 0x0026781D File Offset: 0x00265A1D
		public static bool HasExtraHomeFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraHomeFaction(forQuest) != null;
		}

		// Token: 0x06006DC2 RID: 28098 RVA: 0x00267829 File Offset: 0x00265A29
		public static bool HasExtraMiniFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraMiniFaction(forQuest) != null;
		}

		// Token: 0x06006DC3 RID: 28099 RVA: 0x00267838 File Offset: 0x00265A38
		public static bool HasExtraHomeFaction(this Pawn p, Faction faction)
		{
			QuestUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(p, QuestUtility.tmpExtraFactions, null);
			for (int i = 0; i < QuestUtility.tmpExtraFactions.Count; i++)
			{
				if (QuestUtility.tmpExtraFactions[i].factionType == ExtraFactionType.HomeFaction && QuestUtility.tmpExtraFactions[i].faction == faction)
				{
					QuestUtility.tmpExtraFactions.Clear();
					return true;
				}
			}
			QuestUtility.tmpExtraFactions.Clear();
			return false;
		}

		// Token: 0x06006DC4 RID: 28100 RVA: 0x002678AC File Offset: 0x00265AAC
		public static bool HasExtraMiniFaction(this Pawn p, Faction faction)
		{
			QuestUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(p, QuestUtility.tmpExtraFactions, null);
			for (int i = 0; i < QuestUtility.tmpExtraFactions.Count; i++)
			{
				if (QuestUtility.tmpExtraFactions[i].factionType == ExtraFactionType.MiniFaction && QuestUtility.tmpExtraFactions[i].faction == faction)
				{
					QuestUtility.tmpExtraFactions.Clear();
					return true;
				}
			}
			QuestUtility.tmpExtraFactions.Clear();
			return false;
		}

		// Token: 0x06006DC5 RID: 28101 RVA: 0x00267921 File Offset: 0x00265B21
		public static Faction GetExtraHomeFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.HomeFaction, forQuest);
		}

		// Token: 0x06006DC6 RID: 28102 RVA: 0x0026792B File Offset: 0x00265B2B
		public static Faction GetExtraHostFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.HostFaction, forQuest);
		}

		// Token: 0x06006DC7 RID: 28103 RVA: 0x00267935 File Offset: 0x00265B35
		public static Faction GetExtraMiniFaction(this Pawn p, Quest forQuest = null)
		{
			return p.GetExtraFaction(ExtraFactionType.MiniFaction, forQuest);
		}

		// Token: 0x06006DC8 RID: 28104 RVA: 0x0026793F File Offset: 0x00265B3F
		public static bool InSameExtraFaction(this Pawn p, Pawn target, ExtraFactionType type, Quest forQuest = null)
		{
			return p.GetSharedExtraFaction(target, type, forQuest) != null;
		}

		// Token: 0x06006DC9 RID: 28105 RVA: 0x00267950 File Offset: 0x00265B50
		public static Faction GetSharedExtraFaction(this Pawn p, Pawn target, ExtraFactionType type, Quest forQuest = null)
		{
			Faction extraFaction = p.GetExtraFaction(type, forQuest);
			Faction extraFaction2 = target.GetExtraFaction(type, forQuest);
			if (extraFaction != null && extraFaction == extraFaction2)
			{
				return extraFaction;
			}
			return null;
		}

		// Token: 0x06006DCA RID: 28106 RVA: 0x0026797C File Offset: 0x00265B7C
		public static Faction GetExtraFaction(this Pawn p, ExtraFactionType extraFactionType, Quest forQuest = null)
		{
			QuestUtility.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(p, QuestUtility.tmpExtraFactions, forQuest);
			for (int i = 0; i < QuestUtility.tmpExtraFactions.Count; i++)
			{
				if (QuestUtility.tmpExtraFactions[i].factionType == extraFactionType)
				{
					Faction faction = QuestUtility.tmpExtraFactions[i].faction;
					QuestUtility.tmpExtraFactions.Clear();
					return faction;
				}
			}
			QuestUtility.tmpExtraFactions.Clear();
			return null;
		}

		// Token: 0x06006DCB RID: 28107 RVA: 0x002679F0 File Offset: 0x00265BF0
		public static void GetExtraFactionsFromQuestParts(Pawn pawn, List<ExtraFaction> outExtraFactions, Quest forQuest = null)
		{
			QuestUtility.<>c__DisplayClass117_0 CS$<>8__locals1;
			CS$<>8__locals1.outExtraFactions = outExtraFactions;
			CS$<>8__locals1.outExtraFactions.Clear();
			List<QuestPart_ExtraFaction> extraFactionQuestParts = Find.QuestManager.ExtraFactionQuestParts;
			for (int i = 0; i < extraFactionQuestParts.Count; i++)
			{
				ExtraFaction extraFaction = extraFactionQuestParts[i].extraFaction;
				if (((extraFaction != null) ? extraFaction.faction : null) != null)
				{
					Quest quest = extraFactionQuestParts[i].quest;
					if ((quest.State == QuestState.Ongoing || quest == forQuest || (quest.root == QuestScriptDefOf.Hospitality_Refugee && !pawn.IsColonist)) && !QuestUtility.<GetExtraFactionsFromQuestParts>g__AlreadyAdded|117_0(extraFactionQuestParts[i].extraFaction, ref CS$<>8__locals1) && extraFactionQuestParts[i].affectedPawns.Contains(pawn))
					{
						CS$<>8__locals1.outExtraFactions.Add(extraFactionQuestParts[i].extraFaction);
					}
				}
			}
		}

		// Token: 0x06006DCC RID: 28108 RVA: 0x00267ABC File Offset: 0x00265CBC
		public static bool IsBorrowedByAnyFaction(this Pawn pawn)
		{
			List<Quest> activeQuestsListForReading = Find.QuestManager.ActiveQuestsListForReading;
			int count = activeQuestsListForReading.Count;
			for (int i = 0; i < count; i++)
			{
				if (activeQuestsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = activeQuestsListForReading[i].PartsListForReading;
					int count2 = partsListForReading.Count;
					for (int j = 0; j < count2; j++)
					{
						QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction = partsListForReading[j] as QuestPart_LendColonistsToFaction;
						if (questPart_LendColonistsToFaction != null && questPart_LendColonistsToFaction.LentColonistsListForReading.Contains(pawn))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06006DCD RID: 28109 RVA: 0x00267B44 File Offset: 0x00265D44
		public static int TotalBorrowedColonistCount()
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			int num = 0;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction = partsListForReading[j] as QuestPart_LendColonistsToFaction;
						if (questPart_LendColonistsToFaction != null)
						{
							num += questPart_LendColonistsToFaction.LentColonistsListForReading.Count;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06006DCE RID: 28110 RVA: 0x00267BC3 File Offset: 0x00265DC3
		public static IEnumerable<T> GetAllQuestPartsOfType<T>(bool ongoingOnly = true) where T : class
		{
			QuestUtility.<GetAllQuestPartsOfType>d__120<T> <GetAllQuestPartsOfType>d__ = new QuestUtility.<GetAllQuestPartsOfType>d__120<T>(-2);
			<GetAllQuestPartsOfType>d__.<>3__ongoingOnly = ongoingOnly;
			return <GetAllQuestPartsOfType>d__;
		}

		// Token: 0x06006DCF RID: 28111 RVA: 0x00267BD4 File Offset: 0x00265DD4
		public static void AppendInspectStringsFromQuestParts(StringBuilder sb, ISelectable target)
		{
			int num;
			QuestUtility.AppendInspectStringsFromQuestParts(sb, target, out num);
		}

		// Token: 0x06006DD0 RID: 28112 RVA: 0x00267BEA File Offset: 0x00265DEA
		public static void AppendInspectStringsFromQuestParts(StringBuilder sb, ISelectable target, out int count)
		{
			QuestUtility.AppendInspectStringsFromQuestParts(delegate(string str, Quest quest)
			{
				if (sb.Length != 0)
				{
					sb.AppendLine();
				}
				sb.Append(str);
			}, target, out count);
		}

		// Token: 0x06006DD1 RID: 28113 RVA: 0x00267C0C File Offset: 0x00265E0C
		public static void AppendInspectStringsFromQuestParts(Action<string, Quest> func, ISelectable target, out int count)
		{
			count = 0;
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					QuestUtility.tmpQuestParts.Clear();
					QuestUtility.tmpQuestParts.AddRange(questsListForReading[i].PartsListForReading);
					QuestUtility.tmpQuestParts.SortBy(delegate(QuestPart x)
					{
						QuestPartActivable questPartActivable2 = x as QuestPartActivable;
						if (questPartActivable2 == null)
						{
							return 0;
						}
						return questPartActivable2.EnableTick;
					});
					for (int j = 0; j < QuestUtility.tmpQuestParts.Count; j++)
					{
						QuestPartActivable questPartActivable = QuestUtility.tmpQuestParts[j] as QuestPartActivable;
						if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
						{
							string str = questPartActivable.ExtraInspectString(target);
							if (!str.NullOrEmpty())
							{
								func(str.Formatted(target.Named("TARGET")), questsListForReading[i]);
								count++;
							}
						}
					}
					QuestUtility.tmpQuestParts.Clear();
				}
			}
		}

		// Token: 0x06006DD2 RID: 28114 RVA: 0x00267D11 File Offset: 0x00265F11
		public static IEnumerable<Gizmo> GetQuestRelatedGizmos(Thing thing)
		{
			QuestUtility.<GetQuestRelatedGizmos>d__125 <GetQuestRelatedGizmos>d__ = new QuestUtility.<GetQuestRelatedGizmos>d__125(-2);
			<GetQuestRelatedGizmos>d__.<>3__thing = thing;
			return <GetQuestRelatedGizmos>d__;
		}

		// Token: 0x06006DD3 RID: 28115 RVA: 0x00267D24 File Offset: 0x00265F24
		public static Gizmo GetSelectMonumentMarkerGizmo(Thing thing)
		{
			if (!thing.Spawned || !ModsConfig.RoyaltyActive)
			{
				return null;
			}
			List<Thing> list = thing.Map.listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
			for (int i = 0; i < list.Count; i++)
			{
				MonumentMarker m = (MonumentMarker)list[i];
				if (m.IsPart(thing))
				{
					return new Command_Action
					{
						defaultLabel = "CommandSelectMonumentMarker".Translate(),
						defaultDesc = "CommandSelectMonumentMarkerDesc".Translate(),
						icon = ThingDefOf.MonumentMarker.uiIcon,
						iconAngle = ThingDefOf.MonumentMarker.uiIconAngle,
						iconOffset = ThingDefOf.MonumentMarker.uiIconOffset,
						action = delegate()
						{
							CameraJumper.TrySelect(m, false);
						}
					};
				}
			}
			return null;
		}

		// Token: 0x06006DD4 RID: 28116 RVA: 0x00267E08 File Offset: 0x00266008
		public static bool AnyQuestDisablesRandomMoodCausedMentalBreaksFor(Pawn p)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_DisableRandomMoodCausedMentalBreaks questPart_DisableRandomMoodCausedMentalBreaks = partsListForReading[j] as QuestPart_DisableRandomMoodCausedMentalBreaks;
						if (questPart_DisableRandomMoodCausedMentalBreaks != null && questPart_DisableRandomMoodCausedMentalBreaks.State == QuestPartState.Enabled && questPart_DisableRandomMoodCausedMentalBreaks.pawns.Contains(p))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06006DD5 RID: 28117 RVA: 0x00267E8C File Offset: 0x0026608C
		public static IEnumerable<Quest> GetSubquests(this Quest quest, QuestState? state = null)
		{
			QuestUtility.<GetSubquests>d__128 <GetSubquests>d__ = new QuestUtility.<GetSubquests>d__128(-2);
			<GetSubquests>d__.<>3__quest = quest;
			<GetSubquests>d__.<>3__state = state;
			return <GetSubquests>d__;
		}

		// Token: 0x06006DD6 RID: 28118 RVA: 0x00267EA3 File Offset: 0x002660A3
		public static bool IsSubquestOf(this Quest quest, Quest parent)
		{
			return quest.parent != null && quest.parent == parent;
		}

		// Token: 0x06006DD7 RID: 28119 RVA: 0x00267EB8 File Offset: 0x002660B8
		public static bool IsGoodwillLockedByQuest(Faction a, Faction b)
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].State == QuestState.Ongoing)
				{
					List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_FactionGoodwillLocked questPart_FactionGoodwillLocked = partsListForReading[j] as QuestPart_FactionGoodwillLocked;
						if (questPart_FactionGoodwillLocked != null && questPart_FactionGoodwillLocked.State == QuestPartState.Enabled && questPart_FactionGoodwillLocked.AppliesTo(a, b))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06006DD8 RID: 28120 RVA: 0x00267F38 File Offset: 0x00266138
		public static bool IsEndOnNewArchonexusSettlement(this Quest quest)
		{
			return quest.root.endOnColonyMove && (quest.State == QuestState.NotYetAccepted || quest.State == QuestState.Ongoing);
		}

		// Token: 0x06006DD9 RID: 28121 RVA: 0x00267F5C File Offset: 0x0026615C
		public static IEnumerable<QuestScriptDef> GetGiverQuests(QuestGiverTag tag)
		{
			QuestUtility.<GetGiverQuests>d__132 <GetGiverQuests>d__ = new QuestUtility.<GetGiverQuests>d__132(-2);
			<GetGiverQuests>d__.<>3__tag = tag;
			return <GetGiverQuests>d__;
		}

		// Token: 0x06006DDB RID: 28123 RVA: 0x00267F98 File Offset: 0x00266198
		[CompilerGenerated]
		internal static bool <GetExtraFactionsFromQuestParts>g__AlreadyAdded|117_0(ExtraFaction extraFaction, ref QuestUtility.<>c__DisplayClass117_0 A_1)
		{
			for (int i = 0; i < A_1.outExtraFactions.Count; i++)
			{
				if (A_1.outExtraFactions[i].faction == extraFaction.faction && A_1.outExtraFactions[i].factionType == extraFaction.factionType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040045F2 RID: 17906
		public const string QuestTargetSignalPart_MapGenerated = "MapGenerated";

		// Token: 0x040045F3 RID: 17907
		public const string QuestTargetSignalPart_MapRemoved = "MapRemoved";

		// Token: 0x040045F4 RID: 17908
		public const string QuestTargetSignalPart_MapSettled = "MapSettled";

		// Token: 0x040045F5 RID: 17909
		public const string QuestTargetSignalPart_Spawned = "Spawned";

		// Token: 0x040045F6 RID: 17910
		public const string QuestTargetSignalPart_Despawned = "Despawned";

		// Token: 0x040045F7 RID: 17911
		public const string QuestTargetSignalPart_Destroyed = "Destroyed";

		// Token: 0x040045F8 RID: 17912
		public const string QuestTargetSignalPart_Killed = "Killed";

		// Token: 0x040045F9 RID: 17913
		public const string QuestTargetSignalPart_TookDamage = "TookDamage";

		// Token: 0x040045FA RID: 17914
		public const string QuestTargetSignalPart_TookDamageFromPlayer = "TookDamageFromPlayer";

		// Token: 0x040045FB RID: 17915
		public const string QuestTargetSignalPart_ChangedFaction = "ChangedFaction";

		// Token: 0x040045FC RID: 17916
		public const string QuestTargetSignalPart_ChangedFactionToPlayer = "ChangedFactionToPlayer";

		// Token: 0x040045FD RID: 17917
		public const string QuestTargetSignalPart_ChangedFactionToNonPlayer = "ChangedFactionToNonPlayer";

		// Token: 0x040045FE RID: 17918
		public const string QuestTargetSignalPart_Hacked = "Hacked";

		// Token: 0x040045FF RID: 17919
		public const string QuestTargetSignalPart_HackingStarted = "HackingStarted";

		// Token: 0x04004600 RID: 17920
		public const string QuestTargetSignalPart_LockedOut = "LockedOut";

		// Token: 0x04004601 RID: 17921
		public const string QuestTargetSignalPart_StartedExtractingFromContainer = "StartedExtractingFromContainer";

		// Token: 0x04004602 RID: 17922
		public const string QuestTargetSignalPart_Studied = "Researched";

		// Token: 0x04004603 RID: 17923
		public const string QuestTargetSignalPart_KilledLeavingsLeft = "KilledLeavingsLeft";

		// Token: 0x04004604 RID: 17924
		public const string QuestTargetSignalPart_Unfogged = "Unfogged";

		// Token: 0x04004605 RID: 17925
		public const string QuestTargetSignalPart_Inspected = "Inspected";

		// Token: 0x04004606 RID: 17926
		public const string QuestTargetSignalPart_SwappedMap = "SwappedMap";

		// Token: 0x04004607 RID: 17927
		public const string QuestTargetSignalPart_LeftBehind = "LeftBehind";

		// Token: 0x04004608 RID: 17928
		public const string QuestTargetSignalPart_LeftMap = "LeftMap";

		// Token: 0x04004609 RID: 17929
		public const string QuestTargetSignalPart_SurgeryViolation = "SurgeryViolation";

		// Token: 0x0400460A RID: 17930
		public const string QuestTargetSignalPart_Arrested = "Arrested";

		// Token: 0x0400460B RID: 17931
		public const string QuestTargetSignalPart_Released = "Released";

		// Token: 0x0400460C RID: 17932
		public const string QuestTargetSignalPart_Recruited = "Recruited";

		// Token: 0x0400460D RID: 17933
		public const string QuestTargetSignalPart_Kidnapped = "Kidnapped";

		// Token: 0x0400460E RID: 17934
		public const string QuestTargetSignalPart_ChangedHostFaction = "ChangedHostFaction";

		// Token: 0x0400460F RID: 17935
		public const string QuestTargetSignalPart_NoLongerFactionLeader = "NoLongerFactionLeader";

		// Token: 0x04004610 RID: 17936
		public const string QuestTargetSignalPart_TitleChanged = "TitleChanged";

		// Token: 0x04004611 RID: 17937
		public const string QuestTargetSignalPart_TitleAwardedWhenUpdatingChanged = "TitleAwardedWhenUpdatingChanged";

		// Token: 0x04004612 RID: 17938
		public const string QuestTargetSignalPart_Banished = "Banished";

		// Token: 0x04004613 RID: 17939
		public const string QuestTargetSignalPart_Rescued = "Rescued";

		// Token: 0x04004614 RID: 17940
		public const string QuestTargetSignalPart_RanWild = "RanWild";

		// Token: 0x04004615 RID: 17941
		public const string QuestTargetSignalPart_Enslaved = "Enslaved";

		// Token: 0x04004616 RID: 17942
		public const string QuestTargetSignalPart_PlayerTended = "PlayerTended";

		// Token: 0x04004617 RID: 17943
		public const string QuestTargetSignalPart_XenogermAbsorbed = "XenogermAbsorbed";

		// Token: 0x04004618 RID: 17944
		public const string QuestTargetSignalPart_XenogermReimplanted = "XenogermReimplanted";

		// Token: 0x04004619 RID: 17945
		public const string QuestTargetSignalPart_BecameMutant = "BecameMutant";

		// Token: 0x0400461A RID: 17946
		public const string QuestTargetSignalPart_PsychicRitualTarget = "PsychicRitualTarget";

		// Token: 0x0400461B RID: 17947
		public const string QuestTargetSignalPart_ReceivedItems = "ReceivedItems";

		// Token: 0x0400461C RID: 17948
		public const string QuestTargetSignalPart_ShuttleSentSatisfied = "SentSatisfied";

		// Token: 0x0400461D RID: 17949
		public const string QuestTargetSignalPart_ShuttleSentUnsatisfied = "SentUnsatisfied";

		// Token: 0x0400461E RID: 17950
		public const string QuestTargetSignalPart_ShuttleSentWithExtraColonists = "SentWithExtraColonists";

		// Token: 0x0400461F RID: 17951
		public const string QuestTargetSignalPart_ShuttleUnloaded = "Unloaded";

		// Token: 0x04004620 RID: 17952
		public const string QuestTargetSignalPart_AllEnemiesDefeated = "AllEnemiesDefeated";

		// Token: 0x04004621 RID: 17953
		public const string QuestTargetSignalPart_TradeRequestFulfilled = "TradeRequestFulfilled";

		// Token: 0x04004622 RID: 17954
		public const string QuestTargetSignalPart_PeaceTalksResolved = "Resolved";

		// Token: 0x04004623 RID: 17955
		public const string QuestTargetSignalPart_LaunchedShip = "LaunchedShip";

		// Token: 0x04004624 RID: 17956
		public const string QuestTargetSignalPart_ReactorDestroyed = "ReactorDestroyed";

		// Token: 0x04004625 RID: 17957
		public const string QuestTargetSignalPart_MonumentCompleted = "MonumentCompleted";

		// Token: 0x04004626 RID: 17958
		public const string QuestTargetSignalPart_MonumentDestroyed = "MonumentDestroyed";

		// Token: 0x04004627 RID: 17959
		public const string QuestTargetSignalPart_MonumentCancelled = "MonumentCancelled";

		// Token: 0x04004628 RID: 17960
		public const string QuestTargetSignalPart_AllHivesDestroyed = "AllHivesDestroyed";

		// Token: 0x04004629 RID: 17961
		public const string QuestTargetSignalPart_ExitMentalState = "ExitMentalState";

		// Token: 0x0400462A RID: 17962
		public const string QuestTargetSignalPart_FactionBecameHostileToPlayer = "BecameHostileToPlayer";

		// Token: 0x0400462B RID: 17963
		public const string QuestTargetSignalPart_FactionBuiltBuilding = "BuiltBuilding";

		// Token: 0x0400462C RID: 17964
		public const string QuestTargetSignalPart_FactionPlacedBlueprint = "PlacedBlueprint";

		// Token: 0x0400462D RID: 17965
		public const string QuestTargetSignalPart_FactionMemberArrested = "FactionMemberArrested";

		// Token: 0x0400462E RID: 17966
		public const string QuestTargetSignalPart_CeremonyExpired = "CeremonyExpired";

		// Token: 0x0400462F RID: 17967
		public const string QuestTargetSignalPart_CeremonyFailed = "CeremonyFailed";

		// Token: 0x04004630 RID: 17968
		public const string QuestTargetSignalPart_CeremonyDone = "CeremonyDone";

		// Token: 0x04004631 RID: 17969
		public const string QuestTargetSignalPart_BeingAttacked = "BeingAttacked";

		// Token: 0x04004632 RID: 17970
		public const string QuestTargetSignalPart_Fleeing = "Fleeing";

		// Token: 0x04004633 RID: 17971
		public const string QuestTargetSignalPart_QuestEnded = "QuestEnded";

		// Token: 0x04004634 RID: 17972
		public const string QuestTargetSignalPart_AllPawnsLost = "AllPawnsLost";

		// Token: 0x04004635 RID: 17973
		public const string QuestTargetSignalPart_ShipDisposed = "Disposed";

		// Token: 0x04004636 RID: 17974
		public const string QuestTargetSignalPart_ShipArrived = "Arrived";

		// Token: 0x04004637 RID: 17975
		public const string QuestTargetSignalPart_ShipFlewAway = "FlewAway";

		// Token: 0x04004638 RID: 17976
		public const string QuestTargetSignalPart_ShipThingAdded = "ThingAdded";

		// Token: 0x04004639 RID: 17977
		public const string QuestTargetSignalPart_Activated = "Activated";

		// Token: 0x0400463A RID: 17978
		public const string QuestTargetSignalPart_NodeClosed = "NodeClosed";

		// Token: 0x0400463B RID: 17979
		public const string QuestTargetSignalPart_CoreDefeated = "CoreDefeated";

		// Token: 0x0400463C RID: 17980
		public const string QuestTargetSignalPart_NoActiveThreats = "NoActiveThreats";

		// Token: 0x0400463D RID: 17981
		private static readonly List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x0400463E RID: 17982
		private static List<QuestPart_WorkDisabled> tmpQuestWorkDisabled = new List<QuestPart_WorkDisabled>();

		// Token: 0x0400463F RID: 17983
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		// Token: 0x04004640 RID: 17984
		private static List<QuestPart> tmpQuestParts = new List<QuestPart>();
	}
}
