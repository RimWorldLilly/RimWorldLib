using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200166F RID: 5743
	public abstract class StageFailTrigger : IExposable
	{
		// Token: 0x06009026 RID: 36902 RVA: 0x0034F287 File Offset: 0x0034D487
		public string Reason(LordJob_Ritual ritual, TargetInfo spot)
		{
			return this.desc;
		}

		// Token: 0x06009027 RID: 36903
		public abstract bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus);

		// Token: 0x06009028 RID: 36904 RVA: 0x0034F28F File Offset: 0x0034D48F
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.desc, "desc", null, false);
			Scribe_Values.Look<int>(ref this.allowanceTicks, "allowanceTicks", 0, false);
		}

		// Token: 0x040057F9 RID: 22521
		[MustTranslate]
		public string desc;

		// Token: 0x040057FA RID: 22522
		public int allowanceTicks;
	}
}
