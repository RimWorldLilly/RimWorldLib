using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A69 RID: 6761
	public class Building_ProximityDetector : Building, IThingGlower
	{
		// Token: 0x17001DDF RID: 7647
		// (get) Token: 0x0600A9E5 RID: 43493 RVA: 0x003C3808 File Offset: 0x003C1A08
		public CompPowerTrader PowerTraderComp
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.powerTraderComp) == null)
				{
					result = (this.powerTraderComp = base.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x17001DE0 RID: 7648
		// (get) Token: 0x0600A9E6 RID: 43494 RVA: 0x003C3830 File Offset: 0x003C1A30
		public CompGlower Glower
		{
			get
			{
				CompGlower result;
				if ((result = this.glowerComp) == null)
				{
					result = (this.glowerComp = base.GetComp<CompGlower>());
				}
				return result;
			}
		}

		// Token: 0x17001DE1 RID: 7649
		// (get) Token: 0x0600A9E7 RID: 43495 RVA: 0x003C3856 File Offset: 0x003C1A56
		private TargetInfo TgtInfo
		{
			get
			{
				return new TargetInfo(base.Position, base.Map, false);
			}
		}

		// Token: 0x0600A9E8 RID: 43496 RVA: 0x003C386A File Offset: 0x003C1A6A
		public bool ShouldBeLitNow()
		{
			return this.triggered;
		}

		// Token: 0x0600A9E9 RID: 43497 RVA: 0x003C3872 File Offset: 0x003C1A72
		protected override void Tick()
		{
			base.Tick();
			Effecter effecter = this.detectionEffecter;
			if (effecter != null)
			{
				effecter.EffectTick(this.TgtInfo, this.TgtInfo);
			}
			if (!base.Spawned || !this.IsHashIntervalTick(30))
			{
				return;
			}
			this.RunDetection();
		}

		// Token: 0x0600A9EA RID: 43498 RVA: 0x003C38B0 File Offset: 0x003C1AB0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Proximity detector"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad)
			{
				this.TickDetectionEffect();
			}
		}

		// Token: 0x0600A9EB RID: 43499 RVA: 0x003C38D8 File Offset: 0x003C1AD8
		private void TickDetectionEffect()
		{
			if (this.triggered && this.detectionEffecter == null)
			{
				this.detectionEffecter = EffecterDefOf.ProximityDetectorAlert.Spawn(base.Position, base.Map, 1f);
				return;
			}
			if (!this.triggered && this.detectionEffecter != null)
			{
				this.detectionEffecter.Cleanup();
				this.detectionEffecter = null;
			}
		}

		// Token: 0x0600A9EC RID: 43500 RVA: 0x003C393C File Offset: 0x003C1B3C
		private void RunDetection()
		{
			bool flag = false;
			if (this.PowerTraderComp.PowerOn)
			{
				IReadOnlyList<Pawn> allPawnsSpawned = base.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].Position.InHorDistOf(base.Position, 19.9f) && allPawnsSpawned[i].IsPsychologicallyInvisible())
					{
						flag = true;
						break;
					}
				}
			}
			if (flag != this.triggered)
			{
				this.triggered = flag;
				this.Glower.UpdateLit(base.Map);
				if (this.triggered)
				{
					Messages.Message("MessageProximityDetectorTriggered".Translate(), this, MessageTypeDefOf.ThreatSmall, false);
				}
			}
			this.TickDetectionEffect();
		}

		// Token: 0x0600A9ED RID: 43501 RVA: 0x003C39FC File Offset: 0x003C1BFC
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.triggered)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "ProximityDetectorCreatureDetected".Translate().Colorize(ColorLibrary.RedReadable);
			}
			return text;
		}

		// Token: 0x0600A9EE RID: 43502 RVA: 0x003C3A48 File Offset: 0x003C1C48
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.triggered, "triggered", false, false);
		}

		// Token: 0x04006365 RID: 25445
		private bool triggered;

		// Token: 0x04006366 RID: 25446
		private Effecter detectionEffecter;

		// Token: 0x04006367 RID: 25447
		public const float DetectionRadius = 19.9f;

		// Token: 0x04006368 RID: 25448
		private const int DetectInterval = 30;

		// Token: 0x04006369 RID: 25449
		private CompPowerTrader powerTraderComp;

		// Token: 0x0400636A RID: 25450
		private CompGlower glowerComp;
	}
}
