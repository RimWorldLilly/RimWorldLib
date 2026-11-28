using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001208 RID: 4616
	public class DamageWatcher : IExposable
	{
		// Token: 0x17001385 RID: 4997
		// (get) Token: 0x060072B2 RID: 29362 RVA: 0x00281B1E File Offset: 0x0027FD1E
		public float DamageTakenEver
		{
			get
			{
				return this.everDamage;
			}
		}

		// Token: 0x060072B3 RID: 29363 RVA: 0x00281B26 File Offset: 0x0027FD26
		public void Notify_DamageTaken(Thing damagee, float amount)
		{
			if (damagee.Faction != Faction.OfPlayer)
			{
				return;
			}
			this.everDamage += amount;
		}

		// Token: 0x060072B4 RID: 29364 RVA: 0x00281B44 File Offset: 0x0027FD44
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.everDamage, "everDamage", 0f, false);
		}

		// Token: 0x04004882 RID: 18562
		private float everDamage;
	}
}
