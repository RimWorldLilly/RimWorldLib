using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AD4 RID: 6868
	public class CompDeathrestBindable : ThingComp
	{
		// Token: 0x17001F80 RID: 8064
		// (get) Token: 0x0600AF03 RID: 44803 RVA: 0x003D9FB8 File Offset: 0x003D81B8
		public CompProperties_DeathrestBindable Props
		{
			get
			{
				return (CompProperties_DeathrestBindable)this.props;
			}
		}

		// Token: 0x17001F81 RID: 8065
		// (get) Token: 0x0600AF04 RID: 44804 RVA: 0x003D9FC5 File Offset: 0x003D81C5
		public Pawn BoundPawn
		{
			get
			{
				return this.boundPawn;
			}
		}

		// Token: 0x17001F82 RID: 8066
		// (get) Token: 0x0600AF05 RID: 44805 RVA: 0x003D9FD0 File Offset: 0x003D81D0
		public bool CanIncreasePresence
		{
			get
			{
				if (this.PowerTraderComp != null && !this.PowerTraderComp.PowerOn)
				{
					return false;
				}
				if (this.RefuelableComp != null && !this.RefuelableComp.HasFuel)
				{
					return false;
				}
				if (!this.boundPawn.InBed())
				{
					return false;
				}
				Building_Bed building_Bed = this.boundPawn.CurrentBed();
				if (building_Bed == this.parent)
				{
					return true;
				}
				CompDeathrestBindable compDeathrestBindable = building_Bed.TryGetComp<CompDeathrestBindable>();
				return compDeathrestBindable != null && compDeathrestBindable.BoundPawn == this.boundPawn;
			}
		}

		// Token: 0x17001F83 RID: 8067
		// (get) Token: 0x0600AF06 RID: 44806 RVA: 0x003DA04C File Offset: 0x003D824C
		private CompPowerTrader PowerTraderComp
		{
			get
			{
				if (this.cachedPowerComp == null)
				{
					this.cachedPowerComp = this.parent.TryGetComp<CompPowerTrader>();
				}
				return this.cachedPowerComp;
			}
		}

		// Token: 0x17001F84 RID: 8068
		// (get) Token: 0x0600AF07 RID: 44807 RVA: 0x003DA06D File Offset: 0x003D826D
		private CompRefuelable RefuelableComp
		{
			get
			{
				if (this.cachedRefuelableComp == null)
				{
					this.cachedRefuelableComp = this.parent.TryGetComp<CompRefuelable>();
				}
				return this.cachedRefuelableComp;
			}
		}

		// Token: 0x17001F85 RID: 8069
		// (get) Token: 0x0600AF08 RID: 44808 RVA: 0x003DA08E File Offset: 0x003D828E
		private Gene_Deathrest DeathrestGene
		{
			get
			{
				if (this.cachedDeathrestGene == null)
				{
					Pawn pawn = this.boundPawn;
					Gene_Deathrest gene_Deathrest;
					if (pawn == null)
					{
						gene_Deathrest = null;
					}
					else
					{
						Pawn_GeneTracker genes = pawn.genes;
						gene_Deathrest = ((genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null);
					}
					this.cachedDeathrestGene = gene_Deathrest;
				}
				return this.cachedDeathrestGene;
			}
		}

		// Token: 0x17001F86 RID: 8070
		// (get) Token: 0x0600AF09 RID: 44809 RVA: 0x003DA0C2 File Offset: 0x003D82C2
		private Material HoseMat
		{
			get
			{
				if (this.cachedHoseMat == null)
				{
					this.cachedHoseMat = MaterialPool.MatFrom("Other/DeathrestBuildingConnection");
				}
				return this.cachedHoseMat;
			}
		}

		// Token: 0x0600AF0A RID: 44810 RVA: 0x003DA0E8 File Offset: 0x003D82E8
		public override void PostPostMake()
		{
			if (!ModLister.CheckBiotech("Deathrest binding"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostPostMake();
		}

		// Token: 0x0600AF0B RID: 44811 RVA: 0x003DA109 File Offset: 0x003D8309
		public bool CanBindTo(Pawn pawn)
		{
			return (!this.Props.mustBeLayingInToBind || pawn.CurrentBed() == this.parent) && (this.boundPawn == null || this.boundPawn == pawn);
		}

		// Token: 0x0600AF0C RID: 44812 RVA: 0x003DA13B File Offset: 0x003D833B
		public void BindTo(Pawn pawn)
		{
			this.boundPawn = pawn;
		}

		// Token: 0x0600AF0D RID: 44813 RVA: 0x003DA144 File Offset: 0x003D8344
		public void Notify_DeathrestGeneRemoved()
		{
			this.cachedDeathrestGene = null;
			this.boundPawn = null;
			this.presenceTicks = 0;
		}

		// Token: 0x0600AF0E RID: 44814 RVA: 0x003DA15C File Offset: 0x003D835C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad || this.parent.BeingTransportedOnGravship)
			{
				return;
			}
			this.presenceTicks = 0;
			foreach (Pawn pawn in this.parent.Map.mapPawns.PawnsInFaction(this.parent.Faction))
			{
				if (pawn.Deathresting)
				{
					Pawn_GeneTracker genes = pawn.genes;
					Gene_Deathrest gene_Deathrest = (genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null;
					if (gene_Deathrest != null && gene_Deathrest.CanBindToBindable(this))
					{
						gene_Deathrest.BindTo(this);
					}
				}
			}
		}

		// Token: 0x0600AF0F RID: 44815 RVA: 0x003DA208 File Offset: 0x003D8408
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.parent.BeingTransportedOnGravship)
			{
				return;
			}
			this.presenceTicks = 0;
			if (mode != DestroyMode.WillReplace && this.boundPawn != null)
			{
				Pawn_GeneTracker genes = this.boundPawn.genes;
				if (genes != null)
				{
					Gene_Deathrest firstGeneOfType = genes.GetFirstGeneOfType<Gene_Deathrest>();
					if (firstGeneOfType != null)
					{
						firstGeneOfType.Notify_BoundBuildingDeSpawned(this.parent);
					}
				}
			}
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
			this.sustainer = null;
		}

		// Token: 0x0600AF10 RID: 44816 RVA: 0x003DA284 File Offset: 0x003D8484
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(250))
			{
				this.CompTickRare();
			}
			if (this.presenceTicks > 0)
			{
				if (!this.Props.soundWorking.NullOrUndefined())
				{
					if (this.sustainer == null || this.sustainer.Ended)
					{
						this.sustainer = this.Props.soundWorking.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
					}
					this.sustainer.Maintain();
				}
				CompRefuelable refuelableComp = this.RefuelableComp;
				if (refuelableComp == null)
				{
					return;
				}
				refuelableComp.Notify_UsedThisTick();
			}
		}

		// Token: 0x0600AF11 RID: 44817 RVA: 0x003DA31C File Offset: 0x003D851C
		public override void CompTickRare()
		{
			if (this.PowerTraderComp != null)
			{
				this.PowerTraderComp.PowerOutput = ((this.presenceTicks > 0) ? (-this.PowerTraderComp.Props.PowerConsumption) : (-this.PowerTraderComp.Props.idlePowerDraw));
			}
		}

		// Token: 0x0600AF12 RID: 44818 RVA: 0x003DA36C File Offset: 0x003D856C
		public override void PostDraw()
		{
			if (!this.Props.mustBeLayingInToBind && this.boundPawn != null && this.boundPawn.Map == this.parent.Map && this.boundPawn.Deathresting && this.DeathrestGene != null && this.DeathrestGene.BoundComps.Contains(this) && this.CanIncreasePresence)
			{
				Vector3 a = this.boundPawn.CurrentBed().TrueCenter();
				Vector3 b = this.parent.TrueCenter();
				a.y = (b.y = AltitudeLayer.SmallWire.AltitudeFor());
				Matrix4x4 identity = Matrix4x4.identity;
				identity.SetTRS((a + b) / 2f, Quaternion.Euler(0f, a.AngleToFlat(b) + 90f, 0f), new Vector3(1f, 1f, (a - b).MagnitudeHorizontal()));
				Graphics.DrawMesh(MeshPool.plane10, identity, this.HoseMat, 0);
			}
		}

		// Token: 0x0600AF13 RID: 44819 RVA: 0x003DA488 File Offset: 0x003D8688
		public void Apply()
		{
			if (this.boundPawn != null)
			{
				if (this.Props.hediffToApply != null)
				{
					this.boundPawn.health.AddHediff(this.Props.hediffToApply, null, null, null);
				}
				if (this.Props.hemogenLimitOffset > 0f)
				{
					Pawn_GeneTracker genes = this.boundPawn.genes;
					Gene_Hemogen gene_Hemogen = (genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null;
					if (gene_Hemogen != null)
					{
						gene_Hemogen.SetMax(gene_Hemogen.Max + this.Props.hemogenLimitOffset);
					}
				}
			}
		}

		// Token: 0x0600AF14 RID: 44820 RVA: 0x003DA518 File Offset: 0x003D8718
		public void TryIncreasePresence(int delta)
		{
			if (!this.CanIncreasePresence)
			{
				return;
			}
			if (this.presenceTicks <= 0)
			{
				SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.None);
				if (!this.Props.soundWorking.NullOrUndefined() && (this.sustainer == null || this.sustainer.Ended))
				{
					this.sustainer = this.Props.soundWorking.TrySpawnSustainer(info);
				}
				if (!this.Props.soundStart.NullOrUndefined())
				{
					this.Props.soundStart.PlayOneShot(info);
				}
			}
			this.presenceTicks += delta;
		}

		// Token: 0x0600AF15 RID: 44821 RVA: 0x003DA5B8 File Offset: 0x003D87B8
		public void Notify_DeathrestEnded()
		{
			this.presenceTicks = 0;
			if (this.parent.Spawned)
			{
				if (this.sustainer != null && !this.sustainer.Ended)
				{
					this.sustainer.End();
				}
				if (!this.Props.soundEnd.NullOrUndefined())
				{
					this.Props.soundEnd.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
				}
			}
		}

		// Token: 0x0600AF16 RID: 44822 RVA: 0x003DA62C File Offset: 0x003D882C
		public override string CompInspectStringExtra()
		{
			string text = null;
			if (this.boundPawn != null && this.DeathrestGene != null)
			{
				string str = text;
				string str2 = ("BoundTo".Translate() + ": " + this.boundPawn.NameShortColored).Resolve();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 3);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.DeathrestGene.CurrentCapacity);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.DeathrestGene.DeathrestCapacity);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("DeathrestCapacity".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(")");
				text = str + str2 + defaultInterpolatedStringHandler.ToStringAndClear();
				if (this.Props.displayTimeActive && this.presenceTicks > 0 && this.DeathrestGene.deathrestTicks > 0)
				{
					float f = Mathf.Clamp01((float)this.presenceTicks / (float)this.DeathrestGene.deathrestTicks);
					string str3 = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(10, 5);
					defaultInterpolatedStringHandler2.AppendLiteral("\n");
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("TimeActiveThisDeathrest".Translate());
					defaultInterpolatedStringHandler2.AppendLiteral(": ");
					defaultInterpolatedStringHandler2.AppendFormatted(this.presenceTicks.ToStringTicksToPeriod(true, true, true, true, false));
					defaultInterpolatedStringHandler2.AppendLiteral(" / ");
					defaultInterpolatedStringHandler2.AppendFormatted(this.DeathrestGene.deathrestTicks.ToStringTicksToPeriod(true, true, true, true, false));
					defaultInterpolatedStringHandler2.AppendLiteral(" (");
					defaultInterpolatedStringHandler2.AppendFormatted(f.ToStringPercent());
					defaultInterpolatedStringHandler2.AppendLiteral(")\n");
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("MinimumNeededToApply".Translate(0.75f.ToStringPercent()));
					text = str3 + defaultInterpolatedStringHandler2.ToStringAndClear();
				}
			}
			else
			{
				text += "WillBindOnFirstUse".Translate();
			}
			return text;
		}

		// Token: 0x0600AF17 RID: 44823 RVA: 0x003DA81E File Offset: 0x003D8A1E
		public override void PostExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.boundPawn, "boundPawn", false);
			Scribe_Values.Look<int>(ref this.presenceTicks, "presenceTicks", 0, false);
		}

		// Token: 0x0400652E RID: 25902
		private Pawn boundPawn;

		// Token: 0x0400652F RID: 25903
		public int presenceTicks;

		// Token: 0x04006530 RID: 25904
		[Unsaved(false)]
		private CompPowerTrader cachedPowerComp;

		// Token: 0x04006531 RID: 25905
		[Unsaved(false)]
		private CompRefuelable cachedRefuelableComp;

		// Token: 0x04006532 RID: 25906
		[Unsaved(false)]
		private Gene_Deathrest cachedDeathrestGene;

		// Token: 0x04006533 RID: 25907
		[Unsaved(false)]
		private Material cachedHoseMat;

		// Token: 0x04006534 RID: 25908
		[Unsaved(false)]
		private Sustainer sustainer;
	}
}
