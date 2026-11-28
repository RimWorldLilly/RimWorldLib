using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001976 RID: 6518
	[StaticConstructorOnStartup]
	public class CompShuttle : ThingComp
	{
		// Token: 0x17001C62 RID: 7266
		// (get) Token: 0x0600A39E RID: 41886 RVA: 0x003A6AC8 File Offset: 0x003A4CC8
		public bool IsPlayerShuttle
		{
			get
			{
				if (ModLister.OdysseyInstalled)
				{
					TransportShipDef shipDef = this.Props.shipDef;
					return shipDef != null && shipDef.playerShuttle;
				}
				return false;
			}
		}

		// Token: 0x17001C63 RID: 7267
		// (get) Token: 0x0600A39F RID: 41887 RVA: 0x003A6AE9 File Offset: 0x003A4CE9
		public bool Autoload
		{
			get
			{
				return this.autoload;
			}
		}

		// Token: 0x17001C64 RID: 7268
		// (get) Token: 0x0600A3A0 RID: 41888 RVA: 0x003A6AF1 File Offset: 0x003A4CF1
		public List<CompTransporter> TransportersInGroup
		{
			get
			{
				return this.Transporter.TransportersInGroup(this.parent.Map);
			}
		}

		// Token: 0x17001C65 RID: 7269
		// (get) Token: 0x0600A3A1 RID: 41889 RVA: 0x003A6B09 File Offset: 0x003A4D09
		public CompProperties_Shuttle Props
		{
			get
			{
				return (CompProperties_Shuttle)this.props;
			}
		}

		// Token: 0x17001C66 RID: 7270
		// (get) Token: 0x0600A3A2 RID: 41890 RVA: 0x003A6B18 File Offset: 0x003A4D18
		public bool ShowLoadingGizmos
		{
			get
			{
				return this.IsPlayerShuttle || ((this.shipParent == null || this.shipParent.ShowGizmos) && (this.parent.Faction == null || this.parent.Faction == Faction.OfPlayer));
			}
		}

		// Token: 0x17001C67 RID: 7271
		// (get) Token: 0x0600A3A3 RID: 41891 RVA: 0x003A6B68 File Offset: 0x003A4D68
		public CompTransporter Transporter
		{
			get
			{
				CompTransporter result;
				if ((result = this.cachedCompTransporter) == null)
				{
					result = (this.cachedCompTransporter = this.parent.GetComp<CompTransporter>());
				}
				return result;
			}
		}

		// Token: 0x17001C68 RID: 7272
		// (get) Token: 0x0600A3A4 RID: 41892 RVA: 0x003A6B94 File Offset: 0x003A4D94
		private bool Autoloadable
		{
			get
			{
				if (this.IsPlayerShuttle)
				{
					return false;
				}
				if (this.cachedTransporterList == null)
				{
					this.cachedTransporterList = new List<CompTransporter>
					{
						this.Transporter
					};
				}
				foreach (Pawn thing in TransporterUtility.AllSendablePawns(this.cachedTransporterList, this.parent.Map))
				{
					if (!this.IsRequired(thing))
					{
						return false;
					}
				}
				foreach (Thing thing2 in TransporterUtility.AllSendableItems(this.cachedTransporterList, this.parent.Map))
				{
					if (!this.IsRequired(thing2))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001C69 RID: 7273
		// (get) Token: 0x0600A3A5 RID: 41893 RVA: 0x003A6C78 File Offset: 0x003A4E78
		private int ContainedColonistCount
		{
			get
			{
				int num = 0;
				ThingOwner innerContainer = this.Transporter.innerContainer;
				for (int i = 0; i < innerContainer.Count; i++)
				{
					Pawn pawn = innerContainer[i] as Pawn;
					if (pawn != null && pawn.IsFreeColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17001C6A RID: 7274
		// (get) Token: 0x0600A3A6 RID: 41894 RVA: 0x003A6CC1 File Offset: 0x003A4EC1
		private IEnumerable<Pawn> ContainedPawns
		{
			get
			{
				CompShuttle.<get_ContainedPawns>d__40 <get_ContainedPawns>d__ = new CompShuttle.<get_ContainedPawns>d__40(-2);
				<get_ContainedPawns>d__.<>4__this = this;
				return <get_ContainedPawns>d__;
			}
		}

		// Token: 0x17001C6B RID: 7275
		// (get) Token: 0x0600A3A7 RID: 41895 RVA: 0x003A6CD4 File Offset: 0x003A4ED4
		public bool AllRequiredThingsLoaded
		{
			get
			{
				ThingOwner innerContainer = this.Transporter.innerContainer;
				foreach (Pawn item in this.RequiredPawns)
				{
					if (!innerContainer.Contains(item))
					{
						return false;
					}
				}
				if (this.requireAllColonistsOnMap || this.requiredColonistCount > 0)
				{
					int containedColonistCount = this.ContainedColonistCount;
					int num = this.parent.Map.mapPawns.FreeColonistsCount;
					foreach (Map map in this.parent.Map.ChildPocketMaps)
					{
						num += map.mapPawns.FreeColonistsCount;
					}
					if (this.requireAllColonistsOnMap && containedColonistCount < num)
					{
						return false;
					}
					if (this.requiredColonistCount > 0 && containedColonistCount < this.requiredColonistCount)
					{
						return false;
					}
				}
				CompShuttle.tmpRequiredItemsWithoutDuplicates.Clear();
				for (int i = 0; i < this.requiredItems.Count; i++)
				{
					bool flag = false;
					for (int j = 0; j < CompShuttle.tmpRequiredItemsWithoutDuplicates.Count; j++)
					{
						if (CompShuttle.tmpRequiredItemsWithoutDuplicates[j].ThingDef == this.requiredItems[i].ThingDef)
						{
							CompShuttle.tmpRequiredItemsWithoutDuplicates[j] = CompShuttle.tmpRequiredItemsWithoutDuplicates[j].WithCount(CompShuttle.tmpRequiredItemsWithoutDuplicates[j].Count + this.requiredItems[i].Count);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						CompShuttle.tmpRequiredItemsWithoutDuplicates.Add(this.requiredItems[i]);
					}
				}
				for (int k = 0; k < CompShuttle.tmpRequiredItemsWithoutDuplicates.Count; k++)
				{
					int num2 = 0;
					for (int l = 0; l < innerContainer.Count; l++)
					{
						if (innerContainer[l].def == CompShuttle.tmpRequiredItemsWithoutDuplicates[k].ThingDef)
						{
							num2 += innerContainer[l].stackCount;
						}
					}
					if (num2 < CompShuttle.tmpRequiredItemsWithoutDuplicates[k].Count)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001C6C RID: 7276
		// (get) Token: 0x0600A3A8 RID: 41896 RVA: 0x003A6F50 File Offset: 0x003A5150
		public IEnumerable<Pawn> RequiredPawns
		{
			get
			{
				CompShuttle.<get_RequiredPawns>d__45 <get_RequiredPawns>d__ = new CompShuttle.<get_RequiredPawns>d__45(-2);
				<get_RequiredPawns>d__.<>4__this = this;
				return <get_RequiredPawns>d__;
			}
		}

		// Token: 0x17001C6D RID: 7277
		// (get) Token: 0x0600A3A9 RID: 41897 RVA: 0x003A6F60 File Offset: 0x003A5160
		public TaggedString RequiredThingsLabel
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Pawn pawn in this.RequiredPawns)
				{
					stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
				}
				for (int i = 0; i < this.requiredItems.Count; i++)
				{
					stringBuilder.AppendLine("  - " + this.requiredItems[i].LabelCap);
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
		}

		// Token: 0x17001C6E RID: 7278
		// (get) Token: 0x0600A3AA RID: 41898 RVA: 0x003A701C File Offset: 0x003A521C
		public bool HasPilot
		{
			get
			{
				ThingOwner innerContainer = this.Transporter.innerContainer;
				for (int i = 0; i < innerContainer.Count; i++)
				{
					Pawn pawn = innerContainer[i] as Pawn;
					if (pawn != null && pawn.IsFreeColonist && !StatDefOf.PilotingAbility.Worker.IsDisabledFor(pawn) && pawn.GetStatValue(StatDefOf.PilotingAbility, true, -1) > 0.1f)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001C6F RID: 7279
		// (get) Token: 0x0600A3AB RID: 41899 RVA: 0x003A7088 File Offset: 0x003A5288
		public AcceptanceReport CanLaunch
		{
			get
			{
				TransportShipDef shipDef = this.Props.shipDef;
				if (shipDef != null && !shipDef.playerShuttle)
				{
					if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
					{
						return "CommandLaunchGroupFailNotLoaded".Translate();
					}
					return true;
				}
				else
				{
					if (this.parent.Spawned && !this.HasPilot)
					{
						return "CommandShuttleNoPilot".Translate();
					}
					if (this.parent.Spawned)
					{
						WeatherDef curWeatherPerceived = this.parent.Map.weatherManager.CurWeatherPerceived;
						if (curWeatherPerceived.preventsShuttleLaunch)
						{
							return curWeatherPerceived.LabelCap;
						}
						foreach (GameCondition gameCondition in this.parent.Map.GameConditionManager.ActiveConditions)
						{
							if (gameCondition.def.preventShuttleLaunch)
							{
								return gameCondition.LabelCap;
							}
						}
					}
					return true;
				}
			}
		}

		// Token: 0x0600A3AC RID: 41900 RVA: 0x003A71A8 File Offset: 0x003A53A8
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnyExpansion("Shuttle"))
			{
				return;
			}
			if (!respawningAfterLoad && this.Props.shipDef != null)
			{
				TransportShipDef shipDef = this.Props.shipDef;
				if (shipDef != null && shipDef.playerShuttle)
				{
					this.acceptColonists = true;
					this.acceptChildren = true;
					this.acceptColonyPrisoners = true;
					this.allowSlaves = true;
				}
				if (this.shipParent == null)
				{
					this.shipParent = TransportShipMaker.MakeTransportShip(this.Props.shipDef, null, this.parent);
				}
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x0600A3AD RID: 41901 RVA: 0x003A7234 File Offset: 0x003A5434
		public void SetPawnToLeaveBehind(Func<Pawn, bool> predicate)
		{
			foreach (Pawn pawn in this.requiredPawns)
			{
				if (predicate(pawn))
				{
					this.pawnsToIgnoreIfDownedOfNotOnTheMap.AddUnique(pawn);
				}
			}
		}

		// Token: 0x0600A3AE RID: 41902 RVA: 0x003A7298 File Offset: 0x003A5498
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompShuttle.<CompGetGizmosExtra>d__54 <CompGetGizmosExtra>d__ = new CompShuttle.<CompGetGizmosExtra>d__54(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A3AF RID: 41903 RVA: 0x003A72A8 File Offset: 0x003A54A8
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			CompShuttle.<CompFloatMenuOptions>d__55 <CompFloatMenuOptions>d__ = new CompShuttle.<CompFloatMenuOptions>d__55(-2);
			<CompFloatMenuOptions>d__.<>4__this = this;
			<CompFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <CompFloatMenuOptions>d__;
		}

		// Token: 0x0600A3B0 RID: 41904 RVA: 0x003A72BF File Offset: 0x003A54BF
		public override IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			CompShuttle.<CompMultiSelectFloatMenuOptions>d__57 <CompMultiSelectFloatMenuOptions>d__ = new CompShuttle.<CompMultiSelectFloatMenuOptions>d__57(-2);
			<CompMultiSelectFloatMenuOptions>d__.<>4__this = this;
			<CompMultiSelectFloatMenuOptions>d__.<>3__selPawns = selPawns;
			return <CompMultiSelectFloatMenuOptions>d__;
		}

		// Token: 0x0600A3B1 RID: 41905 RVA: 0x003A72D8 File Offset: 0x003A54D8
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.parent.IsHashIntervalTick(120))
			{
				this.CheckAutoload();
			}
			if (this.parent.IsHashIntervalTick(600))
			{
				foreach (Pawn pawn in this.ContainedPawns)
				{
					if (pawn.TickDeSpawned > 0 && Find.TickManager.TicksGame - pawn.TickDeSpawned > 60000)
					{
						ShipJob_Unload.UnloadThingFromShuttle(this.shipParent, pawn, null, false);
						Lord lord = pawn.GetLord();
						if (lord != null)
						{
							lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
						}
					}
				}
			}
			TransportShipDef shipDef = this.Props.shipDef;
			if (shipDef != null && shipDef.playerShuttle && !this.Transporter.LoadingInProgressOrReadyToLaunch && this.ContainedColonistCount > 0)
			{
				TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(this.Transporter));
			}
		}

		// Token: 0x0600A3B2 RID: 41906 RVA: 0x003A73E0 File Offset: 0x003A55E0
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.shipParent != null && this.shipParent.curJob != null)
			{
				string text = this.shipParent.curJob.ShipThingExtraInspectString();
				if (!text.NullOrEmpty())
				{
					stringBuilder.Append(text);
				}
			}
			CompShuttle.tmpRequiredLabels.Clear();
			if (this.requireAllColonistsOnMap)
			{
				int freeColonistsCount = this.parent.Map.mapPawns.FreeColonistsCount;
				CompShuttle.tmpRequiredLabels.Add(freeColonistsCount.ToString() + " " + ((freeColonistsCount > 1) ? Faction.OfPlayer.def.pawnsPlural : Faction.OfPlayer.def.pawnSingular));
			}
			else if (this.requiredColonistCount > 0)
			{
				CompShuttle.tmpRequiredLabels.Add(this.requiredColonistCount.ToString() + " " + ((this.requiredColonistCount > 1) ? Faction.OfPlayer.def.pawnsPlural : Faction.OfPlayer.def.pawnSingular));
			}
			foreach (Pawn pawn in this.RequiredPawns)
			{
				if (!this.Transporter.innerContainer.Contains(pawn))
				{
					CompShuttle.tmpRequiredLabels.Add(pawn.LabelShort);
				}
			}
			for (int i = 0; i < this.requiredItems.Count; i++)
			{
				if (this.Transporter.innerContainer.TotalStackCountOfDef(this.requiredItems[i].ThingDef) < this.requiredItems[i].Count)
				{
					CompShuttle.tmpRequiredLabels.Add(this.requiredItems[i].Label);
				}
			}
			if (CompShuttle.tmpRequiredLabels.Any<string>())
			{
				stringBuilder.AppendInNewLine("Required".Translate() + ": " + CompShuttle.tmpRequiredLabels.ToCommaList(false, false).CapitalizeFirst());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A3B3 RID: 41907 RVA: 0x003A7604 File Offset: 0x003A5804
		public void SendLaunchedSignals()
		{
			List<CompTransporter> transportersInGroup = this.TransportersInGroup;
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < transportersInGroup.Count; i++)
			{
				for (int j = 0; j < transportersInGroup[i].innerContainer.Count; j++)
				{
					Pawn pawn = transportersInGroup[i].innerContainer[j] as Pawn;
					if (pawn != null && pawn.IsColonist && !this.requiredPawns.Contains(pawn))
					{
						list.Add(pawn);
					}
				}
			}
			if (list.Count != 0)
			{
				for (int k = 0; k < transportersInGroup.Count; k++)
				{
					QuestUtility.SendQuestTargetSignals(transportersInGroup[k].parent.questTags, "SentWithExtraColonists", transportersInGroup[k].parent.Named("SUBJECT"), list.Named("SENTCOLONISTS"));
				}
			}
			string signalPart = this.AllRequiredThingsLoaded ? "SentSatisfied" : "SentUnsatisfied";
			for (int l = 0; l < transportersInGroup.Count; l++)
			{
				QuestUtility.SendQuestTargetSignals(transportersInGroup[l].parent.questTags, signalPart, transportersInGroup[l].parent.Named("SUBJECT"), transportersInGroup[l].innerContainer.ToList<Thing>().Named("SENT"));
			}
		}

		// Token: 0x0600A3B4 RID: 41908 RVA: 0x003A7760 File Offset: 0x003A5960
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<ThingDefCount>(ref this.requiredItems, "requiredItems", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.requiredPawns, "requiredPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawnsToIgnoreIfDownedOfNotOnTheMap, "pawnsToIgnoreIfDownedOfNotOnTheMap", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.requiredColonistCount, "requiredColonistCount", 0, false);
			Scribe_Values.Look<bool>(ref this.requireAllColonistsOnMap, "requireAllColonistsOnMap", false, false);
			Scribe_Values.Look<bool>(ref this.acceptColonists, "acceptColonists", false, false);
			Scribe_Values.Look<bool>(ref this.acceptChildren, "acceptChildren", true, false);
			Scribe_Values.Look<bool>(ref this.onlyAcceptColonists, "onlyAcceptColonists", false, false);
			Scribe_Values.Look<bool>(ref this.allowSlaves, "allowSlaves", false, false);
			Scribe_Values.Look<bool>(ref this.autoload, "autoload", false, false);
			Scribe_Values.Look<bool>(ref this.permitShuttle, "permitShuttle", false, false);
			Scribe_Values.Look<bool>(ref this.acceptColonyPrisoners, "acceptColonyPrisoners", false, false);
			Scribe_Values.Look<int>(ref this.maxColonistCount, "maxColonistCount", -1, false);
			Scribe_References.Look<TransportShip>(ref this.shipParent, "shipParent", false);
			Scribe_Values.Look<float>(ref this.minAge, "minAge", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.requiredPawns.RemoveAll((Pawn x) => x == null);
			}
			if (this.maxColonistCount == 0)
			{
				this.maxColonistCount = -1;
			}
		}

		// Token: 0x0600A3B5 RID: 41909 RVA: 0x003A78D4 File Offset: 0x003A5AD4
		private void CheckAutoload()
		{
			if (!this.autoload || !this.Transporter.LoadingInProgressOrReadyToLaunch || !this.parent.Spawned)
			{
				return;
			}
			CompShuttle.tmpRequiredItems.Clear();
			CompShuttle.tmpRequiredItems.AddRange(this.requiredItems);
			CompShuttle.tmpRequiredPawns.Clear();
			CompShuttle.tmpRequiredPawns.AddRange(this.requiredPawns);
			ThingOwner innerContainer = this.Transporter.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Pawn pawn = innerContainer[i] as Pawn;
				if (pawn != null)
				{
					CompShuttle.tmpRequiredPawns.Remove(pawn);
				}
				else
				{
					int num = innerContainer[i].stackCount;
					for (int j = 0; j < CompShuttle.tmpRequiredItems.Count; j++)
					{
						if (CompShuttle.tmpRequiredItems[j].ThingDef == innerContainer[i].def)
						{
							int num2 = Mathf.Min(CompShuttle.tmpRequiredItems[j].Count, num);
							if (num2 > 0)
							{
								CompShuttle.tmpRequiredItems[j] = CompShuttle.tmpRequiredItems[j].WithCount(CompShuttle.tmpRequiredItems[j].Count - num2);
								num -= num2;
							}
						}
					}
				}
			}
			for (int k = CompShuttle.tmpRequiredItems.Count - 1; k >= 0; k--)
			{
				if (CompShuttle.tmpRequiredItems[k].Count <= 0)
				{
					CompShuttle.tmpRequiredItems.RemoveAt(k);
				}
			}
			if (CompShuttle.tmpRequiredItems.Any<ThingDefCount>() || CompShuttle.tmpRequiredPawns.Any<Pawn>())
			{
				if (this.Transporter.leftToLoad != null)
				{
					this.Transporter.leftToLoad.Clear();
				}
				CompShuttle.tmpAllSendablePawns.Clear();
				CompShuttle.tmpAllSendablePawns.AddRange(TransporterUtility.AllSendablePawns(this.TransportersInGroup, this.parent.Map));
				CompShuttle.tmpAllSendableItems.Clear();
				CompShuttle.tmpAllSendableItems.AddRange(TransporterUtility.AllSendableItems(this.TransportersInGroup, this.parent.Map));
				CompShuttle.tmpAllSendableItems.AddRange(TransporterUtility.ThingsBeingHauledTo(this.TransportersInGroup, this.parent.Map));
				CompShuttle.tmpRequiredPawnsPossibleToSend.Clear();
				for (int l = 0; l < CompShuttle.tmpRequiredPawns.Count; l++)
				{
					if (CompShuttle.tmpAllSendablePawns.Contains(CompShuttle.tmpRequiredPawns[l]))
					{
						TransferableOneWay transferableOneWay = new TransferableOneWay();
						transferableOneWay.things.Add(CompShuttle.tmpRequiredPawns[l]);
						this.Transporter.AddToTheToLoadList(transferableOneWay, 1);
						CompShuttle.tmpRequiredPawnsPossibleToSend.Add(CompShuttle.tmpRequiredPawns[l]);
					}
				}
				for (int m = 0; m < CompShuttle.tmpRequiredItems.Count; m++)
				{
					if (CompShuttle.tmpRequiredItems[m].Count > 0)
					{
						int num3 = 0;
						for (int n = 0; n < CompShuttle.tmpAllSendableItems.Count; n++)
						{
							if (CompShuttle.tmpAllSendableItems[n].def == CompShuttle.tmpRequiredItems[m].ThingDef)
							{
								num3 += CompShuttle.tmpAllSendableItems[n].stackCount;
							}
						}
						if (num3 > 0)
						{
							TransferableOneWay transferableOneWay2 = new TransferableOneWay();
							for (int num4 = 0; num4 < CompShuttle.tmpAllSendableItems.Count; num4++)
							{
								if (CompShuttle.tmpAllSendableItems[num4].def == CompShuttle.tmpRequiredItems[m].ThingDef)
								{
									transferableOneWay2.things.Add(CompShuttle.tmpAllSendableItems[num4]);
								}
							}
							int count = Mathf.Min(CompShuttle.tmpRequiredItems[m].Count, num3);
							this.Transporter.AddToTheToLoadList(transferableOneWay2, count);
						}
					}
				}
				TransporterUtility.MakeLordsAsAppropriate(CompShuttle.tmpRequiredPawnsPossibleToSend, this.TransportersInGroup, this.parent.Map);
				CompShuttle.tmpAllSendablePawns.Clear();
				CompShuttle.tmpAllSendableItems.Clear();
				CompShuttle.tmpRequiredItems.Clear();
				CompShuttle.tmpRequiredPawns.Clear();
				CompShuttle.tmpRequiredPawnsPossibleToSend.Clear();
				return;
			}
			if (this.Transporter.leftToLoad != null)
			{
				this.Transporter.leftToLoad.Clear();
			}
			TransporterUtility.MakeLordsAsAppropriate(CompShuttle.tmpRequiredPawnsPossibleToSend, this.TransportersInGroup, this.parent.Map);
		}

		// Token: 0x0600A3B6 RID: 41910 RVA: 0x003A7D44 File Offset: 0x003A5F44
		public virtual bool IsRequired(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				return this.requiredPawns.Contains(pawn);
			}
			for (int i = 0; i < this.requiredItems.Count; i++)
			{
				if (this.requiredItems[i].ThingDef == thing.def && this.requiredItems[i].Count != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A3B7 RID: 41911 RVA: 0x003A7DB4 File Offset: 0x003A5FB4
		public virtual bool IsAllowed(Thing t)
		{
			TransportShipDef shipDef = this.Props.shipDef;
			if (shipDef != null && shipDef.playerShuttle)
			{
				return true;
			}
			if (this.IsRequired(t))
			{
				return true;
			}
			if (this.acceptColonists)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null && (pawn.IsColonist || (pawn.IsPrisonerOfColony && this.acceptColonyPrisoners) || (!this.onlyAcceptColonists && pawn.IsAnimal && pawn.Faction == Faction.OfPlayer)) && (!pawn.IsSlave || this.allowSlaves) && (!this.onlyAcceptColonists || !pawn.IsQuestLodger()) && (!this.onlyAcceptHealthy || this.PawnIsHealthyEnoughForShuttle(pawn)) && pawn.ageTracker.AgeBiologicalYearsFloat >= this.minAge)
				{
					return true;
				}
			}
			if (!this.acceptChildren)
			{
				Pawn pawn2 = t as Pawn;
				if (pawn2 != null && pawn2.RaceProps.Humanlike && !pawn2.DevelopmentalStage.Adult())
				{
					return false;
				}
			}
			if (this.permitShuttle)
			{
				Pawn pawn3 = t as Pawn;
				if (pawn3 == null)
				{
					return true;
				}
				if ((pawn3.Faction == Faction.OfPlayer && !pawn3.IsQuestLodger()) || pawn3.IsPrisonerOfColony)
				{
					return true;
				}
			}
			return !(t is Pawn) && !this.parent.Map.IsPlayerHome && !GenHostility.AnyHostileActiveThreatToPlayer(this.parent.Map, true, false);
		}

		// Token: 0x0600A3B8 RID: 41912 RVA: 0x003A7F0C File Offset: 0x003A610C
		public virtual bool IsAllowedNow(Thing t)
		{
			if (!this.IsAllowed(t))
			{
				return false;
			}
			if (this.maxColonistCount == -1)
			{
				return true;
			}
			int num = 0;
			foreach (Thing thing in ((IEnumerable<Thing>)this.Transporter.innerContainer))
			{
				if (thing != t)
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null && pawn.IsColonist)
					{
						num++;
					}
				}
			}
			foreach (Pawn pawn2 in this.parent.Map.mapPawns.AllPawns)
			{
				if (pawn2.jobs != null && pawn2.jobs.curDriver != null && pawn2 != t)
				{
					foreach (QueuedJob queuedJob in pawn2.jobs.jobQueue)
					{
						if (this.<IsAllowedNow>g__CheckJob|71_0(queuedJob.job))
						{
							num++;
						}
					}
					if (this.<IsAllowedNow>g__CheckJob|71_0(pawn2.jobs.curJob))
					{
						num++;
					}
				}
			}
			return num < this.maxColonistCount;
		}

		// Token: 0x0600A3B9 RID: 41913 RVA: 0x003A806C File Offset: 0x003A626C
		private bool PawnIsHealthyEnoughForShuttle(Pawn p)
		{
			return !p.Downed && !p.InMentalState && p.health.capacities.CanBeAwake && p.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
		}

		// Token: 0x0600A3BA RID: 41914 RVA: 0x003A80CC File Offset: 0x003A62CC
		public virtual void CleanUpLoadingVars()
		{
			this.autoload = false;
		}

		// Token: 0x0600A3C1 RID: 41921 RVA: 0x003A8278 File Offset: 0x003A6478
		[CompilerGenerated]
		private bool <IsAllowedNow>g__CheckJob|71_0(Job job)
		{
			return typeof(JobDriver_EnterTransporter).IsAssignableFrom(job.def.driverClass) && job.GetTarget(TargetIndex.A).Thing == this.parent;
		}

		// Token: 0x04006098 RID: 24728
		public List<ThingDefCount> requiredItems = new List<ThingDefCount>();

		// Token: 0x04006099 RID: 24729
		public List<Pawn> requiredPawns = new List<Pawn>();

		// Token: 0x0400609A RID: 24730
		public List<Pawn> pawnsToIgnoreIfDownedOfNotOnTheMap = new List<Pawn>();

		// Token: 0x0400609B RID: 24731
		public int requiredColonistCount;

		// Token: 0x0400609C RID: 24732
		public bool requireAllColonistsOnMap;

		// Token: 0x0400609D RID: 24733
		public int maxColonistCount = -1;

		// Token: 0x0400609E RID: 24734
		public bool acceptColonists;

		// Token: 0x0400609F RID: 24735
		public bool acceptChildren;

		// Token: 0x040060A0 RID: 24736
		public bool acceptColonyPrisoners;

		// Token: 0x040060A1 RID: 24737
		public bool onlyAcceptColonists;

		// Token: 0x040060A2 RID: 24738
		public bool onlyAcceptHealthy;

		// Token: 0x040060A3 RID: 24739
		public bool permitShuttle;

		// Token: 0x040060A4 RID: 24740
		public bool allowSlaves;

		// Token: 0x040060A5 RID: 24741
		public float minAge;

		// Token: 0x040060A6 RID: 24742
		private bool autoload;

		// Token: 0x040060A7 RID: 24743
		public TransportShip shipParent;

		// Token: 0x040060A8 RID: 24744
		private CompRefuelable cachedCompRefuelable;

		// Token: 0x040060A9 RID: 24745
		private CompTransporter cachedCompTransporter;

		// Token: 0x040060AA RID: 24746
		private List<CompTransporter> cachedTransporterList;

		// Token: 0x040060AB RID: 24747
		private const int CheckAutoloadIntervalTicks = 120;

		// Token: 0x040060AC RID: 24748
		private const int CheckUnloadPawnIntervalTicks = 600;

		// Token: 0x040060AD RID: 24749
		private static readonly Texture2D AutoloadToggleTex = ContentFinder<Texture2D>.Get("UI/Commands/Autoload", true);

		// Token: 0x040060AE RID: 24750
		private const float MinPilotingSkill = 0.1f;

		// Token: 0x040060AF RID: 24751
		private static readonly List<ThingDefCount> tmpRequiredItemsWithoutDuplicates = new List<ThingDefCount>();

		// Token: 0x040060B0 RID: 24752
		private static List<Pawn> tmpAllowedPawns = new List<Pawn>();

		// Token: 0x040060B1 RID: 24753
		private static List<string> tmpRequiredLabels = new List<string>();

		// Token: 0x040060B2 RID: 24754
		private static List<ThingDefCount> tmpRequiredItems = new List<ThingDefCount>();

		// Token: 0x040060B3 RID: 24755
		private static List<Pawn> tmpRequiredPawns = new List<Pawn>();

		// Token: 0x040060B4 RID: 24756
		private static List<Pawn> tmpAllSendablePawns = new List<Pawn>();

		// Token: 0x040060B5 RID: 24757
		private static List<Thing> tmpAllSendableItems = new List<Thing>();

		// Token: 0x040060B6 RID: 24758
		private static List<Pawn> tmpRequiredPawnsPossibleToSend = new List<Pawn>();
	}
}
