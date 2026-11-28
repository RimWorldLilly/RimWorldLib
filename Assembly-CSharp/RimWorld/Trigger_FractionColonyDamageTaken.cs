using System;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D22 RID: 3362
	public class Trigger_FractionColonyDamageTaken : Trigger
	{
		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x06005E57 RID: 24151 RVA: 0x00227C93 File Offset: 0x00225E93
		private TriggerData_FractionColonyDamageTaken Data
		{
			get
			{
				return (TriggerData_FractionColonyDamageTaken)this.data;
			}
		}

		// Token: 0x06005E58 RID: 24152 RVA: 0x00227CA0 File Offset: 0x00225EA0
		public Trigger_FractionColonyDamageTaken(float desiredColonyDamageFraction, float minDamage = 3.4028235E+38f)
		{
			this.data = new TriggerData_FractionColonyDamageTaken();
			this.desiredColonyDamageFraction = desiredColonyDamageFraction;
			this.minDamage = minDamage;
		}

		// Token: 0x06005E59 RID: 24153 RVA: 0x00227CC1 File Offset: 0x00225EC1
		public override void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
			if (!transition.sources.Contains(previousToil))
			{
				this.Data.startColonyDamage = transition.Map.damageWatcher.DamageTakenEver;
			}
		}

		// Token: 0x06005E5A RID: 24154 RVA: 0x00227CEC File Offset: 0x00225EEC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				if (this.data == null || !(this.data is TriggerData_FractionColonyDamageTaken))
				{
					BackCompatibility.TriggerDataFractionColonyDamageTakenNull(this, lord.Map);
				}
				float num = Mathf.Max((float)lord.initialColonyHealthTotal * this.desiredColonyDamageFraction, this.minDamage);
				return lord.Map.damageWatcher.DamageTakenEver > this.Data.startColonyDamage + num;
			}
			return false;
		}

		// Token: 0x04003573 RID: 13683
		private float desiredColonyDamageFraction;

		// Token: 0x04003574 RID: 13684
		private float minDamage;
	}
}
