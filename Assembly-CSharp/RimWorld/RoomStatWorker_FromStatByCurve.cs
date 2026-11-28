using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200138D RID: 5005
	public class RoomStatWorker_FromStatByCurve : RoomStatWorker
	{
		// Token: 0x06007BCD RID: 31693 RVA: 0x002C0F0C File Offset: 0x002BF10C
		public override float GetScore(Room room)
		{
			return this.def.curve.Evaluate(room.GetStat(this.def.inputStat));
		}
	}
}
