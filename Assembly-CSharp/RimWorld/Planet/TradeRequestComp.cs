using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200236A RID: 9066
	[StaticConstructorOnStartup]
	public class TradeRequestComp : WorldObjectComp
	{
		// Token: 0x1700278B RID: 10123
		// (get) Token: 0x0600E0D3 RID: 57555 RVA: 0x0053884E File Offset: 0x00536A4E
		public bool ActiveRequest
		{
			get
			{
				return this.expiration > Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600E0D4 RID: 57556 RVA: 0x00538864 File Offset: 0x00536A64
		public override string CompInspectStringExtra()
		{
			if (this.ActiveRequest)
			{
				return "CaravanRequestInfo".Translate(TradeRequestUtility.RequestedThingLabel(this.requestThingDef, this.requestCount).CapitalizeFirst(), (this.expiration - Find.TickManager.TicksGame).ToStringTicksToDays("F1"), (this.requestThingDef.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)this.requestCount).ToStringMoney(null));
			}
			return null;
		}

		// Token: 0x0600E0D5 RID: 57557 RVA: 0x005388E9 File Offset: 0x00536AE9
		public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			TradeRequestComp.<GetCaravanGizmos>d__8 <GetCaravanGizmos>d__ = new TradeRequestComp.<GetCaravanGizmos>d__8(-2);
			<GetCaravanGizmos>d__.<>4__this = this;
			<GetCaravanGizmos>d__.<>3__caravan = caravan;
			return <GetCaravanGizmos>d__;
		}

		// Token: 0x0600E0D6 RID: 57558 RVA: 0x00538900 File Offset: 0x00536B00
		public void Disable()
		{
			this.expiration = -1;
		}

		// Token: 0x0600E0D7 RID: 57559 RVA: 0x0053890C File Offset: 0x00536B0C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.requestThingDef, "requestThingDef");
			Scribe_Values.Look<int>(ref this.requestCount, "requestCount", 0, false);
			Scribe_Values.Look<int>(ref this.expiration, "expiration", 0, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600E0D8 RID: 57560 RVA: 0x0053895C File Offset: 0x00536B5C
		private Command FulfillRequestCommand(Caravan caravan)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandFulfillTradeOffer".Translate();
			command_Action.defaultDesc = "CommandFulfillTradeOfferDesc".Translate();
			command_Action.icon = TradeRequestComp.TradeCommandTex;
			Action <>9__1;
			command_Action.action = delegate()
			{
				if (!this.ActiveRequest)
				{
					Log.Error("Attempted to fulfill an unavailable request");
					return;
				}
				if (!CaravanInventoryUtility.HasThings(caravan, this.requestThingDef, this.requestCount, new Func<Thing, bool>(this.PlayerCanGive)))
				{
					Messages.Message("CommandFulfillTradeOfferFailInsufficient".Translate(TradeRequestUtility.RequestedThingLabel(this.requestThingDef, this.requestCount)), MessageTypeDefOf.RejectInput, false);
					return;
				}
				WindowStack windowStack = Find.WindowStack;
				TaggedString text = "CommandFulfillTradeOfferConfirm".Translate(GenLabel.ThingLabel(this.requestThingDef, null, this.requestCount));
				Action confirmedAct;
				if ((confirmedAct = <>9__1) == null)
				{
					confirmedAct = (<>9__1 = delegate()
					{
						this.Fulfill(caravan);
					});
				}
				windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, false, null, WindowLayer.Dialog));
			};
			if (!CaravanInventoryUtility.HasThings(caravan, this.requestThingDef, this.requestCount, new Func<Thing, bool>(this.PlayerCanGive)))
			{
				command_Action.Disable("CommandFulfillTradeOfferFailInsufficient".Translate(TradeRequestUtility.RequestedThingLabel(this.requestThingDef, this.requestCount)));
			}
			return command_Action;
		}

		// Token: 0x0600E0D9 RID: 57561 RVA: 0x00538A1C File Offset: 0x00536C1C
		private void Fulfill(Caravan caravan)
		{
			int remaining = this.requestCount;
			List<Thing> list = CaravanInventoryUtility.TakeThings(caravan, delegate(Thing thing)
			{
				if (this.requestThingDef != thing.def)
				{
					return 0;
				}
				if (!this.PlayerCanGive(thing))
				{
					return 0;
				}
				int num = Mathf.Min(remaining, thing.stackCount);
				remaining -= num;
				return num;
			});
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Destroy(DestroyMode.Vanish);
			}
			if (this.parent.Faction != null)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this.parent.Faction, 12, true, true, HistoryEventDefOf.QuestGoodwillReward, null);
			}
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "TradeRequestFulfilled", this.parent.Named("SUBJECT"), caravan.Named("CARAVAN"));
			this.Disable();
		}

		// Token: 0x0600E0DA RID: 57562 RVA: 0x00538AE0 File Offset: 0x00536CE0
		private bool PlayerCanGive(Thing thing)
		{
			if (thing.GetRotStage() != RotStage.Fresh)
			{
				return false;
			}
			Apparel apparel = thing as Apparel;
			if (apparel != null && apparel.WornByCorpse)
			{
				return false;
			}
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			return compQuality == null || compQuality.Quality >= QualityCategory.Normal;
		}

		// Token: 0x0400901B RID: 36891
		public ThingDef requestThingDef;

		// Token: 0x0400901C RID: 36892
		public int requestCount;

		// Token: 0x0400901D RID: 36893
		public int expiration = -1;

		// Token: 0x0400901E RID: 36894
		public string outSignalFulfilled;

		// Token: 0x0400901F RID: 36895
		private static readonly Texture2D TradeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/FulfillTradeRequest", true);
	}
}
