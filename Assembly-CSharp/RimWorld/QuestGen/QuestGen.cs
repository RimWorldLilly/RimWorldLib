using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02002156 RID: 8534
	public static class QuestGen
	{
		// Token: 0x170025F4 RID: 9716
		// (get) Token: 0x0600D547 RID: 54599 RVA: 0x004F1927 File Offset: 0x004EFB27
		public static QuestScriptDef Root
		{
			get
			{
				return QuestGen.root;
			}
		}

		// Token: 0x170025F5 RID: 9717
		// (get) Token: 0x0600D548 RID: 54600 RVA: 0x004F192E File Offset: 0x004EFB2E
		public static bool Working
		{
			get
			{
				return QuestGen.working;
			}
		}

		// Token: 0x170025F6 RID: 9718
		// (get) Token: 0x0600D549 RID: 54601 RVA: 0x004F1935 File Offset: 0x004EFB35
		public static List<Rule> QuestDescriptionRulesReadOnly
		{
			get
			{
				return QuestGen.questDescriptionRules;
			}
		}

		// Token: 0x170025F7 RID: 9719
		// (get) Token: 0x0600D54A RID: 54602 RVA: 0x004F193C File Offset: 0x004EFB3C
		public static Dictionary<string, string> QuestDescriptionConstantsReadOnly
		{
			get
			{
				return QuestGen.questDescriptionConstants;
			}
		}

		// Token: 0x170025F8 RID: 9720
		// (get) Token: 0x0600D54B RID: 54603 RVA: 0x004F1943 File Offset: 0x004EFB43
		public static List<Rule> QuestNameRulesReadOnly
		{
			get
			{
				return QuestGen.questNameRules;
			}
		}

		// Token: 0x170025F9 RID: 9721
		// (get) Token: 0x0600D54C RID: 54604 RVA: 0x004F194A File Offset: 0x004EFB4A
		public static Dictionary<string, string> QuestNameConstantsReadOnly
		{
			get
			{
				return QuestGen.questNameConstants;
			}
		}

		// Token: 0x170025FA RID: 9722
		// (get) Token: 0x0600D54D RID: 54605 RVA: 0x004F1951 File Offset: 0x004EFB51
		public static List<QuestTextRequest> TextRequestsReadOnly
		{
			get
			{
				return QuestGen.textRequests;
			}
		}

		// Token: 0x170025FB RID: 9723
		// (get) Token: 0x0600D54E RID: 54606 RVA: 0x004F1958 File Offset: 0x004EFB58
		public static List<Rule> QuestContentRulesReadOnly
		{
			get
			{
				return QuestGen.questContentRules;
			}
		}

		// Token: 0x0600D54F RID: 54607 RVA: 0x004F1960 File Offset: 0x004EFB60
		public static string GenerateNewSignal(string signalString, bool ensureUnique = true)
		{
			int num;
			if (!ensureUnique || !QuestGen.generatedSignals.TryGetValue(signalString, out num))
			{
				num = 0;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Quest");
			defaultInterpolatedStringHandler.AppendFormatted<int>(QuestGen.quest.id);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted(signalString);
			defaultInterpolatedStringHandler.AppendFormatted((num == 0) ? "" : (num + 1).ToString());
			string result = defaultInterpolatedStringHandler.ToStringAndClear();
			QuestGen.generatedSignals[signalString] = num + 1;
			return result;
		}

		// Token: 0x0600D550 RID: 54608 RVA: 0x004F19EC File Offset: 0x004EFBEC
		public static string GenerateNewTargetQuestTag(string targetString, bool ensureUnique = true)
		{
			int num;
			if (!ensureUnique || !QuestGen.generatedTargetQuestTags.TryGetValue(targetString, out num))
			{
				num = 0;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Quest");
			defaultInterpolatedStringHandler.AppendFormatted<int>(QuestGen.quest.id);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted(targetString);
			defaultInterpolatedStringHandler.AppendFormatted((num == 0) ? "" : (num + 1).ToString());
			string result = defaultInterpolatedStringHandler.ToStringAndClear();
			QuestGen.generatedTargetQuestTags[targetString] = num + 1;
			return result;
		}

		// Token: 0x0600D551 RID: 54609 RVA: 0x004F1A78 File Offset: 0x004EFC78
		private static void ResetIdCounters()
		{
			QuestGen.generatedSignals.Clear();
			QuestGen.generatedTargetQuestTags.Clear();
		}

		// Token: 0x0600D552 RID: 54610 RVA: 0x004F1A90 File Offset: 0x004EFC90
		public static string GenerateResolvedQuestName(QuestScriptDef root, Slate initialVars)
		{
			if (QuestGen.working)
			{
				Log.Error("Cannot generated quest name while generating another quest.");
				return null;
			}
			QuestGen.InitializeQuestGen(root, initialVars);
			root.InitializeRules();
			try
			{
				QuestNode_ResolveQuestName.Resolve();
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Error while generating quest name: ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			string name = QuestGen.quest.name;
			QuestGen.ClearQuestGenState();
			return name;
		}

		// Token: 0x0600D553 RID: 54611 RVA: 0x004F1B10 File Offset: 0x004EFD10
		public static Quest Generate(QuestScriptDef root, Slate initialVars)
		{
			if (DeepProfiler.enabled)
			{
				DeepProfiler.Start("Generate quest");
			}
			Quest result = null;
			try
			{
				if (QuestGen.working)
				{
					throw new Exception("Called Generate() while already working.");
				}
				QuestGen.InitializeQuestGen(root, initialVars);
				root.Run();
				if (!root.everAcceptableInSpace && !root.autoAccept)
				{
					QuestGen.quest.AcceptanceRequirementNotSpace(null);
				}
				try
				{
					QuestNode_ResolveQuestName.Resolve();
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Error while generating quest name: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				try
				{
					QuestNode_ResolveQuestDescription.Resolve();
				}
				catch (Exception value2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(42, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("Error while generating quest description: ");
					defaultInterpolatedStringHandler2.AppendFormatted<Exception>(value2);
					Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
				try
				{
					QuestNode_ResolveTextRequests.Resolve();
				}
				catch (Exception value3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(37, 1);
					defaultInterpolatedStringHandler3.AppendLiteral("Error while resolving text requests: ");
					defaultInterpolatedStringHandler3.AppendFormatted<Exception>(value3);
					Log.Error(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
				QuestGen.AddSlateQuestTags();
				bool flag = root.autoAccept;
				if (flag)
				{
					List<QuestPart> partsListForReading = QuestGen.quest.PartsListForReading;
					for (int i = 0; i < partsListForReading.Count; i++)
					{
						if (partsListForReading[i].PreventsAutoAccept)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					QuestGen.quest.SetInitiallyAccepted();
				}
				Site site;
				if (QuestGen.slate.TryGet<Site>("site", out site, false) && site.MainSitePartDef.copyQuestName)
				{
					site.customLabel = QuestGen.quest.name;
				}
				result = QuestGen.quest;
			}
			catch (Exception value4)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler4.AppendLiteral("Error in QuestGen: ");
				defaultInterpolatedStringHandler4.AppendFormatted<Exception>(value4);
				Log.Error(defaultInterpolatedStringHandler4.ToStringAndClear());
			}
			finally
			{
				if (DeepProfiler.enabled)
				{
					DeepProfiler.End();
				}
				QuestGen.ClearQuestGenState();
			}
			return result;
		}

		// Token: 0x0600D554 RID: 54612 RVA: 0x004F1D5C File Offset: 0x004EFF5C
		private static void InitializeQuestGen(QuestScriptDef root, Slate initialVars)
		{
			QuestGen.working = true;
			QuestGen.root = root;
			QuestGen.slate.Reset();
			QuestGen.slate.SetAll(initialVars);
			QuestGen.quest = Quest.MakeRaw();
			if (root.expireDaysRange.max > 0f)
			{
				QuestGen.quest.acceptanceExpireTick = GenTicks.TicksGame + (int)(root.expireDaysRange.RandomInRange * 60000f);
			}
			if (root.defaultChallengeRating > 0)
			{
				QuestGen.quest.challengeRating = root.defaultChallengeRating;
			}
			QuestGen.quest.root = root;
			QuestGen.quest.hidden = root.defaultHidden;
			QuestGen.quest.charity = root.defaultCharity;
			QuestGen.slate.SetIfNone<string>("inSignal", QuestGen.quest.InitiateSignal, false);
		}

		// Token: 0x0600D555 RID: 54613 RVA: 0x004F1E28 File Offset: 0x004F0028
		private static void ClearQuestGenState()
		{
			QuestGen.quest = null;
			QuestGen.root = null;
			QuestGen.working = false;
			QuestGen.generatedPawns.Clear();
			QuestGen.textRequests.Clear();
			QuestGen.slate.Reset();
			QuestGen.questDescriptionRules.Clear();
			QuestGen.questDescriptionConstants.Clear();
			QuestGen.questNameRules.Clear();
			QuestGen.questNameConstants.Clear();
			QuestGen.questContentRules.Clear();
			QuestGen.slateQuestTagsToAddWhenFinished.Clear();
			QuestGen.ResetIdCounters();
		}

		// Token: 0x0600D556 RID: 54614 RVA: 0x004F1EA6 File Offset: 0x004F00A6
		public static void AddToGeneratedPawns(Pawn pawn)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add a pawn to generated pawns while not resolving any quest.");
				return;
			}
			if (!QuestGen.generatedPawns.Contains(pawn))
			{
				QuestGen.generatedPawns.Add(pawn);
			}
		}

		// Token: 0x0600D557 RID: 54615 RVA: 0x004F1ED2 File Offset: 0x004F00D2
		public static bool WasGeneratedForQuestBeingGenerated(Pawn pawn)
		{
			return QuestGen.working && QuestGen.generatedPawns.Contains(pawn);
		}

		// Token: 0x0600D558 RID: 54616 RVA: 0x004F1EE8 File Offset: 0x004F00E8
		public static void AddQuestDescriptionRules(RulePack rulePack)
		{
			QuestGen.AddQuestDescriptionRules(rulePack.Rules);
		}

		// Token: 0x0600D559 RID: 54617 RVA: 0x004F1EF5 File Offset: 0x004F00F5
		public static void AddQuestDescriptionRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest description rules while not resolving any quest.");
				return;
			}
			QuestGen.questDescriptionRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x0600D55A RID: 54618 RVA: 0x004F1F1C File Offset: 0x004F011C
		public static void AddQuestDescriptionConstants(Dictionary<string, string> constants)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest description constants while not resolving any quest.");
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in QuestGenUtility.AppendCurrentPrefix(constants))
			{
				if (!QuestGen.questDescriptionConstants.ContainsKey(keyValuePair.Key))
				{
					QuestGen.questDescriptionConstants.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x0600D55B RID: 54619 RVA: 0x004F1FA8 File Offset: 0x004F01A8
		public static void AddQuestNameRules(RulePack rulePack)
		{
			QuestGen.AddQuestNameRules(rulePack.Rules);
		}

		// Token: 0x0600D55C RID: 54620 RVA: 0x004F1FB5 File Offset: 0x004F01B5
		public static void AddQuestNameRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest name rules while not resolving any quest.");
				return;
			}
			QuestGen.questNameRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x0600D55D RID: 54621 RVA: 0x004F1FDC File Offset: 0x004F01DC
		public static void AddQuestNameConstants(Dictionary<string, string> constants)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest name constants while not resolving any quest.");
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in QuestGenUtility.AppendCurrentPrefix(constants))
			{
				if (!QuestGen.questNameConstants.ContainsKey(keyValuePair.Key))
				{
					QuestGen.questNameConstants.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x0600D55E RID: 54622 RVA: 0x004F2068 File Offset: 0x004F0268
		public static void AddQuestContentRules(RulePack rulePack)
		{
			QuestGen.AddQuestContentRules(rulePack.Rules);
		}

		// Token: 0x0600D55F RID: 54623 RVA: 0x004F2075 File Offset: 0x004F0275
		public static void AddQuestContentRules(List<Rule> rules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add quest content rules while not resolving any quest.");
				return;
			}
			QuestGen.questContentRules.AddRange(QuestGenUtility.AppendCurrentPrefix(rules));
		}

		// Token: 0x0600D560 RID: 54624 RVA: 0x004F2099 File Offset: 0x004F0299
		public static void AddSlateQuestTagToAddWhenFinished(string slateVarNameWithPrefix)
		{
			if (!QuestGen.slateQuestTagsToAddWhenFinished.Contains(slateVarNameWithPrefix))
			{
				QuestGen.slateQuestTagsToAddWhenFinished.Add(slateVarNameWithPrefix);
			}
		}

		// Token: 0x0600D561 RID: 54625 RVA: 0x004F20B3 File Offset: 0x004F02B3
		public static void AddTextRequest(string localKeyword, Action<string> setter, RulePack extraLocalRules = null)
		{
			QuestGen.AddTextRequest(localKeyword, setter, (extraLocalRules != null) ? extraLocalRules.Rules : null);
		}

		// Token: 0x0600D562 RID: 54626 RVA: 0x004F20C8 File Offset: 0x004F02C8
		public static void AddTextRequest(string localKeyword, Action<string> setter, List<Rule> extraLocalRules)
		{
			if (!QuestGen.working)
			{
				Log.Error("Tried to add a text request while not resolving any quest.");
				return;
			}
			QuestTextRequest questTextRequest = new QuestTextRequest();
			questTextRequest.keyword = localKeyword;
			if (!QuestGen.slate.CurrentPrefix.NullOrEmpty())
			{
				questTextRequest.keyword = QuestGen.slate.CurrentPrefix + "/" + questTextRequest.keyword;
			}
			questTextRequest.keyword = QuestGenUtility.NormalizeVarPath(questTextRequest.keyword);
			questTextRequest.setter = setter;
			if (extraLocalRules != null)
			{
				questTextRequest.extraRules = QuestGenUtility.AppendCurrentPrefix(extraLocalRules);
			}
			QuestGen.textRequests.Add(questTextRequest);
		}

		// Token: 0x0600D563 RID: 54627 RVA: 0x004F2158 File Offset: 0x004F0358
		private static void AddSlateQuestTags()
		{
			for (int i = 0; i < QuestGen.slateQuestTagsToAddWhenFinished.Count; i++)
			{
				object obj;
				if (QuestGen.slate.TryGet<object>(QuestGen.slateQuestTagsToAddWhenFinished[i], out obj, true))
				{
					string questTagToAdd = QuestGen.GenerateNewTargetQuestTag(QuestGen.slateQuestTagsToAddWhenFinished[i], false);
					QuestUtility.AddQuestTag(obj, questTagToAdd);
				}
			}
			QuestGen.slateQuestTagsToAddWhenFinished.Clear();
		}

		// Token: 0x0400872B RID: 34603
		public static Quest quest;

		// Token: 0x0400872C RID: 34604
		public static Slate slate = new Slate();

		// Token: 0x0400872D RID: 34605
		private static QuestScriptDef root;

		// Token: 0x0400872E RID: 34606
		private static bool working;

		// Token: 0x0400872F RID: 34607
		private static List<QuestTextRequest> textRequests = new List<QuestTextRequest>();

		// Token: 0x04008730 RID: 34608
		private static List<Pawn> generatedPawns = new List<Pawn>();

		// Token: 0x04008731 RID: 34609
		private static Dictionary<string, int> generatedSignals = new Dictionary<string, int>();

		// Token: 0x04008732 RID: 34610
		private static Dictionary<string, int> generatedTargetQuestTags = new Dictionary<string, int>();

		// Token: 0x04008733 RID: 34611
		private static List<Rule> questDescriptionRules = new List<Rule>();

		// Token: 0x04008734 RID: 34612
		private static Dictionary<string, string> questDescriptionConstants = new Dictionary<string, string>();

		// Token: 0x04008735 RID: 34613
		private static List<Rule> questNameRules = new List<Rule>();

		// Token: 0x04008736 RID: 34614
		private static Dictionary<string, string> questNameConstants = new Dictionary<string, string>();

		// Token: 0x04008737 RID: 34615
		private static List<string> slateQuestTagsToAddWhenFinished = new List<string>();

		// Token: 0x04008738 RID: 34616
		private static List<Rule> questContentRules = new List<Rule>();
	}
}
