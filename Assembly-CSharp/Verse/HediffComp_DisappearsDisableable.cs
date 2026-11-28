using System;

namespace Verse
{
	// Token: 0x02000420 RID: 1056
	public class HediffComp_DisappearsDisableable : HediffComp_DisappearsPausable
	{
		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001EF7 RID: 7927 RVA: 0x000BD38B File Offset: 0x000BB58B
		public new HediffCompProperties_DisappearsDisableable Props
		{
			get
			{
				return (HediffCompProperties_DisappearsDisableable)this.props;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001EF8 RID: 7928 RVA: 0x000BD398 File Offset: 0x000BB598
		protected override bool Paused
		{
			get
			{
				return this.disabled;
			}
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x000BD3A0 File Offset: 0x000BB5A0
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.disabled = this.Props.initiallyDisabled;
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001EFA RID: 7930 RVA: 0x000BD3B9 File Offset: 0x000BB5B9
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.disabled)
				{
					return base.CompLabelInBracketsExtra;
				}
				return null;
			}
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x000BD3CC File Offset: 0x000BB5CC
		public override void CopyFrom(HediffComp other)
		{
			base.CopyFrom(other);
			HediffComp_DisappearsDisableable hediffComp_DisappearsDisableable = other as HediffComp_DisappearsDisableable;
			if (hediffComp_DisappearsDisableable != null)
			{
				this.disabled = hediffComp_DisappearsDisableable.disabled;
			}
		}

		// Token: 0x04001843 RID: 6211
		public bool disabled;
	}
}
