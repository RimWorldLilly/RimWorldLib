using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001044 RID: 4164
	public class Quest : IExposable, ILoadReferenceable, ISignalReceiver
	{
		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x060068A8 RID: 26792 RVA: 0x0024FC1F File Offset: 0x0024DE1F
		public List<QuestPart> PartsListForReading
		{
			get
			{
				return this.parts;
			}
		}

		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x060068A9 RID: 26793 RVA: 0x0024FC27 File Offset: 0x0024DE27
		public int TicksSinceAppeared
		{
			get
			{
				return Find.TickManager.TicksGame - this.appearanceTick;
			}
		}

		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x060068AA RID: 26794 RVA: 0x0024FC3A File Offset: 0x0024DE3A
		public int TicksSinceAccepted
		{
			get
			{
				if (this.acceptanceTick >= 0)
				{
					return Find.TickManager.TicksGame - this.acceptanceTick;
				}
				return -1;
			}
		}

		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x060068AB RID: 26795 RVA: 0x0024FC58 File Offset: 0x0024DE58
		public int TicksSinceCleanup
		{
			get
			{
				if (!this.cleanedUp)
				{
					return -1;
				}
				return Find.TickManager.TicksGame - this.cleanupTick;
			}
		}

		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x060068AC RID: 26796 RVA: 0x0024FC75 File Offset: 0x0024DE75
		public int TicksUntilExpiry
		{
			get
			{
				if (this.acceptanceExpireTick == -1)
				{
					return -1;
				}
				return Mathf.Max(this.acceptanceExpireTick - Find.TickManager.TicksGame, 0);
			}
		}

		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x060068AD RID: 26797 RVA: 0x0024FC99 File Offset: 0x0024DE99
		public string AccepterPawnLabelCap
		{
			get
			{
				if (this.accepterPawn == null)
				{
					return this.accepterPawnLabel;
				}
				return this.accepterPawn.LabelCap;
			}
		}

		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x060068AE RID: 26798 RVA: 0x0024FCB8 File Offset: 0x0024DEB8
		public string AddedSignal
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Quest");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.id);
				defaultInterpolatedStringHandler.AppendLiteral(".Added");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x060068AF RID: 26799 RVA: 0x0024FCFC File Offset: 0x0024DEFC
		public string InitiateSignal
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Quest");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.id);
				defaultInterpolatedStringHandler.AppendLiteral(".Initiate");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x060068B0 RID: 26800 RVA: 0x0024FD3F File Offset: 0x0024DF3F
		public bool EverAccepted
		{
			get
			{
				return this.initiallyAccepted || this.acceptanceTick >= 0;
			}
		}

		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x060068B1 RID: 26801 RVA: 0x0024FD57 File Offset: 0x0024DF57
		public Pawn AccepterPawn
		{
			get
			{
				return this.accepterPawn;
			}
		}

		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x060068B2 RID: 26802 RVA: 0x0024FD60 File Offset: 0x0024DF60
		public bool RequiresAccepter
		{
			get
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].RequiresAccepter)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x060068B3 RID: 26803 RVA: 0x0024FD9C File Offset: 0x0024DF9C
		public QuestState State
		{
			get
			{
				if (this.TicksUntilExpiry == 0 && this.acceptanceTick < 0)
				{
					return QuestState.EndedOfferExpired;
				}
				if (this.ended)
				{
					if (this.endOutcome == QuestEndOutcome.Success)
					{
						return QuestState.EndedSuccess;
					}
					if (this.endOutcome == QuestEndOutcome.Fail)
					{
						return QuestState.EndedFailed;
					}
					if (this.endOutcome == QuestEndOutcome.InvalidPreAcceptance)
					{
						return QuestState.EndedInvalid;
					}
					return QuestState.EndedUnknownOutcome;
				}
				else
				{
					if (this.acceptanceTick < 0)
					{
						return QuestState.NotYetAccepted;
					}
					return QuestState.Ongoing;
				}
			}
		}

		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x060068B4 RID: 26804 RVA: 0x0024FDF3 File Offset: 0x0024DFF3
		public IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.QuestLookTargets).Distinct<GlobalTargetInfo>();
			}
		}

		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x060068B5 RID: 26805 RVA: 0x0024FE24 File Offset: 0x0024E024
		public IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.QuestSelectTargets).Distinct<GlobalTargetInfo>();
			}
		}

		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x060068B6 RID: 26806 RVA: 0x0024FE55 File Offset: 0x0024E055
		public IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				return this.parts.SelectMany((QuestPart x) => x.InvolvedFactions).Distinct<Faction>();
			}
		}

		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x060068B7 RID: 26807 RVA: 0x0024FE86 File Offset: 0x0024E086
		public IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				Quest.<get_Hyperlinks>d__59 <get_Hyperlinks>d__ = new Quest.<get_Hyperlinks>d__59(-2);
				<get_Hyperlinks>d__.<>4__this = this;
				return <get_Hyperlinks>d__;
			}
		}

		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x060068B8 RID: 26808 RVA: 0x0024FE96 File Offset: 0x0024E096
		public bool Historical
		{
			get
			{
				return this.State != QuestState.NotYetAccepted && this.State != QuestState.Ongoing;
			}
		}

		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x060068B9 RID: 26809 RVA: 0x0024FEB0 File Offset: 0x0024E0B0
		public bool IncreasesPopulation
		{
			get
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].IncreasesPopulation)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060068BA RID: 26810 RVA: 0x0024FEE9 File Offset: 0x0024E0E9
		public static Quest MakeRaw()
		{
			return new Quest
			{
				id = Find.UniqueIDsManager.GetNextQuestID(),
				appearanceTick = Find.TickManager.TicksGame,
				name = "Unnamed quest"
			};
		}

		// Token: 0x060068BB RID: 26811 RVA: 0x0024FF1C File Offset: 0x0024E11C
		public void QuestTick()
		{
			if (this.Historical)
			{
				if (!this.cleanedUp)
				{
					this.CleanupQuestParts();
				}
				if (this.TicksSinceCleanup >= 1800000)
				{
					this.parts.Clear();
				}
				return;
			}
			if (this.TicksUntilExpiry == 0 && this.State == QuestState.NotYetAccepted && !this.cleanedUp)
			{
				this.CleanupQuestParts();
			}
			if (!this.Historical)
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					QuestPartActivable questPartActivable = this.parts[i] as QuestPartActivable;
					if (questPartActivable != null && questPartActivable.State == QuestPartState.Enabled)
					{
						try
						{
							questPartActivable.QuestPartTick();
						}
						catch (Exception value)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Exception ticking QuestPart: ");
							defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
							Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						if (this.Historical)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x060068BC RID: 26812 RVA: 0x00250000 File Offset: 0x0024E200
		public void AddPart(QuestPart part)
		{
			if (this.parts.Contains(part))
			{
				Log.Error("Tried to add the same QuestPart twice: " + part.ToStringSafe<QuestPart>() + ", quest=" + this.ToStringSafe<Quest>());
				return;
			}
			part.quest = this;
			this.parts.Add(part);
		}

		// Token: 0x060068BD RID: 26813 RVA: 0x00250050 File Offset: 0x0024E250
		public T AddPart<T>() where T : QuestPart
		{
			T t = Activator.CreateInstance<T>();
			this.AddPart(t);
			return t;
		}

		// Token: 0x060068BE RID: 26814 RVA: 0x00250070 File Offset: 0x0024E270
		public T GetFirstOrAddPart<T>() where T : QuestPart
		{
			T result;
			if ((result = this.GetFirstPartOfType<T>()) == null)
			{
				result = this.AddPart<T>();
			}
			return result;
		}

		// Token: 0x060068BF RID: 26815 RVA: 0x00250088 File Offset: 0x0024E288
		public T GetFirstPartOfType<T>() where T : QuestPart
		{
			foreach (QuestPart questPart in this.parts)
			{
				T t = questPart as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060068C0 RID: 26816 RVA: 0x002500F8 File Offset: 0x0024E2F8
		public bool TryGetFirstPartOfType<T>(out T part) where T : QuestPart
		{
			part = this.GetFirstPartOfType<T>();
			return part != null;
		}

		// Token: 0x060068C1 RID: 26817 RVA: 0x00250114 File Offset: 0x0024E314
		public void RemovePart(QuestPart part)
		{
			if (!this.parts.Contains(part))
			{
				Log.Error("Tried to remove QuestPart which doesn't exist: " + part.ToStringSafe<QuestPart>() + ", quest=" + this.ToStringSafe<Quest>());
				return;
			}
			part.quest = null;
			this.parts.Remove(part);
		}

		// Token: 0x060068C2 RID: 26818 RVA: 0x00250164 File Offset: 0x0024E364
		public void Accept(Pawn by)
		{
			if (this.State != QuestState.NotYetAccepted)
			{
				return;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].PreQuestAccept();
			}
			this.acceptanceTick = Find.TickManager.TicksGame;
			this.accepterPawn = by;
			this.dismissed = false;
			this.Initiate();
		}

		// Token: 0x060068C3 RID: 26819 RVA: 0x002501C8 File Offset: 0x0024E3C8
		public void End(QuestEndOutcome outcome, bool sendLetter = true, bool playSound = true)
		{
			if (this.Historical)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to resolve a historical quest. id=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.id);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			this.ended = true;
			this.endOutcome = outcome;
			this.CleanupQuestParts();
			if (!this.EverAccepted && this.State == QuestState.EndedOfferExpired)
			{
				return;
			}
			if (sendLetter && !this.hidden)
			{
				string key = null;
				string key2 = null;
				LetterDef textLetterDef = null;
				switch (this.State)
				{
				case QuestState.EndedUnknownOutcome:
					key2 = "LetterQuestConcludedLabel";
					key = "LetterQuestCompletedConcluded";
					textLetterDef = LetterDefOf.NeutralEvent;
					if (playSound)
					{
						SoundDefOf.Quest_Concluded.PlayOneShotOnCamera(null);
					}
					break;
				case QuestState.EndedSuccess:
					key2 = "LetterQuestCompletedLabel";
					key = "LetterQuestCompletedSuccess";
					textLetterDef = LetterDefOf.PositiveEvent;
					if (playSound)
					{
						SoundDefOf.Quest_Succeded.PlayOneShotOnCamera(null);
					}
					break;
				case QuestState.EndedFailed:
					key2 = "LetterQuestFailedLabel";
					key = "LetterQuestCompletedFail";
					textLetterDef = LetterDefOf.NegativeEvent;
					if (playSound)
					{
						SoundDefOf.Quest_Failed.PlayOneShotOnCamera(null);
					}
					break;
				}
				Find.LetterStack.ReceiveLetter(key2.Translate(), key.Translate(this.name.CapitalizeFirst()), textLetterDef, null, null, this, null, null, 0, playSound);
			}
		}

		// Token: 0x060068C4 RID: 26820 RVA: 0x00250300 File Offset: 0x0024E500
		public bool QuestReserves(Pawn p)
		{
			if (this.Historical)
			{
				return false;
			}
			int count = this.parts.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.parts[i].QuestPartReserves(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060068C5 RID: 26821 RVA: 0x00250348 File Offset: 0x0024E548
		public bool QuestReserves(Faction f)
		{
			if (this.Historical)
			{
				return false;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (this.parts[i].QuestPartReserves(f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060068C6 RID: 26822 RVA: 0x0025038C File Offset: 0x0024E58C
		public bool QuestReserves(TransportShip ship)
		{
			if (this.Historical)
			{
				return false;
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (this.parts[i].QuestPartReserves(ship))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060068C7 RID: 26823 RVA: 0x002503D0 File Offset: 0x0024E5D0
		public void SetInitiallyAccepted()
		{
			this.acceptanceTick = Find.TickManager.TicksGame;
			this.acceptanceExpireTick = -1;
			this.initiallyAccepted = true;
		}

		// Token: 0x060068C8 RID: 26824 RVA: 0x002503F0 File Offset: 0x0024E5F0
		public void SetNotYetAccepted()
		{
			this.acceptanceTick = -1;
		}

		// Token: 0x060068C9 RID: 26825 RVA: 0x002503F9 File Offset: 0x0024E5F9
		public MapParent TryFindNewSuitableMapParentForRetarget()
		{
			return this.TryFindNewSuitablePlayerMapParentForRetarget(true);
		}

		// Token: 0x060068CA RID: 26826 RVA: 0x00250404 File Offset: 0x0024E604
		public MapParent TryFindNewSuitablePlayerMapParentForRetarget(bool checkQuestScript = true)
		{
			if (checkQuestScript)
			{
				return this.root.TryFindNewSuitableMapParentForRetarget();
			}
			using (IEnumerator<Map> enumerator = Current.Game.PlayerHomeMaps.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current.Parent;
				}
			}
			foreach (Map map in Current.Game.Maps)
			{
				if (GravshipUtility.GetPlayerGravEngine_NewTemp(map) != null)
				{
					return map.Parent;
				}
			}
			return null;
		}

		// Token: 0x060068CB RID: 26827 RVA: 0x002504B8 File Offset: 0x0024E6B8
		public bool IsParentSuitableForQuest(MapParent mapParent)
		{
			return this.root.IsParentSuitableForQuest(mapParent);
		}

		// Token: 0x060068CC RID: 26828 RVA: 0x002504C8 File Offset: 0x0024E6C8
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.appearanceTick, "appearanceTick", -1, false);
			Scribe_Values.Look<int>(ref this.acceptanceTick, "acceptanceTick", -1, false);
			Scribe_Values.Look<int>(ref this.acceptanceExpireTick, "acceptanceExpireTick", -1, false);
			Scribe_References.Look<Pawn>(ref this.accepterPawn, "acceptedBy", false);
			Scribe_Values.Look<string>(ref this.accepterPawnLabel, "acceptedByLabel", null, false);
			Scribe_Values.Look<bool>(ref this.ended, "ended", false, false);
			Scribe_Values.Look<QuestEndOutcome>(ref this.endOutcome, "endOutcome", QuestEndOutcome.Unknown, false);
			Scribe_Values.Look<bool>(ref this.cleanedUp, "cleanedUp", false, false);
			Scribe_Values.Look<int>(ref this.cleanupTick, "cleanupTick", -1, false);
			Scribe_Values.Look<bool>(ref this.initiallyAccepted, "initiallyAccepted", false, false);
			Scribe_Values.Look<bool>(ref this.dismissed, "dismissed", false, false);
			Scribe_Values.Look<bool>(ref this.hiddenInUI, "hiddenInUI", false, false);
			Scribe_Values.Look<int>(ref this.challengeRating, "challengeRating", 0, false);
			Scribe_Values.Look<TaggedString>(ref this.description, "description", default(TaggedString), false);
			Scribe_Values.Look<string>(ref this.lastSlateStateDebug, "lastSlateStateDebug", null, false);
			Scribe_Defs.Look<QuestScriptDef>(ref this.root, "root");
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Collections.Look<string>(ref this.signalsReceivedDebug, "signalsReceivedDebug", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<QuestPart>(ref this.parts, "parts", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.tags, "tags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.charity, "charity", false, false);
			Scribe_References.Look<Quest>(ref this.parent, "parent", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.parts.RemoveAll((QuestPart x) => x == null) != 0)
				{
					Log.Error("Some quest parts were null after loading.");
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					this.parts[i].quest = this;
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060068CD RID: 26829 RVA: 0x00250704 File Offset: 0x0024E904
		public void Notify_PawnDiscarded(Pawn pawn)
		{
			if (this.accepterPawn == pawn)
			{
				this.accepterPawn = null;
				this.accepterPawnLabel = pawn.LabelCap;
			}
			foreach (QuestPart questPart in this.PartsListForReading)
			{
				questPart.Notify_PawnDiscarded(pawn);
			}
		}

		// Token: 0x060068CE RID: 26830 RVA: 0x00250774 File Offset: 0x0024E974
		public void Notify_SignalReceived(Signal signal)
		{
			if (!signal.global)
			{
				string tag = signal.tag;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Quest");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.id);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				if (!tag.StartsWith(defaultInterpolatedStringHandler.ToStringAndClear()))
				{
					return;
				}
			}
			for (int i = 0; i < this.parts.Count; i++)
			{
				try
				{
					bool flag;
					switch (this.parts[i].signalListenMode)
					{
					case QuestPart.SignalListenMode.OngoingOnly:
						flag = (this.State == QuestState.Ongoing);
						break;
					case QuestPart.SignalListenMode.NotYetAcceptedOnly:
						flag = (this.State == QuestState.NotYetAccepted);
						break;
					case QuestPart.SignalListenMode.OngoingOrNotYetAccepted:
						flag = (this.State == QuestState.Ongoing || this.State == QuestState.NotYetAccepted);
						break;
					case QuestPart.SignalListenMode.HistoricalOnly:
						flag = this.Historical;
						break;
					case QuestPart.SignalListenMode.Always:
						flag = true;
						break;
					default:
						flag = false;
						break;
					}
					if (flag)
					{
						this.parts[i].Notify_QuestSignalReceived(signal);
					}
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(39, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("Error while processing a quest signal: ");
					defaultInterpolatedStringHandler2.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
		}

		// Token: 0x060068CF RID: 26831 RVA: 0x002508B0 File Offset: 0x0024EAB0
		public void PostAdded()
		{
			Find.SignalManager.SendSignal(new Signal(this.AddedSignal, false));
		}

		// Token: 0x060068D0 RID: 26832 RVA: 0x002508C8 File Offset: 0x0024EAC8
		public void Initiate()
		{
			Find.SignalManager.SendSignal(new Signal(this.InitiateSignal, false));
		}

		// Token: 0x060068D1 RID: 26833 RVA: 0x002508E0 File Offset: 0x0024EAE0
		public void CleanupQuestParts()
		{
			if (this.cleanedUp)
			{
				return;
			}
			this.cleanupTick = Find.TickManager.TicksGame;
			for (int i = 0; i < this.parts.Count; i++)
			{
				try
				{
					this.parts[i].Notify_PreCleanup();
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Error in QuestPart Notify_PreCleanup: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			for (int j = 0; j < this.parts.Count; j++)
			{
				try
				{
					this.parts[j].Cleanup();
				}
				catch (Exception value2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(28, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("Error in QuestPart cleanup: ");
					defaultInterpolatedStringHandler2.AppendFormatted<Exception>(value2);
					Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			this.cleanedUp = true;
			Find.FactionManager.Notify_QuestCleanedUp(this);
			IdeoUtility.Notify_QuestCleanedUp(this, this.State);
			if (this.root.hideOnCleanup)
			{
				this.hiddenInUI = true;
			}
		}

		// Token: 0x060068D2 RID: 26834 RVA: 0x00250A04 File Offset: 0x0024EC04
		public void Notify_ThingsProduced(Pawn worker, List<Thing> things)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_ThingsProduced(worker, things);
			}
		}

		// Token: 0x060068D3 RID: 26835 RVA: 0x00250A3C File Offset: 0x0024EC3C
		public void Notify_PlantHarvested(Pawn worker, Thing harvested)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PlantHarvested(worker, harvested);
			}
		}

		// Token: 0x060068D4 RID: 26836 RVA: 0x00250A74 File Offset: 0x0024EC74
		public void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnKilled(pawn, dinfo);
			}
		}

		// Token: 0x060068D5 RID: 26837 RVA: 0x00250AAC File Offset: 0x0024ECAC
		public void Notify_PawnBorn(Thing baby, Thing birther, Pawn mother, Pawn father)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnBorn(baby, birther, mother, father);
			}
		}

		// Token: 0x060068D6 RID: 26838 RVA: 0x00250AE8 File Offset: 0x0024ECE8
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x060068D7 RID: 26839 RVA: 0x00250B20 File Offset: 0x0024ED20
		public string GetUniqueLoadID()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Quest_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.id);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040042AD RID: 17069
		public int id;

		// Token: 0x040042AE RID: 17070
		private List<QuestPart> parts = new List<QuestPart>();

		// Token: 0x040042AF RID: 17071
		public string name;

		// Token: 0x040042B0 RID: 17072
		public TaggedString description;

		// Token: 0x040042B1 RID: 17073
		public float points;

		// Token: 0x040042B2 RID: 17074
		public int challengeRating = -1;

		// Token: 0x040042B3 RID: 17075
		public List<string> tags = new List<string>();

		// Token: 0x040042B4 RID: 17076
		public string lastSlateStateDebug;

		// Token: 0x040042B5 RID: 17077
		public QuestScriptDef root;

		// Token: 0x040042B6 RID: 17078
		public bool hidden;

		// Token: 0x040042B7 RID: 17079
		public Quest parent;

		// Token: 0x040042B8 RID: 17080
		public int appearanceTick = -1;

		// Token: 0x040042B9 RID: 17081
		public int acceptanceTick = -1;

		// Token: 0x040042BA RID: 17082
		public int acceptanceExpireTick = -1;

		// Token: 0x040042BB RID: 17083
		public bool initiallyAccepted;

		// Token: 0x040042BC RID: 17084
		public bool dismissed;

		// Token: 0x040042BD RID: 17085
		public bool hiddenInUI;

		// Token: 0x040042BE RID: 17086
		private Pawn accepterPawn;

		// Token: 0x040042BF RID: 17087
		private string accepterPawnLabel;

		// Token: 0x040042C0 RID: 17088
		public bool charity;

		// Token: 0x040042C1 RID: 17089
		public bool canGenerateInSpace;

		// Token: 0x040042C2 RID: 17090
		public List<string> signalsReceivedDebug;

		// Token: 0x040042C3 RID: 17091
		private bool ended;

		// Token: 0x040042C4 RID: 17092
		private QuestEndOutcome endOutcome;

		// Token: 0x040042C5 RID: 17093
		private bool cleanedUp;

		// Token: 0x040042C6 RID: 17094
		public int cleanupTick = -1;

		// Token: 0x040042C7 RID: 17095
		public const int MaxSignalsReceivedDebugCount = 25;

		// Token: 0x040042C8 RID: 17096
		private const int RemoveAllQuestPartsAfterTicksSinceCleanup = 1800000;
	}
}
