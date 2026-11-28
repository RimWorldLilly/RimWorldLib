using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001B04 RID: 6916
	public class SignalAction_DormancyWakeUp : SignalAction_Delay
	{
		// Token: 0x17001FE2 RID: 8162
		// (get) Token: 0x0600B0D9 RID: 45273 RVA: 0x003E4F58 File Offset: 0x003E3158
		public override Alert_ActionDelay Alert
		{
			get
			{
				if (this.cachedAlert == null && this.lord.faction != null && this.lord.faction.HostileTo(Faction.OfPlayer) && this.lord.ownedPawns.Count > 0)
				{
					this.cachedAlert = new Alert_DormanyWakeUpDelay(this);
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x17001FE3 RID: 8163
		// (get) Token: 0x0600B0DA RID: 45274 RVA: 0x003E4FB8 File Offset: 0x003E31B8
		public override bool ShouldRemoveNow
		{
			get
			{
				if (this.lord == null || !base.Map.lordManager.lords.Contains(this.lord))
				{
					return true;
				}
				List<Pawn> ownedPawns = this.lord.ownedPawns;
				if (ownedPawns.NullOrEmpty<Pawn>())
				{
					return true;
				}
				for (int i = 0; i < ownedPawns.Count; i++)
				{
					if (!ownedPawns[i].Awake())
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x0600B0DB RID: 45275 RVA: 0x003E5024 File Offset: 0x003E3224
		protected override void Complete()
		{
			base.Complete();
			if (this.lord != null)
			{
				this.lord.Notify_DormancyWakeup();
			}
		}

		// Token: 0x0600B0DC RID: 45276 RVA: 0x003E503F File Offset: 0x003E323F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
		}

		// Token: 0x04006611 RID: 26129
		public Lord lord;

		// Token: 0x04006612 RID: 26130
		private Alert_ActionDelay cachedAlert;
	}
}
