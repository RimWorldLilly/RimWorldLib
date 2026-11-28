using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007C3 RID: 1987
	public class ReverbSetup
	{
		// Token: 0x06003FB2 RID: 16306 RVA: 0x0019786C File Offset: 0x00195A6C
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			if (widgetRow.ButtonText("Setup from preset...", "Set up the reverb filter from a preset.", true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(AudioReverbPreset)))
				{
					AudioReverbPreset audioReverbPreset = (AudioReverbPreset)obj;
					if (audioReverbPreset != AudioReverbPreset.User)
					{
						AudioReverbPreset localPreset = audioReverbPreset;
						list.Add(new FloatMenuOption(audioReverbPreset.ToString(), delegate()
						{
							this.SetupAs(localPreset);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x00197950 File Offset: 0x00195B50
		public void ApplyTo(AudioReverbFilter filter)
		{
			filter.dryLevel = this.dryLevel;
			filter.room = this.room;
			filter.roomHF = this.roomHF;
			filter.roomLF = this.roomLF;
			filter.decayTime = this.decayTime;
			filter.decayHFRatio = this.decayHFRatio;
			filter.reflectionsLevel = this.reflectionsLevel;
			filter.reflectionsDelay = this.reflectionsDelay;
			filter.reverbLevel = this.reverbLevel;
			filter.reverbDelay = this.reverbDelay;
			filter.hfReference = this.hfReference;
			filter.lfReference = this.lfReference;
			filter.diffusion = this.diffusion;
			filter.density = this.density;
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x00197A08 File Offset: 0x00195C08
		public static ReverbSetup Lerp(ReverbSetup A, ReverbSetup B, float t)
		{
			return new ReverbSetup
			{
				dryLevel = Mathf.Lerp(A.dryLevel, B.dryLevel, t),
				room = Mathf.Lerp(A.room, B.room, t),
				roomHF = Mathf.Lerp(A.roomHF, B.roomHF, t),
				roomLF = Mathf.Lerp(A.roomLF, B.roomLF, t),
				decayTime = Mathf.Lerp(A.decayTime, B.decayTime, t),
				decayHFRatio = Mathf.Lerp(A.decayHFRatio, B.decayHFRatio, t),
				reflectionsLevel = Mathf.Lerp(A.reflectionsLevel, B.reflectionsLevel, t),
				reflectionsDelay = Mathf.Lerp(A.reflectionsDelay, B.reflectionsDelay, t),
				reverbLevel = Mathf.Lerp(A.reverbLevel, B.reverbLevel, t),
				reverbDelay = Mathf.Lerp(A.reverbDelay, B.reverbDelay, t),
				hfReference = Mathf.Lerp(A.hfReference, B.hfReference, t),
				lfReference = Mathf.Lerp(A.lfReference, B.lfReference, t),
				diffusion = Mathf.Lerp(A.diffusion, B.diffusion, t),
				density = Mathf.Lerp(A.density, B.density, t)
			};
		}

		// Token: 0x04002A2F RID: 10799
		public float dryLevel;

		// Token: 0x04002A30 RID: 10800
		public float room;

		// Token: 0x04002A31 RID: 10801
		public float roomHF;

		// Token: 0x04002A32 RID: 10802
		public float roomLF;

		// Token: 0x04002A33 RID: 10803
		public float decayTime = 1f;

		// Token: 0x04002A34 RID: 10804
		public float decayHFRatio = 0.5f;

		// Token: 0x04002A35 RID: 10805
		public float reflectionsLevel = -10000f;

		// Token: 0x04002A36 RID: 10806
		public float reflectionsDelay;

		// Token: 0x04002A37 RID: 10807
		public float reverbLevel;

		// Token: 0x04002A38 RID: 10808
		public float reverbDelay = 0.04f;

		// Token: 0x04002A39 RID: 10809
		public float hfReference = 5000f;

		// Token: 0x04002A3A RID: 10810
		public float lfReference = 250f;

		// Token: 0x04002A3B RID: 10811
		public float diffusion = 100f;

		// Token: 0x04002A3C RID: 10812
		public float density = 100f;
	}
}
