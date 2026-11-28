using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D12 RID: 3346
	public class LordToil_Party : LordToil_Gathering
	{
		// Token: 0x06005E0D RID: 24077 RVA: 0x00226DB9 File Offset: 0x00224FB9
		public LordToil_Party(IntVec3 spot, GatheringDef gatheringDef, float joyPerTick = 3.5E-05f) : base(spot, gatheringDef)
		{
			this.joyPerTick = joyPerTick;
		}

		// Token: 0x06005E0E RID: 24078 RVA: 0x00226DD8 File Offset: 0x00224FD8
		public override void LordToilTick()
		{
			base.LordToilTick();
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (GatheringsUtility.InGatheringArea(ownedPawns[i].Position, this.spot, base.Map))
				{
					Need_Joy joy = ownedPawns[i].needs.joy;
					if (joy != null)
					{
						joy.GainJoy(this.joyPerTick, JoyKindDefOf.Social);
					}
				}
			}
		}

		// Token: 0x04003562 RID: 13666
		private float joyPerTick = 3.5E-05f;

		// Token: 0x04003563 RID: 13667
		public const float DefaultJoyPerTick = 3.5E-05f;
	}
}
