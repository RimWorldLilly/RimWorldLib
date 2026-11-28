using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000589 RID: 1417
	public abstract class Mote : Thing
	{
		// Token: 0x17000852 RID: 2130
		// (set) Token: 0x060029CE RID: 10702 RVA: 0x000FF9FD File Offset: 0x000FDBFD
		public float Scale
		{
			set
			{
				this.linearScale = new Vector3(value, 1f, value);
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x060029CF RID: 10703 RVA: 0x000FFA11 File Offset: 0x000FDC11
		public float AgeSecs
		{
			get
			{
				if (this.def.mote.realTime)
				{
					return Time.realtimeSinceStartup - this.spawnRealTime;
				}
				return (float)(Find.TickManager.TicksGame - this.spawnTick - this.pausedTicks) / 60f;
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x060029D0 RID: 10704 RVA: 0x000FFA51 File Offset: 0x000FDC51
		public float AgeSecsPausable
		{
			get
			{
				return (float)this.currentAnimationTick / 60f;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x060029D1 RID: 10705 RVA: 0x000FFA60 File Offset: 0x000FDC60
		protected float SolidTime
		{
			get
			{
				if (this.solidTimeOverride >= 0f)
				{
					return this.solidTimeOverride;
				}
				return this.def.mote.solidTime;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x060029D2 RID: 10706 RVA: 0x000FFA86 File Offset: 0x000FDC86
		public Vector3 ExactScale
		{
			get
			{
				return Vector3.Scale(this.linearScale, this.curvedScale);
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x060029D3 RID: 10707 RVA: 0x000FFA9C File Offset: 0x000FDC9C
		public override Vector3 DrawPos
		{
			get
			{
				float z = 0f;
				if (this.def.mote.archDuration > 0f && this.AgeSecs < this.def.mote.archDuration + this.def.mote.archStartOffset)
				{
					z = (Mathf.Cos(Mathf.Clamp01((this.AgeSecs + this.def.mote.archStartOffset) / this.def.mote.archDuration) * 3.1415927f * 2f - 3.1415927f) + 1f) / 2f * this.def.mote.archHeight;
				}
				int num = this.GetHashCode();
				if (num == -2147483648)
				{
					num++;
				}
				float y = (float)Mathf.Abs(num) / 2.1474836E+09f * 0.03658537f * this.def.mote.yFightingOffsetScalar01;
				return this.exactPosition + this.def.mote.unattachedDrawOffset + new Vector3(0f, y, z);
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x060029D4 RID: 10708 RVA: 0x000FFBB8 File Offset: 0x000FDDB8
		protected virtual bool EndOfLife
		{
			get
			{
				return this.AgeSecs >= this.def.mote.Lifespan;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x060029D5 RID: 10709 RVA: 0x000FFBD8 File Offset: 0x000FDDD8
		public virtual float Alpha
		{
			get
			{
				float ageSecs = this.AgeSecs;
				if (this.def.mote.fadeOutUnmaintained && Find.TickManager.TicksGame - this.lastMaintainTick > 0)
				{
					if (this.def.mote.fadeOutTime > 0f)
					{
						float num = (Find.TickManager.TicksGame - this.lastMaintainTick).TicksToSeconds();
						return 1f - num / this.def.mote.fadeOutTime;
					}
					return 1f;
				}
				else if (ageSecs <= this.def.mote.fadeInTime)
				{
					if (this.def.mote.fadeInTime > 0f)
					{
						return ageSecs / this.def.mote.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (ageSecs <= this.def.mote.fadeInTime + this.SolidTime)
					{
						return 1f;
					}
					if (this.def.mote.fadeOutTime > 0f)
					{
						return 1f - Mathf.InverseLerp(this.def.mote.fadeInTime + this.SolidTime, this.def.mote.fadeInTime + this.SolidTime + this.def.mote.fadeOutTime, ageSecs);
					}
					return 1f;
				}
			}
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x000FFD2C File Offset: 0x000FDF2C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
			this.spawnRealTime = Time.realtimeSinceStartup;
			RealTime.moteList.MoteSpawned(this);
			base.Map.moteCounter.Notify_MoteSpawned();
			if (this.exactPosition == Vector3.zero)
			{
				this.exactPosition = base.Position.ToVector3();
				this.exactPosition.y = this.def.altitudeLayer.AltitudeFor() + this.yOffset;
			}
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x000FFDBF File Offset: 0x000FDFBF
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			RealTime.moteList.MoteDespawned(this);
			map.moteCounter.Notify_MoteDespawned();
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x000FFDE4 File Offset: 0x000FDFE4
		protected override void Tick()
		{
			if (!this.def.mote.realTime)
			{
				this.TimeInterval(0.016666668f);
			}
			if (!this.animationPaused)
			{
				this.currentAnimationTick++;
			}
			if (this.paused)
			{
				this.pausedTicks++;
			}
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x000FFE3A File Offset: 0x000FE03A
		public void RealtimeUpdate()
		{
			if (this.def.mote.realTime)
			{
				this.TimeInterval(Time.deltaTime);
			}
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x000FFE5C File Offset: 0x000FE05C
		protected virtual void TimeInterval(float deltaTime)
		{
			if (this.EndOfLife && !base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.def.mote.needsMaintenance && Find.TickManager.TicksGame > this.lastMaintainTick)
			{
				int num = this.def.mote.fadeOutTime.SecondsToTicks();
				if (!this.def.mote.fadeOutUnmaintained || Find.TickManager.TicksGame - this.lastMaintainTick > num)
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			if (this.def.mote.growthRate != 0f)
			{
				this.linearScale = new Vector3(this.linearScale.x + this.def.mote.growthRate * deltaTime, this.linearScale.y, this.linearScale.z + this.def.mote.growthRate * deltaTime);
				this.linearScale.x = Mathf.Max(this.linearScale.x, 0.0001f);
				this.linearScale.z = Mathf.Max(this.linearScale.z, 0.0001f);
			}
			if (this.def.mote.scalers != null)
			{
				this.curvedScale = this.def.mote.scalers.ScaleAtTime(this.AgeSecs);
			}
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x000FFFC8 File Offset: 0x000FE1C8
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.DrawMote(this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x060029DC RID: 10716 RVA: 0x000FFFE0 File Offset: 0x000FE1E0
		protected void DrawMote(float altitude)
		{
			if (this.paused || Find.UIRoot.HideMotes)
			{
				return;
			}
			this.exactPosition.y = altitude + this.yOffset;
			base.DrawAt(this.exactPosition, false);
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x00100017 File Offset: 0x000FE217
		public void Maintain()
		{
			this.lastMaintainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060029DE RID: 10718 RVA: 0x00100029 File Offset: 0x000FE229
		public void Attach(TargetInfo a, Vector3 offset, bool rotateWithTarget = false)
		{
			this.link1 = new MoteAttachLink(a, offset, rotateWithTarget);
		}

		// Token: 0x060029DF RID: 10719 RVA: 0x00100039 File Offset: 0x000FE239
		public void Attach(TargetInfo a)
		{
			this.link1 = new MoteAttachLink(a, Vector3.zero, false);
		}

		// Token: 0x060029E0 RID: 10720 RVA: 0x0010004D File Offset: 0x000FE24D
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			RealTime.moteList.MoteDespawned(this);
		}

		// Token: 0x060029E1 RID: 10721 RVA: 0x00100060 File Offset: 0x000FE260
		public void ForceSpawnTick(int tick)
		{
			this.spawnTick = tick;
		}

		// Token: 0x04001CF6 RID: 7414
		public Vector3 exactPosition;

		// Token: 0x04001CF7 RID: 7415
		public float exactRotation;

		// Token: 0x04001CF8 RID: 7416
		public Vector3 linearScale = new Vector3(1f, 1f, 1f);

		// Token: 0x04001CF9 RID: 7417
		public Vector3 curvedScale = new Vector3(1f, 1f, 1f);

		// Token: 0x04001CFA RID: 7418
		public float rotationRate;

		// Token: 0x04001CFB RID: 7419
		public float yOffset;

		// Token: 0x04001CFC RID: 7420
		public Color instanceColor = Color.white;

		// Token: 0x04001CFD RID: 7421
		private int lastMaintainTick;

		// Token: 0x04001CFE RID: 7422
		private int currentAnimationTick;

		// Token: 0x04001CFF RID: 7423
		public float solidTimeOverride = -1f;

		// Token: 0x04001D00 RID: 7424
		public int pausedTicks;

		// Token: 0x04001D01 RID: 7425
		public bool paused;

		// Token: 0x04001D02 RID: 7426
		public int spawnTick;

		// Token: 0x04001D03 RID: 7427
		public bool animationPaused;

		// Token: 0x04001D04 RID: 7428
		public int detachAfterTicks = -1;

		// Token: 0x04001D05 RID: 7429
		public float spawnRealTime;

		// Token: 0x04001D06 RID: 7430
		public MoteAttachLink link1 = MoteAttachLink.Invalid;

		// Token: 0x04001D07 RID: 7431
		protected float skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);

		// Token: 0x04001D08 RID: 7432
		public int offsetRandom = Rand.Range(0, 99999);

		// Token: 0x04001D09 RID: 7433
		protected const float MinSpeed = 0.02f;
	}
}
