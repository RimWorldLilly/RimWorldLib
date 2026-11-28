using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D23 RID: 3363
	public class TriggerData_FractionColonyDamageTaken : TriggerData
	{
		// Token: 0x06005E5B RID: 24155 RVA: 0x00227D5E File Offset: 0x00225F5E
		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.startColonyDamage, "startColonyDamage", 0f, false);
		}

		// Token: 0x04003575 RID: 13685
		public float startColonyDamage;
	}
}
