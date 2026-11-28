using System;

namespace Verse
{
	// Token: 0x02000418 RID: 1048
	[StaticConstructorOnStartup]
	public class HediffComp_Chargeable : HediffComp
	{
		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001ECA RID: 7882 RVA: 0x000BCB81 File Offset: 0x000BAD81
		public HediffCompProperties_Chargeable Props
		{
			get
			{
				return (HediffCompProperties_Chargeable)this.props;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001ECB RID: 7883 RVA: 0x000BCB8E File Offset: 0x000BAD8E
		// (set) Token: 0x06001ECC RID: 7884 RVA: 0x000BCB96 File Offset: 0x000BAD96
		public float Charge
		{
			get
			{
				return this.charge;
			}
			protected set
			{
				this.charge = value;
				if (this.charge > this.Props.fullChargeAmount)
				{
					this.charge = this.Props.fullChargeAmount;
				}
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001ECD RID: 7885 RVA: 0x000BCBC3 File Offset: 0x000BADC3
		public bool CanActivate
		{
			get
			{
				return this.charge >= this.Props.minChargeToActivate;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001ECE RID: 7886 RVA: 0x000BCBDC File Offset: 0x000BADDC
		public override string CompLabelInBracketsExtra
		{
			get
			{
				return this.Props.labelInBrackets.Formatted(this.charge.Named("CHARGE"), (this.charge / this.Props.fullChargeAmount).Named("CHARGEFACTOR"));
			}
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x000BCC34 File Offset: 0x000BAE34
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.charge = this.Props.initialCharge;
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x000BCC4D File Offset: 0x000BAE4D
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.charge, "charge", this.Props.initialCharge, false);
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x000BCC71 File Offset: 0x000BAE71
		public virtual void TryCharge(float desiredChargeAmount)
		{
			this.Charge += desiredChargeAmount;
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000BCC84 File Offset: 0x000BAE84
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			base.CompPostTickInterval(ref severityAdjustment, delta);
			if (this.Props.ticksToFullCharge > 0)
			{
				this.TryCharge(this.Props.fullChargeAmount / (float)this.Props.ticksToFullCharge * (float)delta);
				return;
			}
			if (this.Props.ticksToFullCharge == 0)
			{
				this.TryCharge(this.Props.fullChargeAmount);
			}
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000BCCE8 File Offset: 0x000BAEE8
		public float GreedyConsume(float desiredCharge)
		{
			float num;
			if (desiredCharge >= this.charge)
			{
				num = this.charge;
				this.charge = 0f;
			}
			else
			{
				num = desiredCharge;
				this.charge -= num;
			}
			return num;
		}

		// Token: 0x04001830 RID: 6192
		private float charge;
	}
}
