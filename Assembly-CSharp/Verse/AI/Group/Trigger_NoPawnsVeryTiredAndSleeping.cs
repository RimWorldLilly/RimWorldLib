using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000983 RID: 2435
	public class Trigger_NoPawnsVeryTiredAndSleeping : Trigger
	{
		// Token: 0x060049B2 RID: 18866 RVA: 0x001CB1C2 File Offset: 0x001C93C2
		public Trigger_NoPawnsVeryTiredAndSleeping(float extraRestThreshOffset = 0f)
		{
			this.extraRestThreshOffset = extraRestThreshOffset;
		}

		// Token: 0x060049B3 RID: 18867 RVA: 0x001CB1D4 File Offset: 0x001C93D4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Need_Rest rest = lord.ownedPawns[i].needs.rest;
					if (rest != null && rest.CurLevelPercentage < 0.14f + this.extraRestThreshOffset && !lord.ownedPawns[i].Awake())
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x04002F45 RID: 12101
		private float extraRestThreshOffset;
	}
}
