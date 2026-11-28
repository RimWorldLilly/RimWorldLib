using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200136A RID: 4970
	public class CompStunnable : ThingComp
	{
		// Token: 0x17001506 RID: 5382
		// (get) Token: 0x06007B27 RID: 31527 RVA: 0x002BD2B1 File Offset: 0x002BB4B1
		private CompProperties_Stunnable Props
		{
			get
			{
				return (CompProperties_Stunnable)this.props;
			}
		}

		// Token: 0x17001507 RID: 5383
		// (get) Token: 0x06007B28 RID: 31528 RVA: 0x002BD2BE File Offset: 0x002BB4BE
		public StunHandler StunHandler
		{
			get
			{
				return this.stunHandler;
			}
		}

		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x06007B29 RID: 31529 RVA: 0x002BD2C6 File Offset: 0x002BB4C6
		public bool UseLargeEMPEffecter
		{
			get
			{
				return this.Props.useLargeEMPEffecter;
			}
		}

		// Token: 0x17001509 RID: 5385
		// (get) Token: 0x06007B2A RID: 31530 RVA: 0x002BD2D3 File Offset: 0x002BB4D3
		public Vector3? EMPEffecterDimensions
		{
			get
			{
				return this.Props.empEffecterDimensions;
			}
		}

		// Token: 0x1700150A RID: 5386
		// (get) Token: 0x06007B2B RID: 31531 RVA: 0x002BD2E0 File Offset: 0x002BB4E0
		public Vector3? EMPEffecterOffset
		{
			get
			{
				return this.Props.empEffecterOffset;
			}
		}

		// Token: 0x1700150B RID: 5387
		// (get) Token: 0x06007B2C RID: 31532 RVA: 0x002BD2ED File Offset: 0x002BB4ED
		public float? EMPChancePerTick
		{
			get
			{
				return this.Props.empChancePerTick;
			}
		}

		// Token: 0x06007B2D RID: 31533 RVA: 0x002BD2FA File Offset: 0x002BB4FA
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.stunHandler = new StunHandler(this.parent);
		}

		// Token: 0x06007B2E RID: 31534 RVA: 0x002BD314 File Offset: 0x002BB514
		public override void CompTick()
		{
			this.stunHandler.StunHandlerTick();
		}

		// Token: 0x06007B2F RID: 31535 RVA: 0x002BD324 File Offset: 0x002BB524
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<StunHandler>(ref this.stunHandler, "stunHandler", new object[]
			{
				this.parent
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.stunHandler == null)
			{
				this.stunHandler = new StunHandler(this.parent);
			}
		}

		// Token: 0x06007B30 RID: 31536 RVA: 0x002BD378 File Offset: 0x002BB578
		public override string CompInspectStringExtra()
		{
			if (!this.stunHandler.Stunned)
			{
				return null;
			}
			if (this.stunHandler.Hypnotized)
			{
				return "InTrance".Translate();
			}
			if (this.stunHandler.StunFromEMP)
			{
				return "StunnedByEMP".Translate().CapitalizeFirst() + ": " + this.stunHandler.StunTicksLeft.ToStringSecondsFromTicks();
			}
			return "StunLower".Translate().CapitalizeFirst() + ": " + this.stunHandler.StunTicksLeft.ToStringSecondsFromTicks();
		}

		// Token: 0x06007B31 RID: 31537 RVA: 0x002BD42B File Offset: 0x002BB62B
		public void ApplyDamage(DamageInfo damageInfo)
		{
			this.stunHandler.Notify_DamageApplied(damageInfo);
		}

		// Token: 0x06007B32 RID: 31538 RVA: 0x002BD439 File Offset: 0x002BB639
		public bool CanBeStunnedByDamage(DamageDef def)
		{
			return this.Props.affectedDamageDefs.Contains(def);
		}

		// Token: 0x06007B33 RID: 31539 RVA: 0x002BD44C File Offset: 0x002BB64C
		public bool CanAdaptToDamage(DamageDef def)
		{
			return this.Props.adaptableDamageDefs.Contains(def);
		}

		// Token: 0x04004DC0 RID: 19904
		private StunHandler stunHandler;
	}
}
