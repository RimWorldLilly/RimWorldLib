using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B90 RID: 7056
	public class Tradeable_RoyalFavor : Tradeable
	{
		// Token: 0x170020AF RID: 8367
		// (get) Token: 0x0600B51D RID: 46365 RVA: 0x003FCD93 File Offset: 0x003FAF93
		public override bool IsFavor
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170020B0 RID: 8368
		// (get) Token: 0x0600B51E RID: 46366 RVA: 0x003FCD96 File Offset: 0x003FAF96
		public override bool IsCurrency
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170020B1 RID: 8369
		// (get) Token: 0x0600B51F RID: 46367 RVA: 0x003FCD99 File Offset: 0x003FAF99
		public override bool TraderWillTrade
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170020B2 RID: 8370
		// (get) Token: 0x0600B520 RID: 46368 RVA: 0x003FCD9C File Offset: 0x003FAF9C
		public override bool IsThing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170020B3 RID: 8371
		// (get) Token: 0x0600B521 RID: 46369 RVA: 0x003FCD9F File Offset: 0x003FAF9F
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170020B4 RID: 8372
		// (get) Token: 0x0600B522 RID: 46370 RVA: 0x003FCDA2 File Offset: 0x003FAFA2
		public override Thing AnyThing
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170020B5 RID: 8373
		// (get) Token: 0x0600B523 RID: 46371 RVA: 0x003FCDA5 File Offset: 0x003FAFA5
		public override string Label
		{
			get
			{
				return TradeSession.trader.Faction.def.royalFavorLabel;
			}
		}

		// Token: 0x170020B6 RID: 8374
		// (get) Token: 0x0600B524 RID: 46372 RVA: 0x003FCDBB File Offset: 0x003FAFBB
		public override string TipDescription
		{
			get
			{
				return "RoyalFavorDescription".Translate(TradeSession.trader.Faction.Named("FACTION"));
			}
		}

		// Token: 0x0600B525 RID: 46373 RVA: 0x003FCDE0 File Offset: 0x003FAFE0
		public override int CostToInt(float cost)
		{
			return Mathf.CeilToInt(cost);
		}

		// Token: 0x0600B526 RID: 46374 RVA: 0x003FCDE8 File Offset: 0x003FAFE8
		public override void ResolveTrade()
		{
			if (base.ActionToDo == TradeAction.PlayerBuys)
			{
				TradeSession.playerNegotiator.royalty.GainFavor(TradeSession.trader.Faction, base.CountToTransferToSource);
			}
		}

		// Token: 0x0600B527 RID: 46375 RVA: 0x003FCE14 File Offset: 0x003FB014
		public override void DrawIcon(Rect iconRect)
		{
			Faction faction = TradeSession.trader.Faction;
			GUI.color = faction.Color;
			Widgets.DrawTextureRotated(iconRect, faction.def.FactionIcon, 0f, null);
			GUI.color = Color.white;
		}

		// Token: 0x0600B528 RID: 46376 RVA: 0x003FCE58 File Offset: 0x003FB058
		public override int CountHeldBy(Transactor trans)
		{
			if (trans == Transactor.Trader)
			{
				return 99999;
			}
			return 0;
		}

		// Token: 0x0600B529 RID: 46377 RVA: 0x003FCE65 File Offset: 0x003FB065
		public override int GetHashCode()
		{
			return 0;
		}
	}
}
