using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001353 RID: 4947
	internal struct StrikeRecord : IExposable
	{
		// Token: 0x170014DE RID: 5342
		// (get) Token: 0x06007A6A RID: 31338 RVA: 0x002B9F31 File Offset: 0x002B8131
		public bool Expired
		{
			get
			{
				return Find.TickManager.TicksGame > this.ticksGame + 900000;
			}
		}

		// Token: 0x06007A6B RID: 31339 RVA: 0x002B9F4C File Offset: 0x002B814C
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.ticksGame, "ticksGame", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
		}

		// Token: 0x06007A6C RID: 31340 RVA: 0x002B9F98 File Offset: 0x002B8198
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "(";
			int num = 1;
			IntVec3 intVec = this.cell;
			array[num] = intVec.ToString();
			array[2] = ", ";
			int num2 = 3;
			ThingDef thingDef = this.def;
			array[num2] = ((thingDef != null) ? thingDef.ToString() : null);
			array[4] = ", ";
			array[5] = this.ticksGame.ToString();
			array[6] = ")";
			return string.Concat(array);
		}

		// Token: 0x04004D5E RID: 19806
		public IntVec3 cell;

		// Token: 0x04004D5F RID: 19807
		public int ticksGame;

		// Token: 0x04004D60 RID: 19808
		public ThingDef def;

		// Token: 0x04004D61 RID: 19809
		private const int StrikeRecordExpiryDays = 15;
	}
}
