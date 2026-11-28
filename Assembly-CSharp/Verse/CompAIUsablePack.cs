using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000537 RID: 1335
	public abstract class CompAIUsablePack : ThingComp
	{
		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06002750 RID: 10064 RVA: 0x000F6028 File Offset: 0x000F4228
		protected CompProperties_AIUSablePack Props
		{
			get
			{
				return (CompProperties_AIUSablePack)this.props;
			}
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x000F6038 File Offset: 0x000F4238
		public override void CompTick()
		{
			Pawn wearer = ((Apparel)this.parent).Wearer;
			if (this.CanOpportunisticallyUseNow(wearer))
			{
				this.UsePack(wearer);
			}
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x000F6068 File Offset: 0x000F4268
		private bool CanOpportunisticallyUseNow(Pawn wearer)
		{
			return wearer != null && !wearer.Dead && wearer.Spawned && wearer.IsHashIntervalTick(this.Props.checkInterval) && !wearer.Downed && wearer.Awake() && !wearer.IsColonistPlayerControlled && !wearer.InMentalState && Rand.Value < this.ChanceToUse(wearer);
		}

		// Token: 0x06002753 RID: 10067
		protected abstract float ChanceToUse(Pawn wearer);

		// Token: 0x06002754 RID: 10068
		protected abstract void UsePack(Pawn wearer);
	}
}
