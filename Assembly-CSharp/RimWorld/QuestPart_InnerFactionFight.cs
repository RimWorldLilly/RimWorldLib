using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001082 RID: 4226
	public class QuestPart_InnerFactionFight : QuestPartActivable
	{
		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x060069E0 RID: 27104 RVA: 0x00254A50 File Offset: 0x00252C50
		private bool OneSideDefeated
		{
			get
			{
				if (this.secondFaction == null)
				{
					return false;
				}
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].Destroyed && !this.pawns[i].Downed)
					{
						if (this.pawns[i].Faction == this.firstFaction)
						{
							num++;
						}
						else if (this.pawns[i].Faction == this.secondFaction)
						{
							num2++;
						}
					}
				}
				return num == 0 || num2 == 0;
			}
		}

		// Token: 0x060069E1 RID: 27105 RVA: 0x00254AEC File Offset: 0x00252CEC
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			if (!this.OneSideDefeated)
			{
				List<FactionRelation> list = new List<FactionRelation>();
				foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
				{
					if (faction == this.firstFaction)
					{
						list.Add(new FactionRelation(faction, FactionRelationKind.Hostile));
					}
					else if (!faction.def.PermanentlyHostileTo(this.firstFaction.def))
					{
						list.Add(new FactionRelation(faction, FactionRelationKind.Neutral));
					}
				}
				this.secondFaction = FactionGenerator.NewGeneratedFactionWithRelations(this.secondFactionDef, list, true);
				this.secondFaction.temporary = true;
				Find.FactionManager.Add(this.secondFaction);
				List<Thing> list2 = new List<Thing>();
				List<Thing> list3 = new List<Thing>();
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].Destroyed)
					{
						if (i % 2 == 0)
						{
							this.pawns[i].SetFaction(this.secondFaction, null);
							list3.Add(this.pawns[i]);
						}
						else
						{
							list2.Add(this.pawns[i]);
							Lord lord = this.pawns[i].GetLord();
							if (lord != null)
							{
								lord.RemovePawn(this.pawns[i]);
							}
						}
					}
				}
				Lord lord2 = LordMaker.MakeNewLord(this.firstFaction, new LordJob_AssaultThings(this.firstFaction, list3, 1f, false), this.mapParent.Map, null);
				foreach (Thing thing in list2)
				{
					lord2.AddPawn((Pawn)thing);
				}
				lord2.inSignalLeave = this.outSignalComplete;
				Lord lord3 = LordMaker.MakeNewLord(this.secondFaction, new LordJob_AssaultThings(this.secondFaction, list2, 1f, false), this.mapParent.Map, null);
				foreach (Thing thing2 in list3)
				{
					lord3.AddPawn((Pawn)thing2);
				}
				lord3.inSignalLeave = this.outSignalComplete;
				if (!this.outSignalEnabled.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalEnabled, receivedArgs, false));
					return;
				}
			}
			else
			{
				base.Complete();
			}
		}

		// Token: 0x060069E2 RID: 27106 RVA: 0x00254D9C File Offset: 0x00252F9C
		public override void QuestPartTick()
		{
			if (this.OneSideDefeated)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignalComplete, false));
				base.Complete();
			}
		}

		// Token: 0x060069E3 RID: 27107 RVA: 0x00254DC2 File Offset: 0x00252FC2
		public override bool QuestPartReserves(Faction f)
		{
			return f == this.firstFaction || (this.secondFaction != null && f == this.secondFaction);
		}

		// Token: 0x060069E4 RID: 27108 RVA: 0x00254DE2 File Offset: 0x00252FE2
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.firstFaction == f)
			{
				this.firstFaction = null;
				return;
			}
			if (this.secondFaction == f)
			{
				this.secondFaction = null;
			}
		}

		// Token: 0x060069E5 RID: 27109 RVA: 0x00254E08 File Offset: 0x00253008
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.factionBecameHostileSignal && this.secondFaction != null)
			{
				this.secondFaction.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
			}
		}

		// Token: 0x060069E6 RID: 27110 RVA: 0x00254E54 File Offset: 0x00253054
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.firstFaction, "firstFaction", false);
			Scribe_Defs.Look<FactionDef>(ref this.secondFactionDef, "secondFactionDef");
			Scribe_References.Look<Faction>(ref this.secondFaction, "secondFaction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignalEnabled, "outSignalEnabled", null, false);
			Scribe_Values.Look<string>(ref this.outSignalComplete, "outSignalComplete", null, false);
			Scribe_Values.Look<IntVec3>(ref this.meetingPos, "meetingPos", default(IntVec3), false);
			Scribe_Values.Look<string>(ref this.factionBecameHostileSignal, "factionBecameHostileSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400434F RID: 17231
		public Faction firstFaction;

		// Token: 0x04004350 RID: 17232
		public FactionDef secondFactionDef;

		// Token: 0x04004351 RID: 17233
		public Faction secondFaction;

		// Token: 0x04004352 RID: 17234
		public IntVec3 meetingPos;

		// Token: 0x04004353 RID: 17235
		public MapParent mapParent;

		// Token: 0x04004354 RID: 17236
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04004355 RID: 17237
		public List<string> inSignals = new List<string>();

		// Token: 0x04004356 RID: 17238
		public string outSignalEnabled;

		// Token: 0x04004357 RID: 17239
		public string outSignalComplete;

		// Token: 0x04004358 RID: 17240
		public string factionBecameHostileSignal;
	}
}
