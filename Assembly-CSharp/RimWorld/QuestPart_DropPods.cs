using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200110F RID: 4367
	public class QuestPart_DropPods : QuestPart
	{
		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x06006D22 RID: 27938 RVA: 0x00263038 File Offset: 0x00261238
		// (set) Token: 0x06006D23 RID: 27939 RVA: 0x00263050 File Offset: 0x00261250
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
						if (thing.Destroyed)
						{
							Log.Error("Tried to add a destroyed thing to QuestPart_DropPods: " + thing.ToStringSafe<Thing>());
						}
						else
						{
							Pawn pawn = thing as Pawn;
							if (pawn != null)
							{
								this.pawns.Add(pawn);
							}
							else
							{
								this.items.Add(thing);
								ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
								if (thingOwner != null)
								{
									for (int i = 0; i < thingOwner.Count; i++)
									{
										Pawn pawn2 = thingOwner[i] as Pawn;
										if (pawn2 != null)
										{
											this.pawnsInContainers.Add(pawn2);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x170012B4 RID: 4788
		// (get) Token: 0x06006D24 RID: 27940 RVA: 0x00263138 File Offset: 0x00261338
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_DropPods.<get_QuestLookTargets>d__26 <get_QuestLookTargets>d__ = new QuestPart_DropPods.<get_QuestLookTargets>d__26(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x170012B5 RID: 4789
		// (get) Token: 0x06006D25 RID: 27941 RVA: 0x00263148 File Offset: 0x00261348
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		// Token: 0x06006D26 RID: 27942 RVA: 0x00263164 File Offset: 0x00261364
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag != this.inSignal)
			{
				return;
			}
			if (this.mapParent == null || !this.mapParent.HasMap || !this.quest.IsParentSuitableForQuest(this.mapParent))
			{
				if (this.canRetargetAnyMap)
				{
					this.mapParent = this.quest.TryFindNewSuitablePlayerMapParentForRetarget(false);
				}
				else
				{
					this.mapParent = this.quest.TryFindNewSuitablePlayerMapParentForRetarget(true);
				}
			}
			if (this.mapParent != null && this.mapParent.HasMap)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				this.pawnsInContainers.RemoveAll((Pawn x) => x.Destroyed);
				this.items.RemoveAll((Thing x) => x.Destroyed);
				this.tmpThingsToDrop.Clear();
				this.tmpThingsToDrop.AddRange(this.Things);
				for (int i = 0; i < this.thingDefs.Count; i++)
				{
					Thing thing = ThingMaker.MakeThing(this.thingDefs[i].thingDef, GenStuff.RandomStuffByCommonalityFor(this.thingDefs[i].thingDef, TechLevel.Undefined));
					thing.stackCount = this.thingDefs[i].count;
					this.tmpThingsToDrop.Add(thing);
				}
				this.tmpThingsToDrop.RemoveAll((Thing x) => x.Spawned);
				Thing thing2 = (from x in this.tmpThingsToDrop
				where x is Pawn
				select x).MaxByWithFallback((Thing x) => x.MarketValue, null);
				Thing thing3 = this.tmpThingsToDrop.MaxByWithFallback((Thing x) => x.MarketValue * (float)x.stackCount, null);
				if (this.tmpThingsToDrop.Any<Thing>())
				{
					Map map = this.mapParent.Map;
					IntVec3 intVec = this.dropSpot.IsValid ? this.dropSpot : this.GetRandomDropSpot();
					TaggedString taggedString = null;
					TaggedString taggedString2 = null;
					if (this.sendStandardLetter)
					{
						if (this.joinPlayer && this.pawns.Count == 1 && this.pawns[0].RaceProps.Humanlike)
						{
							taggedString = "LetterRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
							taggedString2 = "LetterLabelRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
							PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref taggedString2, this.pawns[0]);
						}
						else
						{
							taggedString = "LetterQuestDropPodsArrived".Translate(GenLabel.ThingsLabel(this.tmpThingsToDrop, "  - "));
							taggedString2 = "LetterLabelQuestDropPodsArrived".Translate();
							PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(this.pawns, ref taggedString2, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
						}
						taggedString2 = (this.customLetterLabel.NullOrEmpty() ? taggedString2 : this.customLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
						taggedString = (this.customLetterText.NullOrEmpty() ? taggedString : this.customLetterText.Formatted(taggedString.Named("BASETEXT")));
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
					if (this.dropAllInSamePod)
					{
						DropPodUtility.DropThingGroupsNear(intVec, map, new List<List<Thing>>
						{
							this.tmpThingsToDrop
						}, 110, false, false, !this.useTradeDropSpot, false, this.allowFogged, false, this.faction);
					}
					else
					{
						DropPodUtility.DropThingsNear(intVec, map, this.tmpThingsToDrop, 110, false, false, !this.useTradeDropSpot, false, this.allowFogged, this.faction);
					}
					for (int l = 0; l < this.pawns.Count; l++)
					{
						this.pawns[l].needs.SetInitialLevels();
						Pawn_MindState mindState = this.pawns[l].mindState;
						if (mindState != null)
						{
							mindState.SetupLastHumanMeatTick();
						}
					}
					if (this.sendStandardLetter)
					{
						IntVec3 cell = intVec;
						for (int m = 0; m < this.tmpThingsToDrop.Count; m++)
						{
							if (this.tmpThingsToDrop[m].SpawnedOrAnyParentSpawned)
							{
								cell = this.tmpThingsToDrop[m].PositionHeld;
								break;
							}
						}
						Find.LetterStack.ReceiveLetter(taggedString2, taggedString, this.customLetterDef ?? LetterDefOf.PositiveEvent, new TargetInfo(cell, map, false), null, this.quest, null, null, 0, true);
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

		// Token: 0x06006D27 RID: 27943 RVA: 0x00263805 File Offset: 0x00261A05
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p) || this.pawnsInContainers.Contains(p);
		}

		// Token: 0x06006D28 RID: 27944 RVA: 0x00263823 File Offset: 0x00261A23
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x06006D29 RID: 27945 RVA: 0x00263834 File Offset: 0x00261A34
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].def == ThingDefOf.PsychicAmplifier)
				{
					Find.History.Notify_PsylinkAvailable();
					return;
				}
			}
		}

		// Token: 0x06006D2A RID: 27946 RVA: 0x00263880 File Offset: 0x00261A80
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.destroyItemsOnCleanup)
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					if (!this.items[i].Destroyed)
					{
						this.items[i].Destroy(DestroyMode.Vanish);
					}
				}
				this.items.Clear();
			}
		}

		// Token: 0x06006D2B RID: 27947 RVA: 0x002638E4 File Offset: 0x00261AE4
		private IntVec3 GetRandomDropSpot()
		{
			Map map = this.mapParent.Map;
			if (this.useTradeDropSpot)
			{
				return DropCellFinder.TradeDropSpot(map);
			}
			IntVec3 result;
			if (CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => x.Standable(map) && !x.Roofed(map) && (this.allowFogged || !x.Fogged(map)) && map.reachability.CanReachColony(x), map, 1000, out result))
			{
				return result;
			}
			return DropCellFinder.RandomDropSpot(map, true);
		}

		// Token: 0x06006D2C RID: 27948 RVA: 0x00263951 File Offset: 0x00261B51
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06006D2D RID: 27949 RVA: 0x00263964 File Offset: 0x00261B64
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_Values.Look<IntVec3>(ref this.dropSpot, "dropSpot", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.useTradeDropSpot, "useTradeDropSpot", false, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawnsInContainers, "pawnsInContainers", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<ThingDefCountClass>(ref this.thingDefs, "thingDefs", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<bool>(ref this.makePrisoners, "makePrisoners", false, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_References.Look<Thing>(ref this.importantLookTarget, "importantLookTarget", false);
			Scribe_Collections.Look<ThingDef>(ref this.thingsToExcludeFromHyperlinks, "thingsToExcludeFromHyperlinks", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.destroyItemsOnCleanup, "destroyItemsOnCleanup", false, false);
			Scribe_Values.Look<bool>(ref this.allowFogged, "allowFogged", false, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.thingsToExcludeFromHyperlinks == null)
				{
					this.thingsToExcludeFromHyperlinks = new List<ThingDef>();
				}
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
				this.pawnsInContainers.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006D2E RID: 27950 RVA: 0x00263B98 File Offset: 0x00261D98
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				List<Thing> list = ThingSetMakerDefOf.DebugQuestDropPodsContents.root.Generate();
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i] as Pawn;
					if (pawn != null)
					{
						pawn.relations.everSeenByPlayer = true;
						if (!pawn.IsWorldPawn())
						{
							Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						}
					}
				}
				this.Things = list;
			}
		}

		// Token: 0x04004564 RID: 17764
		public string inSignal;

		// Token: 0x04004565 RID: 17765
		public string outSignalResult;

		// Token: 0x04004566 RID: 17766
		public IntVec3 dropSpot = IntVec3.Invalid;

		// Token: 0x04004567 RID: 17767
		public bool useTradeDropSpot;

		// Token: 0x04004568 RID: 17768
		public MapParent mapParent;

		// Token: 0x04004569 RID: 17769
		private List<Thing> items = new List<Thing>();

		// Token: 0x0400456A RID: 17770
		private List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400456B RID: 17771
		private List<Pawn> pawnsInContainers = new List<Pawn>();

		// Token: 0x0400456C RID: 17772
		public List<ThingDefCountClass> thingDefs = new List<ThingDefCountClass>();

		// Token: 0x0400456D RID: 17773
		public List<ThingDef> thingsToExcludeFromHyperlinks = new List<ThingDef>();

		// Token: 0x0400456E RID: 17774
		public bool joinPlayer;

		// Token: 0x0400456F RID: 17775
		public bool makePrisoners;

		// Token: 0x04004570 RID: 17776
		public bool destroyItemsOnCleanup = true;

		// Token: 0x04004571 RID: 17777
		public bool dropAllInSamePod;

		// Token: 0x04004572 RID: 17778
		public bool allowFogged;

		// Token: 0x04004573 RID: 17779
		public Faction faction;

		// Token: 0x04004574 RID: 17780
		public bool canRetargetAnyMap;

		// Token: 0x04004575 RID: 17781
		public string customLetterText;

		// Token: 0x04004576 RID: 17782
		public string customLetterLabel;

		// Token: 0x04004577 RID: 17783
		public LetterDef customLetterDef;

		// Token: 0x04004578 RID: 17784
		public bool sendStandardLetter = true;

		// Token: 0x04004579 RID: 17785
		private Thing importantLookTarget;

		// Token: 0x0400457A RID: 17786
		private List<Thing> tmpThingsToDrop = new List<Thing>();
	}
}
