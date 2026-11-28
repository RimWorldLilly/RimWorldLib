using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002368 RID: 9064
	public class TimedMakeFactionHostile : WorldObjectComp
	{
		// Token: 0x17002786 RID: 10118
		// (get) Token: 0x0600E0C2 RID: 57538 RVA: 0x00538594 File Offset: 0x00536794
		public int? TicksLeft
		{
			get
			{
				if (this.ticksLeftMakeFactionHostile != -1)
				{
					return new int?(this.ticksLeftMakeFactionHostile);
				}
				return null;
			}
		}

		// Token: 0x0600E0C3 RID: 57539 RVA: 0x005385BF File Offset: 0x005367BF
		public void SetupTimer(int ticks, string message = null, HistoryEventDef reason = null)
		{
			this.timerMakeFactionHostile = ticks;
			this.ticksLeftMakeFactionHostile = -1;
			this.messageBecameHostile = message;
			this.reasonBecameHostile = reason;
		}

		// Token: 0x0600E0C4 RID: 57540 RVA: 0x005385DD File Offset: 0x005367DD
		public override void PostMyMapRemoved()
		{
			this.ticksLeftMakeFactionHostile = -1;
		}

		// Token: 0x0600E0C5 RID: 57541 RVA: 0x005385E6 File Offset: 0x005367E6
		public override void PostMapGenerate()
		{
			this.ticksLeftMakeFactionHostile = this.timerMakeFactionHostile;
		}

		// Token: 0x0600E0C6 RID: 57542 RVA: 0x005385F4 File Offset: 0x005367F4
		public override void CompTickInterval(int delta)
		{
			if (this.ticksLeftMakeFactionHostile != -1 && this.parent.Faction.HostileTo(Faction.OfPlayer))
			{
				this.ticksLeftMakeFactionHostile = -1;
			}
			if (this.ticksLeftMakeFactionHostile != -1)
			{
				this.ticksLeftMakeFactionHostile = Mathf.Max(this.ticksLeftMakeFactionHostile - delta, 0);
				if (this.ticksLeftMakeFactionHostile == 0)
				{
					if (this.parent.Faction.temporary)
					{
						this.parent.Faction.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
					}
					else
					{
						this.parent.Faction.TryAffectGoodwillWith(Faction.OfPlayer, this.parent.Faction.GoodwillToMakeHostile(Faction.OfPlayer), true, true, this.reasonBecameHostile, null);
					}
					if (this.messageBecameHostile != null)
					{
						Messages.Message(this.messageBecameHostile, MessageTypeDefOf.NegativeEvent, true);
					}
					this.ticksLeftMakeFactionHostile = -1;
				}
			}
		}

		// Token: 0x0600E0C7 RID: 57543 RVA: 0x005386E4 File Offset: 0x005368E4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timerMakeFactionHostile, "timerMakeFactionHostile", -1, false);
			Scribe_Values.Look<int>(ref this.ticksLeftMakeFactionHostile, "ticksLeftMakeFactionHostile", -1, false);
			Scribe_Values.Look<string>(ref this.messageBecameHostile, "messageBecameHostile", null, false);
			Scribe_Defs.Look<HistoryEventDef>(ref this.reasonBecameHostile, "reasonBecameHostile");
		}

		// Token: 0x04009016 RID: 36886
		private int timerMakeFactionHostile = -1;

		// Token: 0x04009017 RID: 36887
		private int ticksLeftMakeFactionHostile = -1;

		// Token: 0x04009018 RID: 36888
		private string messageBecameHostile;

		// Token: 0x04009019 RID: 36889
		public HistoryEventDef reasonBecameHostile;
	}
}
