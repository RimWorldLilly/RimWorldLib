using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200109E RID: 4254
	public class QuestPart_Venerate : QuestPart_MakeLord
	{
		// Token: 0x06006A7B RID: 27259 RVA: 0x00257183 File Offset: 0x00255383
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_Venerate(this.target, this.venerateDurationTicks, this.outSignalVenerationCompleted, this.inSignalForceExit), base.Map, null);
		}

		// Token: 0x06006A7C RID: 27260 RVA: 0x002571B4 File Offset: 0x002553B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<int>(ref this.venerateDurationTicks, "venerateDurationTicks", 0, false);
			Scribe_Values.Look<string>(ref this.inSignalForceExit, "inSignalForceExit", null, false);
		}

		// Token: 0x040043AF RID: 17327
		public Thing target;

		// Token: 0x040043B0 RID: 17328
		public int venerateDurationTicks;

		// Token: 0x040043B1 RID: 17329
		public string outSignalVenerationCompleted;

		// Token: 0x040043B2 RID: 17330
		public string inSignalForceExit;
	}
}
