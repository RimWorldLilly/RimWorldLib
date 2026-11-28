using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193F RID: 6463
	public class CompProperties_ProjectileInterceptor : CompProperties
	{
		// Token: 0x0600A20B RID: 41483 RVA: 0x0039E814 File Offset: 0x0039CA14
		public CompProperties_ProjectileInterceptor()
		{
			this.compClass = typeof(CompProjectileInterceptor);
		}

		// Token: 0x0600A20C RID: 41484 RVA: 0x0039E871 File Offset: 0x0039CA71
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_ProjectileInterceptor.<ConfigErrors>d__26 <ConfigErrors>d__ = new CompProperties_ProjectileInterceptor.<ConfigErrors>d__26(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005FB3 RID: 24499
		public float radius;

		// Token: 0x04005FB4 RID: 24500
		public int cooldownTicks;

		// Token: 0x04005FB5 RID: 24501
		public int disarmedByEmpForTicks;

		// Token: 0x04005FB6 RID: 24502
		public bool interceptGroundProjectiles;

		// Token: 0x04005FB7 RID: 24503
		public bool interceptAirProjectiles;

		// Token: 0x04005FB8 RID: 24504
		public bool interceptNonHostileProjectiles;

		// Token: 0x04005FB9 RID: 24505
		public bool interceptOutgoingProjectiles;

		// Token: 0x04005FBA RID: 24506
		public bool drawWithNoSelection;

		// Token: 0x04005FBB RID: 24507
		public int chargeIntervalTicks;

		// Token: 0x04005FBC RID: 24508
		public int chargeDurationTicks;

		// Token: 0x04005FBD RID: 24509
		public float minAlpha;

		// Token: 0x04005FBE RID: 24510
		public float idlePulseSpeed = 0.7f;

		// Token: 0x04005FBF RID: 24511
		public float minIdleAlpha = -1.7f;

		// Token: 0x04005FC0 RID: 24512
		public int hitPoints = -1;

		// Token: 0x04005FC1 RID: 24513
		public int rechargeHitPointsIntervalTicks = 240;

		// Token: 0x04005FC2 RID: 24514
		[NoTranslate]
		public string gizmoTipKey;

		// Token: 0x04005FC3 RID: 24515
		public bool hitPointsRestoreInstantlyAfterCharge;

		// Token: 0x04005FC4 RID: 24516
		public bool startWithMaxHitPoints = true;

		// Token: 0x04005FC5 RID: 24517
		public bool alwaysShowHitpointsGizmo;

		// Token: 0x04005FC6 RID: 24518
		public bool activated;

		// Token: 0x04005FC7 RID: 24519
		public int activeDuration;

		// Token: 0x04005FC8 RID: 24520
		public Color color = Color.white;

		// Token: 0x04005FC9 RID: 24521
		public EffecterDef reactivateEffect;

		// Token: 0x04005FCA RID: 24522
		public EffecterDef interceptEffect;

		// Token: 0x04005FCB RID: 24523
		public SoundDef activeSound;
	}
}
