using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020010DD RID: 4317
	public class QuestPart_RefugeeInteractions : QuestPartActivable
	{
		// Token: 0x06006BEA RID: 27626 RVA: 0x0025DE40 File Offset: 0x0025C040
		protected override void ProcessQuestSignal(Signal signal)
		{
			Pawn item;
			if (signal.tag == this.inSignalRecruited && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
				if (this.pawns.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalLast_Recruited, signal.args, false));
				}
			}
			Pawn item2;
			if (signal.tag == this.inSignalKidnapped && signal.args.TryGetArg<Pawn>("SUBJECT", out item2) && this.pawns.Contains(item2))
			{
				this.pawns.Remove(item2);
				if (this.pawns.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalLast_Kidnapped, signal.args, false));
				}
			}
			Pawn item3;
			if (signal.tag == this.inSignalBanished && signal.args.TryGetArg<Pawn>("SUBJECT", out item3) && this.pawns.Contains(item3))
			{
				this.pawns.Remove(item3);
				if (this.pawns.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalLast_Banished, signal.args, false));
				}
			}
			Pawn pawn;
			if (signal.tag == this.inSignalLeftMap && signal.args.TryGetArg<Pawn>("SUBJECT", out pawn) && this.pawns.Contains(pawn))
			{
				this.pawns.Remove(pawn);
				if (pawn.Destroyed || pawn.InMentalState || pawn.health.hediffSet.BleedRateTotal > 0.001f)
				{
					this.pawnsLeftUnhealthy++;
				}
				int num = this.pawns.Count((Pawn p) => p.Downed);
				if (this.pawns.Count - num <= 0)
				{
					if (this.pawnsLeftUnhealthy > 0 || num > 0)
					{
						this.pawns.Clear();
						this.pawnsLeftUnhealthy += num;
						Find.SignalManager.SendSignal(new Signal(this.outSignalLast_LeftMapAllNotHealthy, signal.args, false));
					}
					else
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalLast_LeftMapAllHealthy, signal.args, false));
					}
				}
			}
			Pawn item4;
			if (signal.tag == this.inSignalLeftBehind && signal.args.TryGetArg<Pawn>("SUBJECT", out item4) && this.pawns.Contains(item4))
			{
				this.pawns.Remove(item4);
				if (this.pawns.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalLast_LeftBehind, signal.args, false));
				}
				else
				{
					signal.args.Add(this.pawns.Count.Named("PAWNSALIVECOUNT"));
					QuestPart_RefugeeInteractions.InteractionResponseType interactionResponseType = this.ChooseRandomInteraction();
					if (interactionResponseType > QuestPart_RefugeeInteractions.InteractionResponseType.Leave)
					{
						if (interactionResponseType == QuestPart_RefugeeInteractions.InteractionResponseType.BadThought)
						{
							Find.SignalManager.SendSignal(new Signal(this.outSignalLeftBehind_BadThought, signal.args, false));
						}
					}
					else
					{
						this.LeavePlayer();
						Find.SignalManager.SendSignal(new Signal(this.outSignalLeftBehind_LeaveColony, signal.args, false));
					}
				}
			}
			Pawn pawn2;
			if (signal.tag == this.inSignalDestroyed && signal.args.TryGetArg<Pawn>("SUBJECT", out pawn2) && this.pawns.Contains(pawn2))
			{
				this.pawns.Remove(pawn2);
				pawn2.SetFaction(this.faction, null);
				if (this.pawns.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalLast_Destroyed, signal.args, false));
				}
				else
				{
					signal.args.Add(this.pawns.Count.Named("PAWNSALIVECOUNT"));
					switch (this.ChooseRandomInteraction())
					{
					case QuestPart_RefugeeInteractions.InteractionResponseType.AssaultColony:
						this.AssaultColony(HistoryEventDefOf.QuestPawnLost);
						Find.SignalManager.SendSignal(new Signal(this.outSignalDestroyed_AssaultColony, signal.args, false));
						break;
					case QuestPart_RefugeeInteractions.InteractionResponseType.Leave:
						this.LeavePlayer();
						Find.SignalManager.SendSignal(new Signal(this.outSignalDestroyed_LeaveColony, signal.args, false));
						break;
					case QuestPart_RefugeeInteractions.InteractionResponseType.BadThought:
						Find.SignalManager.SendSignal(new Signal(this.outSignalDestroyed_BadThought, signal.args, false));
						break;
					}
				}
			}
			Pawn pawn3;
			if (signal.tag == this.inSignalArrested && signal.args.TryGetArg<Pawn>("SUBJECT", out pawn3) && this.pawns.Contains(pawn3))
			{
				this.pawns.Remove(pawn3);
				bool inAggroMentalState = pawn3.InAggroMentalState;
				if (this.pawns.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalLast_Arrested, signal.args, false));
				}
				else if (!inAggroMentalState)
				{
					signal.args.Add(this.pawns.Count.Named("PAWNSALIVECOUNT"));
					switch (this.ChooseRandomInteraction())
					{
					case QuestPart_RefugeeInteractions.InteractionResponseType.AssaultColony:
						this.AssaultColony(HistoryEventDefOf.QuestPawnArrested);
						Find.SignalManager.SendSignal(new Signal(this.outSignalArrested_AssaultColony, signal.args, false));
						break;
					case QuestPart_RefugeeInteractions.InteractionResponseType.Leave:
						this.LeavePlayer();
						Find.SignalManager.SendSignal(new Signal(this.outSignalArrested_LeaveColony, signal.args, false));
						break;
					case QuestPart_RefugeeInteractions.InteractionResponseType.BadThought:
						Find.SignalManager.SendSignal(new Signal(this.outSignalArrested_BadThought, signal.args, false));
						break;
					}
				}
			}
			Pawn item5;
			if (signal.tag == this.inSignalSurgeryViolation && signal.args.TryGetArg<Pawn>("SUBJECT", out item5) && this.pawns.Contains(item5))
			{
				signal.args.Add(this.pawns.Count.Named("PAWNSALIVECOUNT"));
				switch (this.ChooseRandomInteraction())
				{
				case QuestPart_RefugeeInteractions.InteractionResponseType.AssaultColony:
					this.AssaultColony(HistoryEventDefOf.PerformedHarmfulSurgery);
					Find.SignalManager.SendSignal(new Signal(this.outSignalSurgeryViolation_AssaultColony, signal.args, false));
					break;
				case QuestPart_RefugeeInteractions.InteractionResponseType.Leave:
					this.LeavePlayer();
					Find.SignalManager.SendSignal(new Signal(this.outSignalSurgeryViolation_LeaveColony, signal.args, false));
					break;
				case QuestPart_RefugeeInteractions.InteractionResponseType.BadThought:
					Find.SignalManager.SendSignal(new Signal(this.outSignalSurgeryViolation_BadThought, signal.args, false));
					break;
				}
			}
			Pawn item6;
			if (signal.tag == this.inSignalPsychicRitualTarget && signal.args.TryGetArg<Pawn>("SUBJECT", out item6) && this.pawns.Contains(item6))
			{
				signal.args.Add(this.pawns.Count.Named("PAWNSALIVECOUNT"));
				switch (this.ChooseRandomInteraction())
				{
				case QuestPart_RefugeeInteractions.InteractionResponseType.AssaultColony:
					this.AssaultColony(HistoryEventDefOf.WasPsychicRitualTarget);
					Find.SignalManager.SendSignal(new Signal(this.outSignalPsychicRitualTarget_AssaultColony, signal.args, false));
					break;
				case QuestPart_RefugeeInteractions.InteractionResponseType.Leave:
					this.LeavePlayer();
					Find.SignalManager.SendSignal(new Signal(this.outSignalPsychicRitualTarget_LeaveColony, signal.args, false));
					break;
				case QuestPart_RefugeeInteractions.InteractionResponseType.BadThought:
					Find.SignalManager.SendSignal(new Signal(this.outSignalPsychicRitualTarget_BadThought, signal.args, false));
					break;
				}
			}
			if (this.inSignalAssaultColony != null && signal.tag == this.inSignalAssaultColony)
			{
				this.AssaultColony(null);
			}
		}

		// Token: 0x06006BEB RID: 27627 RVA: 0x0025E60C File Offset: 0x0025C80C
		private void LeavePlayer()
		{
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.faction != this.pawns[i].Faction)
				{
					this.pawns[i].SetFaction(this.faction, null);
				}
			}
			LeaveQuestPartUtility.MakePawnsLeave(this.pawns, false, this.quest, false);
			base.Complete();
		}

		// Token: 0x06006BEC RID: 27628 RVA: 0x0025E67C File Offset: 0x0025C87C
		private void AssaultColony(HistoryEventDef reason)
		{
			if (this.faction.HasGoodwill)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this.faction, Faction.OfPlayer.GoodwillToMakeHostile(this.faction), true, false, reason, null);
			}
			else
			{
				this.faction.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
			}
			for (int i = 0; i < this.pawns.Count; i++)
			{
				Lord lord = this.pawns[i].GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this.pawns[i], PawnLostCondition.ForcedByQuest, null);
				}
			}
			for (int j = 0; j < this.pawns.Count; j++)
			{
				this.pawns[j].SetFaction(this.faction, null);
				if (!this.pawns[j].Awake())
				{
					RestUtility.WakeUp(this.pawns[j], true);
				}
			}
			Dictionary<Map, Lord> dictionary = new Dictionary<Map, Lord>();
			for (int k = 0; k < this.pawns.Count; k++)
			{
				Pawn pawn = this.pawns[k];
				Map mapHeld = pawn.MapHeld;
				if (!pawn.Dead)
				{
					Lord lord2;
					if (!dictionary.TryGetValue(mapHeld, out lord2))
					{
						lord2 = (dictionary[mapHeld] = LordMaker.MakeNewLord(this.faction, new LordJob_AssaultColony(this.faction, true, true, false, false, true, false, true), mapHeld, null));
					}
					lord2.AddPawn(pawn);
				}
			}
			base.Complete();
		}

		// Token: 0x06006BED RID: 27629 RVA: 0x0025E80B File Offset: 0x0025CA0B
		private QuestPart_RefugeeInteractions.InteractionResponseType ChooseRandomInteraction()
		{
			return Gen.RandomEnumValue<QuestPart_RefugeeInteractions.InteractionResponseType>(false);
		}

		// Token: 0x06006BEE RID: 27630 RVA: 0x0025E813 File Offset: 0x0025CA13
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06006BEF RID: 27631 RVA: 0x0025E828 File Offset: 0x0025CA28
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalDestroyed, "inSignalDestroyed", null, false);
			Scribe_Values.Look<string>(ref this.inSignalArrested, "inSignalArrested", null, false);
			Scribe_Values.Look<string>(ref this.inSignalSurgeryViolation, "inSignalSurgeryViolation", null, false);
			Scribe_Values.Look<string>(ref this.inSignalPsychicRitualTarget, "inSignalPsychicRitualTarget", null, false);
			Scribe_Values.Look<string>(ref this.inSignalRecruited, "inSignalRecruited", null, false);
			Scribe_Values.Look<string>(ref this.inSignalKidnapped, "inSignalKidnapped", null, false);
			Scribe_Values.Look<string>(ref this.inSignalAssaultColony, "inSignalAssaultColony", null, false);
			Scribe_Values.Look<string>(ref this.inSignalLeftMap, "inSignalLeftMap", null, false);
			Scribe_Values.Look<string>(ref this.inSignalLeftBehind, "inSignalLeftBehind", null, false);
			Scribe_Values.Look<string>(ref this.inSignalBanished, "inSignalBanished", null, false);
			Scribe_Values.Look<string>(ref this.outSignalDestroyed_AssaultColony, "outSignalDestroyed_AssaultColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalDestroyed_LeaveColony, "outSignalDestroyed_LeaveColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalDestroyed_BadThought, "outSignalDestroyed_BadThought", null, false);
			Scribe_Values.Look<string>(ref this.outSignalArrested_AssaultColony, "outSignalArrested_AssaultColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalArrested_LeaveColony, "outSignalArrested_LeaveColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalArrested_BadThought, "outSignalArrested_BadThought", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLeftBehind_LeaveColony, "outSignalLeftBehind_LeaveColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLeftBehind_BadThought, "outSignalLeftBehind_BadThought", null, false);
			Scribe_Values.Look<string>(ref this.outSignalSurgeryViolation_AssaultColony, "outSignalSurgeryViolation_AssaultColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalSurgeryViolation_LeaveColony, "outSignalSurgeryViolation_LeaveColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalSurgeryViolation_BadThought, "outSignalSurgeryViolation_BadThought", null, false);
			Scribe_Values.Look<string>(ref this.outSignalPsychicRitualTarget_AssaultColony, "outSignalPsychicRitualTarget_AssaultColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalPsychicRitualTarget_LeaveColony, "outSignalPsychicRitualTarget_LeaveColony", null, false);
			Scribe_Values.Look<string>(ref this.outSignalPsychicRitualTarget_BadThought, "outSignalPsychicRitualTarget_BadThought", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_Arrested, "outSignalLastArrested", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_Destroyed, "outSignalLastDestroyed", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_Kidnapped, "outSignalLastKidnapped", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_Recruited, "outSignalLastRecruited", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_LeftMapAllHealthy, "outSignalLastLeftMapAllHealthy", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_LeftMapAllNotHealthy, "outSignalLastLeftMapAllNotHealthy", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_LeftBehind, "outSignalLast_LeftBehind", null, false);
			Scribe_Values.Look<string>(ref this.outSignalLast_Banished, "outSignalLast_Banished", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.pawnsLeftUnhealthy, "pawnsLeftUnhealthy", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x040044A3 RID: 17571
		public string inSignalDestroyed;

		// Token: 0x040044A4 RID: 17572
		public string inSignalArrested;

		// Token: 0x040044A5 RID: 17573
		public string inSignalSurgeryViolation;

		// Token: 0x040044A6 RID: 17574
		public string inSignalPsychicRitualTarget;

		// Token: 0x040044A7 RID: 17575
		public string inSignalRecruited;

		// Token: 0x040044A8 RID: 17576
		public string inSignalKidnapped;

		// Token: 0x040044A9 RID: 17577
		public string inSignalAssaultColony;

		// Token: 0x040044AA RID: 17578
		public string inSignalLeftMap;

		// Token: 0x040044AB RID: 17579
		public string inSignalLeftBehind;

		// Token: 0x040044AC RID: 17580
		public string inSignalBanished;

		// Token: 0x040044AD RID: 17581
		public string outSignalDestroyed_AssaultColony;

		// Token: 0x040044AE RID: 17582
		public string outSignalDestroyed_LeaveColony;

		// Token: 0x040044AF RID: 17583
		public string outSignalDestroyed_BadThought;

		// Token: 0x040044B0 RID: 17584
		public string outSignalArrested_AssaultColony;

		// Token: 0x040044B1 RID: 17585
		public string outSignalArrested_LeaveColony;

		// Token: 0x040044B2 RID: 17586
		public string outSignalArrested_BadThought;

		// Token: 0x040044B3 RID: 17587
		public string outSignalLeftBehind_LeaveColony;

		// Token: 0x040044B4 RID: 17588
		public string outSignalLeftBehind_BadThought;

		// Token: 0x040044B5 RID: 17589
		public string outSignalSurgeryViolation_AssaultColony;

		// Token: 0x040044B6 RID: 17590
		public string outSignalSurgeryViolation_LeaveColony;

		// Token: 0x040044B7 RID: 17591
		public string outSignalSurgeryViolation_BadThought;

		// Token: 0x040044B8 RID: 17592
		public string outSignalPsychicRitualTarget_AssaultColony;

		// Token: 0x040044B9 RID: 17593
		public string outSignalPsychicRitualTarget_LeaveColony;

		// Token: 0x040044BA RID: 17594
		public string outSignalPsychicRitualTarget_BadThought;

		// Token: 0x040044BB RID: 17595
		public string outSignalLast_Arrested;

		// Token: 0x040044BC RID: 17596
		public string outSignalLast_Destroyed;

		// Token: 0x040044BD RID: 17597
		public string outSignalLast_Kidnapped;

		// Token: 0x040044BE RID: 17598
		public string outSignalLast_Recruited;

		// Token: 0x040044BF RID: 17599
		public string outSignalLast_LeftMapAllHealthy;

		// Token: 0x040044C0 RID: 17600
		public string outSignalLast_LeftMapAllNotHealthy;

		// Token: 0x040044C1 RID: 17601
		public string outSignalLast_Banished;

		// Token: 0x040044C2 RID: 17602
		public string outSignalLast_LeftBehind;

		// Token: 0x040044C3 RID: 17603
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040044C4 RID: 17604
		public Faction faction;

		// Token: 0x040044C5 RID: 17605
		public MapParent mapParent;

		// Token: 0x040044C6 RID: 17606
		public int pawnsLeftUnhealthy;

		// Token: 0x02003028 RID: 12328
		private enum InteractionResponseType
		{
			// Token: 0x0400C39D RID: 50077
			AssaultColony,
			// Token: 0x0400C39E RID: 50078
			Leave,
			// Token: 0x0400C39F RID: 50079
			BadThought
		}
	}
}
