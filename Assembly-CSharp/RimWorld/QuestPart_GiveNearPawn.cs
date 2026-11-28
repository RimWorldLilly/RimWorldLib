using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001112 RID: 4370
	public class QuestPart_GiveNearPawn : QuestPart
	{
		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x06006D42 RID: 27970 RVA: 0x002641AD File Offset: 0x002623AD
		// (set) Token: 0x06006D43 RID: 27971 RVA: 0x002641C8 File Offset: 0x002623C8
		public IEnumerable<Thing> Things
		{
			get
			{
				return this.items.Concat(this.pawns.Cast<Thing>());
			}
			set
			{
				this.items.Clear();
				this.pawns.Clear();
				if (value != null)
				{
					foreach (Thing thing in value)
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							this.pawns.Add(pawn);
						}
						else
						{
							this.items.Add(thing);
						}
					}
				}
			}
		}

		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x06006D44 RID: 27972 RVA: 0x00264248 File Offset: 0x00262448
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				QuestPart_GiveNearPawn.<get_Hyperlinks>d__19 <get_Hyperlinks>d__ = new QuestPart_GiveNearPawn.<get_Hyperlinks>d__19(-2);
				<get_Hyperlinks>d__.<>4__this = this;
				return <get_Hyperlinks>d__;
			}
		}

		// Token: 0x170012BC RID: 4796
		// (get) Token: 0x06006D45 RID: 27973 RVA: 0x00264258 File Offset: 0x00262458
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_GiveNearPawn.<get_QuestLookTargets>d__21 <get_QuestLookTargets>d__ = new QuestPart_GiveNearPawn.<get_QuestLookTargets>d__21(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x170012BD RID: 4797
		// (get) Token: 0x06006D46 RID: 27974 RVA: 0x00264268 File Offset: 0x00262468
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		// Token: 0x06006D47 RID: 27975 RVA: 0x00264284 File Offset: 0x00262484
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.nearPawn != null && (this.nearPawn.SpawnedOrAnyParentSpawned || this.nearPawn.IsCaravanMember()))
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				this.items.RemoveAll((Thing x) => x.Destroyed);
				this.tmpThingsToGive.Clear();
				this.tmpThingsToGive.AddRange(this.Things);
				for (int i = 0; i < this.thingDefs.Count; i++)
				{
					Thing thing = ThingMaker.MakeThing(this.thingDefs[i].thingDef, GenStuff.RandomStuffByCommonalityFor(this.thingDefs[i].thingDef, TechLevel.Undefined));
					thing.stackCount = this.thingDefs[i].count;
					this.tmpThingsToGive.Add(thing);
				}
				this.tmpThingsToGive.RemoveAll((Thing x) => x.Spawned);
				Thing thing2 = (from x in this.tmpThingsToGive
				where x is Pawn
				select x).MaxByWithFallback((Thing x) => x.MarketValue, null);
				Thing thing3 = this.tmpThingsToGive.MaxByWithFallback((Thing x) => x.MarketValue * (float)x.stackCount, null);
				if (this.tmpThingsToGive.Any<Thing>())
				{
					TaggedString taggedString = null;
					TaggedString taggedString2 = null;
					if (this.sendStandardLetter)
					{
						if (this.nearPawn.SpawnedOrAnyParentSpawned)
						{
							if (this.joinPlayer && this.pawns.Count == 1 && this.pawns[0].RaceProps.Humanlike)
							{
								taggedString = "LetterRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
								taggedString2 = "LetterLabelRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
								PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref taggedString2, this.pawns[0]);
							}
							else
							{
								taggedString = "LetterQuestDropPodsArrived".Translate(GenLabel.ThingsLabel(this.tmpThingsToGive, "  - "));
								taggedString2 = "LetterLabelQuestDropPodsArrived".Translate();
								PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(this.pawns, ref taggedString2, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
							}
							taggedString2 = (this.customDropPodsLetterLabel.NullOrEmpty() ? taggedString2 : this.customDropPodsLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
							taggedString = (this.customDropPodsLetterText.NullOrEmpty() ? taggedString : this.customDropPodsLetterText.Formatted(taggedString.Named("BASETEXT")));
						}
						else if (this.nearPawn.IsCaravanMember())
						{
							taggedString = "LetterQuestItemsAddedToCaravanInventory".Translate(this.nearPawn.GetCaravan().Named("CARAVAN"), GenLabel.ThingsLabel(this.tmpThingsToGive, "  - ").Named("THINGS"));
							taggedString2 = "LetterLabelQuestItemsAddedToCaravanInventory".Translate(this.nearPawn.GetCaravan().Named("CARAVAN"));
							taggedString2 = (this.customCaravanInventoryLetterLabel.NullOrEmpty() ? taggedString2 : this.customCaravanInventoryLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
							taggedString = (this.customCaravanInventoryLetterText.NullOrEmpty() ? taggedString : this.customCaravanInventoryLetterText.Formatted(taggedString.Named("BASETEXT")));
						}
					}
					if (this.joinPlayer)
					{
						for (int j = 0; j < this.pawns.Count; j++)
						{
							if (this.pawns[j].Faction != Faction.OfPlayer)
							{
								this.pawns[j].SetFaction(Faction.OfPlayer, null);
							}
						}
					}
					else if (this.makePrisoners)
					{
						for (int k = 0; k < this.pawns.Count; k++)
						{
							if (this.pawns[k].RaceProps.Humanlike)
							{
								if (!this.pawns[k].IsPrisonerOfColony)
								{
									this.pawns[k].guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Prisoner);
								}
								HealthUtility.TryAnesthetize(this.pawns[k]);
							}
						}
					}
					GlobalTargetInfo target = GlobalTargetInfo.Invalid;
					if (this.nearPawn.SpawnedOrAnyParentSpawned)
					{
						IntVec3 intVec = DropCellFinder.TradeDropSpot(this.nearPawn.MapHeld);
						DropPodUtility.DropThingsNear(intVec, this.nearPawn.MapHeld, this.tmpThingsToGive, 110, false, false, false, false, true, null);
						target = new GlobalTargetInfo(intVec, this.nearPawn.MapHeld, false);
						for (int l = 0; l < this.tmpThingsToGive.Count; l++)
						{
							if (this.tmpThingsToGive[l].SpawnedOrAnyParentSpawned)
							{
								target = new GlobalTargetInfo(this.tmpThingsToGive[l].PositionHeld, this.nearPawn.MapHeld, false);
								break;
							}
						}
					}
					else if (this.nearPawn.IsCaravanMember())
					{
						for (int m = 0; m < this.tmpThingsToGive.Count; m++)
						{
							Pawn pawn = this.tmpThingsToGive[m] as Pawn;
							if (pawn != null)
							{
								if (pawn.Faction != Faction.OfPlayer)
								{
									pawn.SetFaction(Faction.OfPlayer, null);
								}
								this.nearPawn.GetCaravan().AddPawn(pawn, true);
							}
							else
							{
								CaravanInventoryUtility.GiveThing(this.nearPawn.GetCaravan(), this.tmpThingsToGive[m]);
							}
						}
						target = this.nearPawn.GetCaravan();
					}
					if (this.sendStandardLetter)
					{
						Find.LetterStack.ReceiveLetter(taggedString2, taggedString, this.customLetterDef ?? LetterDefOf.PositiveEvent, target, null, this.quest, null, null, 0, true);
					}
					this.importantLookTarget = this.items.Find((Thing x) => x.GetInnerIfMinified() is MonumentMarker).GetInnerIfMinified();
					this.items.Clear();
					if (!this.outSignalResult.NullOrEmpty())
					{
						if (thing2 != null)
						{
							Find.SignalManager.SendSignal(new Signal(this.outSignalResult, thing2.Named("SUBJECT")));
							return;
						}
						if (thing3 != null)
						{
							Find.SignalManager.SendSignal(new Signal(this.outSignalResult, thing3.Named("SUBJECT")));
							return;
						}
						Find.SignalManager.SendSignal(new Signal(this.outSignalResult, false));
					}
				}
			}
		}

		// Token: 0x06006D48 RID: 27976 RVA: 0x002649BE File Offset: 0x00262BBE
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06006D49 RID: 27977 RVA: 0x002649CC File Offset: 0x00262BCC
		public override void Cleanup()
		{
			base.Cleanup();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!this.items[i].Destroyed)
				{
					this.items[i].Destroy(DestroyMode.Vanish);
				}
			}
			this.items.Clear();
		}

		// Token: 0x06006D4A RID: 27978 RVA: 0x00264A28 File Offset: 0x00262C28
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_References.Look<Pawn>(ref this.nearPawn, "nearPawn", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<ThingDefCountClass>(ref this.thingDefs, "thingDefs", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<bool>(ref this.makePrisoners, "makePrisoners", false, false);
			Scribe_Values.Look<string>(ref this.customDropPodsLetterLabel, "customDropPodsLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customDropPodsLetterText, "customDropPodsLetterText", null, false);
			Scribe_Values.Look<string>(ref this.customCaravanInventoryLetterLabel, "customCaravanInventoryLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customCaravanInventoryLetterText, "customCaravanInventoryLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_References.Look<Thing>(ref this.importantLookTarget, "importantLookTarget", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006D4B RID: 27979 RVA: 0x00264BB0 File Offset: 0x00262DB0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x0400458D RID: 17805
		public string inSignal;

		// Token: 0x0400458E RID: 17806
		public string outSignalResult;

		// Token: 0x0400458F RID: 17807
		public Pawn nearPawn;

		// Token: 0x04004590 RID: 17808
		private List<Thing> items = new List<Thing>();

		// Token: 0x04004591 RID: 17809
		private List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04004592 RID: 17810
		public List<ThingDefCountClass> thingDefs = new List<ThingDefCountClass>();

		// Token: 0x04004593 RID: 17811
		public bool joinPlayer;

		// Token: 0x04004594 RID: 17812
		public bool makePrisoners;

		// Token: 0x04004595 RID: 17813
		public string customDropPodsLetterText;

		// Token: 0x04004596 RID: 17814
		public string customDropPodsLetterLabel;

		// Token: 0x04004597 RID: 17815
		public string customCaravanInventoryLetterText;

		// Token: 0x04004598 RID: 17816
		public string customCaravanInventoryLetterLabel;

		// Token: 0x04004599 RID: 17817
		public LetterDef customLetterDef;

		// Token: 0x0400459A RID: 17818
		public bool sendStandardLetter = true;

		// Token: 0x0400459B RID: 17819
		private Thing importantLookTarget;

		// Token: 0x0400459C RID: 17820
		private List<Thing> tmpThingsToGive = new List<Thing>();
	}
}
