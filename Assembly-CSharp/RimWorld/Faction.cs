using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020015FE RID: 5630
	public class Faction : IExposable, ILoadReferenceable, ICommunicable
	{
		// Token: 0x17001832 RID: 6194
		// (get) Token: 0x06008BC6 RID: 35782 RVA: 0x0032A82B File Offset: 0x00328A2B
		// (set) Token: 0x06008BC7 RID: 35783 RVA: 0x0032A84C File Offset: 0x00328A4C
		public string Name
		{
			get
			{
				if (this.HasName)
				{
					return this.name;
				}
				return this.def.LabelCap;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17001833 RID: 6195
		// (get) Token: 0x06008BC8 RID: 35784 RVA: 0x0032A855 File Offset: 0x00328A55
		public bool HasName
		{
			get
			{
				return this.name != null;
			}
		}

		// Token: 0x17001834 RID: 6196
		// (get) Token: 0x06008BC9 RID: 35785 RVA: 0x0032A860 File Offset: 0x00328A60
		public bool IsPlayer
		{
			get
			{
				return this.def.isPlayer;
			}
		}

		// Token: 0x17001835 RID: 6197
		// (get) Token: 0x06008BCA RID: 35786 RVA: 0x0032A86D File Offset: 0x00328A6D
		public int PlayerGoodwill
		{
			get
			{
				return this.GoodwillWith(Faction.OfPlayer);
			}
		}

		// Token: 0x17001836 RID: 6198
		// (get) Token: 0x06008BCB RID: 35787 RVA: 0x0032A87A File Offset: 0x00328A7A
		public FactionRelationKind PlayerRelationKind
		{
			get
			{
				return this.RelationKindWith(Faction.OfPlayer);
			}
		}

		// Token: 0x17001837 RID: 6199
		// (get) Token: 0x06008BCC RID: 35788 RVA: 0x0032A887 File Offset: 0x00328A87
		public int NaturalGoodwill
		{
			get
			{
				return Find.GoodwillSituationManager.GetNaturalGoodwill(this);
			}
		}

		// Token: 0x17001838 RID: 6200
		// (get) Token: 0x06008BCD RID: 35789 RVA: 0x0032A894 File Offset: 0x00328A94
		public Color Color
		{
			get
			{
				if (this.color != null)
				{
					return this.color.Value;
				}
				if (this.def.colorSpectrum.NullOrEmpty<Color>())
				{
					return Color.white;
				}
				return ColorsFromSpectrum.Get(this.def.colorSpectrum, this.colorFromSpectrum);
			}
		}

		// Token: 0x17001839 RID: 6201
		// (get) Token: 0x06008BCE RID: 35790 RVA: 0x0032A8E8 File Offset: 0x00328AE8
		// (set) Token: 0x06008BCF RID: 35791 RVA: 0x0032A92C File Offset: 0x00328B2C
		public Color AllegianceColor
		{
			get
			{
				Color valueOrDefault = this.allegianceColor.GetValueOrDefault();
				if (this.allegianceColor == null)
				{
					valueOrDefault = this.Color;
					this.allegianceColor = new Color?(valueOrDefault);
				}
				return this.allegianceColor.Value;
			}
			set
			{
				this.allegianceColor = new Color?(value);
			}
		}

		// Token: 0x1700183A RID: 6202
		// (get) Token: 0x06008BD0 RID: 35792 RVA: 0x0032A93C File Offset: 0x00328B3C
		public string LeaderTitle
		{
			get
			{
				if (this.ideos == null || this.ideos.PrimaryIdeo == null || this.ideos.PrimaryIdeo.leaderTitleMale.NullOrEmpty())
				{
					if (this.leader != null && this.leader.gender == Gender.Female && !this.def.leaderTitleFemale.NullOrEmpty())
					{
						return this.def.leaderTitleFemale;
					}
					return this.def.leaderTitle;
				}
				else
				{
					if (this.leader != null && this.leader.gender == Gender.Female)
					{
						return this.ideos.PrimaryIdeo.leaderTitleFemale;
					}
					return this.ideos.PrimaryIdeo.leaderTitleMale;
				}
			}
		}

		// Token: 0x1700183B RID: 6203
		// (get) Token: 0x06008BD1 RID: 35793 RVA: 0x0032A9EC File Offset: 0x00328BEC
		private bool ShouldHaveLeader
		{
			get
			{
				return !this.IsPlayer && !this.Hidden && !this.temporary && this.def.humanlikeFaction;
			}
		}

		// Token: 0x1700183C RID: 6204
		// (get) Token: 0x06008BD2 RID: 35794 RVA: 0x0032AA13 File Offset: 0x00328C13
		public TaggedString NameColored
		{
			get
			{
				if (this.HasName)
				{
					return this.name.CapitalizeFirst().ApplyTag(this);
				}
				return this.def.LabelCap;
			}
		}

		// Token: 0x1700183D RID: 6205
		// (get) Token: 0x06008BD3 RID: 35795 RVA: 0x0032AA3A File Offset: 0x00328C3A
		public bool CanEverGiveGoodwillRewards
		{
			get
			{
				return !this.def.permanentEnemy && this.HasGoodwill;
			}
		}

		// Token: 0x1700183E RID: 6206
		// (get) Token: 0x06008BD4 RID: 35796 RVA: 0x0032AA51 File Offset: 0x00328C51
		public string GetReportText
		{
			get
			{
				return this.def.description + (this.def.HasRoyalTitles ? ("\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(this, null)) : "");
			}
		}

		// Token: 0x1700183F RID: 6207
		// (get) Token: 0x06008BD5 RID: 35797 RVA: 0x0032AA88 File Offset: 0x00328C88
		public bool Hidden
		{
			get
			{
				bool? flag = this.hidden;
				if (flag == null)
				{
					return this.def.hidden;
				}
				return flag.GetValueOrDefault();
			}
		}

		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x06008BD6 RID: 35798 RVA: 0x0032AAB8 File Offset: 0x00328CB8
		public bool HasGoodwill
		{
			get
			{
				return !this.Hidden && !this.temporary;
			}
		}

		// Token: 0x06008BD7 RID: 35799 RVA: 0x0032AAD0 File Offset: 0x00328CD0
		public Faction()
		{
			this.randomKey = Rand.Range(0, int.MaxValue);
			this.kidnapped = new KidnappedPawnsTracker(this);
		}

		// Token: 0x06008BD8 RID: 35800 RVA: 0x0032AB70 File Offset: 0x00328D70
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.leader, "leader", false);
			Scribe_Defs.Look<FactionDef>(ref this.def, "def");
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.randomKey, "randomKey", 0, false);
			Scribe_Values.Look<float>(ref this.colorFromSpectrum, "colorFromSpectrum", 0f, false);
			Scribe_Collections.Look<FactionRelation>(ref this.relations, "relations", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<KidnappedPawnsTracker>(ref this.kidnapped, "kidnapped", new object[]
			{
				this
			});
			Scribe_Deep.Look<FactionIdeosTracker>(ref this.ideos, "ideos", new object[]
			{
				this
			});
			Scribe_Collections.Look<PredatorThreat>(ref this.predatorThreats, "predatorThreats", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.defeated, "defeated", false, false);
			Scribe_Values.Look<int>(ref this.lastTraderRequestTick, "lastTraderRequestTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.lastOrbitalTraderRequestTick, "lastOrbitalTraderRequestTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.lastMilitaryAidRequestTick, "lastMilitaryAidRequestTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.lastExecutionTick, "lastExecutionTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.naturalGoodwillTimer, "naturalGoodwillTimer", 0, false);
			Scribe_Values.Look<bool>(ref this.allowRoyalFavorRewards, "allowRoyalFavorRewards", true, false);
			Scribe_Values.Look<bool>(ref this.allowGoodwillRewards, "allowGoodwillRewards", true, false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool?>(ref this.hidden, "hidden", null, false);
			Scribe_Values.Look<bool>(ref this.temporary, "temporary", false, false);
			Scribe_Values.Look<bool>(ref this.factionHostileOnHarmByPlayer, "factionHostileOnHarmByPlayer", false, false);
			Scribe_Values.Look<Color?>(ref this.color, "color", null, false);
			Scribe_Values.Look<bool>(ref this.neverFlee, "neverFlee", false, false);
			Scribe_Values.Look<Color?>(ref this.allegianceColor, "mechColor", null, false);
			Scribe_Values.Look<bool>(ref this.deactivated, "deactivated", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.predatorThreats.RemoveAll((PredatorThreat x) => x.predator == null);
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06008BD9 RID: 35801 RVA: 0x0032ADD0 File Offset: 0x00328FD0
		public void FactionTick()
		{
			this.CheckReachNaturalGoodwill();
			this.kidnapped.KidnappedPawnsTrackerTick();
			for (int i = this.predatorThreats.Count - 1; i >= 0; i--)
			{
				PredatorThreat predatorThreat = this.predatorThreats[i];
				if (predatorThreat.Expired)
				{
					this.predatorThreats.RemoveAt(i);
					if (predatorThreat.predator.Spawned)
					{
						predatorThreat.predator.Map.attackTargetsCache.UpdateTarget(predatorThreat.predator);
					}
				}
			}
			if (Find.TickManager.TicksGame % 1000 == 200 && this.IsPlayer)
			{
				if (NamePlayerFactionAndSettlementUtility.CanNameFactionNow())
				{
					Settlement settlement = Find.WorldObjects.Settlements.Find((Settlement x) => NamePlayerFactionAndSettlementUtility.CanNameSettlementSoon(x));
					if (settlement != null)
					{
						Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(settlement));
					}
					else
					{
						Find.WindowStack.Add(new Dialog_NamePlayerFaction());
					}
				}
				else
				{
					Settlement settlement2 = Find.WorldObjects.Settlements.Find((Settlement x) => NamePlayerFactionAndSettlementUtility.CanNameSettlementNow(x));
					if (settlement2 != null)
					{
						if (NamePlayerFactionAndSettlementUtility.CanNameFactionSoon())
						{
							Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(settlement2));
						}
						else
						{
							Find.WindowStack.Add(new Dialog_NamePlayerSettlement(settlement2));
						}
					}
				}
			}
			if (this.ShouldHaveLeader && this.leader == null)
			{
				Log.ErrorOnce("Faction leader for " + this.Name + " is null.", this.loadID ^ 441821);
			}
		}

		// Token: 0x06008BDA RID: 35802 RVA: 0x0032AF64 File Offset: 0x00329164
		private void CheckReachNaturalGoodwill()
		{
			if (this.IsPlayer || !this.HasGoodwill || this.def.permanentEnemy)
			{
				return;
			}
			int num = this.BaseGoodwillWith(Faction.OfPlayer);
			IntRange intRange = new IntRange(this.NaturalGoodwill - 50, this.NaturalGoodwill + 50);
			if (intRange.Includes(num))
			{
				this.naturalGoodwillTimer = 0;
				return;
			}
			this.naturalGoodwillTimer++;
			if (num < intRange.min)
			{
				int num2 = 3000000;
				if (this.naturalGoodwillTimer >= num2)
				{
					Faction ofPlayer = Faction.OfPlayer;
					int goodwillChange = Mathf.Min(10, intRange.min - num);
					bool canSendMessage = true;
					HistoryEventDef reachNaturalGoodwill = HistoryEventDefOf.ReachNaturalGoodwill;
					this.TryAffectGoodwillWith(ofPlayer, goodwillChange, canSendMessage, !this.temporary, reachNaturalGoodwill, null);
					this.naturalGoodwillTimer = 0;
					return;
				}
			}
			else if (num > intRange.max)
			{
				int num3 = 3000000;
				if (this.naturalGoodwillTimer >= num3)
				{
					Faction ofPlayer2 = Faction.OfPlayer;
					int goodwillChange2 = -Mathf.Min(10, num - intRange.max);
					bool canSendMessage2 = true;
					HistoryEventDef reachNaturalGoodwill = HistoryEventDefOf.ReachNaturalGoodwill;
					this.TryAffectGoodwillWith(ofPlayer2, goodwillChange2, canSendMessage2, !this.temporary, reachNaturalGoodwill, null);
					this.naturalGoodwillTimer = 0;
				}
			}
		}

		// Token: 0x06008BDB RID: 35803 RVA: 0x0032B088 File Offset: 0x00329288
		public void TryMakeInitialRelationsWith(Faction other)
		{
			if (this.RelationWith(other, true) != null)
			{
				return;
			}
			int a = Faction.<TryMakeInitialRelationsWith>g__GetInitialGoodwill|67_0(this, other);
			int b = Faction.<TryMakeInitialRelationsWith>g__GetInitialGoodwill|67_0(other, this);
			int num = Mathf.Min(a, b);
			FactionRelationKind kind;
			if (num <= -10)
			{
				kind = FactionRelationKind.Hostile;
			}
			else if (num >= 75)
			{
				kind = FactionRelationKind.Ally;
			}
			else
			{
				kind = FactionRelationKind.Neutral;
			}
			FactionRelation factionRelation = new FactionRelation();
			factionRelation.other = other;
			factionRelation.baseGoodwill = num;
			factionRelation.kind = kind;
			this.relations.Add(factionRelation);
			FactionRelation factionRelation2 = new FactionRelation();
			factionRelation2.other = this;
			factionRelation2.baseGoodwill = num;
			factionRelation2.kind = kind;
			other.relations.Add(factionRelation2);
		}

		// Token: 0x06008BDC RID: 35804 RVA: 0x0032B120 File Offset: 0x00329320
		public void SetRelation(FactionRelation relation)
		{
			if (relation.other == this)
			{
				Log.Error("Tried to set relation between faction " + ((this != null) ? this.ToString() : null) + " and itself.");
				return;
			}
			if (relation.other == null)
			{
				Log.Error("Relation is missing faction.");
				return;
			}
			this.relations.RemoveAll((FactionRelation r) => r.other == relation.other);
			relation.other.relations.RemoveAll((FactionRelation r) => r.other == this);
			this.relations.Add(relation);
			FactionRelation factionRelation = new FactionRelation();
			factionRelation.other = this;
			factionRelation.kind = relation.kind;
			relation.other.relations.Add(factionRelation);
		}

		// Token: 0x06008BDD RID: 35805 RVA: 0x0032B208 File Offset: 0x00329408
		public PawnKindDef RandomPawnKind()
		{
			Faction.allPawnKinds.Clear();
			if (this.def.pawnGroupMakers != null)
			{
				for (int i = 0; i < this.def.pawnGroupMakers.Count; i++)
				{
					List<PawnGenOption> options = this.def.pawnGroupMakers[i].options;
					for (int j = 0; j < options.Count; j++)
					{
						if (options[j].kind.RaceProps.Humanlike)
						{
							Faction.allPawnKinds.Add(options[j].kind);
						}
					}
				}
			}
			if (!Faction.allPawnKinds.Any<PawnKindDef>())
			{
				return this.def.basicMemberKind;
			}
			PawnKindDef result = Faction.allPawnKinds.RandomElement<PawnKindDef>();
			Faction.allPawnKinds.Clear();
			return result;
		}

		// Token: 0x06008BDE RID: 35806 RVA: 0x0032B2C9 File Offset: 0x003294C9
		public bool IsPlayerGoodwillMinimum()
		{
			return this.def.permanentEnemy || this.PlayerGoodwill <= -100;
		}

		// Token: 0x06008BDF RID: 35807 RVA: 0x0032B2E8 File Offset: 0x003294E8
		public FactionRelation RelationWith(Faction other, bool allowNull = false)
		{
			if (other == this)
			{
				Log.Error("Tried to get relation between faction " + ((this != null) ? this.ToString() : null) + " and itself.");
				return new FactionRelation();
			}
			for (int i = 0; i < this.relations.Count; i++)
			{
				if (this.relations[i].other == other)
				{
					return this.relations[i];
				}
			}
			if (!allowNull)
			{
				Log.Error(string.Concat(new string[]
				{
					"Faction ",
					this.name,
					" has null relation with ",
					(other != null) ? other.ToString() : null,
					". Returning dummy relation."
				}));
				return new FactionRelation();
			}
			return null;
		}

		// Token: 0x06008BE0 RID: 35808 RVA: 0x0032B3A2 File Offset: 0x003295A2
		public int BaseGoodwillWith(Faction other)
		{
			return this.RelationWith(other, false).baseGoodwill;
		}

		// Token: 0x06008BE1 RID: 35809 RVA: 0x0032B3B4 File Offset: 0x003295B4
		public int GoodwillWith(Faction other)
		{
			int num = this.BaseGoodwillWith(other);
			if (this.IsPlayer)
			{
				num = Mathf.Min(num, Find.GoodwillSituationManager.GetMaxGoodwill(other));
			}
			else if (other.IsPlayer)
			{
				num = Mathf.Min(num, Find.GoodwillSituationManager.GetMaxGoodwill(this));
			}
			return num;
		}

		// Token: 0x06008BE2 RID: 35810 RVA: 0x0032B400 File Offset: 0x00329600
		public FactionRelationKind RelationKindWith(Faction other)
		{
			return this.RelationWith(other, false).kind;
		}

		// Token: 0x06008BE3 RID: 35811 RVA: 0x0032B410 File Offset: 0x00329610
		public bool CanChangeGoodwillFor(Faction other, int goodwillChange)
		{
			return this.HasGoodwill && other.HasGoodwill && !this.def.permanentEnemy && !other.def.permanentEnemy && !this.defeated && !other.defeated && other != this && (!this.def.permanentEnemyToEveryoneExceptPlayer || other.IsPlayer) && (!other.def.permanentEnemyToEveryoneExceptPlayer || this.IsPlayer) && (this.def.permanentEnemyToEveryoneExcept == null || this.def.permanentEnemyToEveryoneExcept.Contains(other.def)) && (other.def.permanentEnemyToEveryoneExcept == null || other.def.permanentEnemyToEveryoneExcept.Contains(this.def)) && (goodwillChange <= 0 || ((!this.IsPlayer || !SettlementUtility.IsPlayerAttackingAnySettlementOf(other)) && (!other.IsPlayer || !SettlementUtility.IsPlayerAttackingAnySettlementOf(this)))) && !QuestUtility.IsGoodwillLockedByQuest(this, other);
		}

		// Token: 0x06008BE4 RID: 35812 RVA: 0x0032B512 File Offset: 0x00329712
		public int GoodwillToMakeHostile(Faction other)
		{
			if (this.HostileTo(other))
			{
				return 0;
			}
			return -75 - this.GoodwillWith(other);
		}

		// Token: 0x06008BE5 RID: 35813 RVA: 0x0032B52C File Offset: 0x0032972C
		public int CalculateAdjustedGoodwillChange(Faction other, int goodwillChange)
		{
			int num = this.GoodwillWith(other);
			if (this.IsPlayer || other.IsPlayer)
			{
				int num2 = this.IsPlayer ? other.NaturalGoodwill : this.NaturalGoodwill;
				if ((num2 < num && goodwillChange < 0) || (num2 > num && goodwillChange > 0))
				{
					int num3 = Mathf.Min(Mathf.Abs(num - num2), Mathf.Abs(goodwillChange));
					int num4 = Mathf.RoundToInt(0.25f * (float)num3);
					if (goodwillChange < 0)
					{
						num4 = -num4;
					}
					goodwillChange += num4;
				}
			}
			return goodwillChange;
		}

		// Token: 0x06008BE6 RID: 35814 RVA: 0x0032B5A8 File Offset: 0x003297A8
		public bool TryAffectGoodwillWith(Faction other, int goodwillChange, bool canSendMessage = true, bool canSendHostilityLetter = true, HistoryEventDef reason = null, GlobalTargetInfo? lookTarget = null)
		{
			if (!this.CanChangeGoodwillFor(other, goodwillChange))
			{
				return false;
			}
			if (goodwillChange == 0)
			{
				return true;
			}
			int num = this.GoodwillWith(other);
			goodwillChange = this.CalculateAdjustedGoodwillChange(other, goodwillChange);
			int num2 = this.BaseGoodwillWith(other);
			int num3 = Mathf.Clamp(num2 + goodwillChange, -100, 100);
			if (num2 == num3)
			{
				return true;
			}
			if (reason != null && (this.IsPlayer || other.IsPlayer))
			{
				Faction arg = this.IsPlayer ? other : this;
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(reason, arg.Named(HistoryEventArgsNames.AffectedFaction), goodwillChange.Named(HistoryEventArgsNames.CustomGoodwill)), true);
			}
			FactionRelation factionRelation = this.RelationWith(other, false);
			factionRelation.baseGoodwill = num3;
			bool flag;
			factionRelation.CheckKindThresholds(this, canSendHostilityLetter, (reason != null) ? reason.LabelCap : null, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			FactionRelation factionRelation2 = other.RelationWith(this, false);
			FactionRelationKind kind = factionRelation2.kind;
			factionRelation2.baseGoodwill = factionRelation.baseGoodwill;
			factionRelation2.kind = factionRelation.kind;
			bool flag2;
			if (kind != factionRelation2.kind)
			{
				other.Notify_RelationKindChanged(this, kind, canSendHostilityLetter, (reason != null) ? reason.LabelCap : null, lookTarget ?? GlobalTargetInfo.Invalid, out flag2);
			}
			else
			{
				flag2 = false;
			}
			int num4 = this.GoodwillWith(other);
			if (canSendMessage && num != num4 && !flag && !flag2 && Current.ProgramState == ProgramState.Playing && (this.IsPlayer || other.IsPlayer))
			{
				Faction faction = this.IsPlayer ? other : this;
				string text;
				if (reason != null)
				{
					text = "MessageGoodwillChangedWithReason".Translate(faction.name, num.ToString("F0"), num4.ToString("F0"), reason.label);
				}
				else
				{
					text = "MessageGoodwillChanged".Translate(faction.name, num.ToString("F0"), num4.ToString("F0"));
				}
				Messages.Message(text, lookTarget ?? GlobalTargetInfo.Invalid, ((float)goodwillChange > 0f) ? MessageTypeDefOf.PositiveEvent : MessageTypeDefOf.NegativeEvent, true);
			}
			return true;
		}

		// Token: 0x06008BE7 RID: 35815 RVA: 0x0032B830 File Offset: 0x00329A30
		public void Notify_GoodwillSituationsChanged(Faction other, bool canSendHostilityLetter, string reason, GlobalTargetInfo? lookTarget)
		{
			FactionRelation factionRelation = this.RelationWith(other, false);
			bool flag;
			factionRelation.CheckKindThresholds(this, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			FactionRelation factionRelation2 = other.RelationWith(this, false);
			FactionRelationKind kind = factionRelation2.kind;
			factionRelation2.kind = factionRelation.kind;
			if (kind != factionRelation2.kind)
			{
				other.Notify_RelationKindChanged(this, kind, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			}
		}

		// Token: 0x06008BE8 RID: 35816 RVA: 0x0032B8B8 File Offset: 0x00329AB8
		public void SetRelationDirect(Faction other, FactionRelationKind kind, bool canSendHostilityLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
		{
			if (this.HasGoodwill && other.HasGoodwill)
			{
				Log.Error("Tried to use SetRelationDirect for factions which use goodwill. The relation would be overriden by goodwill anyway. faction=" + ((this != null) ? this.ToString() : null) + ", other=" + ((other != null) ? other.ToString() : null));
				return;
			}
			FactionRelation factionRelation = this.RelationWith(other, false);
			if (factionRelation.kind != kind)
			{
				FactionRelationKind kind2 = factionRelation.kind;
				factionRelation.kind = kind;
				bool flag;
				this.Notify_RelationKindChanged(other, kind2, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
				other.RelationWith(this, false).kind = kind;
				other.Notify_RelationKindChanged(this, kind2, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			}
		}

		// Token: 0x06008BE9 RID: 35817 RVA: 0x0032B980 File Offset: 0x00329B80
		public void RemoveAllRelations()
		{
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (faction != this)
				{
					faction.relations.RemoveAll((FactionRelation x) => x.other == this);
				}
			}
			this.relations.Clear();
		}

		// Token: 0x06008BEA RID: 35818 RVA: 0x0032B9F8 File Offset: 0x00329BF8
		public void TryAppendRelationKindChangedInfo(StringBuilder text, FactionRelationKind previousKind, FactionRelationKind newKind, string reason = null)
		{
			TaggedString taggedString = null;
			this.TryAppendRelationKindChangedInfo(ref taggedString, previousKind, newKind, reason);
			if (!taggedString.NullOrEmpty())
			{
				text.AppendLine();
				text.AppendLine();
				text.AppendTagged(taggedString);
			}
		}

		// Token: 0x06008BEB RID: 35819 RVA: 0x0032BA38 File Offset: 0x00329C38
		public void TryAppendRelationKindChangedInfo(ref TaggedString text, FactionRelationKind previousKind, FactionRelationKind newKind, string reason = null)
		{
			if (previousKind == newKind)
			{
				return;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			if (newKind == FactionRelationKind.Hostile)
			{
				text += "LetterRelationsChange_Hostile".Translate(this.NameColored);
				if (this.HasGoodwill)
				{
					string key = this.def.hideGiftingInHostilityText ? "LetterRelationsChange_HostileGoodwillDescription_NoGifting" : "LetterRelationsChange_HostileGoodwillDescription";
					text += "\n\n" + key.Translate(this.PlayerGoodwill.ToStringWithSign(), -75.ToStringWithSign(), 0.ToStringWithSign());
				}
				if (!reason.NullOrEmpty())
				{
					text += "\n\n" + "FinalStraw".Translate(reason.CapitalizeFirst());
					return;
				}
			}
			else if (newKind == FactionRelationKind.Ally)
			{
				text += "LetterRelationsChange_Ally".Translate(this.NameColored);
				if (this.HasGoodwill)
				{
					text += "\n\n" + "LetterRelationsChange_AllyGoodwillDescription".Translate(this.PlayerGoodwill.ToStringWithSign(), 75.ToStringWithSign(), 0.ToStringWithSign());
				}
				if (!reason.NullOrEmpty())
				{
					text += "\n\n" + "LastFactionRelationsEvent".Translate() + ": " + reason.CapitalizeFirst();
					return;
				}
			}
			else if (newKind == FactionRelationKind.Neutral)
			{
				if (previousKind == FactionRelationKind.Hostile)
				{
					text += "LetterRelationsChange_NeutralFromHostile".Translate(this.NameColored);
					if (this.HasGoodwill)
					{
						text += "\n\n" + "LetterRelationsChange_NeutralFromHostileGoodwillDescription".Translate(this.NameColored, this.PlayerGoodwill.ToStringWithSign(), 0.ToStringWithSign(), -75.ToStringWithSign(), 75.ToStringWithSign());
					}
					if (!reason.NullOrEmpty())
					{
						text += "\n\n" + "LastFactionRelationsEvent".Translate() + ": " + reason.CapitalizeFirst();
						return;
					}
				}
				else
				{
					text += "LetterRelationsChange_NeutralFromAlly".Translate(this.NameColored);
					if (this.HasGoodwill)
					{
						text += "\n\n" + "LetterRelationsChange_NeutralFromAllyGoodwillDescription".Translate(this.NameColored, this.PlayerGoodwill.ToStringWithSign(), 0.ToStringWithSign(), -75.ToStringWithSign(), 75.ToStringWithSign());
					}
					if (!reason.NullOrEmpty())
					{
						text += "\n\n" + "Reason".Translate() + ": " + reason.CapitalizeFirst();
					}
				}
			}
		}

		// Token: 0x06008BEC RID: 35820 RVA: 0x0032BDC0 File Offset: 0x00329FC0
		public void Notify_MemberTookDamage(Pawn member, DamageInfo dinfo)
		{
			if (dinfo.Instigator == null)
			{
				return;
			}
			if (this.IsPlayer)
			{
				return;
			}
			Pawn pawn = dinfo.Instigator as Pawn;
			if (pawn != null && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.PredatorHunt)
			{
				this.TookDamageFromPredator(pawn);
			}
			if (dinfo.Instigator.Faction == null || !dinfo.Def.ExternalViolenceFor(member) || this.HostileTo(dinfo.Instigator.Faction))
			{
				return;
			}
			if (this.factionHostileOnHarmByPlayer && dinfo.Instigator.Faction == Faction.OfPlayer)
			{
				this.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
			}
			if (member.InAggroMentalState)
			{
				return;
			}
			if (pawn != null && pawn.InAggroMentalState)
			{
				return;
			}
			if (member.InMentalState && member.MentalStateDef.IsExtreme && member.MentalStateDef.category == MentalStateCategory.Malicious && this.PlayerRelationKind == FactionRelationKind.Ally)
			{
				return;
			}
			if (dinfo.Instigator.Faction == Faction.OfPlayer && (PrisonBreakUtility.IsPrisonBreaking(member) || member.IsQuestHelper()))
			{
				return;
			}
			if (pawn != null && SlaveRebellionUtility.IsRebelling(pawn))
			{
				return;
			}
			if (dinfo.Instigator.Faction == Faction.OfPlayer && !this.IsMutuallyHostileCrossfire(dinfo) && !member.IsSlaveOfColony)
			{
				float num = Mathf.Min(100f, dinfo.Amount);
				int goodwillChange = (int)(-1.3f * num);
				Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange, true, !this.temporary, HistoryEventDefOf.AttackedMember, new GlobalTargetInfo?(member));
			}
		}

		// Token: 0x06008BED RID: 35821 RVA: 0x0032BF50 File Offset: 0x0032A150
		public void Notify_BuildingRemoved(Building building, Pawn deconstructor)
		{
			if (this.IsPlayer)
			{
				return;
			}
			if (this.factionHostileOnHarmByPlayer && deconstructor != null && deconstructor.Faction == Faction.OfPlayer)
			{
				this.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
			}
		}

		// Token: 0x06008BEE RID: 35822 RVA: 0x0032BF98 File Offset: 0x0032A198
		public void Notify_BuildingTookDamage(Building building, DamageInfo dinfo)
		{
			if (dinfo.Instigator == null || this.IsPlayer)
			{
				return;
			}
			if (dinfo.Instigator.Faction == null || !dinfo.Def.ExternalViolenceFor(building) || this.HostileTo(dinfo.Instigator.Faction))
			{
				return;
			}
			if (this.factionHostileOnHarmByPlayer && dinfo.Instigator.Faction == Faction.OfPlayer)
			{
				this.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
			}
			if (dinfo.Instigator.Faction == Faction.OfPlayer && !this.IsMutuallyHostileCrossfire(dinfo))
			{
				float num = Mathf.Min(100f, dinfo.Amount);
				int goodwillChange = (int)(-1f * num);
				Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange, true, true, HistoryEventDefOf.AttackedBuilding, new GlobalTargetInfo?(building));
			}
		}

		// Token: 0x06008BEF RID: 35823 RVA: 0x0032C074 File Offset: 0x0032A274
		public void Notify_MemberCaptured(Pawn member, Faction violator)
		{
			if (this.ideos != null)
			{
				this.ideos.Notify_MemberGainedOrLost();
			}
			if (violator == this)
			{
				return;
			}
			if (!this.temporary && !member.IsSlaveOfColony)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this, Faction.OfPlayer.GoodwillToMakeHostile(this), true, !this.temporary, HistoryEventDefOf.MemberCaptured, new GlobalTargetInfo?(member));
			}
		}

		// Token: 0x06008BF0 RID: 35824 RVA: 0x0032C0DC File Offset: 0x0032A2DC
		public void Notify_MemberStripped(Pawn member, Faction violator)
		{
			if (violator == this || this.Hidden || member.Dead)
			{
				return;
			}
			if (violator == Faction.OfPlayer && this.RelationKindWith(violator) != FactionRelationKind.Hostile)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this, -40, true, !this.temporary, HistoryEventDefOf.MemberStripped, new GlobalTargetInfo?(member));
			}
		}

		// Token: 0x06008BF1 RID: 35825 RVA: 0x0032C138 File Offset: 0x0032A338
		public void Notify_MemberDied(Pawn member, DamageInfo? dinfo, bool wasWorldPawn, bool wasGuilty, Map map)
		{
			if (this.ideos != null)
			{
				this.ideos.Notify_MemberGainedOrLost();
			}
			if (this.IsPlayer)
			{
				if (ModsConfig.BiotechActive && map != null && member.IsBloodfeeder())
				{
					this.CheckForPrisonersAssignedToBloodfeederInteractionMode(map, member);
				}
				return;
			}
			if (!wasWorldPawn && !PawnGenerator.IsBeingGenerated(member) && Current.ProgramState == ProgramState.Playing && map != null && !member.IsSubhuman && !member.IsSlaveOfColony && map.IsPlayerHome && !this.HostileTo(Faction.OfPlayer))
			{
				if (dinfo != null && dinfo.Value.Category == DamageInfo.SourceCategory.Collapse)
				{
					bool canSendMessage = MessagesRepeatAvoider.MessageShowAllowed("FactionRelationAdjustmentCrushed-" + this.Name, 5f);
					int goodwillChange = member.RaceProps.Humanlike ? -25 : -15;
					Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange, canSendMessage, !this.temporary, HistoryEventDefOf.MemberCrushed, new GlobalTargetInfo?(new TargetInfo(member.Position, map, false)));
				}
				else if (dinfo != null && (dinfo.Value.Instigator == null || dinfo.Value.Instigator.Faction == null))
				{
					Pawn pawn = dinfo.Value.Instigator as Pawn;
					if (pawn != null && pawn.RaceProps.Animal)
					{
						Pawn_MindState mindState = pawn.mindState;
						if (((mindState != null) ? mindState.mentalStateHandler.CurStateDef : null) == MentalStateDefOf.ManhunterPermanent)
						{
							goto IL_251;
						}
					}
					int goodwillChange2 = member.RaceProps.Humanlike ? -5 : -3;
					Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange2, true, !this.temporary, HistoryEventDefOf.MemberNeutrallyDied, new GlobalTargetInfo?(member));
				}
				else if ((member.kindDef.factionHostileOnDeath || (member.kindDef.factionHostileOnKill && dinfo != null && dinfo.Value.Instigator != null && dinfo.Value.Instigator.Faction == Faction.OfPlayer)) && !wasGuilty)
				{
					Faction.OfPlayer.TryAffectGoodwillWith(this, Faction.OfPlayer.GoodwillToMakeHostile(this), true, !this.temporary, HistoryEventDefOf.MemberKilled, null);
				}
			}
			IL_251:
			if (member == this.leader)
			{
				this.Notify_LeaderDied();
			}
		}

		// Token: 0x06008BF2 RID: 35826 RVA: 0x0032C3A8 File Offset: 0x0032A5A8
		private void CheckForPrisonersAssignedToBloodfeederInteractionMode(Map map, Pawn member)
		{
			List<Pawn> freeColonistsAndPrisoners = map.mapPawns.FreeColonistsAndPrisoners;
			foreach (Pawn pawn in freeColonistsAndPrisoners)
			{
				if (pawn.IsPrisonerOfColony)
				{
					if (pawn.guest.HasInteractionWith((PrisonerInteractionModeDef interaction) => interaction.hideIfNoBloodfeeders))
					{
						this.tmpPrisonersRequiringBloodfeeders.Add(pawn);
					}
				}
			}
			if (!this.tmpPrisonersRequiringBloodfeeders.Any<Pawn>())
			{
				this.tmpPrisonersRequiringBloodfeeders.Clear();
				return;
			}
			foreach (Pawn pawn2 in freeColonistsAndPrisoners)
			{
				if (!pawn2.IsPrisonerOfColony && pawn2.IsBloodfeeder())
				{
					this.tmpPrisonersRequiringBloodfeeders.Clear();
					return;
				}
			}
			Messages.Message("MessageNoBloodfeedersPrisonerInteractionReset".Translate(), this.tmpPrisonersRequiringBloodfeeders, MessageTypeDefOf.NeutralEvent, true);
			foreach (Pawn pawn3 in this.tmpPrisonersRequiringBloodfeeders)
			{
				pawn3.guest.ToggleNonExclusiveInteraction(PrisonerInteractionModeDefOf.Bloodfeed, false);
			}
			this.tmpPrisonersRequiringBloodfeeders.Clear();
		}

		// Token: 0x06008BF3 RID: 35827 RVA: 0x0032C524 File Offset: 0x0032A724
		public void Notify_PawnJoined(Pawn p)
		{
			if (this.ideos != null)
			{
				this.ideos.Notify_MemberGainedOrLost();
			}
			if (p.RaceProps.Humanlike && !this.def.humanlikeFaction && !p.IsSubhuman && !p.IsCreepJoiner && !p.IsDuplicate && !p.Dead)
			{
				Log.Error("Humanlike pawn " + p.LabelShort + " was added to non-humanlike faction " + this.def.label);
			}
		}

		// Token: 0x06008BF4 RID: 35828 RVA: 0x0032C5A4 File Offset: 0x0032A7A4
		public void Notify_LeaderDied()
		{
			Pawn pawn = this.leader;
			string str = "LetterLeadersDeathLabel".Translate(this.name, this.LeaderTitle).Resolve().CapitalizeFirst();
			string text = "LetterLeadersDeath".Translate(this.NameColored, this.LeaderTitle, pawn.Named("OLDLEADER")).Resolve().CapitalizeFirst();
			if (this.TryGenerateNewLeader())
			{
				string str2 = "LetterNewLeader".Translate(this.LeaderTitle, this.leader.Named("NEWLEADER")).Resolve().CapitalizeFirst();
				if (!this.temporary)
				{
					Find.LetterStack.ReceiveLetter(str, text + "\n\n" + str2, LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, this, null, null, null, 0, true);
				}
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"), this.leader.Named("NEWFACTIONLEADER"));
				return;
			}
			if (!this.temporary)
			{
				Find.LetterStack.ReceiveLetter(str, text, LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, this, null, null, null, 0, true);
			}
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"));
		}

		// Token: 0x06008BF5 RID: 35829 RVA: 0x0032C718 File Offset: 0x0032A918
		public void Notify_LeaderLost()
		{
			Pawn pawn = this.leader;
			if (this.TryGenerateNewLeader())
			{
				if (!this.temporary)
				{
					Find.LetterStack.ReceiveLetter("LetterLeaderChangedLabel".Translate(this.name, this.LeaderTitle).Resolve().CapitalizeFirst(), "LetterLeaderChanged".Translate(this.NameColored, this.LeaderTitle, pawn.Named("OLDLEADER")).Resolve().CapitalizeFirst() + "\n\n" + "LetterNewLeader".Translate(this.LeaderTitle, this.leader.Named("NEWLEADER")).Resolve().CapitalizeFirst(), LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, this, null, null, null, 0, true);
				}
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"), this.leader.Named("NEWFACTIONLEADER"));
				return;
			}
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"));
		}

		// Token: 0x06008BF6 RID: 35830 RVA: 0x0032C854 File Offset: 0x0032AA54
		public void Notify_RelationKindChanged(Faction other, FactionRelationKind previousKind, bool canSendLetter, string reason, GlobalTargetInfo lookTarget, out bool sentLetter)
		{
			if (Current.ProgramState != ProgramState.Playing || other != Faction.OfPlayer)
			{
				canSendLetter = false;
			}
			sentLetter = false;
			ColoredText.ClearCache();
			FactionRelationKind factionRelationKind = this.RelationKindWith(other);
			if (factionRelationKind == FactionRelationKind.Hostile)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive.ToList<Pawn>())
					{
						if ((pawn.Faction == this && pawn.HostFaction == other) || (pawn.Faction == other && pawn.HostFaction == this))
						{
							pawn.guest.SetGuestStatus(pawn.HostFaction, GuestStatus.Prisoner);
						}
					}
				}
				if (other == Faction.OfPlayer)
				{
					QuestUtility.SendQuestTargetSignals(this.questTags, "BecameHostileToPlayer", this.Named("SUBJECT"));
				}
			}
			if (other == Faction.OfPlayer && !this.HostileTo(Faction.OfPlayer))
			{
				List<Site> list = new List<Site>();
				List<Site> sites = Find.WorldObjects.Sites;
				for (int i = 0; i < sites.Count; i++)
				{
					if (sites[i].factionMustRemainHostile && sites[i].Faction == this && !sites[i].HasMap)
					{
						list.Add(sites[i]);
					}
				}
				if (list.Any<Site>())
				{
					string str;
					string str2;
					if (list.Count == 1)
					{
						str = "LetterLabelSiteNoLongerHostile".Translate();
						str2 = "LetterSiteNoLongerHostile".Translate(this.NameColored, list[0].Label);
					}
					else
					{
						StringBuilder stringBuilder = new StringBuilder();
						for (int j = 0; j < list.Count; j++)
						{
							if (stringBuilder.Length != 0)
							{
								stringBuilder.AppendLine();
							}
							stringBuilder.Append("  - " + list[j].LabelCap);
							ImportantPawnComp component = list[j].GetComponent<ImportantPawnComp>();
							if (component != null && component.pawn.Any)
							{
								stringBuilder.Append(" (" + component.pawn[0].LabelCap + ")");
							}
						}
						str = "LetterLabelSiteNoLongerHostileMulti".Translate();
						string str3 = "LetterSiteNoLongerHostileMulti".Translate(this.NameColored) + ":\n\n";
						StringBuilder stringBuilder2 = stringBuilder;
						str2 = str3 + ((stringBuilder2 != null) ? stringBuilder2.ToString() : null);
					}
					Find.LetterStack.ReceiveLetter(str, str2, LetterDefOf.NeutralEvent, new LookTargets(from x in list
					select new GlobalTargetInfo(x.Tile)), null, null, null, null, 0, true);
					for (int k = 0; k < list.Count; k++)
					{
						list[k].Destroy();
					}
				}
			}
			if (other == Faction.OfPlayer && this.HostileTo(Faction.OfPlayer))
			{
				List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
				for (int l = 0; l < allWorldObjects.Count; l++)
				{
					if (allWorldObjects[l].Faction == this)
					{
						TradeRequestComp component2 = allWorldObjects[l].GetComponent<TradeRequestComp>();
						if (component2 != null && component2.ActiveRequest)
						{
							component2.Disable();
						}
					}
				}
				foreach (Map map in Find.Maps)
				{
					map.passingShipManager.RemoveAllShipsOfFaction(this);
				}
			}
			if (canSendLetter)
			{
				TaggedString text = "";
				this.TryAppendRelationKindChangedInfo(ref text, previousKind, factionRelationKind, reason);
				if (factionRelationKind == FactionRelationKind.Hostile)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_Hostile".Translate(this.Name), text, LetterDefOf.NegativeEvent, lookTarget, this, null, null, null, 0, true);
					sentLetter = true;
				}
				else if (factionRelationKind == FactionRelationKind.Ally)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_Ally".Translate(this.Name), text, LetterDefOf.PositiveEvent, lookTarget, this, null, null, null, 0, true);
					sentLetter = true;
				}
				else if (factionRelationKind == FactionRelationKind.Neutral)
				{
					if (previousKind == FactionRelationKind.Hostile)
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_NeutralFromHostile".Translate(this.Name), text, LetterDefOf.PositiveEvent, lookTarget, this, null, null, null, 0, true);
						sentLetter = true;
					}
					else
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_NeutralFromAlly".Translate(this.Name), text, LetterDefOf.NeutralEvent, lookTarget, this, null, null, null, 0, true);
						sentLetter = true;
					}
				}
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				List<Map> maps = Find.Maps;
				for (int m = 0; m < maps.Count; m++)
				{
					maps[m].attackTargetsCache.Notify_FactionHostilityChanged(this, other);
					LordManager lordManager = maps[m].lordManager;
					for (int n = 0; n < lordManager.lords.Count; n++)
					{
						Lord lord = lordManager.lords[n];
						if (lord.faction == other)
						{
							lord.Notify_FactionRelationsChanged(this, previousKind);
						}
						else if (lord.faction == this)
						{
							lord.Notify_FactionRelationsChanged(other, previousKind);
						}
					}
				}
			}
		}

		// Token: 0x06008BF7 RID: 35831 RVA: 0x0032CDCC File Offset: 0x0032AFCC
		public void Notify_PlayerTraded(float marketValueSentByPlayer, Pawn playerNegotiator)
		{
			int goodwillChange = (int)(marketValueSentByPlayer / 600f);
			Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange, true, !this.temporary, HistoryEventDefOf.Traded, new GlobalTargetInfo?(playerNegotiator));
		}

		// Token: 0x06008BF8 RID: 35832 RVA: 0x0032CE0C File Offset: 0x0032B00C
		public void Notify_MemberExitedMap(Pawn member, bool freed)
		{
			bool flag = freed && (member.HostFaction == Faction.OfPlayer || member.Faction == Faction.OfPlayer);
			bool flag2 = !freed && !member.Faction.HostileTo(Faction.OfPlayer);
			if (flag || flag2)
			{
				bool flag3;
				bool flag4;
				int goodwillGainForExit = this.GetGoodwillGainForExit(member, freed, out flag3, out flag4);
				if (member.mindState.AvailableForGoodwillReward && (freed || goodwillGainForExit >= 4))
				{
					HistoryEventDef reason = freed ? HistoryEventDefOf.MemberExitedMapHealthy : HistoryEventDefOf.FriendlyExitedMapTended;
					Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillGainForExit, true, !this.temporary, reason, null);
				}
			}
			member.mindState.timesGuestTendedToByPlayer = 0;
		}

		// Token: 0x06008BF9 RID: 35833 RVA: 0x0032CEBC File Offset: 0x0032B0BC
		public int GetGoodwillGainForExit(Pawn member, bool freed, out bool isHealthy, out bool isInMentalState)
		{
			isHealthy = false;
			isInMentalState = member.InMentalState;
			float num = 0f;
			bool flag = !freed && !member.Faction.HostileTo(Faction.OfPlayer) && member.mindState.timesGuestTendedToByPlayer > 0;
			if (member.health.hediffSet.BleedRateTotal < 0.001f && (freed || flag))
			{
				isHealthy = true;
				if (!isInMentalState)
				{
					num += 12f;
					if (PawnUtility.IsFactionLeader(member))
					{
						num += 40f;
					}
				}
			}
			return (int)(num + (float)Mathf.Min(member.mindState.timesGuestTendedToByPlayer, 10) * 1f);
		}

		// Token: 0x06008BFA RID: 35834 RVA: 0x0032CF5A File Offset: 0x0032B15A
		public void Notify_MemberLeftExtraFaction(Pawn member)
		{
			if (member.HomeFaction == this)
			{
				return;
			}
			if (this.leader == member)
			{
				this.Notify_LeaderLost();
			}
		}

		// Token: 0x06008BFB RID: 35835 RVA: 0x0032CF75 File Offset: 0x0032B175
		[Obsolete("Will be removed in the future")]
		public void GenerateNewLeader()
		{
			this.TryGenerateNewLeader();
		}

		// Token: 0x06008BFC RID: 35836 RVA: 0x0032CF80 File Offset: 0x0032B180
		public bool TryGenerateNewLeader()
		{
			Pawn pawn = this.leader;
			this.leader = null;
			if (this.def.generateNewLeaderFromMapMembersOnly)
			{
				for (int i = 0; i < Find.Maps.Count; i++)
				{
					Map map = Find.Maps[i];
					for (int j = 0; j < map.mapPawns.AllPawnsCount; j++)
					{
						if (map.mapPawns.AllPawns[j] != pawn && !map.mapPawns.AllPawns[j].Destroyed && map.mapPawns.AllPawns[j].HomeFaction == this)
						{
							this.leader = map.mapPawns.AllPawns[j];
						}
					}
				}
			}
			else if (this.def.pawnGroupMakers != null)
			{
				List<PawnKindDef> list = new List<PawnKindDef>();
				foreach (PawnGroupMaker pawnGroupMaker in from x in this.def.pawnGroupMakers
				where x.kindDef == PawnGroupKindDefOf.Combat
				select x)
				{
					foreach (PawnGenOption pawnGenOption in pawnGroupMaker.options)
					{
						if (pawnGenOption.kind.factionLeader)
						{
							list.Add(pawnGenOption.kind);
						}
					}
				}
				if (this.def.fixedLeaderKinds != null)
				{
					list.AddRange(this.def.fixedLeaderKinds);
				}
				PawnKindDef pawnKindDef;
				if (list.TryRandomElement(out pawnKindDef))
				{
					PawnKindDef kind = pawnKindDef;
					PawnGenerationContext context = PawnGenerationContext.NonPlayer;
					bool leaderForceGenerateNewPawn = this.def.leaderForceGenerateNewPawn;
					PawnGenerationRequest request = new PawnGenerationRequest(kind, this, context, null, leaderForceGenerateNewPawn, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
					Gender supremeGender = this.ideos.PrimaryIdeo.SupremeGender;
					if (supremeGender != Gender.None)
					{
						request.FixedGender = new Gender?(supremeGender);
					}
					this.leader = PawnGenerator.GeneratePawn(request);
					if (this.leader.RaceProps.IsFlesh)
					{
						this.leader.relations.everSeenByPlayer = true;
					}
					if (!Find.WorldPawns.Contains(this.leader))
					{
						Find.WorldPawns.PassToWorld(this.leader, PawnDiscardDecideMode.KeepForever);
					}
				}
			}
			return this.leader != null;
		}

		// Token: 0x06008BFD RID: 35837 RVA: 0x0032D26C File Offset: 0x0032B46C
		public string GetCallLabel()
		{
			return this.name;
		}

		// Token: 0x06008BFE RID: 35838 RVA: 0x0032D274 File Offset: 0x0032B474
		public string GetInfoText()
		{
			return this.def.LabelCap + ("\n" + "goodwill".Translate().CapitalizeFirst() + ": " + this.PlayerGoodwill.ToStringWithSign());
		}

		// Token: 0x06008BFF RID: 35839 RVA: 0x0032D2D1 File Offset: 0x0032B4D1
		Faction ICommunicable.GetFaction()
		{
			return this;
		}

		// Token: 0x06008C00 RID: 35840 RVA: 0x0032D2D4 File Offset: 0x0032B4D4
		public void TryOpenComms(Pawn negotiator)
		{
			Dialog_Negotiation dialog_Negotiation = new Dialog_Negotiation(negotiator, this, FactionDialogMaker.FactionDialogFor(negotiator, this), true);
			dialog_Negotiation.soundAmbient = SoundDefOf.RadioComms_Ambience;
			Find.WindowStack.Add(dialog_Negotiation);
		}

		// Token: 0x06008C01 RID: 35841 RVA: 0x0032D308 File Offset: 0x0032B508
		private bool LeaderIsAvailableToTalk()
		{
			return this.leader != null && (!this.leader.Spawned || (!this.leader.Downed && !this.leader.IsPrisoner && this.leader.Awake() && !this.leader.InMentalState));
		}

		// Token: 0x06008C02 RID: 35842 RVA: 0x0032D364 File Offset: 0x0032B564
		public FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator)
		{
			if (this.IsPlayer)
			{
				return null;
			}
			string text = "CallOnRadio".Translate(this.GetCallLabel());
			text = string.Concat(new string[]
			{
				text,
				" (",
				this.PlayerRelationKind.GetLabelCap(),
				", ",
				this.PlayerGoodwill.ToStringWithSign(),
				")"
			});
			if (!this.LeaderIsAvailableToTalk())
			{
				string str;
				if (this.leader != null)
				{
					str = "LeaderUnavailable".Translate(this.leader.LabelShort, this.leader);
				}
				else
				{
					str = "LeaderUnavailableNoLeader".Translate();
				}
				return new FloatMenuOption(text + " (" + str + ")", null, this.def.FactionIcon, this.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
			{
				console.GiveUseCommsJob(negotiator, this);
			}, this.def.FactionIcon, this.Color, MenuOptionPriority.InitiateSocial, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false), negotiator, console, "ReservedBy", null);
		}

		// Token: 0x06008C03 RID: 35843 RVA: 0x0032D4C8 File Offset: 0x0032B6C8
		private void TookDamageFromPredator(Pawn predator)
		{
			for (int i = 0; i < this.predatorThreats.Count; i++)
			{
				if (this.predatorThreats[i].predator == predator)
				{
					this.predatorThreats[i].lastAttackTicks = Find.TickManager.TicksGame;
					return;
				}
			}
			PredatorThreat predatorThreat = new PredatorThreat();
			predatorThreat.predator = predator;
			predatorThreat.lastAttackTicks = Find.TickManager.TicksGame;
			this.predatorThreats.Add(predatorThreat);
		}

		// Token: 0x06008C04 RID: 35844 RVA: 0x0032D544 File Offset: 0x0032B744
		public bool HasPredatorRecentlyAttackedAnyone(Pawn predator)
		{
			for (int i = 0; i < this.predatorThreats.Count; i++)
			{
				if (this.predatorThreats[i].predator == predator)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008C05 RID: 35845 RVA: 0x0032D580 File Offset: 0x0032B780
		private bool IsMutuallyHostileCrossfire(DamageInfo dinfo)
		{
			return dinfo.Instigator != null && dinfo.IntendedTarget != null && ((dinfo.IntendedTarget.HostileTo(dinfo.Instigator) && dinfo.IntendedTarget.HostileTo(this)) || (dinfo.IntendedTarget.Faction.HostileTo(dinfo.Instigator.Faction) && dinfo.IntendedTarget.Faction.HostileTo(this)));
		}

		// Token: 0x17001841 RID: 6209
		// (get) Token: 0x06008C06 RID: 35846 RVA: 0x0032D5FA File Offset: 0x0032B7FA
		public static Faction OfPlayer
		{
			get
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				if (ofPlayerSilentFail == null)
				{
					Log.Error("Could not find player faction.");
				}
				return ofPlayerSilentFail;
			}
		}

		// Token: 0x17001842 RID: 6210
		// (get) Token: 0x06008C07 RID: 35847 RVA: 0x0032D60E File Offset: 0x0032B80E
		public static Faction OfMechanoids
		{
			get
			{
				return Find.FactionManager.OfMechanoids;
			}
		}

		// Token: 0x17001843 RID: 6211
		// (get) Token: 0x06008C08 RID: 35848 RVA: 0x0032D61A File Offset: 0x0032B81A
		public static Faction OfInsects
		{
			get
			{
				return Find.FactionManager.OfInsects;
			}
		}

		// Token: 0x17001844 RID: 6212
		// (get) Token: 0x06008C09 RID: 35849 RVA: 0x0032D626 File Offset: 0x0032B826
		public static Faction OfAncients
		{
			get
			{
				return Find.FactionManager.OfAncients;
			}
		}

		// Token: 0x17001845 RID: 6213
		// (get) Token: 0x06008C0A RID: 35850 RVA: 0x0032D632 File Offset: 0x0032B832
		public static Faction OfAncientsHostile
		{
			get
			{
				return Find.FactionManager.OfAncientsHostile;
			}
		}

		// Token: 0x17001846 RID: 6214
		// (get) Token: 0x06008C0B RID: 35851 RVA: 0x0032D63E File Offset: 0x0032B83E
		public static Faction OfEmpire
		{
			get
			{
				return Find.FactionManager.OfEmpire;
			}
		}

		// Token: 0x17001847 RID: 6215
		// (get) Token: 0x06008C0C RID: 35852 RVA: 0x0032D64A File Offset: 0x0032B84A
		public static Faction OfPirates
		{
			get
			{
				return Find.FactionManager.OfPirates;
			}
		}

		// Token: 0x17001848 RID: 6216
		// (get) Token: 0x06008C0D RID: 35853 RVA: 0x0032D656 File Offset: 0x0032B856
		public static Faction OfHoraxCult
		{
			get
			{
				return Find.FactionManager.OfHoraxCult;
			}
		}

		// Token: 0x17001849 RID: 6217
		// (get) Token: 0x06008C0E RID: 35854 RVA: 0x0032D662 File Offset: 0x0032B862
		public static Faction OfEntities
		{
			get
			{
				return Find.FactionManager.OfEntities;
			}
		}

		// Token: 0x1700184A RID: 6218
		// (get) Token: 0x06008C0F RID: 35855 RVA: 0x0032D66E File Offset: 0x0032B86E
		public static Faction OfTradersGuild
		{
			get
			{
				return Find.FactionManager.OfTradersGuild;
			}
		}

		// Token: 0x1700184B RID: 6219
		// (get) Token: 0x06008C10 RID: 35856 RVA: 0x0032D67A File Offset: 0x0032B87A
		public static Faction OfSalvagers
		{
			get
			{
				return Find.FactionManager.OfSalvagers;
			}
		}

		// Token: 0x1700184C RID: 6220
		// (get) Token: 0x06008C11 RID: 35857 RVA: 0x0032D688 File Offset: 0x0032B888
		public static Faction OfPlayerSilentFail
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					GameInitData gameInitData = Find.GameInitData;
					if (gameInitData != null && gameInitData.playerFaction != null)
					{
						return gameInitData.playerFaction;
					}
				}
				return Find.FactionManager.OfPlayer;
			}
		}

		// Token: 0x06008C12 RID: 35858 RVA: 0x0032D6C0 File Offset: 0x0032B8C0
		public void Notify_RoyalThingUseViolation(Def implantOrWeapon, Pawn pawn, string violationSourceName, float detectionChance, int violationSourceLevel = 0)
		{
			if (!this.HostileTo(Faction.OfPlayer))
			{
				RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(implantOrWeapon, this, violationSourceLevel);
				string arg = (minTitleToUse == null) ? "None".Translate() : minTitleToUse.GetLabelCapFor(pawn);
				Faction.OfPlayer.TryAffectGoodwillWith(this, -4, true, true, HistoryEventDefOf.UsedForbiddenThing, new GlobalTargetInfo?(pawn));
				Find.LetterStack.ReceiveLetter("LetterLawViolationDetectedLabel".Translate(pawn.Named("PAWN")).CapitalizeFirst(), "LetterLawViolationDetectedForbiddenThingUse".Translate(arg.Named("TITLE"), pawn.Named("PAWN"), violationSourceName.Named("CULPRIT"), this.Named("FACTION"), 4.ToString().Named("GOODWILL"), detectionChance.ToStringPercent().Named("CHANCE")), LetterDefOf.NegativeEvent, pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06008C13 RID: 35859 RVA: 0x0032D7B8 File Offset: 0x0032B9B8
		public RoyalTitleDef GetMinTitleForImplant(HediffDef implantDef, int level = 0)
		{
			if (this.def.royalImplantRules == null || this.def.royalImplantRules.Count == 0)
			{
				return null;
			}
			foreach (RoyalImplantRule royalImplantRule in this.def.royalImplantRules)
			{
				if (royalImplantRule.implantHediff == implantDef && (royalImplantRule.maxLevel >= level || level == 0))
				{
					return royalImplantRule.minTitle;
				}
			}
			return null;
		}

		// Token: 0x06008C14 RID: 35860 RVA: 0x0032D84C File Offset: 0x0032BA4C
		public RoyalImplantRule GetMaxAllowedImplantLevel(HediffDef implantDef, RoyalTitleDef title)
		{
			if (this.def.royalImplantRules == null || this.def.royalImplantRules.Count == 0)
			{
				return null;
			}
			if (title == null)
			{
				return null;
			}
			int myTitleIdx = this.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(title);
			return this.def.royalImplantRules.Where(delegate(RoyalImplantRule r)
			{
				if (r.implantHediff == implantDef)
				{
					RoyalTitleDef minTitleForImplant = this.GetMinTitleForImplant(implantDef, r.maxLevel);
					int num = this.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleForImplant);
					return myTitleIdx == -1 || myTitleIdx >= num;
				}
				return false;
			}).Last<RoyalImplantRule>();
		}

		// Token: 0x06008C15 RID: 35861 RVA: 0x0032D8CC File Offset: 0x0032BACC
		public static Pair<Faction, RoyalTitleDef> GetMinTitleForImplantAllFactions(HediffDef implantDef)
		{
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				RoyalTitleDef minTitleForImplant = faction.GetMinTitleForImplant(implantDef, 0);
				if (minTitleForImplant != null)
				{
					return new Pair<Faction, RoyalTitleDef>(faction, minTitleForImplant);
				}
			}
			return default(Pair<Faction, RoyalTitleDef>);
		}

		// Token: 0x06008C16 RID: 35862 RVA: 0x0032D940 File Offset: 0x0032BB40
		public void ChangeGoodwill_Debug(Faction other, int goodwillChange)
		{
			int baseGoodwill = Mathf.Clamp(this.BaseGoodwillWith(other) + goodwillChange, -100, 100);
			FactionRelation factionRelation = this.RelationWith(other, false);
			factionRelation.baseGoodwill = baseGoodwill;
			bool flag;
			factionRelation.CheckKindThresholds(this, false, null, GlobalTargetInfo.Invalid, out flag);
			FactionRelation factionRelation2 = other.RelationWith(this, false);
			factionRelation2.baseGoodwill = factionRelation.baseGoodwill;
			factionRelation2.kind = factionRelation.kind;
		}

		// Token: 0x06008C17 RID: 35863 RVA: 0x0032D99F File Offset: 0x0032BB9F
		public string GetUniqueLoadID()
		{
			return "Faction_" + this.loadID.ToString();
		}

		// Token: 0x06008C18 RID: 35864 RVA: 0x0032D9B6 File Offset: 0x0032BBB6
		public override string ToString()
		{
			if (this.name != null)
			{
				return this.name;
			}
			if (this.def != null)
			{
				return this.def.defName;
			}
			return "[faction of no def]";
		}

		// Token: 0x06008C1A RID: 35866 RVA: 0x0032D9EC File Offset: 0x0032BBEC
		[CompilerGenerated]
		internal static int <TryMakeInitialRelationsWith>g__GetInitialGoodwill|67_0(Faction a, Faction b)
		{
			if (a.def.permanentEnemy)
			{
				return -100;
			}
			if (a.def.permanentEnemyToEveryoneExceptPlayer && !b.IsPlayer)
			{
				return -100;
			}
			if (a.def.permanentEnemyToEveryoneExcept != null && !a.def.permanentEnemyToEveryoneExcept.Contains(b.def))
			{
				return -100;
			}
			if (a.def.naturalEnemy)
			{
				return -80;
			}
			return 0;
		}

		// Token: 0x040055F5 RID: 22005
		public FactionDef def;

		// Token: 0x040055F6 RID: 22006
		private string name;

		// Token: 0x040055F7 RID: 22007
		public int loadID = -1;

		// Token: 0x040055F8 RID: 22008
		public int randomKey;

		// Token: 0x040055F9 RID: 22009
		public float colorFromSpectrum = -999f;

		// Token: 0x040055FA RID: 22010
		private List<FactionRelation> relations = new List<FactionRelation>();

		// Token: 0x040055FB RID: 22011
		public Pawn leader;

		// Token: 0x040055FC RID: 22012
		public KidnappedPawnsTracker kidnapped;

		// Token: 0x040055FD RID: 22013
		private List<PredatorThreat> predatorThreats = new List<PredatorThreat>();

		// Token: 0x040055FE RID: 22014
		public bool defeated;

		// Token: 0x040055FF RID: 22015
		public int lastTraderRequestTick = -9999999;

		// Token: 0x04005600 RID: 22016
		public int lastOrbitalTraderRequestTick = -9999999;

		// Token: 0x04005601 RID: 22017
		public int lastMilitaryAidRequestTick = -9999999;

		// Token: 0x04005602 RID: 22018
		public int lastExecutionTick = -9999999;

		// Token: 0x04005603 RID: 22019
		private int naturalGoodwillTimer;

		// Token: 0x04005604 RID: 22020
		public bool allowRoyalFavorRewards = true;

		// Token: 0x04005605 RID: 22021
		public bool allowGoodwillRewards = true;

		// Token: 0x04005606 RID: 22022
		public List<string> questTags;

		// Token: 0x04005607 RID: 22023
		public Color? color;

		// Token: 0x04005608 RID: 22024
		public Color? allegianceColor;

		// Token: 0x04005609 RID: 22025
		public bool? hidden;

		// Token: 0x0400560A RID: 22026
		public bool temporary;

		// Token: 0x0400560B RID: 22027
		public bool factionHostileOnHarmByPlayer;

		// Token: 0x0400560C RID: 22028
		public bool neverFlee;

		// Token: 0x0400560D RID: 22029
		public FactionIdeosTracker ideos;

		// Token: 0x0400560E RID: 22030
		public bool deactivated;

		// Token: 0x0400560F RID: 22031
		private List<Map> avoidGridsBasicKeysWorkingList;

		// Token: 0x04005610 RID: 22032
		private List<ByteGrid> avoidGridsBasicValuesWorkingList;

		// Token: 0x04005611 RID: 22033
		private List<Map> avoidGridsSmartKeysWorkingList;

		// Token: 0x04005612 RID: 22034
		private List<ByteGrid> avoidGridsSmartValuesWorkingList;

		// Token: 0x04005613 RID: 22035
		private const int MinGoodwillForGainFriendlyExit = 4;

		// Token: 0x04005614 RID: 22036
		private static List<PawnKindDef> allPawnKinds = new List<PawnKindDef>();

		// Token: 0x04005615 RID: 22037
		private List<Pawn> tmpPrisonersRequiringBloodfeeders = new List<Pawn>();

		// Token: 0x04005616 RID: 22038
		public const int RoyalThingUseViolationGoodwillImpact = 4;
	}
}
