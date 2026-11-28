using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A34 RID: 6708
	public class CompUseEffect_CallBossgroup : CompUseEffect
	{
		// Token: 0x17001D86 RID: 7558
		// (get) Token: 0x0600A850 RID: 43088 RVA: 0x003BCF2E File Offset: 0x003BB12E
		public CompProperties_Useable_CallBossgroup Props
		{
			get
			{
				return (CompProperties_Useable_CallBossgroup)this.props;
			}
		}

		// Token: 0x17001D87 RID: 7559
		// (get) Token: 0x0600A851 RID: 43089 RVA: 0x003BCF3C File Offset: 0x003BB13C
		public bool ShouldSendSpawnLetter
		{
			get
			{
				return !this.Props.spawnLetterLabelKey.NullOrEmpty() && !this.Props.spawnLetterTextKey.NullOrEmpty() && MechanitorUtility.AnyMechanitorInPlayerFaction() && Find.BossgroupManager.lastBossgroupCalled <= 0;
			}
		}

		// Token: 0x0600A852 RID: 43090 RVA: 0x003BCF88 File Offset: 0x003BB188
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			if (this.Props.effecterDef != null)
			{
				Effecter effecter = new Effecter(this.Props.effecterDef);
				effecter.Trigger(new TargetInfo(this.parent.Position, this.parent.Map, false), TargetInfo.Invalid, -1);
				effecter.Cleanup();
			}
			Effecter effecter2 = this.prepareEffecter;
			if (effecter2 != null)
			{
				effecter2.Cleanup();
			}
			this.prepareEffecter = null;
			this.CallBossgroup();
		}

		// Token: 0x0600A853 RID: 43091 RVA: 0x003BD008 File Offset: 0x003BB208
		private void CallBossgroup()
		{
			GameComponent_Bossgroup component = Current.Game.GetComponent<GameComponent_Bossgroup>();
			if (component == null)
			{
				Log.Error("Trying to call bossgroup with no GameComponent_Bossgroup.");
				return;
			}
			this.Props.bossgroupDef.Worker.Resolve(this.parent.Map, component.NumTimesCalledBossgroup(this.Props.bossgroupDef));
		}

		// Token: 0x0600A854 RID: 43092 RVA: 0x003BD060 File Offset: 0x003BB260
		public override TaggedString ConfirmMessage(Pawn p)
		{
			GameComponent_Bossgroup component = Current.Game.GetComponent<GameComponent_Bossgroup>();
			return "BossgroupWarningDialog".Translate(this.Props.bossgroupDef.boss.kindDef.Named("LEADERKIND"), this.Props.bossgroupDef.GetWaveDescription(component.NumTimesCalledBossgroup(this.Props.bossgroupDef)).Named("PAWNS"));
		}

		// Token: 0x0600A855 RID: 43093 RVA: 0x003BD0CC File Offset: 0x003BB2CC
		public override void PrepareTick()
		{
			if (this.Props.prepareEffecterDef != null && this.prepareEffecter == null)
			{
				this.prepareEffecter = this.Props.prepareEffecterDef.Spawn(this.parent.Position, this.parent.MapHeld, 1f);
			}
			Effecter effecter = this.prepareEffecter;
			if (effecter == null)
			{
				return;
			}
			effecter.EffectTick(this.parent, TargetInfo.Invalid);
		}

		// Token: 0x0600A856 RID: 43094 RVA: 0x003BD140 File Offset: 0x003BB340
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if (Faction.OfMechanoids == null || Faction.OfMechanoids.deactivated)
			{
				return "MechsDisabled".Translate();
			}
			if (!MechanitorUtility.IsMechanitor(p))
			{
				return "RequiresMechanitor".Translate();
			}
			return this.Props.bossgroupDef.Worker.CanResolve(p);
		}

		// Token: 0x0600A857 RID: 43095 RVA: 0x003BD1A0 File Offset: 0x003BB3A0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Call bossgroup"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (respawningAfterLoad)
			{
				return;
			}
			if (this.ShouldSendSpawnLetter)
			{
				this.Props.SendBossgroupDetailsLetter(this.Props.spawnLetterLabelKey, this.Props.spawnLetterTextKey, this.parent.def);
			}
		}

		// Token: 0x040062E8 RID: 25320
		private Effecter prepareEffecter;
	}
}
