using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001F0 RID: 496
	public sealed class GameInfo : IExposable
	{
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000E1F RID: 3615 RVA: 0x00049881 File Offset: 0x00047A81
		public float RealPlayTimeInteracting
		{
			get
			{
				return this.realPlayTimeInteracting;
			}
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00049889 File Offset: 0x00047A89
		public void GameInfoOnGUI()
		{
			if (Event.current.type == EventType.mouseDown || Event.current.type == EventType.mouseMove || Event.current.type == EventType.keyDown)
			{
				this.lastInputRealTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x000498BC File Offset: 0x00047ABC
		public void GameInfoUpdate()
		{
			if (Time.realtimeSinceStartup < this.lastInputRealTime + 90f && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Menu && Current.ProgramState == ProgramState.Playing && !Find.WindowStack.IsOpen<Dialog_Options>())
			{
				this.realPlayTimeInteracting += RealTime.realDeltaTime;
			}
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00049914 File Offset: 0x00047B14
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.realPlayTimeInteracting, "realPlayTimeInteracting", 0f, false);
			Scribe_Values.Look<bool>(ref this.permadeathMode, "permadeathMode", false, false);
			Scribe_Values.Look<string>(ref this.permadeathModeUniqueName, "permadeathModeUniqueName", null, false);
			Scribe_Values.Look<PlanetTile>(ref this.startingTile, "startingTile", PlanetTile.Invalid, false);
			Scribe_Collections.Look<Pawn>(ref this.startingAndOptionalPawns, "startingAndOptionalPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.startingAndOptionalPawns.Count - 1; i >= 0; i--)
				{
					if (this.startingAndOptionalPawns[i] == null)
					{
						this.startingAndOptionalPawns.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x04000E0E RID: 3598
		public bool permadeathMode;

		// Token: 0x04000E0F RID: 3599
		public string permadeathModeUniqueName;

		// Token: 0x04000E10 RID: 3600
		private float realPlayTimeInteracting;

		// Token: 0x04000E11 RID: 3601
		public PlanetTile startingTile = PlanetTile.Invalid;

		// Token: 0x04000E12 RID: 3602
		public List<Pawn> startingAndOptionalPawns = new List<Pawn>();

		// Token: 0x04000E13 RID: 3603
		private float lastInputRealTime;
	}
}
