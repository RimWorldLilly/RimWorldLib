using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ABF RID: 6847
	public class Building_MechRelay : Building
	{
		// Token: 0x17001F45 RID: 8005
		// (get) Token: 0x0600AE30 RID: 44592 RVA: 0x003D62E3 File Offset: 0x003D44E3
		private Material ShadowMaterial
		{
			get
			{
				if (this.cachedShadowMaterial == null)
				{
					this.cachedShadowMaterial = MaterialPool.MatFrom("Things/Skyfaller/SkyfallerShadowDropPod", ShaderDatabase.Transparent);
				}
				return this.cachedShadowMaterial;
			}
		}

		// Token: 0x0600AE31 RID: 44593 RVA: 0x003D6310 File Offset: 0x003D4510
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (base.Spawned)
			{
				if (this.compMechRelay == null)
				{
					this.compMechRelay = base.GetComp<CompMechRelay>();
				}
				Skyfaller.DrawDropSpotShadow(drawLoc, Rot4.North, this.ShadowMaterial, this.def.size.ToVector2(), 65);
				float a = 0.55f + 0.5f * (1f + Mathf.Sin(6.2831855f * (float)GenTicks.TicksGame / 300f)) * 0.35f;
				float t = (this.compMechRelay.DestabilizationTick == -1) ? 0f : Mathf.Pow(1f - Mathf.Clamp01((float)(this.compMechRelay.DestabilizationTick - Find.TickManager.TicksGame) / 120f), 2f);
				float num = Mathf.Lerp(a, 0f, t);
				float extraRotation = Mathf.Lerp(0f, 45f, t);
				drawLoc.z += num;
				this.Graphic.Draw(drawLoc, flip ? base.Rotation.Opposite : base.Rotation, this, extraRotation);
				SilhouetteUtility.DrawGraphicSilhouette(this, drawLoc);
				return;
			}
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x040064D3 RID: 25811
		[Unsaved(false)]
		private Material cachedShadowMaterial;

		// Token: 0x040064D4 RID: 25812
		[Unsaved(false)]
		private CompMechRelay compMechRelay;

		// Token: 0x040064D5 RID: 25813
		private const float ZOffset = 0.55f;

		// Token: 0x040064D6 RID: 25814
		private const float BobHeight = 0.35f;

		// Token: 0x040064D7 RID: 25815
		private const float CrashTicks = 120f;

		// Token: 0x040064D8 RID: 25816
		private const float CrashRotation = 45f;

		// Token: 0x040064D9 RID: 25817
		private const float CrashProgressExponent = 2f;
	}
}
