using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000269 RID: 617
	public struct FleckStatic : IFleck
	{
		// Token: 0x17000361 RID: 865
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x00066BB1 File Offset: 0x00064DB1
		public float SolidTime
		{
			get
			{
				if (this.solidTimeOverride >= 0f)
				{
					return this.solidTimeOverride;
				}
				return this.def.solidTime;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x060011F3 RID: 4595 RVA: 0x00066BD2 File Offset: 0x00064DD2
		public Vector3 DrawPos
		{
			get
			{
				return this.position.ExactPosition;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060011F4 RID: 4596 RVA: 0x00066BDF File Offset: 0x00064DDF
		public float Lifespan
		{
			get
			{
				return this.def.fadeInTime + this.SolidTime + this.def.fadeOutTime;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x060011F5 RID: 4597 RVA: 0x00066BFF File Offset: 0x00064DFF
		public bool EndOfLife
		{
			get
			{
				return this.ageSecs >= this.Lifespan;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060011F6 RID: 4598 RVA: 0x00066C14 File Offset: 0x00064E14
		public float Alpha
		{
			get
			{
				float num = this.ageSecs;
				if (num <= this.def.fadeInTime)
				{
					if (this.def.fadeInTime > 0f)
					{
						return num / this.def.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (num <= this.def.fadeInTime + this.SolidTime)
					{
						return 1f;
					}
					if (this.def.fadeOutTime > 0f)
					{
						return 1f - Mathf.InverseLerp(this.def.fadeInTime + this.SolidTime, this.def.fadeInTime + this.SolidTime + this.def.fadeOutTime, num);
					}
					return 1f;
				}
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060011F7 RID: 4599 RVA: 0x00066CCB File Offset: 0x00064ECB
		public Vector3 ExactScale
		{
			get
			{
				return Vector3.Scale(this.linearScale, this.curvedScale);
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x060011F8 RID: 4600 RVA: 0x00066CDE File Offset: 0x00064EDE
		public Vector3 AddedScale
		{
			get
			{
				return this.ExactScale - this.originalScale;
			}
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00066CF4 File Offset: 0x00064EF4
		public void Setup(FleckCreationData creationData)
		{
			this.def = creationData.def;
			this.linearScale = Vector3.one;
			this.instanceColor = (creationData.instanceColor ?? Color.white);
			this.solidTimeOverride = creationData.solidTimeOverride.GetValueOrDefault(-1f);
			this.skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);
			this.ageSecs = 0f;
			if (creationData.exactScale != null)
			{
				this.linearScale = creationData.exactScale.Value;
			}
			else
			{
				this.linearScale = new Vector3(creationData.scale, 1f, creationData.scale);
			}
			this.originalScale = this.ExactScale;
			this.position = new FleckDrawPosition(creationData.spawnPosition, 0f, Vector3.zero, this.def.unattachedDrawOffset);
			this.spawnPosition = creationData.spawnPosition;
			this.exactRotation = creationData.rotation;
			this.setupTick = Find.TickManager.TicksGame;
			List<CurvedScaler> scalers = this.def.scalers;
			this.curvedScale = ((scalers != null) ? scalers.ScaleAtTime(0f) : Vector3.one);
			if (creationData.ageTicksOverride != -1)
			{
				this.ForceSpawnTick(creationData.ageTicksOverride);
			}
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00066E48 File Offset: 0x00065048
		public bool TimeInterval(float deltaTime, Map map)
		{
			if (this.EndOfLife)
			{
				return true;
			}
			this.ageSecs += deltaTime;
			this.ageTicks++;
			if (this.def.growthRate != 0f)
			{
				float num = Mathf.Sign(this.linearScale.x);
				float num2 = Mathf.Sign(this.linearScale.z);
				this.linearScale = new Vector3(this.linearScale.x + num * (this.def.growthRate * deltaTime), this.linearScale.y, this.linearScale.z + num2 * (this.def.growthRate * deltaTime));
				this.linearScale.x = ((num > 0f) ? Mathf.Max(this.linearScale.x, 0.0001f) : Mathf.Min(this.linearScale.x, -0.0001f));
				this.linearScale.z = ((num2 > 0f) ? Mathf.Max(this.linearScale.z, 0.0001f) : Mathf.Min(this.linearScale.z, -0.0001f));
			}
			if (this.def.scalers != null)
			{
				this.curvedScale = this.def.scalers.ScaleAtTime(this.ageSecs);
			}
			return false;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00066FA6 File Offset: 0x000651A6
		public void Draw(DrawBatch batch)
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor(this.def.altitudeLayerIncOffset), batch);
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00066FCC File Offset: 0x000651CC
		public void Draw(float altitude, DrawBatch batch)
		{
			this.position.worldPosition.y = altitude;
			int num = this.setupTick + this.spawnPosition.GetHashCode();
			((Graphic_Fleck)this.def.GetGraphicData(num).Graphic).DrawFleck(new FleckDrawData
			{
				alpha = this.Alpha,
				color = this.instanceColor,
				drawLayer = 0,
				pos = this.DrawPos,
				rotation = this.exactRotation,
				scale = this.ExactScale,
				ageSecs = this.ageSecs,
				id = (float)num
			}, batch);
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00067087 File Offset: 0x00065287
		public void ForceSpawnTick(int tick)
		{
			this.ageTicks = Find.TickManager.TicksGame - tick;
			this.ageSecs = this.ageTicks.TicksToSeconds();
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x000670AC File Offset: 0x000652AC
		public Vector3 GetPosition()
		{
			return this.position.worldPosition;
		}

		// Token: 0x0400104A RID: 4170
		public FleckDef def;

		// Token: 0x0400104B RID: 4171
		public Map map;

		// Token: 0x0400104C RID: 4172
		public FleckDrawPosition position;

		// Token: 0x0400104D RID: 4173
		public float exactRotation;

		// Token: 0x0400104E RID: 4174
		public Vector3 originalScale;

		// Token: 0x0400104F RID: 4175
		public Vector3 linearScale;

		// Token: 0x04001050 RID: 4176
		public Vector3 curvedScale;

		// Token: 0x04001051 RID: 4177
		public Color instanceColor;

		// Token: 0x04001052 RID: 4178
		public float solidTimeOverride;

		// Token: 0x04001053 RID: 4179
		public float ageSecs;

		// Token: 0x04001054 RID: 4180
		public int ageTicks;

		// Token: 0x04001055 RID: 4181
		public int setupTick;

		// Token: 0x04001056 RID: 4182
		public Vector3 spawnPosition;

		// Token: 0x04001057 RID: 4183
		public float skidSpeedMultiplierPerTick;
	}
}
