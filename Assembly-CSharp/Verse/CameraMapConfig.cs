using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020000F4 RID: 244
	public abstract class CameraMapConfig : IExposable
	{
		// Token: 0x060008DD RID: 2269 RVA: 0x00032A58 File Offset: 0x00030C58
		public virtual void ConfigFixedUpdate_60(ref Vector3 rootPos, ref Vector3 velocity)
		{
			if (this.followSelected)
			{
				List<Pawn> selectedPawns = Find.Selector.SelectedPawns;
				if (selectedPawns.Count > 0)
				{
					Vector3 vector = Vector3.zero;
					int num = 0;
					foreach (Pawn pawn in selectedPawns)
					{
						if (pawn.MapHeld == Find.CurrentMap)
						{
							vector += pawn.TrueCenter();
							num++;
						}
					}
					if (num > 0)
					{
						vector /= (float)num;
						vector.y = rootPos.y;
						rootPos = Vector3.MoveTowards(rootPos, vector, 0.02f * Mathf.Max(Find.TickManager.TickRateMultiplier, 1f) * this.moveSpeedScale);
					}
				}
			}
			if (this.autoPanSpeed > 0f && (Find.TickManager.CurTimeSpeed != TimeSpeed.Paused || this.autoPanWhilePaused))
			{
				velocity.x = Mathf.Cos(this.autoPanAngle) * this.autoPanSpeed;
				velocity.z = Mathf.Sin(this.autoPanAngle) * this.autoPanSpeed;
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00032B88 File Offset: 0x00030D88
		public virtual void ConfigOnGUI()
		{
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00032B8C File Offset: 0x00030D8C
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.moveSpeedScale, "moveSpeedScale", 0f, false);
			Scribe_Values.Look<float>(ref this.zoomSpeed, "zoomSpeed", 0f, false);
			Scribe_Values.Look<FloatRange>(ref this.sizeRange, "sizeRange", default(FloatRange), false);
			Scribe_Values.Look<float>(ref this.zoomPreserveFactor, "zoomPreserveFactor", 0f, false);
			Scribe_Values.Look<bool>(ref this.smoothZoom, "smoothZoom", false, false);
			Scribe_Values.Look<bool>(ref this.followSelected, "followSelected", false, false);
			Scribe_Values.Look<float>(ref this.autoPanTargetAngle, "autoPanTargetAngle", 0f, false);
			Scribe_Values.Look<float>(ref this.autoPanSpeed, "autoPanSpeed", 0f, false);
			Scribe_Values.Look<string>(ref this.fileName, "fileName", null, false);
			Scribe_Values.Look<bool>(ref this.autoPanWhilePaused, "autoPanWhilePaused", false, false);
		}

		// Token: 0x0400040F RID: 1039
		public float dollyRateKeys = 50f;

		// Token: 0x04000410 RID: 1040
		public float dollyRateScreenEdge = 35f;

		// Token: 0x04000411 RID: 1041
		public float camSpeedDecayFactor = 0.85f;

		// Token: 0x04000412 RID: 1042
		public float moveSpeedScale = 2f;

		// Token: 0x04000413 RID: 1043
		public float zoomSpeed = 2.6f;

		// Token: 0x04000414 RID: 1044
		public FloatRange sizeRange = new FloatRange(SteamDeck.IsSteamDeck ? 7.2f : 11f, 60f);

		// Token: 0x04000415 RID: 1045
		public float zoomPreserveFactor;

		// Token: 0x04000416 RID: 1046
		public bool smoothZoom;

		// Token: 0x04000417 RID: 1047
		public bool followSelected;

		// Token: 0x04000418 RID: 1048
		public string fileName;

		// Token: 0x04000419 RID: 1049
		public bool autoPanWhilePaused;

		// Token: 0x0400041A RID: 1050
		public float autoPanTargetAngle;

		// Token: 0x0400041B RID: 1051
		public float autoPanAngle;

		// Token: 0x0400041C RID: 1052
		public float autoPanSpeed;

		// Token: 0x0400041D RID: 1053
		public bool gravshipFreeCam;

		// Token: 0x0400041E RID: 1054
		public bool gravshipPanOnCutsceneStart = true;

		// Token: 0x0400041F RID: 1055
		public bool gravshipEnableOverrideSizeRange;

		// Token: 0x04000420 RID: 1056
		public FloatRange gravshipOverrideSizeRange = new FloatRange(15f, 60f);
	}
}
